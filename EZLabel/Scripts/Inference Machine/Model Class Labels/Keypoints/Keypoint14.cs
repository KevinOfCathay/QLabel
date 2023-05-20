using QLabel.Scripts.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	internal static partial class ModelLabels {
		// Keypoints for crowd pose
		public static ClassTemplate[] keypoints_cp14 = {
									// group,  name,  supercat
				new ClassTemplate("Keypoints", "left shoulder", "upper"),			     // 0 
				new ClassTemplate("Keypoints", "right shoulder eye", "upper"),		     // 1 
				new ClassTemplate("Keypoints", "left elbow", "upper"),			// 2
				new ClassTemplate("Keypoints", "right elbow", "upper" ),		     // 3
				new ClassTemplate("Keypoints", "left wrist", "upper"   ),	     // 4
				new ClassTemplate("Keypoints", "right wrist", "upper"),	// 5
				new ClassTemplate("Keypoints", "left hip", "lower"),			// 6
				new ClassTemplate("Keypoints", "right hip", "lower"),		// 7
				new ClassTemplate("Keypoints", "left knee", "lower"),		// 8
				new ClassTemplate("Keypoints", "right knee", "lower"),			// 9
				new ClassTemplate("Keypoints", "left ankle", "lower"),		// 10
				new ClassTemplate("Keypoints", "right ankle", "lower"),			// 11
				new ClassTemplate("Keypoints", "top head", "lower"),			// 12
				new ClassTemplate("Keypoints", "neck", "lower"),			// 13
		};
		public static (int x, int y, ClassTemplate)[] keypoints_cp14_skeleton = {
				(12, 13, new ClassTemplate("Keypoints", "top head-neck", "skeleton")),
				(13, 0, new ClassTemplate("Keypoints", "neck-left shoulder", "skeleton")),
				(13, 1, new ClassTemplate("Keypoints", "neck-right shoulder", "skeleton")),
				(0, 1, new ClassTemplate("Keypoints", "left shoulder-right shoulder", "skeleton")),
				(0, 2, new ClassTemplate("Keypoints", "left shoulder-left elbow", "skeleton")),
				(0, 6, new ClassTemplate("Keypoints", "left shoulder-left hip", "skeleton")),
				(1, 3, new ClassTemplate("Keypoints", "right shoulder-right elbow", "skeleton")),
				(1, 7, new ClassTemplate("Keypoints", "right shoulder-right hip", "skeleton")),
				(2, 4, new ClassTemplate("Keypoints", "left elbow-left wrist", "skeleton")),
				(3, 5, new ClassTemplate("Keypoints", "right elbow-right wrist", "skeleton")),
				(6, 7, new ClassTemplate("Keypoints", "left hip-right hip", "skeleton")),
				(6, 8, new ClassTemplate("Keypoints", "left hip-left knee", "skeleton")),
				(8, 10, new ClassTemplate("Keypoints", "left knee-left ankle", "skeleton")),
				(7, 9, new ClassTemplate("Keypoints", "right hip-right knee", "skeleton")),
				(9, 11, new ClassTemplate("Keypoints", "right knee-right ankle", "skeleton")),
		};
	}
}