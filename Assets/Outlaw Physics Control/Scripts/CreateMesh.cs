using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
	public int tileSize = 10;

	public float meshWidth = 50f;

	public float meshHeight = 50f;

	[ContextMenu("Create plane")]
	private void CreatePlane()
	{
		Mesh mesh = new Mesh();
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;
		MeshRenderer component = GetComponent<MeshRenderer>();
		if (component == null)
		{
			component = base.gameObject.AddComponent<MeshRenderer>();
		}
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector2> list4 = new List<Vector2>();
		int num = (int)meshWidth / tileSize;
		int num2 = (int)meshHeight / tileSize;
		float num3 = meshWidth % (float)tileSize;
		float num4 = meshHeight % (float)tileSize;
		int num5 = (num3 != 0f) ? 1 : 0;
		int num6 = (num4 != 0f) ? 1 : 0;
		int index = 0;
		for (int i = 0; i < num + num5; i++)
		{
			for (int j = 0; j < num2 + num6; j++)
			{
				float currentTileWidth = tileSize;
				float currentTileHeight = tileSize;
				if (i == num)
				{
					currentTileWidth = num3;
				}
				if (j == num2)
				{
					currentTileHeight = num4;
				}
				AddVertices(i, j, list, currentTileWidth, currentTileHeight);
				index = AddTriangles(index, list2);
				AddNormals(list3);
				AddUvs(i, j, currentTileWidth, currentTileHeight, list4);
			}
		}
		mesh.vertices = list.ToArray();
		mesh.normals = list3.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.uv = list4.ToArray();
		mesh.RecalculateNormals();
	}

	private void AddVertices(int x, int y, List<Vector3> vertices, float currentTileWidth, float currentTileHeight)
	{
		vertices.Add(new Vector3(x * tileSize, 0f, y * tileSize));
		vertices.Add(new Vector3((float)(x * tileSize) + currentTileWidth, 0f, y * tileSize));
		vertices.Add(new Vector3((float)(x * tileSize) + currentTileWidth, 0f, (float)(y * tileSize) + currentTileHeight));
		vertices.Add(new Vector3(x * tileSize, 0f, (float)(y * tileSize) + currentTileHeight));
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
		float num = (float)x * 0.01f * (float)tileSize;
		float num2 = (float)y * 0.01f * (float)tileSize;
		float num3 = 0.01f * (currentTileWidth / (float)tileSize) * (float)tileSize;
		float num4 = 0.01f * (currentTileHeight / (float)tileSize) * (float)tileSize;
		uvs.Add(new Vector2(num, num2));
		uvs.Add(new Vector2(num + num3, num2));
		uvs.Add(new Vector2(num + num3, num2 + num4));
		uvs.Add(new Vector2(num, num2 + num4));
	}
}
