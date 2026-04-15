using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MudArea
{
	public List<Vector2> Chunks;

	public int xMin;

	public int yMin;

	public int xMax;

	public int yMax;

	public int Width;

	public int Height;

	public bool HasMudWater;

	public SurfaceMaterial deformableMaterial;
}
