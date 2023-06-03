using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLabel {
	internal static partial class ImageUtils {
		public static Bitmap ToBitmap (this BitmapImage bitmapImage) {
			// https://stackoverflow.com/a/6484754
			using MemoryStream outStream = new();
			BitmapEncoder enc = new BmpBitmapEncoder();
			enc.Frames.Add(BitmapFrame.Create(bitmapImage));
			enc.Save(outStream);
			Bitmap bitmap = new Bitmap(outStream);
			return new Bitmap(bitmap);
		}

		/// <summary>
		/// Async 将 BitmapImage 转化为 byte 数组
		/// </summary>
		public static async Task<byte[]> BitmapToByteArrayAsync (BitmapImage image) {
			return await Task.Run(
				() => {
					int w = image.PixelWidth;
					int h = image.PixelHeight;

					int stride = image.PixelWidth * 4;
					int size = image.PixelHeight * stride;
					byte[] pixels = new byte[size];
					image.CopyPixels(pixels, stride, 0);

					// Getting Pixel Information from a BitmapImage
					// https://social.msdn.microsoft.com/Forums/vstudio/en-US/82a5731e-e201-4aaf-8d4b-062b138338fe/getting-pixel-information-from-a-bitmapimage?forum=wpf
					// int index = y * stride + 4 * x;

					return pixels;
				});
		}
	}
}
