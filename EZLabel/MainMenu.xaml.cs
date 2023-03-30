using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using QLabel.Actions;
using QLabel.Scripts.Projects;
using QLabel.Windows.CropWindow;
using QLabel.Windows.Export_Window;
using QLabel.Windows.Import_Window;
using QLabel.Windows.Popup_Windows.Auto_Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

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
		private async void NewClick (object sender, RoutedEventArgs e) {
			// 打开文件浏览界面，设置 filter
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";

			if ( openFileDialog.ShowDialog() == true ) {
				string selected_file = openFileDialog.FileName;
				await NewProject(selected_file);
			}
		}
		private async void LoadClick (object sender, RoutedEventArgs e) {
			using ( var dialog = new CommonOpenFileDialog() ) {
				// 设置 dialog 的一些基本参数
				dialog.IsFolderPicker = true;
				dialog.Multiselect = false;
				dialog.EnsureFileExists = true;
				dialog.EnsurePathExists = true;

				if ( dialog.ShowDialog() == CommonFileDialogResult.Ok ) {
					await LoadProject(dialog.FileName);
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
		private async void SaveClick (object sender, RoutedEventArgs e) {
			await ProjectManager.SaveProjectAsync();
		}
		/// <summary>
		/// 导出注释数据到其他格式
		/// </summary>
		private void Auto_Click (object sender, RoutedEventArgs e) {
			AutoWindow window = new AutoWindow(main, main.main_canvas);
			window.Initialized += (object? _, EventArgs _) => { main.LockWindow(); };
			window.Closed += (object? _, EventArgs _) => { main.UnlockWindow(); };
			window.Show();
		}
		/// <summary>
		/// 导出注释数据到其他格式
		/// </summary>
		private void ImportClick (object sender, RoutedEventArgs e) {
			ImportWindow window = new ImportWindow(main);
			window.Initialized += (object? _, EventArgs _) => { main.LockWindow(); };
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
		private void ToPolygon_Click (object sender, RoutedEventArgs e) {
			main.ToPolygon();
		}
		/// <summary>
		/// 打开裁剪窗口，裁剪注释并保存
		/// </summary>
		private void Densify_Click (object sender, RoutedEventArgs e) {
			main.Densify();
		}
	}
}
