using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Inference_Machine.Model_Configs {
	public class Config {
		public string model_name;
		public string model_path;
		public int width, height;
	}
	public static class ModelConfigs {
		public static List<Config> configs = new List<Config> {
			new Config {
				model_name = "COCO Object Detection (Yolov5m)",
				model_path = "",
				width = 640, height = 640
			}
		};
	}
}
