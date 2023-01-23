using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Utils;
using QLabel.Windows.Main_Canvas;
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

namespace QLabel.Windows.Annotation_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AnnoTree.xaml
	/// </summary>
	public partial class AnnoTree : UserControl {
		private sealed class ItemNode {
			public TreeViewItem node = null;
			public AnnoData data = null;
		}
		private sealed class ClassNode {
			public string name;      // Class name
			public TreeViewItem node = null;
			public List<ItemNode> items = null;
			public List<CheckboxWithLabel> checkboxes = new List<CheckboxWithLabel>();
		}
		private sealed class GroupNode {
			public string name = null; // GroupNode name
			public TreeViewItem node = null;
			public List<ClassNode> classes = new List<ClassNode>();
			public List<CheckboxWithLabel> checkboxes = new List<CheckboxWithLabel>();
		}
		private List<AnnoData> datalist = new List<AnnoData>();
		private Dictionary<string, GroupNode> groups = new Dictionary<string, GroupNode>();

		public AnnoTree () {
			InitializeComponent();
		}
		public void AddAnnoData (IAnnotationElement elem) {
			var data = elem.data;
			datalist.Add(data);

			// 将数据加入到 Tree 中
			// 结构：GroupNode Name -- Class Name -- Item Name
			var clas = data.clas;
			string group_name = clas.group;
			GroupNode group_node;
			if ( !groups.ContainsKey(group_name) ) {
				// 创建一个新的 group node
				TreeViewItem groupnode = new TreeViewItem();
				CheckboxWithLabel cbxlbl = new CheckboxWithLabel(group_name, check: true);
				groupnode.Header = cbxlbl;
				annotree.Items.Add(groupnode);
				var new_group_node = new GroupNode { name = group_name, node = groupnode, classes = new List<ClassNode>(), checkboxes = new List<CheckboxWithLabel>() };
				groups.Add(group_name, new_group_node);

				cbxlbl.eChecked += (_, _) => { foreach ( var bx in new_group_node.checkboxes ) { bx.Check(); } };
				cbxlbl.eUnchecked += (_, _) => { foreach ( var bx in new_group_node.checkboxes ) { bx.Uncheck(); } };
			}
			// 获取 group node
			group_node = groups[group_name];

			string class_name = clas.name;
			var class_node = group_node.classes.Find((i) => { return i.name == class_name; });
			if ( class_node == null ) {
				// 在 group 下创建一个新的 class node
				TreeViewItem classnode = new TreeViewItem();
				CheckboxWithLabel c_cbxlbl = new CheckboxWithLabel(class_name, check: true);

				classnode.Header = c_cbxlbl;
				group_node.node.Items.Add(classnode);

				class_node = new ClassNode() { name = class_name, node = classnode, items = new List<ItemNode>(), checkboxes = new List<CheckboxWithLabel>() };
				group_node.classes.Add(class_node);

				c_cbxlbl.eChecked += (_, _) => { foreach ( var bx in class_node.checkboxes ) { bx.Check(); } };
				c_cbxlbl.eUnchecked += (_, _) => { foreach ( var bx in class_node.checkboxes ) { bx.Uncheck(); } };
				group_node.checkboxes.Add(c_cbxlbl);
			}

			// 在 class 下面添加一行
			TreeViewItem itemnode = new TreeViewItem();
			CheckboxWithLabel i_cbxlbl = new CheckboxWithLabel(string.Join(" ", class_name, data.points.to_shortstring()), check: true);
			i_cbxlbl.eChecked += (_, _) => { elem.Show(); };
			i_cbxlbl.eUnchecked += (_, _) => { elem.Hide(); };
			itemnode.Header = i_cbxlbl;
			class_node.node.Items.Add(itemnode);
			class_node.items.Add(new ItemNode { data = data, node = itemnode });

			class_node.checkboxes.Add(i_cbxlbl);
		}
		public void RemoveAnnoData (IAnnotationElement elem) {
			datalist.Remove(elem.data);
		}
	}
}
