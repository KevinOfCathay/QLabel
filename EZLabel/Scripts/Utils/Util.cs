using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLabel {
	internal static class Util {
		/// <summary>
		/// Async 从文件路径中读取图片
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static async Task<BitmapImage> ReadImageFromFileAsync (
			string path, int decode_width = -1, int decode_height = -1) {
			return await Task.Run(
				() => {
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
