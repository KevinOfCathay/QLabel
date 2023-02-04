using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Utils;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLabel.Windows.Annotation_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AnnoList.xaml
	/// </summary>
	public partial class AnnoList : UserControl {
		public record class Row {
			public int Index { get; set; }
			public AnnoData.Type Type { get; set; }
			public string Points { get; set; }
			public string Class { get; set; }
			public string Label { get; set; }
			public string Group { get; set; }
			public string Supercategory { get; set; }
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
						default: return Index;
					}
				}
			}
		}
		public ObservableCollection<Row> rows { get; } = new ObservableCollection<Row>();
		public int col_sort_index = 1;

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
				Class = data.clas.name,
				Group = data.clas.group,
				Supercategory = data.clas.supercategory,
				Points = data.points.to_string(),
				Label = data.label,
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
						// TODO: 以下代码应当被优化
						rows[i].Type = data.type;
						rows[i].Class = data.clas.name;
						rows[i].Group = data.clas.group;
						rows[i].Supercategory = data.clas.supercategory;
						rows[i].Points = data.points.to_string();
						rows[i].Label = data.label;
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
		private void ListViewItem_PreviewMouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
			var item = sender as ListViewItem;
			if ( item != null ) {         // node 本身不为 null
				if ( item.DataContext != null ) {       // node 所关联的 datacontext 不为 null
					Row row = item.DataContext as Row;
					if ( row != null && row.elem != null ) {
						row.elem.Highlight();
					}
				}
			}
		}
	}
}
