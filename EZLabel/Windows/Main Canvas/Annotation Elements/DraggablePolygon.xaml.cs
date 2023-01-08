﻿using QLabel.Scripts.AnnotationData;
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

namespace QLabel.Windows.Main_Canvas.Annotation_Elements {
	/// <summary>
	/// Interaction logic for DraggablePolygon.xaml
	/// </summary>
	public partial class DraggablePolygon : UserControl, IAnnotationElement {
		public DraggablePolygon () {
			InitializeComponent();
		}

		AnnoData _data;   // 这个多边形所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }
		private List<DraggableDot> dots = new List<DraggableDot>();

		public void Delete (MainCanvas canvas) {
			throw new NotImplementedException();
		}

		public void Draw (Canvas canvas, Point[] points) {
			throw new NotImplementedException();
		}
	}
}