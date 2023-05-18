using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel {
	// Some extension methods
	public static partial class utils {
		public static Tensor<T> take<T> (this Tensor<T> tensor, int[] indices, int axis = 0) {
			// 首先获取维度
			ReadOnlySpan<int> dims = tensor.Dimensions;
			DenseTensor<T> new_tensor = new DenseTensor<T>(dims);
			int rank = dims.Length;
			int[] index = new int[rank];
			int[] cap = new int[rank];

			// 初始化 index 和 cap
			for ( int i = 0; i < rank; i += 1 ) {
				index[i] = 0;
				cap[i] = dims[i];
			}
			while ( true ) {
				int[] new_index = index.ToArray();
				int index_axis_old = new_index[axis];
				int index_axis_new = indices[index_axis_old];
				new_index[axis] = index_axis_new;
				new_tensor[index] = tensor[new_index];

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
			return new_tensor;
		}
	}
}
