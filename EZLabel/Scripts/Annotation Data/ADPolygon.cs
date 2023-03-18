using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 多边形，不检测是否有交叉
	/// </summary>
	public record ADPolygon : AnnoData {
		public int vertex;
		public ADPolygon
			(ReadOnlySpan<Vector2> rpoints, ClassLabel clas, float conf = 1.0f) :
			base(rpoints, Type.Polygon, clas, conf) {
			vertex = rpoints.Length;
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			// 创建一个矩形
			var cpoints = canvas.CanvasPosition(rpoints);
			DraggablePolygon polygon = new DraggablePolygon(canvas, cpoints) { data = this };
			// 将新创建的矩形加入到画布中
			canvas.annotation_canvas.Children.Add(polygon);
			return polygon;
		}
	}
}
