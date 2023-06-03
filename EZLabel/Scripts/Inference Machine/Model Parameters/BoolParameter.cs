using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Inference_Machine {
	internal class BoolParameter : ModelParameter {
		public BoolParameter (string name, object value, bool editable) : base(name, typeof(bool), value, editable) {
		}
	}
}
