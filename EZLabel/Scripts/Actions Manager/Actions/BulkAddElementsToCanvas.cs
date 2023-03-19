using QLabel.Scripts.AnnotationData;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLabel.Scripts.Projects;
using System.Windows.Controls;

namespace QLabel.Actions {
	internal class BulkAddElementsToCanvas : IAction {
		private readonly IEnumerable<IAnnotationElement> elements;
		private readonly MainCanvas canvas;
		private readonly bool add_ui_element_to_canvas;
		public string name => "Add Elements To Canvas";
		public BulkAddElementsToCanvas (
			MainCanvas canvas, IEnumerable<IAnnotationElement> elements, bool add_ui_element_to_canvas = false) {
			this.canvas = canvas;
			this.elements = elements;
			this.add_ui_element_to_canvas = add_ui_element_to_canvas;
		}

		public void Do () {
			foreach ( var element in elements ) {
				canvas.AddAnnoElements(element, add_ui_element_to_canvas);
				ProjectManager.AddAnnoData(ProjectManager.cur_datafile, element.data);
			}
		}

		public void Undo () {
			foreach ( var element in elements ) {
				canvas.RemoveAnnoElements(element);
				ProjectManager.RemoveAnnoData(ProjectManager.cur_datafile, element.data);
			}
		}
	}
}
