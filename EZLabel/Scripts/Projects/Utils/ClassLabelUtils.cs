using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal static partial class Utils {
		public static HashSet<ClassTemplate> GetLabelSet (List<ClassLabelStat> label_stats) {
			// 创建一个 label_stats 的 copy
			var stats_copy = label_stats.ToList();
			stats_copy.Sort((a, b) => { return a.index.CompareTo(b.index); });
			HashSet<ClassTemplate> labels = new HashSet<ClassTemplate>(label_stats.Count);

			foreach ( var stat in label_stats ) {
				labels.Add(stat.template);
			}
			return labels;
		}
	}
}
