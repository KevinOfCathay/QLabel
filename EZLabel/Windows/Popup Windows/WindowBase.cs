using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Windows.Popup_Windows {
	public abstract class WindowBase : Window {
		public abstract void Initialization ();
		public WindowBase (string title) : base() {
			this.Title = title;
			Initialization();
		}
	}
}
