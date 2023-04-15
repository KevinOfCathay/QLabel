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
		[JsonProperty] private List<Guid> line_ids;
		public override int visualize_priority => 2;

		/// <summary>
		/// Deserialize
		/// </summary>
		public ADDot
			(Vector2 point, ClassLabel clas, List<Guid> line_ids, Guid id, DateTime createtime, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[1] { point }), Type.Dot, clas, conf, id, createtime) {
			this.line_ids = line_ids;
		}
		public ADDot
			(Vector2 point, ClassLabel clas, IEnumerable<Guid> line_ids, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[1] { point }), Type.Dot, clas, conf) {
			this.line_ids = line_ids.ToList();
		}
		public void AddLink (Guid guid) { line_ids.Add(guid); }
		public void AddLinks (IEnumerable<Guid> guids) { line_ids.AddRange(guids); }
		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			DraggableDot dot = new DraggableDot(canvas.CanvasPosition(rpoints[0]), line_ids) { data = this };
			return dot;
		}
		public override void Visualize (Bitmap bitmap, Vector2 target_size, Color color) {
			// 获取源图像大小
			Vector2 size = new Vector2(bitmap.Width, bitmap.Height);
			Vector2 scale = size / target_size;
			var p = this.rpoints[0] / scale;
			var rectf = new RectangleF(p.X - 3f, p.Y - 3f, 6f, 6f);

			using ( var g = Graphics.FromImage(bitmap) ) {
				Brush brush = new SolidBrush(color);
				g.FillEllipse(brush, rectf);
			}
		}
	}
}
