using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	internal static partial class ImageUtils {
		public enum CornerAlign { TopLeft, Center }
		/// <summary>
		/// 裁剪并加边
		/// </summary>
		public static Bitmap CropPadBitmap (Bitmap image,
			in Rect2f crop_area, int target_width, int target_height,
			CornerAlign align = CornerAlign.TopLeft,
			InterpolationMode interp = InterpolationMode.Bilinear) {

			float s_width = crop_area.Width;
			float s_height = crop_area.Height;
			float t_width = target_width;
			float t_height = target_height;

			// 首先根据区域裁剪图像
			Bitmap cropped_bitmap = new Bitmap((int) s_width, (int) s_height);
			var g = Graphics.FromImage(cropped_bitmap);
			g.DrawImage(image,
				new RectangleF(0f, 0f, s_width, s_height),
				new RectangleF(crop_area.X, crop_area.Y, s_width, s_height), GraphicsUnit.Pixel);

			// 同时拥有宽和高
			if ( target_width != -1 && target_height != -1 ) {
				// 计算比例
				// 如果 w200 x h100 (2:1) 的源图像裁切为 w120 x h80 (1.5:1) 的图像
				// 裁剪一个 w200 x h133.3  的区域并缩放为 w120 x h80,   133.3 = 200 * (80 / 120)
				// 超出的区域需要由空白补足，所以不能取 150x100
				// 如果裁剪为 300x50 (6:1) 的图像，则裁剪一个600x100 的区域然后缩放
				float w_ratio = s_width / t_width;
				float h_ratio = s_height / t_height;

				float new_width = 0, new_height = 0, margin = 0f;
				RectangleF destination;
				// 如果宽比例大于高比例，则应当补高
				if ( w_ratio > h_ratio ) {
					new_width = s_width;  // 维持源宽尺寸不变
					new_height = MathF.Ceiling((float) s_width * ( t_height / t_width ));
					switch ( align ) {
						default:
						case CornerAlign.TopLeft:
							destination = new RectangleF(0f, 0f, t_width, t_height);
							break;
						case CornerAlign.Center:
							margin = ( new_height - s_height ) / ( s_width / t_width );    // 多出的高度
							destination = new RectangleF(0f, margin / 2f, t_width, t_height);
							break;
					}
				}
				// 如果宽比例大于高比例，则应当补宽
				else {
					new_width = MathF.Ceiling((float) s_height * ( t_width / t_height ));
					new_height = s_height;
					switch ( align ) {
						default:
						case CornerAlign.TopLeft:
							destination = new RectangleF(0f, 0f, t_width, t_height);
							break;
						case CornerAlign.Center:
							margin = ( new_width - s_width ) / ( s_height / t_height );    // 多出的宽度
							destination = new RectangleF(margin / 2f, 0f, t_width, t_height);
							break;
					}
				}
				// 裁剪后的 bitmap
				// 如果裁剪区域超出了边界，那么超出边界的地方将会透明
				Bitmap crop_pad_bitmap = new Bitmap(target_width, target_height);
				using ( var g2 = Graphics.FromImage(crop_pad_bitmap) ) {
					g2.InterpolationMode = interp;
					g2.DrawImage(cropped_bitmap,
						destination,
						new RectangleF(0f, 0f, new_width, new_height), GraphicsUnit.Pixel);
				}
				return crop_pad_bitmap;
			} else {
				// 拥有宽，但未指定高
				if ( target_width != -1 ) {
					// 计算比例
					// 如果 w200 x h100 (2:1) 的源图像裁切为 w120 x h_  的图像
					// 缩放为 w120 x h60,   60 = 120 * (100 / 200)
					float ratio = s_height / s_width;
					float new_width = t_width;
					float new_height = t_width * ratio;

					Bitmap crop_pad_bitmap = new Bitmap((int) new_width, (int) new_height);
					using ( var g2 = Graphics.FromImage(crop_pad_bitmap) ) {
						g2.InterpolationMode = interp;
						g2.DrawImage(cropped_bitmap,
							new RectangleF(0f, 0f, new_width, new_height),
							new RectangleF(0f, 0f, s_width, s_height), GraphicsUnit.Pixel);
					}
					return crop_pad_bitmap;
				} else if ( target_height != -1 ) {
					// 拥有高，但未指定宽
					// 如果 w200 x h100 (2:1) 的源图像裁切为 ｗ_ x h60  的图像
					// 缩放为 w180 x h60,   180 = 60 * (200 / 100)
					float ratio = s_width / s_height;
					float new_width = t_height * ratio;
					float new_height = t_height;

					Bitmap crop_pad_bitmap = new Bitmap((int) new_width, (int) new_height);
					using ( var g2 = Graphics.FromImage(crop_pad_bitmap) ) {
						g2.InterpolationMode = interp;
						g2.DrawImage(cropped_bitmap,
							new RectangleF(0f, 0f, new_width, new_height),
							new RectangleF(0f, 0f, s_width, s_height), GraphicsUnit.Pixel);
					}
					return crop_pad_bitmap;
				}
				// 未指定宽、高
				return cropped_bitmap;
			}
		}
	}
}
