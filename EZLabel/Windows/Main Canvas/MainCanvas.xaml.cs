using EZLabel.Custom_Control.Image_View;
using EZLabel.Scripts.AnnotationToolManager;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EZLabel.Windows.Main_Canvas {
	public partial class MainCanvas : UserControl {
		private double zoom;          // zoom level
		public Point image_size { get; private set; } = new Point(0, 0);      // 图片大小，用于计算annotation的位置，在未加载时图片大小为 0
		public Point image_offset { get; private set; } = new Point(0, 0);      // 图片在画布上的偏移量，用于计算annotation的位置，在未加载时图片，或者图片居中时大小为 0
		public Action<MainCanvas, ImageFileData> eImageLoaded;
		public Action<MainCanvas, MouseEventArgs> eMouseDown, eMouseUp, eMouseMove;
		public RectangularBoxAnnotationTool tool = new RectangularBoxAnnotationTool();

		public MainCanvas () {
			InitializeComponent();
			tool.Activate(this);
			this.image_quick_info_panel.canvas = this.annotation_canvas;
		}

		public Point GetImageSize () {
			return new Point(canvas_image.ActualWidth, canvas_image.ActualHeight);
		}
		public Point GetImagePosition () {
			return new Point(Canvas.GetTop(canvas_grid), Canvas.GetLeft(canvas_grid));
		}

		public void LoadImage (ImageFileData data) {
			var hscale = data.height / annotation_canvas.ActualHeight;
			var wscale = data.width / annotation_canvas.ActualWidth;

			double target_height, target_width;
			// Resize canvas to auto
			if ( hscale <= 1 && wscale <= 1 ) {
				target_height = data.height;
				target_width = data.width;
				zoom = 1.0;
			} else {
				var scale = hscale > wscale ? hscale : wscale;
				target_height = annotation_canvas.ActualHeight;
				target_width = annotation_canvas.ActualWidth;
				zoom = 1 / scale;
			}
			canvas_image.Width = target_width;
			canvas_image.Height = target_height;
			canvas_image.Source = data.source;

			// 重置图像的 offset
			image_offset = GetOffsetFromScroll();

			// 设置底层信息栏的文字
			image_quick_info_panel.SetZoomText(zoom);

			eImageLoaded?.Invoke(this, data);
		}

		/// <summary>
		/// 从 scrollbar 中获取当前的 offset
		/// 如果 offset = 0, 意味着 scrollbar 没有转动
		/// </summary>
		private Point GetOffsetFromScroll () {
			return new Point(scroll.HorizontalOffset, scroll.VerticalOffset);
		}

		private void canvas_PreviewMouseDown (object sender, MouseButtonEventArgs e) {
			Debug.WriteLine("canvas mouse down");
			eMouseDown?.Invoke(this, e);
		}

		private void scroll_ScrollChanged (object sender, ScrollChangedEventArgs e) {
			// 重新设定图片的 offset
			Point p = GetOffsetFromScroll();
			image_offset = p;
		}

		private void annotation_canvas_PreviewMouseMove (object sender, MouseEventArgs e) {
			eMouseMove?.Invoke(this, e);
		}

		private void canvas_PreviewMouseUp (object sender, MouseButtonEventArgs e) {
			Debug.WriteLine("canvas mouse up");
			eMouseUp?.Invoke(this, e);
		}
	}
}
