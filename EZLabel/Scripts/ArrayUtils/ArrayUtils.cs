using System;
using System.Collections.Concurrent;
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
			ConcurrentStack<(int, int)> where = new ConcurrentStack<(int, int)>();
			int x = t.GetLength(0);
			int y = t.GetLength(1);

			for ( int i = 0; i < x; i += 1 ) {
				Parallel.For(0, y, (int k) => { if ( p.Invoke(t[i, k]) ) { where.Push((i, k)); } }
				);
			}
			return where.ToArray();
		}
		public static (int, int, int)[] where<T> (this T[,,] t, Predicate<T> p) where T : INumber<T> {
			ConcurrentStack<(int, int, int)> where = new ConcurrentStack<(int, int, int)>();
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			int z = t.GetLength(2);
			for ( int i = 0; i < x; i += 1 ) {
				for ( int k = 0; k < y; k += 1 ) {
					Parallel.For(0, z, (int h) => {
						if ( p.Invoke(t[i, k, h]) ) { where.Push((i, k, h)); }
					});
				}
			}
			return where.ToArray();
		}
		public static T[,] clone<T> (this T[,] t) where T : INumber<T> {
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			T[,] arr = new T[x, y];
			for ( int i = 0; i < x; i += 1 ) {
				Parallel.For(0, x, (int k) => {
					arr[i, k] = t[i, k];
				});
			}
			return arr;
		}
	}
}
