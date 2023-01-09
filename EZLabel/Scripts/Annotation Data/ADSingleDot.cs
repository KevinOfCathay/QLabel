﻿using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 单一的点
	/// </summary>
	public record ADSingleDot : AnnoData {
		public ADSingleDot
			(ReadOnlySpan<Vector2> points, int clas = 0, string label = "", float conf = 1.0f) :
			base(points, Type.Dot, clas, label, conf) {
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}
	}
}
