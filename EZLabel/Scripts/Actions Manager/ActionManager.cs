using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Actions {
	public static class ActionManager {
		private static int cap = 50;
		static LinkedList<IAction> actions = new LinkedList<IAction>();
		public static void PushAndExecute (IAction action) {
			actions.AddLast(action);
			action.Do();
		}
		public static void PushAction (IAction action) {
			actions.AddLast(action);
			if ( actions.Count > cap ) {
				actions.RemoveFirst();
			}
		}
		public static void PopAction () {
			if ( actions.Count > 0 ) {
				var action = actions.Last();       // Get last element
				action.Undo();
				actions.RemoveLast();         // Pop last element
			}
		}
		public static void Clear () {
			actions.Clear();
		}
	}
}
