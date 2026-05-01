using RootMotion.FinalIK;
using UnityEngine;

namespace RootMotion.Demos
{
	public class TerrainOffset : MonoBehaviour
	{
		public AimIK aimIK;

		public Vector3 raycastOffset = new Vector3(0f, 2f, 1.5f);

		public LayerMask raycastLayers;

		public float min = -2f;

		public float max = 2f;

		public float lerpSpeed = 10f;

		private RaycastHit hit;

		private Vector3 offset;

		private void LateUpdate()
		{
			Vector3 b = base.transform.rotation * raycastOffset;
			Vector3 groundHeightOffset = GetGroundHeightOffset(base.transform.position + b);
			offset = Vector3.Lerp(offset, groundHeightOffset, Time.deltaTime * lerpSpeed);
			Vector3 vector = base.transform.position + new Vector3(b.x, 0f, b.z);
			aimIK.solver.transform.LookAt(vector);
			aimIK.solver.IKPosition = vector + offset;
		}

		private Vector3 GetGroundHeightOffset(Vector3 worldPosition)
		{
			UnityEngine.Debug.DrawRay(worldPosition, Vector3.down * raycastOffset.y * 2f, Color.green);
			if (Physics.Raycast(worldPosition, Vector3.down, out hit, raycastOffset.y * 2f))
			{
				Vector3 point = hit.point;
				float y = point.y;
				Vector3 position = base.transform.position;
				return Mathf.Clamp(y - position.y, min, max) * Vector3.up;
			}
			return Vector3.zero;
		}
	}
}
