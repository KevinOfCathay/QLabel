using Newtonsoft.Json;
using QLabel.Windows.Main_Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLabel.Scripts.AnnotationData {
	public record ADCircle : AnnoData {
		[JsonProperty] public float radius = 0f;
		public override int visualize_priority => 1;

		public ADCircle
			(ReadOnlySpan<Vector2> points, ClassLabel clas, float conf = 1.0f, float radius = 0f) :
			base(points, Type.Circle, clas, conf) {
			this.radius = radius;
		}

		public override IAnnotationElement CreateAnnotationElement (MainCanvas canvas) {
			throw new NotImplementedException();
		}

		public override void Visualize (Bitmap bitmap, Vector2 img_size, Color color) {
			throw new NotImplementedException();
		}
	}
}
