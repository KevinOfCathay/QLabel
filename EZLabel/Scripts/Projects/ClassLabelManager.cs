using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Projects {
	internal class ClassLabelManager {
		/// <summary> Project 下所有数据的类别标签统计 </summary>
		private HashSet<ClassTemplate> templates = new HashSet<ClassTemplate>();
		public ICollection<ClassTemplate> label_set_full { get { return templates; } }

		/// <summary> Project 下所有数据的类别标签统计 </summary>
		private List<ClassLabelStat> label_stats = new List<ClassLabelStat>();
		public ICollection<ClassTemplate> label_set_used { get { return Utils.GetLabelSet(label_stats); } }

		/// <summary> 刷新 / 创建一个新的 Manager </summary>
		public void New () {
			label_stats.Clear();
		}

		public void AddClassTemplates (ClassTemplate[] templates) {
			foreach ( var template in templates ) {
				this.templates.Add(template);
			}
		}

		/// <summary>
		/// 这个类别标签可以是使用过/出现过的
		/// 也可以是未使用的（例如，自动识别时，一些类从来都没有被使用过，或者用户自己加入到类别集合中）
		/// </summary>
		public void AddClassTemplate (ClassTemplate template) {
			templates.Add(template);
		}

		public void AddClassLabels (ClassLabel[] labels) {
			foreach ( var label in labels ) {
				AddClassLabel(label);
			}
		}
		/// <summary>
		/// 增加一个类别标签
		/// 这个类别标签必须是被使用过的
		/// </summary>
		public void AddClassLabel (ClassLabel label) {
			var labelstat = label_stats.Find((x) => { return x.template.guid == label.template.guid; });
			if ( labelstat == null ) {
				label_stats.Add(
					new ClassLabelStat {
						template = label.template,
						index = label_stats.Count,
						counts = 1
					}); ;
			} else {
				labelstat.counts += 1;
			}
		}
		/// <summary>
		/// 去除一个类别标签
		/// 当类别标签的统计数为 0 时，这个标签从 label_stats 集合中去除
		/// </summary>
		public void RemoveClassLabel (ClassLabel label) {
			var labelstat = label_stats.Find((x) => { return x.template.guid == label.template.guid; });
			if ( labelstat != null ) {
				labelstat.counts -= 1;
				if ( label_stats.Count == 0 ) {
					label_stats.Remove(labelstat);
				}
			}
		}
		/// <summary>
		/// 以 Json 的形式保存所有的标签
		/// </summary>
		public async Task SaveLabelStatisticsAsync (string save_path) {
			await Task.Run(
			    delegate () {
				    JsonSerializer serializer = new JsonSerializer();
				    using ( JsonWriter writer = new JsonTextWriter(new StreamWriter(save_path)) ) {
					    serializer.Serialize(writer, new {
						    labels = label_stats,
						    save_date = DateTime.Now.ToShortDateString()
					    });
				    }
			    }
		    );
		}
	}
}
