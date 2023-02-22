using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace QLabel.Windows.Main_Canvas {
	/// <summary>
	/// Interaction logic for ImageQuickInfoPanel.xaml
	/// </summary>
	public partial class ImageQuickInfoPanel : UserControl {
		public MainCanvas canvas { get; set; }

		public ImageQuickInfoPanel () {
			InitializeComponent();
		}
		public void SetZoomText (double zoom) { zoom_level.Text = ( zoom * 100.0 ).ToString("N3") + "%"; }
		public void SetZoomText (int zoom) { zoom_level.Text = zoom.ToString() + "%"; }
		public void SetMousePositionText (Point mouse_pos) { mouse_x_pos.Text = ( (int) mouse_pos.X ).ToString(); mouse_y_pos.Text = ( (int) mouse_pos.Y ).ToString(); }
		public void SetRelativePositionText (Point relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetRelativePositionText (Vector2 relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetImageSize (double width, double height) { image_width.Text = ( (int) width ).ToString(); image_height.Text = ( (int) height ).ToString(); }
		public void SetImageSize (int width, int height) { image_width.Text = width.ToString(); image_height.Text = height.ToString(); }

		public void QuarterImageSize () {
			canvas.SetImageScale(0.25f);
			SetZoomText(25);
		}
		public void HalfImageSize () {
			canvas.SetImageScale(0.5f);
			SetZoomText(50);
		}
		public void FullImageSize () {
			canvas.SetImageScale(1f);
			SetZoomText(100);
		}
		public void FitCanvas () {
			var img = canvas.canvas_image;
			var data = img.Source;

			var hscale = data.Height / canvas.ActualHeight;
			var wscale = data.Width / canvas.ActualWidth;

			var scale = hscale > wscale ? hscale : wscale;

			canvas.ChangeCanvasSize(canvas.ActualWidth, canvas.ActualHeight);

			img.Width = canvas.ActualWidth;
			img.Height = canvas.ActualHeight;

			SetZoomText(1.0 / scale);
		}
		private void OneToOneClick (object sender, RoutedEventArgs e) {
			FullImageSize();
		}
		private void QuarterClick (object sender, RoutedEventArgs e) {
			QuarterImageSize();
		}
		private void HalfClick (object sender, RoutedEventArgs e) {
			HalfImageSize();
		}
		private void fill_canvas_Click (object sender, RoutedEventArgs e) {
			FitCanvas();
		}

	}
}

