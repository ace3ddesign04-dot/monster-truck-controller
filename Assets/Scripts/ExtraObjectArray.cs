using System;
using UnityEngine;

[Serializable]
public class ExtraObjectArray
{
	public string displayedName;

	public GameObject[] prefabs;

	public float minDistanceInterval;

	[Tooltip("This is the count of the objects on big terrain(500x500)")]
	public int baseObjectsCount;
}
