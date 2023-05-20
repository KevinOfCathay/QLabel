using Newtonsoft.Json;
using QLabel.Scripts.AnnotationData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal class ProjectManager {
		/// <summary> 当前是否有项目被加载 </summary>
		public bool empty { get { return project == null; } }
		/// <summary> 当前被加载项目的所有 image data </summary>
		public ICollection<ImageData>? datas { get { if ( !empty ) { return project.datas; } else { return null; } } }


		/// <summary>  当前打开的 project
		/// 这个 project 不应该被直接操作 (即，有关的操作应该在 manager 内部完成)  </summary>
		protected Project project { get; private set; }
		/// <summary>  当前 project 的 class labels 管理 </summary>
		public ClassLabelManager class_label_manager = new ClassLabelManager();
		/// <summary>  当前打开的文件夹的路径 </summary>
		private string cur_dir = null;
		private string save_dir = null;
		public ImageData cur_datafile;

		public ClassTemplate cur_label = new ClassTemplate("None", "None");
		public int cur_label_index = 0;
		public string PROJECT_DIR_NAME = "_project";
		public string VISUAL_DIR_NAME = "_vis";
		public string SAVE_JSON_NAME = "_saved_project";
		public string SAVE_CLASS_STATS_NAME = "_saved_classes";

		public event Action<ImageData, AnnoData>? eAnnoDataAdded, eAnnoDataRemoved;
		public bool NewProject (string directory) {
			if ( directory != cur_dir ) {
				if ( cur_dir != null && empty ) {
					try {
						project.SaveProjectAsync(Path.Join(save_dir, SAVE_JSON_NAME + ".json"));      // 保存当前打开的项目
					} catch {
						return false;        // 如果没有保存成功则直接返回
					}
				}
				cur_dir = directory;
				project = new Project(); // 创建一个新的项目
				class_label_manager.New();

				save_dir = Path.Join(cur_dir, PROJECT_DIR_NAME);
				if ( !Path.Exists(save_dir) ) {
					Directory.CreateDirectory(save_dir);
				}
			}
			return true;
		}
		public void AddImageData (ImageData data) {
			if ( !empty ) {
				project.AddImageData(data);
				foreach ( var anno in data.annodatas ) {
					class_label_manager.AddClassTemplate(anno.class_label.template);
				}
			}
		}
		public void AddAnnoData (ImageData imgdata, AnnoData annodata) {
			if ( imgdata != null && annodata != null && !imgdata.annodatas.Contains(annodata) ) {
				imgdata.AddAnnoData(annodata);   // 加入到 annodatas 中
				class_label_manager.AddClassTemplate(annodata.class_label.template);
				eAnnoDataAdded?.Invoke(imgdata, annodata);
			}
		}
		public void RemoveAnnoData (ImageData imgdata, AnnoData annodata) {
			if ( imgdata != null && annodata != null && imgdata.annodatas.Contains(annodata) ) {
				imgdata.RemoveAnnoData(annodata);   // 加入到 annodatas 中
				eAnnoDataRemoved?.Invoke(imgdata, annodata);
			}
		}
		public async Task VisualizeAsync (ImageData data) {
			await Task.Run(async () => {
				if ( data.annodatas.Count == 0 ) { return; }
				var imagetask = ImageUtils.ReadBitmapAsync(data.path);
				var dir_path = Path.Join(cur_dir, VISUAL_DIR_NAME);
				if ( !Path.Exists(dir_path) ) {
					Directory.CreateDirectory(dir_path);
				}
				var save_path = Path.Join(dir_path, data.filename + ".png");
				// 首先对所有的 anno 排序
				var annos = data.annodatas.ToArray();
				Array.Sort(annos, (a, b) => {
					if ( a.visualize_priority > b.visualize_priority ) { return 1; } else if ( a.visualize_priority == b.visualize_priority ) { return 0; } else { return -1; }
				});

				var image = await imagetask;
				foreach ( var anno in annos ) {
					anno.Visualize(image, new Vector2(image.Width, image.Height), Color.Chocolate);
				}
				await ImageUtils.WriteBitmapAsync(image, save_path);
				image.Dispose();
			});
		}

		#region Save
		public async Task SaveProjectAsync () {
			Task save_proj_task = project.SaveProjectAsync(Path.Join(save_dir, SAVE_JSON_NAME + ".json"));
			Task save_class_stats_task = class_label_manager.SaveLabelStatisticsAsync(Path.Join(save_dir, SAVE_CLASS_STATS_NAME + ".json"));

			await save_proj_task;
			await save_class_stats_task;
		}
		#endregion Save

		#region Load
		public async Task LoadProjectAsync (string path) {
			// 清除之前的 Project
			if ( project != null ) { await project.SaveProjectAsync(Path.Join(save_dir, SAVE_JSON_NAME + ".json")); }

			// 建立一个 string --> Enum 的表格
			Dictionary<string, PixelFormat> table = new Dictionary<string, PixelFormat>();
			foreach ( PixelFormat fmt in Enum.GetValues(typeof(PixelFormat)) ) {
				table[fmt.ToString()] = fmt;
			}
			await Task.Run(() => {
				JsonSerializer serializer = new JsonSerializer();
				using ( JsonReader reader = new JsonTextReader(new StreamReader(path)) ) {
					dynamic json_obj = serializer.Deserialize<dynamic>(reader);
					if ( json_obj == null ) { return; }
					var datas = json_obj.datas; var labels = json_obj.labels;        // 读取 datas 和 templates
					List<ImageData> image_datas = new List<ImageData>(datas.Count);
					foreach ( var data in datas ) {
						// 从 json 中复原 ImageData 并加入到 list 中
						var imgdata = new ImageData {
							path = data.path,
							size = data.size,
							width = data.width,
							height = data.height,
							depth = data.depth,
							format = table[data.format.Value]
						};
						IEnumerable<AnnoData> annodatas = MakeAnnoData(data.annodata);
						if ( annodatas != null ) {
							foreach ( var annodata in annodatas ) {
								imgdata.AddAnnoData(annodata);
							}
						}
						image_datas.Add(imgdata);
					}
					foreach ( var data in image_datas ) {
						project.AddImageData(data);
					}
				}
			});
		}
		private IEnumerable<AnnoData> MakeAnnoData (dynamic datas) {
			if ( datas.Count == 0 ) { return Array.Empty<AnnoData>(); }
			List<AnnoData> annodatas = new List<AnnoData>();

			foreach ( var data in datas ) {
				var rps = data.rpoints;
				Vector2[] rpoints = new Vector2[rps.Count];
				for ( int i = 0; i < rps.Count; i += 1 ) {
					var p = rps[i];
					rpoints[i] = new Vector2((float) p.X.Value, (float) p.Y.Value);
				}
				var clas = data.clas;
				ClassTemplate clabel = new ClassTemplate(clas.group.Value, clas.name.Value, clas.supercategory.Value);
				string caption = data.caption.Value;
				Guid guid = Guid.Parse(data.guid.Value);
				DateTime createtime = data.createtime.Value;
				float conf = (float) data.conf.Value;
				bool truncated = data.truncated.Value;
				bool occluded = data.occluded.Value;
				switch ( data.type.Value ) {
					case "Dot":
						// 读取点相关联的线 ID
						var addot = new ADDot(
							rpoints[0], new ClassLabel(clabel) { confidence = conf }, new List<Guid>(),
							guid, createtime) { caption = caption, truncated = truncated, occluded = occluded };
						annodatas.Add(addot);
						break;
					case "Rectangle":
						var adrect = new ADRect(
							new ReadOnlySpan<Vector2>(rpoints),
							new ClassLabel(clabel) { confidence = conf }, guid, createtime, false) { caption = caption, truncated = truncated, occluded = occluded };
						annodatas.Add(adrect);
						break;
					case "Polygon":
						break;
					case "Line":
						// 读取线相关联的点 ID
						Guid dot_a_id = Guid.Parse(data.dot_a_id.Value);
						Guid dot_b_id = Guid.Parse(data.dot_b_id.Value);
						var adline = new ADLine(
							rpoints[0], rpoints[1], dot_a_id, dot_b_id,
							new ClassLabel(clabel) { confidence = conf }, guid, createtime) { caption = caption, truncated = truncated, occluded = occluded };
						annodatas.Add(adline);
						break;
					case "Circle":
						break;
					default:
						throw new NotImplementedException("Annodata's type is not implemented");
				}
			}
			return annodatas;
		}
		#endregion Load
	}
}
