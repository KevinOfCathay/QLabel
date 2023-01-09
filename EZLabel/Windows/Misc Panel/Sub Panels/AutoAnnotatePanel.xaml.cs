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
		private Yolov5Inference machine = new Yolov5Inference(@"Resources/Models/yolov5m-coco.onnx", ClassLabels.coco80);
		private MainCanvas canvas;         // TODO: replace this
		public AutoAnnotatePanel () {
			InitializeComponent();

			if ( App.main != null ) {     // TODO: replace this
				canvas = App.main.main_canvas;
			}

			InitializeListItems();
		}
		/// <summary>
		/// 动态的初始化列表中的所有元素
		/// </summary>
		private void InitializeListItems () {
			// 读取所有的 model config
			foreach ( var config in ModelConfigs.configs ) {
				ListBoxItem item = new ListBoxItem();
				item.Content = config.model_name;

				string[] labels = config.class_labels;
				item.Selected += (object sender, RoutedEventArgs e) => {
					SetClassLabels(labels);
				};
				model_list.Items.Add(item);
			}
		}
		private void SetClassLabels (string[] labels) {
			class_list.Items.Clear();
			// 更改 class list 中的所有元素
			foreach ( var label in labels ) {
				var new_item = new ListBoxItem();
				new_item.Content = label;
				class_list.Items.Add(new_item);
			}
		}
		/// <summary>
		/// 当前的 inference 只适用于当前被打开的图像
		/// </summary>
		private void apply_button_Click (object sender, RoutedEventArgs e) {
			machine.BuildSession();
			if ( canvas != null && canvas.cur_file != null ) {
				var ads = machine.RunInference(canvas.cur_file);
				foreach ( var ad in ads ) {
					var element = ad.CreateAnnotationElement(canvas);
					canvas.AddAnnoElements(element);
				}
			}
		}
		private void apply_all_button_Click (object sender, RoutedEventArgs e) {

		}
	}
}
