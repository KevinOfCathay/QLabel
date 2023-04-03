using QLabel.Actions;
using QLabel.Custom_Control.Label_Tree;
using QLabel.Custom_Control.Small_Tools;
using QLabel.Scripts;
using QLabel.Scripts.Inference_Machine;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace QLabel.Windows.Popup_Windows.Auto_Window {
	public partial class AutoWindow : Window {
		private MainWindow main;
		private MainCanvas canvas;
		private BaseInferenceMachine selected_machine;
		private HashSet<int> accepted_classes = new HashSet<int>();
		private HashSet<ImageData> accepted_files = new HashSet<ImageData>();
		internal event Action<BaseInferenceMachine>? eRunBefore, eRunAfter;

		public AutoWindow () {
			InitializeComponent();
			InitializeListItems();
		}
		internal AutoWindow (
			MainWindow main, MainCanvas canvas,
			Action<BaseInferenceMachine>? eRunBefore = null, Action<BaseInferenceMachine>? eRunAfter = null) {
			InitializeComponent();
			this.main = main;
			this.canvas = canvas;
			if ( eRunBefore != null ) { this.eRunBefore += eRunBefore; }
			if ( eRunAfter != null ) {
				this.eRunAfter += eRunAfter;
			}
			confirm_cancel.eConfirmClick += Run;
			confirm_cancel.eConfirmClick += (_, _) => { this.Close(); };
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
		/// <summary>
		/// 当前的 inference 只适用于当前被打开的图像
		/// </summary>
		private async void Run (object sender, RoutedEventArgs e) {
			if ( selected_machine == null ) { return; }

			selected_machine.BuildSession();
			if ( canvas != null && canvas.can_annotate ) {


				if ( accepted_classes.Count == 0 ) { return; }
				var bitmap = ImageUtils.GetBitmapFromPath(ProjectManager.cur_datafile.path);
				eRunBefore?.Invoke(selected_machine);
				var ads = selected_machine.RunInference(bitmap, accepted_classes);
				eRunAfter?.Invoke(selected_machine);
				List<IAnnotationElement> elements = new List<IAnnotationElement>(ads.Length);
				foreach ( var ad in ads ) {
					var element = ad.CreateAnnotationElement(canvas);
					elements.Add(element);
					ProjectManager.AddAnnoData(ProjectManager.cur_datafile, ad);
				}
				BulkAddElementsToCanvas bulk_add_elements = new BulkAddElementsToCanvas(canvas, elements, add_ui_element_to_canvas: true);
				ActionManager.PushAndExecute(bulk_add_elements);
			}
		}
	}
}
