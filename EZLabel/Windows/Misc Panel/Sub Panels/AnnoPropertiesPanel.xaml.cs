using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Utils;
using QLabel.Windows.Main_Canvas;
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

namespace QLabel.Windows.Misc_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AnnoPropertiesPanel.xaml
	/// </summary>
	public partial class AnnoPropertiesPanel : UserControl {
		public AnnoPropertiesPanel () {
			InitializeComponent();
			caption.IsEnabled = false;
			truncated.IsEnabled = false;
			occluded.IsEnabled = false;
		}
		public void SetUI (IAnnotationElement elem) {
			if ( elem != null ) {
				var data = elem.data;
				if ( data != null ) {
					points.Text = data.rpoints.to_shortstring(25);
					caption.Text = data.caption;
					truncated.IsChecked = data.truncated;
					occluded.IsChecked = data.occluded;
				}
			}
		}
	}
}
