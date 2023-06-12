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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLabel.Custom_Control.Small_Tools.Option_Box {
	[ContentProperty("ExpanderContent")]
	public partial class OptionCollapse : UserControl {
		public OptionCollapse () {
			InitializeComponent();
		}
		public object ExpanderContent {
			get { return expander.Content; }
			set { expander.Content = value; }
		}

		public object HeaderText {
			get { return GetValue(HeaderTextProperty); }
			set { SetValue(HeaderTextProperty, value); }
		}
		public static readonly DependencyProperty HeaderTextProperty =
		    DependencyProperty.Register("HeaderText",
			    propertyType: typeof(string), ownerType: typeof(OptionCollapse),
			    typeMetadata: new PropertyMetadata("header"));

	}
}
