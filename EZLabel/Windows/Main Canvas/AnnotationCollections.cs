using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Windows.Main_Canvas {
	public class AnnotationCollections {
		public List<IAnnotationElement> elements { get; private set; } = new List<IAnnotationElement>();

		public void AddElement (IAnnotationElement elem) {
			elements.Add(elem);
		}

		public void RemoveElement (IAnnotationElement elem) {
			elements.Remove(elem);
			elem.Delete();
		}
	}
}
