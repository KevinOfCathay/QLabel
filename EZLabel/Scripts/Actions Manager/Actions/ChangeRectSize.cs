using QLabel.Scripts.AnnotationData;
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
	public class ChangeRectSize : IAction {
		public ChangeRectSize (
			MainCanvas canvas, DraggableRectangle elem,
			AnnoData data_before, AnnoData data_after) {
			this.canvas = canvas;
			this.elem = elem;
			this.data_before = data_before;
			this.data_after = data_after;
		}
		private readonly MainCanvas canvas;
		private readonly DraggableRectangle elem;
		private readonly AnnoData data_before, data_after;
		public void Do () {
			// 新的 annodata
			elem.data = data_after;
			var points_after = new Vector2[data_after.points.Length];
			for ( var i = 0; i < data_after.points.Length; i += 1 ) {
				points_after[i] = canvas.CanvasPosition(data_after.points[i]);
			}
			canvas.ModifiedAnnoElements(elem);
		}
		public void Undo () {
			// 还原旧的 annodata
			elem.data = data_before;
			var points_before = new Vector2[data_before.points.Length];
			for ( var i = 0; i < data_before.points.Length; i += 1 ) {
				points_before[i] = canvas.CanvasPosition(data_before.points[i]);
			}
			elem.Draw(canvas.canvas, points_before);
			canvas.ModifiedAnnoElements(elem);
		}
	}
}
