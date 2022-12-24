using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EZLabel.Scripts.AnnotationData {
	public record AnnoData {
		public enum Type { Dot, Rectangle, Square, Line, Circle }

		public AnnoData () { createtime = DateTime.Now; }

		/// <summary>
		/// 这个注释数据的点的位置 (x, y)
		/// </summary>
		public Vector2[] points;

		/// <summary>
		/// 这个注释数据的类
		/// </summary>
		public int clas;

		/// <summary>
		/// 这个注释数据的标签
		/// </summary>
		public string label;

		/// <summary>
		/// 这个注释数据被创建的时间，在 AnnoData 初始化时被设置
		/// </summary>
		public DateTime createtime { get; protected set; }

		/// <summary>
		/// 这个注释数据的类型
		/// </summary>
		public Type type { get; protected set; }
	}
}
