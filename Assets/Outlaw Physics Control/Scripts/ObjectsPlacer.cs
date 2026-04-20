using System.Collections.Generic;
using UnityEngine;

public class ObjectsPlacer : MonoBehaviour
{
	public ObjectGroup[] objectGroups;

	public void PlaceObjects()
	{
		Terrain activeTerrain = Terrain.activeTerrain;
		if (activeTerrain == null)
		{
			UnityEngine.Debug.LogError("No terrain found!");
			return;
		}
		GameObject gameObject = GameObject.Find("_ObjectsPlacer");
		if (gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		gameObject = new GameObject("_ObjectsPlacer");
		string text = "Placed: \n";
		ObjectGroup[] array = objectGroups;
		foreach (ObjectGroup objectGroup in array)
		{
			if (objectGroup.prefab == null)
			{
				UnityEngine.Debug.LogError("Assign prefab!");
				return;
			}
			Transform transform = new GameObject(objectGroup.prefab.name).transform;
			transform.parent = gameObject.transform;
			List<Vector3> list = new List<Vector3>();
			int num = 0;
			for (int j = 0; (float)j < objectGroup.count; j++)
			{
				bool flag = false;
				int num2 = 0;
				do
				{
					num2++;
					Vector3 vector = RandomTerrainPoint(activeTerrain);
					flag = true;
					foreach (Vector3 item in list)
					{
						if (Vector3.Distance(item, vector) < objectGroup.minDistanceInterval)
						{
							flag = false;
						}
					}
					if (flag && objectGroup.terrainTextures != null && objectGroup.terrainTextures.Length > 0)
					{
						Texture2D mainTextureAtPosition = GetMainTextureAtPosition(activeTerrain, vector);
						Texture2D[] terrainTextures = objectGroup.terrainTextures;
						foreach (Texture2D texture2D in terrainTextures)
						{
							if (!mainTextureAtPosition.name.Equals(texture2D.name))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						float steepnessAngle = GetSteepnessAngle(activeTerrain, vector);
						if (objectGroup.minSteepness > steepnessAngle)
						{
							flag = false;
						}
					}
					if (flag)
					{
						Vector3 position = activeTerrain.GetPosition();
						Vector3 a = Vector3.up * objectGroup.minHeight;
						Vector3 size = activeTerrain.terrainData.size;
						Vector3 vector2 = position + a * size.y;
						Vector3 position2 = activeTerrain.GetPosition();
						Vector3 a2 = Vector3.up * objectGroup.maxHeight;
						Vector3 size2 = activeTerrain.terrainData.size;
						Vector3 vector3 = position2 + a2 * size2.y;
						if (vector.y < vector2.y || vector.y > vector3.y)
						{
							flag = false;
						}
					}
					if (flag)
					{
						float x = objectGroup.randomXRotation ? UnityEngine.Random.Range(0, 360) : 0;
						float y = objectGroup.randomYRotation ? UnityEngine.Random.Range(0, 360) : 0;
						float z = objectGroup.randomZRotation ? UnityEngine.Random.Range(0, 360) : 0;
						Quaternion rotation = Quaternion.Euler(x, y, z);
						if (objectGroup.alignByNormal)
						{
							rotation = Quaternion.LookRotation(GetNormalAtPosition(activeTerrain, vector));
						}
						GameObject gameObject2 = UnityEngine.Object.Instantiate(objectGroup.prefab, vector + Vector3.up * objectGroup.heightOffset, rotation);
						gameObject2.transform.parent = transform;
						list.Add(vector);
						num++;
					}
				}
				while (!flag && num2 < 1000);
			}
			string text2 = text;
			text = text2 + objectGroup.prefab.name + ": " + num + "/" + objectGroup.count + "\n";
		}
		UnityEngine.Debug.Log(text);
	}

	private Vector3 GetNormalAtPosition(Terrain terrain, Vector3 pos)
	{
		float x = pos.x;
		Vector3 position = terrain.GetPosition();
		float num = x - position.x;
		Vector3 size = terrain.terrainData.size;
		float x2 = num / size.x;
		float z = pos.z;
		Vector3 position2 = terrain.GetPosition();
		float num2 = z - position2.z;
		Vector3 size2 = terrain.terrainData.size;
		float y = num2 / size2.z;
		return terrain.terrainData.GetInterpolatedNormal(x2, y);
	}

	private float GetSteepnessAngle(Terrain terrain, Vector3 position)
	{
		Vector3 normalAtPosition = GetNormalAtPosition(terrain, position);
		Vector3 to = Vector3.ProjectOnPlane(normalAtPosition, Vector3.up);
		return 90f - Vector3.Angle(normalAtPosition, to);
	}

	private Texture2D GetMainTextureAtPosition(Terrain terrain, Vector3 position)
	{
		Vector3Int splatMapCoords = GetSplatMapCoords(position, terrain);
		float[,,] alphamaps = terrain.terrainData.GetAlphamaps(splatMapCoords.x, splatMapCoords.z, 1, 1);
		float[] array = new float[alphamaps.GetUpperBound(2) + 1];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = alphamaps[0, 0, i];
		}
		int num = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] > array[num])
			{
				num = j;
			}
		}
		SplatPrototype splatPrototype = terrain.terrainData.splatPrototypes[num];
		return splatPrototype.texture;
	}

	private Vector3Int GetSplatMapCoords(Vector3 worldPos, Terrain terrain)
	{
		Vector3Int zero = Vector3Int.zero;
		float x = worldPos.x;
		Vector3 position = terrain.GetPosition();
		float num = x - position.x;
		Vector3 size = terrain.terrainData.size;
		zero.x = (int)(num / size.x * (float)terrain.terrainData.alphamapWidth);
		float z = worldPos.z;
		Vector3 position2 = terrain.GetPosition();
		float num2 = z - position2.z;
		Vector3 size2 = terrain.terrainData.size;
		zero.z = (int)(num2 / size2.z * (float)terrain.terrainData.alphamapHeight);
		zero.x = Mathf.Clamp(zero.x, 0, terrain.terrainData.alphamapWidth);
		zero.z = Mathf.Clamp(zero.z, 0, terrain.terrainData.alphamapHeight);
		return zero;
	}

	private Vector3 RandomTerrainPoint(Terrain terrain)
	{
		Vector3 position = terrain.GetPosition();
		Vector3 size = terrain.terrainData.size;
		float x = size.x;
		Vector3 size2 = terrain.terrainData.size;
		float z = size2.z;
		float x2 = UnityEngine.Random.Range(position.x, position.x + x);
		float z2 = UnityEngine.Random.Range(position.z, position.z + z);
		float y = terrain.SampleHeight(new Vector3(x2, 0f, z2));
		return new Vector3(x2, y, z2);
	}

	private Vector3 TerrainCenter(Terrain terrain)
	{
		Vector3 position = terrain.GetPosition();
		Vector3 a = position;
		Vector3 size = terrain.terrainData.size;
		float x = size.z / 2f;
		Vector3 size2 = terrain.terrainData.size;
		return a + new Vector3(x, 0f, size2.z / 2f);
	}

	private void OnValidate()
	{
		ObjectGroup[] array = objectGroups;
		foreach (ObjectGroup objectGroup in array)
		{
			if (objectGroup.prefab != null)
			{
				objectGroup.name = objectGroup.prefab.name;
			}
			else
			{
				objectGroup.name = "Element";
			}
			if (objectGroup.minHeight == 0f && objectGroup.maxHeight == 0f)
			{
				objectGroup.maxHeight = 1f;
			}
			if (objectGroup.maxHeight <= objectGroup.minHeight)
			{
				objectGroup.maxHeight = objectGroup.minHeight + 0.01f;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			return;
		}
		Terrain activeTerrain = Terrain.activeTerrain;
		if (activeTerrain == null)
		{
			return;
		}
		ObjectGroup[] array = objectGroups;
		foreach (ObjectGroup objectGroup in array)
		{
			if (objectGroup.displayHeightPlanes)
			{
				Vector3 size = activeTerrain.terrainData.size;
				size.y = 1f;
				Vector3 a = TerrainCenter(activeTerrain);
				Vector3 a2 = Vector3.up * objectGroup.minHeight;
				Vector3 size2 = activeTerrain.terrainData.size;
				Vector3 center = a + a2 * size2.y;
				Color color = Color.green;
				color.a = 0.6f;
				Gizmos.color = color;
				Gizmos.DrawCube(center, size);
				Vector3 a3 = TerrainCenter(activeTerrain);
				Vector3 a4 = Vector3.up * objectGroup.maxHeight;
				Vector3 size3 = activeTerrain.terrainData.size;
				Vector3 center2 = a3 + a4 * size3.y;
				color = Color.red;
				color.a = 0.6f;
				Gizmos.color = color;
				Gizmos.DrawCube(center2, size);
			}
		}
	}
}
