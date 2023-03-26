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

		public BaseInferenceMachine (int[] input_dims, int[] output_dims) {
			this.input_dims = input_dims;
			this.output_dims = output_dims;
		}
		/// <summary>
		/// 从 path 中加载 session
		/// </summary>
		/// <param name="model_path"></param>
		public virtual void BuildSession () {
			session = new InferenceSession(model_path);
			if ( session == null ) {
				Debug.WriteLine("Failed to load session.");
			}
		}
		/// <summary>
		/// 将图片转换为 tensor
		/// </summary>
		protected abstract DenseTensor<float> GetInputTensor (Bitmap image);
		/// <summary>
		/// 执行模型并返回结果
		/// </summary>
		public abstract AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null);
	}
}
