using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenCvSharp.Aruco;

namespace QLabel.Scripts.Projects {
	internal class Project {
		public Project () {
			_datas = new List<ImageData>();
			class_label_manager = new ClassLabelManager();
		}
		public ClassLabelManager class_label_manager;
		/// <summary> 所有的 data（只能获取不能进行变更） </summary>
		public IEnumerable<ImageData> datas { get { return _datas; } }

		/// <summary> Project 包含的数据源 </summary>
		private List<ImageData> _datas;
		public ClassLabel[] label_set { get { return class_label_manager.label_set; } }

		public void AddImageData (ImageData data) {
			if ( !_datas.Contains(data) ) {          // 不重复添加 image data
				_datas.Add(data);
				foreach ( var anno in data.annodata ) {
					var class_label = anno.clas;
					class_label_manager.AddClassLabel(class_label);
				}
			}
		}
	}
}
