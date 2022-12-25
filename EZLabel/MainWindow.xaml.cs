using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EZLabel.Custom_Control.Image_View;
using System.IO;
using EZLabel.Windows.Main_Canvas;
using System.Diagnostics;
using EZLabel.Scripts.AnnotationToolManager;

namespace QLabel {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			App.main = this;

			InitializeComponent();
			InitEvents();
			InitComponents();
		}

		public void InitEvents () {
			ilw.eImageListUICreated += (ImageListWindow window, ImageListItem item) => {
				item.eSwitchImage += (i) => {
					main_canvas.LoadImage(i.data);          // 点击 List 中的 image 图像来加载图片
				};
			};
		}
		public void InitComponents () {
			this.toolbar.Init(this.main_canvas);
			this.main_menu.Init(this);
		}
	}
}
