using EZLabel.Windows.Main_Canvas;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace EZLabel.Scripts.AnnotationToolManager {
	public class DotAnnotationTool : AnnotationToolBase {
		DraggableDot dot;
		public override void Activate (MainCanvas canvas) {
			canvas.eMouseDown += CreateNewDot;
		}

		public override void Deactivate (MainCanvas canvas) {
			canvas.eMouseDown -= CreateNewDot;
		}

		public void CreateNewDot (MainCanvas canvas, MouseEventArgs e) {
			dot = new DraggableDot();

			// 在鼠标的位置放置点
			var p = e.GetPosition(canvas.annotation_canvas);
			Canvas.SetLeft(dot, p.X);
			Canvas.SetTop(dot, p.Y);

			canvas.annotation_canvas.Children.Add(dot);

			e.Handled = true;
		}
	}
}
