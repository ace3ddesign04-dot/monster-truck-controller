using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public GameObject target;

	public int speed;

	private void Update()
	{
		base.transform.LookAt(target.transform);
	}
}
