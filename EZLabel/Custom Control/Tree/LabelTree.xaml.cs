﻿using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Custom_Control.Label_Tree {
	public partial class LabelTree : UserControl {
		public string MainText {
			get { return (string) GetValue(maintextproperty); }
			set { SetValue(maintextproperty, value); }
		}
		public static readonly DependencyProperty maintextproperty =
		    DependencyProperty.Register("MainText",
			    propertyType: typeof(string), ownerType: typeof(LabelTree),
			    typeMetadata: new PropertyMetadata("main text"));
		public string SubText {
			get { return (string) GetValue(subtextproperty); }
			set { SetValue(subtextproperty, value); }
		}
		public static readonly DependencyProperty subtextproperty =
		    DependencyProperty.Register("SubText",
			    propertyType: typeof(string), ownerType: typeof(LabelTree),
			    typeMetadata: new PropertyMetadata("sub text"));

		public bool checkbox_active = true;
		public event Action<ClassTemplate> eCheck, eUncheck;
		public LabelTree () {
			InitializeComponent();
		}
		public LabelTree (bool checkbox_active = true) {
			InitializeComponent();
			this.checkbox_active = checkbox_active;
		}
		#region Classes
		private class Node {
			public string name;
			public TreeViewItem node = null;
			public List<Node> children = null;
			public CheckboxWithLabel checkbox;
		}
		#endregion Classes
		private List<Node> groups = new List<Node>();
		private List<ClassTemplate> _selected_labels = new List<ClassTemplate>();
		public IEnumerable<ClassTemplate> selected_labels { get => selected_labels; }

		public void SetUI (IEnumerable<ClassTemplate> labels,
			Action<ClassTemplate> check = null,
			Action<ClassTemplate> uncheck = null,
			bool expanded = false,
			bool clear_previous = true,
			bool is_checked = true) {

			// 清空之前的所有内容
			if ( clear_previous ) {
				labeltree.Items.Clear(); groups.Clear();
				this.eCheck = null; this.eUncheck = null;
			}

			// 设置 events
			this.eCheck += check;
			this.eUncheck += uncheck;

			// 将 ClassTemplate 加入到树中
			foreach ( var data in labels ) {
				AddElementToTree(data, is_checked);
			}
			if ( expanded ) {
				foreach ( var group in groups ) {
					group.node.IsExpanded = true;
					foreach ( var category in group.children ) {
						category.node.IsExpanded = true;
						foreach ( var clas in category.children ) {
							clas.node.IsExpanded = true;
						}
					}
				}
			}
		}
		private void AddElementToTree (ClassTemplate label, bool is_checked) {
			// 将数据加入到 Tree 中
			// 结构：GroupNode  -- ClassNode -- ItemNode --> Item
			string class_name = label.name;
			string group_name = label.group;
			string category_name = label.supercategory;

			Node group_node = groups.Find((g) => { return g.name == group_name; });
			if ( group_node == null ) {
				// 如果 group child 为空（未找到）
				// 创建一个新的 group child
				TreeViewItem group_item = new TreeViewItem();
				CheckboxWithLabel cbxlbl = new CheckboxWithLabel(group_name, is_checked: is_checked, enable_checkbox: checkbox_active);
				group_item.Header = cbxlbl;

				labeltree.Items.Add(group_item);
				group_node = new Node {
					name = group_name, node = group_item,
					children = new List<Node>(), checkbox = cbxlbl
				};
				groups.Add(group_node);

				//  eCheck/eUncheck 事件
				cbxlbl.eChecked += (_, _) => { foreach ( var child in group_node.children ) { child.checkbox.Check(); } };
				cbxlbl.eUnchecked += (_, _) => { foreach ( var child in group_node.children ) { child.checkbox.Uncheck(); } };
			}

			// 搜寻当前 group child 下是否含有 supercategory
			Node category_node = group_node.children.Find((c) => { return c.name == category_name; });
			if ( category_node == null ) {
				// 创建一个新的 supercategory child
				TreeViewItem category_item = new TreeViewItem();
				CheckboxWithLabel cbxlbl = new CheckboxWithLabel(category_name, is_checked: is_checked, enable_checkbox: checkbox_active);
				category_item.Header = cbxlbl;
				category_node = new Node {
					name = category_name, node = category_item,
					children = new List<Node>(), checkbox = cbxlbl
				};
				// 在 group child 下创建
				group_node.children.Add(category_node);

				//  eCheck / eUncheck 事件
				cbxlbl.eChecked += (_, _) => { foreach ( var child in category_node.children ) { child.checkbox.Check(); } };
				cbxlbl.eUnchecked += (_, _) => { foreach ( var child in category_node.children ) { child.checkbox.Uncheck(); } };
				group_node.node.Items.Add(category_item);
			}

			Node class_node = category_node.children.Find((c) => { return c.name == class_name; });

			if ( class_node == null ) {
				// 在 supercategory 下创建一个新的 class child
				TreeViewItem class_item = new TreeViewItem();
				CheckboxWithLabel c_cbxlbl = new CheckboxWithLabel(
					class_name, is_checked: is_checked, enable_checkbox: checkbox_active,
					delegate (object _, RoutedEventArgs _) { eCheck?.Invoke(label); _selected_labels.Add(label); },
					delegate (object _, RoutedEventArgs _) { eUncheck?.Invoke(label); _selected_labels.Remove(label); }
					);
				class_item.Header = c_cbxlbl;
				class_node = new Node() {
					name = class_name, node = class_item,
					children = new List<Node>(), checkbox = c_cbxlbl
				};
				category_node.children.Add(class_node);
				category_node.node.Items.Add(class_item);

				if ( is_checked ) { c_cbxlbl.Check(); }
			}
		}
	}
}
