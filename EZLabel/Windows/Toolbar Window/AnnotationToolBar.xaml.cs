using QLabel.Scripts.AnnotationToolManager;
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

namespace QLabel.Windows.Toolbar_Window {
	/// <summary>
	/// Interaction logic for AnnotationToolBar.xaml
	/// </summary>
	public partial class AnnotationToolBar : UserControl {
		private static Brush button_background_normal = new SolidColorBrush(Color.FromArgb(255, 221, 221, 211));
		private static Brush button_background_highlight = new SolidColorBrush(Color.FromArgb(255, 160, 150, 210));
		private ToolBase selected_tool = null;
		private MainCanvas mc { get; set; }

		public Button[] button_group;
		public int cur_button_index = 0;

		public AnnotationToolBar () {
			InitializeComponent();

			button_group = new Button[] {
				Mouse_Button,
				DotTool_Button,
				RectangleTool_Button,
				SquareTool_Button,
				Tetragon_Button,
				PolygonTool_Button,
				Circle_Button,
				Selection_Button
			};
		}
		public void Init (MainCanvas mc) {
			this.mc = mc;
		}
		public void SetCurrentTool (int tool_index) {
			NormalButton(button_group[cur_button_index]);
			Button button = button_group[tool_index];

			// 切换 selected_tool
			SwitchTool(cur_button_index);
			cur_button_index = tool_index;

			HighlightButton(button);
		}
		private void SwitchTool (int tool_index) {
			switch ( tool_index ) {
				case 0:
					SwitchTool(new MouseTool());
					break;
				case 1:
					SwitchTool(new DotAnnotationTool());
					break;
				case 2:
					SwitchTool(new RectAnnotationTool());
					break;
				default:
					break;
			}
		}
		public void SwitchTool (ToolBase tool) {
			if ( mc != null ) {
				if ( this.selected_tool != null ) { this.selected_tool.Deactivate(mc); }
				this.selected_tool = tool;
				tool.Activate(mc);
			}
		}
		private void ToolClick (object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			NormalButton(button_group[cur_button_index]);
			cur_button_index = Array.IndexOf(button_group, button);

			// 切换 selected_tool
			SwitchTool(cur_button_index);

			HighlightButton(button);
			e.Handled = true;
		}
		private void HighlightButton (Button button) {
			button.Background = button_background_highlight;
		}
		private void NormalButton (Button button) {
			button.Background = button_background_normal;
		}
	}
}
