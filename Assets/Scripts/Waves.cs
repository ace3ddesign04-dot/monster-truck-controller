using System;
using UnityEngine;

[Serializable]
public class Waves : MonoBehaviour
{
	public float scale;

	public float speed;

	public float power;

	public Vector3 startPos;

	public Waves()
	{
		scale = 10f;
		speed = 1f;
		power = 0.3f;
	}

	public void Awake()
	{
		startPos = transform.position;
	}

	public void Update()
	{
		transform.position = startPos + Vector3.up * Mathf.Sin(Time.time / 2f) * scale;
	}

	public void Main()
	{
	}
}
