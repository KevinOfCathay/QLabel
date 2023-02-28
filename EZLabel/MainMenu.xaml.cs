using Microsoft.Win32;
using QLabel.Scripts.Projects;
using QLabel.Windows.CropWindow;
using QLabel.Windows.Export_Window;
using QLabel.Windows.Import_Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
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
		public void LoadImageFromData (ImageData[] data_list) {

		}
		private async void New_Click (object sender, RoutedEventArgs e) {
			if ( main != null ) {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
				if ( openFileDialog.ShowDialog() == true ) {
					string selected_file = openFileDialog.FileName;
					main.ilw.Clear();   // 清空上一个 Project 加载的内容
					try {
						var directory = Path.GetDirectoryName(selected_file);
						if ( directory != null ) {
							// 创建一个新的 project
							ProjectManager.NewProject(directory);

							// 读取文件夹下的所有文件
							// 如果符合图像格式，则将路径加入到 list 中
							var files = Directory.GetFiles(directory);

							int index = 0;
							foreach ( var fi in files ) {
								string ext = System.IO.Path.GetExtension(fi).ToLower();
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
											ProjectManager.project.AddImageData(imgdata);
											if ( fi == selected_file ) {
												// 如果当前被选择的文件属于图像
												// 则加载该图像
												// TODO: 移动到 for 循环外面, Async 加载
												if ( accepted_ext.Contains(Path.GetExtension(selected_file)) ) {
													await main.main_canvas.LoadImage(imgdata);
													ProjectManager.cur_datafile = imgdata;
												}
											}
											index += 1;
										}
									}
								}
							}
							await main.ilw.SetListUI(ProjectManager.project.data_list);
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
		/// 导出注释数据到其他格式
		/// </summary>
		private void ExportClick (object sender, RoutedEventArgs e) {
			if ( !ProjectManager.empty ) {
				ExportWindow window = new ExportWindow();
				main.LockWindow();
				window.Closed += (object? _, EventArgs _) => { main.UnlockWindow(); };
				window.Show();
			} else {
				MessageBox.Show("There is no project opened.", "Error", MessageBoxButton.OK);
			}
		}
		/// <summary>
		/// 导出注释数据到其他格式
		/// </summary>
		private void ImportClick (object sender, RoutedEventArgs e) {
			ImportWindow window = new ImportWindow();
			main.LockWindow();
			window.Closed += (object? _, EventArgs _) => { main.UnlockWindow(); };
			window.Show();
		}
		/// <summary>
		/// 打开裁剪窗口，裁剪注释并保存
		/// </summary>
		private void Crop_Click (object sender, RoutedEventArgs e) {
			if ( !ProjectManager.empty ) {
				CropWindow window = new CropWindow();
				main.LockWindow();
				window.Closed += (object? sender, EventArgs e) => { main.UnlockWindow(); };
				window.Show();
			} else {
				MessageBox.Show("There is no project opened.", "Error", MessageBoxButton.OK);
			}
		}
		/// <summary>
		/// 打开裁剪窗口，裁剪注释并保存
		/// </summary>
		private void ToPolygon_Click (object sender, RoutedEventArgs e) {
			if ( main.selected_element != null ) {
				main.selected_element.ToPolygon(main.main_canvas);
			}
		}
		/// <summary>
		/// 打开裁剪窗口，裁剪注释并保存
		/// </summary>
		private void Densify_Click (object sender, RoutedEventArgs e) {
			if ( main.selected_element != null ) {
				main.selected_element.Densify(main.main_canvas);
			}
		}
	}
}
