using QLabel.Scripts.AnnotationData;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 点 (非Annotation Element)
	/// </summary>
	public partial class Dot : UserControl {
		public event Action<Dot, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;
		public bool activate { private set; get; } = false;
		private Vector2 position;

		AnnoData _data;   // 这个点所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }

		public Dot () {
			InitializeComponent();
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
