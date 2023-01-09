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
	public record ADCircle : AnnoData {
		public int radius = 0;
		public ADCircle (ReadOnlySpan<Vector2> points, int radius = 0, int clas = 0, string label = "") : base(points, clas, label) {
			type = Type.Circle;
			this.radius = radius;
		}
	}
}
