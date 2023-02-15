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
		public event Action<Dot, EventArgs> eMouseEnter, eMouseLeave;
		public bool activate { private set; get; } = false;
		private Vector2 position;

		AnnoData _data;   // 这个点所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }

		public Dot () {
			InitializeComponent();
		}
		public Dot (Vector2 cpoint) {
			InitializeComponent();
			Canvas.SetLeft(this, cpoint.X);
			Canvas.SetTop(this, cpoint.Y);
		}
		private void DotMouseEnter (object sender, MouseEventArgs e) {
			eMouseEnter?.Invoke(this, e);
		}
		private void DotMouseLeave (object sender, MouseEventArgs e) {
			eMouseLeave?.Invoke(this, e);
		}
		public void Show () {
			Visibility = Visibility.Visible;
		}
		public void Hide () {
			Visibility = Visibility.Hidden;
		}
	}
}
