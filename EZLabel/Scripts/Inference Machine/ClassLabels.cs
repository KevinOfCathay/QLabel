using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static QLabel.Windows.Annotation_Panel.Sub_Panels.AnnoList;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace QLabel.Scripts.Inference_Machine {
	public static class ClassLabels {
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
	}
}
