using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainPath
{
	public ModAction pathAction;

	public List<Vector3> pathPositions;

	public float pathWidth;

	public float pathStrength;

	public int pathPattern;

	public int extraInt;

	public string Serialize()
	{
		string empty = string.Empty;
		empty = empty + (int)pathAction + "|";
		empty = empty + Mathf.Round(pathWidth * 100f) / 100f + "|";
		empty = empty + Mathf.Round(pathStrength * 100f) / 100f + "|";
		empty = empty + pathPattern + "|";
		empty = empty + extraInt + "|";
		for (int i = 0; i < pathPositions.Count; i++)
		{
			Vector3 vector = pathPositions[i];
			object arg = (int)vector.x;
			Vector3 vector2 = pathPositions[i];
			string str = arg + "/" + (int)vector2.z;
			empty = empty + str + "|";
		}
		return empty;
	}

	public void Deserialize(string data)
	{
		string[] array = data.Split('|');
		pathAction = (ModAction)int.Parse(array[0]);
		pathWidth = float.Parse(array[1]);
		pathStrength = float.Parse(array[2]);
		pathPattern = int.Parse(array[3]);
		extraInt = int.Parse(array[4]);
		pathPositions = new List<Vector3>();
		for (int i = 5; i < array.Length; i++)
		{
			string text = array[i];
			string[] array2 = text.Split('/');
			if (array2.Length == 2)
			{
				Vector3 item = new Vector3(int.Parse(array2[0]), 0f, int.Parse(array2[1]));
				pathPositions.Add(item);
			}
		}
	}
}
