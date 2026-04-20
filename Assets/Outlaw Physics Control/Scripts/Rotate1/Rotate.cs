using System;
using UnityEngine;

[Serializable]
public class Rotate : MonoBehaviour
{
	public float speed;

	public Rotate()
	{
		speed = 10f;
	}

	public void Update()
	{
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}

	public void Main()
	{
	}
}
