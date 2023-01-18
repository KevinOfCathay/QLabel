using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 线
	/// </summary>
	public record ADLine : AnnoData {
		public ADLine
			(ReadOnlySpan<Vector2> points, ClassLabel clas, string label = "", float conf = 1.0f) :
			base(points, Type.Line, clas, label, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
