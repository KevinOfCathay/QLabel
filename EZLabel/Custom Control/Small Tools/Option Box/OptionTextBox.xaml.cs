using QLabel.Custom_Control.Small_Tools;
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
	/// Interaction logic for OptionTextBox.xaml
	/// </summary>
	public partial class OptionTextBox : UserControl {
		private static HashSet<char> valid_float_input = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '.' };
		private static HashSet<char> valid_int_input = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-' };
		public enum AcceptedType { None, Int, Float }
		public AcceptedType Type {
			get { return (AcceptedType) GetValue(accepted_type_property); }
			set { SetValue(accepted_type_property, value); }
		}
		public static readonly DependencyProperty accepted_type_property = DependencyProperty.Register("Type", typeof(AcceptedType),
				  typeof(OptionTextBox), new PropertyMetadata(AcceptedType.None));

		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(OptionTextBox),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(OptionTextBox),
			    typeMetadata: new PropertyMetadata("sub text"));

		public OptionTextBox () {
			InitializeComponent();
		}

		public string Text { get { return inputbox.Text; } set { inputbox.Text = value; } }
		public int CaretIndex { get { return inputbox.CaretIndex; } set { inputbox.CaretIndex = value; } }
		public new bool IsEnabled { get { return inputbox.IsEnabled; } set { inputbox.IsEnabled = value; } }

		private void InputboxTextChanged (object sender, TextChangedEventArgs e) {
			int cursoridex;
			string previous_input, new_input;
			StringBuilder sb;
			switch ( Type ) {
				case AcceptedType.None:
					return;
				case AcceptedType.Int:
					cursoridex = inputbox.CaretIndex;
					previous_input = inputbox.Text;
					sb = new StringBuilder();
					foreach ( char c in previous_input ) {
						if ( valid_int_input.Contains(c) ) { sb.Append(c); }
					}
					new_input = sb.ToString();
					inputbox.Text = new_input;
					if ( new_input.Length <= cursoridex ) {
						inputbox.CaretIndex = new_input.Length;
					} else {
						inputbox.CaretIndex = cursoridex;
					}
					return;
				case AcceptedType.Float:
					cursoridex = inputbox.CaretIndex;
					previous_input = inputbox.Text;
					sb = new StringBuilder();
					foreach ( char c in previous_input ) {
						if ( valid_float_input.Contains(c) ) { sb.Append(c); }
					}
					new_input = sb.ToString();
					inputbox.Text = new_input;
					if ( new_input.Length <= cursoridex ) {
						inputbox.CaretIndex = new_input.Length;
					} else {
						inputbox.CaretIndex = cursoridex;
					}
					return;
				default:
					return;
			}
		}
	}
}
