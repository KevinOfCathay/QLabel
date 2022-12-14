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

namespace EZLabel.Windows.Main_Canvas {
	/// <summary>
	/// Interaction logic for ImageQuickInfoPanel.xaml
	/// </summary>
	public partial class ImageQuickInfoPanel : UserControl {
		public ImageQuickInfoPanel () {
			InitializeComponent();
		}

		public void SetZoomText (double zoom) {
			zoom_level.Text = ( zoom * 100.0 ).ToString("N3") + "%";
		}
	}
}
