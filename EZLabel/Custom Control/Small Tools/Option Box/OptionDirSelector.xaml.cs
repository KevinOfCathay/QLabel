using Microsoft.WindowsAPICodePack.Dialogs;
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

namespace QLabel.Custom_Control.Small_Tools.Option_Box {
	/// <summary>
	/// Interaction logic for OptionDirSelector.xaml
	/// </summary>
	public partial class OptionDirSelector : UserControl {
		public string directory { get => this.path.Text; }
		public event Action<string> eDirectorySelected;
		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionDirSelector),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionDirSelector),
			    typeMetadata: new PropertyMetadata("sub text"));

		public OptionDirSelector () {
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
		}
	}
}
