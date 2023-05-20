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
		}

		/// <summary> Project 包含的数据源 </summary>
		private List<ImageData> _datas;
		/// <summary> 所有的 data（只能获取不能变更） </summary>
		public ICollection<ImageData> datas { get { return _datas; } }

		public void AddImageData (ImageData data) {
			if ( !_datas.Contains(data) ) {          // 不重复添加 image data
				_datas.Add(data);
			}
		}
	}
}
