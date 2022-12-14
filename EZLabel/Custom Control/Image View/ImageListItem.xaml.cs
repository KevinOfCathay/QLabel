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

namespace EZLabel.Custom_Control.Image_View {
	/// <summary>
	/// Interaction logic for ImageListItem.xaml
	/// </summary>
	public partial class ImageListItem : UserControl {
		public Action<ImageListItem> eImageButtonClick;
		public ImageFileData data;
		public ImageListItem () {
			InitializeComponent();
		}

		private void ImageButtonClick (object sender, RoutedEventArgs e) {
			eImageButtonClick?.Invoke(this);
		}
	}
}
