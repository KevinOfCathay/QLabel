using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLabel {
	public class ChangeAnnoelemSize : IAction {
		public ChangeAnnoelemSize (Canvas canvas, IAnnotationElement elem, Vector2[] size_before, Vector2[] size_after) {
			this.canvas = canvas;
			this.elem = elem;
			this.size_before = size_before;
			this.size_after = size_after;
		}
		private readonly Canvas canvas;
		private readonly IAnnotationElement elem;
		private readonly Vector2[] size_before, size_after;
		public void Do () {
			elem.Draw(canvas, size_after);
		}

		public void Undo () {
			elem.Draw(canvas, size_before);
		}
	}
}
