using System.Collections.Generic;
using UnityEngine;

public class PlacementSettings : MonoBehaviour
{
	public Axis upAxis = Axis.Y;

	[Header("Position settings")]
	public float verticalOffset;

	public bool HideLowerPartUnderTerrain;

	public float cullingPlaneHeight;

	[Header("Rotation settings")]
	public bool randomX;

	public bool randomY;

	public bool randomZ;

	public bool alignByNormal;

	[Header("Debug")]
	[Range(5f, 50f)]
	public float planeSize = 5f;

	[Range(0.2f, 2f)]
	public float verticesSize = 0.5f;

	private void OnDrawGizmosSelected()
	{
		if (!HideLowerPartUnderTerrain)
		{
			return;
		}
		Vector3 size = Vector3.one * planeSize;
		size.y = 0.001f;
		Color red = Color.red;
		red.a = 0.5f;
		Gizmos.color = red;
		Gizmos.DrawCube(base.transform.position + new Vector3(0f, cullingPlaneHeight, 0f), size);
		MeshFilter componentInChildren = GetComponentInChildren<MeshFilter>();
		Vector3[] vertices = componentInChildren.sharedMesh.vertices;
		if (!(componentInChildren != null))
		{
			return;
		}
		Vector3[] array = vertices;
		foreach (Vector3 vertice in array)
		{
			switch (upAxis)
			{
			case Axis.X:
			{
				Vector3 vector3 = RootLocalVerticePos(vertice, componentInChildren);
				float x = vector3.x;
				Vector3 vector4 = RootLocalPlanePos();
				if (x < vector4.x)
				{
					Gizmos.DrawSphere(WorldVerticePos(vertice, componentInChildren), verticesSize);
				}
				break;
			}
			case Axis.Y:
			{
				Vector3 vector5 = RootLocalVerticePos(vertice, componentInChildren);
				float y = vector5.y;
				Vector3 vector6 = RootLocalPlanePos();
				if (y < vector6.y)
				{
					Gizmos.DrawSphere(WorldVerticePos(vertice, componentInChildren), verticesSize);
				}
				break;
			}
			case Axis.Z:
			{
				Vector3 vector = RootLocalVerticePos(vertice, componentInChildren);
				float z = vector.z;
				Vector3 vector2 = RootLocalPlanePos();
				if (z < vector2.z)
				{
					Gizmos.DrawSphere(WorldVerticePos(vertice, componentInChildren), verticesSize);
				}
				break;
			}
			}
		}
	}

	[ContextMenu("Align myself")]
	public void Align()
	{
		if (alignByNormal)
		{
			LevelEditorTools.AlignByNormal(Terrain.activeTerrain, base.transform);
		}
		if (HideLowerPartUnderTerrain)
		{
			MakeAllVerticesBeUnderTerrain();
		}
		else
		{
			AlignByTerrainHeight();
		}
		ApplyVerticalOffset();
	}

	private void ApplyVerticalOffset()
	{
		base.transform.position += Vector3.up * verticalOffset;
	}

	private void MakeAllVerticesBeUnderTerrain()
	{
		if (randomX || randomZ)
		{
			return;
		}
		Terrain activeTerrain = Terrain.activeTerrain;
		base.transform.position += Vector3.up * 10f;
		MeshFilter componentInChildren = GetComponentInChildren<MeshFilter>();
		Mesh sharedMesh = componentInChildren.sharedMesh;
		List<int> list = new List<int>();
		Vector3[] vertices = sharedMesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			switch (upAxis)
			{
			case Axis.X:
			{
				Vector3 vector3 = RootLocalVerticePos(vertices[i], componentInChildren);
				float x = vector3.x;
				Vector3 vector4 = RootLocalPlanePos();
				if (x < vector4.x)
				{
					list.Add(i);
				}
				break;
			}
			case Axis.Y:
			{
				Vector3 vector5 = RootLocalVerticePos(vertices[i], componentInChildren);
				float y = vector5.y;
				Vector3 vector6 = RootLocalPlanePos();
				if (y < vector6.y)
				{
					list.Add(i);
				}
				break;
			}
			case Axis.Z:
			{
				Vector3 vector = RootLocalVerticePos(vertices[i], componentInChildren);
				float z = vector.z;
				Vector3 vector2 = RootLocalPlanePos();
				if (z < vector2.z)
				{
					list.Add(i);
				}
				break;
			}
			}
		}
		int num = 0;
		if (!AreAllVerticesUnderTerrain(componentInChildren, vertices, list, activeTerrain))
		{
			do
			{
				num++;
				base.transform.position -= Vector3.up * 0.1f;
			}
			while (!AreAllVerticesUnderTerrain(componentInChildren, vertices, list, activeTerrain) && num < 10000);
		}
	}

	private void AlignByTerrainHeight()
	{
		Terrain activeTerrain = Terrain.activeTerrain;
		Vector3 position = base.transform.position;
		position.y = activeTerrain.SampleHeight(position);
		base.transform.position = position;
	}

	public void ApplyRandomRotation()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		Vector3 vector = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
		if (randomX)
		{
			eulerAngles.x = vector.x;
		}
		if (randomY)
		{
			eulerAngles.y = vector.y;
		}
		if (randomZ)
		{
			eulerAngles.z = vector.z;
		}
		base.transform.eulerAngles = eulerAngles;
	}

	private Vector3 WorldVerticePos(Vector3 vertice, MeshFilter filter)
	{
		return filter.transform.TransformPoint(vertice) + (filter.transform.position - base.transform.position);
	}

	private Vector3 RootLocalVerticePos(Vector3 vertice, MeshFilter filter)
	{
		return base.transform.InverseTransformPoint(WorldVerticePos(vertice, filter));
	}

	private Vector3 RootLocalPlanePos()
	{
		switch (upAxis)
		{
		case Axis.X:
			return base.transform.InverseTransformPoint(base.transform.position + base.transform.right * cullingPlaneHeight);
		case Axis.Y:
			return base.transform.InverseTransformPoint(base.transform.position + base.transform.up * cullingPlaneHeight);
		case Axis.Z:
			return base.transform.InverseTransformPoint(base.transform.position + base.transform.forward * cullingPlaneHeight);
		default:
			return Vector3.zero;
		}
	}

	private bool AreAllVerticesUnderTerrain(MeshFilter filter, Vector3[] allVertices, List<int> verticeIDs, Terrain terrain)
	{
		foreach (int verticeID in verticeIDs)
		{
			Vector3 worldPosition = WorldVerticePos(allVertices[verticeID], filter);
			float num = terrain.SampleHeight(worldPosition);
			if (worldPosition.y > num)
			{
				return false;
			}
		}
		return true;
	}
}
