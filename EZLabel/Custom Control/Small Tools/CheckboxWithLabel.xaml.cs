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
	/// Interaction logic for CheckboxWithLabel.xaml
	/// </summary>
	public partial class CheckboxWithLabel : UserControl {
		public event RoutedEventHandler eChecked {
			add { checkbox.Checked += value; }
			remove { checkbox.Checked -= value; }
		}
		public event RoutedEventHandler eUnchecked {
			add { checkbox.Unchecked += value; }
			remove { checkbox.Unchecked -= value; }
		}
		public object Text {
			get { return GetValue(textproperty); }
			set { SetValue(textproperty, value); }
		}
		public static readonly DependencyProperty textproperty =
		    DependencyProperty.Register("Text",
			    propertyType: typeof(object),
			    ownerType: typeof(CheckboxWithLabel),
			    typeMetadata: new PropertyMetadata("text"));

		public double BoxSize {
			get { return (double) GetValue(boxsize_property); }
			set { SetValue(boxsize_property, value); }
		}
		public static readonly DependencyProperty boxsize_property =
		    DependencyProperty.Register("BoxSize",
			    propertyType: typeof(double),
			    ownerType: typeof(CheckboxWithLabel),
			    typeMetadata: new PropertyMetadata(16.0));

		public CheckboxWithLabel () {
			InitializeComponent();
		}
		public CheckboxWithLabel (string text, bool is_checked,
			bool enable_checkbox = true, RoutedEventHandler check_event = null, RoutedEventHandler uncheck_event = null
			) : base() {
			InitializeComponent();
			if ( check_event != null ) { eChecked += check_event; }
			if ( uncheck_event != null ) { eUnchecked += uncheck_event; }

			label.Content = text;
			this.checkbox.IsChecked = is_checked;
			if ( !enable_checkbox ) { checkbox.IsEnabled = false; }
		}
		public void SetLabel (string text) {
			label.Content = text;
		}
		public void Check () {
			this.checkbox.IsChecked = true;
		}
		public void Uncheck () {
			this.checkbox.IsChecked = false;
		}
	}
}
