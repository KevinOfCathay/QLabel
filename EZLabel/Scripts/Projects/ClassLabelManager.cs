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
		private List<ClassLabelStat> label_stats = new List<ClassLabelStat>();

		public ICollection<ClassTemplate> label_set_full { get { return Utils.GetLabelSet(label_stats); } }

		/// <summary> 刷新 / 创建一个新的 Manager </summary>
		public void New () {
			label_stats.Clear();
		}

		public void AddClassTemplates (ClassTemplate[] templates) {
			foreach ( var label in templates ) {
				AddClassTemplate(label);
			}
		}
		/// <summary>
		/// 增加一个类别标签
		/// 这个类别标签可以是使用过/出现过的（即，作为 annodatas 的 classlabel 加入到 imagedata 中）
		/// 也可以是未使用的（例如，自动识别时，一些类从来都没有被使用过，或者用户自己加入到类别集合中）
		/// </summary>
		public void AddClassTemplate (ClassTemplate template) {
			var labelstat = label_stats.Find((x) => { return x.template.guid == template.guid; });
			if ( labelstat == null ) {
				label_stats.Add(
					new ClassLabelStat {
						template = template,
						index = label_stats.Count,
						counts = 1
					}); ;
			} else {
				labelstat.counts += 1;
			}
		}
		/// <summary>
		/// 去除一个类别标签
		/// </summary>
		public void RemoveClassTemplate (ClassTemplate label) {
			var labelstat = label_stats.Find((x) => { return x.template == label; });
			if ( labelstat != null ) {
				label_stats.Remove(labelstat);
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
