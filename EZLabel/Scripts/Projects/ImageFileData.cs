using QLabel.Scripts.AnnotationData;
using System;
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
		public PixelFormat format { get; set; }

		public long size { get; set; }     // 文件大小
		public List<AnnoData> annodata = new List<AnnoData>();

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

			writer.WriteEndElement();
			WriteXMLAttr(writer, "width", width.ToString());
			WriteXMLAttr(writer, "height", height.ToString());
			writer.WriteEndDocument();

			foreach ( var ad in annodata ) {
				WriteVOCObject(writer, ad.clas.name, (int) ad.bbox.tl.X, (int) ad.bbox.tl.Y, (int) ad.bbox.br.X, (int) ad.bbox.br.Y);
			}
			writer.Close();
		}
		#endregion

		#region Serialize
		public void ToXML (string path) {
			if ( path == null ) { return; }
			var xmls = new XmlSerializer(typeof(ImageData));
			try {
				TextWriter writer = new StreamWriter(path);
				xmls.Serialize(writer, this);
				writer.Close();
			} catch ( Exception ) { }
		}
		#endregion
	}
}