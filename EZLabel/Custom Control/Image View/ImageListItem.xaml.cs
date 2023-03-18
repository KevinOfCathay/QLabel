﻿using System;
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
using QLabel.Scripts.Projects;

namespace QLabel.Custom_Control.Image_View {
	/// <summary>
	/// Interaction logic for ImageListItem.xaml
	/// </summary>
	public partial class ImageListItem : UserControl {
		private static readonly SolidColorBrush border_highlight = new SolidColorBrush(Color.FromArgb(255, 255, 190, 155));
		private static readonly SolidColorBrush text_highlight = new SolidColorBrush(Color.FromArgb(255, 180, 96, 96));
		private static readonly SolidColorBrush border_normal = new SolidColorBrush(Color.FromArgb(255, 211, 211, 211));
		private static readonly SolidColorBrush text_normal = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
		public ImageData data;        // 文件路径

		public ImageListItem () {
			InitializeComponent();
		}
		public void Highlight () {
			bdr.BorderBrush = border_highlight;
			image_name.Foreground = text_highlight;
		}
		public void UnHighlight () {
			bdr.BorderBrush = border_normal;
			image_name.Foreground = text_normal;
		}
	}
}
