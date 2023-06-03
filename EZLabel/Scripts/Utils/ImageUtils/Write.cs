using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLabel {
	internal static partial class ImageUtils {
		public static async Task WriteBitmapAsync (Bitmap bitmap, string save_path) {
			await Task.Run(delegate () { bitmap.Save(save_path); });
		}

		public static async Task WriteCroppedImageAsync (CroppedBitmap cropped_image, string save_path) {
			await Task.Run(
			    () => {
				    BitmapEncoder encoder = new PngBitmapEncoder();
				    var frame = BitmapFrame.Create(cropped_image);
				    encoder.Frames.Add(frame);
				    using ( FileStream fs = File.Create(save_path) ) {
					    encoder.Save(fs);
				    }
			    }
			);
		}
	}
}
