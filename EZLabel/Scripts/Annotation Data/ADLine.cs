using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 单一的点
	/// </summary>
	public record ADLine : AnnoData {
		public ADLine () : base() {
			type = Type.Line;
		}
	}
}
