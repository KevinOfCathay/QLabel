using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp.Dnn;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using QLabel.Scripts.Projects;
using QLabel.Scripts.AnnotationData;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Media.Media3D;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System.Diagnostics;

namespace QLabel.Scripts.Inference_Machine {
	internal sealed class HRNetInferenceBU : InferenceBase {
		private readonly ClassTemplate[] labels;
		private readonly (int x, int y, ClassTemplate)[] skeletons;
		private readonly int num_joints;
		private readonly int in_width, in_height;
		private readonly int o_width, o_height;
		private readonly int[] flip_indices = new int[] { 1, 0, 3, 2, 5, 4, 7, 6, 9, 8, 11, 10, 12, 13 };
		private readonly string post_model_path;
		private InferenceSession session_postprocessing;
		private bool post_process = true;
		private float keypoint_threshold = 0.35f;

		/// <summary>
		/// 初始化参数
		/// </summary>
		public HRNetInferenceBU (
			string model_path, string post_model_path,
			ClassTemplate[] labels, (int x, int y, ClassTemplate)[] skeletons, int[] input_dims, int[] output_dims) :
			base(input_dims, output_dims) {
			base.model_path = model_path;
			base.model_path = model_path;
			this.labels = labels;
			this.skeletons = skeletons;
			this.post_model_path = post_model_path;
			this.num_joints = labels.Length;

			this.in_width = input_dims[3];
			this.in_height = input_dims[2];
			this.o_width = output_dims[3];
			this.o_height = output_dims[2];

		}

		public override void BuildSession () {
			session = new InferenceSession(model_path);
			session_postprocessing = new InferenceSession(post_model_path);
			if ( session == null || session_postprocessing == null ) {
				Debug.WriteLine("Failed to load session.");
			}
		}
		protected override AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null) {
			var bitmap = ImageUtils.ResizeBitmap(image, new OpenCvSharp.Size(in_width, in_height));
			var input_tensor = GetInputTensor(bitmap);
			var input_node = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input.1", input_tensor) };
			var input_tensor_flipped = input_tensor.flip(axis: 3);
			var input_flipped = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input.1", input_tensor_flipped) };
			Vector2 scale = new Vector2(( (float) image.Width ) / ( (float) o_width ), ( (float) image.Height ) / ( (float) o_height ));
			if ( session != null && session_postprocessing != null ) {
				var run_output = session.Run(input_node);
				var output = run_output.First().AsTensor<float>();

				var run_output_flipped = session.Run(input_flipped);
				var output_flipped = run_output_flipped.First().AsTensor<float>();

				var (heatmap, tag) = SplitAEOutput(output);

				// 在反转的图像上在执行一次相同操作
				var (heatmap_flipped, tag_flipped) = SplitAEOutput(output_flipped);
				heatmap_flipped = FlipFeatureMap(heatmap_flipped, flip_indices);
				tag_flipped = FlipFeatureMap(tag_flipped, flip_indices);

				var aggregated_heatmaps = AggregateAverage(heatmap, heatmap_flipped, new Vector2(image.Height, image.Width));
				var aggregated_tags = AggregateConcat(tag, tag_flipped, new Vector2(image.Height, image.Width));
			}
			throw new NotImplementedException();
		}
		protected override DenseTensor<float> GetInputTensor (Bitmap image) {
			var input = new DenseTensor<float>(input_dims);

			// https://stackoverflow.com/a/74337947
			BitmapData bitmap_data = image.LockBits(new Rectangle(0, 0, in_width, in_height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			int bytesPerPixel = Image.GetPixelFormatSize(bitmap_data.PixelFormat) / 8;
			int stride = bitmap_data.Stride;

			float[] mean = new float[input_dims[1]];
			float count = in_width * in_height;
			unsafe {
				Parallel.For(0, in_height, (y) => {
					Parallel.For(0, in_width, (x) => {
						var rgb = (byte*) ( bitmap_data.Scan0 + y * stride );
						float R = (float) rgb[x * bytesPerPixel + 0] / 255f;
						float G = (float) rgb[x * bytesPerPixel + 1] / 255f;
						float B = (float) rgb[x * bytesPerPixel + 2] / 255f;

						// 这里的顺序应当为 batch, channel, y, x
						input[0, 0, y, x] = R;
						input[0, 1, y, x] = G;
						input[0, 2, y, x] = B;

						mean[0] += R;
						mean[1] += G;
						mean[2] += B;
					});
				});
			}
			mean[0] /= count;
			mean[1] /= count;
			mean[2] /= count;

			float[] sum = new float[input_dims[1]];
			foreach ( var w in Enumerable.Range(0, this.in_width) ) {
				foreach ( var h in Enumerable.Range(0, this.in_height) ) {
					foreach ( var c in Enumerable.Range(0, this.input_dims[1]) ) {
						sum[c] += ( input[0, c, h, w] - mean[c] ) * ( input[0, c, h, w] - mean[c] );
						input[0, c, h, w] -= mean[c];
					}
				}
			}
			foreach ( var c in Enumerable.Range(0, this.input_dims[1]) ) {
				sum[c] = MathF.Sqrt(sum[c] / ( this.in_width * this.in_height ));
			}
			foreach ( var w in Enumerable.Range(0, this.in_width) ) {
				foreach ( var h in Enumerable.Range(0, this.in_height) ) {
					foreach ( var c in Enumerable.Range(0, this.input_dims[1]) ) {
						input[0, c, h, w] /= sum[c];
					}
				}
			}
			return input;
		}
		private (Tensor<float> heatmap, Tensor<float> tag) SplitAEOutput (Tensor<float> output) {
			// Split multi-stage outputs into heatmaps & tags.
			int[] new_dims = output_dims.ToArray();
			new_dims[1] = num_joints;
			var heatmap = new DenseTensor<float>(new_dims);
			var tag = new DenseTensor<float>(new_dims);

			int[] index = new int[4] { 0, 0, 0, 0 };
			while ( true ) {
				heatmap[index] = output[index];

				int[] new_index = index.ToArray();
				new_index[1] = index[1] + num_joints;
				tag[index] = output[new_index];

				int axis = 3;
				index[axis] += 1;
				while ( index[axis] >= new_dims[axis] ) {
					index[axis] = 0;
					axis -= 1;
					if ( axis < 0 ) { break; }
					index[axis] += 1;
				}
				if ( axis < 0 ) { break; }
			}
			return (heatmap, tag);
		}
		private Tensor<float> AggregateAverage (Tensor<float> feature_map, Tensor<float> feature_map_flipped, Vector2 size) {
			var feature_map_resized = Resize(feature_map, size);
			var feature_map_flipped_resized = Resize(feature_map_flipped, size);
			DenseTensor<float> output_feature_map = new DenseTensor<float>(feature_map_resized.Dimensions);
			int length = 1;
			foreach ( int len in feature_map_resized.Dimensions ) {
				length *= len;
			}
			for ( int i = 0; i < length; i += 1 ) {
				output_feature_map.SetValue(i,
				    ( feature_map_resized.GetValue(i) + feature_map_flipped_resized.GetValue(i) ) / 2);
			}
			return output_feature_map;
		}
		private Tensor<float> AggregateConcat (Tensor<float> feature_map, Tensor<float> feature_map_flipped, Vector2 size) {
			var feature_map_resized = Resize(feature_map, size);
			var feature_map_flipped_resized = Resize(feature_map_flipped, size);

			// 建立一个合并后的 tensor
			int rank = feature_map_resized.Dimensions.Length;
			int[] new_dims = new int[rank + 1];
			for ( int i = 0; i < rank; i += 1 ) {
				new_dims[i] = feature_map_resized.Dimensions[i];
			}
			new_dims[rank] = 2;
			DenseTensor<float> output_feature_map = new DenseTensor<float>(new_dims);

			// 复制数据
			int length = 1;
			foreach ( int len in feature_map_resized.Dimensions ) {
				length *= len;
			}
			for ( int i = 0; i < length; i += 1 ) {
				output_feature_map.SetValue(i * 2, feature_map_resized.GetValue(i));
				output_feature_map.SetValue(i * 2 + 1, feature_map_flipped_resized.GetValue(i));
			}
			return output_feature_map;
		}
		private Tensor<float> Resize (Tensor<float> feature_map, Vector2 resize_size) {
			Vector2 original_size = new Vector2(feature_map.Dimensions[2], feature_map.Dimensions[3]);
			if ( original_size != resize_size ) {
				// Interpolate
				var dims = feature_map.Dimensions;
				var (batch, channel, in_height, in_width) = (dims[0], dims[1], dims[2], dims[3]);
				var (out_height, out_width) = ((int) resize_size.X, (int) resize_size.Y);
				float height_scale = (float) in_height / (float) out_height;
				float width_scale = (float) in_width / (float) out_width;
				Tensor<float> resized_tensor = new DenseTensor<float>(new int[] { batch, channel, out_height, out_width });
				for ( int h = 0; h < out_height; h += 1 ) {
					float in_y = ( (float) ( h ) + 0.5f ) * height_scale - 0.5f;
					int top_y_index = in_y > 0f ? (int) MathF.Floor(in_y) : 0;
					int bottom_y_index = ( in_y < in_height - 1 ) ? (int) MathF.Ceiling(in_y) : in_height - 1;
					float y_lerp = in_y - MathF.Floor(in_y);

					for ( int w = 0; w < out_width; w += 1 ) {
						for ( int b = 0; b < batch; b += 1 ) {
							for ( int c = 0; c < channel; c += 1 ) {
								float in_x = ( (float) ( w ) + 0.5f ) * width_scale - 0.5f;
								int left_x_index = in_x > 0f ? (int) MathF.Floor(in_x) : 0;
								int right_x_index = ( in_x < in_width - 1 ) ? (int) MathF.Ceiling(in_x) : in_width - 1;
								float x_lerp = in_x - left_x_index;

								float top_left = feature_map[b, c, top_y_index, left_x_index];
								float top_right = feature_map[b, c, top_y_index, right_x_index];
								float bottom_left = feature_map[b, c, bottom_y_index, left_x_index];
								float bottom_right = feature_map[b, c, bottom_y_index, right_x_index];

								float top = top_left + ( top_right - top_left ) * x_lerp;
								float bottom = bottom_left + ( bottom_right - bottom_left ) * x_lerp;
								resized_tensor[b, c, h, w] = top + ( bottom - top ) * y_lerp;
							}
						}
					}
				}
				return resized_tensor;
			} else {
				return feature_map;
			}
		}
		private Tensor<float> FlipFeatureMap (Tensor<float> feature_map, int[] indices) {
			return feature_map.flip(3).take(indices, 1);
		}
	}
}
