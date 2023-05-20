using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using OpenCvSharp;

namespace QLabel {
	internal static partial class ImageUtils {
		public static Bitmap ResizeBitmap (Bitmap source, in OpenCvSharp.Size size, InterpolationMode interp = InterpolationMode.Bilinear) {
			var result = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			using ( Graphics g = Graphics.FromImage(result) ) {
				g.InterpolationMode = interp;
				g.DrawImage(source, 0, 0, size.Width, size.Height);
			}
			return result;
		}
	}
}
