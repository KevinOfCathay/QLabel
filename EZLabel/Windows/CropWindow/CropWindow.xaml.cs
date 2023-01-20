using QLabel.Scripts;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QLabel.Windows.CropWindow {
	/// <summary>
	/// Interaction logic for CropWindow.xaml
	/// </summary>
	public partial class CropWindow : Window {
		private enum CropTarget { Current, All };

		private CropTarget target = CropTarget.Current;
		public CropWindow () {
			InitializeComponent();
		}
		public void SetUI () {
		}
		private void ConfirmButton_Click (object sender, RoutedEventArgs e) {
			CropAndSave();
			CloseWindow();
		}
		private void CancelButton_Click (object sender, RoutedEventArgs e) {
			CloseWindow();
		}
		private void CloseWindow () {
			this.Close();
		}
		/// <summary>
		/// 裁剪并保存图像
		/// </summary>
		private void CropAndSave () {
			switch ( target ) {
				// 只裁剪当前的图像
				case CropTarget.Current:
					Crop(ProjectManager.cur_datafile);
					break;
				// 裁剪所有的图像
				case CropTarget.All:
					break;
				default:
					break;
			}
		}
		private void Crop (ImageData data) {

		}

		private void crop_target_SelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if ( crop_target.SelectedItem != null ) {
				target = (CropTarget) crop_target.SelectedIndex;
			}
		}
	}
}
