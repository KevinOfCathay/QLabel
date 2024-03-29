﻿using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Utils;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace QLabel.Windows.Annotation_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AnnoList.xaml
	/// </summary>
	public partial class AnnoList : UserControl {
		public record class Row : INotifyPropertyChanged {
			private int _index;
			private AnnoData.Type _type;
			private string _points;
			private string _class;
			private string _label;
			private string _group;
			private bool _truncated;
			private bool _occluded;
			private string _supercategory;

			public int Index { get { return _index; } set { _index = value; OnPropertyChanged("Index"); } }
			public AnnoData.Type Type { get { return _type; } set { _type = value; OnPropertyChanged("Type"); } }
			public string Points { get { return _points; } set { _points = value; OnPropertyChanged("Points"); } }
			public string Class { get { return _class; } set { _class = value; OnPropertyChanged("Class"); } }
			public string Label { get { return _label; } set { _label = value; OnPropertyChanged("Label"); } }
			public string Group { get { return _group; } set { _group = value; OnPropertyChanged("Group"); } }
			public string Supercategory { get { return _supercategory; } set { _supercategory = value; OnPropertyChanged("Supercategory"); } }
			public bool Truncated { get { return _truncated; } set { _truncated = value; OnPropertyChanged("Truncated"); } }
			public bool Occluded { get { return _occluded; } set { _occluded = value; OnPropertyChanged("Occluded"); } }
			public IAnnotationElement elem { get; set; }
			public IComparable this[int i] {
				get {
					switch ( i ) {
						case 0: return Index;
						case 1: return Type;
						case 2: return Points;
						case 3: return Class;
						case 4: return Label;
						case 5: return Group;
						case 6: return Supercategory;
						case 7: return Truncated;
						case 8: return Occluded;
						default: return Index;
					}
				}
			}

			// https://stackoverflow.com/a/21050642
			public event PropertyChangedEventHandler? PropertyChanged;
			protected virtual void OnPropertyChanged (string PropertyName) {
				if ( PropertyChanged != null ) {
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
				}
			}
		}
		public ObservableCollection<Row> rows { get; } = new ObservableCollection<Row>();
		public int col_sort_index = 1;

		public event Action<IAnnotationElement> eItemSelected;

		public AnnoList () {
			InitializeComponent();
		}
		/// <summary>
		/// 在列表中加入一行
		/// </summary>
		public void AddItem (IAnnotationElement elem) {
			var data = elem.data;
			var num_rows = rows.Count;

			// 从上往下进行排序
			var row = new Row {
				Index = this.listview.Items.Count,
				Type = data.type,
				Class = data.class_label.name,
				Group = data.class_label.group,
				Supercategory = data.class_label.supercategory,
				Points = data.rpoints.to_string(),
				elem = elem
			};
			int new_index = 0;
			foreach ( var r in rows ) {
				if ( r[col_sort_index].CompareTo(row[col_sort_index]) > 0 ) {
					break;
				}
				new_index += 1;
			}
			rows.Add(row);
			rows.Move(num_rows, new_index);
		}
		public void RefreshItem (IAnnotationElement elem) {
			for ( int i = 0; i < rows.Count; i += 1 ) {
				if ( rows[i].elem == elem ) { // 找到当前的 elem
					var data = elem.data;
					if ( data != null ) {
						if ( data.type != rows[i].Type ) { rows[i].Type = data.type; }
						if ( data.class_label.name != rows[i].Class ) { rows[i].Class = data.class_label.name; }
						if ( data.class_label.group != rows[i].Group ) { rows[i].Group = data.class_label.group; }
						if ( data.class_label.supercategory != rows[i].Supercategory ) { rows[i].Supercategory = data.class_label.supercategory; }
						if ( data.truncated != rows[i].Truncated ) { rows[i].Truncated = data.truncated; }
						if ( data.occluded != rows[i].Occluded ) { rows[i].Occluded = data.occluded; }
						rows[i].Points = data.rpoints.to_string();
					}
					break;
				}
			}
		}
		public void RemoveItem (IAnnotationElement elem) {
			for ( int i = 0; i < rows.Count; i += 1 ) {
				if ( rows[i].elem == elem ) {
					rows.RemoveAt(i);
					return;
				}
			}
		}
		/// <summary>
		/// 清除列表中所有的内容
		/// </summary>
		public void ClearList () {
			this.rows.Clear();
		}
		private void Col_Click (object sender, RoutedEventArgs e) {
			// https://stackoverflow.com/a/47199297
			var s = e.OriginalSource as GridViewColumnHeader;
			if ( s != null ) {
				GridViewHeaderRowPresenter presenter = s.Parent as GridViewHeaderRowPresenter;
				if ( presenter != null ) {
					col_sort_index = presenter.Columns.IndexOf(s.Column);
					SortRows();
				}
			}
		}
		/// <summary>
		/// 重新排列所有的行
		/// </summary>
		private void SortRows () {
			int count = rows.Count;
			Row[] new_rows = new Row[count];
			for ( int i = 0; i < count; i += 1 ) {
				new_rows[i] = rows[i];
			}
			Array.Sort(new_rows, (A, B) => { return A[col_sort_index].CompareTo(B[col_sort_index]); });
			rows.Clear();
			for ( int i = 0; i < count; i += 1 ) {
				rows.Add(new_rows[i]);
			}
		}
		private void ListViewItemSelected (object sender, RoutedEventArgs e) {
			var item = sender as ListViewItem;
			if ( item != null && item.DataContext != null ) {       // item 本身不为 null, item 所关联的 datacontext 不为 null
				var context = item.DataContext as Row;
				if ( context != null ) {
					eItemSelected?.Invoke(context.elem);
				}
			}
		}
	}
}
