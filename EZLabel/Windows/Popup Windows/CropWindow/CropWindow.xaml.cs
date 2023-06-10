using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Windows.CropWindow {
	/// <summary>
	/// Interaction logic for CropWindow.xaml
	/// </summary>
	public partial class CropWindow : Window {
		private enum Target { Current, All };
		private List<ClassTemplate> accepted_labels = new List<ClassTemplate>();
		private string save_dir = "";
		public CropWindow () {
			InitializeComponent();
			crop_target.SetCombobox(new string[] { "Current File", "All Files" });
			crop_align.SetCombobox(new string[] { "Top Left", "Center" });
			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; this.Topmost = true; };

			var classlabels = App.project_manager.class_label_manager.label_set_used;
			SetClassListbox(classlabels);
		}
		private async void ConfirmClick (object sender, RoutedEventArgs e) {
			// 裁剪并保存图像
			switch ( (Target) crop_target.SelectedIndex ) {
				// 只裁剪当前的图像
				case Target.Current:
					await CropSaveAsync(App.project_manager.cur_datafile, save_dir);
					break;
				// 裁剪所有的图像
				case Target.All:
					foreach ( var datafile in App.project_manager.datas ) {
						await CropSaveAsync(datafile, save_dir);
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
		private void SetClassListbox (IEnumerable<ClassTemplate> labels) {
			// 首先清除 accepted_labels 中的全部内容
			accepted_labels.Clear();

			labeltree.SetUI(labels,
				check: (ClassTemplate cl) => { accepted_labels.Add(cl); },
				uncheck: (ClassTemplate cl) => { accepted_labels.Remove(cl); }
				);
		}
		private async Task CropSaveAsync (ImageData data, string top_dir) {
			string path = data.path;
			if ( Path.Exists(path) ) {
				// 读取图片
				var load_bitmap_task = ImageUtils.ReadBitmapAsync(path);
				var ads = data.annodatas;
				int index = 0;
				foreach ( var ad in ads ) {
					if ( !accepted_labels.Contains(ad.class_label.template) ) { continue; }
					var cropped_bitmap = CropBitmap(await load_bitmap_task, ad);
					try {
						cropped_bitmap.Save(Path.Join(top_dir,
							string.Join("_", data.filename, ad.class_label.group, ad.class_label.name, index.ToString()) + ".png"));
						index += 1;
					} catch { }
				}
			}
		}
		private Bitmap CropBitmap (Bitmap source_image, AnnoData data) {
			// 读取前端 preferred_width, preferred_height,  target_margin 参数
			int target_width = GetValidUInt(this.preferred_width.Text);
			int target_height = GetValidUInt(this.preferred_height.Text);
			int target_margin = GetValidUInt(this.add_margin.Text);
			if ( target_margin < 0 ) { target_margin = 0; }

			// 裁剪
			OpenCvSharp.Rect2f crop_box = new OpenCvSharp.Rect2f(
									data.bbox.X - target_margin, data.bbox.Y - data.bbox.X,
									data.bbox.Width + target_margin * 2, data.bbox.Height + target_margin * 2);
			return ImageUtils.CropPadBitmap(source_image, crop_box, target_width, target_height, (ImageUtils.CornerAlign) crop_align.SelectedIndex);
		}
		private int GetValidUInt (string text) {
			if ( text == null || text.Length == 0 ) { return -1; }
			int number;
			bool valid = int.TryParse(text, out number);
			if ( !valid ) {
				return -1;
			} else {
				if ( number < 0 ) {
					return -1;
				} else {
					return number;
				}
			}
		}
	}
}
