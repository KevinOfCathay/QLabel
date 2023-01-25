using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.Projects;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QLabel.Windows.CropWindow {
	/// <summary>
	/// Interaction logic for CropWindow.xaml
	/// </summary>
	public partial class CropWindow : Window {
		private enum Target { Current, All };

		private Target target = Target.Current;
		private List<ClassLabel> accepted_labels = new List<ClassLabel>();
		private string save_dir = "";
		public CropWindow () {
			InitializeComponent();

			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; };
			this.dir_selector.eDialogClosed += () => { this.Topmost = true; this.Activate(); };

			var classlabels = GetClassLabels(new ImageData[] { ProjectManager.cur_datafile });
			SetClassListbox(classlabels);
		}
		private void ConfirmButton_Click (object sender, RoutedEventArgs e) {
			// 裁剪并保存图像
			switch ( target ) {
				// 只裁剪当前的图像
				case Target.Current:
					CropSave(ProjectManager.cur_datafile, save_dir);
					break;
				// 裁剪所有的图像
				case Target.All:
					foreach ( var datafile in ProjectManager.project.data_list ) {
						CropSave(datafile, save_dir);
					}
					break;
				default:
					break;
			}
			CloseWindow();
		}
		private void CancelButton_Click (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}
		private void SetClassListbox (HashSet<ClassLabel> labels) {
			// 首先清除当前 listbox 中的全部内容
			class_listbox.Items.Clear();
			accepted_labels.Clear();

			// 加入所有的注释
			foreach ( var label in labels ) {
				CheckboxWithLabel new_item = new CheckboxWithLabel(label.name, true);
				var cl = label;
				new_item.eChecked += (_, _) => { accepted_labels.Add(cl); };
				new_item.eUnchecked += (_, _) => { accepted_labels.Remove(cl); };
				accepted_labels.Add(cl);
				class_listbox.Items.Add(new_item);
			}
		}
		private async void CropSave (ImageData data, string top_dir) {
			string path = data.path;
			if ( Path.Exists(path) ) {
				// 读取图片
				BitmapImage bitmap = await Util.ReadImageFromFileAsync(path);

				var ads = data.GetAnnoData();
				int index = 0;
				foreach ( var ad in ads ) {
					if ( !accepted_labels.Contains(ad.clas) ) { continue; }
					var cropped = new CroppedBitmap(bitmap, ad.brect);
					cropped.Freeze();
					try {
						await Util.SaveCroppedImage(cropped,
							Path.Join(top_dir,
							string.Join("_", data.filename, ad.clas.group, ad.clas.name, index.ToString()) + ".png"));
						index += 1;
					} catch { }
				}
			}
		}
		private HashSet<ClassLabel> GetClassLabels (IEnumerable<ImageData> data) {
			HashSet<ClassLabel> labels = new HashSet<ClassLabel>();
			foreach ( var d in data ) {
				foreach ( var (lb, _) in d.GetLabelCounts() ) {
					labels.Add(lb);
				}
			}
			return labels;
		}
		private void crop_target_SelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if ( crop_target.SelectedItem != null ) {
				target = (Target) crop_target.SelectedIndex;
				HashSet<ClassLabel> classlabels;
				switch ( target ) {
					case Target.Current:
						classlabels = GetClassLabels(new ImageData[] { ProjectManager.cur_datafile });
						break;
					case Target.All:
						classlabels = GetClassLabels(ProjectManager.project.data_list);
						break;
					default:
						classlabels = GetClassLabels(new ImageData[] { ProjectManager.cur_datafile });
						break;
				}
				SetClassListbox(classlabels);
			}
		}
	}
}
