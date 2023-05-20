using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace QLabel.Scripts.Inference_Machine {
	internal abstract class InferenceBase {
		protected InferenceSession session;
		protected string model_path;
		protected bool use_gpu;
		protected readonly int[] input_dims, output_dims;

		public InferenceBase (int[] input_dims, int[] output_dims, bool use_gpu = false) {
			this.input_dims = input_dims;
			this.output_dims = output_dims;
			this.use_gpu = use_gpu;
		}

		/// <summary>
		/// 从 path 中加载 session
		/// </summary>
		public virtual void BuildSession () {
			if ( use_gpu ) {
				session = new InferenceSession(model_path, SessionOptions.MakeSessionOptionWithCudaProvider(0));
			} else {
				session = new InferenceSession(model_path);
			}
			if ( session == null ) {
				Debug.WriteLine("Failed to load session.");
			}
		}

		/// <summary>
		/// 输入多张图片，执行模型并返回多张图片的结果
		/// </summary>
		public AnnoData[][] Run (ICollection<Bitmap> images, HashSet<int> class_filter = null) {
			AnnoData[][] res = new AnnoData[images.Count][];
			int index = 0;
			foreach ( var image in images ) {
				res[index] = RunInference(image, class_filter);
			}
			return res;
		}

		/// <summary>
		/// 将图片转换为 tensor
		/// </summary>
		protected abstract DenseTensor<float> GetInputTensor (Bitmap image);

		/// <summary>
		/// 执行模型并返回结果
		/// </summary>
		protected abstract AnnoData[] RunInference (Bitmap image, HashSet<int> class_filter = null);
	}
}
