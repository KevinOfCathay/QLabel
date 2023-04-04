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
	/// Interaction logic for ConfirmCancel.xaml
	/// </summary>
	public partial class ConfirmCancel : UserControl {
		public event RoutedEventHandler ConfirmClick {
			add { this.confirm.Click += value; }
			remove { this.confirm.Click -= value; }
		}
		public event RoutedEventHandler CancelClick {
			add { this.cancel.Click += value; }
			remove { this.cancel.Click -= value; }
		}
		public ConfirmCancel () {
			InitializeComponent();
		}
	}
}
