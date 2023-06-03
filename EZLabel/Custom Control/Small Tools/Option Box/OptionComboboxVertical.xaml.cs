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
	public partial class OptionComboboxVertical : UserControl {

		public event SelectionChangedEventHandler SelectionChanged {
			add { this.combobox.SelectionChanged += value; }
			remove { this.combobox.SelectionChanged -= value; }
		}

		public List<string> ComboList {
			get { return (List<string>) GetValue(combolist_property); }
			set { SetValue(maintext_property, value); SetUI(value); }
		}
		private static readonly DependencyProperty combolist_property = DependencyProperty.Register(
			 name: "ComboList",
			 propertyType: typeof(List<string>),
			 ownerType: typeof(OptionComboboxVertical)
		    );
		public string MainText {
			get { return (string) GetValue(maintext_property); }
			set { SetValue(maintext_property, value); }
		}
		public static readonly DependencyProperty maintext_property = DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionComboboxVertical),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionComboboxVertical),
			    typeMetadata: new PropertyMetadata("sub text"));

		public ItemCollection Items { get => combobox.Items; }
		public int SelectedIndex { get => combobox.SelectedIndex; }
		public object SelectedItem { get => combobox.SelectedItem; }


		public OptionComboboxVertical () {
			InitializeComponent();
		}
		public OptionComboboxVertical (IEnumerable<string> options) {
			InitializeComponent();
			SetUI(options);
		}
		public void SetUI (IEnumerable<string> options) {
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
