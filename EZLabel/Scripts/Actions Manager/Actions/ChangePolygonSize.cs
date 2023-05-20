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

namespace QLabel.Actions {
	internal class ChangePolygonSize : ChangeElementSize, IAction {
		public ChangePolygonSize (
			MainCanvas canvas, DraggablePolygon elem,
			AnnoData data_before, AnnoData data_after) :
			base(canvas, data_before, data_after) {
			this.elem = elem;
			initialize = true;
		}
		private bool initialize;
		private readonly DraggablePolygon elem;
		public string name => "Change Polygon Size";
		public void Do () {
			// 新的 annodatas
			elem.data = data_after;
			if ( !initialize ) {
				var points_after = new Vector2[data_after.rpoints.Length];
				for ( var i = 0; i < data_after.rpoints.Length; i += 1 ) {
					points_after[i] = canvas.CanvasPosition(data_after.rpoints[i]);
				}
			}
			initialize = false;
			canvas.ModifiedAnnoElements(elem);
		}
		public void Undo () {
			// 还原旧的 annodatas
			elem.data = data_before;
			var points_before = new Vector2[data_before.rpoints.Length];
			for ( var i = 0; i < data_before.rpoints.Length; i += 1 ) {
				points_before[i] = canvas.CanvasPosition(data_before.rpoints[i]);
			}
			elem.Draw(canvas, points_before);
			canvas.ModifiedAnnoElements(elem);
		}
	}
}
