using QLabel.Scripts.AnnotationData;
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
	/// 点
	/// </summary>
	public partial class DraggableDot : UserControl, IAnnotationElement {
		public Action<DraggableDot, MouseEventArgs> eMouseDown, eMouseMove, eMouseUp;
		public bool activate { private set; get; } = false;

		AnnoData _data;   // 这个点所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }

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

		public void Delete () {
			throw new NotImplementedException();
		}
	}
}
