using System;

namespace QLabel.Scripts.Inference_Machine {
	internal class ModelParameter {
		public ModelParameter (string name, Type type, object value, bool editable) {
			this.name = name;
			this.type = type;
			this.value = value;
			this.editable = editable;
		}

		/// <summary> 参数的类型 </summary>
		public Type type;

		/// <summary> 参数的名字 </summary>
		public string name;

		/// <summary> 参数的值 / 默认值 </summary>
		public object value;

		/// <summary> 用户是否可以更高参数的值 </summary>
		public bool editable;

		/// <summary> 参数的说明 </summary>
		public string description;
	}
}
