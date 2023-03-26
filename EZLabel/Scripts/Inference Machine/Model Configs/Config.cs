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
	}
	internal class HRNetConfig : Config {
		public HRNetConfig (
			string model_name, string model_path, ClassLabel[] class_labels, (int x, int y, ClassLabel)[] skeletons) :
			base(model_name, model_path, 288, 384, class_labels,
				new HRNetInference(model_path, class_labels, skeletons,
					input_dims: new int[] { 1, 3, 384, 288 },
					output_dims: new int[] { 1, 17, 96, 72 })
			) { }
	}
	internal class HRNetBUConfig : Config {
		public HRNetBUConfig (
			string model_name, string model_path, string post_model_path,
			ClassLabel[] class_labels, (int x, int y, ClassLabel)[] skeletons,
			int[] input_dims, int[] output_dims) :
			base(model_name, model_path, input_dims[3], input_dims[2], class_labels,
				new HRNetInferenceBU(model_path, post_model_path, class_labels, skeletons,
					input_dims: input_dims,
					output_dims: output_dims)
			) { }
	}
	internal class Yolov5Config : Config {
		public Yolov5Config (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov5Inference(model_path, class_labels, width, height)
				) { }
	}
	internal class Yolov7Config : Config {
		public Yolov7Config (
			string model_name, string model_path, int width, int height, ClassLabel[] class_labels) :
			base(model_name, model_path, width, height, class_labels,
				new Yolov7Inference(model_path, class_labels, width, height)
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
