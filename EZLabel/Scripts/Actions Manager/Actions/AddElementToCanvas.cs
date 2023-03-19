using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Actions {
	public class AddElementToCanvas : IAction {
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
			canvas.AddAnnoElements(element, add_ui_element_to_canvas);
			ProjectManager.AddAnnoData(ProjectManager.cur_datafile, element.data);
		}
		public void Undo () {
			canvas.RemoveAnnoElements(element);
			ProjectManager.RemoveAnnoData(ProjectManager.cur_datafile, element.data);
		}
	}
}
