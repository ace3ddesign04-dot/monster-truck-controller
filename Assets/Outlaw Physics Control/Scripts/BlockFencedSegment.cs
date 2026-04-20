using System;
using UnityEngine;

[Serializable]
public class BlockFencedSegment
{
	public float SegmentWidth = 10f;

	public GameObject BlockPrefab;

	public int StartWaypoint;

	public float DistanceBetweenBlocks;
}
