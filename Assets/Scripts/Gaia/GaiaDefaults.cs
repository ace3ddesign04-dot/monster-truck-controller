using Gaia.FullSerializer;
using System;
using System.Text;
using UnityEngine;

namespace Gaia
{
	public class GaiaDefaults : ScriptableObject
	{
		[Tooltip("Unique identifier for these defaults.")]
		[HideInInspector]
		public string m_defaultsID = Guid.NewGuid().ToString();

		[Tooltip("The absolute height of the sea or water table in meters. All spawn criteria heights are calculated relative to this. Used to populate initial sea level in new resources files.")]
		public float m_seaLevel = 50f;

		[Tooltip("The beach height in meters. Beaches are spawned at sea level and are extended for this height above sea level. This is used when creating default spawn rules in order to create a beach in the zone between water and land. Only used to populate initial beach height in new resources files.")]
		public float m_beachHeight = 5f;

		[Range(1f, 20f)]
		[Tooltip("Number of tiles in X direction.")]
		[HideInInspector]
		public int m_tilesX = 1;

		[Range(1f, 20f)]
		[Tooltip("Number of tiles in Z direction.")]
		[HideInInspector]
		public int m_tilesZ = 1;

		[Header("Base Terrain:")]
		[Space(5f)]
		[Tooltip("The accuracy of the mapping between the terrain maps (heightmap, textures, etc) and the generated terrain; higher values indicate lower accuracy but lower rendering overhead.")]
		[Range(1f, 200f)]
		public int m_pixelError = 5;

		[Tooltip("The maximum distance at which terrain textures will be displayed at full resolution. Beyond this distance, a lower resolution composite image will be used for efficiency.")]
		[Range(0f, 2000f)]
		public int m_baseMapDist = 1024;

		[Tooltip("Whether or not the terrain casts shadows.")]
		public bool m_castShadows = true;

		[Tooltip("The material used to render the terrain. This should use a suitable shader, for example Nature/Terrain/Diffuse. The default terrain shader is used if no material is supplied.")]
		public Material m_material;

		[Tooltip("The Physic Material used for the terrain surface to specify its friction and bounce.")]
		public PhysicMaterial m_physicsMaterial;

		[Header("Tree & Detail Objects:")]
		[Space(5f)]
		[Tooltip("Draw trees, grass & details.")]
		public bool m_draw = true;

		[Tooltip("The distance (from camera) beyond which details will be culled.")]
		[Range(0f, 250f)]
		public int m_detailDistance = 120;

		[Tooltip("The number of detail/grass objects in a given unit of area. The value can be set lower to reduce rendering overhead.")]
		[Range(0f, 1f)]
		public float m_detailDensity = 1f;

		[Tooltip("The distance (from camera) beyond which trees will be culled.")]
		[Range(0f, 2000f)]
		public int m_treeDistance = 500;

		[Tooltip("The distance (from camera) at which 3D tree objects will be replaced by billboard images.")]
		[Range(5f, 2000f)]
		public int m_billboardStart = 50;

		[Tooltip("Distance over which trees will transition between 3D objects and billboards.There is often a rotation effect as this kicks in.")]
		[Range(0f, 200f)]
		public int m_fadeLength = 20;

		[Tooltip("The maximum number of visible trees that will be represented as solid 3D meshes. Beyond this limit, trees will be replaced with billboards.")]
		[Range(0f, 10000f)]
		public int m_maxMeshTrees = 50;

		[Header("Wind Settings:")]
		[Space(5f)]
		[Tooltip("The speed of the wind as it blows grass.")]
		[Range(0f, 1f)]
		public float m_speed = 0.35f;

		[Tooltip("The size of the “ripples” on grassy areas as the wind blows over them.")]
		[Range(0f, 1f)]
		public float m_size = 0.12f;

		[Tooltip("The degree to which grass objects are bent over by the wind.")]
		[Range(0f, 1f)]
		public float m_bending = 0.1f;

		[Tooltip("Overall color tint applied to grass objects.")]
		public Color m_grassTint = new Color(0.7058824f, 0.7058824f, 0.7058824f, 1f);

		[Header("Resolution Settings:")]
		[Space(5f)]
		[Tooltip("The size of terrain tile in X & Z axis (in world units).")]
		public int m_terrainSize = 2048;

		[Tooltip("The height of the terrain in world unit meters")]
		public int m_terrainHeight = 700;

		[Tooltip("Pixel resolution of the terrain’s heightmap (should be a power of two plus one e.g. 513 = 512 + 1). Higher resolutions allow for more detailed terrain features, at the cost of poorer performance.")]
		public int m_heightmapResolution = 1025;

		[Tooltip("Resolution of the map that determines the separate patches of details/grass. Higher resolution gives smaller and more detailed patches.")]
		public int m_detailResolution = 1024;

		[Tooltip("Length/width of the square of patches rendered with a single draw call.")]
		public int m_detailResolutionPerPatch = 8;

		[Tooltip("Resolution of the “splatmap” that controls the blending of the different terrain textures. Higher resolutions consumer more memory, but provide more accurate texturing.")]
		public int m_controlTextureResolution = 1024;

		[Tooltip("Resolution of the composite texture used on the terrain when viewed from a distance greater than the Basemap Distance (see above).")]
		public int m_baseMapSize = 1024;

		public void CreateTerrain()
		{
			GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
			if (sessionManager != null && !sessionManager.IsLocked())
			{
				sessionManager.m_session.m_terrainWidth = m_tilesX * m_terrainSize;
				sessionManager.m_session.m_terrainDepth = m_tilesZ * m_terrainSize;
				sessionManager.m_session.m_terrainHeight = m_terrainHeight;
				sessionManager.AddDefaults(this);
				sessionManager.SetSeaLevel(m_seaLevel);
				GaiaOperation gaiaOperation = new GaiaOperation();
				gaiaOperation.m_description = "Creating terrain";
				gaiaOperation.m_generatedByID = m_defaultsID;
				gaiaOperation.m_generatedByName = base.name;
				gaiaOperation.m_generatedByType = GetType().ToString();
				gaiaOperation.m_isActive = true;
				gaiaOperation.m_operationDateTime = DateTime.Now.ToString();
				gaiaOperation.m_operationType = GaiaOperation.OperationType.CreateTerrain;
				sessionManager.AddOperation(gaiaOperation);
			}
			Terrain[,] world = new Terrain[m_tilesX, m_tilesZ];
			for (int i = 0; i < m_tilesX; i++)
			{
				for (int j = 0; j < m_tilesZ; j++)
				{
					CreateTile(i, j, ref world, null);
				}
			}
			RemoveWorldSeams(ref world);
		}

		public void UpdateFromTerrain()
		{
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain == null)
			{
				UnityEngine.Debug.Log("Could not update from active terrain - no current active terrain");
				return;
			}
			m_baseMapDist = (int)activeTerrain.basemapDistance;
			m_castShadows = activeTerrain.castShadows;
			m_detailDensity = activeTerrain.detailObjectDensity;
			m_detailDistance = (int)activeTerrain.detailObjectDistance;
			m_pixelError = (int)activeTerrain.heightmapPixelError;
			m_billboardStart = (int)activeTerrain.treeBillboardDistance;
			m_fadeLength = (int)activeTerrain.treeCrossFadeLength;
			m_treeDistance = (int)activeTerrain.treeDistance;
			m_maxMeshTrees = activeTerrain.treeMaximumFullLODCount;
			if (activeTerrain.materialType == Terrain.MaterialType.Custom)
			{
				m_material = activeTerrain.materialTemplate;
			}
			TerrainCollider component = activeTerrain.GetComponent<TerrainCollider>();
			if (component != null)
			{
				m_physicsMaterial = component.material;
			}
			TerrainData terrainData = activeTerrain.terrainData;
			m_controlTextureResolution = terrainData.alphamapResolution;
			m_baseMapSize = terrainData.baseMapResolution;
			m_detailResolution = terrainData.detailResolution;
			m_heightmapResolution = terrainData.heightmapResolution;
			m_bending = terrainData.wavingGrassAmount;
			m_size = terrainData.wavingGrassSpeed;
			m_speed = terrainData.wavingGrassStrength;
			m_grassTint = terrainData.wavingGrassTint;
			Vector3 size = terrainData.size;
			m_terrainSize = (int)size.x;
			Vector3 size2 = terrainData.size;
			m_terrainHeight = (int)size2.y;
		}

		public void CreateTerrain(GaiaResource resources)
		{
			resources.AssociateAssets();
			GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
			if (sessionManager != null && !sessionManager.IsLocked())
			{
				sessionManager.m_session.m_terrainWidth = m_tilesX * m_terrainSize;
				sessionManager.m_session.m_terrainDepth = m_tilesZ * m_terrainSize;
				sessionManager.m_session.m_terrainHeight = m_terrainHeight;
				sessionManager.AddDefaults(this);
				sessionManager.SetSeaLevel(m_seaLevel);
				sessionManager.AddResource(resources);
				resources.ChangeSeaLevel(m_seaLevel);
				sessionManager.AddOperation(GetTerrainCreationOperation(resources));
			}
			Terrain[,] world = new Terrain[m_tilesX, m_tilesZ];
			for (int i = 0; i < m_tilesX; i++)
			{
				for (int j = 0; j < m_tilesZ; j++)
				{
					CreateTile(i, j, ref world, resources);
				}
			}
			RemoveWorldSeams(ref world);
		}

		public GaiaOperation GetTerrainCreationOperation(GaiaResource resources)
		{
			GaiaOperation gaiaOperation = new GaiaOperation();
			gaiaOperation.m_description = "Creating terrain";
			gaiaOperation.m_generatedByID = m_defaultsID;
			gaiaOperation.m_generatedByName = base.name;
			gaiaOperation.m_generatedByType = GetType().ToString();
			gaiaOperation.m_isActive = true;
			gaiaOperation.m_operationDateTime = DateTime.Now.ToString();
			gaiaOperation.m_operationType = GaiaOperation.OperationType.CreateTerrain;
			return gaiaOperation;
		}

		private void CreateTile(int tx, int tz, ref Terrain[,] world, GaiaResource resources)
		{
			if (tx < 0 || tx >= m_tilesX)
			{
				UnityEngine.Debug.LogError("X value out of bounds");
				return;
			}
			if (tz < 0 || tz >= m_tilesZ)
			{
				UnityEngine.Debug.LogError("Z value out of bounds");
				return;
			}
			GetAndFixDefaults();
			Vector2 vector = new Vector2((float)(-m_terrainSize * m_tilesX) * 0.5f, (float)(-m_terrainSize * m_tilesZ) * 0.5f);
			if (world.Length < m_tilesX)
			{
				world = new Terrain[m_tilesX, m_tilesZ];
			}
			TerrainData terrainData = new TerrainData();
			terrainData.name = $"Terrain_{tx}_{tz}-{DateTime.Now:yyyyMMdd-HHmmss}";
			terrainData.alphamapResolution = m_controlTextureResolution;
			terrainData.baseMapResolution = m_baseMapSize;
			terrainData.SetDetailResolution(m_detailResolution, m_detailResolutionPerPatch);
			terrainData.heightmapResolution = m_heightmapResolution;
			terrainData.wavingGrassAmount = m_bending;
			terrainData.wavingGrassSpeed = m_size;
			terrainData.wavingGrassStrength = m_speed;
			terrainData.wavingGrassTint = m_grassTint;
			terrainData.size = new Vector3(m_terrainSize, m_terrainHeight, m_terrainSize);
			Terrain component = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
			component.name = terrainData.name;
			component.transform.position = new Vector3((float)(m_terrainSize * tx) + vector.x, 0f, (float)(m_terrainSize * tz) + vector.y);
			component.basemapDistance = m_baseMapDist;
			component.castShadows = m_castShadows;
			component.detailObjectDensity = m_detailDensity;
			component.detailObjectDistance = m_detailDistance;
			component.heightmapPixelError = m_pixelError;
			component.treeBillboardDistance = m_billboardStart;
			component.treeCrossFadeLength = m_fadeLength;
			component.treeDistance = m_treeDistance;
			component.treeMaximumFullLODCount = m_maxMeshTrees;
			if (m_material != null)
			{
				component.materialType = Terrain.MaterialType.Custom;
				component.materialTemplate = m_material;
			}
			if (m_physicsMaterial != null)
			{
				TerrainCollider component2 = component.GetComponent<TerrainCollider>();
				if (component2 != null)
				{
					component2.material = m_physicsMaterial;
				}
				else
				{
					UnityEngine.Debug.LogWarning("Unable to assign physics material to terrain!");
				}
			}
			if (resources != null)
			{
				resources.ApplyPrototypesToTerrain(component);
			}
			else
			{
				component.Flush();
			}
			world[tx, tz] = component;
			GameObject gameObject = GameObject.Find("Gaia Environment");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia Environment");
			}
			component.transform.parent = gameObject.transform;
		}

		private void RemoveWorldSeams(ref Terrain[,] world)
		{
			for (int i = 0; i < m_tilesX; i++)
			{
				for (int j = 0; j < m_tilesZ; j++)
				{
					Terrain right = null;
					Terrain left = null;
					Terrain bottom = null;
					Terrain top = null;
					if (i > 0)
					{
						left = world[i - 1, j];
					}
					if (i < m_tilesX - 1)
					{
						right = world[i + 1, j];
					}
					if (j > 0)
					{
						bottom = world[i, j - 1];
					}
					if (j < m_tilesZ - 1)
					{
						top = world[i, j + 1];
					}
					world[i, j].SetNeighbors(left, top, right, bottom);
				}
			}
		}

		public string GetAndFixDefaults()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!Mathf.IsPowerOfTwo(m_terrainSize))
			{
				stringBuilder.AppendFormat("Terrain size must be power of 2! {0} was changed to {1}.\n", m_terrainSize, Mathf.ClosestPowerOfTwo(m_terrainSize));
				m_terrainSize = Mathf.ClosestPowerOfTwo(m_terrainSize);
			}
			if (!Mathf.IsPowerOfTwo(m_heightmapResolution - 1))
			{
				stringBuilder.AppendFormat("Height map size must be power of 2 + 1 number! {0} was changed to {1}.\n", m_heightmapResolution, Mathf.ClosestPowerOfTwo(m_heightmapResolution) + 1);
				m_heightmapResolution = Mathf.ClosestPowerOfTwo(m_heightmapResolution) + 1;
			}
			if (!Mathf.IsPowerOfTwo(m_controlTextureResolution))
			{
				stringBuilder.AppendFormat("Control texture resolution must be power of 2! {0} was changed to {1}.\n", m_controlTextureResolution, Mathf.ClosestPowerOfTwo(m_controlTextureResolution));
				m_controlTextureResolution = Mathf.ClosestPowerOfTwo(m_controlTextureResolution);
			}
			if (m_controlTextureResolution > 2048)
			{
				stringBuilder.AppendFormat("Control texture resolution must be <= 2048! {0} was changed to {1}.\n", m_controlTextureResolution, 2048);
				m_controlTextureResolution = 2048;
			}
			if (!Mathf.IsPowerOfTwo(m_baseMapSize))
			{
				stringBuilder.AppendFormat("Basemap size must be power of 2! {0} was changed to {1}.\n", m_baseMapSize, Mathf.ClosestPowerOfTwo(m_baseMapSize));
				m_baseMapSize = Mathf.ClosestPowerOfTwo(m_baseMapSize);
			}
			if (m_baseMapSize > 2048)
			{
				stringBuilder.AppendFormat("Basemap size must be <= 2048! {0} was changed to {1}.\n", m_baseMapSize, 2048);
				m_baseMapSize = 2048;
			}
			if (!Mathf.IsPowerOfTwo(m_detailResolution))
			{
				stringBuilder.AppendFormat("Detail map size must be power of 2! {0} was changed to {1}.\n", m_detailResolution, Mathf.ClosestPowerOfTwo(m_detailResolution));
				m_detailResolution = Mathf.ClosestPowerOfTwo(m_detailResolution);
			}
			if (m_detailResolutionPerPatch < 8)
			{
				stringBuilder.AppendFormat("Detail resolution per patch must be >= 8! {0} was changed to {1}.\n", m_detailResolutionPerPatch, 8);
				m_detailResolutionPerPatch = 8;
			}
			return stringBuilder.ToString();
		}

		public string SerialiseJson()
		{
			fsSerializer fsSerializer = new fsSerializer();
			fsSerializer.TrySerialize(this, out fsData data);
			return fsJsonPrinter.CompressedJson(data);
		}

		public void DeSerialiseJson(string json)
		{
			fsData data = fsJsonParser.Parse(json);
			fsSerializer fsSerializer = new fsSerializer();
			GaiaDefaults instance = this;
			fsSerializer.TryDeserialize(data, ref instance);
		}
	}
}
