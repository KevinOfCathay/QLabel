﻿using Newtonsoft.Json;
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
		[JsonProperty] public readonly Guid guid;
		/// <summary> 组 </summary>
		[JsonProperty] public readonly string group;
		/// <summary> 上级标签 </summary>
		[JsonProperty] public readonly string supercategory;
		/// <summary> 标签名 </summary>
		[JsonProperty] public readonly string name;

		[JsonConstructor]
		private ClassTemplate () { }

		/// <summary> Copy Constructor </summary>
		public ClassTemplate (ClassTemplate source) {
			this.guid = source.guid;
			this.group = source.group;
			this.name = source.name;
			this.supercategory = source.supercategory;
		}
		/// <summary>
		/// 创建一个新的 ClassTemplate
		/// 若没有指定 supercategory, 则 supercategory = name
		/// </summary>
		public ClassTemplate (string group, string name, string supercategory = null) {
			this.guid = Guid.NewGuid();
			this.group = ( group != null && group != "" ) ? group : "None";
			this.name = ( name != null && name != "" ) ? name : "None";
			this.supercategory = ( supercategory != null && supercategory != "" ) ? supercategory : name;
		}
		public string GetName () {
			return ( ( group != null && group != "" ) ? group : "" ) + ( ( name != null && name != "" ) ? name : "" );
		}
		public override int GetHashCode () {
			return guid.GetHashCode();
		}
		public override bool Equals (object? obj) {
			ClassTemplate? target = obj as ClassTemplate;
			if ( target == null ) { return false; }
			return guid.Equals(target.guid);
		}
	}
}
