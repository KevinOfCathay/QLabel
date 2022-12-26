using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace QLabel.Scripts.Inference_Machine {
	public abstract class BaseInferenceMachine {
		/// <summary>
		/// 加载图片并且 resampling
		/// </summary>
		public Bitmap LoadImage (ImageFileData data) {
			return new Bitmap(Image.FromFile(data.filename), width, height);
		}

	}
}
