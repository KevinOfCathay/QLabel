using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QLabel.Scripts.Projects {
	public class Project {
		/// <summary> Project 包含的数据源 </summary>
		public List<ImageFileData> data_list = new List<ImageFileData>();
		/// <summary> Project 下包含的类别标签 </summary>
		private HashSet<ClassLabel> class_labels = new HashSet<ClassLabel>();

		private int cur_label_index = 0;
		public event Action<ClassLabel> eAddLabel, eRemoveLabel;

		public ClassLabel GetCurrentLabel () {
			return class_labels.ElementAt(cur_label_index);
		}
		public ClassLabel GetLabel (int index) {
			return class_labels.ElementAt(index);
		}
		public void AddLabels (ClassLabel[] labels) {
			foreach ( var label in labels ) {
				class_labels.Add(label);
				eAddLabel?.Invoke(label);
			}
		}
		public void AddLabel (ClassLabel label) {
			class_labels.Add(label);
			eAddLabel?.Invoke(label);
		}
		public void RemoveLabel (ClassLabel label) {
			class_labels.Remove(label);
			eRemoveLabel?.Invoke(label);
		}
	}
}
