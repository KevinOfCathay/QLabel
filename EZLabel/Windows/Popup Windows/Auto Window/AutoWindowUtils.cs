using QLabel.Actions;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace QLabel.Windows.Popup_Windows.Auto_Window {
	public partial class AutoWindow : Window {
		private async void Run (object sender, RoutedEventArgs e) {
			if ( selected_machine == null ) { return; }

			selected_machine.BuildSession();
			if ( canvas != null && canvas.can_annotate ) {
				if ( accepted_classes.Count == 0 ) { return; }
				foreach ( var image_data in accepted_image_datas ) {
					eRunBefore?.Invoke(selected_machine);
					// 将深度模型内置的所有 class 加入到 project 中
					App.project_manager.class_label_manager.AddClassTemplates(selected_machine.templates);

					var run_task = selected_machine.Run(image_data, accepted_classes);
					eRunAfter?.Invoke(selected_machine);

					var ads = await run_task;
					List<IAnnotationElement> elements = new List<IAnnotationElement>(ads.Length);
					foreach ( var ad in ads ) {
						var element = ad.CreateAnnotationElement(canvas);
						elements.Add(element);
						App.project_manager.AddAnnoData(image_data, ad);
					}
					bool add_to_canvas = ( image_data == App.project_manager.cur_datafile );
					BulkAddElementsToCanvas bulk_add_elements = new BulkAddElementsToCanvas(canvas, elements, add_ui_element_to_canvas: add_to_canvas);
					ActionManager.PushAndExecute(bulk_add_elements);
				}
			}
		}
	}
}