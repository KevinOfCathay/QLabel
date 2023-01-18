using QLabel.Scripts.Inference_Machine;
using QLabel.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts {
	public class Config {
		public Config (
			string model_name,
			string model_path,
			int width,
			int height,
			string[] class_labels,
			BaseInferenceMachine inf) {
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
		public readonly string[] class_labels;
		public readonly BaseInferenceMachine inf;

		/// <summary>
		/// 从 label 中创建 ClassLabel
		/// </summary>
		/// <returns></returns>
		public ClassLabel[] GetClassLabels () {
			if ( class_labels == null ) { return new ClassLabel[0]; }
			ClassLabel[] labels = new ClassLabel[class_labels.Length];
			int i = 0;
			foreach ( var label in class_labels ) {
				labels[i] = new ClassLabel(
					group: null,
					name: label) {
					color = new Color { R = (byte) RNG.rng.Next(100, 250), G = (byte) RNG.rng.Next(100, 250), B = (byte) RNG.rng.Next(100, 250) }
				};
			}
			return labels;
		}
	}

	public class Yolov5Config : Config {
		public Yolov5Config (
			string model_name, string model_path, int width, int height, string[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov5Inference(model_path, class_labels, width, height, class_labels.Length)
				) { }
	}
	public class Yolov7Config : Config {
		public Yolov7Config (
			string model_name, string model_path, int width, int height, string[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov7Inference(model_path, class_labels, width, height, class_labels.Length)
				) { }
	}
}
