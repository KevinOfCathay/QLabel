using QLabel.Custom_Control.Small_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLabel.Custom_Control.Label_Tree {
	internal class Node<T> {
		public string name;
		public T? item = default;
		public List<Node<T>> children = null;
		public CheckboxWithLabel checkbox;
	}
}
