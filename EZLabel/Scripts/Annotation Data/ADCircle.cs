using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADCircle : AnnoData {
		public ADCircle () : base() {
			type = Type.Circle;
		}
	}
}
