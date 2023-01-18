using QLabel.Windows.Main_Canvas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QLabel.Scripts.Projects;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.ConstrainedExecution;
using QLabel.Windows.CropWindow;

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
					string sfile = openFileDialog.FileName;
					main.ilw.Clear();   // 清空上一个 Project 加载的内容
					try {
						var directory = System.IO.Path.GetDirectoryName(sfile);
						if ( directory != null ) {
							// 创建一个新的 project
							ProjectManager.NewProject(directory);

							// 读取文件夹下的所有文件
							// 如果符合图像格式，则将路径加入到 list 中
							var files = Directory.GetFiles(directory);
							var paths = new List<string>(capacity: files.Length); // 重置 paths
							foreach ( var f in files ) {
								string ext = System.IO.Path.GetExtension(f);
								if ( accepted_ext.Contains(ext) ) {
									paths.Add(f);
								}
							}
							main.ilw.SetListUI(paths);

							// 如果当前被选择的文件属于图像
							// 则加载该图像
							if ( accepted_ext.Contains(System.IO.Path.GetExtension(sfile)) ) {
								main.main_canvas.LoadImage(sfile);
							}
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
