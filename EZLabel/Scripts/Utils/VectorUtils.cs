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
		public static Point[] to_points (this Span<Vector2> vector2s) {
			int length = vector2s.Length;
			Point[] points = new Point[length];
			for ( int i = 0; i < length; i += 1 ) {
				points[i] = new Point(vector2s[i].X, vector2s[i].Y);
			}
			return points;
		}
		/// <summary>
		/// Lexicographical sort (in-place)
		/// </summary>
		public static void sort (this Vector2[] vector2s) {
			Array.Sort(vector2s,
			    (x, y) => {
				    if ( x.X < y.X ) {
					    return 1;
				    } else if ( x.X > y.X ) {
					    return -1;
				    } else {
					    if ( x.Y < y.Y ) {
						    return 1;
					    } else if ( x.Y > y.Y ) {
						    return -1;
					    }
					    return 0;
				    }
			    });
		}
	}
}
