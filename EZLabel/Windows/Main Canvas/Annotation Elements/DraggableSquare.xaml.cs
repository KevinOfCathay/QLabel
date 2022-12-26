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

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// 正方形
	/// </summary>
	public partial class DraggableSquare : UserControl {
		public DraggableSquare () {
			InitializeComponent();
		}
		private void container_MouseEnter (object sender, MouseEventArgs e) {
		}

		private void container_MouseLeave (object sender, MouseEventArgs e) {
		}
	}
}