using EZLabel.Scripts.AnnotationData;
using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using EZLabel.Windows.Main_Canvas;
using OpenCvSharp.Flann;
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
using System.Collections.ObjectModel;
using QLabel.Scripts.Utils;

namespace EZLabel.Windows.Annotation_List.Anno_List {
	/// <summary>
	/// Interaction logic for AnnoList.xaml
	/// </summary>
	public partial class AnnoList : UserControl {
		public record class Row {
			public string Index { get; set; }
			public string Type { get; set; }
			public string Class { get; set; }
			public string Label { get; set; }
			public string Points { get; set; }
		}
		public ObservableCollection<Row> rows { get; } = new ObservableCollection<Row>();

		public AnnoList () {
			InitializeComponent();
			RegisterEvents();
		}

		/// <summary>
		/// 在列表中加入一行
		/// </summary>
		public void AddItemToList (AnnoData data) {
			var row = new Row {
				Index = this.listview.Items.Count.ToString(),
				Type = data.type.ToString(),
				Class = data.clas.ToString(),
				Points = data.points.to_string(),
				Label = data.label
			};
			rows.Add(row);
		}
		/// <summary>
		/// 清除列表中所有的内容
		/// </summary>
		public void ClearList () {
			this.listview.Items.Clear();
		}
		public void RegisterEvents () {
			var canvas = QLabel.App.main.main_canvas;
			if ( canvas != null ) {
				canvas.eAnnotationElementAdded += (MainCanvas mc, IAnnotationElement iae) => {
					var annodata = iae.data;
					AddItemToList(annodata);
				};
			}
		}
	}
}
