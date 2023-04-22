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

		#region Classes
		private class Node {
			public string name;
			public TreeViewItem node = null;
			public List<Node> children = null;
			public CheckboxWithLabel checkbox;
		}
		#endregion

		private List<Node> top = new List<Node>();

		public FileTree () {
			InitializeComponent();
		}
		public FileTree (IEnumerable<ImageData> files,
			Action<ClassLabel> check = null, Action<ClassLabel> uncheck = null) {
			InitializeComponent();
			SetUI(files, check, uncheck);
		}

		public void SetUI (IEnumerable<ImageData> files,
			Action<ClassLabel> check = null,
			Action<ClassLabel> uncheck = null,
			bool expanded = false, bool clear_previous = true) {
			// 清空之前的所有内容
			if ( clear_previous ) { filetree.Items.Clear(); top.Clear(); }

			// 将 file 加入到树中
			foreach ( var data in files ) {
				AddElementToTree(data);
			}
		}
		private void AddElementToTree (ImageData data) {
			string filename = data.filename;
			Node file_node = top.Find((g) => { return g.name == filename; });
			if ( file_node == null ) {
				TreeViewItem file_item = new TreeViewItem();
			}
		}
	}
}