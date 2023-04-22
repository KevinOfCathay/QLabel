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
		public Vector2 canvas_actual_size { get { return new Vector2((float) this.annotation_canvas.ActualWidth, (float) this.annotation_canvas.ActualHeight); } }
		public Vector2 canvas_size { get { return new Vector2((float) this.annotation_canvas.Width, (float) this.annotation_canvas.Height); } }
		public float image_scale { get; private set; } = 0f;          // image scale level
		public Vector2 image_size { get; private set; } = new Vector2(0, 0);      // 图片大小，用于计算annotation的位置，在未加载时图片大小为 0
		public Vector2 scroll_offset { get { return new Vector2((float) scroll.HorizontalOffset, (float) scroll.VerticalOffset); } }      // 图片在画布上的偏移量，用于计算annotation的位置，在未加载时图片，或者图片居中时大小为 0		
		public MouseButtonState button_state = MouseButtonState.Released;
		public event Action<MainCanvas, BitmapImage> eImageLoaded;
		public event Action<MainCanvas, IAnnotationElement> eAnnotationElementAdded, eAnnotationElementModified, eAnnotationElementRemoved;
		public event Action<MainCanvas, MouseEventArgs> eMouseDown, eMouseUp, eMouseMove, eCanvasMouseMove;
		public event Action<MainCanvas> eCanvasImageSizeChanged;      // 画布改变大小（影响画布上所有元素的大小和位置）
		public event Action<MainCanvas, Vector2> eCanvasSizeChanged;
		public List<IAnnotationElement> annotation_elements = new List<IAnnotationElement>();        // 用于存放所有 annotation 的地方

		public MainCanvas () {
			InitializeComponent();

			// 加入一个 size changed event
			// 只有在 size changed 之后，才能获得 re-rendered ActualWidth 和 ActualHeight
			annotation_canvas.SizeChanged += (object sender, SizeChangedEventArgs e) => {
				foreach ( var elem in annotation_elements ) {
					// 重新绘制元素
					var cpoints = CanvasPosition(elem.data.rpoints);
					elem.Draw(this, cpoints);
				}
				eCanvasImageSizeChanged?.Invoke(this);
			};
		}
		public Vector2 GetImageSize () {
			return image_size * image_scale;
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
			Vector2 canvas_sz = canvas_actual_size;
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 2. 考虑 scroll offset 的影响
			// 例如：当 offset = (10, 5) 时，坐标 （2, 2）所对应的实际点为 （10+2, 5+2）
			// 右下角点的坐标 + max offset = 图像尺寸
			Vector2 offset = scroll_offset;

			// 3.  计算点与左上角点的差
			Vector2 diff = new Vector2(cpoint.X, cpoint.Y) - tl + offset;

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
			Vector2 canvas_sz = canvas_actual_size;
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 1. 逆运算，考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 * 0.5
			float imgsc = ( image_scale == 0f ? 1f : image_scale );    // 防止 inf
			var cpoint = rpoint * imgsc;

			// 2. 考虑 scroll offset 的影响
			Vector2 offset = scroll_offset;

			// 3.  计算点与左上角点的差
			cpoint = cpoint + tl - offset;

			return cpoint;
		}
		/// <summary>
		/// 图片上的点对应的画布上的点
		/// </summary>
		public Vector2[] CanvasPosition (Vector2[] rpoints) {
			if ( rpoints.Length == 0 ) { return new Vector2[0]; }
			Vector2 canvas_sz = canvas_actual_size;
			Vector2 img_sz = GetImageSize();
			Vector2 tl = ( canvas_sz - img_sz ) / new Vector2(2f, 2f);

			// 1. 逆运算，考虑图片尺寸缩放的影响
			// 如果图片缩放比例为 0.5, 则所有坐标都需要 * 0.5
			float imgsc = ( image_scale == 0f ? 1f : image_scale );          // 防止 inf

			// 2. 考虑 scroll offset 的影响
			Vector2 offset = scroll_offset;

			Span<Vector2> cpoints = stackalloc Vector2[rpoints.Length];
			for ( int index = 0; index < rpoints.Length; index += 1 ) {
				var cpoint = rpoints[index] * imgsc;
				cpoint = cpoint + ( tl - offset );
				cpoints[index] = cpoint;
			}
			return cpoints.ToArray();
		}
		public void ClearCanvas () {
			foreach ( var elem in annotation_elements ) {
				annotation_canvas.Children.Remove(elem.ui_element);
			}
			annotation_elements.Clear();
		}
		/// <summary>
		/// 将 ImageData 中的所有 annotation 放置到画布上
		/// </summary>
		public void LoadAnnotations (ImageData data) {
			foreach ( var anno in data.annodata ) {
				var elem = anno.CreateAnnotationElement(this);
				AddAnnoElements(elem, add_ui_element: true);
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
			canvas_size_before = canvas_actual_size;

			// 加载完了图片以后就可以开始 annotate
			can_annotate = true;
			eImageLoaded?.Invoke(this, image);
		}
		public void SetImageScale (float scale) {
			if ( scale <= 0f ) { return; }

			double image_height = image_size.Y * scale, image_width = image_size.X * scale;
			canvas_image.Width = image_width;
			canvas_image.Height = image_height;

			double canvas_width = annotation_canvas.ActualWidth, canvas_height = annotation_canvas.ActualHeight;
			double new_width = double.NaN, new_height = double.NaN;
			if ( image_width + margin > canvas_width ) {
				new_width = image_width + margin;
			}
			if ( image_height + margin > canvas_height ) {
				new_height = image_height + margin;
			}
			image_scale = scale;
			ChangeCanvasSize(new_width, new_height);
		}
		public void AddAnnoElements (IAnnotationElement element, bool add_ui_element = false) {
			if ( can_annotate && element != null ) {
				annotation_elements.Add(element);
				if ( add_ui_element && element.ui_element != null ) {
					annotation_canvas.Children.Add(element.ui_element);
				}
				eAnnotationElementAdded?.Invoke(this, element);
			}
		}
		public void AddBulkAnnoElements (IEnumerable<IAnnotationElement> elements, bool add_ui_element = false) {
			if ( can_annotate && elements != null ) {
				foreach ( var element in elements ) {
					annotation_elements.Add(element);
					if ( add_ui_element && element.ui_element != null ) {
						annotation_canvas.Children.Add(element.ui_element);
					}
					eAnnotationElementAdded?.Invoke(this, element);
				}
			}
		}
		public IAnnotationElement GetElementByID (Guid guid) {
			return annotation_elements.Find((x) => { return x.data != null && x.data.guid == guid; });
		}
		public void ModifiedAnnoElements (IAnnotationElement element) {
			if ( element != null && annotation_elements.Contains(element) ) {
				eAnnotationElementModified?.Invoke(this, element);
			}
		}
		public void RemoveAnnoElements (IAnnotationElement element) {
			if ( element != null && annotation_elements.Contains(element) ) {
				annotation_elements.Remove(element);
				canvas.Children.Remove(element.ui_element);         // 从画布上删除 UI
				eAnnotationElementRemoved?.Invoke(this, element);
				element.PostDelete(this);         // Clean-up
			}
		}
		public void ChangeCanvasSize (double width, double height) {
			annotation_canvas.Width = width;
			annotation_canvas.Height = height;
		}

		private void AnnotationCanvasSizeChanged (object sender, SizeChangedEventArgs e) {
			var new_size = canvas_actual_size;
			foreach ( var elem in annotation_elements ) {
				elem.Shift(this.annotation_canvas, ( new_size - canvas_size_before ) / 2f);
			}
			canvas_size_before = new_size;
			eCanvasSizeChanged?.Invoke(this, new_size);
		}

		private void MouseWheelChanged (object sender, MouseWheelEventArgs e) {
			if ( Keyboard.Modifiers == ModifierKeys.Alt ) {
				if ( e.Delta > 0 ) {
					SetImageScale(image_scale + 0.2f);
				} else if ( e.Delta < 0 ) {
					SetImageScale(image_scale - 0.2f);
				}
			}
		}

		private void scroll_ScrollChanged (object sender, ScrollChangedEventArgs e) { }

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
		#endregion MouseEvents
	}
}