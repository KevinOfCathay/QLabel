using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADRect : AnnoData {
		public ADRect () : base() {
			type = Type.Rectangle;
		}
	}
}
