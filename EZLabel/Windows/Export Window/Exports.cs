using QLabel.Scripts;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace QLabel.Windows.Export_Window {
	public partial class ExportWindow : Window {
		private void ExportToCoco (ImageData[] data_array, string path) {
			string save_loc = Path.Join(path, "coco.json");
			var class_labels = ProjectManager.project.class_labels;     // categories
			if ( !Path.Exists(save_loc) ) {    // 不要覆盖现有的文件
				var coco_obj = new {
					info = new Dictionary<string, string> {
						{ "version", "0.0.0" },
						{ "description", "this is a dataset created with QLabel" },
						{ "contributor", "__noname__" },
						{ "url", "__nourl__" },
						{ "date_created", DateTime.Today.ToLongDateString() },
					},
					// TODO: 补全以下信息
					licenses = new List<Dictionary<string, string>> { },
					categories = ClassLabelsToCategory(class_labels),
					image = new List<Dictionary<string, string>> { },
					annotations = new List<Dictionary<string, string>> { },
				};
				try {
					string coco_string = JsonSerializer.Serialize(coco_obj);
					File.WriteAllText(save_loc, coco_string);
				} catch ( Exception ) { }
			}
		}
		private List<Dictionary<string, string>> ClassLabelsToCategory (IEnumerable<ClassLabel> class_labels) {
			List<Dictionary<string, string>> categories = new List<Dictionary<string, string>>();

			int id = 0;
			foreach ( var label in class_labels ) {
				categories.Add(
					new Dictionary<string, string> {
						{"id", id.ToString() },
						{"name", label.name },
						{"supercategory", label.supercategory },
						{"isthing", "0" },
					}
				);
			}
			return categories;
		}
		private void ExportToVOC (ImageData[] data_array, string path) {
			foreach ( var d in data_array ) {
				string save_loc = Path.Join(path, data_array[0].filename + "_voc.xml");
				if ( !Path.Exists(save_loc) ) {    // 不要覆盖现有的文件
					d.ToVOC(save_loc);
				}
			}
		}
	}
}
