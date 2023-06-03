using QLabel.Scripts;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Windows.Export_Window {
	public partial class ExportWindow : Window {
		private async Task ExportToCoco (ImageData[] data_array, string path) {
			string save_loc = Path.Join(path, "coco.json");
			var class_labels = App.project_manager.class_label_manager.label_set_used;

			var categories_task = ClassTemplatesToCategoryAsync(class_labels);
			var image_task = ImageDatasToImageAsync(App.project_manager.datas);

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
					categories = await categories_task,
					image = await image_task,
					annotations = new List<Dictionary<string, string>> { },
				};
				try {
					string coco_string = JsonSerializer.Serialize(coco_obj);
					File.WriteAllText(save_loc, coco_string);
				} catch ( Exception ) { }
			}
		}
		/// <summary>  将 class template 转换为 coco categories 的格式 </summary>
		private async Task<List<Dictionary<string, string>>> ClassTemplatesToCategoryAsync (IEnumerable<ClassTemplate> class_labels) {
			return await Task.Run(() => ClassTemplatesToCategory(class_labels));
		}
		private List<Dictionary<string, string>> ClassTemplatesToCategory (IEnumerable<ClassTemplate> class_labels) {
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
				id += 1;
			}
			return categories;
		}
		private async Task<List<Dictionary<string, string>>> ImageDatasToImageAsync (IEnumerable<ImageData> image_datas) {
			return await Task.Run(() => ImageDatasToImage(image_datas));
		}
		private List<Dictionary<string, string>> ImageDatasToImage (IEnumerable<ImageData> image_datas) {
			List<Dictionary<string, string>> images = new List<Dictionary<string, string>>();

			int id = 0;
			foreach ( var data in image_datas ) {
				images.Add(
					new Dictionary<string, string> {
						{"id", id.ToString() },
						{"file_name", data.filename },
						{"license", "0" },
						{"width", data.width.ToString() },
						{"height", data.height.ToString() },
						{"coco_url", data.path },
					}
				);
				id += 1;
			}
			return images;
		}
		private async Task ExportToVOC (ImageData[] data_array, string path) {
			await Task.Run(() => {
				foreach ( var d in data_array ) {
					string save_loc = Path.Join(path, d.filename + "_voc.xml");
					if ( !Path.Exists(save_loc) ) {    // 不要覆盖现有的文件
						d.ToVOC(save_loc);
					}
				}
			});
		}
		private async Task ExportToYolo (ImageData[] datas, string path, YOLOFormat format, bool percentage) {
			await Task.Run(() => {
				foreach ( var data in datas ) {
					string save_loc = Path.Join(path, data.filename + ".txt");
					if ( !Path.Exists(save_loc) ) {    // 不要覆盖现有的文件
						ClassTemplate[] label_templates = new ClassTemplate[App.project_manager.class_label_manager.label_set_used.Count];
						App.project_manager.class_label_manager.label_set_used.CopyTo(label_templates, 0);
						switch ( format ) {
							case YOLOFormat.XYWH:
								data.ToYoloXYWH(save_loc, label_templates, percentage);
								break;
							case YOLOFormat.XYs:
								data.ToYoloXYCoords(save_loc, label_templates, percentage);
								break;
						}
					}
				}
			});
		}
	}
}
