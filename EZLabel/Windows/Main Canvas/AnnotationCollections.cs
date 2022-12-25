using EZLabel.Windows.Main_Canvas.Annotation_Elements;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Windows.Main_Canvas
{
    public class AnnotationCollections {
		public Dictionary<ImageFileData, List<IAnnotationElement>> elements { get; private set; } =
			new Dictionary<ImageFileData, List<IAnnotationElement>>();

		public void AddElement (ImageFileData file, IAnnotationElement elem) {
			if ( !elements.ContainsKey(file) ) {         // 如果没有创建过 list 则需要创建一个 list
				elements[file] = new List<IAnnotationElement>(10);
			}
			elements[file].Add(elem);
		}
		public void RemoveElement (ImageFileData file, IAnnotationElement elem) {
			if ( !elements.ContainsKey(file) ) {         // 如果没有创建过 list 则需要创建一个 list
				elements[file].Remove(elem);
			}
		}
	}
}
