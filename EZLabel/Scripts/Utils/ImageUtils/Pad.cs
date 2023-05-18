using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	internal static partial class ImageUtils {
		public static Bitmap Pad (Bitmap source, OpenCvSharp.Size size) {
			Bitmap target = new Bitmap(size.Width, size.Height);
			using ( Graphics g = Graphics.FromImage(target) ) {
				g.DrawImage(source, new Rectangle(0, 0, size.Width, size.Height),
				   new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
			}
			return target;
		}
	}
}
