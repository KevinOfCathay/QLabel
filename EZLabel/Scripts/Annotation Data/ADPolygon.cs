using Newtonsoft.Json;
using QLabel.Windows.Main_Canvas;
using QLabel.Windows.Main_Canvas.Annotation_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 多边形，不检测是否有交叉
	/// </summary>
	public record ADPolygon : AnnoData {
		[JsonProperty] public int vertex;
		public override int visualize_priority => 1;
		public ADPolygon
			(ReadOnlySpan<Vector2> rpoints, ClassLabel clas, float conf = 1.0f) :
			base(rpoints, Type.Polygon, clas, conf) {
			vertex = rpoints.Length;
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			var cpoints = canvas.CanvasPosition(rpoints);
			DraggablePolygon polygon = new DraggablePolygon(canvas, cpoints) { data = this };
			return polygon;
		}

		public override void Visualize (Bitmap bitmap, Vector2 target_size, Color color) {
			// 获取源图像大小
			Vector2 size = new Vector2(bitmap.Width, bitmap.Height);
			Vector2 scale = size / target_size;

			using ( var g = Graphics.FromImage(bitmap) ) {
				Pen pen = new Pen(color, 3);
				int len = rpoints.Length;
				var points = new PointF[len];
				for ( var i = 0; i < len; i += 1 ) {
					var p = rpoints[i] / scale;
					points[i] = new PointF(p.X, p.Y);
				}
				g.DrawPolygon(pen, points);
			}
		}
	}
}
