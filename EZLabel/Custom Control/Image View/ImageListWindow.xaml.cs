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
		public Action<ImageListItem, ImageListItem> eSwitchImage;
		public ImageListItem old_item;
		public List<string> paths;

		public ImageListWindow () {
			InitializeComponent();
		}

		public void Clear () {
			this.image_view_listbox.Items.Clear();
			this.image_listbox.Items.Clear();
		}

		/// <summary>
		/// 从路径列表中设置 UI
		/// </summary>
		public async Task SetListUI (IEnumerable<ImageData> datalist) {
			foreach ( var data in datalist ) {
				ImageListItem new_item = new ImageListItem();
				new_item.MouseDown += (object o, MouseButtonEventArgs a) => {
					old_item?.UnHighlight();
					new_item.Highlight();
					eSwitchImage?.Invoke(old_item, new_item);
					old_item = new_item;
				};
				Task<BitmapImage> readimagethumbnail = ImageUtils.ReadImageFromFileAsync(data.path, decode_width: 100);
				new_item.data = data;
				new_item.image_name.Text = data.filename;

				this.image_view_listbox.Items.Add(new_item);

				TextBlock txtblock = new TextBlock { Text = data.filename };
				this.image_listbox.Items.Add(txtblock);

				new_item.thumbnail_image.Source = await readimagethumbnail;
			}
		}
	}
}
