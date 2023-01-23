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
using System.Windows.Media.Media3D;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 矩形
	/// </summary>
	public partial class DraggableRectangle : UserControl, IAnnotationElement {
		/// <summary>
		/// 当长方形被重新绘制时触发
		/// </summary>
		public Action<DraggableRectangle, float, float> eRedraw;
		public DraggableDot[] dots = new DraggableDot[5];
		private Vector2 topleft, bottomright, size;

		AnnoData _data;   // 这个矩形所对应的注释数据
		public AnnoData data {
			get { return _data; }
			set { _data = value; this.class_label.Content = value.clas.name; }
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
		}
		public void RegisterEvent () {
			// 拖动 dots 的时候进行大小的调整

		}

		/// <summary>
		/// 绘制矩形区域
		/// </summary>
		public void Resize (Canvas canvas, float width, float height) {
			container.Width = MathF.Abs(width);
			container.Height = MathF.Abs(height);
			eRedraw?.Invoke(this, width, height);
		}

		/// <summary>
		/// 在画布上绘制/重新绘制
		/// 矩形区域的大小与位置由左上角的顶点 tl 与右下角的顶点 br 确定
		/// </summary>
		public void Redraw (Canvas canvas, Vector2 tl, Vector2 br) {
			// 定义矩形 (左上角) 的位置 
			Canvas.SetLeft(this, tl.X); this.topleft = tl;
			Canvas.SetTop(this, tl.Y); this.bottomright = br;

			// 定义矩形的长和宽
			// X: width, Y: height
			size = new Vector2(MathF.Abs(bottomright.X - topleft.X), MathF.Abs(bottomright.Y - topleft.Y));
			container.Width = size.X;
			container.Height = size.Y;

			eRedraw?.Invoke(this, size.X, size.Y);
		}

		/// <summary>
		/// 在画布上绘制/重新绘制
		/// 矩形区域的大小与位置由左上角的顶点 tl 与右下角的顶点 br 确定
		/// </summary>
		public void Redraw (Canvas canvas, Vector2 tl, float width, float height) {
			// 定义矩形 (左上角) 的位置 
			Canvas.SetTop(this, tl.X); this.topleft = tl;
			Canvas.SetLeft(this, tl.Y); this.bottomright = tl + new Vector2(width, height);
			size = new Vector2(MathF.Abs(bottomright.X - topleft.X), MathF.Abs(bottomright.Y - topleft.Y));

			container.Width = Math.Abs(width);
			container.Height = Math.Abs(height);

			eRedraw?.Invoke(this, width, height);
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
				dot.dot.Stroke = Brushes.brush_dot_stroke_solid;
				dot.dot.Fill = Brushes.brush_dot_fill_solid;
			}
			class_label.Foreground = Brushes.brush_label_text_foreground_solid;
		}

		private void container_MouseLeave (object sender, MouseEventArgs e) {
			// 改变四个点的透明度
			foreach ( var dot in dots ) {
				dot.dot.Stroke = Brushes.brush_dot_stroke_transparent;
				dot.dot.Fill = Brushes.brush_dot_fill_transparent;
			}
			class_label.Foreground = Brushes.brush_label_text_foreground_transparent;
		}
		/// <summary>
		/// 从画布上移除这个元素
		/// </summary>
		public void Delete (MainCanvas canvas) {
			canvas.annotation_canvas.Children.Remove(this);
		}
		public void Draw (Canvas canvas, Point[] points) {
			throw new NotImplementedException();
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
	}
}
