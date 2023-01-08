using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace QLabel.Scripts.Inference_Machine {
	public abstract class BaseInferenceMachine {
		protected string model_path;
		protected InferenceSession session;

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
		protected abstract Bitmap LoadImage (ImageFileData data);
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
