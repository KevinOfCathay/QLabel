using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal class ClassLabelManager {
		/// <summary> Project 下所有数据的类别标签的个数统计 </summary>
		private List<ClassLabelStat> label_stats = new List<ClassLabelStat>();

		public ICollection<ClassTemplate> label_set {
			get {
				return Utils.GetLabelSet(label_stats);
			}
		}

		public void AddClassLabels (ClassTemplate[] templates) {
			foreach ( var label in templates ) {
				AddClassLabel(label);
			}
		}
		/// <summary>
		/// 增加一个类别标签
		/// 这个类别标签可以是使用过/出现过的（即，作为 annodata 的 classlabel 加入到 imagedata 中）
		/// 也可以是未使用的（例如，自动识别时，一些类从来都没有被使用过，或者用户自己加入到类别集合中）
		/// </summary>
		public void AddClassLabel (ClassTemplate template) {
			var labelstat = label_stats.Find((x) => { return x.template == template; });
			if ( labelstat == null ) {
				label_stats.Add(
					new ClassLabelStat {
						template = template,
						index = label_stats.Count
					}); ;
			}
		}
		/// <summary>
		/// 去除一个类别标签
		/// </summary>
		public void RemoveLabel (ClassLabel label) {
			var labelstat = label_stats.Find((x) => { return x.template == label; });
			if ( labelstat != null ) {
				label_stats.Remove(labelstat);
			}
		}
	}
}
