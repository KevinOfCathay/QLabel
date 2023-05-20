using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QLabel.Scripts.Projects {
	public class ClassLabel : ClassTemplate {
		/// <summary>从 template 中创建一个 class template </summary>
		public ClassLabel (ClassTemplate template) : base(template) { }
		/// <summary>从 template 中创建一个 class template，并加上 Color 和 Confidence </summary>
		public ClassLabel (ClassTemplate template, Color color, float confidence) : base(template) {
			this.color = color;
			this.confidence = confidence;
		}
		/// <summary>Copy Constructor </summary>
		public ClassLabel (ClassLabel label) : base(label.group, label.name, label.supercategory) {
			this.color = label.color;
			this.confidence = label.confidence;
		}

		/// <summary> 代表这个标签的颜色 </summary>
		public Color color;
		/// <summary> 这个标签的 confidence（如果没有在） </summary>
		public float confidence;
	}
}
