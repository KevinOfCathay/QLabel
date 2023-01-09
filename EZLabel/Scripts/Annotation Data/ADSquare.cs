using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADSquare : AnnoData {
		public ADSquare (ReadOnlySpan<Vector2> points, int clas = 0, string label = "") : base(points, clas, label) {
			type = Type.Square;
		}
	}
}
