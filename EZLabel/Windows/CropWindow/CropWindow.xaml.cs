using QLabel.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Windows.CropWindow {
	/// <summary>
	/// Interaction logic for CropWindow.xaml
	/// </summary>
	public partial class CropWindow : Window {
		public CropWindow () {
			InitializeComponent();
		}
		public void SetUI () {
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
