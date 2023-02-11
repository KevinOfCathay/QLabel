using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Windows.Main_Canvas {
	/// <summary>
	/// Interaction logic for ImageQuickInfoPanel.xaml
	/// </summary>
	public partial class ImageQuickInfoPanel : UserControl {
		private Canvas _canvas;
		private Image _img;
		public Canvas canvas {
			private get { return _canvas; }
			set {
				_canvas = value;
				_img = value.FindName("canvas_image") as Image;
			}
		}

		public ImageQuickInfoPanel () {
			InitializeComponent();
		}
		public void SetZoomText (double zoom) { zoom_level.Text = ( zoom * 100.0 ).ToString("N3") + "%"; }
		public void SetMousePositionText (Point mouse_pos) { mouse_x_pos.Text = ( (int) mouse_pos.X ).ToString(); mouse_y_pos.Text = ( (int) mouse_pos.Y ).ToString(); }
		public void SetRelativePositionText (Point relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetRelativePositionText (Vector2 relative_pos) { real_x_pos.Text = ( (int) relative_pos.X ).ToString(); real_y_pos.Text = ( (int) relative_pos.Y ).ToString(); }
		public void SetImageSize (double width, double height) { image_width.Text = ( (int) width ).ToString(); image_height.Text = ( (int) height ).ToString(); }
		public void SetImageSize (int width, int height) { image_width.Text = width.ToString(); image_height.Text = height.ToString(); }

		public void OneToOneRatio () {
			var data = _img.Source;

			// 改变 canvas 的尺寸（否则 scrollbar 不会出现）
			canvas.Width = data.Width;
			canvas.Height = data.Height;

			_img.Width = data.Width;
			_img.Height = data.Height;

			SetZoomText(1.0);
		}
		public void FillRatio () {
			var data = _img.Source;
			var hscale = data.Height / _canvas.ActualHeight;
			var wscale = data.Width / _canvas.ActualWidth;

			var scale = hscale > wscale ? hscale : wscale;

			_img.Width = _canvas.ActualWidth;
			_img.Height = _canvas.ActualHeight;
			SetZoomText(1.0 / scale);
		}
		private void one_one_ratio_Click (object sender, RoutedEventArgs e) {
			OneToOneRatio();
		}
		private void fill_canvas_Click (object sender, RoutedEventArgs e) {
			FillRatio();
		}

	}
}

