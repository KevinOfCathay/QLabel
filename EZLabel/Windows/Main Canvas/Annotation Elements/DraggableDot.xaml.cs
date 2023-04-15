using OpenCvSharp;
using QLabel.Actions;
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
		private Vector2 mouse_down_position;
		private float _radius = 8f;

		AnnoData _data;   // 这个点所对应的注释数据
		public UIElement ui_element => this;
		public AnnoData data { get { return _data; } set { _data = value; } }
		public Vector2 _cpoint;
		public Vector2[] cpoints { get { return new Vector2[] { _cpoint }; } }
		public Vector2[] convex_hull { get { return cpoints; } }
		public Color stroke_color { set { dot.Stroke = new SolidColorBrush(value); } }
		public Color fill_color { set { dot.Fill = new SolidColorBrush(value); } }

		/// <summary>
		/// 这个点相关联的线
		/// </summary>
		private IEnumerable<Guid> links_id = new List<Guid>();
		public Dictionary<Guid, DraggableLine> linked_lines_a = new Dictionary<Guid, DraggableLine>();
		public Dictionary<Guid, DraggableLine> linked_lines_b = new Dictionary<Guid, DraggableLine>();

		public DraggableDot () {
			InitializeComponent();
		}
		public DraggableDot (Vector2 cpoint, IEnumerable<Guid> lines, float radius = 8f) {
			InitializeComponent();
			Height = radius; Width = radius; _radius = radius;
			this._cpoint = cpoint;
			Canvas.SetLeft(this, cpoint.X - radius / 2);
			Canvas.SetTop(this, cpoint.Y - radius / 2);
			links_id = lines;
		}
		public void LinkLines (MainCanvas canvas, IEnumerable<Guid> line_ids) {
			foreach ( var id in line_ids ) {
				if ( linked_lines_a.ContainsKey(id) || linked_lines_b.ContainsKey(id) ) {
					continue;
				}
				var line = canvas.GetElementByID(id) as DraggableLine;
				if ( line != null ) {
					if ( data.guid == line.dot_a_id ) {
						linked_lines_a.Add(line.data.guid, line);
					} else if ( data.guid == line.dot_b_id ) {
						linked_lines_b.Add(line.data.guid, line);
					}
				}
			}
		}
		public void Draw (MainCanvas canvas, Vector2[] points) {
			LinkLines(canvas, links_id);
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
		public void PostDelete (MainCanvas canvas) {
			// 删除和这个点相关联的所有线段
			// (线段必须要拥有2个端点)
			foreach ( var id in links_id ) {
				var element = canvas.GetElementByID(id);
				canvas.RemoveAnnoElements(element);
			}
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
			mouse_down_position = new Vector2((float) position.X, (float) position.Y);
		}
		/// <summary>
		/// 拖动点将会移动点的位置
		/// </summary>
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var eposition = e.GetPosition(canvas);
			var mouse_current_position = new Vector2((float) eposition.X, (float) eposition.Y);
			var shift = mouse_current_position - mouse_down_position;

			Draw(canvas, new Vector2[] { this.cpoints[0] + shift });
			mouse_down_position = mouse_current_position;
		}
		public new void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			// 创建新的 AnnoData
			List<Guid> link_ids = new List<Guid>();
			link_ids.AddRange(linked_lines_a.Keys);
			link_ids.AddRange(linked_lines_b.Keys);
			ADDot new_data = new ADDot(canvas.RealPosition(cpoints[0]), data.clas, link_ids, data.conf);
			ChangeDotSize changesize = new ChangeDotSize(canvas, this, data, new_data);
			ActionManager.PushAndExecute(changesize);
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
