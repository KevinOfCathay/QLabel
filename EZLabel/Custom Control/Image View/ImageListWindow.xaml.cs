using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using System.Diagnostics;
using QLabel.Scripts.Projects;
using System.Drawing;

namespace QLabel.Custom_Control.Image_View {
	/// <summary>
	/// Interaction logic for ImageListPanel.xaml
	/// </summary>
	public partial class ImageListWindow : UserControl {
		public Action<ImageListWindow, ImageListItem> eImageListUICreated;
		public List<string> paths;

		public ImageListWindow () {
			InitializeComponent();
		}

		/// <summary>
		/// 从路径列表中设置 UI
		/// </summary>
		public async void SetListUI (List<string> paths) {
			foreach ( var file in paths ) {
				ImageListItem new_item = new ImageListItem();

				string filename = System.IO.Path.GetFileName(file);
				new_item.thumbnail_image.Source = await LoadImageThumbnailFromFile(file);
				new_item.image_name.Text = filename;
				new_item.eSwitchImage += (b) => { Trace.WriteLine("Image button clicked"); };

				this.image_view_listbox.Items.Add(new_item);

				TextBlock txtblock = new TextBlock { Text = filename };
				this.image_listbox.Items.Add(txtblock);

				eImageListUICreated?.Invoke(this, new_item); // 触发 UI 创建事件
			}
		}

		public async Task<BitmapImage> LoadImageThumbnailFromFile (string path) {
			return await Task.Run(
				() => {
					BitmapImage image = new BitmapImage();
					image.BeginInit();
					image.DecodePixelWidth = 100;
					image.UriSource = new Uri(path);
					image.EndInit();
					image.Freeze();
					return image;
				}
				);
		}
	}
}
