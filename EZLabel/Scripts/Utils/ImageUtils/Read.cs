using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		public static async Task<BitmapImage[]> ReadBitmapImagesAsync (string[] paths, int decode_width = -1, int decode_height = -1) {
			return await Task.Run(
				delegate () {
					BitmapImage[] images = new BitmapImage[paths.Length];
					for ( int i = 0; i < paths.Length; i += 1 ) {
						BitmapImage image = new BitmapImage();
						image.BeginInit();
						image.UriSource = new Uri(paths[i]);
						if ( decode_width > 0 ) { image.DecodePixelWidth = decode_width; }
						if ( decode_height > 0 ) { image.DecodePixelHeight = decode_height; }
						image.CacheOption = BitmapCacheOption.OnLoad;
						image.EndInit();
						image.Freeze();

						images[i] = image;
					}
					return images;
				}
			);
		}

		public static async Task<Bitmap> ReadBitmapAsync (string path, int decode_width = -1, int decode_height = -1) {
			return await Task.Run(
				delegate () {
					Bitmap bitmap = new Bitmap(path);
					if ( decode_width > 0 && decode_height > 0 ) {
						var new_bitmap = new Bitmap(decode_width, decode_height);
						using ( var graphics = Graphics.FromImage(new_bitmap) ) {
							graphics.InterpolationMode = InterpolationMode.Bilinear;
							graphics.DrawImage(
								bitmap,
								new Rectangle(0, 0, decode_width, decode_height),
								0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
						}
						bitmap = new_bitmap;
					}
					return bitmap;
				}
			);
		}

	}
}
