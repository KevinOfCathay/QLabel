using EZLabel.Scripts.AnnotationToolManager;
using EZLabel.Windows.Main_Canvas;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EZLabel.Scripts.AnnotationToolManager {
	public class RectangularBoxAnnotationTool : ToolBase {
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
			if ( canvas.can_annotate ) {
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
		}
		public void ResizeRectangle (MainCanvas canvas, MouseEventArgs e) {
			if ( canvas.can_annotate && dragging ) {
				var cur_p = e.GetPosition(canvas.annotation_canvas);
				if ( rect != null ) {
					double top = y, bottom = cur_p.Y, left = x, right = cur_p.X;
					// 如果当前的点小于起始点，则需要调整正方形的起始位置
					if ( cur_p.X < x ) {
						(left, right) = (right, left);          // 交换值
					}
					if ( cur_p.Y < y ) {
						(top, bottom) = (bottom, top);          // 交换值
					}
					rect.Redraw(canvas.annotation_canvas, new Point(top, left), right - left, bottom - top);
				}
			}
		}
		public void StopDraw (MainCanvas canvas, MouseEventArgs e) {
			dragging = false;
			e.Handled = true;

			if ( canvas.can_annotate ) {
				if ( rect != null ) {
					// 计算矩形四个点的实际位置
					double left = Canvas.GetLeft(rect);
					double top = Canvas.GetTop(rect);
					var tl_real = canvas.RelativePosition(new Point(left, top));    // 左上角
					var tr_real = canvas.RelativePosition(new Point(left + rect.ActualWidth, top));     // 右上角
					var bl_real = canvas.RelativePosition(new Point(left, top + rect.ActualHeight));     // 左下角
					var br_real = canvas.RelativePosition(new Point(left + rect.ActualWidth, top + rect.ActualHeight));     // 右下角

					// 创建 anno 数据
					rect.data = new AnnotationData.ADRect() {
						points = new Vector2[] { tl_real, tr_real, bl_real, br_real }
					};

					canvas.AddAnnoElements(rect);
				}
			}
		}
	}
}
