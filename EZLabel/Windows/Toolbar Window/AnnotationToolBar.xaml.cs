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
		public enum Tool { Mouse, Dot, Rectangle, Square, Tetragon, Polygon, Circle, Selection }

		private static Brush button_background_normal = new SolidColorBrush(Color.FromArgb(255, 221, 221, 211));
		private static Brush button_background_highlight = new SolidColorBrush(Color.FromArgb(255, 189, 205, 214));
		private Tool _cur_tool = Tool.Selection;
		private ToolBase selected_tool = null;
		private MainCanvas mc { get; set; }

		public Button[] button_group;
		/// <summary>
		/// <para>Get: 返回当前选择的 Tool </para>
		/// <para>Set: 设置 Tool </para>
		/// </summary>
		public Tool cur_tool { get { return _cur_tool; } set { SetCurrentTool(value); } }

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
			cur_tool = Tool.Mouse;        // 设置 default tool 为鼠标
		}
		public void Init (MainCanvas mc) {
			this.mc = mc;
		}
		public void SetCurrentTool (Tool tool) {
			if ( tool == _cur_tool ) { return; }
			NormalButton(button_group[(int) _cur_tool]);
			Button button = button_group[(int) tool];

			// 切换 selected_tool
			SwitchTool(tool);
			_cur_tool = tool;

			HighlightButton(button);
		}
		private void SwitchTool (Tool tool) {
			switch ( tool ) {
				case Tool.Mouse:
					SwitchTool(new MouseTool());
					break;
				case Tool.Dot:
					SwitchTool(new DotAnnotationTool());
					break;
				case Tool.Rectangle:
					SwitchTool(new RectAnnotationTool());
					break;
				default:
					break;
			}
		}
		private void SwitchTool (ToolBase tool) {
			if ( mc != null ) {
				if ( this.selected_tool != null ) { this.selected_tool.Deactivate(mc); }
				this.selected_tool = tool;
				tool.Activate(mc);
			}
		}
		private void ToolClick (object sender, RoutedEventArgs e) {
			NormalButton(button_group[(int) _cur_tool]);
			if ( sender is Button button ) {
				var tool_selected = (Tool) Array.IndexOf(button_group, button);

				// 切换 selected_tool
				SwitchTool(tool_selected);
				HighlightButton(button);
			}
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