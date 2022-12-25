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
using QLabel.Scripts.Projects;

namespace EZLabel.Custom_Control.Image_View
{
    /// <summary>
    /// Interaction logic for ImagePropertiesWindow.xaml
    /// </summary>
    public partial class ImagePropertiesWindow : UserControl {
		public ImagePropertiesWindow () {
			InitializeComponent();
		}

		public void SetImageUIProperties (ImageFileData data) {
			file_path.Text = data.path;
			dimension.Text = data.width.ToString() + " × " + data.height.ToString();
			file_size.Text = data.size.ToString();
		}
	}
}
