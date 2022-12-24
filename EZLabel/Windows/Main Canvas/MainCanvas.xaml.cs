﻿using EZLabel.Custom_Control.Image_View;
using EZLabel.Scripts.AnnotationData;
using EZLabel.Scripts.AnnotationToolManager;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace EZLabel.Windows.Main_Canvas {
	public partial class MainCanvas : UserControl {
		private float image_scale = 0f;          // image scale level
		private ImageFileData cur_file;
		public Point image_size { get; private set; } = new Point(0, 0);      // 图片大小，用于计算annotation的位置，在未加载时图片大小为 0
		public Point image_offset { get; private set; } = new Point(0, 0);      // 图片在画布上的偏移量，用于计算annotation的位置，在未加载时图片，或者图片居中时大小为 0
		public Action<MainCanvas, ImageFileData> eImageLoaded;
		public Action<MainCanvas, IAnnotationElement> eAnnotationElementAdded, eAnnotationElementRemoved;
		public Action<MainCanvas, MouseEventArgs> eMouseDown, eMouseUp, eMouseMove;
		public AnnotationCollections annotation_collections = new AnnotationCollections();        // 用于存放所有 annotation 的地方
		public RectangularBoxAnnotationTool tool = new RectangularBoxAnnotationTool();

		public MainCanvas () {
			InitializeComponent();
			tool.Activate(this);
			this.image_quick_info_panel.canvas = this.annotation_canvas;
			eMouseMove += (MainCanvas c, MouseEventArgs e) => {
				Point pos = e.GetPosition(this);
				this.image_quick_info_panel.SetMousePositionText(pos);
				this.image_quick_info_panel.SetRelativePositionText(RelativePosition(pos));
			};
			eImageLoaded += (MainCanvas c, ImageFileData img) => {
				image_quick_info_panel.SetImageSize(img.width, img.height);
			};
		}

		public Vector2 GetImageSize () {
			return new Vector2((float) canvas_image.ActualWidth, (float) canvas_image.ActualHeight);
		}

		public Vector2 GetCanvasSize () {
			return new Vector2((float) annotation_canvas.ActualWidth, (float) annotation_canvas.ActualHeight);
		}

		/// <summary>
		/// 画布上的点对应的图片的实际点
		/// (position relative to the image)
		/// </summary>
		public Vector2 RelativePosition (Point point) {
			// 1. 首先获得图片左上角的点对应的位置，
			// 不考虑 scrollview 的 offset
			// 例如：图片尺寸为 128×128 且居中，
			// 左上角的点为 (画布宽W-128)/2,  (画布高H-128)/2
			// 即，点 （  (画布宽W-128)/2,  (画布高H-128)/2 ）--> （0, 0）
			Vector2 canvas_sz = GetCanvasSize();
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 2. 考虑 scroll offset 的影响
			// 例如：当 x offset = 10 时，坐标 （0, 0）所对应的实际点为 （10, 0）
			Vector2 offset = new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset);

			// 3.  计算点与左上角点的差
			Vector2 diff = new Vector2((float) point.X, (float) point.Y) - ( tl + offset );

			// 4. 考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 × 2
			float imgsc = image_scale == 0f ? 1f : image_scale;    // 防止 inf
			diff = diff / new Vector2(imgsc, imgsc);

			return diff;
		}

		public void ResizeImage () {

		}
		public void LoadImage (ImageFileData data) {
			var hscale = data.height / annotation_canvas.ActualHeight;
			var wscale = data.width / annotation_canvas.ActualWidth;

			double target_height, target_width;
			// Resize canvas to auto
			if ( hscale <= 1 && wscale <= 1 ) {
				target_height = data.height;
				target_width = data.width;
				image_scale = 1f;
			} else {
				var scale = hscale > wscale ? hscale : wscale;
				target_height = annotation_canvas.ActualHeight;
				target_width = annotation_canvas.ActualWidth;
				image_scale = 1f / (float) scale;
			}
			canvas_image.Width = target_width;
			canvas_image.Height = target_height;

			canvas_image.Source = data.source;

			// 重置图像的 offset
			image_offset = GetOffsetFromScroll();

			// 设置底层信息栏的文字
			image_quick_info_panel.SetZoomText(image_scale);

			// 设置当前的图像为 data
			cur_file = data;

			eImageLoaded?.Invoke(this, data);
		}
		public void AddAnnoElements (IAnnotationElement element) {
			annotation_collections.AddElement(cur_file, element);
			eAnnotationElementAdded?.Invoke(this, element);
		}
		public void RemoveAnnoElements (IAnnotationElement element) {
			annotation_collections.RemoveElement(cur_file, element);
			element.Delete();
			eAnnotationElementRemoved?.Invoke(this, element);
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
