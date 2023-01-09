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

namespace QLabel.Scripts.Inference_Machine {
	public class Yolov5Inference : BaseInferenceMachine {
		public int width, height, classes;
		private string[] labels;
		private float conf_threshold = 0.35f;
		private float score_threshold = 0.5f;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public Yolov5Inference (string path, string[] labels, int width = 640, int height = 640, int classes = 80) :
			base(new[] { 1, 3, height, width }, new[] { 25200, classes + 5 }) {
			model_path = path;
			this.width = width;
			this.height = height;
			this.classes = classes;
			this.labels = labels;
		}
		protected override DenseTensor<float> GetInputTensor (Bitmap image) {
			var input = new DenseTensor<float>(input_dims);

			foreach ( var x in Enumerable.Range(0, width) ) {
				foreach ( var y in Enumerable.Range(0, height) ) {
					var pixel = image.GetPixel(x, y);
					// 这里的顺序应当为 batch, channel, y, x
					input[0, 0, y, x] = ( (float) ( pixel.R ) ) / 255f;
					input[0, 1, y, x] = ( (float) ( pixel.G ) ) / 255f;
					input[0, 2, y, x] = ( (float) ( pixel.B ) ) / 255f;
				}
			}
			return input;
		}

		/// <summary>
		/// 运行模型并以 float[] 形式返回结果
		/// </summary>
		public float[] Run<T> (DenseTensor<T> input) {
			// 从 Dense Tensor 创建一个 Input
			var input_node = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<T>("images", input) };
			if ( session != null ) {
				var output = session.Run(input_node);
				return output.First().AsEnumerable<float>().ToArray();
			} else {
				throw new ApplicationException("Session 未被加载. 无法进行 inference.");
			}
		}

		protected (int[] ind, float[] scores, Rect[] boxes, int[] classes) NMS (float[] output) {
			List<Rect> boxes = new List<Rect>();
			List<float> scores = new List<float>();
			List<int> classes = new List<int>();
			Mat res = new Mat(output_dims, MatType.CV_32F, output);

			for ( int r = 0; r < 25200; r += 1 ) {
				float conf = res.At<float>(r, 4);
				if ( conf > conf_threshold ) {
					float cx = res.At<float>(r, 0);
					float cy = res.At<float>(r, 1);
					float w = res.At<float>(r, 2);
					float h = res.At<float>(r, 3);

					Mat scr = res.Row(r).ColRange(5, this.classes + 5);
					scr *= conf;
					double minV, maxV;
					OpenCvSharp.Point minI, maxI;
					Cv2.MinMaxLoc(scr, out minV, out maxV, out minI, out maxI);
					scores.Add((float) maxV);
					boxes.Add(new Rect(
					    (int) ( cx - w / 2f ),
					    (int) ( cy - h / 2f ),
					    (int) ( w ), (int) ( h )));
					classes.Add((int) maxI.X);
				}
			}
			int[] ind;
			CvDnn.NMSBoxes(boxes, scores, conf_threshold, score_threshold, out ind);

			int[] final_classes = new int[ind.Length];
			float[] final_scores = new float[ind.Length];
			Rect[] final_boxes = new Rect[ind.Length];
			int count = 0;
			foreach ( var index in ind ) {
				final_classes[count] = classes[index];
				final_boxes[count] = boxes[index];
				final_scores[count] = scores[index];
				count += 1;
			}
			return (ind, final_scores, final_boxes, final_classes);
		}
		public override AnnoData[] RunInference (ImageFileData img_file) {
			var bitmap = LoadImage(img_file, width, height);

			var input_tensor = GetInputTensor(bitmap);
			var output = Run(input_tensor);
			var (ind, final_scores, final_boxes, final_classes) = NMS(output);

			int len = ind.Length;
			AnnoData[] data = new ADRect[len];
			Vector2 scale = new Vector2((float) img_file.width / (float) width, (float) img_file.height / (float) height);
			// 根据 result 建立 annodata
			for ( int i = 0; i < len; i += 1 ) {
				ReadOnlySpan<Vector2> points = new ReadOnlySpan<Vector2>(
					new Vector2[] {
						// 将输出的点映射到源图像上的点
						new Vector2(final_boxes[i].X,final_boxes[i].Y)*scale,
						new Vector2(final_boxes[i].X+final_boxes[i].Width,final_boxes[i].Y)*scale,
						new Vector2(final_boxes[i].X,final_boxes[i].Y+final_boxes[i].Height)*scale,
						new Vector2(final_boxes[i].X+final_boxes[i].Width,final_boxes[i].Y+final_boxes[i].Height)*scale,
						}
					);
				int c = final_classes[i];
				data[i] = new ADRect(
					points, c, label: labels[c], conf: final_scores[i]
					);
			}
			return data;
		}
	}
}
