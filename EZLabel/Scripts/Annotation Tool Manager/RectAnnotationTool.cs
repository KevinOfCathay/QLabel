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
using QLabel.Scripts.Projects;

namespace QLabel.Scripts.AnnotationToolManager {
	public class RectAnnotationTool : ToolBase {
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

		public void DrawRectangle (MainCanvas canvas, Vector2 tl, Vector2 br) {
			rect = new DraggableRectangle();
			rect.Draw(canvas.annotation_canvas, new Vector2[] { tl, br });
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
					float top = (float) y, bottom = (float) cur_p.Y, left = (float) x, right = (float) cur_p.X;
					// 如果当前的点小于起始点，则需要调整正方形的起始位置
					if ( cur_p.X < x ) {
						(left, right) = (right, left);          // 交换值
					}
					if ( cur_p.Y < y ) {
						(top, bottom) = (bottom, top);          // 交换值
					}
					rect.Redraw(canvas.annotation_canvas, new Vector2(top, left), right - left, bottom - top);
				}
			}
		}
		public void StopDraw (MainCanvas canvas, MouseEventArgs e) {
			dragging = false;
			e.Handled = true;

			if ( canvas.can_annotate ) {
				if ( rect != null ) {

					// 计算矩形四个点的实际位置
					float left = (float) Canvas.GetLeft(rect);
					float top = (float) Canvas.GetTop(rect);
					float w = (float) rect.ActualWidth;
					float h = (float) rect.ActualHeight;

					var tl_real = canvas.RealPosition(new Vector2(left, top));    // 左上角
					var tr_real = canvas.RealPosition(new Vector2(left + w, top));     // 右上角
					var bl_real = canvas.RealPosition(new Vector2(left, top + h));     // 左下角
					var br_real = canvas.RealPosition(new Vector2(left + w, top + h));     // 右下角

					// 创建 anno 数据
					var data = new AnnotationData.ADRect(
						new ReadOnlySpan<Vector2>(new Vector2[] { tl_real, tr_real, bl_real, br_real }),
						 ProjectManager.cur_label
						);
					rect.data = data;

					CreateAnnotationElementManual create_rect = new CreateAnnotationElementManual(rect, canvas);
					create_rect.Do();

					ActionManager.PushAction(create_rect);
				}
			}
		}
		/// <summary>
		/// 创建一个 annotation 并将其放置在画布上
		/// </summary>
		public static void Create (MainCanvas canvas, Vector2[] rpoints, ClassLabel clas, string label = "") {
			if ( canvas.can_annotate ) {
				// 创建一个新的矩形
				var data = new AnnotationData.ADRect(new ReadOnlySpan<Vector2>(rpoints), clas, label);
				DraggableRectangle r = new DraggableRectangle {
					data = data
				};

				canvas.annotation_canvas.Children.Add(r);
				CreateAnnotationElementManual create_rect = new CreateAnnotationElementManual(r, canvas);
				create_rect.Do();

				ActionManager.PushAction(create_rect);
			}
		}
	}
}
