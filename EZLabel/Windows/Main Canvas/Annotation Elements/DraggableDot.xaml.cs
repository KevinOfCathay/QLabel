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
		private Vector2 mouse_position;
		private float _radius = 8f;

		AnnoData _data;   // 这个点所对应的注释数据
		public UIElement ui_element => this;
		public AnnoData data { get { return _data; } set { _data = value; } }
		public Vector2 _cpoint;
		public Vector2[] cpoints {
			get { return new Vector2[] { _cpoint }; }
		}
		public Vector2[] convex_hull { get { return cpoints; } }
		public Color stroke_color { set { dot.Stroke = new SolidColorBrush(value); } }
		public Color fill_color { set { dot.Fill = new SolidColorBrush(value); } }
		/// <summary>
		/// 这个点相关联的线
		/// </summary>
		public Dictionary<Guid, DraggableLine> linked_lines_a = new Dictionary<Guid, DraggableLine>();
		public Dictionary<Guid, DraggableLine> linked_lines_b = new Dictionary<Guid, DraggableLine>();

		public DraggableDot () {
			InitializeComponent();
		}
		public DraggableDot (Vector2 cpoint, IEnumerable<Guid> line_id_a, IEnumerable<Guid> line_id_b, float radius = 8f) {
			InitializeComponent();
			Height = radius; Width = radius; _radius = radius;
			this._cpoint = cpoint;
			Canvas.SetLeft(this, cpoint.X - radius / 2);
			Canvas.SetTop(this, cpoint.Y - radius / 2);
			foreach ( var id in line_id_a ) {
				linked_lines_a.Add(id, null);
			}
			foreach ( var id in line_id_b ) {
				linked_lines_b.Add(id, null);
			}
		}
		public DraggableDot (Vector2 cpoint, DraggableLine[] lines, float radius = 8f) {
			InitializeComponent();
			Height = radius; Width = radius; _radius = radius;
			this._cpoint = cpoint;
			Canvas.SetLeft(this, cpoint.X - radius / 2);
			Canvas.SetTop(this, cpoint.Y - radius / 2);
			if ( lines != null ) {
				foreach ( var line in lines ) {
					if ( data.guid == line.dot_a_id ) {
						linked_lines_a.Add(line.data.guid, line);
					} else if ( data.guid == line.dot_b_id ) {
						linked_lines_b.Add(line.data.guid, line);
					}
				}
			}
		}
		public void Draw (MainCanvas canvas, Vector2[] points) {
			if ( points != null ) {
				switch ( points.Length ) {
					case 1:
						_cpoint = points[0];
						Canvas.SetLeft(this, _cpoint.X - _radius / 2);
						Canvas.SetTop(this, _cpoint.Y - _radius / 2);
						List<(Guid id, DraggableLine line)> temp = new List<(Guid id, DraggableLine line)>(capacity: linked_lines_a.Count);
						foreach ( var (id, line) in linked_lines_a ) {
							if ( line != null ) {
								line.Reposition(_cpoint, DraggableLine.Side.A);
							} else {
								var element = canvas.GetElementByID(id);
								if ( element != null && element is DraggableLine ) {
									var l = ( element as DraggableLine );
									l.Reposition(_cpoint, DraggableLine.Side.A);
									temp.Add((id, l));
								}
							}
						}
						foreach ( var (id, line) in temp ) {
							linked_lines_a[id] = line;
						}
						temp = new List<(Guid id, DraggableLine line)>(capacity: linked_lines_b.Count);
						foreach ( var (id, line) in linked_lines_b ) {
							if ( line != null ) {
								line.Reposition(_cpoint, DraggableLine.Side.B);
							} else {
								var element = canvas.GetElementByID(id);
								if ( element != null && element is DraggableLine ) {
									var l = ( element as DraggableLine );
									l.Reposition(_cpoint, DraggableLine.Side.B);
									temp.Add((id, l));
								}
							}
						}
						foreach ( var (id, line) in temp ) {
							linked_lines_b[id] = line;
						}
						break;
					default:
						throw new ArgumentException();
				}
			}
		}
		public void Shift (Canvas canvas, Vector2 shift) {
			this._cpoint += shift;
			Canvas.SetLeft(this, this._cpoint.X - _radius / 2);
			Canvas.SetTop(this, this._cpoint.Y - _radius / 2);
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
