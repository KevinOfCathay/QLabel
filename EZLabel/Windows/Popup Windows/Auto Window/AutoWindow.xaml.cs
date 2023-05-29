using QLabel.Actions;
using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.Inference_Machine;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Windows.Popup_Windows.Auto_Window {
	public partial class AutoWindow : Window {
		private MainWindow main;
		private MainCanvas canvas;
		private InferenceBase selected_machine;
		private HashSet<int> accepted_classes = new HashSet<int>();
		private HashSet<ImageData> accepted_image_datas = new HashSet<ImageData>();
		internal event Action<InferenceBase>? eRunBefore, eRunAfter;

		public AutoWindow () {
			InitializeComponent();
			SetListItems();
		}
		internal AutoWindow (
			MainWindow main, MainCanvas canvas,
			Action<InferenceBase>? eRunBefore = null, Action<InferenceBase>? eRunAfter = null) {
			InitializeComponent();

			this.main = main;
			this.canvas = canvas;
			if ( eRunBefore != null ) { this.eRunBefore += eRunBefore; }
			if ( eRunAfter != null ) {
				this.eRunAfter += eRunAfter;
			}
			confirm_cancel.ConfirmClick += Run;
			confirm_cancel.ConfirmClick += (_, _) => { this.Close(); };
			SetListItems(); SetFileTree();
		}

		/// <summary>
		/// 初始化文件树
		/// </summary>
		private void SetFileTree () {
			filetree.SetUI(App.project_manager.datas,
				check: delegate (ImageData data) { accepted_image_datas.Add(data); },
				uncheck: delegate (ImageData data) { accepted_image_datas.Remove(data); }
			);
		}

		/// <summary>
		/// 动态的初始化列表中的所有元素
		/// </summary>
		private void SetListItems () {
			// 读取所有的 model sets
			foreach ( var set in ModelSets.sets ) {
				ListBoxItem item = new ListBoxItem();
				item.Content = set.name;

				item.Selected += (object sender, RoutedEventArgs e) => { SetModelVersions(set); };
				model_list.Items.Add(item);
			}
		}
		private void SetModelVersions (ModelSet set) {
			// 清除之前的内容
			model_version_list.Items.Clear();
			bool selected = false;

			foreach ( var config in set.model_configs ) {
				// 针对每个 config, 设置 UI
				var labels = config.class_labels;
				var tags = config.tags;

				ComboBoxItem item = new ComboBoxItem();
				item.Content = config.model_name;

				// 选择某个特定的模型时，触发以下事件
				item.Selected += (object sender, RoutedEventArgs e) => {
					accepted_classes.Clear();
					SetTags(tags);
					labeltree.SetUI(labels,
						(clabel) => { accepted_classes.Add(Array.IndexOf(labels, clabel)); },
						(clabel) => { accepted_classes.Remove(Array.IndexOf(labels, clabel)); },
						expanded: true
					);
					selected_machine = config.inf;
				};
				if ( !selected ) { item.IsSelected = true; selected = !selected; }
				model_version_list.Items.Add(item);
			}
		}
		private void SetTags (string[] tags) {
			// 清空 tag list 中的所有元素
			tag_list.Children.Clear();

			// 将新的元素加入到 class list 中
			for ( int i = 0; i < tags.Length; i += 1 ) {
				var tag = tags[i];

				RoundedLabel rlabel = new RoundedLabel();
				rlabel.label.Content = tag;
				tag_list.Children.Add(rlabel);
			}
		}
	}
}
