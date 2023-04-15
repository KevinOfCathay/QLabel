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
using System.Windows.Controls;
using System.Xml.Serialization;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace QLabel.Scripts.AnnotationData {
	/// <summary>
	/// 矩形
	/// </summary>
	public record ADRect : AnnoData {
		[JsonProperty] public bool is_square;
		private Vector2 tl, br;
		public override int visualize_priority => 1;
		/// <summary>
		/// Deserialization
		/// </summary>
		public ADRect (ReadOnlySpan<Vector2> rpoints, ClassLabel clabel, float conf, Guid guid, DateTime createtime, bool is_square) :
			base(rpoints, Type.Rectangle, clabel, conf, guid, createtime) {
			this.is_square = is_square;
			this.tl = rpoints[0];
			this.br = rpoints[3];
		}
		public ADRect
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f, bool is_square = false) :
			base(points, Type.Rectangle, clas, conf) {
			this.is_square = is_square;
			this.tl = rpoints[0];
			this.br = rpoints[3];
		}
		public ADRect
			(float xmin, float ymin, float xmax, float ymax, ClassLabel clas, float conf = 1.0f, bool is_square = false) :
			base(new ReadOnlySpan<Vector2>(new Vector2[] { new Vector2(xmin, ymin), new Vector2(xmax, ymin), new Vector2(xmin, ymax), new Vector2(xmax, ymax) }),
				Type.Rectangle, clas, conf) {
			this.is_square = is_square;
			this.tl = new Vector2(xmin, ymin);
			this.br = new Vector2(xmax, ymax);
		}
		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			// 创建一个矩形
			DraggableRectangle rect = new DraggableRectangle { data = this };
			Vector2 tl = canvas.CanvasPosition(rpoints[0]);
			Vector2 br = canvas.CanvasPosition(rpoints[3]);
			rect.Draw(canvas, new Vector2[] { tl, br });
			return rect;
		}
		public override void Visualize (Bitmap bitmap, Vector2 target_size, Color color) {
			// 获取源图像大小
			Vector2 size = new Vector2(bitmap.Width, bitmap.Height);
			Vector2 scale = size / target_size;
			var topleft = tl / scale;
			var bottomright = br / scale;
			var rectf = new RectangleF(topleft.X, topleft.Y, bottomright.X - topleft.X, bottomright.Y - topleft.Y);

			using ( var g = Graphics.FromImage(bitmap) ) {
				Pen pen = new Pen(color, 3);
				g.DrawRectangle(pen, rectf);
			}
		}
	}
}
