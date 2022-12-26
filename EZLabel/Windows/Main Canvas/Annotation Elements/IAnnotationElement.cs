using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 在 Canvas 上绘制的 Annotation 元素的 Base Class
	/// </summary>
	public interface IAnnotationElement {
		AnnoData data { get; set; }

		/// <summary>
		/// 从画布上删除元素
		/// </summary>
		public void Delete ();
	}
}
