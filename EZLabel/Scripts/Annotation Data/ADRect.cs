﻿using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADRect : AnnoData {
		public ADRect
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f) :
			base(points, Type.Rectangle, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			// 创建一个矩形
			DraggableRectangle rect = new DraggableRectangle { data = this };
			Vector2 tl = canvas.CanvasPosition(rpoints[0]);
			Vector2 br = canvas.CanvasPosition(rpoints[3]);
			rect.Draw(canvas.annotation_canvas, new Vector2[] { tl, br });

			// 将新创建的矩形加入到画布中
			canvas.annotation_canvas.Children.Add(rect);

			return rect;
		}
	}
}
