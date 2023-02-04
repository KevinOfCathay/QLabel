using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenCvSharp.Aruco;

namespace QLabel.Scripts.Projects {
	public class Project {
		public Project () {
			_data_list = new List<ImageData>();
			label_counts = new Dictionary<ClassLabel, int>();
			label_counts = new Dictionary<ClassLabel, int>();
		}
		public ClassLabel[] class_labels { get { return label_counts.Keys.ToArray(); } }
		/// <summary> 所有的 data（只能获取不能进行变更） </summary>
		public IEnumerable<ImageData> data_list { get { return _data_list; } }

		/// <summary> Project 包含的数据源 </summary>
		private List<ImageData> _data_list;
		/// <summary> Project 下所有数据的类别标签的个数统计 </summary>
		private Dictionary<ClassLabel, int> label_counts;

		public event Action<ClassLabel> eAddLabel, eRemoveLabel;

		public void AddImageData (ImageData data) {
			_data_list.Add(data);
		}
		public void AddLabels (ClassLabel[] labels) {
			foreach ( var label in labels ) {
				if ( label_counts.ContainsKey(label) ) {
					label_counts[label] += 1;
				} else {
					label_counts[label] = 1;
				}
				eAddLabel?.Invoke(label);
			}
		}
		/// <summary>
		/// 增加一个类别标签
		/// </summary>
		public void AddLabel (ClassLabel label) {
			if ( label_counts.ContainsKey(label) ) {
				label_counts[label] += 1;
			} else {
				label_counts[label] = 1;
			}
			eAddLabel?.Invoke(label);
		}
		/// <summary>
		/// 去除一个类别标签
		/// </summary>
		public void RemoveLabel (ClassLabel label) {
			if ( label_counts.ContainsKey(label) ) {
				label_counts[label] -= 1;
				if ( label_counts[label] == 0 ) {
					label_counts.Remove(label);
				}
			}
			eRemoveLabel?.Invoke(label);
		}
	}
}
