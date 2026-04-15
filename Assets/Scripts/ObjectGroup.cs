using System;
using UnityEngine;

[Serializable]
public class ObjectGroup
{
	[HideInInspector]
	public string name;

	public GameObject prefab;

	public float count;

	[Header("Placement")]
	public Texture2D[] terrainTextures;

	[Range(0f, 90f)]
	public float minSteepness;

	public float minDistanceInterval;

	public float heightOffset;

	[Range(0f, 1f)]
	public float minHeight;

	[Range(0f, 1f)]
	public float maxHeight;

	public bool displayHeightPlanes;

	[Header("Rotation")]
	public bool randomXRotation;

	public bool randomYRotation;

	public bool randomZRotation;

	public bool alignByNormal;
}
