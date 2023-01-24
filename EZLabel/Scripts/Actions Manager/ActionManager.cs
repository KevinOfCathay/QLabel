using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	public static class ActionManager {
		static Stack<IAction> actions = new Stack<IAction>(capacity: 50);
		public static void PushAction (IAction action) {
			actions.Push(action);
		}
		public static void PopAction () {
			if ( actions.Count > 0 ) {
				var action = actions.Pop();
				action.Undo();
			}
		}
		public static void Flush () {
			actions.Clear();
		}
	}
}
