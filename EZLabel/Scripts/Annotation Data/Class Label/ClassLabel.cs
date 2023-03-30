using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QLabel.Scripts {
	public class ClassLabel {
		public ClassLabel (ClassLabel source) {
			this.group = ( source.group != null && source.group != "" ) ? source.group : "None";
			this.name = ( source.name != null && source.name != "" ) ? source.name : "None";
			this.supercategory = ( source.supercategory != null && source.name != "" ) ? source.supercategory : source.name;
		}
		public ClassLabel (string group, string name, string supercategory = null) {
			this.group = ( group != null && group != "" ) ? group : "None";
			this.name = ( name != null && name != "" ) ? name : "None";
			this.supercategory = ( supercategory != null && supercategory != "" ) ? supercategory : name;
		}

		/// <summary> 组 </summary>
		public readonly string group;
		/// <summary> 上级标签 </summary>
		public readonly string supercategory;
		/// <summary> 标签名 </summary>
		public readonly string name;
		/// <summary> 代表这个标签的颜色 </summary>
		public Color color;

		public string GetName () {
			return ( ( group != null && group != "" ) ? group : "" ) + ( ( name != null && name != "" ) ? name : "" );
		}
		public override int GetHashCode () {
			return name.GetHashCode() + group.GetHashCode();
		}
		public override bool Equals ([NotNullWhen(true)] object? obj) {
			ClassLabel? target = obj as ClassLabel;
			if ( target == null ) { return false; }
			return name.Equals(target.name) && group.Equals(target.group) && supercategory.Equals(target.supercategory);
		}
	}
}
