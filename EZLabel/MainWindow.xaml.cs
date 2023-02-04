using QLabel.Custom_Control.Image_View;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QLabel {
	public partial class MainWindow : Window {

		public MainWindow () {
			App.main = this;

			InitializeComponent();
			RegisterEvents();
			InitComponents();
		}

		public void RegisterEvents () {
			ilw.eImageListUICreated += (ImageListWindow window, ImageListItem item) => {
				// 切换图片时的事件
				item.eSwitchImage += async (i) => {
					// 判断当前点击的文件是否是已经被打开的文件
					if ( i.data != ProjectManager.cur_datafile ) {
						// 清空之前图片的注释
						annolistpanel.annolist.ClearList();
						// 清除上一张图片在画布上的所有元素
						main_canvas.ClearCanvas();
						// 点击 List 中的 image 图像来加载图片
						// 这个需要 await，所以 img_scale 可以正确被设置
						Task loadimg = main_canvas.LoadImage(item.data);
						await loadimg;
						main_canvas.LoadAnnotations(item.data);
						// 设置图像属性UI
						misc_panel.image_properties_panel.SetUI(item.data);
						// 将当前的文件设置为打开的图片文件
						ProjectManager.cur_datafile = item.data;
					}
				};
			};

			var canvas = main_canvas;
			if ( canvas != null ) {
				canvas.eAnnotationElementAdded += (MainCanvas mc, IAnnotationElement iae) => {
					// 当有注释被加入时，更新注释列表以及注释树
					annolistpanel.annolist.AddItem(iae);
					annolistpanel.annotree.AddAnnoData(iae);
				};
				canvas.eAnnotationElementModified += (MainCanvas mc, IAnnotationElement iae) => {
					// 当有注释被移除时，更新注释列表以及注释树
					annolistpanel.annolist.RefreshItem(iae);
				};
				canvas.eAnnotationElementRemoved += (MainCanvas mc, IAnnotationElement iae) => {
					// 当有注释被移除时，更新注释列表以及注释树
					annolistpanel.annolist.RemoveItem(iae);
					annolistpanel.annotree.RemoveAnnoData(iae);
				};
			}
		}
		public void InitComponents () {
			this.toolbar.Init(this.main_canvas);
			this.main_menu.Init(this);
		}

		private void Window_KeyDown (object sender, KeyEventArgs e) {
			if ( e.Key == Key.Z ) {
				Debug.WriteLine("触发了按键Z");
			} else if ( e.Key >= Key.D1 && e.Key <= Key.D8 ) {
				toolbar.SetCurrentTool(e.Key - Key.D1);
			}
		}
		public void LockWindow () {
			this.IsEnabled = false;
		}
		public void UnlockWindow () {
			this.IsEnabled = true;
		}
	}
}
