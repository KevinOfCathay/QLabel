using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Utils {
	public static class ConvertToString {
		/// <summary>
		/// 标注点 --> string
		/// </summary>
		public static string to_string (this Vector2[] points) {
			StringBuilder sb = new StringBuilder(capacity: points.Length * 10);
			foreach ( var point in points ) {
				sb.Append(string.Join(" ", '(', ( (int) point.X ).ToString(), ',', ( (int) point.Y ).ToString(), ')'));
			}
			return sb.ToString();
		}
	}
}
