using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts {
	public struct ClassLabel {
		public ClassLabel () { }

		public string parent = "";
		public string name = "";
		/// <summary>
		/// 这个标签的代表颜色
		/// </summary>
		public Color color;
	}
}
