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
		public event Action<object, RoutedEventArgs> eConfirmClick, eCancelClick;
		public ConfirmCancel () {
			InitializeComponent();
		}
		private void Confirm_Click (object sender, RoutedEventArgs e) {
			eConfirmClick?.Invoke(sender, e);
		}
		private void Cancel_Click (object sender, RoutedEventArgs e) {
			eCancelClick?.Invoke(sender, e);
		}
	}
}
