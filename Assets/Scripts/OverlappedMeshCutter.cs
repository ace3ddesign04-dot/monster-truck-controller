using Battlehub.Integration;
using Battlehub.MeshTools;
using System.Collections.Generic;
using UnityEngine;

public class OverlappedMeshCutter : MonoBehaviour
{
	public GameObject[] GameobjectsToProcess;

	private Terrain currentTerrain;

	[ContextMenu("Cut overlapping meshes")]
	public void CutOverlappingMeshes()
	{
		if (GameobjectsToProcess == null || GameobjectsToProcess.Length == 0)
		{
			UnityEngine.Debug.LogError("Fill -GameObjects to process- array");
			return;
		}
		CutObject[] array = new CutObject[GameobjectsToProcess.Length];
		for (int i = 0; i < GameobjectsToProcess.Length; i++)
		{
			array[i] = new CutObject();
			array[i].GO = GameobjectsToProcess[i];
			array[i].meshFilter = GameobjectsToProcess[i].GetComponent<MeshFilter>();
			array[i].meshCollider = array[i].GO.AddComponent<MeshCollider>();
			array[i].meshCollider.convex = true;
			if (array[i].meshCollider.bounds.size == Vector3.zero)
			{
				array[i].meshCollider.inflateMesh = true;
			}
		}
		for (int j = 0; j < GameobjectsToProcess.Length; j++)
		{
			for (int k = 0; k < GameobjectsToProcess.Length; k++)
			{
				if (k != j && array[j].meshCollider.bounds.Intersects(array[k].meshCollider.bounds))
				{
					CutMesh(array[j].meshFilter, array[k].meshCollider);
				}
			}
		}
		for (int l = 0; l < GameobjectsToProcess.Length; l++)
		{
			UnityEngine.Object.DestroyImmediate(array[l].meshCollider);
		}
	}

	public void CutMeshesUnderTerrains()
	{
		if (GameobjectsToProcess == null || GameobjectsToProcess.Length == 0)
		{
			UnityEngine.Debug.LogError("Fill -GameObjects to process- array");
			return;
		}
		CutObject[] array = new CutObject[GameobjectsToProcess.Length];
		for (int i = 0; i < GameobjectsToProcess.Length; i++)
		{
			array[i] = new CutObject();
			if (GameobjectsToProcess[i] == null)
			{
				UnityEngine.Debug.LogError("One of objects in -Gameobjects To Process- array is null");
				return;
			}
			array[i].GO = GameobjectsToProcess[i];
			array[i].meshFilter = GameobjectsToProcess[i].GetComponent<MeshFilter>();
			array[i].meshCollider = array[i].GO.AddComponent<MeshCollider>();
			array[i].meshCollider.convex = true;
			if (array[i].meshCollider.bounds.size == Vector3.zero)
			{
				array[i].meshCollider.inflateMesh = true;
			}
		}
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (activeTerrains.Length == 0)
		{
			UnityEngine.Debug.LogError("No terrains in scene");
			return;
		}
		for (int j = 0; j < array.Length; j++)
		{
			Terrain[] array2 = activeTerrains;
			foreach (Terrain terrain in array2)
			{
				Vector3 position = terrain.transform.position;
				Vector3 position2 = array[j].GO.transform.position;
				if (!(position2.x > position.x) || !(position2.z > position.z))
				{
					continue;
				}
				float x = position2.x;
				float x2 = position.x;
				Vector3 size = terrain.terrainData.size;
				if (x < x2 + size.x)
				{
					float z = position2.z;
					float z2 = position.z;
					Vector3 size2 = terrain.terrainData.size;
					if (z < z2 + size2.z)
					{
						currentTerrain = terrain;
					}
				}
			}
			if (currentTerrain != null)
			{
				CutMeshByHeight(array[j].meshFilter, currentTerrain);
			}
		}
		for (int l = 0; l < GameobjectsToProcess.Length; l++)
		{
			UnityEngine.Object.DestroyImmediate(array[l].meshCollider);
		}
	}

	private void CutMeshByHeight(MeshFilter meshFilterToBeCut, Terrain terrain)
	{
		Mesh mesh = meshFilterToBeCut.mesh;
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		List<int> list = new List<int>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 worldPosition = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i]]);
			Vector3 worldPosition2 = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i + 1]]);
			Vector3 worldPosition3 = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i + 2]]);
			if (worldPosition.y > terrain.SampleHeight(worldPosition) || worldPosition2.y > terrain.SampleHeight(worldPosition2) || worldPosition3.y > terrain.SampleHeight(worldPosition3))
			{
				list.Add(triangles[i]);
				list.Add(triangles[i + 1]);
				list.Add(triangles[i + 2]);
			}
		}
		mesh.triangles = list.ToArray();
	}

	private void CutMesh(MeshFilter meshFilterToBeCut, MeshCollider meshColliderThatOverlaps)
	{
		Mesh mesh = meshFilterToBeCut.mesh;
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		List<int> list = new List<int>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 vector = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i]]);
			Vector3 vector2 = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i + 1]]);
			Vector3 vector3 = meshFilterToBeCut.transform.TransformPoint(vertices[triangles[i + 2]]);
			if (Vector3.Distance(meshColliderThatOverlaps.ClosestPoint(vector), vector) > 0.01f || Vector3.Distance(meshColliderThatOverlaps.ClosestPoint(vector2), vector2) > 0.01f || Vector3.Distance(meshColliderThatOverlaps.ClosestPoint(vector3), vector3) > 0.01f)
			{
				list.Add(triangles[i]);
				list.Add(triangles[i + 1]);
				list.Add(triangles[i + 2]);
			}
		}
		mesh.triangles = list.ToArray();
	}

	public void MergeMeshes()
	{
		GameObject[] gameobjectsToProcess = GameobjectsToProcess;
		foreach (GameObject x in gameobjectsToProcess)
		{
			if (x == null)
			{
				UnityEngine.Debug.LogError("One of objects in -Gameobjects To Process- array is null");
				return;
			}
		}
		CombineResult combineResult = MeshUtils.Combine(GameobjectsToProcess);
		if (combineResult != null)
		{
			MeshCombinerIntegration.RaiseCombined(combineResult.GameObject, combineResult.Mesh);
		}
	}
}
