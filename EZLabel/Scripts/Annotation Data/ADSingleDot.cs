using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Scripts.AnnotationData {
	/// <summary>
	/// 单一的点
	/// </summary>
	public record ADSingleDot : AnnoData {
		public ADSingleDot () : base() {
			type = Type.Dot;
		}
	}
}
