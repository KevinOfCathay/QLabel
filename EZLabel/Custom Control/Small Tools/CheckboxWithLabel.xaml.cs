﻿using System;
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
	/// Interaction logic for CheckboxWithLabel.xaml
	/// </summary>
	public partial class CheckboxWithLabel : UserControl {
		public event Action<CheckboxWithLabel, object, RoutedEventArgs> eChecked, eUnchecked;
		public CheckboxWithLabel () {
			InitializeComponent();
		}
		public void SetLabel (string text) {
			label.Content = text;
		}
		public void Check () {
			this.checkbox.IsChecked = true;
		}
		private void CheckBox_Checked (object sender, RoutedEventArgs e) {
			eChecked?.Invoke(this, sender, e);
		}
		private void CheckBox_Unchecked (object sender, RoutedEventArgs e) {
			eUnchecked?.Invoke(this, sender, e);
		}
	}
}