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

namespace QLabel.Windows.Export_Window {
	/// <summary>
	/// Interaction logic for ExportWindow.xaml
	/// </summary>
	public partial class ExportWindow : Window {
		public ExportWindow () {
			InitializeComponent();
		}
		private void ConfirmButton_Click (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CancelButton_Click (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}
	}
}
