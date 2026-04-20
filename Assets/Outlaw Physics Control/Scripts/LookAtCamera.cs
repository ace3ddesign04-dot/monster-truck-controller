using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private Camera cam;

	private void Start()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		if (!(cam == null))
		{
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - cam.transform.position);
		}
	}
}
