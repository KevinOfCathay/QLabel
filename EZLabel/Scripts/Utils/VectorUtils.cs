using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLabel {
	internal static class VectorUtils {
		public static Point[] to_points (this Vector2[] vector2s) {
			int length = vector2s.Length;
			Point[] points = new Point[length];
			Parallel.For(0, length, i => {
				points[i] = new Point(vector2s[i].X, vector2s[i].Y);
			});
			return points;
		}
	}
}
