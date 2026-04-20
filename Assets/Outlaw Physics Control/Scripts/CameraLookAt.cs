using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
	public Transform target;

	private void Start()
	{
	}

	private void Update()
	{
		if (target != null)
		{
			base.transform.LookAt(target);
		}
	}
}
