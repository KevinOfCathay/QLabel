using QLabel.Scripts.AnnotationData;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Actions {
	internal class ChangeElementSize {
		public ChangeElementSize (MainCanvas canvas, AnnoData data_before, AnnoData data_after) {
			this.canvas = canvas;
			this.data_before = data_before;
			this.data_after = data_after;
		}
		protected readonly MainCanvas canvas;
		protected readonly AnnoData data_before, data_after;
	}
}
