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
		readonly IAnnotationElement element;
		readonly MainCanvas cvs;
		public CreateAnnotationElementManual (IAnnotationElement element, MainCanvas canvas) {
			this.element = element;
			this.cvs = canvas;
		}
		public void Do () {
			cvs.AddAnnoElements(element);
		}
		public void Undo () {
			cvs.RemoveAnnoElements(element);
		}
	}
}
