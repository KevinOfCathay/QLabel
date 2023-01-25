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
			class_labels = new HashSet<ClassLabel>();
		}
		/// <summary> Project 包含的数据源 </summary>
		private List<ImageData> _data_list;
		/// <summary> 封装（只能获取不能设置） </summary>
		public IEnumerable<ImageData> data_list { get { return _data_list; } }
		/// <summary> Project 下所有数据的类别标签的个数统计 </summary>
		private Dictionary<ClassLabel, int> label_counts;
		/// <summary> Project 下的类别 </summary>
		private HashSet<ClassLabel> class_labels;

		public event Action<ClassLabel> eAddLabel, eRemoveLabel;

		public void AddImageData (ImageData data) {
			_data_list.Add(data);
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
		/// <summary>
		/// 增加一个类别标签
		/// </summary>
		public void AddLabel (ClassLabel label) {
			class_labels.Add(label);
			eAddLabel?.Invoke(label);
		}
		/// <summary>
		/// 去除一个类别标签
		/// </summary>
		public void RemoveLabel (ClassLabel label) {
			class_labels.Remove(label);
			eRemoveLabel?.Invoke(label);
		}
	}
}
