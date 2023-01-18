using Microsoft.Win32;
using QLabel.Scripts.Projects;
using QLabel.Windows.CropWindow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace QLabel {
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : UserControl {
		MainWindow main;
		private HashSet<string> accepted_ext = new HashSet<string> { ".bmp", ".jpg", ".png", ".jpeg" };

		public MainMenu () {
			InitializeComponent();
		}
		public void Init (MainWindow mw) {
			this.main = mw;
		}
		private void New_Click (object sender, RoutedEventArgs e) {
			if ( main != null ) {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
				if ( openFileDialog.ShowDialog() == true ) {
					string selected_file = openFileDialog.FileName;
					main.ilw.Clear();   // 清空上一个 Project 加载的内容
					try {
						var directory = System.IO.Path.GetDirectoryName(selected_file);
						if ( directory != null ) {
							// 创建一个新的 project
							ProjectManager.NewProject(directory);

							// 读取文件夹下的所有文件
							// 如果符合图像格式，则将路径加入到 list 中
							var files = Directory.GetFiles(directory);
							var paths = new List<string>(capacity: files.Length); // 重置 paths

							int index = 0;
							List<ImageData> data = new List<ImageData>();          // 存放 ImageData 的 List
							foreach ( var fi in files ) {
								string ext = System.IO.Path.GetExtension(fi);
								// 如果当前文件属于图片文件，则加载
								if ( accepted_ext.Contains(ext) ) {
									if ( fi == selected_file ) {
										// 如果当前被选择的文件属于图像
										// 则加载该图像
										if ( accepted_ext.Contains(System.IO.Path.GetExtension(selected_file)) ) {
											main.main_canvas.LoadImage(selected_file);
											ProjectManager.cur_file_index = index;
										}
									}

									// https://stackoverflow.com/a/9687096
									// Getting image dimensions without reading the entire file
									using ( FileStream file = new FileStream(fi, FileMode.Open, FileAccess.Read) ) {
										using ( var tif = System.Drawing.Image.FromStream(stream: file,
													 useEmbeddedColorManagement: false,
													 validateImageData: false) ) {
											float width = tif.PhysicalDimension.Width;
											float height = tif.PhysicalDimension.Height;

											data.Add(new ImageData {
												path = fi,
												filename = Path.GetFileName(fi),
												size = new System.IO.FileInfo(fi).Length,
												width = width,
												height = height
											});
											paths.Add(fi);
											index += 1;
										}
									}

								}
							}
							ProjectManager.project.data_list = data;          // 更新当前 project 的 Datalist
							main.ilw.SetListUI(data);
						}
					} catch ( Exception ) {
					}
				}
			}
		}
		private void Undo_Click (object sender, RoutedEventArgs e) {
			ActionManager.PopAction();
		}

		/// <summary>
		/// 一键裁剪当前所有的注释
		/// </summary>
		private void Crop_Click (object sender, RoutedEventArgs e) {
			CropWindow window = new CropWindow();
			main.LockWindow();
			window.Closed += (object? sender, EventArgs e) => { main.UnlockWindow(); };
			window.Show();
		}
	}
}
