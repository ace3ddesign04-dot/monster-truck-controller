using UnityEngine;

namespace Crosstales.UI
{
	public class UIDrag : MonoBehaviour
	{
		private float offsetX;

		private float offsetY;

		public void BeginDrag()
		{
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			offsetX = x - mousePosition.x;
			Vector3 position2 = base.transform.position;
			float y = position2.y;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			offsetY = y - mousePosition2.y;
		}

		public void OnDrag()
		{
			Transform transform = base.transform;
			float num = offsetX;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			float x = num + mousePosition.x;
			float num2 = offsetY;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			transform.position = new Vector3(x, num2 + mousePosition2.y);
		}
	}
}
