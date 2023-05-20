using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Inference_Machine {
	internal class IntParameter : ModelParameter {
		public IntParameter (string name, object value, bool editable) : base(name, typeof(int), value, editable) {
		}
	}
}
