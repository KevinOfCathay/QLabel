using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Custom_Control.Label_Tree {
	/// <summary>
	/// Interaction logic for FileTree.xaml
	/// </summary>
	public partial class FileTree : UserControl {
		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(FileTree),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(FileTree),
			    typeMetadata: new PropertyMetadata("sub text"));

		private Node<TreeViewItem> top = new Node<TreeViewItem>();
		private List<ImageData> _selected_data = new List<ImageData>();

		public FileTree () {
			InitializeComponent();
		}
		public FileTree (IEnumerable<ImageData> files,
			Action<ImageData> check = null, Action<ImageData> uncheck = null) {
			InitializeComponent();
			SetUI(files, check, uncheck);
		}

		public void SetUI (IEnumerable<ImageData> datas,
			Action<ImageData> check = null,
			Action<ImageData> uncheck = null,
			bool expanded = true, bool clear_previous = true) {
			// 清空之前的所有内容
			if ( clear_previous ) { filetree.Items.Clear(); top = null; }

			// 将文件路径加入到树中
			foreach ( var data in datas ) {
				AddElementToTree(data, true, expanded, check, uncheck);
			}
		}

		private Node<TreeViewItem> AddElementToTree (ImageData data, bool is_checked, bool is_expanded,
												Action<ImageData> eCheck = null,
												Action<ImageData> eUncheck = null) {
			string filename = data.filename;

			// 建立 top node
			if ( top == null ) {
				top = new Node<TreeViewItem>() { name = "All", children = new List<Node<TreeViewItem>>() };
				CheckboxWithLabel c_cbxlbl = new CheckboxWithLabel(
						"All", is_checked: is_checked, enable_checkbox: true,
						delegate (object _, RoutedEventArgs _) { foreach ( var child in top.children ) { child.checkbox.Check(); } },
						delegate (object _, RoutedEventArgs _) { foreach ( var child in top.children ) { child.checkbox.Uncheck(); } }
						);
				TreeViewItem top_item = new TreeViewItem() { Header = c_cbxlbl };
				top.item = top_item;
				top.checkbox = c_cbxlbl;
				if ( is_expanded ) { top.item.IsExpanded = true; }

				filetree.Items.Add(top_item);
			}

			// 寻找 item
			Node<TreeViewItem> file_node = top.children.Find((g) => { return g.name == filename; });
			if ( file_node == null ) {
				CheckboxWithLabel c_cbxlbl = new CheckboxWithLabel(
						filename, is_checked: is_checked, enable_checkbox: true,
						delegate (object _, RoutedEventArgs _) { eCheck?.Invoke(data); _selected_data.Add(data); },
						delegate (object _, RoutedEventArgs _) { eUncheck?.Invoke(data); _selected_data.Remove(data); }
						);
				TreeViewItem file_item = new TreeViewItem() { Header = c_cbxlbl };
				top.item.Items.Add(file_item);

				file_node = new Node<TreeViewItem> {
					item = file_item, checkbox = c_cbxlbl, children = new List<Node<TreeViewItem>>(), name = filename
				};
				top.children.Add(file_node);
			}
			return file_node;
		}
	}
}