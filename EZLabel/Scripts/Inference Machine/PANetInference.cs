using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp.Dnn;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using QLabel.Scripts.Projects;
using QLabel.Scripts.AnnotationData;
using System.Drawing.Imaging;
using static OpenCvSharp.LineIterator;

namespace QLabel.Scripts.Inference_Machine {
	/// <summary>
	/// PANet labeltext Detection
	/// </summary>
	internal sealed class PANetInference : BaseInferenceMachine {
		public int width, height, classes;
		private const int out_width = 160, out_height = 160;
		public float downsample_ratio = 0.25f;
		private readonly ClassLabel[] labels;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public PANetInference (string path, ClassLabel[] labels, int width = 640, int height = 640, int classes = 80) :
			base(new int[] { 1, 3, width, height }, new int[] { 1, 6, width / 4, height / 4 }) {
			model_path = path;
			this.width = width;
			this.height = height;
			this.classes = classes;
			this.labels = labels;
		}

		public override AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null) {
			var resized = ImageUtils.ResizeBitmap(image, width, height);
			var input_tensor = GetInputTensor(resized);
			var input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input", input_tensor) };
			Vector2 scale = new Vector2(( (float) image.Width ) / ( (float) width ), ( (float) image.Height ) / ( (float) height ));
			if ( session != null ) {
				var run_output = session.Run(input);
				var output_tensor = run_output.First().AsTensor<float>();
				var (boundaries, scores) = GetBoundaries(output_tensor);

				var len = boundaries.Count;
				AnnoData[] data = new AnnoData[len];
				for ( int i = 0; i < len; i += 1 ) {
					// 创建 annodata
					ClassLabel cl = new ClassLabel(labels[0]);

					var boundary = boundaries[i];
					Vector2[] rpoints = new Vector2[boundary.Length];
					Parallel.For(0, boundary.Length, (index) => {
						rpoints[index] = new Vector2(boundary[index].X, boundary[index].Y) * scale;
					});
					data[i] = new ADPolygon(rpoints, cl, conf: scores[i]);
				}
				return data;
			} else {
				throw new ApplicationException("Session 未被加载. 无法进行 inference.");
			}
		}

		private (List<OpenCvSharp.Point[]>, List<float>) GetBoundaries (Tensor<float> output) {
			// adapted from
			// mmocr\models\textdet\dense_heads\head_mixin.py
			// mmocr\models\textdet\postprocess\pan_postprocessor.py

			// Squeeze
			// score_maps = score_maps.squeeze()
			float[,] score = new float[out_width, out_height];
			byte[,] text = new byte[out_width, out_height];
			byte[,] kernel = new byte[out_width, out_height];
			float[,,] embeddings = new float[out_width, out_height, 4];

			for ( int x = 0; x < out_width; x += 1 ) {
				for ( int y = 0; y < out_height; y += 1 ) {
					// preds[:2, :, :] = torch.sigmoid(preds[:2, :, :])
					// Text = preds[0] > self.min_text_confidence
					var score_temp = output[0, 0, x, y];
					score[x, y] = 1f / ( 1f + MathF.Exp(-score_temp) );
					text[x, y] = (byte) ( score[x, y] > 0.5f ? 1 : 0 );

					var kernel_temp = 1f / ( 1f + MathF.Exp(-output[0, 1, x, y]) );
					kernel_temp = kernel_temp > 0.5f ? 1f : 0f;
					kernel[x, y] = (byte) ( kernel_temp * text[x, y] );

					// embeddings = preds[2:].transpose((1, 2, 0))  # (h, w, 4)
					//         1  2  0                        0  1  2
					embeddings[x, y, 0] = output[0, 2, x, y];
					embeddings[x, y, 1] = output[0, 3, x, y];
					embeddings[x, y, 2] = output[0, 4, x, y];
					embeddings[x, y, 3] = output[0, 5, x, y];
				}
			}

			Mat mat_kernel = new Mat(new int[] { out_width, out_height }, MatType.CV_8U, kernel);
			Mat mat_kernel_contour = new Mat(new int[] { out_width, out_height }, MatType.CV_8U, new byte[out_width, out_height]);
			int[,] kernel_labels; OpenCvSharp.Point[][] contours;
			int region_nums = Cv2.ConnectedComponents(mat_kernel, out kernel_labels, PixelConnectivity.Connectivity4);

			// contours, _ = cv2.findContours((kernel * 255).astype(np.uint8), cv2.RETR_LIST, cv2.CHAIN_APPROX_NONE)
			Cv2.FindContours(mat_kernel * 255, out contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxNone);
			Cv2.DrawContours(mat_kernel_contour, contours, -1, new Scalar(255));

			byte[,] kernel_contour = new byte[out_width, out_height];
			for ( int x = 0; x < out_width; x += 1 ) {
				for ( int y = 0; y < out_height; y += 1 ) {
					kernel_contour[x, y] = mat_kernel_contour.At<byte>(x, y);
				}
			}
			// score, OK
			// Text, OK
			// embeddings, OK
			// kernel_labels, OK
			// kernel_contour, OK
			// region_nums OK
			var text_points = PixelGroup(score, text, embeddings, kernel_labels, kernel_contour, region_nums);

			// boundaries = []
			List<OpenCvSharp.Point[]> boundaries = new List<OpenCvSharp.Point[]>();
			List<float> scores = new List<float>();
			// for text_point in text_points:
			foreach ( var text_point in text_points ) {
				var text_confidence = text_point[0];

				// text_point = text_point[2:]
				int[,] text_point_reshaped = new int[( text_point.Count - 2 ) / 2, 2];
				int index = 0;
				foreach ( var p in text_point.GetRange(2, text_point.Count - 2) ) {
					// text_point = np.array(text_point, dtype=int).reshape(-1, 2)
					// 重新排列
					text_point_reshaped.at(index) = (int) p;
					index += 1;
				}
				int area = text_point_reshaped.GetLength(0);
				if ( !is_valid_instance(area, text_confidence) ) {
					continue;
				}

				// vertices_confidence = PointsToBoundary(text_point, self.text_repr_type,
				//                                      text_confidence)
				var boundary = PointsToBoundary(text_point_reshaped);
				boundaries.Add(ResizeBoundary(boundary, downsample_ratio));
				scores.Add(text_confidence);
			}
			return (boundaries, scores);
		}

		// https://github.com/open-mmlab/mmcv/blob/master/mmcv/ops/csrc/pytorch/cpu/PixelGroup.cpp
		// std::vector<std::vector<float>> pixel_group_cpu(
		// Tensor score, Tensor mask, Tensor embedding, Tensor kernel_label,
		// Tensor kernel_contour, int kernel_region_num, float dis_threshold)

		// score (160, 160) [float32]
		// mask (160, 160) [bool]
		// embeddings (160, 160, 4) [float32]
		// kernel_contour (160, 160) [uint8]
		private List<List<float>> PixelGroup (
		    float[,] score, byte[,] mask, float[,,] embedding, int[,] kernel_label,
		    byte[,] kernel_contour, int kernel_region_num, float dis_threshold = 0.85f) {
			int height = out_height;
			int width = out_width;
			int embedding_dim = 4;
			float threshold_square = dis_threshold * dis_threshold;
			Queue<(int, int, int)> contour_pixels = new Queue<(int, int, int)>();
			float[,] kernel_vector = new float[kernel_region_num, embedding_dim + 1];
			var text_label = kernel_label.clone();

			for ( int i = 0; i < height; i += 1 ) {
				var ptr_embedding_tmp = i * width * embedding_dim;  // index
				var ptr_kernel_label_tmp = i * width;  // index
				var ptr_kernel_contour_tmp = i * width;    // index

				for ( int j = 0, k = 0; j < width && k < width * embedding_dim; j += 1, k += embedding_dim ) {
					int label = kernel_label.at(ptr_kernel_label_tmp + j);
					if ( label > 0 ) {
						for ( int d = 0; d < embedding_dim; d++ ) {
							kernel_vector[label, d] += embedding.at(ptr_embedding_tmp + k + d);
						}
						kernel_vector[label, embedding_dim] += 1;
						if ( kernel_contour.at(ptr_kernel_contour_tmp + j) > 0 ) {
							contour_pixels.Enqueue((i, j, label));
						}
					}
				}
			}
			for ( int i = 0; i < kernel_region_num; i += 1 ) {
				for ( int j = 0; j < embedding_dim; j += 1 ) {
					kernel_vector[i, j] /= kernel_vector[i, embedding_dim];
				}
			}
			int[] dx = new int[4] { -1, 1, 0, 0 };
			int[] dy = new int[4] { 0, 0, -1, 1 };

			while ( contour_pixels.Count > 0 ) {
				var query_pixel = contour_pixels.Dequeue();
				int y = query_pixel.Item1;
				int x = query_pixel.Item2;
				int l = query_pixel.Item3;

				// var kernel_cv = kernel_vector[l];
				for ( int idx = 0; idx < 4; idx += 1 ) {
					int tmpy = y + dy[idx];
					int tmpx = x + dx[idx];
					if ( tmpy < 0 || tmpy >= height || tmpx < 0 || tmpx >= width ) { continue; }
					var ptr_text_label_tmp = tmpy * width; // index
					if ( mask.at(tmpy * width + tmpx) == 0 || text_label.at(ptr_text_label_tmp + tmpx) > 0 ) { continue; }

					float dis = 0;
					var idx_embedding_tmp = tmpy * width * embedding_dim; // index
					for ( int i = 0; i < embedding_dim; i += 1 ) {
						dis += MathF.Pow(kernel_vector[l, i] - embedding.at(idx_embedding_tmp + tmpx * embedding_dim + i), 2f);
						// ignore further computing if dis is big enough
						if ( dis >= threshold_square ) break;
					}
					if ( dis >= threshold_square ) continue;
					contour_pixels.Enqueue((tmpy, tmpx, l));
					text_label.at(ptr_text_label_tmp + tmpx) = l;
				}
			}
			var p = EstimateConfidence(text_label, score, kernel_region_num,
							 height, width);

			// p OK
			return p;
		}
		private List<List<float>> EstimateConfidence (int[,] label, float[,] score, int label_num, int height, int width) {
			List<List<float>> point_vector = new List<List<float>>();
			for ( int i = 0; i < label_num; i += 1 ) {
				point_vector.Add(new List<float>() { 0, 0 });
			}
			for ( int y = 0; y < height; y++ ) {
				var label_tmp = y * width;
				var score_tmp = y * width;
				for ( int x = 0; x < width; x++ ) {
					var l = label.at(label_tmp + x);
					if ( l > 0 ) {
						float confidence = score.at(score_tmp + x);
						point_vector[l].Add(x);
						point_vector[l].Add(y);
						point_vector[l][0] += confidence;
						point_vector[l][1] += 1;
					}
				}
			}
			for ( int l = 0; l < point_vector.Count; l += 1 )
				if ( point_vector[l][1] > 0 ) {
					point_vector[l][0] /= point_vector[l][1];
				}
			return point_vector;
		}
		private bool is_valid_instance (int area, float confidence,
		    int area_thresh = 16, float confidence_thresh = 0.85f) {

			// return bool(area >= area_thresh and confidence > confidence_thresh)
			return ( area >= area_thresh ) && ( confidence > confidence_thresh );
		}
		private OpenCvSharp.Point[][] PointsToBoundary (int[,] text_point) {
			int h = int.MinValue, w = int.MinValue;
			int x = text_point.GetLength(0), y = text_point.GetLength(1);
			for ( int i = 0; i < x; i += 1 ) {
				if ( text_point[i, 0] > w ) { w = text_point[i, 0]; }
				if ( text_point[i, 1] > h ) { h = text_point[i, 1]; }
			}
			h += 10; w += 10;
			byte[,] mask = new byte[h, w];
			for ( int i = 0; i < x; i += 1 ) {
				mask[text_point[i, 1], text_point[i, 0]] = (byte) 255;
			}

			OpenCvSharp.Point[][] contours;
			Cv2.FindContours(
			    new Mat(new int[] { h, w }, MatType.CV_8U, mask),
			    out contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
			//  if rescale:
			//      boundaries = self.ResizeBoundary(boundaries,
			//      1.0 / self.downsample_ratio / img_metas[0]['scale_factor'])
			return contours;
		}

		private OpenCvSharp.Point[] ResizeBoundary (OpenCvSharp.Point[][] boundaries, float scale_factor) {
			// 由于是正方形的图像，所以 scale_factor 只需要单一值来代表
			// for b in boundaries:
			//      sz = len(b)
			//      b[:sz - 1] = (np.array(b[:sz - 1]) * (np.tile(scale_factor[:2], int(
			//                (sz - 1) / 2)).reshape(1, sz - 1))).flatten().tolist()
			// return boundaries
			List<OpenCvSharp.Point> result = new List<OpenCvSharp.Point>();
			foreach ( var points in boundaries ) {
				foreach ( var point in points ) {
					result.Add(new OpenCvSharp.Point {
						X = (int) ( point.X * ( 1 / scale_factor ) ),
						Y = (int) ( point.Y * ( 1 / scale_factor ) )
					});
				}
			}
			return result.ToArray();
		}

		protected override DenseTensor<float> GetInputTensor (Bitmap image) {
			var input = new DenseTensor<float>(input_dims);

			// https://stackoverflow.com/a/74337947
			BitmapData bitmap_data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			int bytesPerPixel = Image.GetPixelFormatSize(bitmap_data.PixelFormat) / 8;
			int stride = bitmap_data.Stride;

			unsafe {
				Parallel.For(0, height, (y) => {
					Parallel.For(0, width, (x) => {
						var rgb = (byte*) ( bitmap_data.Scan0 + y * stride );
						// 这里的顺序应当为 batch, channel, y, x
						input[0, 0, y, x] = ( (float) ( rgb[x * bytesPerPixel + 0] ) - 123.675f ) / 58.395f;
						input[0, 1, y, x] = ( (float) ( rgb[x * bytesPerPixel + 1] ) - 116.28f ) / 57.12f;
						input[0, 2, y, x] = ( (float) ( rgb[x * bytesPerPixel + 2] ) - 103.53f ) / 57.375f;
					});
				});
			}

			return input;
		}
	}
}

