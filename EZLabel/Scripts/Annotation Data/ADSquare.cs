using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 正方形
	/// </summary>
	public record ADSquare : AnnoData {
		public ADSquare
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f) :
			base(points, Type.Square, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
