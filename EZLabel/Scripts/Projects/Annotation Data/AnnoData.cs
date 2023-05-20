using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace QLabel.Scripts.AnnotationData {
	public abstract record AnnoData {
		public enum Type { Dot, Rectangle, Tetragon, Line, Circle, Polygon }
		/// <summary>
		/// Copy Constructor
		/// </summary>
		public AnnoData (AnnoData source) {
			rpoints = source.rpoints;
			bbox = source.bbox;
			brect = source.brect;
			createtime = source.createtime;
			guid = source.guid;

			type = source.type;
			class_label = source.class_label;
			caption = source.caption;
			truncated = source.truncated;
			occluded = source.occluded;
		}
		/// <summary>
		/// 初始化所有的 readonly 属性
		/// </summary>
		public AnnoData (ReadOnlySpan<Vector2> rpoints, Type type, ClassLabel class_label) {
			createtime = DateTime.Now;
			this.rpoints = rpoints.ToArray();
			this.type = type;
			this.class_label = class_label;
			this.guid = Guid.NewGuid();
			bbox = GetBoundingBox(rpoints);
			brect = new Int32Rect((int) bbox.tl.X, (int) bbox.tl.Y, (int) ( bbox.br.X - bbox.tl.X ), (int) ( bbox.br.Y - bbox.tl.Y ));
		}
		/// <summary>
		/// 初始化所有的 readonly 属性
		/// </summary>
		public AnnoData (ReadOnlySpan<Vector2> rpoints, Type type, ClassLabel class_label, Guid guid, DateTime createtime) {
			this.createtime = createtime;
			this.rpoints = rpoints.ToArray();
			this.type = type;
			this.class_label = class_label;
			this.guid = guid;
			bbox = GetBoundingBox(rpoints);
			brect = new Int32Rect((int) bbox.tl.X, (int) bbox.tl.Y, (int) ( bbox.br.X - bbox.tl.X ), (int) ( bbox.br.Y - bbox.tl.Y ));
		}

		#region Read-Only Fields
		/// <summary>  这个注释数据的类型 (readonly) </summary>
		[JsonProperty][JsonConverter(typeof(StringEnumConverter))] public readonly Type type;
		/// <summary>  这个注释数据的点的位置 (x, y) (readonly) </summary>
		[JsonProperty] public readonly Vector2[] rpoints;
		/// <summary> 约束这个 annotation 的边框 (readonly) </summary>
		[JsonProperty] public readonly (Vector2 tl, Vector2 br) bbox;
		/// <summary> 约束这个 annotation 的边框（以rect的形式） (readonly) </summary>
		[JsonProperty] public readonly Int32Rect brect;
		/// <summary> 这个注释数据的类 (readonly) </summary>
		[JsonProperty] public readonly ClassLabel class_label;
		/// <summary> 这个注释数据被创建的时间，在 AnnoData 初始化时被设置 (readonly) </summary>
		[JsonProperty] public readonly DateTime createtime;
		/// <summary>  这个注释数据的 GUID</summary>
		[JsonProperty] public readonly Guid guid;
		#endregion

		/// <summary>  这个注释数据的描述文字 </summary>
		[JsonProperty] public string caption = string.Empty;
		/// <summary> more than 15-20% of the object lies outside the bounding box</summary>
		[JsonProperty] public bool truncated = false;
		/// <summary> more than 5% of the object lies outside the bounding box</summary>
		[JsonProperty] public bool occluded = false;
		/// <summary>这个 annodata 是否 checked </summary>
		[JsonProperty] public bool check = true;

		public abstract int visualize_priority { get; }

		/// <summary>
		/// 从这个 AnnoData 中创建 AnnotationElement
		/// </summary>
		public abstract IAnnotationElement CreateAnnotationElement (MainCanvas canvas);
		public abstract void Visualize (Bitmap bitmap, Vector2 target_size, Color color);
		/// <summary>
		/// 获得约束这个 annotation 的边框
		/// </summary>
		protected virtual (Vector2 tl, Vector2 br) GetBoundingBox (ReadOnlySpan<Vector2> rpoints) {
			float top = float.MaxValue, left = float.MaxValue, bottom = float.MinValue, right = float.MinValue;
			foreach ( var point in rpoints ) {
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
					createtime.ToLongTimeString() + class_label.ToString()
					);
				byte[] hashBytes = md5.ComputeHash(inputBytes);
				return Convert.ToHexString(hashBytes);
			}
		}
	}
}
