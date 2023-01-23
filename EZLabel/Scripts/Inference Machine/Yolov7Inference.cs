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

namespace QLabel.Scripts.Inference_Machine {
	internal sealed class Yolov7Inference : BaseInferenceMachine {
		public int width, height, classes;
		private string[] labels;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public Yolov7Inference (string path, string[] labels, int width = 640, int height = 640, int classes = 14) :
			base(new[] { 1, 3, height, width }, null) {
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

		public override AnnoData[] RunInference (ImageData img_file, HashSet<int> class_filter = null) {
			eRunBefore?.Invoke(this);

			var bitmap = LoadImage(img_file, width, height);

			var input_tensor = GetInputTensor(bitmap);
			var output = Run(input_tensor);

			int len = output.Length;

			List<AnnoData> data = new List<AnnoData>();
			Vector2 scale = new Vector2((float) img_file.width / (float) width, (float) img_file.height / (float) height);
			// 根据 result 建立 annodata
			for ( int i = 0; i < len / 7; i += 1 ) {
				float x = ClipX(output[i * 7 + 1]);
				float y = ClipY(output[i * 7 + 2]);
				float x2 = ClipX(output[i * 7 + 3]);
				float y2 = ClipY(output[i * 7 + 4]);


				ReadOnlySpan<Vector2> points = new ReadOnlySpan<Vector2>(
					new Vector2[] {
						// 将输出的点映射到源图像上的点
						new Vector2(x, y)*scale,
						new Vector2(x2, y)*scale,
						new Vector2(x, y2)*scale,
						new Vector2(x2, y2)*scale,
						}
					);
				int c = (int) output[i * 7 + 5];

				// 类别过滤
				if ( class_filter != null ) {
					if ( !class_filter.Contains(c) ) {
						continue;
					}
				}
				ClassLabel cl = new ClassLabel(group: "None", name: labels[c]);
				data.Add(new ADRect(
					points, cl, label: "", conf: output[i * 7 + 6]
					));
			}

			eRunAfter?.Invoke(this);
			return data.ToArray();
		}
		private float ClipX (float x) {
			if ( x < 0 ) { return 0; }
			if ( x >= width ) { return width - 1; }
			return x;
		}
		private float ClipY (float y) {
			if ( y < 0 ) { return 0; }
			if ( y >= height ) { return height - 1; }
			return y;
		}
	}
}
