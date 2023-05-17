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
			public CheckboxWithLabel checkbox;
		}
		private sealed class ClassNode {
			public string name;      // Class name
			public TreeViewItem node = null;
			public List<ItemNode> items = null;
			public CheckboxWithLabel checkbox;
		}
		private sealed class SuperCategoryNode {
			public string name = null; // SuperCategory name
			public TreeViewItem node = null;
			public List<ClassNode> classes = new List<ClassNode>();
			public CheckboxWithLabel checkbox;
		}
		private sealed class GroupNode {
			public string name = null; // GroupNode name
			public TreeViewItem node = null;
			public List<SuperCategoryNode> categories = new List<SuperCategoryNode>();
			public CheckboxWithLabel checkbox;
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
			string category_name = clas.supercategory;
			GroupNode group_node;
			if ( !groups.ContainsKey(group_name) ) {
				// 创建一个新的 group item
				TreeViewItem group_item = new TreeViewItem();
				CheckboxWithLabel cbxlbl = new CheckboxWithLabel(group_name, is_checked: true);
				group_item.Header = cbxlbl;
				annotree.Items.Add(group_item);
				var new_group_node = new GroupNode {
					name = group_name, node = group_item,
					categories = new List<SuperCategoryNode>(), checkbox = cbxlbl
				};
				groups.Add(group_name, new_group_node);

				//  eCheck/eUncheck 事件
				cbxlbl.eChecked += (_, _) => {
					foreach ( var cat in new_group_node.categories ) { cat.checkbox.Check(); }
				};
				cbxlbl.eUnchecked += (_, _) => {
					foreach ( var cat in new_group_node.categories ) { cat.checkbox.Uncheck(); }
				};
			}
			// 获取 group item
			group_node = groups[group_name];

			// 搜寻当前 group item 下是否含有 supercategory
			var category_node = group_node.categories.Find((c) => { return c.name == category_name; });
			if ( category_node == null ) {
				// 创建一个新的 supercategory item
				TreeViewItem category_item = new TreeViewItem();
				CheckboxWithLabel cbxlbl = new CheckboxWithLabel(category_name, is_checked: true);
				category_item.Header = cbxlbl;
				category_node = new SuperCategoryNode {
					name = category_name, node = category_item, classes = new List<ClassNode>(), checkbox = cbxlbl
				};
				group_node.categories.Add(category_node);

				//  eCheck/eUncheck 事件
				cbxlbl.eChecked += (_, _) => {
					foreach ( var clss in category_node.classes ) { clss.checkbox.Check(); }
				};
				cbxlbl.eUnchecked += (_, _) => {
					foreach ( var clss in category_node.classes ) { clss.checkbox.Uncheck(); }
				};
				group_node.node.Items.Add(category_item);
			}

			string class_name = clas.name;
			var class_node = category_node.classes.Find((c) => { return c.name == class_name; });

			if ( class_node == null ) {
				// 在 supercategory 下创建一个新的 class item
				TreeViewItem class_item = new TreeViewItem();
				CheckboxWithLabel c_cbxlbl = new CheckboxWithLabel(class_name, is_checked: true);
				class_item.Header = c_cbxlbl;
				class_node = new ClassNode() {
					name = class_name, node = class_item, items = new List<ItemNode>(), checkbox = c_cbxlbl
				};
				category_node.classes.Add(class_node);

				c_cbxlbl.eChecked += (_, _) => {
					foreach ( var item in class_node.items ) { item.checkbox.Check(); }
				};
				c_cbxlbl.eUnchecked += (_, _) => {
					foreach ( var item in class_node.items ) { item.checkbox.Uncheck(); }
				};
				category_node.node.Items.Add(class_item);
			}

			// 在 class 下面添加一行
			TreeViewItem item_item = new TreeViewItem();
			CheckboxWithLabel i_cbxlbl = new CheckboxWithLabel(
				string.Join(" ", class_name, data.rpoints.to_shortstring()), is_checked: elem.data.check);
			item_item.Header = i_cbxlbl;
			ItemNode item_node = new ItemNode {
				data = data, node = item_item, checkbox = i_cbxlbl
			};
			i_cbxlbl.eChecked += (_, _) => { elem.data.check = true; elem.Show(); };
			i_cbxlbl.eUnchecked += (_, _) => { elem.data.check = false; elem.Hide(); };
			class_node.items.Add(item_node);
			class_node.node.Items.Add(item_item);

		}
		public void RemoveAnnoData (IAnnotationElement elem) {
			datalist.Remove(elem.data);
		}
	}
}
