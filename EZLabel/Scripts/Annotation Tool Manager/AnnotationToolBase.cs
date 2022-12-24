﻿using EZLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZLabel.Scripts.AnnotationToolManager {
	public abstract class ToolBase {
		public abstract void Activate (MainCanvas canvas);
		public abstract void Deactivate (MainCanvas canvas);
	}
}
