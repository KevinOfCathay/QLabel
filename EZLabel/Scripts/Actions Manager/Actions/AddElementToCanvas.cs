﻿using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Actions {
	internal class AddElementToCanvas : IAction {
		private readonly IAnnotationElement element;
		private readonly MainCanvas canvas;
		private readonly bool add_ui_element_to_canvas;
		public string name => "Add Element To Canvas";
		public AddElementToCanvas (MainCanvas canvas, IAnnotationElement element, bool add_ui_element_to_canvas = false) {
			this.element = element;
			this.canvas = canvas;
			this.add_ui_element_to_canvas = add_ui_element_to_canvas;
		}
		public void Do () {
			canvas.AddAnnoElement(element, add_ui_element_to_canvas);
			App.project_manager.AddAnnoData(App.project_manager.cur_datafile, element.data);
		}
		public void Undo () {
			canvas.RemoveAnnoElements(element);
			App.project_manager.RemoveAnnoData(App.project_manager.cur_datafile, element.data);
		}
	}
}
