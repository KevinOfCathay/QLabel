﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QLabel.Custom_Control.Image_View;
using System.IO;
using QLabel.Windows.Main_Canvas;
using System.Diagnostics;
using QLabel.Scripts.AnnotationToolManager;

namespace QLabel {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			App.main = this;

			InitializeComponent();
			InitEvents();
			InitComponents();
		}

		public void InitEvents () {
			ilw.eImageListUICreated += (ImageListWindow window, ImageListItem item) => {
				item.eSwitchImage += (i) => {
					main_canvas.LoadImage(i.data);          // 点击 List 中的 image 图像来加载图片
				};
			};
		}
		public void InitComponents () {
			this.toolbar.Init(this.main_canvas);
			this.main_menu.Init(this);
		}

		private void Window_KeyDown (object sender, KeyEventArgs e) {
			if ( e.Key == Key.Z ) {
				Debug.WriteLine("触发了按键Z");
			}
		}
	}
}
