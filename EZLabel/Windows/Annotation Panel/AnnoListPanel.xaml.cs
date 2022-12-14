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
using EZLabel.Scripts.AnnotationData;

namespace EZLabel.Windows.Annotation_List
{
    /// <summary>
    /// Interaction logic for AnnoListPanel.xaml
    /// </summary>
    public partial class AnnoListPanel : UserControl {
		public List<AnnoData> anno_data_list;
		public AnnoListPanel () {
			InitializeComponent();
			anno_data_list = new List<AnnoData>();
		}

		public void AddAnnotation (AnnoData anno_data) {
			anno_data_list.Add(anno_data);
		}
	}
}
