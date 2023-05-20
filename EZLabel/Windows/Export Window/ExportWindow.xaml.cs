using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
		private HashSet<ClassTemplate> exported_classes = new HashSet<ClassTemplate>();
		public ExportWindow () {
			InitializeComponent();
			labeltree.SetUI(
				App.project_manager.class_label_manager.label_set,
				(label) => { exported_classes.Add(label); },
				(label) => { exported_classes.Remove(label); });
		}
		private void RegisterEvents (object? sender, EventArgs e) {
			// Register Events
			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; };
			this.dir_selector.eDirectoryClosed += () => { this.Topmost = true; this.Activate(); };
		}
		private void ConfirmClick (object sender, RoutedEventArgs e) {
			List<ImageData> datalist = new List<ImageData>();
			switch ( tar ) {
				case Target.Current:
					datalist.Add(App.project_manager.cur_datafile);
					break;
				case Target.All:
					foreach ( var datafile in App.project_manager.datas ) {
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
