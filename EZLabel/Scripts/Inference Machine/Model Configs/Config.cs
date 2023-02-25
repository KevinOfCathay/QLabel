using QLabel.Scripts.Inference_Machine;
using QLabel.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts {
	internal class Config {
		public Config (
			string model_name, string model_path,
			int width, int height, ClassLabel[] class_labels, BaseInferenceMachine inf) {

			this.model_name = model_name;
			this.model_path = model_path;
			this.width = width;
			this.height = height;
			this.class_labels = class_labels;
			this.inf = inf;
		}
		public readonly string model_name;
		public readonly string model_path;
		public readonly int width, height;
		public readonly ClassLabel[] class_labels;
		public readonly BaseInferenceMachine inf;
		public string[] tags = new string[0];     // 这个模型的额外标签（用于描述这个模型）

		/// <summary>
		/// 从 label 中创建 ClassLabel
		/// </summary>
		/// <returns></returns>
		private ClassLabel[] GetClassLabels (string[] labels) {
			if ( labels == null ) { return new ClassLabel[0]; }
			ClassLabel[] class_labels = new ClassLabel[labels.Length];
			int i = 0;
			foreach ( var label in labels ) {
				class_labels[i] = new ClassLabel(
					group: null,
					name: label) {
					color = new Color { R = (byte) RNG.rng.Next(100, 250), G = (byte) RNG.rng.Next(100, 250), B = (byte) RNG.rng.Next(100, 250) }
				};
				i += 1;
			}
			return class_labels;
		}
	}
	internal class HRNetConfig : Config {
		public HRNetConfig (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new HRNetInference(model_path, class_labels,
					input_dims: new int[] { 1, 3, 384, 288 },
					output_dims: new int[] { 1, 17, 96, 72 })
			) { }
	}
	internal class Yolov5Config : Config {
		public Yolov5Config (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov5Inference(model_path, class_labels, width, height, class_labels.Length)
				) { }
	}
	internal class Yolov7Config : Config {
		public Yolov7Config (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov7Inference(model_path, class_labels, width, height, class_labels.Length)
				) { }
	}
	internal class PANetConfig : Config {
		public PANetConfig (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new PANetInference(model_path, class_labels, width, height, class_labels.Length)
				) { }
	}
}
