using QLabel.Scripts;
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
	/// Interaction logic for LabelsPanel.xaml
	/// </summary>
	public partial class LabelsPanel : UserControl {
		public event Action<LabelsPanel, ListBoxItem> eItemAdded, eItemRemoved;
		public List<ClassLabel> labels = new List<ClassLabel>();

		public LabelsPanel () { InitializeComponent(); }
		public void AddItem (ListBoxItem item) {
			listbox.Items.Add(item);
			eItemAdded?.Invoke(this, item);
		}
		public void RemoveItem (ListBoxItem item) {
			listbox.Items.Remove(item);
			eItemRemoved?.Invoke(this, item);
		}
	}
}
