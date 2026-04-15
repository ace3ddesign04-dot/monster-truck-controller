using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class RagdollUtilityDemo : MonoBehaviour
	{
		public RagdollUtility ragdollUtility;

		public Transform root;

		public Rigidbody pelvis;

		private void OnGUI()
		{
			GUILayout.Label(" Press R to switch to ragdoll. \n Weigh in one of the FBBIK effectors to make kinematic changes to the ragdoll pose.\n A to blend back to animation");
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.R))
			{
				ragdollUtility.EnableRagdoll();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.A))
			{
				ragdollUtility.DisableRagdoll();
			}
		}
	}
}
