using OpenCvSharp;
using QLabel.Scripts.Inference_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	public static class ClassLabels {
		public static string[] manga = new string[] {
			 "female", "text", "laptop", "male", "chatbox", "building", "sky", "cup", "car", "phone", "bag", "unknown", "highlight box", "bowl", "sword", "cup", "city"
		};
		public static string[] coco80 = new string[] {
			  "person"
			 ,"bicycle"
			 ,"car"
			 ,"motorcycle"
			 ,"airplane"
			 ,"bus"
			 ,"train"
			 ,"truck"
			 ,"boat"
			 ,"traffic_light"
			 ,"fire_hydrant"
			 ,"stop_sign"
			 ,"parking_meter"
			 ,"bench"
			 ,"bird"
			 ,"cat"
			 ,"dog"
			 ,"horse"
			 ,"sheep"
			 ,"cow"
			 ,"elephant"
			 ,"bear"
			 ,"zebra"
			 ,"giraffe"
			 ,"backpack"
			 ,"umbrella"
			 ,"handbag"
			 ,"tie"
			 ,"suitcase"
			 ,"frisbee"
			 ,"skis"
			 ,"snowboard"
			 ,"sports_ball"
			 ,"kite"
			 ,"baseball_bat"
			 ,"baseball_glove"
			 ,"skateboard"
			 ,"surfboard"
			 ,"tennis_racket"
			 ,"bottle"
			 ,"wine_glass"
			 ,"cup"
			 ,"fork"
			 ,"knife"
			 ,"spoon"
			 ,"bowl"
			 ,"banana"
			 ,"apple"
			 ,"sandwich"
			 ,"orange"
			 ,"broccoli"
			 ,"carrot"
			 ,"hot_dog"
			 ,"pizza"
			 ,"donut"
			 ,"cake"
			 ,"chair"
			 ,"couch"
			 ,"potted_plant"
			 ,"bed"
			 ,"dining_table"
			 ,"toilet"
			 ,"tv"
			 ,"laptop"
			 ,"mouse"
			 ,"remote"
			 ,"keyboard"
			 ,"cell_phone"
			 ,"microwave"
			 ,"oven"
			 ,"toaster"
			 ,"sink"
			 ,"refrigerato"
			 ,"book"
			 ,"clock"
			 ,"vase"
			 ,"scissors"
			 ,"teddy_bea"
			 ,"hair_drier	"
			 ,"toothbrush"
		};
		public static string[] voc = new string[] { };
		public static string[] text = new string[] { };
	}
	public static class ModelConfigs {
		public static List<Config> configs = new List<Config> {
			new Yolov7Config (
				model_name : "Manga Object Detection (Yolov7)",
				model_path : @"Resources/Models/yolov7-manga.onnx",
				width : 640, height : 640,
				class_labels : ClassLabels.manga
			),
			new Yolov5Config (
				model_name : "COCO Object Detection (Yolov5m)",
				model_path : @"Resources/Models/yolov5m-coco.onnx",
				width : 640, height : 640,
				class_labels : ClassLabels.coco80
			),
			new Yolov5Config (
				model_name : "VOC Object Detection (Yolov5m)",
				model_path : @"Resources/Models/yolov5m-voc.onnx",
				width : 640, height : 640,
				class_labels : ClassLabels.voc
			),
			new Config (
				model_name : "Text Detection (PANet)",
				model_path : "",
				width : 640, height : 640,
				class_labels : ClassLabels.text,
				inf: null
			),
		};
	}
}
