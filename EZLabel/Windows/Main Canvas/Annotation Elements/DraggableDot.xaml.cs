using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Numerics;
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
	/// 点
	/// </summary>
	public partial class DraggableDot : UserControl, IAnnotationElement {
		public event Action<DraggableDot, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;
		public bool activate { private set; get; } = false;

		AnnoData _data;   // 这个点所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }

		public DraggableDot () {
			InitializeComponent();
		}

		public void Draw (Canvas canvas, Vector2[] points) {
			if ( points != null && points.Length >= 1 ) {
				Canvas.SetLeft(this, points[0].X);
				Canvas.SetTop(this, points[0].Y);
			}
		}

		private void dot_PreviewMouseDown (object sender, MouseButtonEventArgs e) {
			if ( sender == e.OriginalSource ) {
				activate = true;
				eMouseDown?.Invoke(this, e);
			}
		}

		private void dot_PreviewMouseMove (object sender, MouseEventArgs e) {
			if ( activate ) {
				eMouseMove?.Invoke(this, e);
				Debug.WriteLine("mouse moving");
			}
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
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
	}
}
