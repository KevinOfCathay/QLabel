using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QLabel.Windows.CropWindow {
	/// <summary>
	/// Interaction logic for CropWindow.xaml
	/// </summary>
	public partial class CropWindow : Window {
		private enum Target { Current, All };
		private Target target = Target.Current;
		private List<ClassTemplate> accepted_labels = new List<ClassTemplate>();
		private string save_dir = "";
		public CropWindow () {
			InitializeComponent();
			this.crop_target.InitializeCombobox(new string[] { "Current File", "All Files" });
			this.dir_selector.eDirectorySelected += (path) => { save_dir = path; this.Topmost = true; };

			var classlabels = App.project_manager.class_label_manager.label_set_used;
			SetClassListbox(classlabels);
		}
		private async void ConfirmClick (object sender, RoutedEventArgs e) {
			// 裁剪并保存图像
			switch ( target ) {
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
		private void crop_target_SelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if ( crop_target.SelectedItem != null ) {
				target = (Target) crop_target.SelectedIndex;
			}
		}
		private Bitmap CropBitmap (Bitmap source_image, AnnoData data) {
			// 读取前端 preferred_width, preferred_height 参数
			bool has_width = IsValidateUint(this.preferred_width.Text);
			bool has_height = IsValidateUint(this.preferred_height.Text);

			int bbox_width = data.brect.Width, bbox_height = data.brect.Height;
			int bbox_x = data.brect.X, bbox_y = data.brect.Y;
			int bbox_center_x = data.brect.X + bbox_width / 2, bbox_center_y = data.brect.Y + bbox_height / 2;
			int target_width, target_height;
			if ( has_width && has_height ) {
				// 同时拥有 width, height
				// 设置宽和高为目标宽、高
				target_width = int.Parse(this.preferred_width.Text);
				target_height = int.Parse(this.preferred_height.Text);

				// 计算比例
				// 如果 20×10 的 source 图像目标为 10×8 target
				// 则裁剪 20×16 的区域并缩放为 10×8
				float w_ratio = (float) bbox_width / (float) target_width;
				float h_ratio = (float) bbox_height / (float) target_height;

				float cropped_width, cropped_height, scale_ratio;
				if ( w_ratio >= h_ratio ) {
					cropped_width = bbox_width;
					cropped_height = target_height * w_ratio;
					scale_ratio = w_ratio;
				} else {
					cropped_width = target_width * h_ratio;
					cropped_height = bbox_height;
					scale_ratio = h_ratio;
				}
				// 裁剪后的 bitmap
				// 如果裁剪区域超出了边界，那么超出边界的地方将会透明
				Bitmap cropped_bitmap = new Bitmap(target_width, target_height);
				var g = Graphics.FromImage(cropped_bitmap);
				g.DrawImage(source_image,
					new RectangleF(0f, 0f, (float) target_width, (float) target_height),
					new RectangleF(
						(float) bbox_center_x - cropped_width / 2f, (float) bbox_center_y - cropped_height / 2f,
						cropped_width, cropped_height), GraphicsUnit.Pixel);
				return cropped_bitmap;
			} else if ( has_width ) {
				target_width = int.Parse(this.preferred_width.Text);
				// 计算比例
				// 如果 20×10 的 bbox 图像目标为 10×. target
				// 则裁剪 20×10 的区域并缩放为 10×5
				float w_ratio = (float) bbox_width / (float) target_width;
				float cropped_width = target_width;
				float cropped_height = bbox_height / w_ratio;

				Bitmap cropped_bitmap = new Bitmap((int) cropped_width, (int) cropped_height);
				var g = Graphics.FromImage(cropped_bitmap);
				g.DrawImage(source_image,
					new RectangleF(0f, 0f, cropped_width, cropped_height),
					new RectangleF(
						bbox_x, bbox_y,
						cropped_width, cropped_height), GraphicsUnit.Pixel);
				return cropped_bitmap;
			} else if ( has_height ) {
				target_height = int.Parse(this.preferred_height.Text);
				float h_ratio = (float) bbox_width / (float) target_height;
				float cropped_width = bbox_width / h_ratio;
				float cropped_height = target_height;

				Bitmap cropped_bitmap = new Bitmap((int) cropped_width, (int) cropped_height);
				var g = Graphics.FromImage(cropped_bitmap);
				g.DrawImage(source_image,
					new RectangleF(0f, 0f, cropped_width, cropped_height),
					new RectangleF(
						bbox_x, bbox_y,
						cropped_width, cropped_height), GraphicsUnit.Pixel);
				return cropped_bitmap;
			} else {
				// 没有设置任何 preferred width, height
				Bitmap cropped_bitmap = new Bitmap(bbox_width, bbox_height);
				using ( var g = Graphics.FromImage(cropped_bitmap) ) {
					g.DrawImage(source_image,
						new Rectangle(0, 0, bbox_width, bbox_height),
						new Rectangle(
							bbox_x, bbox_y,
							bbox_width, bbox_height),
						 GraphicsUnit.Pixel);
				}
				return cropped_bitmap;
			}
		}
		private bool IsValidateUint (string s) {
			if ( s == null || s == string.Empty ) { return false; }
			int number;
			bool valid = int.TryParse(s, out number);
			if ( !valid ) {
				return false;
			} else {
				if ( number < 0 ) {
					return false;
				} else {
					return true;
				}
			}
		}
	}
}
