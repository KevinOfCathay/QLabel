using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 四个点组成的四边形
	/// 不检测是否有交叉
	/// </summary>
	public record ADTetragon : AnnoData {
		public ADTetragon (ReadOnlySpan<Vector2> points, int clas = 0, string label = "") : base(points, clas, label) {
			type = Type.Tetragon;
		}
	}
}
