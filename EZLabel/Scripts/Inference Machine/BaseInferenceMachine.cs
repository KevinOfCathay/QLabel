using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using QLabel.Scripts.AnnotationData;
using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace QLabel.Scripts.Inference_Machine {
	internal abstract class InferenceBase {
		internal readonly ClassTemplate[] templates;
		protected InferenceSession session;
		protected string model_path;
		protected bool use_gpu;
		protected readonly int[] input_dims, output_dims;

		public InferenceBase (int[] input_dims, int[] output_dims, ClassTemplate[] templates, bool use_gpu = false) {
			this.input_dims = input_dims;
			this.output_dims = output_dims;
			this.templates = templates;
			this.use_gpu = use_gpu;
		}

		/// <summary>
		/// 从路径（model_path）中加载 session
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
		/// 输入图片，执行模型并返回多张图片的结果
		/// class_filter: 模型识别时，仅输出一部分的 class 的结果
		/// target_class: 识别图片中目标 class 边界框中的内容
		/// </summary>
		public async Task<AnnoData[]> Run (ImageData data, HashSet<int> class_filter = null, ClassTemplate target_class = null) {
			var bitmap = await ImageUtils.ReadBitmapAsync(data.path);
			if ( target_class != null ) {
				return await Task<AnnoData[]>.Run(
					delegate () {
						// 截取 bitmap 中的所有 class
						List<AnnoData> datas = new List<AnnoData>();
						var target_annodatas = data.annodatas.FindAll((x) => { return x.class_label.template.guid == target_class.guid; });
						if ( target_annodatas != null ) {
							foreach ( var data in target_annodatas ) {
								var cropped = ImageUtils.CropBitmap(bitmap, data.bbox);
								var res = RunInference(cropped, class_filter);
								datas.AddRange(res);
							}
						}
						return datas.ToArray();
					}
				);
			} else {
				return await Task<AnnoData[]>.Run(
					delegate () { return RunInference(bitmap, class_filter); }
				);
			}
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
