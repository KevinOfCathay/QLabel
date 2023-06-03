using QLabel.Scripts.Inference_Machine;
using System.Collections.Generic;

namespace QLabel.Scripts {
	internal static class ModelSets {
		public static ModelSet[] sets = new ModelSet[] {
			new ModelSet {
				name = "Manga Object Detection",
				model_configs = new Config[] {
								new Yolov7Config (
										model_name : "v230322",
										model_path : @"Resources/Models/yolov7-manga-230322.onnx",
										input_dims: new int[4]{1,3,640,640},
										output_dims: new int[2]{-1, 7},
										class_labels : ModelLabels.manga_v230322
									){ tags = new string[]{ "yolov7", "640×640", "69.3GFLOPS", "map.5=47.6%", "23-03-22" }},
								new Yolov7Config (
										model_name : "v230224",
										model_path : @"Resources/Models/yolov7-manga-230224.onnx",
										input_dims: new int[4]{1,3,640,640},
										output_dims: new int[2]{-1, 7},
										class_labels : ModelLabels.manga_v230224
									){ tags = new string[]{ "yolov7", "640×640", "75.5 GFlops", "map.5=50.4%", "23-02-24" }},
								new Yolov7Config (
										model_name : "v230214",
										model_path : @"Resources/Models/yolov7-manga-230214.onnx",
										input_dims: new int[4]{1,3,640,640},
										output_dims: new int[2]{-1, 7},
										class_labels : ModelLabels.manga_v2302
									){ tags = new string[]{ "yolov7", "640×640", "62 GFlops", "23-02-14" } },
								new Yolov7Config (
										model_name : "v230130",
										model_path : @"Resources/Models/yolov7-manga-230130.onnx",
										input_dims: new int[4]{1,3,640,640},
										output_dims: new int[2]{-1, 7},
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
													class_labels : ModelLabels.keypoints17,
													skeletons: ModelLabels.keypoints17_skeleton,
													input_dims: new int[] { 1, 3, 384, 288 },
													output_dims: new int[] { 1, 17, 96, 72 }
											),
											new HRNetBUConfig (
													model_name : "Keypoint Detection (HRNet Bottom-up)",
													model_path : @"Resources/Models/associative_embedding_higher_hrnet_512x512_crowdpose.onnx",
													post_model_path : @"Resources/Models/associative_embedding_hrnet_post_processing_rmv.onnx",
													class_labels : ModelLabels.keypoints17,
													skeletons: ModelLabels.keypoints17_skeleton,
													input_dims: new int[] { 1, 3, 512, 512 },
													output_dims: new int[] { 1, 28, 128, 128 }
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
			new ModelSet {
				name = "Text Recognition",
				model_configs = new Config[] {
											new PaddleRecConfig (
													model_name : "PaddleOCR Recognition",
													model_path : @"Resources/Models/paddle_rec_jp.onnx",
													charset: ModelLabels.jp_charset,
													class_labels : ModelLabels.textdet,
													input_dims: new int[] { 1, 3, 48, -1 },
													output_dims: new int[] { 1, -1, 4401 }
											){},
				}
			},
		};
	}
}
