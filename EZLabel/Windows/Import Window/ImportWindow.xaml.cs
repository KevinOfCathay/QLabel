using OpenCvSharp.Dnn;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLabel.Windows.Import_Window {
	public partial class ImportWindow : Window {
		/// <summary>
		/// VOC: VOC 数据格式
		private enum Format { VOC }

		private readonly MainWindow main;
		private Format fmt = Format.VOC;
		private string load_dir;
		public ImportWindow () {
			InitializeComponent();
		}
		public ImportWindow (MainWindow main) {
			InitializeComponent();
			this.main = main;
		}
		private void RegisterEvents (object? sender, EventArgs e) {
			this.dir_selector.eDirectorySelected += (path) => { load_dir = path; };
			this.dir_selector.eDialogClosed += () => { this.Topmost = true; this.Activate(); };

			confirm_cancel.eConfirmClick += ConfirmClick;
			confirm_cancel.eCancelClick += CancelClick;
		}
		private void ConfirmClick (object sender, RoutedEventArgs e) {
			Import(main);
			CloseWindow();
		}
		private void CancelClick (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}
		private async void Import (MainWindow main) {
			switch ( fmt ) {
				case Format.VOC:
					var paths = GetVOCFiles(load_dir);
					string? local_path = missing.SelectedIndex == 0 ? load_dir : null;
					var image_data = await ImportFromVOCAsync(paths, local_path);
					await main.SetImageData(image_data);
					break;
				default:
					break;
			}
		}
		private void FormatSelectionChanged (object sender, SelectionChangedEventArgs e) {
			if ( format.SelectedItem != null ) {
				fmt = (Format) format.SelectedIndex;
			}
		}
	}
}