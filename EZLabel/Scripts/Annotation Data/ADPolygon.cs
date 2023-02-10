using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 多边形，不检测是否有交叉
	/// </summary>
	public record ADPolygon : AnnoData {
		public ADPolygon
			(ReadOnlySpan<Vector2> rpoints, ClassLabel clas, float conf = 1.0f) :
			base(rpoints, Type.Polygon, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
