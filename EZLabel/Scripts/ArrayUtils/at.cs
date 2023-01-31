using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	public static partial class ArrayUtils {
		public static ref T at<T> (this T[,] t, int index) where T : INumber<T> {
			int x = t.GetLength(1);
			return ref t[index / x, index % x];
		}
		public static ref T at<T> (this T[,,] t, int index) where T : INumber<T> {
			int x = t.GetLength(1);
			int y = t.GetLength(2);
			return ref t[index / ( x * y ), ( index / y ) % x, index % y];
		}
		public static ref T at<T> (this T[,,,] t, int index) where T : INumber<T> {
			int x = t.GetLength(1);
			int y = t.GetLength(2);
			int z = t.GetLength(3);
			return ref t[index / ( x * y * z ), ( index / z / y ) % x, ( index / z ) % y, index % z];
		}
	}
}
