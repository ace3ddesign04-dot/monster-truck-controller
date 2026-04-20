using System;
using UnityEngine;

[Serializable]
public class TerrainStamp
{
	public ModAction stampAction;

	public int stampTextureID;

	public Vector2 stampPosition;

	public float stampRotation;

	[Range(0.1f, 20f)]
	public float stampSize;

	[Range(0f, 1f)]
	public float stampStrength;

	public int extraInt;

	public string Serialize()
	{
		string empty = string.Empty;
		empty = empty + (int)stampAction + "|";
		empty = empty + stampTextureID + "|";
		empty = empty + (int)stampPosition.x + "|";
		empty = empty + (int)stampPosition.y + "|";
		empty = empty + (int)stampRotation + "|";
		empty = empty + (int)stampSize + "|";
		empty = empty + Mathf.Round(stampStrength * 100f) / 100f + "|";
		return empty + extraInt;
	}

	public void Deserialize(string s)
	{
		string[] array = s.Split('|');
		stampAction = (ModAction)int.Parse(array[0]);
		stampTextureID = int.Parse(array[1]);
		stampPosition.x = float.Parse(array[2]);
		stampPosition.y = float.Parse(array[3]);
		stampRotation = float.Parse(array[4]);
		stampSize = float.Parse(array[5]);
		stampStrength = float.Parse(array[6]);
		extraInt = int.Parse(array[7]);
	}
}
