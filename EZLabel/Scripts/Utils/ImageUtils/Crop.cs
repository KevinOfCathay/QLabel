using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using OpenCvSharp;
using System.Numerics;

namespace QLabel {
	internal static partial class ImageUtils {
		/// <summary>
		/// 根据给出的方框 box（顶点位置tl，长宽wh）
		/// 裁剪 bitmap
		/// </summary>
		public static Bitmap CropBitmap (Bitmap source, in Rect box, InterpolationMode interp = InterpolationMode.Bilinear) {
			Bitmap target = new Bitmap(box.Width, box.Height);
			using ( Graphics g = Graphics.FromImage(target) ) {
				g.InterpolationMode = interp;
				g.DrawImage(source, new Rectangle(0, 0, box.Width, box.Height),
				   new Rectangle(box.X, box.Y, box.Width, box.Height), GraphicsUnit.Pixel);
			}
			return target;
		}

		/// <summary>
		/// 根据给出的方框 box（顶点位置tl，长宽wh）
		/// 裁剪 bitmap
		/// </summary>
		public static Bitmap CropBitmap (Bitmap source, in Rect2f box, InterpolationMode interp = InterpolationMode.Bilinear) {
			int width = (int) box.Width, height = (int) box.Height;
			Bitmap target = new Bitmap(width, height);
			using ( Graphics g = Graphics.FromImage(target) ) {
				g.InterpolationMode = interp;
				g.DrawImage(source, new Rectangle(0, 0, width, height),
				   new Rectangle((int) box.X, (int) box.Y, width, height), GraphicsUnit.Pixel);
			}
			return target;
		}

		/// <summary>
		/// 在一个 Bitmap 上裁剪下多个方框
		/// </summary>
		public static List<Bitmap> CropBitmap (Bitmap source, in ICollection<Rect> boxes, InterpolationMode interp = InterpolationMode.Bilinear) {
			List<Bitmap> res = new List<Bitmap>(boxes.Count);
			foreach ( var box in boxes ) {
				Bitmap target = new Bitmap(box.Width, box.Height);
				using ( Graphics g = Graphics.FromImage(target) ) {
					g.InterpolationMode = interp;
					g.DrawImage(source, new Rectangle(0, 0, box.Width, box.Height),
					   new Rectangle(box.X, box.Y, box.Width, box.Height), GraphicsUnit.Pixel);
				}
				res.Add(target);
			}
			return res;
		}
	}
}
