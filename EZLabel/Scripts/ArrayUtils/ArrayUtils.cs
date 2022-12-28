using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace QLabel {
	// Some extension methods
	public static class ArrayUtils {
		public static ref T at<T> (this T[,] t, int index) where T : INumber<T> {
			int y = t.GetLength(1);
			return ref t[index / y, index % y];
		}
		public static ref T at<T> (this T[,,] t, int index) where T : INumber<T> {
			int y = t.GetLength(1);
			int z = t.GetLength(2);
			return ref t[index / ( y * z ), ( index / z ) % y, index % z];
		}
		public static (int, int)[] where<T> (this T[,] t, Predicate<T> p) where T : INumber<T> {
			List<(int, int)> where = new List<(int, int)>();
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			for ( int i = 0; i < x; i += 1 ) {
				for ( int k = 0; k < y; k += 1 ) {
					if ( p.Invoke(t[i, k]) ) {
						where.Add((i, k));
					}
				}
			}
			return where.ToArray();
		}
		public static (int, int, int)[] where<T> (this T[,,] t, Predicate<T> p) where T : INumber<T> {
			List<(int, int, int)> where = new List<(int, int, int)>();
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			int z = t.GetLength(2);
			for ( int i = 0; i < x; i += 1 ) {
				for ( int k = 0; k < y; k += 1 ) {
					for ( int h = 0; k < z; k += 1 ) {
						if ( p.Invoke(t[i, k, h]) ) {
							where.Add((i, k, h));
						}
					}
				}
			}
			return where.ToArray();
		}
		public static T[,] clone<T> (this T[,] t) where T : INumber<T> {
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			T[,] arr = new T[x, y];
			for ( int i = 0; i < x; i += 1 ) {
				for ( int k = 0; k < x; k += 1 ) {
					arr[i, k] = t[i, k];
				}
			}
			return arr;
		}
	}
}
