using QLabel.Scripts.AnnotationData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QLabel.Scripts.Projects {
	public class ImageData {
		private string _path = string.Empty;
		public string folder { get; private set; }
		public string filename { get; private set; }

		public string path {
			get { return _path; }
			set {
				_path = value; folder = Directory.GetParent(value).FullName; filename = Path.GetFileName(path);
			}
		}
		public double width { get; set; }
		public double height { get; set; }
		public int depth { get; set; } = 3;
		public PixelFormat format { get; set; }

		public long size { get; set; }     // 文件大小
		private List<AnnoData> annodata = new List<AnnoData>();
		private Dictionary<ClassLabel, int> label_counts = new Dictionary<ClassLabel, int>();  // 这个文件所包含的 class label

		#region Data
		public IEnumerable<AnnoData> GetAnnoData () {
			return annodata;
		}
		public void AddAnnoData (AnnoData data) {
			annodata.Add(data);
			if ( label_counts.ContainsKey(data.clas) ) {
				label_counts[data.clas] += 1;
			} else {
				label_counts[data.clas] = 1;
			}
		}
		public void RemoveAnnoData (AnnoData data) {
			annodata.Remove(data);
			if ( label_counts.ContainsKey(data.clas) ) {
				label_counts[data.clas] -= 1;
				if ( label_counts[data.clas] <= 0 ) { label_counts.Remove(data.clas); }
			}
		}
		public Dictionary<ClassLabel, int> GetLabelCounts () {
			return label_counts;
		}
		#endregion

		#region Export
		private void WriteXMLAttr (XmlWriter writer, string attr, string text) {
			writer.WriteStartElement(attr);
			writer.WriteString(text);
			writer.WriteEndElement();
		}
		private void WriteVOCObject (XmlWriter writer, string name, int xmin, int ymin, int xmax, int ymax) {
			writer.WriteStartElement("object");

			writer.WriteStartElement("name");
			writer.WriteString(name);
			writer.WriteEndElement();

			writer.WriteStartElement("bndbox");
			WriteXMLAttr(writer, "xmin", xmin.ToString());
			WriteXMLAttr(writer, "ymin", ymin.ToString());
			WriteXMLAttr(writer, "xmax", xmax.ToString());
			WriteXMLAttr(writer, "ymax", ymax.ToString());
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		public void ToVOC (string save_path) {
			XmlWriter writer = XmlWriter.Create(save_path);
			writer.WriteStartDocument();
			writer.WriteStartElement("annotation");

			WriteXMLAttr(writer, "folder", folder);
			WriteXMLAttr(writer, "filename", filename);
			WriteXMLAttr(writer, "path", path);

			writer.WriteStartElement("size");
			WriteXMLAttr(writer, "width", width.ToString());
			WriteXMLAttr(writer, "height", height.ToString());
			WriteXMLAttr(writer, "depth", depth.ToString());
			writer.WriteEndElement();

			foreach ( var ad in annodata ) {
				WriteVOCObject(writer, ad.clas.name, (int) ad.bbox.tl.X, (int) ad.bbox.tl.Y, (int) ad.bbox.br.X, (int) ad.bbox.br.Y);
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}
		#endregion
	}
}