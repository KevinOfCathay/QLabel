using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace QLabel.Scripts.Projects {
	public class ImageData {
		private string _path = string.Empty;
		public string folder { get; private set; }
		public string filename { get; private set; }

		[XmlIgnore]
		public string path {
			get { return _path; }
			set { _path = value; folder = Directory.GetParent(value).FullName; filename = Path.GetFileName(path); }
		}

		public double width { get; set; }
		public double height { get; set; }
		public int depth { get; set; } = 3;
		[JsonConverter(typeof(StringEnumConverter))] public PixelFormat format { get; set; }

		public long size { get; set; }     // 文件大小
		public List<AnnoData> annodata { get; private set; } = new List<AnnoData>();
		private Dictionary<ClassLabel, int> label_counts = new Dictionary<ClassLabel, int>();  // 这个文件所包含的 class label

		#region Data
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
		private void WriteVOCObject (XmlWriter writer, AnnoData data) {
			writer.WriteStartElement("object");

			writer.WriteStartElement("name");
			writer.WriteString(data.clas.name);
			writer.WriteEndElement();

			WriteXMLAttr(writer, "truncated", data.truncated.ToString());
			WriteXMLAttr(writer, "occluded", data.occluded.ToString());

			writer.WriteStartElement("bndbox");
			WriteXMLAttr(writer, "xmin", data.bbox.tl.X.ToString());
			WriteXMLAttr(writer, "ymin", data.bbox.tl.Y.ToString());
			WriteXMLAttr(writer, "xmax", data.bbox.br.X.ToString());
			WriteXMLAttr(writer, "ymax", data.bbox.br.Y.ToString());
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		public void ToYoloXYWH (string save_path, ClassLabel[] labelset, bool percentage = true) {
			using ( StreamWriter sw = new StreamWriter(save_path) ) {
				foreach ( var data in annodata ) {
					var bbox = data.bbox;
					int class_index = Array.IndexOf(labelset, data.clas);
					if ( percentage ) {
						sw.WriteLine(string.Join(" ", class_index.ToString(),
							( ( ( bbox.br.X + bbox.tl.X ) / 2f ) / width ).ToString(), ( ( ( bbox.br.Y + bbox.tl.Y ) / 2f ) / height ).ToString(),
							( ( bbox.br.X - bbox.tl.X ) / width ).ToString(), ( ( bbox.br.Y - bbox.tl.Y ) / height ).ToString()
							));
					} else {
						sw.WriteLine(string.Join(" ", class_index.ToString(),
							( ( bbox.br.X + bbox.tl.X ) / 2f ).ToString(), ( ( bbox.br.Y + bbox.tl.Y ) / 2f ).ToString(),
							( bbox.br.X - bbox.tl.X ).ToString(), ( bbox.br.Y - bbox.tl.Y ).ToString()
							));
					}
				}
			}
		}
		public void ToYoloXYCoords (string save_path, ClassLabel[] labelset, bool percentage = true) {
			using ( StreamWriter sw = new StreamWriter(save_path) ) {
				foreach ( var data in annodata ) {
					int class_index = Array.IndexOf(labelset, data.clas);
					int len = data.rpoints.Length;
					string[] str = new string[len * 2 + 1]; str[0] = class_index.ToString();
					if ( percentage ) {
						for ( int i = 0; i < len; i += 1 ) {
							str[i * 2 + 1] = ( data.rpoints[i].X / width ).ToString();
							str[i * 2 + 2] = ( data.rpoints[i].Y / height ).ToString();
						}
					} else {
						for ( int i = 0; i < len; i += 1 ) {
							str[i * 2 + 1] = ( data.rpoints[i].X ).ToString();
							str[i * 2 + 2] = ( data.rpoints[i].Y ).ToString();
						}
					}
					sw.WriteLine(string.Join(" ", str));
				}
			}
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
				WriteVOCObject(writer, ad);
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}
		#endregion
	}
}