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
	/// <summary>
	/// PANet Text Detection
	/// </summary>
	public class PANetInference : BaseInferenceMachine {
		public int width, height, classes;
		public string[] labels;

		/// <summary>
		/// 初始化所有参数
		/// </summary>
		/// <param name="path">模型的路径</param>
		public PANetInference (string path, int width = 640, int height = 640, int classes = 80) {
			model_path = path;
			this.width = width;
			this.height = height;
			this.classes = classes;
			this.labels = ClassLabels.coco80;
		}

		public override Bitmap LoadImage (ImageFileData data) {
			return new Bitmap(Image.FromFile(data.filename), width, height);
		}
	}
}
