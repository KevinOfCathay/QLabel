using System;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Custom_Control.Small_Tools {
	public partial class ButtonWithText : UserControl {
		// 按钮按下时触发事件
		public Action<RoutedEventArgs> click;
		public ButtonWithText () {
			InitializeComponent();
		}

		private void button_Click (object sender, RoutedEventArgs e) {
			click?.Invoke(e);
		}
	}
}
