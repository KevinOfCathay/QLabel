using EZLabel.Scripts.AnnotationToolManager;
using EZLabel.Windows.Main_Canvas;
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

namespace EZLabel.Windows.Toolbar_Window {
	/// <summary>
	/// Interaction logic for AnnotationToolBar.xaml
	/// </summary>
	public partial class AnnotationToolBar : UserControl {
		private ToolManager manager = new ToolManager();

		public AnnotationToolBar () {
			InitializeComponent();
		}
		public void Init (MainCanvas mc) {
			manager.mc = mc;
		}
		private void RectangleTool_Button_Click (object sender, RoutedEventArgs e) {
			manager.SwitchTool(new RectangularBoxAnnotationTool());
		}

		private void DotTool_Button_Click (object sender, RoutedEventArgs e) {
			manager.SwitchTool(new DotAnnotationTool());
		}
	}
}
