using OpenCvSharp;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal static class ProjectManager {
		public static Project project { get; private set; }

		/// <summary>
		/// 当前是否有项目被加载
		/// </summary>
		public static bool empty { get { return project == null; } }
		/// <summary>
		/// 当前打开的文件夹的路径
		/// </summary>
		private static string cur_dir = null;
		private static string save_dir = null;
		public static ImageData cur_datafile;
		public static ClassLabel cur_label = new ClassLabel("None", "None");
		public static int cur_label_index = 0;
		private const string project_name = "_project";

		public static event Action<ImageData, AnnoData>? eAnnoDataAdded, eAnnoDataRemoved;
		public static bool NewProject (string directory) {
			if ( directory != cur_dir ) {
				if ( cur_dir != null ) {
					try {
						SaveProjectAsync();      // 保存当前打开的项目
					} catch {
						return false;        // 如果没有保存成功则直接返回
					}
				}
				cur_dir = directory;
				project = new Project(); // 创建一个新的项目
				save_dir = Path.Join(cur_dir, project_name);
				if ( !Path.Exists(save_dir) ) {
					Directory.CreateDirectory(save_dir);
				}
			}
			return true;
		}
		public static void AddAnnoData (ImageData imgdata, AnnoData annodata) {
			if ( imgdata != null && annodata != null && !imgdata.annodata.Contains(annodata) ) {
				imgdata.AddAnnoData(annodata);   // 加入到 annodata 中
				project.class_label_manager.AddClassLabel(annodata.clas);
				eAnnoDataAdded?.Invoke(imgdata, annodata);
			}
		}
		public static void RemoveAnnoData (ImageData imgdata, AnnoData annodata) {
			if ( imgdata != null && annodata != null && imgdata.annodata.Contains(annodata) ) {
				imgdata.RemoveAnnoData(annodata);   // 加入到 annodata 中
				eAnnoDataRemoved?.Invoke(imgdata, annodata);
			}
		}
		public static async Task SaveProjectAsync () {
			await Task.Run(() => {
				JsonSerializer serializer = new JsonSerializer();
				using ( JsonWriter writer = new JsonTextWriter(
					new StreamWriter(
						Path.Join(save_dir, "saved project.json"))) ) {
					serializer.Serialize(writer, new {
						datas = project.datas,
						labels = project.label_set,
						save_date = DateTime.Now.ToShortDateString()
					});
				}
			});
		}
	}
}
