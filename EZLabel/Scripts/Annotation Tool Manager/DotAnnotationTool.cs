using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
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
using QLabel.Scripts.Projects;
using QLabel.Actions;

namespace QLabel.Scripts.AnnotationToolManager {
	public class DotAnnotationTool : ToolBase {
		DraggableDot dot;
		public override void Activate (MainCanvas canvas) {
			canvas.eMouseDown += CreateNewDot;
		}

		public override void Deactivate (MainCanvas canvas) {
			canvas.eMouseDown -= CreateNewDot;
		}

		public void CreateNewDot (MainCanvas canvas, MouseEventArgs e) {
			// 在鼠标的位置放置点
			var p = e.GetPosition(canvas.annotation_canvas);
			Vector2 cpoint = new Vector2((float) p.X, (float) p.Y);
			dot = new DraggableDot(cpoint, null);      // 初始化大小（否则不会在画布上显示）
			Canvas.SetZIndex(dot, 10);

			var rpoint = canvas.RealPosition(new Vector2((float) p.X, (float) p.Y));    // 坐标转换

			// 创建 anno 数据
			var data = new AnnotationData.ADDot(rpoint, ProjectManager.cur_label, Array.Empty<Guid>());
			dot.data = data;

			AddElementToCanvas create_dot = new AddElementToCanvas(canvas, dot);
			create_dot.Do();

			ActionManager.PushAction(create_dot);

			canvas.annotation_canvas.Children.Add(dot);
			e.Handled = true;
		}
	}
}
