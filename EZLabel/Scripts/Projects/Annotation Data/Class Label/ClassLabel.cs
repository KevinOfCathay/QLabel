using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace QLabel.Scripts.Projects {
	public class ClassLabel {
		/// <summary>从 template 中创建一个 class template </summary>
		public ClassLabel (ClassTemplate template) {
			this.template = template;
			confidence = 1.0f;
			var guid_bytes = template.guid.ToByteArray();
			color = Color.FromArgb(255, guid_bytes[0], guid_bytes[1], guid_bytes[2]);
		}
		/// <summary>从 template 中创建一个 class template，并加上 Color 和 Confidence </summary>
		public ClassLabel (ClassTemplate template, float confidence) {
			this.template = template;
			var guid_bytes = template.guid.ToByteArray();
			color = Color.FromArgb(255, guid_bytes[0], guid_bytes[1], guid_bytes[2]);
			this.confidence = confidence;
		}
		/// <summary>Copy Constructor </summary>
		public ClassLabel (ClassLabel label) {
			this.template = label.template;
			this.color = label.color;
			this.confidence = label.confidence;
		}

		public string name { get { return template.name; } }
		public string group { get { return template.group; } }
		public string supercategory { get { return template.supercategory; } }

		/// <summary> 这个标签底层所用的 template </summary>
		public ClassTemplate template;
		/// <summary> 代表这个标签的颜色 </summary>
		public Color color;
		/// <summary> 这个标签的 confidence（如果没有在） </summary>
		public float confidence;
	}
}
