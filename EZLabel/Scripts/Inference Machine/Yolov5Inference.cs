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

namespace QLabel.Scripts.Inference_Machine {
	public class Yolov5Inference : BaseInferenceMachine {
		public int width, height, classes;
		public string[] labels;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public Yolov5Inference (string path, int width = 640, int height = 640, int classes = 80) {
			model_path = path;
			this.width = width;
			this.height = height;
			this.classes = classes;
			this.labels = ClassLabels.coco80;
		}

		public override Bitmap LoadImage (ImageFileData data) {
			if ( data != null ) {
				return new Bitmap(Image.FromFile(data.path), width, height);
			} else {
				throw new ArgumentNullException("当前没有任何图片");
			}
		}

		/// <summary>
		/// 将图片转换为 Dense Tensor
		/// </summary>
		public DenseTensor<float> GetInputTensor (Bitmap image) {
			var input = new DenseTensor<float>(new[] { 1, 3, height, width });

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
		public float[] RunInference<T> (DenseTensor<T> input) {
			// 从 Dense Tensor 创建一个 Input
			var input_node = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<T>("images", input) };
			if ( session != null ) {
				var output = session.Run(input_node);
				return output.First().AsEnumerable<float>().ToArray();
			} else {
				throw new ApplicationException("Session 未被加载. 无法进行 inference.");
			}
		}

		public (int[] indices, List<float> scores, List<Rect> boxes, List<int> classes) NMS (float[] output) {
			List<Rect> boxes = new List<Rect>();
			List<int> indices = new List<int>();
			List<float> scores = new List<float>();
			List<int> classes = new List<int>();
			Mat res = new Mat(new int[] { 25200, this.classes + 5 }, MatType.CV_32F, output);

			for ( int r = 0; r < 25200; r += 1 ) {
				float cx = res.At<float>(r, 0);
				float cy = res.At<float>(r, 1);
				float w = res.At<float>(r, 2);
				float h = res.At<float>(r, 3);
				float sc = res.At<float>(r, 4);
				Mat confs = res.Row(r).ColRange(5, this.classes + 5);
				confs *= sc;
				double minV, maxV;
				Cv2.MinMaxIdx(confs, out minV, out maxV);
				scores.Add((float) maxV);
				boxes.Add(new Rect(
				    (int) ( cx - w / 2f ),
				    (int) ( cy - h / 2f ),
				    (int) ( w ), (int) ( h )));
				indices.Add(r);
				classes.Add((int) maxV);
			}
			int[] ind = indices.ToArray();
			CvDnn.NMSBoxes(boxes, scores, 0.3f, 0.45f, out ind);

			return (ind, scores, boxes, classes);
		}
	}
}
