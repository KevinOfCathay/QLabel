﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QLabel.Scripts.Projects;
using QLabel.Windows.Main_Canvas;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows;

namespace QLabel.Scripts.AnnotationData {
	public abstract record AnnoData {
		public enum Type { Dot, Rectangle, Tetragon, Line, Circle, Polygon }

		/// <summary> Copy Constructor </summary>
		public AnnoData (AnnoData source) {
			guid = source.guid;
			rpoints = source.rpoints;
			bbox = source.bbox;
			createtime = source.createtime;

			type = source.type;
			class_label = source.class_label;
			caption = source.caption;
			truncated = source.truncated;
			occluded = source.occluded;
		}

		public AnnoData (ReadOnlySpan<Vector2> rpoints, Type type, ClassLabel class_label) {
			createtime = DateTime.Now;
			this.rpoints = rpoints.ToArray();
			this.type = type;
			this.class_label = class_label;
			this.guid = Guid.NewGuid();
			bbox = GetBoundingBox(rpoints);
		}

		public AnnoData (ReadOnlySpan<Vector2> rpoints, Type type, ClassLabel class_label, Guid guid, DateTime createtime) {
			this.createtime = createtime;
			this.rpoints = rpoints.ToArray();
			this.type = type;
			this.class_label = class_label;
			this.guid = guid;
			bbox = GetBoundingBox(rpoints);
		}

		#region Properties
		/// <summary> 约束这个 annotation 的边框（以rect的形式）</summary>
		public Int32Rect brect {
			get { return new Int32Rect((int) bbox.X, (int) bbox.Y, (int) bbox.Width, (int) bbox.Height); }
		}
		#endregion

		#region Read-Only Fields
		/// <summary>  这个注释数据的 GUID</summary>
		[JsonProperty] public readonly Guid guid;
		/// <summary>  这个注释数据的类型 (readonly) </summary>
		[JsonProperty][JsonConverter(typeof(StringEnumConverter))] public readonly Type type;
		/// <summary>  这个注释数据的点的位置 (x, y) (readonly) </summary>
		[JsonProperty] public readonly Vector2[] rpoints;
		/// <summary> 约束这个 annotation 的边框 (readonly) </summary>
		[JsonProperty] public readonly OpenCvSharp.Rect2f bbox;
		/// <summary> 这个注释数据的类 (readonly) </summary>
		[JsonProperty] public readonly ClassLabel class_label;
		/// <summary> 这个注释数据被创建的时间，在 AnnoData 初始化时被设置 (readonly) </summary>
		[JsonProperty] public readonly DateTime createtime;
		#endregion

		/// <summary>  这个注释数据的描述文字 </summary>
		[JsonProperty] public string caption = string.Empty;
		/// <summary> more than 15-20% of the object lies outside the bounding box</summary>
		[JsonProperty] public bool truncated = false;
		/// <summary> more than 5% of the object lies outside the bounding box</summary>
		[JsonProperty] public bool occluded = false;
		/// <summary>这个 annodatas 是否 checked (UI 相关) </summary>
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
		protected virtual OpenCvSharp.Rect2f GetBoundingBox (ReadOnlySpan<Vector2> rpoints) {
			float top = float.MaxValue, left = float.MaxValue, bottom = float.MinValue, right = float.MinValue;
			foreach ( var point in rpoints ) {
				if ( point.X < left ) { left = point.X; }
				if ( point.X > right ) { right = point.X; }
				if ( point.Y < top ) { top = point.Y; }
				if ( point.Y > bottom ) { bottom = point.Y; }
			}
			return new OpenCvSharp.Rect2f(left, top, right - left, bottom - top);
		}
	}
}
