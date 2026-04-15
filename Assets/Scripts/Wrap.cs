using System;
using UnityEngine;

[Serializable]
public class Wrap
{
	public int ID;

	public Vector4 Coords;

	public Color color;

	public Wrap(int _id, Vector4 _coords, Color _color)
	{
		ID = _id;
		Coords = _coords;
		color = _color;
	}

	public Wrap()
	{
	}
}
