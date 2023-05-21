using Microsoft.Win32;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLabel {
	public partial class MainMenu : UserControl {
		/// <summary>
		/// Open existing project or create new project
		/// </summary>
		private async Task NewProjectAsync (string selected_file) {
			if ( main != null ) {
				main.ilw.Clear();   // 清空上一个 Project 加载的内容
				try {
					var directory = Path.GetDirectoryName(selected_file);
					if ( directory != null ) {
						// 创建一个新的 project
						App.project_manager.NewProject(directory);

						// 读取文件夹下的所有文件
						// 如果符合图像格式，则将路径加入到 list 中
						var files = Directory.GetFiles(directory);

						int index = 0;
						foreach ( var fi in files ) {
							string ext = Path.GetExtension(fi).ToLower();
							// 如果当前文件属于图片文件，则加载
							if ( accepted_ext.Contains(ext) ) {
								// https://stackoverflow.com/a/9687096
								// Getting image dimensions without reading the entire file
								using ( FileStream file = new FileStream(fi, FileMode.Open, FileAccess.Read) ) {
									using ( var tif = System.Drawing.Image.FromStream(stream: file,
												 useEmbeddedColorManagement: false,
												 validateImageData: false) ) {
										var imgdata = new ImageData {
											path = fi,
											size = new FileInfo(fi).Length,
											width = tif.PhysicalDimension.Width,
											height = tif.PhysicalDimension.Height,
											format = tif.PixelFormat
										};
										App.project_manager.AddImageData(imgdata);
										if ( fi == selected_file ) {
											// 如果当前被选择的文件属于图像
											// 则加载该图像
											// TODO: 移动到 for 循环外面, Async 加载
											if ( accepted_ext.Contains(Path.GetExtension(selected_file)) ) {
												await main.main_canvas.LoadImage(imgdata);
												App.project_manager.cur_datafile = imgdata;
											}
										}
										index += 1;
									}
								}
							}
						}
						await main.ilw.SetListUI(App.project_manager.datas);
					}
				} catch ( Exception ) {
				}
			}
		}
		private async Task LoadProject (string dir_path) {
			// 检查文件夹下面是否含有 saved project.json
			string project_loc = Path.Join(dir_path, ProjectManager.SAVE_JSON_NAME + ".json");
			if ( !Path.Exists(project_loc) ) { return; }

			await App.project_manager.LoadProjectAsync(project_loc);

		}
	}
}

