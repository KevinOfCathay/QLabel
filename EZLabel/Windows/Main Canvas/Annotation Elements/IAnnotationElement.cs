using OpenCvSharp;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QLabel.Windows.Main_Canvas {
	/// <summary>
	/// 在 Canvas 上绘制的 Annotation 元素的 Base Class
	/// </summary>
	public interface IAnnotationElement {
		#region Brushes
		protected static Brush brush_dot_stroke_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF6A20D4"));
		protected static Brush brush_dot_stroke_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#556A20D4"));

		protected static Brush brush_dot_fill_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFB5DCFF"));
		protected static Brush brush_dot_fill_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#55B5DCFF"));

		protected static Brush brush_label_text_foreground_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF3B7E61"));
		protected static Brush brush_label_text_foreground_transparent = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#553B7E61"));

		protected static Brush brush_label_text_background_solid = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFEBFBFF"));
		protected static Brush brush_label_text_background_transparent = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
		#endregion

		/// <summary>
		/// 这个元素所关联的 anno 数据
		/// </summary>
		AnnoData data { get; set; }
		/// <summary>
		/// 获取和这个注释元素在画布上的点
		/// </summary>
		Vector2[] cpoints { get; }
		/// <summary>
		/// 获取和这个注释元素的 convex hull
		/// </summary>
		Vector2[] convex_hull { get; }

		public event Action<IAnnotationElement> eSelected;
		public void MouseDown (MainCanvas canvas, MouseEventArgs e);
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e);
		public void MouseUp (MainCanvas canvas, MouseEventArgs e);


		/// <summary>
		/// 根据所定义的点，
		/// 对元素进行绘制 / 重新绘制
		/// </summary>
		public void Draw (Canvas canvas, Vector2[] points);
		/// <summary>
		/// 平移元素，shift 定义了元素平移量（x, y)
		/// </summary>
		public void Shift (Canvas canvas, Vector2 shift);
		/// <summary>
		/// 在画布上选择了元素
		/// </summary>
		public void Select ();
		/// <summary>
		/// 从画布上删除元素
		/// </summary>
		public void Delete (MainCanvas canvas);
		/// <summary>
		/// 将对象转化为多边形（不是每个物体都可以转化）
		/// </summary>
		public void ToPolygon (MainCanvas canvas);
		/// <summary>
		/// 在画布上突出元素的位置
		/// </summary>
		public void Highlight ();
		/// <summary>
		/// 在画布上显示元素
		/// </summary>
		public void Show ();
		/// <summary>
		/// 从画布上隐藏元素
		/// </summary>
		public void Hide ();
	}
}
