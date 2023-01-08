using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	public interface IAction {
		public abstract void Do();
		public abstract void Undo ();
	}
}
