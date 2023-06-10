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
	/// Interaction logic for OptionCombobox.xaml
	/// </summary>
	public partial class OptionCombobox : UserControl {
		public event SelectionChangedEventHandler SelectionChanged {
			add { this.combobox.SelectionChanged += value; }
			remove { this.combobox.SelectionChanged -= value; }
		}
		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionCombobox),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionCombobox),
			    typeMetadata: new PropertyMetadata("sub text"));
		public int SelectedIndex { get => combobox.SelectedIndex; }
		public object SelectedItem { get => combobox.SelectedItem; }

		public OptionCombobox () {
			InitializeComponent();
		}
		public OptionCombobox (IEnumerable<string> options) {
			InitializeComponent();
			SetCombobox(options);
		}
		public void SetCombobox (IEnumerable<string> options) {
			bool first_item = true;
			foreach ( var option in options ) {
				ComboBoxItem item = new ComboBoxItem();
				item.Content = option;
				if ( first_item ) { item.IsSelected = true; first_item = false; }
				this.combobox.Items.Add(item);
			}
		}
	}
}
