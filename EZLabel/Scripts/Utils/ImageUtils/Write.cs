using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	internal static partial class ImageUtils {
		public static async Task WriteBitmapAsync (Bitmap bitmap, string save_path) {
			await Task.Run(delegate () { bitmap.Save(save_path); });
		}
	}
}
