using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// Interaction logic for DraggablePolygon.xaml
	/// </summary>
	public partial class DraggablePolygon : UserControl, IAnnotationElement {
		public event Action<IAnnotationElement> eSelected;
		private Vector2 mouse_down_position, mouse_cur_position;
		private const float dot_radius = 8f;
		public DraggablePolygon () {
			InitializeComponent();
			dots = Array.Empty<Dot>();
		}
		public DraggablePolygon (MainCanvas mc, Span<Vector2> cpoints) {
			InitializeComponent();
			vertex = cpoints.Length;

			// 从 cpoints 中创建多边形的顶点
			polygon.Points = new PointCollection(cpoints.to_points());
			dots = new Dot[cpoints.Length];
			int index = 0;
			foreach ( var cpoint in cpoints ) {
				// 创建点
				Dot dot = new Dot(cpoint, dot_radius);
				Canvas.SetLeft(dot, cpoint.X - dot_radius / 2);
				Canvas.SetTop(dot, cpoint.Y - dot_radius / 2);

				// 为点创建事件
				dot.eMouseEnter += delegate (Dot _, EventArgs _) { dot_index = index; };
				mc.annotation_canvas.Children.Add(dot);
				dots[index] = dot;
			}
			_convex_hull = CalculateConvexHull(cpoints);
		}
		AnnoData _data;   // 这个多边形所对应的注释数据
		private Dot[] dots;
		private int vertex; private int dot_index;
		private Vector2[] _convex_hull;
		public AnnoData data { get { return _data; } set { _data = value; } }
		public Vector2[] cpoints {
			get {
				Vector2[] _cpoints = new Vector2[vertex];
				int index = 0;
				foreach ( var point in polygon.Points ) {
					_cpoints[index] = new Vector2((float) point.X, (float) point.Y);
					index += 1;
				}
				return _cpoints;
			}
		}
		public Vector2[] convex_hull {
			get { if ( _convex_hull != null ) { return _convex_hull; } else { throw new NullReferenceException(); } }
		}
		public void Delete (MainCanvas canvas) {
			canvas.annotation_canvas.Children.Remove(this);
		}
		public void Draw (Canvas canvas, Vector2[] points) {
			throw new NotImplementedException();
		}
		public void Shift (Canvas canvas, Vector2 shift) {
			Vector2[] new_points = cpoints;
			Parallel.For(0, new_points.Length, (i) => {
				new_points[i] += shift;
			});
			polygon.Points = new PointCollection(new_points.to_points());
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
		public new void MouseDown (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var position = e.GetPosition(canvas);
			mouse_down_position = new Vector2((float) position.X, (float) position.Y);
			mouse_cur_position = mouse_down_position;
		}
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			// 记录按下时的鼠标位置
			var position = e.GetPosition(canvas);
			var mouse_temp_position = new Vector2((float) position.X, (float) position.Y);
			var shift = mouse_temp_position - mouse_cur_position;
		}
		public new void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public void Select () {
			eSelected?.Invoke(this);
		}
		public void Highlight () {
			// if ( highlight_storyboard != null ) { BeginStoryboard(highlight_storyboard); }
		}
		public void Densify (MainCanvas canvas) {

		}
		public void ToPolygon (MainCanvas canvas) {
			// 什么都不做
		}
		public Vector2[] CalculateConvexHull (Span<Vector2> points) {
			// 两个点以下时，返回点
			if ( points.Length <= 2 ) {
				return points.ToArray();
			}
			// lexicographical sort
			points.Sort((x, y) => {
				if ( x.X < y.X ) {
					return 1;
				} else if ( x.X > y.X ) {
					return -1;
				} else {
					if ( x.Y < y.Y ) {
						return 1;
					} else if ( x.Y > y.Y ) {
						return -1;
					}
					return 0;
				}
			});
			int k = 0, len = points.Length;
			Span<Vector2> hull = stackalloc Vector2[2 * len];
			// Build lower hull
			foreach ( var point in points ) {
				while ( k >= 2 && cross(hull[k - 2], hull[k - 1], point) <= 0 ) {
					k -= 1;
				}
				hull[k] = point; k += 1;
			}
			// Build upper hull
			for ( int i = len - 2, t = k + 1; i >= 0; i -= 1 ) {
				while ( k >= t && cross(hull[k - 2], hull[k - 1], points[i]) <= 0 ) {
					k -= 1;
				}
				hull[k] = points[i];
				k += 1;
			}
			if ( k > 1 ) {
				return hull.Slice(0, k - 1).ToArray();
			}
			return hull.ToArray();
		}
		private float cross (Vector2 a, Vector2 x, Vector2 y) {
			return ( x.X - a.X ) * ( y.Y - a.Y ) - ( x.Y - a.Y ) * ( y.X - a.X );
		}
	}
}
