using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.Inference_Machine;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace QLabel.Windows.Misc_Panel.Sub_Panels {
	/// <summary>
	/// Interaction logic for AutoAnnotatePanel.xaml
	/// </summary>
	public partial class AutoAnnotatePanel : UserControl {
		private BaseInferenceMachine selected_machine;
		private MainCanvas canvas;         // TODO: replace this
		private HashSet<int> accepted_classes;
		private CheckboxWithLabel[] cbxlabels;

		public AutoAnnotatePanel () {
			InitializeComponent();

			if ( App.main != null ) {     // TODO: replace this
				canvas = App.main.main_canvas;
			}

			InitializeListItems();
		}
		/// <summary>
		/// 动态的初始化列表中的所有元素
		/// </summary>
		private void InitializeListItems () {
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
				accepted_classes = new HashSet<int>();
				var labels = config.class_labels;
				var tags = config.tags;

				ComboBoxItem item = new ComboBoxItem();
				item.Content = config.model_name;

				item.Selected += delegate (object sender, RoutedEventArgs e) {
					SetClassLabels(labels);
					SetTags(tags);
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
		private void SetClassLabels (ClassLabel[] labels) {
			// 清空 class list 中的所有元素
			class_list.Items.Clear();
			cbxlabels = new CheckboxWithLabel[labels.Length];

			// 将新的元素加入到 class list 中
			for ( int i = 0; i < labels.Length; i += 1 ) {
				var label = labels[i];
				var new_item = new ListBoxItem();

				var new_cbxlabel = new CheckboxWithLabel();
				new_cbxlabel.label.Content = label.name;
				int class_index = i;
				new_cbxlabel.eChecked += (_, _) => { accepted_classes.Add(class_index); };
				new_cbxlabel.eUnchecked += (_, _) => { accepted_classes.Remove(class_index); };
				// 默认设置为选中状态
				new_cbxlabel.Check();

				new_item.Content = new_cbxlabel;
				class_list.Items.Add(new_item);
				cbxlabels[i] = new_cbxlabel;  // 将创建的元素加入到数组中
			}
		}
		/// <summary>
		/// 当前的 inference 只适用于当前被打开的图像
		/// </summary>
		private void ApplyClick (object sender, RoutedEventArgs e) {
			if ( selected_machine != null ) {
				selected_machine.BuildSession();
				if ( canvas != null && canvas.can_annotate ) {
					var ads = selected_machine.RunInference(ProjectManager.cur_datafile, accepted_classes);
					foreach ( var ad in ads ) {
						var element = ad.CreateAnnotationElement(canvas);
						canvas.AddAnnoElements(element);
						ProjectManager.AddAnnoData(ProjectManager.cur_datafile, ad);
					}
				}
			}
		}
		private void ApplyAllClick (object sender, RoutedEventArgs e) {
			if ( selected_machine != null ) {
				selected_machine.BuildSession();
				if ( canvas != null && canvas.can_annotate ) {
					foreach ( var file in ProjectManager.project.data_list ) {
						var ads = selected_machine.RunInference(file, accepted_classes);
						foreach ( var ad in ads ) {
							ProjectManager.AddAnnoData(file, ad);
							if ( file == ProjectManager.cur_datafile ) {
								var element = ad.CreateAnnotationElement(canvas);
								canvas.AddAnnoElements(element);
							}
						}
					}
				}
			}
		}
		private void UnselectAllClick (object sender, RoutedEventArgs e) {
			foreach ( var label in cbxlabels ) {
				label.Uncheck();
			}
		}
		private void SelectAllClick (object sender, RoutedEventArgs e) {
			foreach ( var label in cbxlabels ) {
				label.Check();
			}
		}
	}
}
