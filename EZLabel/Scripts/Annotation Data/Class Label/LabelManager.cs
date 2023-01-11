using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	public class LabelManager {
		private List<ClassLabel> class_labels = new List<ClassLabel>();
		public event Action<ClassLabel> eAddLabel, eRemoveLabel;

		public void AddLabel (ClassLabel label) {
			class_labels.Add(label);
			eAddLabel?.Invoke(label);
		}
		public void RemoveLabel (ClassLabel label) {
			class_labels.Remove(label);
			eRemoveLabel?.Invoke(label);
		}
	}
}
