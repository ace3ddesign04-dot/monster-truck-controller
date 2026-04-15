using System;
using UnityEngine;

[Serializable]
public class SnapPoint
{
	public Transform transform;

	public Transform leftAffector;

	public Transform rightAffector;

	[HideInInspector]
	public Vector3 leftAffectorDefPos;

	[HideInInspector]
	public Vector3 rightAffectorDefPos;

	[HideInInspector]
	public bool busy;

	public void ResetAffectors()
	{
		if (leftAffector != null && leftAffectorDefPos != Vector3.zero)
		{
			leftAffector.localPosition = leftAffectorDefPos;
		}
		if (rightAffector != null && rightAffectorDefPos != Vector3.zero)
		{
			rightAffector.localPosition = rightAffectorDefPos;
		}
	}
}
