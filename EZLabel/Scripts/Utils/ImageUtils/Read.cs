using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLabel {
	internal static partial class ImageUtils {
		public static async Task<Bitmap> ReadBitmapAsync (string path) {
			return await Task<Bitmap>.Run(delegate () { return new Bitmap(Image.FromFile(path)); });
		}

		public static async Task<BitmapImage> ReadBitmapImageAsync (string path, int decode_width = -1, int decode_height = -1) {
			return await Task.Run(
				delegate () {
					BitmapImage image = new BitmapImage();
					image.BeginInit();
					image.UriSource = new Uri(path);
					if ( decode_width > 0 ) { image.DecodePixelWidth = decode_width; }
					if ( decode_height > 0 ) { image.DecodePixelHeight = decode_height; }
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.EndInit();
					image.Freeze();
					return image;
				}
			);
		}
	}
}
