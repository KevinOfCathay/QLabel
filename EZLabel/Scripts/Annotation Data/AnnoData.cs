using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace QLabel.Scripts.AnnotationData {
	public abstract record AnnoData {
		public enum Type { Dot, Rectangle, Square, Tetragon, Line, Circle, Polygon }

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
		/// 这个注释数据的额外标签
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

		/// <summary>
		/// 获得约束这个 annotation 的边框
		/// </summary>
		public virtual (Vector2 tl, Vector2 br) GetBoundingBox () {
			float top = float.MaxValue, left = float.MaxValue, bottom = float.MinValue, right = float.MinValue;
			foreach ( var point in points ) {
				if ( point.X < top ) { top = point.X; }
				if ( point.X > bottom ) { bottom = point.X; }
				if ( point.Y < left ) { left = point.Y; }
				if ( point.Y > right ) { right = point.Y; }
			}
			return (new Vector2(top, left), new Vector2(bottom, right));
		}

		/// <summary>
		/// 获取 ID （目前没有任何用处）
		/// </summary>
		public string ID () {
			using ( System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create() ) {
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(
					createtime.ToLongTimeString() + clas.ToString() + label.ToString()
					);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				return Convert.ToHexString(hashBytes);
			}
		}
	}
}
