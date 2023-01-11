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

namespace QLabel.Custom_Control.Small_Tools {
	/// <summary>
	/// Interaction logic for RectWithLabel.xaml
	/// </summary>
	public partial class RectWithLabel : UserControl {
		public RectWithLabel () {
			InitializeComponent();
		}
		public void SetUI (string text, Color color) {
			rect.Background = new SolidColorBrush(color);
			label.Content = text;
		}
	}
}
