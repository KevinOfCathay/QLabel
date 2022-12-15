using System;
using System.Collections.Generic;
using System.Linq;
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

namespace EZLabel.Windows.Main_Canvas {
	/// <summary>
	/// Interaction logic for ImageQuickInfoPanel.xaml
	/// </summary>
	public partial class ImageQuickInfoPanel : UserControl {
		public Canvas canvas {
			private get { return _canvas; }
			set {
				_canvas = value;
				_img = value.FindName("canvas_image") as Image;
			}
		}
		private Canvas _canvas;
		private Image _img;

		public ImageQuickInfoPanel () {
			InitializeComponent();
		}
		public void SetZoomText (double zoom) {
			zoom_level.Text = ( zoom * 100.0 ).ToString("N3") + "%";
		}
		public void OneToOneRatio () {
			var data = _img.Source;

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
