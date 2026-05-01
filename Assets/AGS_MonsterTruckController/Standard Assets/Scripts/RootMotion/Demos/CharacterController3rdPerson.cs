using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(AnimatorController3rdPerson))]
	public class CharacterController3rdPerson : MonoBehaviour
	{
		[SerializeField]
		private CameraController cam;

		private AnimatorController3rdPerson animatorController;

		private static Vector3 inputVector => new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));

		private static Vector3 inputVectorRaw => new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0f, UnityEngine.Input.GetAxisRaw("Vertical"));

		private void Start()
		{
			animatorController = GetComponent<AnimatorController3rdPerson>();
			cam.enabled = false;
		}

		private void LateUpdate()
		{
			cam.UpdateInput();
			cam.UpdateTransform();
			Vector3 inputVector = CharacterController3rdPerson.inputVector;
			bool isMoving = CharacterController3rdPerson.inputVector != Vector3.zero || inputVectorRaw != Vector3.zero;
			Vector3 forward = cam.transform.forward;
			Vector3 aimTarget = cam.transform.position + forward * 10f;
			animatorController.Move(inputVector, isMoving, forward, aimTarget);
		}
	}
}
