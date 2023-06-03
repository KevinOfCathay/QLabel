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

namespace QLabel.Custom_Control.Small_Tools {
	/// <summary>
	/// 两个按钮的组合
	/// </summary>
	public partial class DualButton : UserControl {
		public event RoutedEventHandler AClick {
			add { this.a.Click += value; }
			remove { this.a.Click -= value; }
		}
		public event RoutedEventHandler BClick {
			add { this.b.Click += value; }
			remove { this.b.Click -= value; }
		}

		public string TextA {
			get { return (string) GetValue(text_a_property); }
			set { SetValue(text_a_property, value); }
		}
		public static readonly DependencyProperty text_a_property =
		    DependencyProperty.Register("TextA",
			    propertyType: typeof(string),
			    ownerType: typeof(DualButton),
			    typeMetadata: new PropertyMetadata("Confirm"));

		public string TextB {
			get { return (string) GetValue(text_b_property); }
			set { SetValue(text_b_property, value); }
		}
		public static readonly DependencyProperty text_b_property =
		    DependencyProperty.Register("TextB",
			    propertyType: typeof(string),
			    ownerType: typeof(DualButton),
			    typeMetadata: new PropertyMetadata("Cancel"));

		public DualButton () {
			InitializeComponent();
		}
	}
}
