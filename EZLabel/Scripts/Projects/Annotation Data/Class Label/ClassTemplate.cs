using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	/// <summary>
	/// 类别标签的 template, 
	/// 在 AnnoData 上加入类别标签时，将其转化为 ClassLabel
	/// </summary>
	public class ClassTemplate {
		/// <summary> 组 </summary>
		public readonly string group;
		/// <summary> 上级标签 </summary>
		public readonly string supercategory;
		/// <summary> 标签名 </summary>
		public readonly string name;

		public ClassTemplate (ClassTemplate source) {
			this.group = ( source.group != null && source.group != "" ) ? source.group : "None";
			this.name = ( source.name != null && source.name != "" ) ? source.name : "None";
			this.supercategory = ( source.supercategory != null && source.name != "" ) ? source.supercategory : source.name;
		}
		public ClassTemplate (string group, string name, string supercategory = null) {
			this.group = ( group != null && group != "" ) ? group : "None";
			this.name = ( name != null && name != "" ) ? name : "None";
			this.supercategory = ( supercategory != null && supercategory != "" ) ? supercategory : name;
		}

		public string GetName () {
			return ( ( group != null && group != "" ) ? group : "" ) + ( ( name != null && name != "" ) ? name : "" );
		}
		public override int GetHashCode () {
			return name.GetHashCode() + group.GetHashCode();
		}
		public override bool Equals (object? obj) {
			ClassTemplate? target = obj as ClassTemplate;
			if ( target == null ) { return false; }
			return name.Equals(target.name) && group.Equals(target.group) && supercategory.Equals(target.supercategory);
		}
	}
}
