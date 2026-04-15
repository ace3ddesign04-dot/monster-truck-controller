using System;
using UnityEngine;

[Serializable]
public class DynamicPositionPart
{
	public Transform part;

	public Transform[] Positions;

	public void UpdatePosition()
	{
		Transform[] positions = Positions;
		foreach (Transform transform in positions)
		{
			if (transform.gameObject.activeInHierarchy)
			{
				part.position = transform.position;
			}
		}
	}
}
