using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	public static class ProjectManager {
		public static Project project { get; private set; }

		/// <summary>
		/// 当前打开的文件夹的路径
		/// </summary>
		private static string cur_dir = null;
		private static string save_dir = null;
		private const string project_name = "project";

		public static bool NewProject (string directory) {
			if ( directory != cur_dir ) {
				if ( cur_dir != null ) {
					try {
						SaveProject();      // 保存当前打开的项目
					} catch {
						return false;        // 如果没有保存成功则直接返回
					}
				}
				cur_dir = directory;
				save_dir = Path.Join(cur_dir, project_name);
				if ( !Path.Exists(save_dir) ) {
					Directory.CreateDirectory(save_dir);
				}
			}
			return true;
		}
		public static void LoadProject () {

		}
		public static void SaveProject () {
			foreach ( var data in project.data_list ) {
				data.ToXML(save_dir);
			}
		}

	}
}
