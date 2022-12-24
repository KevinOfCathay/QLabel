using EZLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Scripts.AnnotationToolManager {
	public class ToolManager {
		private ToolBase tool;
		public MainCanvas mc { private get; set; }

		public void SwitchTool (ToolBase tool) {
			if ( mc != null ) {
				if ( this.tool != null ) { this.tool.Deactivate(mc); }
				this.tool = tool;
				this.tool.Activate(mc);
			}
		}
	}
}
