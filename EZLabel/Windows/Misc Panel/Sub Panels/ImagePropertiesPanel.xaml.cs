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

namespace QLabel.Windows.Misc_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for ImagePropertiesWindow.xaml
	/// </summary>
	public partial class ImagePropertiesPanel : UserControl {
		public ImagePropertiesPanel () {
			InitializeComponent();
		}
		/// <summary>
		/// 设置 UI 上的 text
		/// </summary>
		public void SetUI (ImageData data) {
			file_path.Text = data.path;
			file_name.Text = data.filename;
			dimension.Text = data.width.ToString() + " × " + data.height.ToString();
			file_size.Text = data.size.ToString();
		}
	}
}
