using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	public static partial class utils {
		public static int shape<T> (this T[] t) {
			return t.Length;
		}
		public static (int, int) shape<T> (this T[,] t) {
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			return (x, y);
		}
		public static (int, int, int) shape<T> (this T[,,] t) {
			int x = t.GetLength(0);
			int y = t.GetLength(1);
			int z = t.GetLength(2);
			return (x, y, z);
		}
		public static (int, int, int, int) shape<T> (this T[,,,] t) {
			int b = t.GetLength(0);
			int c = t.GetLength(1);
			int h = t.GetLength(2);
			int w = t.GetLength(3);
			return (b, c, h, w);
		}
	}
}
