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
		private readonly int input_channels, input_height;
		private readonly int output_chars;

		/// <summary>
		/// 文字识别模型使用的字符集
		/// </summary>
		private readonly string charset;

		/// <summary>
		/// 最大的高宽比，
		/// 当输入图像的宽高比 r 大于这个值 max_wh_ratio 时，
		/// 将宽高比 max_wh_ratio 设置为 r，
		/// 否则图像的宽高比为 max_wh_ratio，即设置宽为  320f
		/// </summary>
		private const float _max_wh_ratio = 320f / 48f;
		private float max_wh_ratio = _max_wh_ratio;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		public PaddleRecInference (string model_path, ClassTemplate[] labels, string charset, int[] input_dims, int[] output_dims) :
			base(input_dims, output_dims, labels) {
			base.model_path = model_path;
			this.charset = charset;

			// 输入中，只有高是固定的，宽可变
			this.input_channels = input_dims[1];
			this.input_height = input_dims[2];
			this.output_chars = output_dims[2];
		}
		protected override AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null) {
			var input = GetInputTensor(image);
			var input_node = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("x", input) };
			if ( session != null ) {
				var output = session.Run(input_node).First().AsTensor<float>();  // 返回 tensor 更加容易 index
				PostProcessing(output);
			}
			throw new NotImplementedException();
		}
		protected override DenseTensor<float> GetInputTensor (Bitmap image) {
			// 最大宽度（Pad 之后的宽度）
			float max_w = input_height * _max_wh_ratio;

			// 首先确定输入尺寸（宽）
			float wh_ratio = ( (float) image.Width ) / ( (float) image.Height );
			if ( wh_ratio > _max_wh_ratio ) {
				max_w = image.Height * wh_ratio;
			}

			// 根据固定的高度等比例缩放图像并 Pad
			int image_width = (int) ( ( (float) input_height ) * wh_ratio );
			int input_width = (int) max_w;
			var image_resized = ImageUtils.ResizeBitmap(image, new OpenCvSharp.Size(image_width, input_height));
			var image_padded = ImageUtils.Pad(image_resized, new OpenCvSharp.Size(input_width, input_height));

			// 构建 Tensor
			var input = new DenseTensor<float>(new int[4] { 1, 3, input_height, input_width });
			BitmapData bitmap_data = image_padded.LockBits(new Rectangle(0, 0, input_width, input_height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			int bytesPerPixel = Image.GetPixelFormatSize(bitmap_data.PixelFormat) / 8;
			int stride = bitmap_data.Stride;

			float count = input_width * input_height;
			unsafe {
				Parallel.For(0, input_height, (y) => {
					Parallel.For(0, input_width, (x) => {
						// 将范围缩放到 [-1, 1]
						var rgb = (byte*) ( bitmap_data.Scan0 + y * stride );
						float R = (float) rgb[x * bytesPerPixel + 0] / 127.5f - 1f;
						float G = (float) rgb[x * bytesPerPixel + 1] / 127.5f - 1f;
						float B = (float) rgb[x * bytesPerPixel + 2] / 127.5f - 1f;

						// 这里的顺序应当为 batch, channel, y, x
						input[0, 0, y, x] = R;
						input[0, 1, y, x] = G;
						input[0, 2, y, x] = B;
					});
				});
			}
			return input;
		}
		private void PostProcessing (Tensor<float> output) {
			int width = output.Dimensions[1];
			int chars = output.Dimensions[2];

			float[] preds_prob = new float[width];
			float[] preds_idx = new float[width];
			Parallel.For(0, width, (w) => {
				float max = float.MinValue;
				int argmax = 0;
				Parallel.For(0, chars, (c) => {
					if ( output[0, w, c] > max ) {
						max = output[1, w, c]; argmax = c;
					}
				});
				preds_prob[w] = max;
				preds_idx[w] = argmax;
			});

			// Decode
			var ignored_tokens = new List<int> { 0 };

		}
	}
}
