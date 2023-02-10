using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 单一的点
	/// </summary>
	public record ADSingleDot : AnnoData {
		public ADSingleDot
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f) :
			base(points, Type.Dot, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
