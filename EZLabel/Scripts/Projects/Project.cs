using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenCvSharp.Aruco;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QLabel.Scripts.Projects {
	internal class Project {
		public Project () { }

		/// <summary> Project 包含的数据源 </summary>
		private List<ImageData> _datas = new List<ImageData>();
		/// <summary> 所有的 data（只能获取不能变更） </summary>
		public ICollection<ImageData> datas { get { return _datas; } }

		/// <summary> 创建一个新的 project </summary>
		public void New () {
			_datas = new List<ImageData>();
		}
		public void AddImageData (ImageData data) {
			if ( !_datas.Contains(data) ) {          // 不重复添加 image data
				_datas.Add(data);
			}
		}
		public async Task SaveProjectAsync (string save_path) {
			await Task.Run(() => {
				JsonSerializer serializer = new JsonSerializer();
				using ( JsonWriter writer = new JsonTextWriter(
					new StreamWriter(save_path)) ) {
					serializer.Serialize(writer, new {
						datas = this._datas,
						save_date = DateTime.Now.ToShortDateString()
					});
				}
			});
		}
	}
}
