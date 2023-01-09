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
	public record ADLine : AnnoData {
		public ADLine (ReadOnlySpan<Vector2> points, int clas = 0, string label = "") : base(points, clas, label) {
			type = Type.Line;
		}
	}
}
