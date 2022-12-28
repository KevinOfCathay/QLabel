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
	/// Interaction logic for Line.xaml
	/// </summary>
	public partial class Line : UserControl, IAnnotationElement {
		public Line () {
			InitializeComponent();
		}

		AnnoData _data;   // 这个矩形所对应的注释数据
		public AnnoData data { get { return _data; } set { _data = value; } }

		public void Delete () {
			throw new NotImplementedException();
		}

		public void Draw (Canvas canvas, Point[] points) {
			if ( points != null && points.Length >= 1 ) {
				this.line.X1 = points[0].X;
				this.line.Y1 = points[0].Y;
				this.line.X2 = points[1].X;
				this.line.Y2 = points[1].Y;
			}
		}
	}
}
