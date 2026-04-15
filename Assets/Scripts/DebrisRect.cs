using System;
using UnityEngine;

[Serializable]
public struct DebrisRect
{
	public Transform FL;

	public Transform FR;

	public Transform RL;

	public Transform RR;

	public Vector3 GetRandomPos()
	{
		Vector3 a = FL.position - RL.position;
		Vector3 a2 = FR.position - FL.position;
		return RL.position + a * UnityEngine.Random.Range(0f, 1f) + a2 * UnityEngine.Random.Range(0f, 1f);
	}
}
