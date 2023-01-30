using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace QLabel.Windows.Export_Window {
	/// <summary>
	/// Interaction logic for ExportWindow.xaml
	/// </summary>
	public partial class ExportWindow : Window {
		/// <summary>
		/// VOC: VOC 数据格式
		/// COCO: COCO 数据格式
		/// Image: 导出为包含了注释的图片
		/// </summary>
		private enum Format { VOC, COCO, Image }
		private enum Target { Current, All };

		private Format fmt = Format.VOC;
		private Target tar = Target.Current;
		private string save_dir = "";
		public ExportWindow () {
			InitializeComponent();

			// Register Events
			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; };
			this.dir_selector.eDialogClosed += () => { this.Topmost = true; this.Activate(); };

			confirm_cancel.eConfirmClick += ConfirmClick;
			confirm_cancel.eCancelClick += CancelClick;

		}
		private void ConfirmClick (object sender, RoutedEventArgs e) {
			switch ( tar ) {
				case Target.Current:
					Export(ProjectManager.cur_datafile, fmt, save_dir);
					break;
				case Target.All:
					foreach ( var datafile in ProjectManager.project.data_list ) {
						Export(datafile, fmt, save_dir);
					}
					break;
				default:
					break;
			}
			CloseWindow();
		}
		private void CancelClick (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}

		/// <summary>
		/// 导出一个单一文件
		/// </summary>
		private void Export (ImageData data, Format fmt, string path) {
			string save_loc;
			switch ( fmt ) {
				case Format.VOC:
					save_loc = Path.Join(path, data.filename + "_voc.xml");
					data.ToVOC(save_loc);
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
		private void TargetSelectionChanged (object sender, SelectionChangedEventArgs e) {
			if ( target.SelectedItem != null ) {
				tar = (Target) target.SelectedIndex;
			}
		}
	}
}
