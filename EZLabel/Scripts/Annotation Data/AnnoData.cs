using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace QLabel.Scripts.AnnotationData {
	public abstract record AnnoData {
		public enum Type { Dot, Rectangle, Square, Tetragon, Line, Circle, Polygon }
		public AnnoData (ReadOnlySpan<Vector2> points, Type type, ClassLabel clas, string label, float conf) {
			createtime = DateTime.Now;
			this.points = points.ToArray();
			this.conf = conf;
			this.type = type;
			this.label = label;
			this.clas = clas;
			bbox = GetBoundingBox(points);
			brect = new Int32Rect((int) bbox.tl.X, (int) bbox.tl.Y, (int) ( bbox.br.X - bbox.tl.X ), (int) ( bbox.br.Y - bbox.tl.Y ));
		}
		public AnnoData (ReadOnlySpan<Vector2> points, Type type, int class_index, string label, float conf) {
			createtime = DateTime.Now;
			this.points = points.ToArray();
			this.conf = conf;
			this.type = type;
			this.label = label;
			this.clas = ProjectManager.project.GetLabel(class_index);
			bbox = GetBoundingBox(points);
			brect = new Int32Rect((int) bbox.tl.X, (int) bbox.tl.Y, (int) ( bbox.br.X - bbox.tl.X ), (int) ( bbox.br.Y - bbox.tl.Y ));
		}

		/// <summary>  这个注释数据的点的位置 (x, y) (readonly) </summary>
		public readonly Vector2[] points;
		/// <summary> 这个注释数据的 confidence (readonly) </summary>
		public readonly float conf;
		/// <summary> 约束这个 annotation 的边框 (readonly) </summary>
		public readonly (Vector2 tl, Vector2 br) bbox;
		/// <summary> 约束这个 annotation 的边框（以rect的形式） (readonly) </summary>
		public readonly Int32Rect brect;
		/// <summary> 这个注释数据的类 (readonly) </summary>
		public readonly ClassLabel clas;
		/// <summary>  这个注释数据的额外标签 (readonly) </summary>
		public readonly string label;
		/// <summary> 这个注释数据被创建的时间，在 AnnoData 初始化时被设置 (readonly) </summary>
		public readonly DateTime createtime;
		/// <summary>  这个注释数据的类型 (readonly) </summary>
		public Type type { get; }

		/// <summary>
		/// 从这个 AnnoData 中创建 AnnotationElement
		/// </summary>
		public abstract IAnnotationElement CreateAnnotationElement (MainCanvas canvas);

		/// <summary>
		/// 获得约束这个 annotation 的边框
		/// </summary>
		protected virtual (Vector2 tl, Vector2 br) GetBoundingBox (ReadOnlySpan<Vector2> points) {
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
