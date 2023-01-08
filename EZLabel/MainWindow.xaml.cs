using System.Windows;
using System.Windows.Input;
using QLabel.Custom_Control.Image_View;
using System.Diagnostics;

namespace QLabel {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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
	}
}
