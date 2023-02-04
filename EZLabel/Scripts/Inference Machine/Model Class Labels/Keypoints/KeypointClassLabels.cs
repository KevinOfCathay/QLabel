using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	internal static partial class ModelLabels {
		public static ClassLabel[] keypoints16 = {
									// group,  name,  supercat
				new ClassLabel("Keypoints", "nose", "upper"),
				new ClassLabel("Keypoints", "left_eye", "upper"),
				new ClassLabel("Keypoints", "right_eye", "upper"),
				new ClassLabel("Keypoints", "left_ear", "upper" ),
				new ClassLabel("Keypoints", "right_ear", "upper"   ),
				new ClassLabel("Keypoints", "left_shoulder", "upper"),
				new ClassLabel("Keypoints", "right_shoulder", "upper"),
				new ClassLabel("Keypoints", "left_elbow", "upper"),
				new ClassLabel("Keypoints", "right_elbow", "upper"),
				new ClassLabel("Keypoints", "left_wrist", "upper"),
				new ClassLabel("Keypoints", "right_wrist", "upper"),
				new ClassLabel("Keypoints", "left_hip", "lower"),
				new ClassLabel("Keypoints", "right_hip", "lower"),
				new ClassLabel("Keypoints", "left_knee", "lower"),
				new ClassLabel("Keypoints", "right_knee", "lower"),
				new ClassLabel("Keypoints", "left_ankle", "lower"),
				new ClassLabel("Keypoints", "right_ankle", "lower"),
		};
	}
}