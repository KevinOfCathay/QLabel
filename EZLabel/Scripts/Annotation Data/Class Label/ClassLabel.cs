using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts {
	public struct ClassLabel {
		public ClassLabel (string group, string name) {
			this.group = ( group != null && group != "" ) ? group : "None";
			this.name = ( name != null && name != "" ) ? name : "None";
		}

		public readonly string group;
		public readonly string name;
		/// <summary> 代表这个标签的颜色 </summary>
		public Color color;
		public override int GetHashCode () {
			return name.GetHashCode() + group.GetHashCode();
		}
		public override bool Equals ([NotNullWhen(true)] object? obj) {
			var target = (ClassLabel) obj;
			return name.Equals(target.name) && group.Equals(target.group);
		}
	}
}
