using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel {
	public class CreateAnnotationElementManual : IAction {
		private readonly IAnnotationElement element;
		private readonly MainCanvas canvas;
		public CreateAnnotationElementManual (IAnnotationElement element, MainCanvas canvas) {
			this.element = element;
			this.canvas = canvas;
		}
		public void Do () {
			canvas.AddAnnoElements(element);
			ProjectManager.AddAnnoData(ProjectManager.cur_datafile, element.data);
		}
		public void Undo () {
			canvas.RemoveAnnoElements(element);
			ProjectManager.RemoveAnnoData(ProjectManager.cur_datafile, element.data);
		}
	}
}
