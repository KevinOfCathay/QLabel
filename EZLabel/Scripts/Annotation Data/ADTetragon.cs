using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 四个点组成的四边形
	/// 不检测是否有交叉
	/// </summary>
	public record ADTetragon : AnnoData {
		public ADTetragon () : base() {
			type = Type.Tetragon;
		}
	}
}
