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
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace QLabel.Custom_Control.Small_Tools.Textboxes {
	/// <summary>
	/// Interaction logic for IntBox.xaml
	/// </summary>
	public partial class IntBox : UserControl {
		private static HashSet<char> valid_input = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-' };
		public IntBox () {
			InitializeComponent();
		}
		private void TextBoxTextChanged (object sender, TextChangedEventArgs e) {
			try {
				// Validate input
				int cursoridex = textbox.CaretIndex;
				string previous_input = textbox.Text;
				StringBuilder sb = new StringBuilder();
				foreach ( char c in previous_input ) {
					if ( valid_input.Contains(c) ) {
						sb.Append(c);
					}
				}
				string new_input = sb.ToString();
				textbox.Text = new_input;
				if ( new_input.Length <= cursoridex ) {
					textbox.CaretIndex = new_input.Length;
				} else {
					textbox.CaretIndex = cursoridex;
				}
			} catch ( Exception ) {
			}
		}
	}
}
