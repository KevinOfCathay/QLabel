using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 线段（不是 Annotation Element）
	/// </summary>
	public partial class Line : UserControl {
		public event Action<Line> eSelected, eUnselected;
		public event Action<Line, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;

		public List<DraggableDot> linked_dots;

		public Vector2[] cpoints { get { return new Vector2[] { new Vector2((float) line.X1, (float) line.Y1), new Vector2((float) line.X2, (float) line.Y2) }; } }
		public Line () {
			InitializeComponent();
		}
		public Line (DraggableDot[] dots) {
			InitializeComponent();
			this.linked_dots = new List<DraggableDot>(dots);
		}
	}
}
