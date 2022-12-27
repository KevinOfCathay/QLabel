using QLabel.Scripts.Inference_Machine;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLabel.Windows.Misc_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AutoAnnotatePanel.xaml
	/// </summary>
	public partial class AutoAnnotatePanel : UserControl {
		private Yolov5Inference machine = new Yolov5Inference(@"Resources/Models/yolov5m-coco.onnx");
		private MainCanvas canvas;         // TODO: replace this
		public AutoAnnotatePanel () {
			InitializeComponent();

			if ( App.main != null ) {     // TODO: replace this
				canvas = App.main.main_canvas;
			}
		}

		private void apply_button_Click (object sender, RoutedEventArgs e) {
			machine.BuildSession();
			if ( canvas != null ) {
				var image = machine.LoadImage(canvas.cur_file);
				if ( image != null ) {
					var input = machine.GetInputTensor(image);
					var output = machine.RunInference(input);
					var result = machine.NMS(output);
				}
			}
		}

		private void apply_all_button_Click (object sender, RoutedEventArgs e) {

		}
	}
}
