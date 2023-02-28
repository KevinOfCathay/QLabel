using QLabel.Properties;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace QLabel.Windows.Import_Window {
	public partial class ImportWindow : Window {
		private HashSet<string> accepted_ext = new HashSet<string> { ".bmp", ".jpg", ".png", ".jpeg" };
		private string[] GetVOCFiles (string path) {
			var paths = Directory.GetFiles(path);
			int len = paths.Length; int count = 0;
			Span<string> xmls = new Span<string>(new string[len]);
			for ( int i = 0; i < len; i += 1 ) {
				if ( Path.GetExtension(paths[i]) == ".xml" ) {
					xmls[count] = paths[i];
					count += 1;
				}
			}
			return xmls.Slice(0, count).ToArray();
		}
		private async Task<ImageData[]> ImportFromVOCAsync (string[] paths, string? local_path) {
			return await Task.Run(() => { return ImportFromVOC(paths, local_path); });
		}
		private ImageData[] ImportFromVOC (string[] paths, string? local_path) {
			List<ImageData> data_list = new List<ImageData>(capacity: paths.Length);
			foreach ( var path in paths ) {
				if ( Path.Exists(path) ) {
					try {
						var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
						XmlDocument doc = new XmlDocument();
						doc.Load(stream);

						ImageData data = new ImageData();
						// 获取文件路径
						var elems = doc.GetElementsByTagName("path");
						if ( elems != null && elems.Count > 0 ) {
							var p = elems[0].InnerText;

							// 路径为图像时判断路径是否存在，否则直接进入下一个
							if ( accepted_ext.Contains(Path.GetExtension(p)) ) {
								if ( Path.Exists(p) ) {
									data.path = p;
								} else if ( local_path != null ) {
									var filename = Path.GetFileName(p);
									var local = Path.Join(local_path, filename);
									if ( Path.Exists(local) ) {
										data.path = local;
									} else { continue; }
								} else { continue; }
							} else { continue; }
						} else { continue; }

						// 获取图片尺寸
						var w = doc.GetElementsByTagName("width");
						var h = doc.GetElementsByTagName("height");
						int width, height;
						if ( w != null && w.Count > 0 ) { width = int.Parse(w[0].InnerText); } else { continue; }
						if ( h != null && h.Count > 0 ) { height = int.Parse(h[0].InnerText); } else { continue; }
						data.width = width; data.height = height;

						// 获取所有的边框
						var objs = doc.GetElementsByTagName("object");
						if ( objs != null && objs.Count > 0 ) {
							foreach ( var obj in objs ) {
								Span<bool> has_element = stackalloc bool[5];
								string box_name = string.Empty;
								float xmin = 0f, ymin = 0f, xmax = 0f, ymax = 0f;

								var attrs = ( obj as XmlElement ).ChildNodes;
								foreach ( var attr in attrs ) {
									var node = attr as XmlElement;
									if ( node.Name.ToLower() == "name" ) {
										box_name = node.InnerText;
										has_element[0] = true;
									} else if ( node.Name.ToLower() == "bndbox" ) {
										foreach ( var coord in node ) {
											var name = ( coord as XmlElement ).Name;
											var content = ( coord as XmlElement ).InnerText;
											switch ( name.ToLower() ) {
												case "xmin":
													xmin = float.Parse(content);
													has_element[1] = true;
													break;
												case "ymin":
													ymin = float.Parse(content);
													has_element[2] = true;
													break;
												case "xmax":
													xmax = float.Parse(content);
													has_element[3] = true;
													break;
												case "ymax":
													ymax = float.Parse(content);
													has_element[4] = true;
													break;
												default:
													break;
											}
										}
									}
								}
								if ( has_element[0] && has_element[1] && has_element[2] && has_element[3] ) {
									ADRect rect = new ADRect(xmin, ymin, xmax, ymax, new Scripts.ClassLabel("None", box_name));
									data.AddAnnoData(rect);
								}
							}
						}
						data_list.Add(data);
					} catch ( Exception ) { }
				}
			}
			return data_list.ToArray();
		}
	}
}