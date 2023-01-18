using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.AnnotationToolManager;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace QLabel.Windows.Main_Canvas {
	public partial class MainCanvas : UserControl {
		private float image_scale = 0f;          // image scale level
		public bool can_annotate { get; private set; } = false;      // 当前画布是否可以进行标注
		public Point image_size { get; private set; } = new Point(0, 0);      // 图片大小，用于计算annotation的位置，在未加载时图片大小为 0
		public Point image_offset { get; private set; } = new Point(0, 0);      // 图片在画布上的偏移量，用于计算annotation的位置，在未加载时图片，或者图片居中时大小为 0		
		public Action<MainCanvas, double> eCanvasRescale;      // 画布改变大小（影响画布上所有元素的大小和位置）
		public Action<MainCanvas, BitmapImage> eImageLoaded;
		public Action<MainCanvas, IAnnotationElement> eAnnotationElementAdded, eAnnotationElementRemoved;
		public Action<MainCanvas, MouseEventArgs> eMouseDown, eMouseUp, eMouseMove;
		public List<IAnnotationElement> annotation_collection = new List<IAnnotationElement>();        // 用于存放所有 annotation 的地方

		public MainCanvas () {
			InitializeComponent();
			this.image_quick_info_panel.canvas = this.annotation_canvas;
			eMouseMove += (MainCanvas c, MouseEventArgs e) => {
				Point pos = e.GetPosition(this);
				this.image_quick_info_panel.SetMousePositionText(pos);
				this.image_quick_info_panel.SetRelativePositionText(RealPosition(new Vector2((float) pos.X, (float) pos.Y)));
			};
			//eImageLoaded += (MainCanvas c, BitmapImage img) => {
			//	image_quick_info_panel.SetImageSize(img.width, img.height);
			//};
		}
		public Vector2 GetImageSize () {
			return new Vector2((float) canvas_image.ActualWidth, (float) canvas_image.ActualHeight);
		}
		public Vector2 GetCanvasSize () {
			return new Vector2((float) annotation_canvas.ActualWidth, (float) annotation_canvas.ActualHeight);
		}
		/// <summary>
		/// 画布上的点对应的图片上的点
		/// </summary>
		public Vector2 RealPosition (Vector2 point) {
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
			Vector2 diff = new Vector2(point.X, point.Y) - ( tl + offset );

			// 4. 考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 / 0.5 （即×2）
			float imgsc = image_scale == 0f ? 1f : image_scale;    // 防止 inf
			diff = diff / imgsc;

			return diff;
		}
		/// <summary>
		/// 图片上的点对应的画布上的点
		/// </summary>
		public Vector2 CanvasPosition (Vector2 point) {
			Vector2 canvas_sz = GetCanvasSize();
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 1. 逆运算，考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 * 0.5
			float imgsc = ( image_scale == 0f ? 1f : image_scale );    // 防止 inf
			var cpoint = point * imgsc;

			// 2. 考虑 scroll offset 的影响
			Vector2 offset = new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset);

			// 3.  计算点与左上角点的差
			cpoint = new Vector2(cpoint.X, cpoint.Y) + ( tl + offset );

			return cpoint;
		}
		public void ClearCanvas () {
			foreach ( var elem in annotation_collection ) {
				elem.Delete(this);
			}
			annotation_collection.Clear();
		}
		public void ResizeImage () {

		}
		/// <summary>
		/// 将图片加载到画布上
		/// </summary>
		public async void LoadImage (string path) {
			BitmapImage image = await LoadImageFromFile(path);
			double height = image.PixelHeight;
			double width = image.PixelWidth;

			// 设置当前的图像源
			canvas_image.Source = image;

			var hscale = height / annotation_canvas.ActualHeight;
			var wscale = width / annotation_canvas.ActualWidth;

			double target_height, target_width;
			// Resize canvas to auto
			if ( hscale <= 1 && wscale <= 1 ) {
				target_height = height;
				target_width = width;
				image_scale = 1f;
			} else {
				var scale = hscale > wscale ? hscale : wscale;
				target_height = annotation_canvas.ActualHeight;
				target_width = annotation_canvas.ActualWidth;
				image_scale = 1f / (float) scale;
			}
			canvas_image.Width = target_width;
			canvas_image.Height = target_height;

			// 重置图像的 offset
			image_offset = GetOffsetFromScroll();

			// 设置底层信息栏的文字
			image_quick_info_panel.SetZoomText(image_scale);

			// 加载完了图片以后就可以开始 annotate
			can_annotate = true;
			eImageLoaded?.Invoke(this, image);
		}
		private async Task<BitmapImage> LoadImageFromFile (string path) {
			return await Task.Run(
				() => {
					BitmapImage image = new BitmapImage();
					image.BeginInit();
					image.UriSource = new Uri(path);
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.EndInit();
					image.Freeze();
					return image;
				}
				);
		}
		public void AddAnnoElements (IAnnotationElement element) {
			if ( can_annotate ) {          // 只有在画布上有内容时才会加入 element
				if ( element != null ) {
					annotation_collection.Add(element);
					// 将 class label 加入到 label 集合中
					ProjectManager.project.AddLabel(element.data.clas);
					eAnnotationElementAdded?.Invoke(this, element);
				}
			}
		}
		public void AddBulkAnnoElements (IAnnotationElement[] elements) {
			if ( can_annotate ) {          // 只有在画布上有内容时才会加入 element
				if ( elements != null ) {
					foreach ( var element in elements ) {
						annotation_collection.Add(element);
						// 将 class label 加入到 label 集合中
						ProjectManager.project.AddLabel(element.data.clas);
						eAnnotationElementAdded?.Invoke(this, element);
					}
				}
			}
		}
		public void RemoveAnnoElements (IAnnotationElement element) {
			if ( element != null && annotation_collection.Contains(element) ) {
				annotation_collection.Remove(element);
				element.Delete(this);
				eAnnotationElementRemoved?.Invoke(this, element);
			}
		}
		/// <summary>
		/// 从 scrollbar 中获取当前的 offset
		/// 如果 offset = 0, 意味着 scrollbar 没有转动
		/// </summary>
		private Point GetOffsetFromScroll () {
			return new Point(scroll.HorizontalOffset, scroll.VerticalOffset);
		}
		private void canvas_PreviewMouseDown (object sender, MouseButtonEventArgs e) {
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
