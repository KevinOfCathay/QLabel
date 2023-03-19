using System.Collections.Generic;

namespace QLabel.Scripts {
	internal static class ModelSets {
		public static ModelSet[] sets = new ModelSet[] {
			new ModelSet {
				name = "Manga Object Detection",
				model_configs = new Config[] {
								new Yolov7Config (
										model_name : "v230224",
										model_path : @"Resources/Models/yolov7-manga-230224.onnx",
										width : 640, height : 640,
										class_labels : ModelLabels.manga_v230224
									){ tags = new string[]{ "yolov7", "640×640", "75.5 GFlops", "map.5=50.4%", "23-02-24" } },
								new Yolov7Config (
										model_name : "v230214",
										model_path : @"Resources/Models/yolov7-manga-230214.onnx",
										width : 640, height : 640,
										class_labels : ModelLabels.manga_v2302
									){ tags = new string[]{ "yolov7", "640×640", "62 GFlops", "23-02-14" } },
								new Yolov7Config (
								model_name : "v230130",
								model_path : @"Resources/Models/yolov7-manga-230130.onnx",
								width : 640, height : 640,
								class_labels : ModelLabels.manga_v2301
									){ tags = new string[]{ "yolov7", "640×640", "62 GFlops", "23-01-30" } },
				}
			},
			new ModelSet {
				name = "COCO Object Detection",
				model_configs = new Config[] {
								new Yolov5Config (
										model_name : "COCO Object Detection",
										model_path : @"Resources/Models/yolov5m-coco.onnx",
										width : 640, height : 640,
										class_labels : ModelLabels.coco80
									){tags = new string[]{ "yolov5", "640×640", "medium", "default" } },
				}
			},
			new ModelSet {
				name = "VOC Object Detection",
				model_configs = new Config[] {
											new Yolov5Config (
												model_name : "VOC Object Detection",
												model_path : @"Resources/Models/yolov5m-voc.onnx",
												width : 640, height : 640,
												class_labels : ModelLabels.voc
											){tags = new string[]{ "yolov5", "640×640", "medium" } },
				}
			},
			new ModelSet {
				name = "Body Keypoint Detection",
				model_configs = new Config[] {
											new HRNetConfig (
													model_name : "Keypoint Detection (HRNet)",
													model_path : @"Resources/Models/topdown_hrnet_w32_coco_384x288_rm_inzr.onnx",
													width : 640, height : 640,
													class_labels : ModelLabels.keypoints16,
													skeletons: ModelLabels.keypoints16_skeleton
											){},
				}
			},
			new ModelSet {
				name = "Text Detection",
				model_configs = new Config[] {
											new PANetConfig (
													model_name : "Text Detection (PANet)",
													model_path : @"Resources/Models/PAnet-text-detection.onnx",
													width : 640, height : 640,
													class_labels : ModelLabels.textdet
											){},
				}
			},
		};
	}
}
