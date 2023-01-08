using QLabel.Windows.Main_Canvas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLabel {
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : UserControl {
		MainWindow main;

		public MainMenu () {
			InitializeComponent();
		}
		public void Init (MainWindow mw) {
			this.main = mw;
		}
		private void New_Click (object sender, RoutedEventArgs e) {
			if ( main != null ) {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
				if ( openFileDialog.ShowDialog() == true ) {
					string file = openFileDialog.FileName;
					try {
						var dir = System.IO.Path.GetDirectoryName(file);
						// Load all images from dir
						if ( dir != null ) {
							var data = main.ilw.LoadImagesFromDir(dir);
							main.ilw.SetListUI(data);

							// Load selected file
							var curdata = data.Find((x) => { return x.path == file; });
							if ( curdata != null ) {
								main.main_canvas.LoadImage(curdata);
							}
						}
					} catch ( Exception ) {
					}
				}
			}
		}
		private void Undo_Click (object sender, RoutedEventArgs e) {
			ActionManager.PopAction();
		}
	}
}
