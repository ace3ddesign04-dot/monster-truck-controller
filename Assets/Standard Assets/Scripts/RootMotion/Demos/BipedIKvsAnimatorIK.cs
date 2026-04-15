using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class BipedIKvsAnimatorIK : MonoBehaviour
	{
		[LargeHeader("References")]
		public BipedIK bipedIK;

		[LargeHeader("Look At")]
		public Transform lookAtTargetBiped;

		[Range(0f, 1f)]
		public float lookAtWeight = 1f;

		[Range(0f, 1f)]
		public float lookAtBodyWeight = 1f;

		[Range(0f, 1f)]
		public float lookAtHeadWeight = 1f;

		[Range(0f, 1f)]
		public float lookAtEyesWeight = 1f;

		[Range(0f, 1f)]
		public float lookAtClampWeight = 0.5f;

		[Range(0f, 1f)]
		public float lookAtClampWeightHead = 0.5f;

		[Range(0f, 1f)]
		public float lookAtClampWeightEyes = 0.5f;

		[LargeHeader("Hand")]
		public Transform handTargetBiped;

		public Transform rightHandTargetBiped;

		[Range(0f, 1f)]
		public float handPositionWeight;

		[Range(0f, 1f)]
		public float handRotationWeight;

		private void Update()
		{
			bipedIK.SetLookAtPosition(lookAtTargetBiped.position);
			bipedIK.SetLookAtWeight(lookAtWeight, lookAtBodyWeight, lookAtHeadWeight, lookAtEyesWeight, lookAtClampWeight, lookAtClampWeightHead, lookAtClampWeightEyes);
			bipedIK.SetIKPosition(AvatarIKGoal.LeftHand, handTargetBiped.position);
			bipedIK.SetIKRotation(AvatarIKGoal.LeftHand, handTargetBiped.rotation);
			bipedIK.SetIKPositionWeight(AvatarIKGoal.LeftHand, handPositionWeight);
			bipedIK.SetIKRotationWeight(AvatarIKGoal.LeftHand, handRotationWeight);
			bipedIK.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetBiped.position);
			bipedIK.SetIKRotation(AvatarIKGoal.RightHand, rightHandTargetBiped.rotation);
			bipedIK.SetIKPositionWeight(AvatarIKGoal.RightHand, handPositionWeight);
			bipedIK.SetIKRotationWeight(AvatarIKGoal.RightHand, handRotationWeight);
		}
	}
}
