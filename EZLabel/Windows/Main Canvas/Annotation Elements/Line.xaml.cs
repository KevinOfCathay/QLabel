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
	/// Interaction logic for Line.xaml
	/// </summary>
	public partial class Line : UserControl, IAnnotationElement {
		public Line () {
			InitializeComponent();
		}

		public void Delete () {
			throw new NotImplementedException();
		}
	}
}
