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
	public partial class DraggableLine : UserControl, IAnnotationElement {
		public enum Side { A, B };
		public event Action<IAnnotationElement> eSelected, eUnselected;
		public event Action<DraggableLine, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;

		public readonly Guid dot_a_id, dot_b_id;
		public DraggableDot dot_a, dot_b;
		AnnoData _data;   // 这个矩形所对应的注释数据

		public UIElement ui_element => this;
		public AnnoData data { get => _data; set => _data = value; }
		public Vector2[] cpoints { get { return new Vector2[] { new Vector2((float) line.X1, (float) line.Y1), new Vector2((float) line.X2, (float) line.Y2) }; } }
		public Vector2[] convex_hull { get { return Array.Empty<Vector2>(); } set { } }

		public DraggableLine () {
			InitializeComponent();
		}
		public DraggableLine (Vector2 ca, Vector2 cb, Guid a_id, Guid b_id) {
			InitializeComponent();
			line.X1 = ca.X;
			line.Y1 = ca.Y;

			line.X2 = cb.X;
			line.Y2 = cb.Y;

			this.dot_a_id = a_id;
			this.dot_b_id = b_id;
		}
		public DraggableLine (DraggableDot dot_a, DraggableDot dot_b) {
			InitializeComponent();
			line.X1 = dot_a.cpoints[0].X;
			line.Y1 = dot_a.cpoints[0].Y;

			line.X2 = dot_b.cpoints[0].X;
			line.Y2 = dot_b.cpoints[0].Y;

			this.dot_a_id = dot_a.data.guid;
			this.dot_b_id = dot_b.data.guid;
		}
		public void Reposition (Vector2 new_cpoint, Side side) {
			switch ( side ) {
				case Side.A:
					line.X1 = new_cpoint.X;
					line.Y1 = new_cpoint.Y;
					break;
				case Side.B:
					line.X2 = new_cpoint.X;
					line.Y2 = new_cpoint.Y;
					break;
				default:
					break;
			}
		}
		void IAnnotationElement.MouseDown (MainCanvas canvas, MouseEventArgs e) {
		}
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
		}
		void IAnnotationElement.MouseUp (MainCanvas canvas, MouseEventArgs e) {
		}
		public void Draw (MainCanvas canvas, Vector2[] cpoints) {
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
		public void PostDelete (MainCanvas canvas) {
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
