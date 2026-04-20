using System;
using UnityEngine;

namespace Gaia
{
	public class ResourceVisualiser : MonoBehaviour
	{
		[Tooltip("Choose the resources - these are the resources that will be managed.")]
		public GaiaResource m_resources;

		[Tooltip("Visualiser range - controls how far the visualiser extends. Make smaller on lower powered computers.")]
		public float m_range = 200f;

		[Tooltip("Visualiser resolution. Make larger on lower powered computers.")]
		[Range(3f, 50f)]
		public float m_resolution = 25f;

		[Tooltip("Minimum fitness - points with fitness less than this value will not be shown.")]
		[Range(0f, 1f)]
		public float m_minimumFitness;

		[Tooltip("Controls which layers are checked for collisions. Must at least include the layer the terrain is on. Add additional layers if other collisions need to be detected as well. Influences terrain detection, tree detection and game object detection.")]
		public LayerMask m_fitnessCollisionLayers;

		[Tooltip("Colour of high fitness locations.")]
		public Color m_fitColour = Color.green;

		[Tooltip("Colour of low fitness locations.")]
		public Color m_unfitColour = Color.red;

		[HideInInspector]
		public Spawner m_spawner;

		[HideInInspector]
		public Vector3 m_lastHitPoint;

		[HideInInspector]
		public string m_lastHitObjectname;

		[HideInInspector]
		public float m_lastHitFitness;

		[HideInInspector]
		public float m_lastHitHeight;

		[HideInInspector]
		public float m_lastHitTerrainHeight;

		[HideInInspector]
		public float m_lastHitTerrainRelativeHeight;

		[HideInInspector]
		public float m_lastHitTerrainSlope;

		[HideInInspector]
		public float m_lastHitAreaSlope;

		[HideInInspector]
		public bool m_lastHitWasVirgin = true;

		[HideInInspector]
		public GaiaConstants.SpawnerResourceType m_selectedResourceType;

		[HideInInspector]
		public int m_selectedResourceIdx;

		[HideInInspector]
		private DateTime m_lastUpdateDate = DateTime.Now;

		[HideInInspector]
		private DateTime m_lastCacheUpdateDate = DateTime.Now;

		private UnityHeightMap m_terrainHeightMap;

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
		}

		private void OnEnable()
		{
			Initialise();
		}

		public void Initialise()
		{
			m_fitnessCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			m_spawner = GetComponent<Spawner>();
			if (m_spawner == null)
			{
				m_spawner = base.gameObject.AddComponent<Spawner>();
				m_spawner.m_spawnCollisionLayers = m_fitnessCollisionLayers;
				m_spawner.hideFlags = HideFlags.HideInInspector;
				m_spawner.m_resources = m_resources;
				m_spawner.m_spawnRange = m_range;
				m_spawner.m_showGizmos = false;
				m_spawner.Initialise();
			}
			else
			{
				m_spawner.m_spawnCollisionLayers = m_fitnessCollisionLayers;
				m_spawner.Initialise();
			}
		}

		public void Visualise()
		{
			m_terrainHeightMap = new UnityHeightMap(TerrainHelper.GetActiveTerrain());
		}

		public SpawnInfo GetSpawnInfo(Vector3 location)
		{
			SpawnInfo spawnInfo = new SpawnInfo();
			spawnInfo.m_textureStrengths = new float[Terrain.activeTerrain.terrainData.alphamapLayers];
			if (m_spawner.CheckLocation(location, ref spawnInfo))
			{
				spawnInfo.m_fitness = GetFitness(ref spawnInfo);
			}
			else
			{
				spawnInfo.m_fitness = 0f;
			}
			return spawnInfo;
		}

		public float GetFitness(ref SpawnInfo spawnInfo)
		{
			SpawnCritera[] spawnCriteria;
			switch (m_selectedResourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_texturePrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			default:
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			}
			if (spawnCriteria == null || spawnCriteria.Length == 0)
			{
				return 0f;
			}
			float num = float.MinValue;
			float num2 = 0f;
			foreach (SpawnCritera spawnCritera in spawnCriteria)
			{
				if (spawnCritera.m_checkType == GaiaConstants.SpawnerLocationCheckType.BoundedAreaCheck && !spawnInfo.m_spawner.CheckLocationBounds(ref spawnInfo, GetMaxScaledRadius(ref spawnInfo)))
				{
					return 0f;
				}
				num2 = spawnCritera.GetFitness(ref spawnInfo);
				if (num2 > num)
				{
					num = num2;
					if (num >= 1f)
					{
						return num;
					}
				}
			}
			if (num == float.MinValue)
			{
				return 0f;
			}
			return num;
		}

		public float GetMinFitness(ref SpawnInfo spawnInfo)
		{
			SpawnCritera[] spawnCriteria;
			switch (m_selectedResourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				if (m_selectedResourceIdx >= spawnInfo.m_spawner.m_resources.m_detailPrototypes.Length)
				{
					return 0f;
				}
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				if (m_selectedResourceIdx >= spawnInfo.m_spawner.m_resources.m_texturePrototypes.Length)
				{
					return 0f;
				}
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_texturePrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				if (m_selectedResourceIdx >= spawnInfo.m_spawner.m_resources.m_treePrototypes.Length)
				{
					return 0f;
				}
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			default:
				if (m_selectedResourceIdx >= spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes.Length)
				{
					return 0f;
				}
				spawnCriteria = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_selectedResourceIdx].m_spawnCriteria;
				break;
			}
			if (spawnCriteria == null || spawnCriteria.Length == 0)
			{
				return 0f;
			}
			float num = float.MaxValue;
			float num2 = 0f;
			foreach (SpawnCritera spawnCritera in spawnCriteria)
			{
				if (spawnCritera.m_checkType == GaiaConstants.SpawnerLocationCheckType.BoundedAreaCheck && !spawnInfo.m_spawner.CheckLocationBounds(ref spawnInfo, GetMaxScaledRadius(ref spawnInfo)))
				{
					return 0f;
				}
				num2 = spawnCritera.GetFitness(ref spawnInfo);
				if (num2 < num)
				{
					num = num2;
					if (num <= 0f)
					{
						return num;
					}
				}
			}
			if (num == float.MaxValue)
			{
				return 0f;
			}
			return num;
		}

		public float GetMaxScaledRadius(ref SpawnInfo spawnInfo)
		{
			switch (m_selectedResourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				return 1f;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				return spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_selectedResourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_selectedResourceIdx].m_dna.m_maxScale;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				return spawnInfo.m_spawner.m_resources.m_treePrototypes[m_selectedResourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_treePrototypes[m_selectedResourceIdx].m_dna.m_maxScale;
			default:
				return spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_selectedResourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_selectedResourceIdx].m_dna.m_maxScale;
			}
		}

		private void OnDrawGizmos()
		{
			if (m_resources == null || m_spawner == null || m_terrainHeightMap == null)
			{
				return;
			}
			Vector3 position = base.transform.position;
			float y = position.y;
			Vector3 position2 = base.transform.position;
			float num = position2.x - m_range;
			Vector3 position3 = base.transform.position;
			float num2 = position3.x + m_range;
			Vector3 position4 = base.transform.position;
			float num3 = position4.z - m_range;
			Vector3 position5 = base.transform.position;
			float num4 = position5.z + m_range;
			float radius = Mathf.Clamp(m_resolution * 0.25f, 0.5f, 5f);
			m_spawner.m_spawnRange = m_range;
			m_spawner.m_spawnerBounds = new Bounds(base.transform.position, new Vector3(m_range * 2f, m_range * 20f, m_range * 2f));
			SpawnInfo spawnInfo = new SpawnInfo();
			Vector3 locationWU = default(Vector3);
			float num5 = 0f;
			if ((DateTime.Now - m_lastCacheUpdateDate).TotalSeconds > 5.0)
			{
				m_lastCacheUpdateDate = DateTime.Now;
				m_spawner.DeleteSpawnCaches();
				m_spawner.CreateSpawnCaches(m_selectedResourceType, m_selectedResourceIdx);
				Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
				if (terrain != null)
				{
					Transform transform = base.transform;
					Vector3 position6 = base.transform.position;
					float x = position6.x;
					float y2 = terrain.SampleHeight(base.transform.position) + 5f;
					Vector3 position7 = base.transform.position;
					transform.position = new Vector3(x, y2, position7.z);
				}
			}
			spawnInfo.m_textureStrengths = new float[Terrain.activeTerrain.terrainData.alphamapLayers];
			for (float num6 = num; num6 < num2; num6 += m_resolution)
			{
				for (float num7 = num3; num7 < num4; num7 += m_resolution)
				{
					locationWU.Set(num6, y, num7);
					if (m_spawner.CheckLocation(locationWU, ref spawnInfo))
					{
						num5 = GetFitness(ref spawnInfo);
						if (!(num5 < m_minimumFitness))
						{
							Gizmos.color = Color.Lerp(m_unfitColour, m_fitColour, num5);
							Gizmos.DrawSphere(spawnInfo.m_hitLocationWU, radius);
						}
					}
				}
			}
			if (m_resources != null)
			{
				Bounds bounds = default(Bounds);
				if (TerrainHelper.GetTerrainBounds(base.transform.position, ref bounds))
				{
					Vector3 center = bounds.center;
					float x2 = center.x;
					float seaLevel = m_resources.m_seaLevel;
					Vector3 center2 = bounds.center;
					bounds.center = new Vector3(x2, seaLevel, center2.z);
					Vector3 size = bounds.size;
					float x3 = size.x;
					Vector3 size2 = bounds.size;
					bounds.size = new Vector3(x3, 0.05f, size2.z);
					Color blue = Color.blue;
					float r = blue.r;
					Color blue2 = Color.blue;
					float g = blue2.g;
					Color blue3 = Color.blue;
					float b = blue3.b;
					Color blue4 = Color.blue;
					Gizmos.color = new Color(r, g, b, blue4.a / 4f);
					Gizmos.DrawCube(bounds.center, bounds.size);
				}
			}
		}
	}
}
