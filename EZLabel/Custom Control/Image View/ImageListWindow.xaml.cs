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

namespace EZLabel.Custom_Control.Image_View {
	/// <summary>
	/// Interaction logic for ImageListPanel.xaml
	/// </summary>
	public partial class ImageListWindow : UserControl {
		private HashSet<string> accepted_ext = new HashSet<string> { ".bmp", ".jpg", ".png", ".jpeg" };
		public Action<ImageListWindow, ImageListItem> eImageListUICreated;
		public List<ImageFileData> ifd_list;

		public ImageListWindow () {
			InitializeComponent();
		}

		public List<ImageFileData> LoadImagesFromDir (string dir_path) {
			var files = Directory.GetFiles(dir_path);
			List<ImageFileData> res = new List<ImageFileData>(capacity: files.Length);
			foreach ( var file in files ) {
				string ext = System.IO.Path.GetExtension(file);
				if ( accepted_ext.Contains(ext) ) {
					BitmapImage image = new BitmapImage(new Uri(file));
					ImageFileData ifd = new ImageFileData {
						filename = System.IO.Path.GetFileName(file), path = file, source = image,
						width = image.PixelWidth, height = image.PixelHeight, size = ( new FileInfo(file) ).Length
					};
					res.Add(ifd);
				}
			}
			this.ifd_list = res;
			return res;
		}

		public void SetListUI (List<ImageFileData> data_list) {
			foreach ( var data in data_list ) {
				ImageListItem ili = new ImageListItem { data = data };
				ili.thumbnail_image.Source = data.source;
				ili.image_name.Text = data.filename;
				ili.eImageButtonClick += (b) => { Trace.WriteLine("Image button clicked"); };
				eImageListUICreated?.Invoke(this, ili);

				this.image_view_listbox.Items.Add(ili);

				TextBlock txtblock = new TextBlock { Text = data.filename };
				this.image_listbox.Items.Add(txtblock);
			}
		}
	}
}
