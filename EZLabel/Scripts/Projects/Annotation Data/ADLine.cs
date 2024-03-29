﻿using Newtonsoft.Json;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 线段。
	/// 线段的两端必定有点（ADDot），由 Guid 来定义。
	/// </summary>
	public record ADLine : AnnoData {
		[JsonProperty] private Guid dot_a_id, dot_b_id;
		public override int visualize_priority => 0;

		/// <summary> Deserialize </summary>
		public ADLine
			(Vector2 rx, Vector2 ry, Guid dot_a_id, Guid dot_b_id,
			ClassLabel clas, Guid id, DateTime createtime) :
			base(new ReadOnlySpan<Vector2>(new Vector2[2] { rx, ry }), Type.Line, clas, id, createtime) {
			this.dot_a_id = dot_a_id;
			this.dot_b_id = dot_b_id;
		}
		public ADLine
			(Vector2 rx, Vector2 ry, ADDot dot_a, ADDot dot_b, ClassLabel clas) :
			base(new ReadOnlySpan<Vector2>(new Vector2[2] { rx, ry }), Type.Line, clas) {
			if ( dot_a != null ) { this.dot_a_id = dot_a.guid; }
			if ( dot_b != null ) { this.dot_b_id = dot_b.guid; }
		}
		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			var rp = canvas.CanvasPosition(rpoints);
			DraggableLine line = new DraggableLine(rp[0], rp[1], this.dot_a_id, this.dot_b_id) { data = this };
			return line;
		}
		public override void Visualize (Bitmap bitmap, Vector2 target_size, Color color) {
			// 获取源图像大小
			Vector2 size = new Vector2(bitmap.Width, bitmap.Height);
			Vector2 scale = size / target_size;
			var p0 = this.rpoints[0] / scale;
			var p1 = this.rpoints[1] / scale;

			using ( var g = Graphics.FromImage(bitmap) ) {
				Pen pen = new Pen(color, 3);
				g.DrawLine(pen, p0.X, p0.Y, p1.X, p1.Y);
			}
		}
	}
}
