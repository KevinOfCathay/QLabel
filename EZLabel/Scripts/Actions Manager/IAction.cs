using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Actions {
	public interface IAction {
		public string name { get; }
		public abstract void Do ();
		public abstract void Undo ();
	}
}
