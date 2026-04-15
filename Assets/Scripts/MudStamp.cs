using System;
using UnityEngine;

[Serializable]
public class MudStamp
{
	public int stampTextureID;

	public float stampRotation;

	public Vector3 stampPosition;

	public float stampSize;

	public GameObject stampIndicator;

	public float mudDepth;

	public float mudViscosity;

	public float boundsRadius => Mathf.Sqrt(stampSize * stampSize + stampSize * stampSize);

	public string Serialize()
	{
		string empty = string.Empty;
		empty = empty + stampTextureID + "|";
		empty = empty + stampRotation + "|";
		string text = empty;
		empty = text + stampPosition.x + "/" + stampPosition.y + "/" + stampPosition.z + "|";
		empty = empty + stampSize + "|";
		empty = empty + mudDepth + "|";
		return empty + mudViscosity;
	}

	public void Deserialize(string data)
	{
		string[] array = data.Split('|');
		stampTextureID = int.Parse(array[0]);
		stampRotation = float.Parse(array[1]);
		string[] array2 = array[2].Split('/');
		stampPosition = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
		stampSize = float.Parse(array[3]);
		mudDepth = float.Parse(array[4]);
		mudViscosity = float.Parse(array[5]);
	}
}
