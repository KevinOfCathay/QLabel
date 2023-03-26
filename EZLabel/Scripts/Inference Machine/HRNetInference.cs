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

namespace QLabel.Scripts.Inference_Machine {
	internal sealed class HRNetInference : BaseInferenceMachine {
		private readonly ClassLabel[] labels;
		private readonly (int x, int y, ClassLabel)[] skeletons;
		private readonly int in_width, in_height;
		private readonly int o_width, o_height;
		private bool post_process = true;
		private float keypoint_threshold = 0.35f;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public HRNetInference (string path, ClassLabel[] labels, (int x, int y, ClassLabel)[] skeletons, int[] input_dims, int[] output_dims) :
			base(input_dims, output_dims) {
			model_path = path;
			this.labels = labels;
			this.skeletons = skeletons;

			this.in_width = input_dims[3];
			this.in_height = input_dims[2];
			this.o_width = output_dims[3];
			this.o_height = output_dims[2];
		}
		public override AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null) {
			var bitmap = ImageUtils.ResizeBitmap(image, in_width, in_height);
			var input = GetInputTensor(bitmap); var input_node = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input.1", input) };
			Vector2 scale = new Vector2(( (float) image.Width ) / ( (float) o_width ), ( (float) image.Height ) / ( (float) o_height ));
			if ( session != null ) {
				var run_output = session.Run(input_node);
				var output = run_output.First().AsTensor<float>();          // 返回 tensor 更加容易 index
				var (idx, maxvals) = GetMaxPreds(output);
				int len = maxvals.Length;
				List<AnnoData> datas = new List<AnnoData>(len);
				Dictionary<int, ADDot> keypoints = new Dictionary<int, ADDot>(len);

				for ( int i = 0; i < len; i += 1 ) {
					if ( maxvals[i] > keypoint_threshold ) {
						Vector2 rpoint = new Vector2(idx[i, 0], idx[i, 1]) * scale;
						ClassLabel cl = new ClassLabel(labels[i]);
						ADDot dot = new ADDot(rpoint, cl, Array.Empty<Guid>(), Array.Empty<Guid>(), maxvals[i]);    // 生成 AnnoData
						datas.Add(dot);
						keypoints.Add(i, dot);
					}
				}
				foreach ( var (x, y, label) in skeletons ) {
					// 两端都有点时，创建一个线段
					if ( keypoints.ContainsKey(x) && keypoints.ContainsKey(y) ) {
						ClassLabel cl = new ClassLabel(label);
						ADLine line = new ADLine(
							new Vector2(idx[x, 0], idx[x, 1]) * scale,
							new Vector2(idx[y, 0], idx[y, 1]) * scale,
							 keypoints[x], keypoints[y], cl);
						datas.Add(line);
					}
				}
				return datas.ToArray();
			} else {
				throw new ApplicationException("Session 未被加载. 无法进行 inference.");
			}
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
		private (float[,], float[]) GetMaxPreds (Tensor<float> output) {
			int[,] idx = new int[output_dims[1], 2];
			float[] maxvals = new float[output_dims[1]];    // [1, 17, 1]   

			// 获取 argmax 和 max
			//  idx = np.argmax(heatmaps_reshaped, 2).reshape((N, K, 1))    # [1, 17, 1]
			//  maxvals = np.amax(heatmaps_reshaped, 2).reshape((N, K, 1))  # [1, 17, 1]
			Parallel.For(0, output_dims[1], k => {
				for ( int n = 0; n < output_dims[0]; n += 1 ) {
					for ( int h = 0; h < output_dims[2]; h += 1 ) {
						for ( int w = 0; w < output_dims[3]; w += 1 ) {
							if ( output[n, k, h, w] > maxvals[k] ) {
								//  preds[:, :, 0] = preds[:, :, 0] % W
								//  preds[:, :, 1] = preds[:, :, 1] // W
								idx[k, 0] = w;
								idx[k, 1] = h;
								maxvals[k] = output[n, k, h, w];
							}
						}
					}
				}
			});
			for ( int k = 0; k < output_dims[1]; k += 1 ) {
				if ( maxvals[k] < 0f ) {
					//  preds[:, :, 0] = preds[:, :, 0] % W
					//  preds[:, :, 1] = preds[:, :, 1] // W
					idx[k, 0] = -1;
					idx[k, 1] = -1;
				}
			}
			float[,] idx_f = new float[output_dims[1], 2];
			for ( int a = 0; a < 2; a += 1 ) {
				for ( int b = 0; b < output_dims[1]; b += 1 ) {
					idx_f[b, a] = idx[b, a];
				}
			}
			if ( this.post_process ) {
				for ( int k = 0; k < output_dims[1]; k += 1 ) {
					var px = idx[k, 0];
					var py = idx[k, 1];
					if ( ( 1 < px && px < this.in_width - 1 ) && ( 1 < py && py < this.in_height - 1 ) ) {
						// mmpose\core\evaluation\top_down_eval.py
						// diff = np.array([
						// heatmap[py][px + 1] - heatmap[py][px - 1],
						//          heatmap[py + 1][px] - heatmap[py - 1][px]])
						// preds[n][k] += np.sign(diff) * .25
						idx_f[k, 0] += ( output[0, k, py, px + 1] - output[0, k, py, px - 1] ) > 0 ? 0.25f : -0.25f;
						idx_f[k, 1] += ( output[0, k, py + 1, px] - output[0, k, py - 1, px] ) > 0 ? 0.25f : -0.25f;
					}
				}
			}
			return (idx_f, maxvals);
		}
	}
}
