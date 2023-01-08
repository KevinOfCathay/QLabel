using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel.Scripts.AnnotationToolManager {
	public abstract class ToolBase {
		public abstract void Activate (MainCanvas canvas);
		public abstract void Deactivate (MainCanvas canvas);
	}
}
