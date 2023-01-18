using System.Windows;
using System.Windows.Input;
using QLabel.Custom_Control.Image_View;
using System.Diagnostics;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using QLabel.Scripts;

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
				item.eSwitchImage += (i) => {
					main_canvas.LoadImage(i.path);          // 点击 List 中的 image 图像来加载图片
				};
			};
			main_canvas.eImageLoaded += (MainCanvas canvas, ImageFileData data) => {
				misc_panel.image_properties_panel.SetUI(data);
			};
		}
		public void InitComponents () {
			this.toolbar.Init(this.main_canvas);
			this.main_menu.Init(this);
		}

		private void Window_KeyDown (object sender, KeyEventArgs e) {
			if ( e.Key == Key.Z ) {
				Debug.WriteLine("触发了按键Z");
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
