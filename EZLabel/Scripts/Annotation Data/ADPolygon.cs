using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 多边形，不检测是否有交叉
	/// </summary>
	public record ADPolygon : AnnoData {
		public ADPolygon () : base() {
			type = Type.Polygon;
		}
	}
}
