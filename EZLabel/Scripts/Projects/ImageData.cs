﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace QLabel.Scripts.Projects {
	public class ImageData {
		// ImageData 相关 Properties
		public string path {
			get { return _path; }
			set { _path = value; folder = Directory.GetParent(value).FullName; filename = Path.GetFileName(path); }
		}

		// ImageData 相关属性 
		private string _path = string.Empty;
		public string folder { get; private set; }
		public string filename { get; private set; }

		public double width { get; set; }
		public double height { get; set; }
		public int depth { get; set; } = 3;
		[JsonConverter(typeof(StringEnumConverter))] public PixelFormat format { get; set; }

		public long size { get; set; }     // 文件大小
		public List<AnnoData> annodatas { get; private set; } = new List<AnnoData>();

		public ImageData () { }

		#region Data
		public void AddAnnoData (AnnoData data) {
			if ( data != null ) {
				annodatas.Add(data);
			}
		}
		public void RemoveAnnoData (AnnoData data) {
			annodatas.Remove(data);
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
			writer.WriteString(data.class_label.template.name);
			writer.WriteEndElement();

			WriteXMLAttr(writer, "truncated", data.truncated.ToString());
			WriteXMLAttr(writer, "occluded", data.occluded.ToString());

			writer.WriteStartElement("bndbox");
			WriteXMLAttr(writer, "xmin", data.bbox.Left.ToString());
			WriteXMLAttr(writer, "ymin", data.bbox.Top.ToString());
			WriteXMLAttr(writer, "xmax", data.bbox.Right.ToString());
			WriteXMLAttr(writer, "ymax", data.bbox.Bottom.ToString());
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		public void ToYoloXYWH (string save_path, ClassTemplate[] labelset, bool percentage = true) {
			using ( StreamWriter sw = new StreamWriter(save_path) ) {
				foreach ( var data in annodatas ) {
					var bbox = data.bbox;
					var label = data.class_label;
					int class_index = Array.FindIndex(labelset, (temp) => { return label.template == temp; });
					if ( percentage ) {
						sw.WriteLine(string.Join(" ", class_index.ToString(),
							( ( ( bbox.Width ) / 2f ) / width ).ToString(), ( ( ( bbox.Height ) / 2f ) / height ).ToString(),
							( ( bbox.Width ) / width ).ToString(), ( ( bbox.Height ) / height ).ToString()
							));
					} else {
						sw.WriteLine(string.Join(" ", class_index.ToString(),
							( ( bbox.Width ) / 2f ).ToString(), ( ( bbox.Height ) / 2f ).ToString(),
							( bbox.Width ).ToString(), ( bbox.Height ).ToString()
							));
					}
				}
			}
		}
		public void ToYoloXYCoords (string save_path, ClassTemplate[] labelset, bool percentage = true) {
			using ( StreamWriter sw = new StreamWriter(save_path) ) {
				foreach ( var data in annodatas ) {
					var label = data.class_label;
					int class_index = Array.FindIndex(labelset, (temp) => { return label.template == temp; });
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

			foreach ( var ad in annodatas ) {
				WriteVOCObject(writer, ad);
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}
		#endregion
	}
}