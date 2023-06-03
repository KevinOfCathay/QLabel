using System;

namespace QLabel.Scripts.Inference_Machine {
	internal class ModelParameter {
		public ModelParameter (string name, Type type, object value, bool editable, string description = null) {
			this.name = name;
			this.type = type;
			this.value = value;
			this.editable = editable;
			this.description = ( description == null ? string.Empty : description );
		}

		/// <summary> 参数的类型 </summary>
		public readonly Type type;

		/// <summary> 参数的名字 </summary>
		public readonly string name;

		/// <summary> 参数的值 / 默认值 </summary>
		public readonly object value;

		/// <summary> 用户是否可以更改参数的值 </summary>
		public readonly bool editable;

		/// <summary> 参数的说明 </summary>
		public readonly string description = string.Empty;
	}
}
