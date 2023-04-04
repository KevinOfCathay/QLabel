using Newtonsoft.Json;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace QLabel.Scripts.AnnotationData {
	public record ADDot : AnnoData {
		[JsonProperty] private IEnumerable<Guid> line_id_a, line_id_b;
		/// <summary>
		/// Deserialize
		/// </summary>
		public ADDot
			(Vector2 point, ClassLabel clas, IEnumerable<Guid> line_id_a, IEnumerable<Guid> line_id_b,
			Guid id, DateTime createtime, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[1] { point }), Type.Dot, clas, conf, id, createtime) {
				this.line_id_a = line_id_a;
				this.line_id_b = line_id_b;
		}
		public ADDot
			(Vector2 point, ClassLabel clas, IEnumerable<Guid> line_id_a, IEnumerable<Guid> line_id_b, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[1] { point }), Type.Dot, clas, conf) {
			this.line_id_a = line_id_a;
			this.line_id_b = line_id_b;
		}
		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			DraggableDot dot = new DraggableDot(canvas.CanvasPosition(rpoints[0]), line_id_a, line_id_b) { data = this };
			return dot;
		}
		public override void Visualize (Bitmap bitmap, Vector2 scale, Vector2 offset) {
			throw new NotImplementedException();
		}
	}
}
