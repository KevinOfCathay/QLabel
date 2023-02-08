using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 矩形
	/// </summary>
	public partial class DraggableRectangle : UserControl, IAnnotationElement {
		private enum DotType { Center, TopLeft, TopRight, BottomLeft, BottomRight }

		/// <summary>
		/// 当长方形被重新绘制时触发
		/// </summary>
		public Action<DraggableRectangle, float, float> eDraw;
		public Dot[] dots = new Dot[5];
		public Vector2 size {
			get { return new Vector2(MathF.Abs(bottomright.X - topleft.X), MathF.Abs(bottomright.Y - topleft.Y)); }
		}
		private Vector2 topleft, bottomright;


		private Vector2 mouse_down_position, mouse_cur_position;
		private int dot_click_index = -1;

		AnnoData _data;   // 这个矩形所对应的注释数据

		public AnnoData data {
			get { return _data; }
			set { _data = value; this.class_label.Content = value.clas.name; }
		}
		public Vector2[] cpoints {
			get {
				float left = (float) Canvas.GetLeft(this);
				float top = (float) Canvas.GetTop(this);
				float w = (float) ActualWidth;
				float h = (float) ActualHeight;
				return new Vector2[] {
					new Vector2(left, top),    // 左上角
					new Vector2(left + w, top),     // 右上角
					new Vector2(left, top + h),     // 左下角
					new Vector2(left + w, top + h)     // 右下角
				};
			}
		}

		public DraggableRectangle () {
			InitializeComponent();

			// 初始化宽和高为 0
			container.Width = 0;
			container.Height = 0;

			dots[0] = center_dot;
			dots[1] = top_left_dot;
			dots[2] = top_right_dot;
			dots[3] = bottom_left_dot;
			dots[4] = bottom_right_dot;

			RegisterEvents();
		}
		public void RegisterEvents () {
			for ( int i = 0; i < 5; i += 1 ) {
				var index = i;
				dots[i].eMouseEnter += (dot, e) => { this.dot_click_index = index; };
			}
		}
		/// <summary>
		/// 绘制矩形区域
		/// </summary>
		public void Resize (Canvas canvas, float width, float height) {
			container.Width = MathF.Abs(width);
			container.Height = MathF.Abs(height);
			eDraw?.Invoke(this, width, height);
		}
		/// <summary>
		/// 设置标签（显示）
		/// </summary>
		public void SetLabel (string label) {
			class_label.Content = label;
		}
		private void container_MouseEnter (object sender, MouseEventArgs e) {
			// 改变四个点的透明度
			foreach ( var dot in dots ) {
				dot.dot.Stroke = IAnnotationElement.brush_dot_stroke_solid;
				dot.dot.Fill = IAnnotationElement.brush_dot_fill_solid;
			}
			class_label.Foreground = IAnnotationElement.brush_label_text_foreground_solid;
			class_label.Background = IAnnotationElement.brush_label_text_background_solid;
		}
		private void container_MouseLeave (object sender, MouseEventArgs e) {
			// 改变四个点的透明度
			foreach ( var dot in dots ) {
				dot.dot.Stroke = IAnnotationElement.brush_dot_stroke_transparent;
				dot.dot.Fill = IAnnotationElement.brush_dot_fill_transparent;
			}
			class_label.Foreground = IAnnotationElement.brush_label_text_foreground_transparent;
			class_label.Background = IAnnotationElement.brush_label_text_background_transparent;
		}
		/// <summary>
		/// 从画布上移除这个元素
		/// </summary>
		public void Delete (MainCanvas canvas) {
			canvas.annotation_canvas.Children.Remove(this);
		}
		/// <summary>
		/// 在画布上绘制/重新绘制
		/// 矩形区域的大小与位置由左上角的顶点 tl (left, top) 与右下角的顶点 br (bottom, right) 确定
		/// </summary>
		public void Draw (Canvas canvas, Vector2[] points) {
			// 定义矩形 (左上角，右下角) 的位置 
			Vector2 tl, br;
			switch ( points.Length ) {
				case 2:
					tl = points[0]; br = points[1];
					break;
				case 4:
					tl = points[0]; br = points[3];
					break;
				default:
					throw new ArgumentException("The length of points should be either 2 or 4.");
			}
			Canvas.SetLeft(this, tl.X); this.topleft = tl;
			Canvas.SetTop(this, tl.Y); this.bottomright = br;

			// 定义矩形的长和宽
			// X: width, Y: height
			container.Width = size.X;
			container.Height = size.Y;

			// 触发绘制/重新绘制事件
			eDraw?.Invoke(this, size.X, size.Y);
		}
		public void Shift (Canvas canvas, Vector2 shift) {
			this.topleft += shift;
			this.bottomright += shift;

			Canvas.SetLeft(this, this.topleft.X);
			Canvas.SetTop(this, this.topleft.Y);

			// 触发绘制/重新绘制事件
			eDraw?.Invoke(this, size.X, size.Y);
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
		public new void MouseDown (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var position = e.GetPosition(canvas);
			mouse_down_position = new Vector2((float) position.X, (float) position.Y);
			mouse_cur_position = mouse_down_position;
		}
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var position = e.GetPosition(canvas);
			var mouse_temp_position = new Vector2((float) position.X, (float) position.Y);
			var shift = mouse_temp_position - mouse_cur_position;

			switch ( dot_click_index ) {
				case 0:   // Center
					Shift(canvas.annotation_canvas, shift);
					break;
				case 1:   // Top Left
					Draw(canvas.annotation_canvas, new Vector2[] { this.topleft + shift, this.bottomright });
					break;
				case 2:   // Top Right
					Draw(canvas.annotation_canvas, new Vector2[] { this.topleft + new Vector2(0f, shift.Y), this.bottomright + new Vector2(shift.X, 0f) });
					break;
				case 3:   // Bottom Left
					Draw(canvas.annotation_canvas, new Vector2[] { this.topleft + new Vector2(shift.X, 0), this.bottomright + new Vector2(0f, shift.Y) });
					break;
				case 4:   // Bottom Right
					Draw(canvas.annotation_canvas, new Vector2[] { this.topleft, this.bottomright + shift });
					break;
				default:
					break;
			}
			mouse_cur_position = mouse_temp_position;
		}
		public new void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			// 创建新的 AnnoData
			ADRect new_data = new ADRect(
				new ReadOnlySpan<Vector2>(new Vector2[] {
					canvas.RealPosition(cpoints[0]), canvas.RealPosition(cpoints[1]),
					canvas.RealPosition(cpoints[2]), canvas.RealPosition(cpoints[3]) }),
				data.clas, data.label, data.conf);
			ChangeRectSize changesize = new ChangeRectSize(canvas, this, data, new_data);
			changesize.Do();
			ActionManager.PushAction(changesize);
		}
		public void Highlight () {
			// Error:
			// if ( highlight_storyboard != null ) { BeginStoryboard(highlight_storyboard); }
		}
	}
}
