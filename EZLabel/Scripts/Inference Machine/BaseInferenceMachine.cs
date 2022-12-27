using Microsoft.ML.OnnxRuntime;
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
		public abstract Bitmap LoadImage (ImageFileData data);

	}
}
