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
	/// Interaction logic for RoundedLabel.xaml
	/// </summary>
	public partial class RoundedLabel : UserControl {

		public RoundedLabel () {
			InitializeComponent();
		}
		public RoundedLabel (string text, double fontsize = 9.5) {
			InitializeComponent();
			this.label.Content = text;
			this.label.FontSize = fontsize;
		}
	}
}
