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
	public partial class OptionListBoxVertical : UserControl {
		public string MainText {
			get { return (string) GetValue(maintext_property); }
			set { SetValue(maintext_property, value); }
		}
		public static readonly DependencyProperty maintext_property = DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionListBoxVertical),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionListBoxVertical),
			    typeMetadata: new PropertyMetadata("sub text"));

		public ItemCollection Items { get { return listbox.Items; } }

		public OptionListBoxVertical () {
			InitializeComponent();
		}
	}
}
