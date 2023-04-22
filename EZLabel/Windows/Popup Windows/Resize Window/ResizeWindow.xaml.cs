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
using System.Windows.Shapes;

namespace QLabel.Windows.Popup_Windows.Resize_Window {
	/// <summary>
	/// Interaction logic for ResizeWindow.xaml
	/// </summary>
	public partial class ResizeWindow : Window {
		public ResizeWindow () {
			InitializeComponent();
			scale.InitializeCombobox(new string[] { "Pixel", "Percentage" });
		}
		private void scale_SelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if ( scale.SelectedItem != null ) {

			}
		}
	}
}
