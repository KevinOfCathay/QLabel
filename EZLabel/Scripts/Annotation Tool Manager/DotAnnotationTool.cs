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
using OpenCvSharp;
using System.Numerics;

namespace EZLabel.Scripts.AnnotationToolManager {
	public class DotAnnotationTool : ToolBase {
		DraggableDot dot;
		public override void Activate (MainCanvas canvas) {
			canvas.eMouseDown += CreateNewDot;
		}

		public override void Deactivate (MainCanvas canvas) {
			canvas.eMouseDown -= CreateNewDot;
		}

		public void CreateNewDot (MainCanvas canvas, MouseEventArgs e) {
			dot = new DraggableDot { Width = 8, Height = 8 };		// 初始化大小（否则不会在画布上显示）

			// 在鼠标的位置放置点
			var p = e.GetPosition(canvas.annotation_canvas);

			Canvas.SetLeft(dot, p.X);
			Canvas.SetTop(dot, p.Y);
			Canvas.SetZIndex(dot, 10);

			var pos = canvas.RelativePosition(p);    // 坐标转换

			// 创建 anno 数据
			dot.data = new AnnotationData.ADSingleDot() {
				points = new Vector2[] { pos }
			};

			canvas.annotation_canvas.Children.Add(dot);
			e.Handled = true;
		}
	}
}
