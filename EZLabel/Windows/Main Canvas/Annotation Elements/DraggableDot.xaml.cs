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

namespace EZLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// Interaction logic for DraggableDot.xaml
	/// </summary>
	public partial class DraggableDot : UserControl {
		public Action<DraggableDot, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;
		public bool activate { private set; get; } = false;

		public DraggableDot () {
			InitializeComponent();
		}

		private void dot_PreviewMouseDown (object sender, MouseButtonEventArgs e) {
			activate = true;
			eMouseDown?.Invoke(this, e);

			e.Handled = true;
		}

		private void dot_PreviewMouseMove (object sender, MouseEventArgs e) {
			eMouseMove?.Invoke(this, e);
		}

		private void dot_PreviewMouseUp (object sender, MouseButtonEventArgs e) {
			activate = false;
			eMouseDown?.Invoke(this, e);

			e.Handled = true;
		}

		private void Drag () {

		}
	}
}
