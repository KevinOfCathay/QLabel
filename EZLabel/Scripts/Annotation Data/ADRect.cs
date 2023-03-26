﻿using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADRect : AnnoData {
		public ADRect
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f) :
			base(points, Type.Rectangle, clas, conf) {
		}
		public ADRect
			(float xmin, float ymin, float xmax, float ymax, ClassLabel clas, float conf = 1.0f) :
			base(new ReadOnlySpan<Vector2>(new Vector2[] { new Vector2(xmin, ymin), new Vector2(xmax, ymin), new Vector2(xmin, ymax), new Vector2(xmax, ymax) }),
				Type.Rectangle, clas, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			// 创建一个矩形
			DraggableRectangle rect = new DraggableRectangle { data = this };
			Vector2 tl = canvas.CanvasPosition(rpoints[0]);
			Vector2 br = canvas.CanvasPosition(rpoints[3]);
			rect.Draw(canvas, new Vector2[] { tl, br });
			return rect;
		}

		public override void Visualize (Bitmap bitmap, Vector2 scale, Vector2 offset) {
			throw new NotImplementedException();
		}
	}
}
