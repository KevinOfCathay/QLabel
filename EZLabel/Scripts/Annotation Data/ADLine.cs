using Newtonsoft.Json;
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
	public record ADLine : AnnoData {
		[JsonProperty] private Guid[] dots_ids;
		public ADLine
			(Vector2 rx, Vector2 ry, ADDot[] linked_dots, ClassLabel clas, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[2] { rx, ry }), Type.Line, clas, conf) {
			if ( linked_dots != null ) {
				int dots = linked_dots.Length;
				Guid[] dots_ids = new Guid[dots];
				for ( int i = 0; i < dots; i += 1 ) {
					dots_ids[i] = linked_dots[i].guid;
				}
				this.dots_ids = dots_ids;
			}
		}
		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			Vector2 cx = canvas.CanvasPosition(this.rpoints[0]);
			Vector2 cy = canvas.CanvasPosition(this.rpoints[1]);
			List<DraggableDot> dots = new List<DraggableDot>(dots_ids.Length);
			foreach ( var id in dots_ids ) {
				var elem = canvas.GetElementByID(id);
				if ( elem != null && elem is DraggableDot ) {
					dots.Add(elem as DraggableDot);
				}
			}
			Line line = new Line(cx, cy, dots) { data = this };
			return line;
		}
	}
}
