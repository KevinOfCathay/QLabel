using EZLabel.Scripts.AnnotationToolManager;
using EZLabel.Windows.Main_Canvas;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EZLabel.Scripts.AnnotationToolManager {
	public class RectangularBoxAnnotationTool : AnnotationToolBase {
		DraggableRectangle rect;
		bool dragging = false;
		double x, y;

		public override void Activate (MainCanvas canvas) {
			canvas.eMouseDown += CreateNewRectangle;
			canvas.eMouseMove += ResizeRectangle;
			canvas.eMouseUp += StopDraw;
		}
		public override void Deactivate (MainCanvas canvas) {
			canvas.eMouseDown -= CreateNewRectangle;
			canvas.eMouseMove -= ResizeRectangle;
			canvas.eMouseUp -= StopDraw;
		}

		public void DrawRectangle (MainCanvas canvas, Point tl, Point br) {
			rect = new DraggableRectangle();
			rect.Redraw(canvas.annotation_canvas, tl, br);
			canvas.annotation_canvas.Children.Add(rect);
		}

		public void CreateNewRectangle (MainCanvas canvas, MouseEventArgs e) {
			rect = new DraggableRectangle();

			// 记录下起始时刻的 x, y
			var p = e.GetPosition(canvas.annotation_canvas);
			this.x = p.X;
			this.y = p.Y;

			Canvas.SetLeft(rect, x);
			Canvas.SetTop(rect, y);

			canvas.annotation_canvas.Children.Add(rect);

			// 开始拖动
			dragging = true;
			e.Handled = true;
		}
		public void ResizeRectangle (MainCanvas canvas, MouseEventArgs e) {
			if ( dragging ) {
				var p = e.GetPosition(canvas.annotation_canvas);

				if ( rect != null ) {
					rect.Resize(canvas.annotation_canvas, p.X - x, p.Y - y);
				}
			}
		}
		public void StopDraw (MainCanvas canvas, MouseEventArgs e) {
			dragging = false;
			e.Handled = true;

			// 计算矩形四个点的实际位置
			var img_size = canvas.GetImageSize();
			var img_pos = canvas.GetImagePosition();

			// 当前没有图片被加载
			if ( img_pos.X == double.NaN || img_pos.Y == double.NaN ) {
				return;
			} else {

			}
		}
	}
}
