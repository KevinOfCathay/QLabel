using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	public record ADCircle : AnnoData {
		public float radius = 0f;
		public ADCircle
			(ReadOnlySpan<Vector2> points, ClassLabel clas, string label = "", float conf = 1.0f, float radius = 0f) :
			base(points, Type.Circle, clas, label, conf) {
			this.radius = radius;
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
