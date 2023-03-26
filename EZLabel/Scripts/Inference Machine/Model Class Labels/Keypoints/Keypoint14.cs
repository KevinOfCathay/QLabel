using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabel.Scripts {
	internal static partial class ModelLabels {
		// Keypoints for crowd pose
		public static ClassLabel[] keypoints_cp14 = {
									// group,  name,  supercat
				new ClassLabel("Keypoints", "left shoulder", "upper"),			     // 0 
				new ClassLabel("Keypoints", "right shoulder eye", "upper"),		     // 1 
				new ClassLabel("Keypoints", "left elbow", "upper"),			// 2
				new ClassLabel("Keypoints", "right elbow", "upper" ),		     // 3
				new ClassLabel("Keypoints", "left wrist", "upper"   ),	     // 4
				new ClassLabel("Keypoints", "right wrist", "upper"),	// 5
				new ClassLabel("Keypoints", "left hip", "lower"),			// 6
				new ClassLabel("Keypoints", "right hip", "lower"),		// 7
				new ClassLabel("Keypoints", "left knee", "lower"),		// 8
				new ClassLabel("Keypoints", "right knee", "lower"),			// 9
				new ClassLabel("Keypoints", "left ankle", "lower"),		// 10
				new ClassLabel("Keypoints", "right ankle", "lower"),			// 11
				new ClassLabel("Keypoints", "top head", "lower"),			// 12
				new ClassLabel("Keypoints", "neck", "lower"),			// 13
		};
		public static (int x, int y, ClassLabel)[] keypoints_cp14_skeleton = {
				(12, 13, new ClassLabel("Keypoints", "top head-neck", "skeleton")),
				(13, 0, new ClassLabel("Keypoints", "neck-left shoulder", "skeleton")),
				(13, 1, new ClassLabel("Keypoints", "neck-right shoulder", "skeleton")),
				(0, 1, new ClassLabel("Keypoints", "left shoulder-right shoulder", "skeleton")),
				(0, 2, new ClassLabel("Keypoints", "left shoulder-left elbow", "skeleton")),
				(0, 6, new ClassLabel("Keypoints", "left shoulder-left hip", "skeleton")),
				(1, 3, new ClassLabel("Keypoints", "right shoulder-right elbow", "skeleton")),
				(1, 7, new ClassLabel("Keypoints", "right shoulder-right hip", "skeleton")),
				(2, 4, new ClassLabel("Keypoints", "left elbow-left wrist", "skeleton")),
				(3, 5, new ClassLabel("Keypoints", "right elbow-right wrist", "skeleton")),
				(6, 7, new ClassLabel("Keypoints", "left hip-right hip", "skeleton")),
				(6, 8, new ClassLabel("Keypoints", "left hip-left knee", "skeleton")),
				(8, 10, new ClassLabel("Keypoints", "left knee-left ankle", "skeleton")),
				(7, 9, new ClassLabel("Keypoints", "right hip-right knee", "skeleton")),
				(9, 11, new ClassLabel("Keypoints", "right knee-right ankle", "skeleton")),
		};
	}
}