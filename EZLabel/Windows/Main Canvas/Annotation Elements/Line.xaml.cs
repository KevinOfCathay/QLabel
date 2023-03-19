using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 线段
	/// </summary>
	public partial class Line : UserControl, IAnnotationElement {
		public event Action<IAnnotationElement> eSelected, eUnselected;
		public event Action<Line, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;

		public List<DraggableDot> linked_dots;
		AnnoData _data;   // 这个矩形所对应的注释数据

		public Vector2[] cpoints { get { return new Vector2[] { new Vector2((float) line.X1, (float) line.Y1), new Vector2((float) line.X2, (float) line.Y2) }; } }
		public AnnoData data { get => _data; set => _data = value; }
		public UIElement ui_element => this;
		public Vector2[] convex_hull { get { return Array.Empty<Vector2>(); } set { } }


		public Line () {
			InitializeComponent();
		}
		public Line (DraggableDot[] dots) {
			InitializeComponent();
			this.linked_dots = new List<DraggableDot>(dots);
		}

		void IAnnotationElement.MouseDown (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		void IAnnotationElement.MouseUp (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		public void Draw (Canvas canvas, Vector2[] points) {
			throw new NotImplementedException();
		}

		public void Shift (Canvas canvas, Vector2 shift) {
			throw new NotImplementedException();
		}

		public void Select () {
			throw new NotImplementedException();
		}

		public void Unselect () {
			throw new NotImplementedException();
		}

		public void Delete (MainCanvas canvas) {
			throw new NotImplementedException();
		}

		public IAnnotationElement ToPolygon (MainCanvas canvas) {
			throw new NotImplementedException();
		}

		public void Densify (MainCanvas canvas) {
			throw new NotImplementedException();
		}

		public void Highlight () {
			throw new NotImplementedException();
		}

		public void Show () {
			throw new NotImplementedException();
		}

		public void Hide () {
			throw new NotImplementedException();
		}
	}
}
