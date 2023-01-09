using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Windows.Main_Canvas {
	/// <summary>
	/// 在 Canvas 上绘制的 Annotation 元素的 Base Class
	/// </summary>
	public interface IAnnotationElement {
		/// <summary>
		/// 这个元素所关联的 anno 数据
		/// </summary>
		AnnoData data { get; set; }

		/// <summary>
		/// 根据所定义的点，
		/// 对元素进行绘制 / 重新绘制
		/// </summary>
		public void Draw(Canvas canvas, Point[] points);

		/// <summary>
		/// 从画布上删除元素
		/// </summary>
		public void Delete (MainCanvas canvas);
	}
}
