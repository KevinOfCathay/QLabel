using OpenCvSharp;
using QLabel.Scripts.Inference_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	public static class ModelConfigs {
		public static List<Config> configs = new List<Config> {
			new Yolov7Config (
				model_name : "Manga Object Detection (Yolov7)",
				model_path : @"Resources/Models/yolov7-manga-e2e.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.manga
			),
			new Yolov5Config (
				model_name : "COCO Object Detection (Yolov5m)",
				model_path : @"Resources/Models/yolov5m-coco.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.coco80
			),
			new Yolov5Config (
				model_name : "VOC Object Detection (Yolov5m)",
				model_path : @"Resources/Models/yolov5m-voc.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.voc
			),
			new Config (
				model_name : "Text Detection (PANet)",
				model_path : "",
				width : 640, height : 640,
				class_labels : ModelLabels.text,
				inf: null
			),
		};
	}
}
