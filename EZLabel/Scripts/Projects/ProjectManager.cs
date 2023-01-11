using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	public class ProjectManager {
		public Project project;

		public Project CreateNewProject () {
			return new Project();
		}
		public void SaveProject () {
			foreach ( var data in project.data_list ) {
				data.ToXML(project.save_dir);
			}
		}

	}
}
