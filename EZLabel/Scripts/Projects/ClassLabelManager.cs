using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal class ClassLabelManager {
		/// <summary> Project 下所有数据的类别标签的个数统计 </summary>
		private List<ClassLabelStat> label_stats = new List<ClassLabelStat>();

		public ClassLabel[] label_set {
			get {
				var stats_copy = label_stats.ToList();
				stats_copy.Sort((a, b) => { return a.index.CompareTo(b.index); });
				ClassLabel[] labels = new ClassLabel[label_stats.Count];
				int index = 0;
				foreach ( var stat in label_stats ) {
					labels[index] = stat.label;
					index += 1;
				}
				return labels;
			}
		}
		public event Action<ClassLabel> eAddLabel, eRemoveLabel;

		public void AddClassLabels (ClassLabel[] labels) {
			foreach ( var label in labels ) {
				AddClassLabel(label);
			}
		}
		/// <summary>
		/// 增加一个类别标签
		/// </summary>
		public void AddClassLabel (ClassLabel label) {
			var labelstat = label_stats.Find((x) => { return x.label == label; });
			if ( labelstat != null ) {
				labelstat.count += 1;
			} else {
				label_stats.Add(
					new ClassLabelStat {
						label = label,
						count = 1,
						index = label_stats.Count
					}); ;
			}
			eAddLabel?.Invoke(label);
		}
		/// <summary>
		/// 去除一个类别标签
		/// </summary>
		public void RemoveLabel (ClassLabel label) {
			var labelstat = label_stats.Find((x) => { return x.label == label; });
			if ( labelstat != null ) {
				labelstat.count -= 1;
				if ( labelstat.count == 0 ) {
					label_stats.Remove(labelstat);
				}
			}
			eRemoveLabel?.Invoke(label);
		}
	}
}
