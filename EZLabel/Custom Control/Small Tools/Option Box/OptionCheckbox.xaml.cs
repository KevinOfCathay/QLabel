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
	/// Interaction logic for OptionCheckbox.xaml
	/// </summary>
	public partial class OptionCheckbox : UserControl {
		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionCheckbox),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionCheckbox),
			    typeMetadata: new PropertyMetadata("sub text"));


		public bool? IsChecked { get { return checkbox.IsChecked; } set { checkbox.IsChecked = value; } }
		public new bool IsEnabled { get { return checkbox.IsEnabled; } set { checkbox.IsEnabled = value; } }

		public OptionCheckbox () {
			InitializeComponent();
		}
	}
}
