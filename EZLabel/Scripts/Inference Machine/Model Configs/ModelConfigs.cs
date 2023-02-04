using System.Collections.Generic;

namespace QLabel.Scripts {
	internal static class ModelConfigs {
		public static List<Config> configs = new List<Config> {
			new Yolov7Config (
				model_name : "Manga Object Detection",
				model_path : @"Resources/Models/yolov7-manga-230130.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.manga
			){ tags = new string[]{ "yolov7", "62G Flops", "23-01-30" } },
			new Yolov5Config (
				model_name : "COCO Object Detection",
				model_path : @"Resources/Models/yolov5m-coco.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.coco80
			){tags = new string[]{ "yolov5", "medium" } },
			new Yolov5Config (
				model_name : "VOC Object Detection (Yolov5m)",
				model_path : @"Resources/Models/yolov5m-voc.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.voc
			),
			new HRNetConfig (
				model_name : "Keypoint Detection (HRNet)",
				model_path : @"topdown_hrnet_w32_coco_384x288_rm_inzr.onnx",
				width : 640, height : 640,
				class_labels : ModelLabels.keypoints16
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
