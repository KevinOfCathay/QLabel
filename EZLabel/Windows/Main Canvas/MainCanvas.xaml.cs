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
using System.Windows.Media.Media3D;

namespace QLabel.Windows.Main_Canvas {
	public partial class MainCanvas : UserControl {
		public const float margin = 10f;         // 图片四周预留的 margin

		public Canvas canvas { get { return annotation_canvas; } }
		public bool can_annotate { get; private set; } = false;      // 当前画布是否可以进行标注
		public Vector2 canvas_size_before;
		public Vector2 canvas_size { get { return new Vector2((float) this.scroll.ActualWidth, (float) this.scroll.ActualHeight); } }
		public float image_scale { get; private set; } = 0f;          // image scale level
		public Vector2 image_size { get; private set; } = new Vector2(0, 0);      // 图片大小，用于计算annotation的位置，在未加载时图片大小为 0
		public Vector2 image_offset { get; private set; } = new Vector2(0, 0);      // 图片在画布上的偏移量，用于计算annotation的位置，在未加载时图片，或者图片居中时大小为 0		
		public MouseButtonState button_state = MouseButtonState.Released;
		public event Action<MainCanvas, BitmapImage> eImageLoaded;
		public event Action<MainCanvas, IAnnotationElement> eAnnotationElementAdded, eAnnotationElementModified, eAnnotationElementRemoved;
		public event Action<MainCanvas, MouseEventArgs> eMouseDown, eMouseUp, eMouseMove, eCanvasMouseMove;
		public event Action<MainCanvas> eCanvasImageSizeChanged;      // 画布改变大小（影响画布上所有元素的大小和位置）
		public event Action<MainCanvas, Vector2> eCanvasSizeChanged;
		public List<IAnnotationElement> annotation_elements = new List<IAnnotationElement>();        // 用于存放所有 annotation 的地方

		public MainCanvas () {
			InitializeComponent();
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
		public Vector2 RealPosition (Vector2 cpoint) {
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
			Vector2 diff = new Vector2(cpoint.X, cpoint.Y) - ( tl + offset );

			// 4. 考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 / 0.5 （即×2）
			float imgsc = image_scale == 0f ? 1f : image_scale;    // 防止 inf
			diff = diff / imgsc;

			return diff;
		}
		/// <summary>
		/// 图片上的点对应的画布上的点
		/// </summary>
		public Vector2 CanvasPosition (Vector2 rpoint) {
			Vector2 canvas_sz = GetCanvasSize();
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 1. 逆运算，考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 * 0.5
			float imgsc = ( image_scale == 0f ? 1f : image_scale );    // 防止 inf
			var cpoint = rpoint * imgsc;

			// 2. 考虑 scroll offset 的影响
			Vector2 offset = new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset);

			// 3.  计算点与左上角点的差
			cpoint = new Vector2(cpoint.X, cpoint.Y) + ( tl + offset );

			return cpoint;
		}
		/// <summary>
		/// 图片上的点对应的画布上的点
		/// </summary>
		public Vector2[] CanvasPosition (Vector2[] rpoints) {
			Vector2 canvas_sz = GetCanvasSize();
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 1. 逆运算，考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 * 0.5
			float imgsc = ( image_scale == 0f ? 1f : image_scale );

			// 防止 inf
			// 2. 考虑 scroll offset 的影响
			Vector2 offset = new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset);

			Vector2[] cpoints = new Vector2[rpoints.Length];
			Parallel.For(0, rpoints.Length, (index) => {
				var cpoint = rpoints[index] * imgsc;
				cpoint = new Vector2(cpoint.X, cpoint.Y) + ( tl + offset );
				cpoints[index] = cpoint;
			});
			return cpoints;
		}
		public void ClearCanvas () {
			foreach ( var elem in annotation_elements ) {
				elem.Delete(this);
			}
			annotation_elements.Clear();
		}
		/// <summary>
		/// 将 ImageData 中的所有 annotation 放置到画布上
		/// </summary>
		public void LoadAnnotations (ImageData data) {
			foreach ( var anno in data.GetAnnoData() ) {
				var elem = anno.CreateAnnotationElement(this);
				AddAnnoElements(elem);
			}
		}
		/// <summary>
		/// 将图片加载到画布上
		/// </summary>
		public async Task LoadImage (ImageData data) {
			BitmapImage image = await ImageUtils.ReadImageFromFileAsync(data.path);

			// 获取图像尺寸
			double image_height = image.PixelHeight;
			double image_width = image.PixelWidth;
			image_size = new Vector2((float) image_width, (float) image_height);

			// 设置当前的图像源
			canvas_image.Source = image;

			// 宽 20, 画布 10, scale = 2, scale = 1/2 = 50%
			var hscale = image_height / ( canvas.ActualHeight - margin );
			var wscale = image_width / ( canvas.ActualWidth - margin );

			double target_height, target_width;
			if ( hscale <= 1 && wscale <= 1 ) {
				// 如果高和宽都小于画布尺寸，则不做任何处理
				canvas_image.Width = image_width;
				canvas_image.Height = image_height;
				image_scale = 1f;
			} else {
				// 如果有一边超出画布尺寸，则做缩放处理
				var scale = hscale > wscale ? hscale : wscale;
				canvas_image.Width = image_width / scale;
				canvas_image.Height = image_height / scale;
				image_scale = 1f / (float) scale;
			}

			// 设置画布的尺寸
			target_height = annotation_canvas.ActualHeight;
			target_width = annotation_canvas.ActualWidth;
			ChangeCanvasSize(target_width, target_height);
			canvas_size_before = canvas_size;

			// 重置图像的 offset
			image_offset = GetOffsetFromScroll();

			// 加载完了图片以后就可以开始 annotate
			can_annotate = true;
			eImageLoaded?.Invoke(this, image);
		}
		public void SetImageScale (float scale = 0.5f) {
			double image_height = image_size.Y * scale, image_width = image_size.X * scale;
			canvas_image.Width = image_width;
			canvas_image.Height = image_height;

			var canvas_width = annotation_canvas.ActualWidth;
			var canvas_height = annotation_canvas.ActualHeight;

			double new_width = double.NaN, new_height = double.NaN;
			if ( image_width + margin > canvas_width ) {
				new_width = image_width + margin;
			}
			if ( image_height + margin > canvas_height ) {
				new_height = image_height + margin;
			}
			ChangeCanvasSize(new_width, new_height);
		}
		public void AddAnnoElements (IAnnotationElement element) {
			if ( can_annotate ) {          // 只有在画布上有内容时才会加入 element
				if ( element != null ) {
					annotation_elements.Add(element);
					eAnnotationElementAdded?.Invoke(this, element);
				}
			}
		}
		public void AddBulkAnnoElements (IAnnotationElement[] elements) {
			if ( can_annotate ) {          // 只有在画布上有内容时才会加入 element
				if ( elements != null ) {
					foreach ( var element in elements ) {
						annotation_elements.Add(element);
						eAnnotationElementAdded?.Invoke(this, element);
					}
				}
			}
		}
		public void ModifiedAnnoElements (IAnnotationElement element) {
			if ( element != null && annotation_elements.Contains(element) ) {
				eAnnotationElementModified?.Invoke(this, element);
			}
		}
		public void RemoveAnnoElements (IAnnotationElement element) {
			if ( element != null && annotation_elements.Contains(element) ) {
				annotation_elements.Remove(element);
				eAnnotationElementRemoved?.Invoke(this, element);
				element.Delete(this);
			}
		}
		public void ChangeCanvasSize (double width, double height) {
			annotation_canvas.Width = width;
			annotation_canvas.Height = height;

			foreach ( var elem in annotation_elements ) {
				// 重新绘制元素
				var cpoints = CanvasPosition(elem.data.rpoints);
				elem.Draw(this.annotation_canvas, cpoints);
			}
			eCanvasImageSizeChanged?.Invoke(this);
		}
		/// <summary>
		/// 从 scrollbar 中获取当前的 offset
		/// 如果 offset = 0, 意味着 scrollbar 没有转动
		/// </summary>
		private Vector2 GetOffsetFromScroll () {
			return new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset);
		}

		private void AnnotationCanvasSizeChanged (object sender, SizeChangedEventArgs e) {
			var new_size = canvas_size;
			foreach ( var elem in annotation_elements ) {
				elem.Shift(this.annotation_canvas, ( new_size - canvas_size_before ) / 2f);
			}
			canvas_size_before = new_size;
			eCanvasSizeChanged?.Invoke(this, new_size);
		}
		private void scroll_ScrollChanged (object sender, ScrollChangedEventArgs e) {
			// 重新设定图片的 offset
			Vector2 p = GetOffsetFromScroll();
			image_offset = p;
		}

		#region MouseEvents
		private void CanvasMouseDown (object sender, MouseButtonEventArgs e) {
			button_state = MouseButtonState.Pressed;
			eMouseDown?.Invoke(this, e);
		}
		private void CanvasMouseMove (object sender, MouseEventArgs e) {
			if ( button_state == MouseButtonState.Pressed ) {
				eCanvasMouseMove?.Invoke(this, e);
			}
			eMouseMove?.Invoke(this, e);
		}
		private void CanvasMouseUp (object sender, MouseButtonEventArgs e) {
			if ( button_state == MouseButtonState.Pressed ) {
				eMouseUp?.Invoke(this, e);
			}
			button_state = MouseButtonState.Released;
		}
		private void CanvasMouseLeave (object sender, MouseEventArgs e) {
			if ( button_state == MouseButtonState.Pressed ) {
				eMouseUp?.Invoke(this, e);
			}
			button_state = MouseButtonState.Released;
		}
		#endregion
	}
}