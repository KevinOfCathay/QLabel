using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace QLabel.Windows.Main_Canvas.Objects {
	/// <summary>
	/// Interaction logic for LabelRectangle.xaml
	/// </summary>
	public partial class AnnoLabel : UserControl {
		public AnnoLabel () {
			InitializeComponent();
		}
		public AnnoLabel (Point cpoint, string text) {
			InitializeComponent();

			this.text.Text = text;
			Canvas.SetLeft(this, cpoint.X);
			Canvas.SetTop(this, cpoint.Y);
		}
	}
}
