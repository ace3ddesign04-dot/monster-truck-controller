using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public static List<Map> Maps = new List<Map>();

	public MapManager()
	{
		Maps.Add(new Map("Map1", isCurrent: true));
	}

	public Map Current()
	{
		return Maps.Find((Map m) => m.IsCurrent);
	}
}
