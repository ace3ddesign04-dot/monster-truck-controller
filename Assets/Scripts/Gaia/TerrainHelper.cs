using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	public class TerrainHelper : MonoBehaviour
	{
		[Range(1f, 5f)]
		[Tooltip("Number of smoothing interations to run. Can be run multiple times.")]
		public int m_smoothIterations = 1;

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
		}

		public static void Flatten()
		{
			FlattenTerrain(Terrain.activeTerrains);
		}

		public static void FlattenTerrain(Terrain terrain)
		{
			int heightmapWidth = terrain.terrainData.heightmapResolution;
			int heightmapHeight = terrain.terrainData.heightmapResolution;
			float[,] heights = new float[heightmapWidth, heightmapHeight];
			terrain.terrainData.SetHeights(0, 0, heights);
		}

		public static void FlattenTerrain(Terrain[] terrains)
		{
			foreach (Terrain terrain in terrains)
			{
				int heightmapWidth = terrain.terrainData.heightmapResolution;
				int heightmapHeight = terrain.terrainData.heightmapResolution;
				float[,] heights = new float[heightmapWidth, heightmapHeight];
				terrain.terrainData.SetHeights(0, 0, heights);
			}
		}

		public static void Stitch()
		{
			StitchTerrains(Terrain.activeTerrains);
		}

		public static void StitchTerrains(Terrain[] terrains)
		{
			Terrain terrain = null;
			Terrain terrain2 = null;
			Terrain terrain3 = null;
			Terrain terrain4 = null;
			foreach (Terrain terrain5 in terrains)
			{
				terrain = null;
				terrain2 = null;
				terrain3 = null;
				terrain4 = null;
				foreach (Terrain terrain6 in terrains)
				{
					Vector3 position = terrain6.transform.position;
					float x = position.x;
					Vector3 position2 = terrain5.transform.position;
					if (x == position2.x)
					{
						Vector3 position3 = terrain6.transform.position;
						float z = position3.z;
						Vector3 size = terrain6.terrainData.size;
						float num = z + size.z;
						Vector3 position4 = terrain5.transform.position;
						if (num == position4.z)
						{
							terrain4 = terrain6;
							continue;
						}
						Vector3 position5 = terrain5.transform.position;
						float z2 = position5.z;
						Vector3 size2 = terrain5.terrainData.size;
						float num2 = z2 + size2.z;
						Vector3 position6 = terrain6.transform.position;
						if (num2 == position6.z)
						{
							terrain3 = terrain6;
						}
						continue;
					}
					Vector3 position7 = terrain6.transform.position;
					float z3 = position7.z;
					Vector3 position8 = terrain5.transform.position;
					if (z3 != position8.z)
					{
						continue;
					}
					Vector3 position9 = terrain6.transform.position;
					float x2 = position9.x;
					Vector3 size3 = terrain6.terrainData.size;
					float num3 = x2 + size3.z;
					Vector3 position10 = terrain5.transform.position;
					if (num3 == position10.z)
					{
						terrain2 = terrain6;
						continue;
					}
					Vector3 position11 = terrain5.transform.position;
					float x3 = position11.x;
					Vector3 size4 = terrain5.terrainData.size;
					float num4 = x3 + size4.x;
					Vector3 position12 = terrain6.transform.position;
					if (num4 == position12.x)
					{
						terrain = terrain6;
					}
				}
				terrain5.SetNeighbors(terrain2, terrain4, terrain, terrain3);
			}
		}

		public void Smooth()
		{
			Smooth(m_smoothIterations);
		}

		public static void Smooth(int iterations)
		{
			UnityHeightMap unityHeightMap = new UnityHeightMap(Terrain.activeTerrain);
			unityHeightMap.Smooth(iterations);
			unityHeightMap.SaveToTerrain(Terrain.activeTerrain);
		}

		public static Vector3 GetActiveTerrainCenter(bool flushToGround = true)
		{
			Bounds bounds = default(Bounds);
			Terrain activeTerrain = GetActiveTerrain();
			if (GetTerrainBounds(activeTerrain, ref bounds))
			{
				if (flushToGround)
				{
					Vector3 center = bounds.center;
					float x = center.x;
					float y = activeTerrain.SampleHeight(bounds.center);
					Vector3 center2 = bounds.center;
					return new Vector3(x, y, center2.z);
				}
				return bounds.center;
			}
			return Vector3.zero;
		}

		public static Terrain GetActiveTerrain()
		{
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain != null && activeTerrain.isActiveAndEnabled)
			{
				return activeTerrain;
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				activeTerrain = Terrain.activeTerrains[i];
				if (activeTerrain != null && activeTerrain.isActiveAndEnabled)
				{
					return activeTerrain;
				}
			}
			return null;
		}

		public static LayerMask GetActiveTerrainLayer()
		{
			LayerMask result = default(LayerMask);
			Terrain activeTerrain = GetActiveTerrain();
			if (activeTerrain != null)
			{
				result.value = 1 << activeTerrain.gameObject.layer;
				return result;
			}
			result.value = 1 << LayerMask.NameToLayer("Default");
			return result;
		}

		public static LayerMask GetActiveTerrainLayerAsInt()
		{
			LayerMask mask = GetActiveTerrainLayer().value;
			for (int i = 0; i < 32; i++)
			{
				if ((int)mask == 1 << i)
				{
					return i;
				}
			}
			return LayerMask.NameToLayer("Default");
		}

		public static int GetActiveTerrainCount()
		{
			int num = 0;
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain != null && terrain.isActiveAndEnabled)
				{
					num++;
				}
			}
			return num;
		}

		public static Terrain GetTerrain(Vector3 locationWU)
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain != null)
			{
				vector = activeTerrain.GetPosition();
				vector2 = vector + activeTerrain.terrainData.size;
				if (locationWU.x >= vector.x && locationWU.x <= vector2.x && locationWU.z >= vector.z && locationWU.z <= vector2.z)
				{
					return activeTerrain;
				}
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				activeTerrain = Terrain.activeTerrains[i];
				vector = activeTerrain.GetPosition();
				vector2 = vector + activeTerrain.terrainData.size;
				if (locationWU.x >= vector.x && locationWU.x <= vector2.x && locationWU.z >= vector.z && locationWU.z <= vector2.z)
				{
					return activeTerrain;
				}
			}
			return null;
		}

		public static bool GetTerrainBounds(Terrain terrain, ref Bounds bounds)
		{
			if (terrain == null)
			{
				return false;
			}
			bounds.center = terrain.transform.position;
			bounds.size = terrain.terrainData.size;
			bounds.center += bounds.extents;
			return true;
		}

		public static bool GetTerrainBounds(Vector3 locationWU, ref Bounds bounds)
		{
			Terrain terrain = GetTerrain(locationWU);
			if (terrain == null)
			{
				return false;
			}
			bounds.center = terrain.transform.position;
			bounds.size = terrain.terrainData.size;
			bounds.center += bounds.extents;
			return true;
		}

		public static Vector3 GetRandomPositionOnTerrain(Terrain terrain, Vector3 start, float radius)
		{
			Vector3 position = terrain.GetPosition();
			Vector3 vector = position + terrain.terrainData.size;
			Vector3 vector2;
			do
			{
				vector2 = UnityEngine.Random.insideUnitSphere * radius;
				vector2 = start + vector2;
			}
			while (!(vector2.x >= position.x) || !(vector2.x <= vector.x) || !(vector2.z >= position.z) || !(vector2.z <= vector.z));
			vector2.y = terrain.SampleHeight(vector2);
			return vector2;
		}

		public static void ClearTrees()
		{
			List<TreeInstance> list = new List<TreeInstance>();
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				terrain.terrainData.treeInstances = list.ToArray();
				terrain.Flush();
			}
			Spawner[] array = UnityEngine.Object.FindObjectsOfType<Spawner>();
			Spawner[] array2 = array;
			foreach (Spawner spawner in array2)
			{
				spawner.SetUpSpawnerTypeFlags();
				if (spawner.IsTreeSpawner())
				{
					spawner.ResetSpawner();
				}
			}
		}

		public static void ClearDetails()
		{
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				int[,] details = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
				for (int j = 0; j < terrain.terrainData.detailPrototypes.Length; j++)
				{
					terrain.terrainData.SetDetailLayer(0, 0, j, details);
				}
				terrain.Flush();
			}
			Spawner[] array = UnityEngine.Object.FindObjectsOfType<Spawner>();
			Spawner[] array2 = array;
			foreach (Spawner spawner in array2)
			{
				if (spawner.IsDetailSpawner())
				{
					spawner.ResetSpawner();
				}
			}
		}

		public static float GetRangeFromTerrain()
		{
			Terrain activeTerrain = GetActiveTerrain();
			if (activeTerrain != null)
			{
				Vector3 size = activeTerrain.terrainData.size;
				float x = size.x;
				Vector3 size2 = activeTerrain.terrainData.size;
				return Mathf.Max(x, size2.z) / 2f;
			}
			return 0f;
		}
	}
}
