using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Custom_Control.Small_Tools {
	public partial class ButtonWithText : UserControl {
		public event RoutedEventHandler Click {
			add { this.button.Click += value; }
			remove { this.button.Click -= value; }
		}
		public string Text {
			get { return (string) GetValue(textproperty); }
			set { SetValue(textproperty, value); }
		}
		public static readonly DependencyProperty textproperty =
		    DependencyProperty.Register("Text",
			    propertyType: typeof(string),
			    ownerType: typeof(ButtonWithText),
			    typeMetadata: new PropertyMetadata("text"));

		public string ImgSource {
			get { return (string) GetValue(imgsourceproperty); }
			set { SetValue(imgsourceproperty, value); }
		}
		public static readonly DependencyProperty imgsourceproperty =
		    DependencyProperty.Register("ImgSource",
			    propertyType: typeof(string),
			    ownerType: typeof(ButtonWithText),
			    typeMetadata: new PropertyMetadata());

		public ButtonWithText () {
			InitializeComponent();
		}
	}
}
