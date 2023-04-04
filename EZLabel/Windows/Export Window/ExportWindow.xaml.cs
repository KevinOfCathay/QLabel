using QLabel.Scripts;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
		/// </summary>
		private enum Format { VOC, COCO, YOLO }
		private enum YOLOFormat { XYWH, XYs }
		private enum Target { Current, All };

		private Format fmt = Format.VOC;
		private Target tar = Target.Current;
		private string save_dir = "";
		private HashSet<ClassLabel> exported_classes = new HashSet<ClassLabel>();
		public ExportWindow () {
			InitializeComponent();
			labeltree.SetUI(
				ProjectManager.project.label_set,
				(label) => { exported_classes.Add(label); },
				(label) => { exported_classes.Remove(label); });
		}
		private void RegisterEvents (object? sender, EventArgs e) {
			// Register Events
			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; };
			this.dir_selector.eDialogClosed += () => { this.Topmost = true; this.Activate(); };
		}
		private void ConfirmClick (object sender, RoutedEventArgs e) {
			List<ImageData> datalist = new List<ImageData>();
			switch ( tar ) {
				case Target.Current:
					datalist.Add(ProjectManager.cur_datafile);
					break;
				case Target.All:
					foreach ( var datafile in ProjectManager.project.datas ) {
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
				case Format.YOLO:
					await ExportToYolo(data, path,
						(YOLOFormat) yolo_format_combobox.SelectedIndex,
						yolo_scale_combobox.SelectedIndex == 0 ? true : false
						);
					break;
				default:
					break;
			}
		}
		private void FormatSelectionChanged (object sender, SelectionChangedEventArgs e) {
			if ( format.SelectedItem != null ) {
				fmt = (Format) format.SelectedIndex;
				if ( fmt == Format.YOLO ) {
					if ( yolo_format_label != null ) {
						yolo_format_label.Visibility = Visibility.Visible;
					}
					if ( yolo_scale_label != null ) {
						yolo_scale_label.Visibility = Visibility.Visible;
					}
				} else {
					if ( yolo_format_label != null ) {
						yolo_format_label.Visibility = Visibility.Collapsed;
					}
					if ( yolo_scale_label != null ) {
						yolo_scale_label.Visibility = Visibility.Collapsed;
					}
				}
			}
		}
		private void TargetSelectionChanged (object sender, SelectionChangedEventArgs e) {
			if ( target.SelectedItem != null ) {
				tar = (Target) target.SelectedIndex;
			}
		}
	}
}
