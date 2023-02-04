using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Inference_Machine {
	internal abstract class BaseInferenceMachine {
		protected string model_path;
		protected InferenceSession session;
		protected readonly int[] input_dims, output_dims;
		public Action<BaseInferenceMachine>? eRunBefore, eRunAfter;

		public BaseInferenceMachine (int[] input_dims, int[] output_dims) {
			this.input_dims = input_dims;
			this.output_dims = output_dims;
		}
		/// <summary>
		/// 从 path 中加载 session
		/// </summary>
		/// <param name="model_path"></param>
		public void BuildSession () {
			session = new InferenceSession(model_path);
			if ( session == null ) {
				Debug.WriteLine("failed to load session.");
			}
		}
		/// <summary>
		/// 加载图片
		/// </summary>
		protected Bitmap ResizeImage (Bitmap source, int target_width, int target_height) {
			if ( source != null ) {
				int w = source.Width; int h = source.Height;
				var result = new Bitmap(target_width, target_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				using ( Graphics g = Graphics.FromImage((Image) result) ) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
					g.DrawImage(source, 0, 0, target_width, target_height);
				}
				return result;
			} else {
				throw new ArgumentNullException("当前没有任何图片");
			}
		}
		public Bitmap CropImage (Bitmap source, Vector2 xy, Vector2 wh) {
			int width = (int) ( wh.X );
			int height = (int) ( wh.Y );

			Bitmap target = new Bitmap(width, height);
			using ( Graphics g = Graphics.FromImage(target) ) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
				g.DrawImage(source, new Rectangle(0, 0, width, height),
				   new Rectangle((int) xy.X, (int) xy.Y, width, height), GraphicsUnit.Pixel);
			}
			target.Save("f.png");
			return target;
		}
		/// <summary>
		/// 将图片转换为 tensor
		/// </summary>
		protected abstract DenseTensor<float> GetInputTensor (Bitmap image);
		/// <summary>
		/// 执行模型并返回结果
		/// </summary>
		public abstract AnnoData[] RunInference (ImageData data, HashSet<int> class_filter = null);
	}
}
