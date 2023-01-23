using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts.Utils {
	public static class ConvertToString {
		public static ReadOnlySpan<char> to_string (this Vector2 point) {
			return ( "( " + ( (int) point.X ).ToString() + " , " + ( (int) point.Y ).ToString() + " )" );
		}
		/// <summary>
		/// 标注点 --> string
		/// </summary>
		public static string to_string (this Vector2[] points) {
			StringBuilder sb = new StringBuilder(capacity: points.Length * 10);
			foreach ( var point in points ) {
				sb.Append(point.to_string());
			}
			return sb.ToString();
		}
		/// <summary>
		/// 过长的字符串将会省略一些内容
		/// </summary>
		public static string to_shortstring (this Vector2[] points, int count = 20) {
			StringBuilder sb = new StringBuilder(count + 10);
			foreach ( var point in points ) {
				sb.Append(point.to_string());
				if ( sb.Length > count ) {
					return sb.ToString(0, count) + "...";
				}
			}
			return sb.ToString();
		}
	}
}
