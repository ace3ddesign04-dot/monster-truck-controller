using System.Collections.Generic;
using UnityEngine;

public class MudTest : MonoBehaviour
{
	public int xStart;

	public int xEnd;

	public int yStart;

	public int yEnd;

	public float amount;

	public float tileSize = 10f;

	public float meshWidth = 50f;

	public float meshHeight = 50f;

	public Material planeMat;

	public GameObject plane;

	[ContextMenu("Lower")]
	private void LowerTerrain()
	{
		Terrain component = GetComponent<Terrain>();
		TerrainData terrainData = component.terrainData;
		float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
		for (int i = 0; i < terrainData.heightmapResolution; i++)
		{
			for (int j = 0; j < terrainData.heightmapResolution; j++)
			{
				if (i > xStart && i < xEnd && j > yStart && j < yEnd)
				{
					heights[j, i] -= amount;
				}
			}
		}
		terrainData.SetHeights(0, 0, heights);
	}

	[ContextMenu("Show verts")]
	private void ShowVerts()
	{
		Terrain component = GetComponent<Terrain>();
		TerrainData terrainData = component.terrainData;
		for (int i = 0; i < terrainData.heightmapResolution; i++)
		{
			for (int j = 0; j < terrainData.heightmapResolution; j++)
			{
				Vector3 position = component.transform.position;
				float num = (float)i / (float)(terrainData.heightmapResolution - 1);
				Vector3 size = terrainData.size;
				float x = num * size.x;
				float num2 = (float)j / (float)(terrainData.heightmapResolution - 1);
				Vector3 size2 = terrainData.size;
				Vector3 vector = position + new Vector3(x, 0f, num2 * size2.z);
				vector.y = component.SampleHeight(vector);
				UnityEngine.Debug.DrawRay(vector, Vector3.up, Color.red, 5f);
			}
		}
	}

	[ContextMenu("Create plane")]
	private void CreatePlane()
	{
		Terrain component = GetComponent<Terrain>();
		TerrainData terrainData = component.terrainData;
		Mesh mesh = new Mesh();
		if (plane == null)
		{
			plane = new GameObject("MudPlane");
		}
		int num = xStart - 1;
		int num2 = xEnd + 1;
		int num3 = yStart - 1;
		int num4 = yEnd + 1;
		float num5 = (float)(num2 - num) / (float)terrainData.heightmapResolution;
		Vector3 size = terrainData.size;
		meshWidth = num5 * size.x;
		float num6 = (float)(num4 - num3) / (float)terrainData.heightmapResolution;
		Vector3 size2 = terrainData.size;
		meshHeight = num6 * size2.z;
		Vector3 position = component.transform.position;
		float num7 = (float)num / (float)(terrainData.heightmapResolution - 1);
		Vector3 size3 = terrainData.size;
		float x = num7 * size3.x;
		float num8 = (float)num3 / (float)(terrainData.heightmapResolution - 1);
		Vector3 size4 = terrainData.size;
		Vector3 vector = position + new Vector3(x, 0f, num8 * size4.z);
		vector.y = component.SampleHeight(vector);
		plane.transform.position = vector;
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		int num9 = (int)(meshWidth / tileSize);
		int num10 = (int)(meshHeight / tileSize);
		float num11 = meshWidth % tileSize;
		float num12 = meshHeight % tileSize;
		int num13 = (num11 != 0f) ? 1 : 0;
		int num14 = (num12 != 0f) ? 1 : 0;
		int[] array = new int[(num9 + num13) * (num10 + num14) * 2 * 3];
		Vector3[] array2 = new Vector3[(num9 + num13) * (num10 + num14)];
		int num15 = 0;
		for (int i = 0; i < num9 + num13; i++)
		{
			for (int j = 0; j < num10 + num14; j++)
			{
				float num16 = tileSize;
				float num17 = tileSize;
				if (i == num9)
				{
					num16 = num11;
				}
				if (j == num10)
				{
					num17 = num12;
				}
				array2[i * num10 + num14 + j] = new Vector3(terrainData.heightmapResolution / (num9 + num13 - 1) * j, 0f, terrainData.heightmapResolution / (num10 + num14 - 1) * i);
				if (i > 0 && j > 0 && i < num9 + num13 - 1 && j < num10 + num14 - 1)
				{
					array[num15] = i * num10 + num14 + j - 1;
					array[num15 + 1] = (i - 1) * num10 + num14 + j;
					array[num15 + 2] = (i - 1) * num10 + num14 + j - 1;
					num15 += 3;
					array[num15] = i * num10 + num14 + j;
					array[num15 + 1] = (i - 1) * num10 + num14 + j;
					array[num15 + 2] = i * num10 + num14 + j - 1;
					num15 += 3;
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			Vector3 vector2 = plane.transform.TransformPoint(list[k]);
			vector2.y = component.SampleHeight(vector2);
			list[k] = plane.transform.InverseTransformPoint(vector2);
		}
		mesh.vertices = array2;
		mesh.triangles = array;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = plane.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;
		MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = plane.AddComponent<MeshRenderer>();
		}
		meshRenderer.sharedMaterial = planeMat;
		plane.transform.position -= Vector3.up * 0.1f;
	}

	private void AddVertices(int x, int y, List<Vector3> vertices, float currentTileWidth, float currentTileHeight)
	{
		vertices.Add(new Vector3((float)x * tileSize, 0f, (float)y * tileSize));
		vertices.Add(new Vector3((float)x * tileSize + currentTileWidth, 0f, (float)y * tileSize));
		vertices.Add(new Vector3((float)x * tileSize + currentTileWidth, 0f, (float)y * tileSize + currentTileHeight));
		vertices.Add(new Vector3((float)x * tileSize, 0f, (float)y * tileSize + currentTileHeight));
	}

	private int AddTriangles(int index, List<int> triangles)
	{
		triangles.Add(index + 2);
		triangles.Add(index + 1);
		triangles.Add(index);
		triangles.Add(index);
		triangles.Add(index + 3);
		triangles.Add(index + 2);
		index += 4;
		return index;
	}

	private void AddNormals(List<Vector3> normals)
	{
		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);
	}

	private void AddUvs(int x, int y, float currentTileWidth, float currentTileHeight, List<Vector2> uvs)
	{
		float num = (float)x * 0.01f * tileSize;
		float num2 = (float)y * 0.01f * tileSize;
		float num3 = 0.01f * (currentTileWidth / tileSize) * tileSize;
		float num4 = 0.01f * (currentTileHeight / tileSize) * tileSize;
		uvs.Add(new Vector2(num, num2));
		uvs.Add(new Vector2(num + num3, num2));
		uvs.Add(new Vector2(num + num3, num2 + num4));
		uvs.Add(new Vector2(num, num2 + num4));
	}
}
