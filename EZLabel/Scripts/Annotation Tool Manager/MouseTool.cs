using QLabel.Scripts.AnnotationToolManager;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace QLabel.Scripts.AnnotationToolManager {
	public class MouseTool : ToolBase {
		private IAnnotationElement selected_elem = null;
		private bool mouse_down = false;
		private Vector2 mouse_pos;

		public override void Activate (MainCanvas canvas) {
			canvas.eMouseDown += MouseDown;
			canvas.eCanvasMouseMove += MouseMove;
			canvas.eMouseUp += MouseUp;
		}
		public override void Deactivate (MainCanvas canvas) {
			canvas.eMouseDown -= MouseDown;
			canvas.eCanvasMouseMove -= MouseMove;
			canvas.eMouseUp -= MouseUp;
		}
		private void MouseDown (MainCanvas canvas, MouseEventArgs e) {
			// 初始化 mouse down 为 false
			mouse_down = false;

			// 用户点击了画布、图像以外的元素
			if ( e.Source != canvas.annotation_canvas && e.Source != canvas.canvas_image ) {
				if ( e.Source is IAnnotationElement ) {
					var elem = e.Source as IAnnotationElement;
					// 触发元素的点击效果
					if ( elem != null ) {
						mouse_down = true;       // mouse down 设置为 true (用户点击了物体)
						selected_elem = elem;
						elem.MouseDown(canvas, e);
					}
				}
			} else {
				// 当用户点击画布、图像时，解除 selected
				App.main.selected_element = null;
				if ( selected_elem != null ) {
					selected_elem.Unselect();
					selected_elem = null;
				}
			}
			e.Handled = true;
		}
		private void MouseMove (MainCanvas canvas, MouseEventArgs e) {
			if ( mouse_down && selected_elem != null ) {
				selected_elem.MouseDrag(canvas, e);
			}
			e.Handled = true;
		}
		private void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			if ( mouse_down && selected_elem != null ) {
				// 将被选中的 element 设置为 selected_element
				selected_elem.Select();
				selected_elem.MouseUp(canvas, e);
			}
			mouse_down = false;
			e.Handled = true;
		}
	}
}
