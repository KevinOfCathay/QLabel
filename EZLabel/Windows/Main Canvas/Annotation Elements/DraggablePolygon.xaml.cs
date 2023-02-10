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
		public DraggablePolygon () {
			InitializeComponent();
		}
		public DraggablePolygon (Vector2[] cpoints) : base() {
			vertex = cpoints.Length;

			// 从 cpoints 中创建多边形的顶点
			polygon.Points = new PointCollection(cpoints.to_points());
			foreach ( var cpoint in cpoints ) {
				// 创建点
			}
		}
		AnnoData _data;   // 这个多边形所对应的注释数据

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
		public Vector2[] convex_hull { get { throw new NotImplementedException(); } }
		private int vertex;
		private List<Dot> dots = new List<Dot>();

		public void Delete (MainCanvas canvas) {
			canvas.annotation_canvas.Children.Remove(this);
		}
		public void Draw (Canvas canvas, Vector2[] points) {
			throw new NotImplementedException();
		}
		public void Shift (Canvas canvas, Vector2 shift) {
			throw new NotImplementedException();
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
		public new void MouseDown (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public void MouseDrag (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public new void MouseUp (MainCanvas canvas, MouseEventArgs e) {
			throw new NotImplementedException();
		}
		public void Highlight () {
			// if ( highlight_storyboard != null ) { BeginStoryboard(highlight_storyboard); }
		}
		public void ToPolygon (MainCanvas canvas) {
			// 什么都不做
		}
	}
}
