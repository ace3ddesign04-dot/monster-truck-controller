using UnityEngine;

public class rotate : MonoBehaviour
{
	public Vector3 axis;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		base.transform.Rotate(axis * Time.deltaTime);
	}
}
