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
			List<ImageData> datalist = new List<ImageData>();
			switch ( tar ) {
				case Target.Current:
					datalist.Add(ProjectManager.cur_datafile);
					break;
				case Target.All:
					foreach ( var datafile in ProjectManager.project.data_list ) {
						datalist.Add(datafile);
					}
					break;
				default:
					break;
			}
			Export(datalist.ToArray(), fmt, save_dir);
			CloseWindow();
		}
		private void CancelClick (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}
		private async void Export (ImageData[] data, Format fmt, string path) {
			switch ( fmt ) {
				case Format.VOC:
					await ExportToVOC(data, path);
					break;
				case Format.COCO:
					await ExportToCoco(data, path);
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
