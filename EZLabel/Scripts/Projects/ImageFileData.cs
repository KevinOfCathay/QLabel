using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace QLabel.Scripts.Projects {
	public class ImageData {
		public string filename { get; set; } = string.Empty;
		public string path { get; set; } = string.Empty;
		public double width { get; set; }
		public double height { get; set; }
		public long size { get; set; }
		public List<AnnoData> data = new List<AnnoData>();

		public void ToXML (string path) {
			if ( path == null ) { return; }
			var xmls = new XmlSerializer(typeof(ImageData));
			try {
				TextWriter writer = new StreamWriter(path);
				xmls.Serialize(writer, this);
				writer.Close();
			} catch ( Exception ) { }
		}
	}
}