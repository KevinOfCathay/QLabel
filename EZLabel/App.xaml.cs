using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		internal static ProjectManager project_manager = new ProjectManager();
		public static MainWindow main;
	}
}
