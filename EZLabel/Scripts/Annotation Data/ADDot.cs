using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace QLabel.Scripts.AnnotationData {
	public record ADDot : AnnoData {
		public ADDot
			(Vector2 point, ClassLabel clas, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[1] { point }), Type.Dot, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			DraggableDot dot = new DraggableDot(canvas.CanvasPosition(rpoints[0]), Array.Empty<Line>()) { data = this };
			return dot;
		}
	}
}
