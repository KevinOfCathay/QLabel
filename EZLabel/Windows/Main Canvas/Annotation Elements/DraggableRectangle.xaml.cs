using EZLabel.Scripts.AnnotationData;
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
using System.Windows.Media.Media3D;

namespace EZLabel.Windows.Main_Canvas.Annotation_Elements {
	public partial class DraggableRectangle : UserControl, IAnnotationElement {
		/// <summary>
		/// 当长方形被重新绘制时触发
		/// </summary>
		public Action<DraggableRectangle, double, double> eRedraw;

		public AnnoData data;
		public DraggableDot[] dots = new DraggableDot[5];
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

		/// <summary>
		/// 绘制正方形区域
		/// </summary>
		public void Resize (Canvas canvas, double width, double height) {
			container.Width = Math.Abs(width);
			container.Height = Math.Abs(height);
			eRedraw?.Invoke(this, width, height);
		}

		/// <summary>
		/// 在画布上绘制/重新绘制
		/// 矩形区域的大小与位置由左上角的顶点 tl 与右下角的顶点 br 确定
		/// </summary>
		public void Redraw (Canvas canvas, Point tl, Point br) {
			// 定义矩形 (左上角) 的位置 
			Canvas.SetLeft(this, tl.X);
			Canvas.SetTop(this, tl.Y);

			// 定义矩形的长和宽
			var width = br.X - tl.X;
			var height = br.Y - tl.Y;
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

		private void DragBottomRightDot () {

		}

		private void container_MouseEnter (object sender, MouseEventArgs e) {
			// 改变四个点的透明度
			foreach ( var dot in dots ) {
				dot.dot.Stroke = Brushes.brush_dot_stroke_solid;
				dot.dot.Fill = Brushes.brush_dot_fill_solid;
			}
		}

		private void container_MouseLeave (object sender, MouseEventArgs e) {
			// 改变四个点的透明度
			foreach ( var dot in dots ) {
				dot.dot.Stroke = Brushes.brush_dot_stroke_transparent;
				dot.dot.Fill = Brushes.brush_dot_fill_transparent;
			}
		}

		public void Delete () {
			throw new NotImplementedException();
		}
	}
}
