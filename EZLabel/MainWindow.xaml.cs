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

namespace EZLabel {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			InitializeComponent();
			InitEvents();
			RegisterShortcut();
		}

		private void MenuItem_NewFile_Click (object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
			if ( openFileDialog.ShowDialog() == true ) {
				string file = openFileDialog.FileName;
				try {
					var dir = System.IO.Path.GetDirectoryName(file);
					// Load all images from dir
					if ( dir != null ) {
						var data = ilw.LoadImagesFromDir(dir);
						ilw.SetListUI(data);

						// Load selected file
						var curdata = data.Find((x) => { return x.path == file; });
						if ( curdata != null ) {
							main_canvas.LoadImage(curdata);
						}
					}
				} catch ( Exception ex ) {
				}
			}
		}

		public void RegisterShortcut () {
			try {
				RoutedCommand firstSettings = new RoutedCommand();
				firstSettings.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
				CommandBindings.Add(new CommandBinding(firstSettings, MenuItem_NewFile_Click));
			} catch ( Exception ex ) {
			}
		}
		public void InitEvents () {
			ilw.eImageListUICreated += (ImageListWindow window, ImageListItem item) => {
				item.eImageButtonClick += (i) => {
					main_canvas.LoadImage(i.data);          // Click button to load image onto canvas
				};
			};
			main_canvas.eImageLoaded += (MainCanvas m, ImageFileData data) => {
				ipw.SetImageUIProperties(data);
			};
		}

		private void TestFunction (object sender, RoutedEventArgs e) {
			Debug.WriteLine("测试用函数被触发");
			main_canvas.tool.DrawRectangle(main_canvas, new System.Windows.Point(150, 50), new System.Windows.Point(200, 250));
		}
	}
}
