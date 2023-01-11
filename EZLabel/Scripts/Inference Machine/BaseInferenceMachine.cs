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
using System.Windows.Media.Media3D;

namespace QLabel.Scripts.Inference_Machine {
	public abstract class BaseInferenceMachine {
		protected string model_path;
		protected InferenceSession session;
		protected readonly int[] input_dims, output_dims;
		public Action<BaseInferenceMachine> eRunBefore, eRunAfter;

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
		protected Bitmap LoadImage (ImageFileData data, int target_width, int target_height) {
			if ( data != null ) {
				return new Bitmap(Image.FromFile(data.path), target_width, target_height);
			} else {
				throw new ArgumentNullException("当前没有任何图片");
			}
		}
		/// <summary>
		/// 将图片转换为 tensor
		/// </summary>
		protected abstract DenseTensor<float> GetInputTensor (Bitmap image);
		/// <summary>
		/// 给出
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public abstract AnnoData[] RunInference (ImageFileData data);
	}
}
