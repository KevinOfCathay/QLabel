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

namespace QLabel.Windows.Popup_Windows {
	/// <summary>
	/// Interaction logic for FindWindow.xaml
	/// </summary>
	public partial class FindWindow : WindowBase {
		public FindWindow () : base("Find...") {
			InitializeComponent();
		}
		public override void Initialization () {
		}
	}
}
