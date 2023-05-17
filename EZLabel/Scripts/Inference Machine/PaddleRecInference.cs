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
	internal sealed class PaddleRecInference : InferenceBase {
		private readonly ClassLabel[] labels;
		private readonly int input_channels, input_height;
		private readonly int output_chars;
		private readonly string charset;
		private bool post_process = true;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		public PaddleRecInference (string model_path, ClassLabel[] labels, string charset, int[] input_dims, int[] output_dims) :
			base(input_dims, output_dims) {
			base.model_path = model_path;
			this.labels = labels;
			this.charset = charset;

			// 输入中，只有高是固定的，宽可变
			this.input_channels = input_dims[1];
			this.input_height = input_dims[2];
			this.output_chars = output_dims[2];
		}
		public override AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null) {
			throw new NotImplementedException();
		}
		protected override DenseTensor<float> GetInputTensor (Bitmap image) {
			var input = new DenseTensor<float>(input_dims);
			int image_width = 0;

			// https://stackoverflow.com/a/74337947
			BitmapData bitmap_data = image.LockBits(new Rectangle(0, 0, image_width, input_height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			int bytesPerPixel = Image.GetPixelFormatSize(bitmap_data.PixelFormat) / 8;
			int stride = bitmap_data.Stride;

			float count = image_width * input_height;
			unsafe {
				Parallel.For(0, input_height, (y) => {
					Parallel.For(0, image_width, (x) => {
						var rgb = (byte*) ( bitmap_data.Scan0 + y * stride );
						float R = (float) rgb[x * bytesPerPixel + 0] / 255f;
						float G = (float) rgb[x * bytesPerPixel + 1] / 255f;
						float B = (float) rgb[x * bytesPerPixel + 2] / 255f;

						// 这里的顺序应当为 batch, channel, y, x
						input[0, 0, y, x] = R;
						input[0, 1, y, x] = G;
						input[0, 2, y, x] = B;
					});
				});
			}
			throw new NotImplementedException();
		}
	}
}
