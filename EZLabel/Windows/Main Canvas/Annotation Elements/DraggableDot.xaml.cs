using OpenCvSharp;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 点
	/// </summary>
	public partial class DraggableDot : UserControl, IAnnotationElement {
		public event Action<IAnnotationElement> eSelected, eUnselected;
		public event Action<DraggableDot, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;
		public bool activate { private set; get; } = false;
		private Vector2 position, mouse_position;

		AnnoData _data;   // 这个点所对应的注释数据
		public UIElement ui_element => this;
		public AnnoData data { get { return _data; } set { _data = value; } }
		public Vector2[] cpoints {
			get {
				float left = (float) Canvas.GetLeft(dot);
				float top = (float) Canvas.GetTop(dot);
				return new Vector2[] { new Vector2(left, top) };
			}
		}
		public Vector2[] convex_hull { get { return cpoints; } }
		public Color stroke_color { set { dot.Stroke = new SolidColorBrush(value); } }
		public Color fill_color { set { dot.Fill = new SolidColorBrush(value); } }
		/// <summary>
		/// 这个点相关联的线
		/// </summary>
		public List<Line> linked_line = new List<Line>();

		public DraggableDot () {
			InitializeComponent();
		}
		public DraggableDot (Vector2 cpoint, Line[] lines) {
			InitializeComponent();
			Canvas.SetLeft(this, cpoint.X);
			Canvas.SetTop(this, cpoint.Y);
			if ( lines != null ) {
				linked_line.AddRange(lines);
				foreach ( var line in lines ) { line.AddLink(this); }
			}
		}
		public void Draw (Canvas canvas, Vector2[] points) {
			if ( points != null ) {
				switch ( points.Length ) {
					case 1:
						var p = points[0];
						Canvas.SetLeft(this, p.X);
						Canvas.SetTop(this, p.Y);
						position = p;
						break;
					default:
						throw new ArgumentException();
				}
			}
		}
		public void Shift (Canvas canvas, Vector2 shift) {
			this.position += shift;
			Canvas.SetLeft(this, this.position.X);
			Canvas.SetTop(this, this.position.Y);
		}
		private void dot_PreviewMouseDown (object sender, MouseButtonEventArgs e) {
			if ( sender == e.OriginalSource ) {
				activate = true;
				eMouseDown?.Invoke(this, e);
			}
		}
		private void dot_PreviewMouseMove (object sender, MouseEventArgs e) {
			if ( activate ) { eMouseMove?.Invoke(this, e); }
		}
		private void dot_PreviewMouseUp (object sender, MouseButtonEventArgs e) {
			activate = false;
			eMouseDown?.Invoke(this, e);
		}
		public void Delete (MainCanvas canvas) {
			canvas.annotation_canvas.Children.Remove(this);
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Select () {
			eSelected?.Invoke(this);
		}
		public void Unselect () { eUnselected?.Invoke(this); }
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
		public new void MouseDown (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var position = e.GetPosition(canvas);
			mouse_position = new Vector2((float) position.X, (float) position.Y);
		}
		/// <summary>
		/// 拖动点将会移动点的位置
		/// </summary>
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public new void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public void Highlight () {
			throw new NotImplementedException();
		}
		public void Densify (MainCanvas canvas) {

		}
		public IAnnotationElement ToPolygon (MainCanvas canvas) {
			return this;
		}
	}
}
