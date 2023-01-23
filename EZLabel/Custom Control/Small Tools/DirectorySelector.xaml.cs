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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace QLabel.Custom_Control.Small_Tools {
	public partial class DirectorySelector : UserControl {
		public event Action<string> eDirectorySelected;
		public event Action eDialogClosed;
		public DirectorySelector () {
			InitializeComponent();
		}
		private void SelectDirectoryClick (object sender, RoutedEventArgs e) {
			using ( var dialog = new CommonOpenFileDialog() ) {
				dialog.IsFolderPicker = true;
				dialog.EnsureFileExists = true;
				dialog.EnsurePathExists = true;

				if ( dialog.ShowDialog() == CommonFileDialogResult.Ok ) {
					var path = dialog.FileName;
					this.path.Text = path;

					eDirectorySelected?.Invoke(path);
				}
			}
			eDialogClosed?.Invoke();
		}
	}
}
