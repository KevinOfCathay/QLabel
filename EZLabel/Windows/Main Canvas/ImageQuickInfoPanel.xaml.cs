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
		public void SetMousePositionText (Point mouse_pos) { mouse_x_pos.Text = ( (int) mouse_pos.X ).ToString(); mouse_y_pos.Text = ( (int) mouse_pos.Y ).ToString(); }
		public void SetRelativePositionText (Point relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetRelativePositionText (Vector2 relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetImageSize (double width, double height) { image_width.Text = ( (int) width ).ToString(); image_height.Text = ( (int) height ).ToString(); }
		public void SetImageSize (int width, int height) { image_width.Text = width.ToString(); image_height.Text = height.ToString(); }

		public void HalfImageSize () {
			var img = canvas.canvas_image;
			var data = img.Source;

			// 改变 canvas 的尺寸（否则 scrollbar 不会出现）
			canvas.annotation_canvas.Width = data.Width * 0.5;
			canvas.annotation_canvas.Height = data.Height * 0.5;

			img.Width = data.Width * 0.5;
			img.Height = data.Height * 0.5;

			SetZoomText(1.0);
		}
		public void FullImageSize () {
			var img = canvas.canvas_image;
			var data = img.Source;

			img.Width = data.Width;
			img.Height = data.Height;

			canvas.ChangeCanvasSize(data.Width, data.Height);
			SetZoomText(1.0);
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
		private void HalfClick (object sender, RoutedEventArgs e) {
			HalfImageSize();
		}
		private void fill_canvas_Click (object sender, RoutedEventArgs e) {
			FitCanvas();
		}

	}
}

