using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	// Some extension methods
	public static partial class utils {
		/// <summary>
		/// Perform an inplace flip
		/// </summary>
		public static Tensor<T> flip<T> (this Tensor<T> tensor, int axis = 0) {
			ReadOnlySpan<int> dims = tensor.Dimensions;
			int rank = dims.Length;
			int[] index = new int[rank];
			int[] cap = new int[rank];
			for ( int i = 0; i < rank; i += 1 ) {
				index[i] = 0;
				if ( i == axis ) {
					cap[i] = dims[i] / 2;
				} else {
					cap[i] = dims[i];
				}
			}
			while ( true ) {
				int[] new_index = index.ToArray();
				new_index[axis] = ( cap[axis] - 1 ) - index[axis];
				tensor[index] = tensor[new_index];

				// Increment index
				int ax = rank - 1;
				index[ax] += 1;
				while ( index[ax] >= cap[ax] ) {
					index[ax] = 0;
					ax -= 1;
					if ( ax < 0 ) { break; }
					index[ax] += 1;
				}
				if ( ax < 0 ) { break; }
			}
			return tensor;
		}
	}
}
