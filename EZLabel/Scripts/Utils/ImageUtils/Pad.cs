using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	internal static partial class ImageUtils {
		internal enum Align { Left, Right }

		/// <param name="align"> 左对齐或者右对齐，左对齐时，原图像贴着左侧边界，右侧补0；右对齐同理 </param>
		public static Bitmap Pad (Bitmap source, in OpenCvSharp.Size size, Align align = Align.Left) {
			Bitmap target = new Bitmap(size.Width, size.Height);
			using ( Graphics g = Graphics.FromImage(target) ) {
				switch ( align ) {
					default:
					case Align.Left:
						g.DrawImage(source, new Rectangle(0, 0, size.Width, size.Height),
						   new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
						break;
					case Align.Right:
						g.DrawImage(source, new Rectangle(size.Width - source.Width, 0, size.Width, size.Height),
						   new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
						break;
				}
			}
			return target;
		}
	}
}
