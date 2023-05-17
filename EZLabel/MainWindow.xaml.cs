using QLabel.Custom_Control.Image_View;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using QLabel.Actions;
using static QLabel.Windows.Toolbar_Window.AnnotationToolBar;
using System.Xml.Linq;

namespace QLabel {
	public partial class MainWindow : Window {
		public IAnnotationElement selected_element = null;

		public MainWindow () {
			App.main = this;
			InitializeComponent();
			SetTitle();
			InitComponents();
		}
		private void RegisterEvents (object? sender, EventArgs e) {
			// 切换图片时的事件
			ilw.eSwitchImage += async (ImageListItem old, ImageListItem nw) => {
				// 判断当前点击的文件是否是已经被打开的文件
				if ( nw.data != ProjectManager.cur_datafile ) {
					// 点击 List 中的 image 图像来加载图片
					// 需要 await，所以 img_scale 可以正确被设置
					Task loadimg = main_canvas.LoadImage(nw.data);
					// 将当前的文件设置为打开的图片文件
					ProjectManager.cur_datafile = nw.data;
					// 清空之前图片的注释
					annolistpanel.annolist.ClearList();
					// 清除上一张图片在画布上的所有元素
					main_canvas.ClearCanvas();
					await loadimg;
					main_canvas.LoadAnnotations(nw.data);
					// 设置图像属性UI
					misc_panel.image_properties_panel.SetUI(nw.data);
				};
			};
			var canvas = main_canvas;
			if ( canvas != null ) {
				canvas.eAnnotationElementAdded += (MainCanvas _, IAnnotationElement iae) => {
					// 当有注释被加入时，更新注释列表以及注释树
					annolistpanel.annolist.AddItem(iae);
					annolistpanel.annotree.AddAnnoData(iae);

					// elem 被选择时的事件
					iae.eSelected += (elem) => { selected_element = elem; };
					iae.eUnselected += (elem) => { selected_element = null; };
				};
				canvas.eAnnotationElementModified += (MainCanvas _, IAnnotationElement iae) => {
					// 当有注释被移除时，更新注释列表以及注释树
					annolistpanel.annolist.RefreshItem(iae);
				};
				canvas.eAnnotationElementRemoved += (MainCanvas _, IAnnotationElement iae) => {
					// 当有注释被移除时，更新注释列表以及注释树
					annolistpanel.annolist.RemoveItem(iae);
					annolistpanel.annotree.RemoveAnnoData(iae);

					// 如果这个 elem 被选择，则将其移除
					if ( selected_element == iae ) { selected_element = null; }
				};
				canvas.eMouseMove += (MainCanvas mc, MouseEventArgs e) => {
					// 设置 Quick Info Panel
					Point pos = e.GetPosition(mc);
					this.image_quick_info_panel.SetMousePositionText(pos);
					this.image_quick_info_panel.SetRelativePositionText(mc.RealPosition(new Vector2((float) pos.X, (float) pos.Y)));
				};
				canvas.eImageLoaded += (MainCanvas mc, BitmapImage image) => {
					// 设置底层信息栏的文字
					this.image_quick_info_panel.SetZoomText(mc.image_scale);
					this.image_quick_info_panel.SetImageSize(image.PixelWidth, image.PixelHeight);
				};
				annolistpanel.annolist.eItemSelected += (IAnnotationElement elem) => {
					misc_panel.anno_properties_panel.SetUI(elem);
				};
			}
		}
		public void InitComponents () {
			this.toolbar.Init(this.main_canvas);
			this.main_menu.Init(this);
			this.image_quick_info_panel.canvas = this.main_canvas;
		}
		/// <summary>
		/// 根据 Image data 设置 UI 等
		/// </summary>
		public async Task SetImageData (ImageData[] datas) {
			// 设置底层的的 UI 缩略图等
			var set_list_ui_task = ilw.SetListUI(datas);

			await set_list_ui_task;
		}
		private void SetTitle () {
			try {
				Title = string.Join(" ", "Qlabel", "Ver", Assembly.GetExecutingAssembly().GetName().Version.ToString());
			} catch {
				Title = "Qlabel";
			}
		}
		private void Window_KeyDown (object sender, KeyEventArgs e) {
			if ( e.Key == Key.Z ) {
				if ( e.KeyboardDevice.Modifiers == ModifierKeys.Control ) {
					ActionManager.PopAction();         // 撤销
				}
			} else if ( e.Key >= Key.D1 && e.Key <= Key.D8 ) {
				toolbar.SetCurrentTool((Tool) ( e.Key - Key.D1 ));          // 切换工具
			} else if ( e.Key == Key.P ) {
				if ( e.KeyboardDevice.Modifiers == ModifierKeys.Control ) {
					ToPolygon();        // 转换为多边形
				}
			} else if ( e.Key == Key.D ) {
				if ( e.KeyboardDevice.Modifiers == ModifierKeys.Control ) {
					Densify();          // 增加点密度
				}
			} else if ( e.Key == Key.Delete ) {
				DeleteSelectedElement();
			}
		}
		public void DeleteSelectedElement () {
			if ( selected_element != null ) {
				selected_element.PostDelete(main_canvas);
			}
		}
		public void LockWindow () {
			this.IsEnabled = false;
		}
		public void UnlockWindow () {
			this.IsEnabled = true;
		}
		public void ToPolygon () {
			if ( toolbar.cur_tool == Tool.Mouse && selected_element != null ) {
				selected_element = selected_element.ToPolygon(main_canvas);          // To Polygon
			}
		}
		public void Densify () {
			if ( toolbar.cur_tool == Tool.Mouse && selected_element != null ) {
				selected_element.Densify(main_canvas);          // Densify
			}
		}
	}
}
