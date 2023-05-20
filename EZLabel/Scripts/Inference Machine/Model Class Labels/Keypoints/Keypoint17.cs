using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	internal static partial class ModelLabels {
		public static ClassTemplate[] keypoints17 = {
									// group,  name,  supercat
				new ClassTemplate("Keypoints", "nose", "upper"),			     // 0 
				new ClassTemplate("Keypoints", "left eye", "upper"),		     // 1 
				new ClassTemplate("Keypoints", "right eye", "upper"),			// 2
				new ClassTemplate("Keypoints", "left ear", "upper" ),		     // 3
				new ClassTemplate("Keypoints", "right ear", "upper"   ),	     // 4
				new ClassTemplate("Keypoints", "left shoulder", "upper"),	// 5
				new ClassTemplate("Keypoints", "right shoulder", "upper"),	// 6
				new ClassTemplate("Keypoints", "left elbow", "upper"),		// 7
				new ClassTemplate("Keypoints", "right elbow", "upper"),		// 8
				new ClassTemplate("Keypoints", "left wrist", "upper"),			// 9
				new ClassTemplate("Keypoints", "right wrist", "upper"),		// 10
				new ClassTemplate("Keypoints", "left hip", "lower"),			// 11
				new ClassTemplate("Keypoints", "right hip", "lower"),			// 12
				new ClassTemplate("Keypoints", "left knee", "lower"),			// 13
				new ClassTemplate("Keypoints", "right knee", "lower"),	    // 14
				new ClassTemplate("Keypoints", "left ankle", "lower"),			// 15
				new ClassTemplate("Keypoints", "right ankle", "lower"),		// 16
		};
		public static (int x, int y, ClassTemplate)[] keypoints17_skeleton = {
				(0, 1, new ClassTemplate("Keypoints", "nose-left eye", "skeleton")),
				(0, 2, new ClassTemplate("Keypoints", "nose-right eye", "skeleton")),
				(1, 2, new ClassTemplate("Keypoints", "left eye-right eye", "skeleton")),
				(1, 3, new ClassTemplate("Keypoints", "left eye-left ear", "skeleton")),
				(2, 4, new ClassTemplate("Keypoints", "right eye-right ear", "skeleton")),
				(3, 5, new ClassTemplate("Keypoints", "left ear-left shoulder", "skeleton")),
				(4, 6, new ClassTemplate("Keypoints", "right ear-right shoulder", "skeleton")),
				(5, 6, new ClassTemplate("Keypoints", "left shoulder-right shoulder", "skeleton")),
				(5, 7, new ClassTemplate("Keypoints", "left shoulder-left elbow", "skeleton")),
				(6, 8, new ClassTemplate("Keypoints", "right shoulder-right elbow", "skeleton")),
				(7, 9, new ClassTemplate("Keypoints", "left elbow-left wrist", "skeleton")),
				(8, 10, new ClassTemplate("Keypoints", "right elbow-right wrist", "skeleton")),
				(5, 11, new ClassTemplate("Keypoints", "left shoulder-left hip", "skeleton")),
				(6, 12, new ClassTemplate("Keypoints", "right shoulder-right hip", "skeleton")),
				(11, 12, new ClassTemplate("Keypoints", "left hip-right hip", "skeleton")),
				(11, 13, new ClassTemplate("Keypoints", "left hip-left knee", "skeleton")),
				(12, 14, new ClassTemplate("Keypoints", "right hip-right knee", "skeleton")),
				(13, 15, new ClassTemplate("Keypoints", "left knee-left ankle", "skeleton")),
				(14, 16, new ClassTemplate("Keypoints", "right knee-right ankle", "skeleton"))
		};
	}
}