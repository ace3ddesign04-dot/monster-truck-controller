using Gaia.FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[ExecuteInEditMode]
	public class Spawner : MonoBehaviour
	{
		private class SpawnLocation
		{
			public Vector3 m_location;

			public float m_seedDistance;
		}

		[fsIgnore]
		public GaiaResource m_resources;

		public string m_resourcesPath;

		public string m_spawnerID = Guid.NewGuid().ToString();

		public GaiaConstants.OperationMode m_mode;

		public int m_seed = DateTime.Now.Millisecond;

		public GaiaConstants.SpawnerShape m_spawnerShape;

		public GaiaConstants.SpawnerRuleSelector m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.WeightedFittest;

		public GaiaConstants.SpawnerLocation m_spawnLocationAlgorithm;

		public GaiaConstants.SpawnerLocationCheckType m_spawnLocationCheckType;

		public float m_locationIncrement = 1f;

		public float m_maxJitteredLocationOffsetPct = 0.9f;

		public int m_locationChecksPerInt = 1;

		public int m_maxRandomClusterSize = 50;

		public float m_spawnRange = 500f;

		public AnimationCurve m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		public GaiaConstants.ImageFitnessFilterMode m_areaMaskMode;

		public bool m_enableColliderCacheAtRuntime;

		public Texture2D m_imageMask;

		public bool m_imageMaskInvert;

		public bool m_imageMaskNormalise;

		public bool m_imageMaskFlip;

		public int m_imageMaskSmoothIterations = 3;

		[fsIgnore]
		public HeightMap m_imageMaskHM;

		private FractalGenerator m_noiseGenerator;

		public float m_noiseMaskSeed;

		public int m_noiseMaskOctaves = 8;

		public float m_noiseMaskPersistence = 0.25f;

		public float m_noiseMaskFrequency = 1f;

		public float m_noiseMaskLacunarity = 1.5f;

		public float m_noiseZoom = 10f;

		public bool m_noiseInvert;

		public float m_spawnInterval = 5f;

		public string m_triggerTags = "Player";

		public float m_triggerRange = 130f;

		public List<SpawnRule> m_spawnerRules = new List<SpawnRule>();

		public LayerMask m_spawnCollisionLayers;

		public int m_spawnColliderLayer;

		public bool m_showGizmos = true;

		public bool m_showDebug;

		public bool m_showStatistics = true;

		public bool m_showTerrainHelper = true;

		public XorshiftPlus m_rndGenerator;

		private XorshiftPlus m_rndSubGenerator;

		private bool m_cacheDetails;

		private Dictionary<int, List<HeightMap>> m_detailMapCache = new Dictionary<int, List<HeightMap>>();

		private bool m_cacheTextures;

		private bool m_textureMapsDirty;

		private Dictionary<int, List<HeightMap>> m_textureMapCache = new Dictionary<int, List<HeightMap>>();

		private bool m_cacheTags;

		private Dictionary<string, Quadtree<GameObject>> m_taggedGameObjectCache = new Dictionary<string, Quadtree<GameObject>>();

		private bool m_cacheHeightMaps;

		private bool m_heightMapDirty;

		private Dictionary<int, UnityHeightMap> m_heightMapCache = new Dictionary<int, UnityHeightMap>();

		private Dictionary<string, HeightMap> m_stampCache = new Dictionary<string, HeightMap>();

		[fsIgnore]
		public GameObject m_areaBoundsColliderCache;

		[fsIgnore]
		public GameObject m_goColliderCache;

		[fsIgnore]
		public GameObject m_goParentGameObject;

		private bool m_cancelSpawn;

		public int m_totalRuleCnt;

		public int m_activeRuleCnt;

		public int m_inactiveRuleCnt;

		public ulong m_maxInstanceCnt;

		public ulong m_activeInstanceCnt;

		public ulong m_inactiveInstanceCnt;

		public ulong m_totalInstanceCnt;

		private float m_terrainHeight;

		private float m_checkDistance;

		private RaycastHit m_checkHitInfo = default(RaycastHit);

		public IEnumerator m_updateCoroutine;

		public float m_updateTimeAllowed = 71f / (678f * (float)Math.PI);

		public float m_spawnProgress;

		public bool m_spawnComplete = true;

		public Bounds m_spawnerBounds = default(Bounds);

		private bool m_isTextureSpawner;

		private bool m_isDetailSpawner;

		private bool m_isTreeSpawnwer;

		private bool m_isGameObjectSpawner;

		private void OnEnable()
		{
			if (m_spawnCollisionLayers.value == 0)
			{
				m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			}
			m_spawnColliderLayer = TerrainHelper.GetActiveTerrainLayerAsInt();
			if (m_rndGenerator == null)
			{
				m_rndGenerator = new XorshiftPlus(m_seed);
			}
		}

		private void OnDisable()
		{
		}

		public void StartEditorUpdates()
		{
		}

		public void StopEditorUpdates()
		{
		}

		private void EditorUpdate()
		{
		}

		private void Start()
		{
			if (Application.isPlaying)
			{
				Transform transform = base.transform.Find("Bounds_ColliderCache");
				if (transform != null)
				{
					m_areaBoundsColliderCache = transform.gameObject;
					m_areaBoundsColliderCache.SetActive(value: false);
				}
				if (!m_enableColliderCacheAtRuntime)
				{
					transform = base.transform.Find("GameObject_ColliderCache");
					if (transform != null)
					{
						m_goColliderCache = transform.gameObject;
						m_goColliderCache.SetActive(value: false);
					}
				}
			}
			if (m_mode == GaiaConstants.OperationMode.RuntimeInterval || m_mode == GaiaConstants.OperationMode.RuntimeTriggeredInterval)
			{
				Initialise();
				InvokeRepeating("RunSpawnerIteration", 1f, m_spawnInterval);
			}
		}

		public void Initialise()
		{
			if (m_showDebug)
			{
				UnityEngine.Debug.Log("Initialising spawner");
			}
			m_spawnColliderLayer = TerrainHelper.GetActiveTerrainLayerAsInt();
			List<Transform> list = new List<Transform>();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform item = (Transform)enumerator.Current;
					list.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			foreach (Transform item2 in list)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(item2.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(item2.gameObject);
				}
			}
			SetUpSpawnerTypeFlags();
			if (IsGameObjectSpawner())
			{
				m_goParentGameObject = new GameObject("Spawned_GameObjects");
				m_goParentGameObject.transform.parent = base.transform;
			}
			if (IsTreeSpawner() || IsGameObjectSpawner())
			{
				m_areaBoundsColliderCache = new GameObject("Bounds_ColliderCache");
				m_areaBoundsColliderCache.transform.parent = base.transform;
			}
			if (IsGameObjectSpawner())
			{
				m_goColliderCache = new GameObject("GameObject_ColliderCache");
				m_goColliderCache.transform.parent = base.transform;
			}
			ResetRandomGenertor();
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain != null)
			{
				Vector3 size = terrain.terrainData.size;
				m_terrainHeight = size.y;
			}
			m_spawnerBounds = new Bounds(base.transform.position, new Vector3(m_spawnRange * 2f, m_spawnRange * 2f, m_spawnRange * 2f));
			foreach (SpawnRule spawnerRule in m_spawnerRules)
			{
				spawnerRule.m_currInstanceCnt = 0uL;
				spawnerRule.m_activeInstanceCnt = 0uL;
				spawnerRule.m_inactiveInstanceCnt = 0uL;
			}
			UpdateCounters();
		}

		private void PreSpawnInitialise()
		{
			m_spawnerBounds = new Bounds(base.transform.position, new Vector3(m_spawnRange * 2f, m_spawnRange * 2f, m_spawnRange * 2f));
			if (m_rndGenerator == null)
			{
				ResetRandomGenertor();
			}
			m_spawnColliderLayer = TerrainHelper.GetActiveTerrainLayerAsInt();
			SetUpSpawnerTypeFlags();
			if (IsGameObjectSpawner() && base.transform.Find("Spawned_GameObjects") == null)
			{
				m_goParentGameObject = new GameObject("Spawned_GameObjects");
				m_goParentGameObject.transform.parent = base.transform;
			}
			if ((IsTreeSpawner() || IsGameObjectSpawner()) && base.transform.Find("Bounds_ColliderCache") == null)
			{
				m_areaBoundsColliderCache = new GameObject("Bounds_ColliderCache");
				m_areaBoundsColliderCache.transform.parent = base.transform;
			}
			if (IsGameObjectSpawner() && base.transform.Find("GameObject_ColliderCache") == null)
			{
				m_goColliderCache = new GameObject("GameObject_ColliderCache");
				m_goColliderCache.transform.parent = base.transform;
			}
			foreach (SpawnRule spawnerRule in m_spawnerRules)
			{
				spawnerRule.Initialise(this);
			}
			if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.PerlinNoise)
			{
				m_noiseGenerator = new FractalGenerator(m_noiseMaskFrequency, m_noiseMaskLacunarity, m_noiseMaskOctaves, m_noiseMaskPersistence, m_noiseMaskSeed, FractalGenerator.Fractals.Perlin);
			}
			else if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.BillowNoise)
			{
				m_noiseGenerator = new FractalGenerator(m_noiseMaskFrequency, m_noiseMaskLacunarity, m_noiseMaskOctaves, m_noiseMaskPersistence, m_noiseMaskSeed, FractalGenerator.Fractals.Billow);
			}
			else if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.RidgedNoise)
			{
				m_noiseGenerator = new FractalGenerator(m_noiseMaskFrequency, m_noiseMaskLacunarity, m_noiseMaskOctaves, m_noiseMaskPersistence, m_noiseMaskSeed, FractalGenerator.Fractals.RidgeMulti);
			}
			UpdateCounters();
		}

		public void SetUpSpawnerTypeFlags()
		{
			m_isDetailSpawner = false;
			for (int i = 0; i < m_spawnerRules.Count; i++)
			{
				if (m_spawnerRules[i].m_resourceType == GaiaConstants.SpawnerResourceType.TerrainDetail)
				{
					m_isDetailSpawner = true;
					break;
				}
			}
			m_isTextureSpawner = false;
			for (int j = 0; j < m_spawnerRules.Count; j++)
			{
				if (m_spawnerRules[j].m_resourceType == GaiaConstants.SpawnerResourceType.TerrainTexture)
				{
					m_isTextureSpawner = true;
					break;
				}
			}
			for (int k = 0; k < m_spawnerRules.Count; k++)
			{
				if (m_spawnerRules[k].m_resourceType == GaiaConstants.SpawnerResourceType.TerrainTree)
				{
					m_isTreeSpawnwer = true;
					break;
				}
			}
			m_isGameObjectSpawner = false;
			int num = 0;
			while (true)
			{
				if (num < m_spawnerRules.Count)
				{
					if (m_spawnerRules[num].m_resourceType == GaiaConstants.SpawnerResourceType.GameObject)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_isGameObjectSpawner = true;
		}

		public void AssociateAssets()
		{
			if (m_resources != null)
			{
				m_resources.AssociateAssets();
			}
			else
			{
				UnityEngine.Debug.LogWarning("Could not associated assets for " + base.name + " - resources file was missing");
			}
		}

		public int[] GetMissingResources()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < m_spawnerRules.Count; i++)
			{
				if (m_spawnerRules[i].m_isActive && !m_spawnerRules[i].ResourceIsLoadedInTerrain(this))
				{
					list.Add(i);
				}
			}
			return list.ToArray();
		}

		public void AddResourcesToTerrain(int[] rules)
		{
			for (int i = 0; i < rules.GetLength(0); i++)
			{
				if (!m_spawnerRules[rules[i]].ResourceIsLoadedInTerrain(this))
				{
					m_spawnerRules[rules[i]].AddResourceToTerrain(this);
				}
			}
		}

		private void PostSpawn()
		{
			m_spawnProgress = 0f;
			m_spawnComplete = true;
			m_updateCoroutine = null;
			UpdateCounters();
		}

		public bool IsTextureSpawner()
		{
			return m_isTextureSpawner;
		}

		public bool IsDetailSpawner()
		{
			return m_isDetailSpawner;
		}

		public bool IsTreeSpawner()
		{
			return m_isTreeSpawnwer;
		}

		public bool IsGameObjectSpawner()
		{
			return m_isGameObjectSpawner;
		}

		public void ResetSpawner()
		{
			Initialise();
		}

		public void CancelSpawn()
		{
			m_cancelSpawn = true;
			m_spawnProgress = 0f;
		}

		public bool IsSpawning()
		{
			return !m_spawnComplete;
		}

		private bool CanSpawnInstances()
		{
			bool result = false;
			for (int i = 0; i < m_spawnerRules.Count; i++)
			{
				SpawnRule spawnRule = m_spawnerRules[i];
				if (spawnRule.m_isActive)
				{
					if (spawnRule.m_ignoreMaxInstances)
					{
						return true;
					}
					if (spawnRule.m_activeInstanceCnt < spawnRule.m_maxInstances)
					{
						return true;
					}
				}
			}
			return result;
		}

		public void RunSpawnerIteration()
		{
			m_cancelSpawn = false;
			m_spawnComplete = false;
			PreSpawnInitialise();
			if (m_activeRuleCnt <= 0)
			{
				if (m_showDebug)
				{
					UnityEngine.Debug.Log($"{base.gameObject.name}: There are no active spawn rules. Can't spawn without rules.");
				}
				m_spawnComplete = true;
				return;
			}
			if (!CanSpawnInstances())
			{
				if (m_showDebug)
				{
					UnityEngine.Debug.Log($"{base.gameObject.name}: Can't spawn or activate new instance - max instance count reached.");
				}
				m_spawnComplete = true;
				return;
			}
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain != null)
			{
				Vector3 size = terrain.terrainData.size;
				m_terrainHeight = size.y;
				if (m_resources != null && m_resources.m_terrainHeight != m_terrainHeight)
				{
					UnityEngine.Debug.LogWarning($"There is a mismatch between your resources Terrain Height {m_resources.m_terrainHeight} and your actual Terrain Height {m_terrainHeight}. Your Spawn may not work as intended!");
				}
			}
			if (m_mode == GaiaConstants.OperationMode.RuntimeTriggeredInterval)
			{
				m_checkDistance = m_triggerRange + 1f;
				List<GameObject> list = new List<GameObject>();
				string[] array = new string[0];
				if (!string.IsNullOrEmpty(m_triggerTags))
				{
					array = m_triggerTags.Split(',');
				}
				else
				{
					UnityEngine.Debug.LogError("You have not supplied a trigger tag. Spawner will not spawn!");
				}
				int num = 0;
				if (m_triggerTags.Length <= 0 || array.Length <= 0)
				{
					if (m_showDebug)
					{
						UnityEngine.Debug.Log($"{base.gameObject.name}: No triggers found");
					}
					m_spawnComplete = true;
					return;
				}
				for (num = 0; num < array.Length; num++)
				{
					list.AddRange(GameObject.FindGameObjectsWithTag(array[num]));
				}
				for (num = 0; num < list.Count; num++)
				{
					m_checkDistance = Vector3.Distance(base.transform.position, list[num].transform.position);
					if (m_checkDistance <= m_triggerRange)
					{
						break;
					}
				}
				if (m_checkDistance > m_triggerRange)
				{
					if (m_showDebug)
					{
						UnityEngine.Debug.Log($"{base.gameObject.name}: No triggers were close enough");
					}
					m_spawnComplete = true;
					return;
				}
			}
			if (!Application.isPlaying)
			{
				AddToSession(GaiaOperation.OperationType.Spawn, "Spawning " + base.transform.name);
			}
			if (m_spawnLocationAlgorithm == GaiaConstants.SpawnerLocation.RandomLocation || m_spawnLocationAlgorithm == GaiaConstants.SpawnerLocation.RandomLocationClustered)
			{
				StartCoroutine(RunRandomSpawnerIteration());
			}
			else
			{
				StartCoroutine(RunAreaSpawnerIteration());
			}
		}

		public IEnumerator RunRandomSpawnerIteration()
		{
			if (m_showDebug)
			{
				UnityEngine.Debug.Log($"{base.gameObject.name}: Running random iteration");
			}
			SpawnInfo spawnInfo = new SpawnInfo();
			List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
			int spawnLocationsIdx = 0;
			int failedSpawns = 0;
			m_spawnProgress = 0f;
			m_spawnComplete = false;
			float currentTime = Time.realtimeSinceStartup;
			float accumulatedTime = 0f;
			spawnInfo.m_textureStrengths = new float[Terrain.activeTerrain.terrainData.alphamapLayers];
			CreateSpawnCaches();
			LoadImageMask();
			for (int checks = 0; checks < m_locationChecksPerInt; checks++)
			{
				SpawnLocation spawnLocation = new SpawnLocation();
				if (m_spawnLocationAlgorithm == GaiaConstants.SpawnerLocation.RandomLocation)
				{
					spawnLocation.m_location = GetRandomV3(m_spawnRange);
					spawnLocation.m_location = base.transform.position + spawnLocation.m_location;
				}
				else if (spawnLocations.Count == 0 || spawnLocations.Count > m_maxRandomClusterSize || failedSpawns > m_maxRandomClusterSize)
				{
					spawnLocation.m_location = GetRandomV3(m_spawnRange);
					spawnLocation.m_location = base.transform.position + spawnLocation.m_location;
					failedSpawns = 0;
					spawnLocationsIdx = 0;
					spawnLocations.Clear();
				}
				else
				{
					if (spawnLocationsIdx >= spawnLocations.Count)
					{
						spawnLocationsIdx = 0;
					}
					spawnLocation.m_location = GetRandomV3(spawnLocations[spawnLocationsIdx].m_seedDistance);
					SpawnLocation spawnLocation2 = spawnLocation;
					List<SpawnLocation> list = spawnLocations;
					int index;
					spawnLocationsIdx = (index = spawnLocationsIdx) + 1;
					spawnLocation2.m_location = list[index].m_location + spawnLocation.m_location;
				}
				if (CheckLocation(spawnLocation.m_location, ref spawnInfo))
				{
					if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.All)
					{
						for (int ruleIdx3 = 0; ruleIdx3 < m_spawnerRules.Count; ruleIdx3++)
						{
							SpawnRule rule4 = m_spawnerRules[ruleIdx3];
							spawnInfo.m_fitness = rule4.GetFitness(ref spawnInfo);
							if (TryExecuteRule(ref rule4, ref spawnInfo))
							{
								failedSpawns = 0;
								spawnLocation.m_seedDistance = rule4.GetSeedThrowRange(ref spawnInfo);
								spawnLocations.Add(spawnLocation);
							}
							else
							{
								failedSpawns++;
							}
						}
					}
					else if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.Random)
					{
						SpawnRule rule4 = m_spawnerRules[GetRandomInt(0, m_spawnerRules.Count - 1)];
						spawnInfo.m_fitness = rule4.GetFitness(ref spawnInfo);
						if (TryExecuteRule(ref rule4, ref spawnInfo))
						{
							failedSpawns = 0;
							spawnLocation.m_seedDistance = rule4.GetSeedThrowRange(ref spawnInfo);
							spawnLocations.Add(spawnLocation);
						}
						else
						{
							failedSpawns++;
						}
					}
					else if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.Fittest)
					{
						SpawnRule fittestRule2 = null;
						float maxFitness2 = 0f;
						for (int ruleIdx3 = 0; ruleIdx3 < m_spawnerRules.Count; ruleIdx3++)
						{
							SpawnRule rule4 = m_spawnerRules[ruleIdx3];
							float fitness2 = rule4.GetFitness(ref spawnInfo);
							if (fitness2 > maxFitness2)
							{
								maxFitness2 = fitness2;
								fittestRule2 = rule4;
							}
							else if (Utils.Math_ApproximatelyEqual(fitness2, maxFitness2, 0.005f) && GetRandomFloat(0f, 1f) > 0.5f)
							{
								maxFitness2 = fitness2;
								fittestRule2 = rule4;
							}
						}
						spawnInfo.m_fitness = maxFitness2;
						if (TryExecuteRule(ref fittestRule2, ref spawnInfo))
						{
							failedSpawns = 0;
							spawnLocation.m_seedDistance = fittestRule2.GetSeedThrowRange(ref spawnInfo);
							spawnLocations.Add(spawnLocation);
						}
						else
						{
							failedSpawns++;
						}
					}
					else
					{
						SpawnRule spawnRule;
						SpawnRule selectedRule = spawnRule = null;
						SpawnRule fittestRule2 = spawnRule;
						float num;
						float selectedFitness = num = 0f;
						float maxFitness2 = num;
						for (int ruleIdx3 = 0; ruleIdx3 < m_spawnerRules.Count; ruleIdx3++)
						{
							SpawnRule rule4 = m_spawnerRules[ruleIdx3];
							float fitness2 = rule4.GetFitness(ref spawnInfo);
							if (GetRandomFloat(0f, 1f) < fitness2)
							{
								selectedRule = rule4;
								selectedFitness = fitness2;
							}
							if (fitness2 > maxFitness2)
							{
								fittestRule2 = rule4;
								maxFitness2 = fitness2;
							}
						}
						if (selectedRule == null)
						{
							selectedRule = fittestRule2;
							selectedFitness = maxFitness2;
						}
						if (selectedRule != null)
						{
							spawnInfo.m_fitness = selectedFitness;
							if (TryExecuteRule(ref selectedRule, ref spawnInfo))
							{
								failedSpawns = 0;
								spawnLocation.m_seedDistance = selectedRule.GetSeedThrowRange(ref spawnInfo);
								spawnLocations.Add(spawnLocation);
							}
							else
							{
								failedSpawns++;
							}
						}
					}
				}
				m_spawnProgress = (float)checks / (float)m_locationChecksPerInt;
				float newTime = Time.realtimeSinceStartup;
				float stepTime = newTime - currentTime;
				currentTime = newTime;
				accumulatedTime += stepTime;
				if (accumulatedTime > m_updateTimeAllowed)
				{
					accumulatedTime = 0f;
					yield return null;
				}
				if (!CanSpawnInstances() || m_cancelSpawn)
				{
					break;
				}
			}
			DeleteSpawnCaches();
			PostSpawn();
		}

		public IEnumerator RunAreaSpawnerIteration()
		{
			if (m_showDebug)
			{
				UnityEngine.Debug.Log($"{base.gameObject.name}: Running area iteration");
			}
			SpawnInfo spawnInfo = new SpawnInfo();
			Vector3 location = default(Vector3);
			m_spawnProgress = 0f;
			m_spawnComplete = false;
			float currentTime = Time.realtimeSinceStartup;
			float accumulatedTime = 0f;
			CreateSpawnCaches();
			LoadImageMask();
			spawnInfo.m_textureStrengths = new float[Terrain.activeTerrain.terrainData.alphamapLayers];
			Vector3 position = base.transform.position;
			float xWUMin = position.x - m_spawnRange + m_locationIncrement / 2f;
			float xWUMax = xWUMin + m_spawnRange * 2f;
			Vector3 position2 = base.transform.position;
			float yMid = position2.y;
			Vector3 position3 = base.transform.position;
			float zWUMin = position3.z - m_spawnRange + m_locationIncrement / 2f;
			float zWUMax = zWUMin + m_spawnRange * 2f;
			float jitMin = -1f * m_maxJitteredLocationOffsetPct * m_locationIncrement;
			float jitMax = 1f * m_maxJitteredLocationOffsetPct * m_locationIncrement;
			long currChecks = 0L;
			long totalChecks = (long)((xWUMax - xWUMin) / m_locationIncrement * ((zWUMax - zWUMin) / m_locationIncrement));
			for (float xWU = xWUMin; xWU < xWUMax; xWU += m_locationIncrement)
			{
				for (float zWU = zWUMin; zWU < zWUMax; zWU += m_locationIncrement)
				{
					currChecks++;
					location.x = xWU;
					location.y = yMid;
					location.z = zWU;
					if (m_spawnLocationAlgorithm == GaiaConstants.SpawnerLocation.EveryLocationJittered)
					{
						location.x += GetRandomFloat(jitMin, jitMax);
						location.z += GetRandomFloat(jitMin, jitMax);
					}
					if (CheckLocation(location, ref spawnInfo))
					{
						if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.All)
						{
							for (int ruleIdx4 = 0; ruleIdx4 < m_spawnerRules.Count; ruleIdx4++)
							{
								SpawnRule rule4 = m_spawnerRules[ruleIdx4];
								spawnInfo.m_fitness = rule4.GetFitness(ref spawnInfo);
								TryExecuteRule(ref rule4, ref spawnInfo);
							}
						}
						else if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.Random)
						{
							int ruleIdx4 = GetRandomInt(0, m_spawnerRules.Count - 1);
							SpawnRule rule4 = m_spawnerRules[ruleIdx4];
							spawnInfo.m_fitness = rule4.GetFitness(ref spawnInfo);
							TryExecuteRule(ref rule4, ref spawnInfo);
						}
						else if (m_spawnRuleSelector == GaiaConstants.SpawnerRuleSelector.Fittest)
						{
							SpawnRule fittestRule2 = null;
							float maxFitness2 = 0f;
							for (int ruleIdx4 = 0; ruleIdx4 < m_spawnerRules.Count; ruleIdx4++)
							{
								SpawnRule rule4 = m_spawnerRules[ruleIdx4];
								float fitness2 = rule4.GetFitness(ref spawnInfo);
								if (fitness2 > maxFitness2)
								{
									maxFitness2 = fitness2;
									fittestRule2 = rule4;
								}
								else if (Utils.Math_ApproximatelyEqual(fitness2, maxFitness2, 0.005f) && GetRandomFloat(0f, 1f) > 0.5f)
								{
									maxFitness2 = fitness2;
									fittestRule2 = rule4;
								}
							}
							spawnInfo.m_fitness = maxFitness2;
							TryExecuteRule(ref fittestRule2, ref spawnInfo);
						}
						else
						{
							SpawnRule spawnRule;
							SpawnRule selectedRule = spawnRule = null;
							SpawnRule fittestRule2 = spawnRule;
							float num;
							float selectedFitness = num = 0f;
							float maxFitness2 = num;
							for (int ruleIdx4 = 0; ruleIdx4 < m_spawnerRules.Count; ruleIdx4++)
							{
								SpawnRule rule4 = m_spawnerRules[ruleIdx4];
								float fitness2 = rule4.GetFitness(ref spawnInfo);
								if (GetRandomFloat(0f, 1f) < fitness2)
								{
									selectedRule = rule4;
									selectedFitness = fitness2;
								}
								if (fitness2 > maxFitness2)
								{
									fittestRule2 = rule4;
									maxFitness2 = fitness2;
								}
							}
							if (selectedRule == null)
							{
								selectedRule = fittestRule2;
								selectedFitness = maxFitness2;
							}
							if (selectedRule != null)
							{
								spawnInfo.m_fitness = selectedFitness;
								TryExecuteRule(ref selectedRule, ref spawnInfo);
							}
						}
						if (m_textureMapsDirty)
						{
							List<HeightMap> textureMaps = spawnInfo.m_spawner.GetTextureMaps(spawnInfo.m_hitTerrain.GetInstanceID());
							if (textureMaps != null)
							{
								for (int i = 0; i < spawnInfo.m_textureStrengths.Length; i++)
								{
									textureMaps[i][spawnInfo.m_hitLocationNU.z, spawnInfo.m_hitLocationNU.x] = spawnInfo.m_textureStrengths[i];
								}
							}
						}
					}
					m_spawnProgress = (float)currChecks / (float)totalChecks;
					float newTime = Time.realtimeSinceStartup;
					float stepTime = newTime - currentTime;
					currentTime = newTime;
					accumulatedTime += stepTime;
					if (accumulatedTime > m_updateTimeAllowed)
					{
						accumulatedTime = 0f;
						yield return null;
					}
					if (!CanSpawnInstances() || m_cancelSpawn)
					{
						break;
					}
				}
			}
			DeleteSpawnCaches(flushDirty: true);
			PostSpawn();
		}

		public void GroundToTerrain()
		{
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain == null)
			{
				terrain = Terrain.activeTerrain;
			}
			if (terrain == null)
			{
				UnityEngine.Debug.LogError("Could not fit to terrain - no terrain present");
				return;
			}
			Bounds bounds = default(Bounds);
			if (TerrainHelper.GetTerrainBounds(terrain, ref bounds))
			{
				Transform transform = base.transform;
				Vector3 position = base.transform.position;
				float x = position.x;
				Vector3 position2 = terrain.transform.position;
				float y = position2.y;
				Vector3 position3 = base.transform.position;
				transform.position = new Vector3(x, y, position3.z);
			}
		}

		public void FitToTerrain()
		{
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain == null)
			{
				terrain = Terrain.activeTerrain;
			}
			if (terrain == null)
			{
				UnityEngine.Debug.LogError("Could not fit to terrain - no terrain present");
				return;
			}
			Bounds bounds = default(Bounds);
			if (TerrainHelper.GetTerrainBounds(terrain, ref bounds))
			{
				Transform transform = base.transform;
				Vector3 center = bounds.center;
				float x = center.x;
				Vector3 position = terrain.transform.position;
				float y = position.y;
				Vector3 center2 = bounds.center;
				transform.position = new Vector3(x, y, center2.z);
				Vector3 extents = bounds.extents;
				m_spawnRange = extents.x;
			}
		}

		public bool IsFitToTerrain()
		{
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain == null)
			{
				terrain = Terrain.activeTerrain;
			}
			if (terrain == null)
			{
				UnityEngine.Debug.LogError("Could not check if fit to terrain - no terrain present");
				return false;
			}
			Bounds bounds = default(Bounds);
			if (TerrainHelper.GetTerrainBounds(terrain, ref bounds))
			{
				Vector3 center = bounds.center;
				float x = center.x;
				Vector3 position = base.transform.position;
				if (x == position.x)
				{
					Vector3 center2 = bounds.center;
					float z = center2.z;
					Vector3 position2 = base.transform.position;
					if (z == position2.z)
					{
						Vector3 extents = bounds.extents;
						if (extents.x == m_spawnRange)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		public bool LoadImageMask()
		{
			m_imageMaskHM = null;
			if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.None || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.PerlinNoise)
			{
				return false;
			}
			if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.ImageRedChannel || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.ImageGreenChannel || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.ImageBlueChannel || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.ImageAlphaChannel || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.ImageGreyScale)
			{
				if (m_imageMask == null)
				{
					UnityEngine.Debug.LogError("You requested an image mask but did not supply one. Please select mask texture.");
					return false;
				}
				Utils.MakeTextureReadable(m_imageMask);
				m_imageMaskHM = new HeightMap(m_imageMask.width, m_imageMask.height);
				for (int i = 0; i < m_imageMaskHM.Width(); i++)
				{
					for (int j = 0; j < m_imageMaskHM.Depth(); j++)
					{
						switch (m_areaMaskMode)
						{
						case GaiaConstants.ImageFitnessFilterMode.ImageGreyScale:
							m_imageMaskHM[i, j] = m_imageMask.GetPixel(i, j).grayscale;
							break;
						case GaiaConstants.ImageFitnessFilterMode.ImageRedChannel:
						{
							HeightMap imageMaskHM4 = m_imageMaskHM;
							int x4 = i;
							int z4 = j;
							Color pixel4 = m_imageMask.GetPixel(i, j);
							imageMaskHM4[x4, z4] = pixel4.r;
							break;
						}
						case GaiaConstants.ImageFitnessFilterMode.ImageGreenChannel:
						{
							HeightMap imageMaskHM3 = m_imageMaskHM;
							int x3 = i;
							int z3 = j;
							Color pixel3 = m_imageMask.GetPixel(i, j);
							imageMaskHM3[x3, z3] = pixel3.g;
							break;
						}
						case GaiaConstants.ImageFitnessFilterMode.ImageBlueChannel:
						{
							HeightMap imageMaskHM2 = m_imageMaskHM;
							int x2 = i;
							int z2 = j;
							Color pixel2 = m_imageMask.GetPixel(i, j);
							imageMaskHM2[x2, z2] = pixel2.b;
							break;
						}
						case GaiaConstants.ImageFitnessFilterMode.ImageAlphaChannel:
						{
							HeightMap imageMaskHM = m_imageMaskHM;
							int x = i;
							int z = j;
							Color pixel = m_imageMask.GetPixel(i, j);
							imageMaskHM[x, z] = pixel.a;
							break;
						}
						}
					}
				}
			}
			else
			{
				if (Terrain.activeTerrain == null)
				{
					UnityEngine.Debug.LogError("You requested an terrain texture mask but there is no active terrain.");
					return false;
				}
				Terrain activeTerrain = Terrain.activeTerrain;
				switch (m_areaMaskMode)
				{
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture0:
					if (activeTerrain.terrainData.splatPrototypes.Length < 1)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 0 but there is no active texture in slot 0.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 0);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture1:
					if (activeTerrain.terrainData.splatPrototypes.Length < 2)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 1 but there is no active texture in slot 1.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 1);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture2:
					if (activeTerrain.terrainData.splatPrototypes.Length < 3)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 2 but there is no active texture in slot 2.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 2);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture3:
					if (activeTerrain.terrainData.splatPrototypes.Length < 4)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 3 but there is no active texture in slot 3.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 3);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture4:
					if (activeTerrain.terrainData.splatPrototypes.Length < 5)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 4 but there is no active texture in slot 4.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 4);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture5:
					if (activeTerrain.terrainData.splatPrototypes.Length < 6)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 5 but there is no active texture in slot 5.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 5);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture6:
					if (activeTerrain.terrainData.splatPrototypes.Length < 7)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 6 but there is no active texture in slot 6.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 6);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture7:
					if (activeTerrain.terrainData.splatPrototypes.Length < 8)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 7 but there is no active texture in slot 7.");
						return false;
					}
					m_imageMaskHM = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, activeTerrain.terrainData.alphamapWidth, activeTerrain.terrainData.alphamapHeight), 7);
					break;
				}
				m_imageMaskHM.Flip();
			}
			if (m_imageMaskSmoothIterations > 0)
			{
				m_imageMaskHM.Smooth(m_imageMaskSmoothIterations);
			}
			if (m_imageMaskFlip)
			{
				m_imageMaskHM.Flip();
			}
			if (m_imageMaskNormalise)
			{
				m_imageMaskHM.Normalise();
			}
			if (m_imageMaskInvert)
			{
				m_imageMaskHM.Invert();
			}
			return true;
		}

		public void CreateSpawnCaches()
		{
			m_cacheTextures = false;
			m_textureMapsDirty = false;
			int i;
			for (i = 0; i < m_spawnerRules.Count; i++)
			{
				if (m_spawnerRules[i].CacheTextures(this))
				{
					CacheTextureMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
					m_cacheTextures = true;
					break;
				}
			}
			m_cacheDetails = false;
			for (i = 0; i < m_spawnerRules.Count; i++)
			{
				if (m_spawnerRules[i].CacheDetails(this))
				{
					CacheDetailMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
					m_cacheDetails = true;
					break;
				}
			}
			m_cacheTags = false;
			List<string> tagList = new List<string>();
			for (i = 0; i < m_spawnerRules.Count; i++)
			{
				m_spawnerRules[i].AddProximityTags(this, ref tagList);
			}
			if (tagList.Count > 0)
			{
				CacheTaggedGameObjectsFromScene(tagList);
				m_cacheTags = true;
			}
			m_cacheHeightMaps = false;
			i = 0;
			while (true)
			{
				if (i < m_spawnerRules.Count)
				{
					if (m_spawnerRules[i].CacheHeightMaps(this))
					{
						break;
					}
					i++;
					continue;
				}
				return;
			}
			CacheHeightMapFromTerrain(Terrain.activeTerrain.GetInstanceID());
			m_cacheHeightMaps = true;
		}

		public void CreateSpawnCaches(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			m_cacheTextures = false;
			m_textureMapsDirty = false;
			m_cacheDetails = false;
			m_cacheTags = false;
			switch (resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				if (resourceIdx < m_resources.m_texturePrototypes.Length)
				{
					CacheTextureMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
					m_cacheTextures = true;
					List<string> tagList3 = new List<string>();
					m_resources.m_texturePrototypes[resourceIdx].AddTags(ref tagList3);
					if (tagList3.Count > 0)
					{
						CacheTaggedGameObjectsFromScene(tagList3);
						m_cacheTags = true;
					}
				}
				break;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				if (resourceIdx < m_resources.m_detailPrototypes.Length)
				{
					CacheDetailMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
					m_cacheDetails = true;
					if (m_resources.m_detailPrototypes[resourceIdx].ChecksTextures())
					{
						CacheTextureMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
						m_cacheTextures = true;
					}
					List<string> tagList2 = new List<string>();
					m_resources.m_detailPrototypes[resourceIdx].AddTags(ref tagList2);
					if (tagList2.Count > 0)
					{
						CacheTaggedGameObjectsFromScene(tagList2);
						m_cacheTags = true;
					}
				}
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				if (resourceIdx < m_resources.m_treePrototypes.Length)
				{
					if (m_resources.m_treePrototypes[resourceIdx].ChecksTextures())
					{
						CacheTextureMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
						m_cacheTextures = true;
					}
					List<string> tagList4 = new List<string>();
					m_resources.m_treePrototypes[resourceIdx].AddTags(ref tagList4);
					if (tagList4.Count > 0)
					{
						CacheTaggedGameObjectsFromScene(tagList4);
						m_cacheTags = true;
					}
				}
				break;
			case GaiaConstants.SpawnerResourceType.GameObject:
				if (resourceIdx < m_resources.m_gameObjectPrototypes.Length)
				{
					if (m_resources.m_gameObjectPrototypes[resourceIdx].ChecksTextures())
					{
						CacheTextureMapsFromTerrain(Terrain.activeTerrain.GetInstanceID());
						m_cacheTextures = true;
					}
					List<string> tagList = new List<string>();
					m_resources.m_gameObjectPrototypes[resourceIdx].AddTags(ref tagList);
					if (tagList.Count > 0)
					{
						CacheTaggedGameObjectsFromScene(tagList);
						m_cacheTags = true;
					}
				}
				break;
			}
		}

		public void DeleteSpawnCaches(bool flushDirty = false)
		{
			if (m_cacheTextures)
			{
				if (flushDirty && m_textureMapsDirty && !m_cancelSpawn)
				{
					m_textureMapsDirty = false;
					SaveTextureMapsToTerrain(Terrain.activeTerrain.GetInstanceID());
				}
				DeleteTextureMapCache();
				m_cacheTextures = false;
			}
			if (m_cacheDetails)
			{
				if (!m_cancelSpawn)
				{
					SaveDetailMapsToTerrain(Terrain.activeTerrain.GetInstanceID());
				}
				DeleteDetailMapCache();
				m_cacheDetails = false;
			}
			if (m_cacheTags)
			{
				DeleteTagCache();
				m_cacheTags = false;
			}
			if (m_cacheHeightMaps)
			{
				if (flushDirty && m_heightMapDirty && !m_cancelSpawn)
				{
					m_heightMapDirty = false;
					SaveHeightMapToTerrain(Terrain.activeTerrain.GetInstanceID());
				}
				DeleteHeightMapCache();
				m_cacheHeightMaps = false;
			}
		}

		public bool TryExecuteRule(ref SpawnRule rule, ref SpawnInfo spawnInfo)
		{
			if (rule != null && (rule.m_ignoreMaxInstances || rule.m_activeInstanceCnt < rule.m_maxInstances))
			{
				spawnInfo.m_fitness *= m_spawnFitnessAttenuator.Evaluate(Mathf.Clamp01(spawnInfo.m_hitDistanceWU / m_spawnRange));
				if (m_areaMaskMode != 0)
				{
					if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.PerlinNoise || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.BillowNoise || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.RidgedNoise)
					{
						if (!m_noiseInvert)
						{
							spawnInfo.m_fitness *= m_noiseGenerator.GetNormalisedValue(100000f + spawnInfo.m_hitLocationWU.x * (1f / m_noiseZoom), 100000f + spawnInfo.m_hitLocationWU.z * (1f / m_noiseZoom));
						}
						else
						{
							spawnInfo.m_fitness *= 1f - m_noiseGenerator.GetNormalisedValue(100000f + spawnInfo.m_hitLocationWU.x * (1f / m_noiseZoom), 100000f + spawnInfo.m_hitLocationWU.z * (1f / m_noiseZoom));
						}
					}
					else if (m_imageMaskHM.HasData())
					{
						float x = spawnInfo.m_hitLocationWU.x;
						Vector3 position = base.transform.position;
						float x2 = (x - (position.x - m_spawnRange)) / (m_spawnRange * 2f);
						float z = spawnInfo.m_hitLocationWU.z;
						Vector3 position2 = base.transform.position;
						float z2 = (z - (position2.z - m_spawnRange)) / (m_spawnRange * 2f);
						spawnInfo.m_fitness *= m_imageMaskHM[x2, z2];
					}
				}
				if (spawnInfo.m_fitness > rule.m_minViableFitness && GetRandomFloat(0f, 1f) > rule.m_failureRate)
				{
					rule.Spawn(ref spawnInfo);
					return true;
				}
			}
			return false;
		}

		public bool CheckLocation(Vector3 locationWU, ref SpawnInfo spawnInfo)
		{
			spawnInfo.m_spawner = this;
			spawnInfo.m_outOfBounds = true;
			spawnInfo.m_wasVirginTerrain = false;
			spawnInfo.m_spawnRotationY = 0f;
			spawnInfo.m_hitDistanceWU = Vector3.Distance(base.transform.position, locationWU);
			spawnInfo.m_hitLocationWU = locationWU;
			spawnInfo.m_hitNormal = Vector3.zero;
			spawnInfo.m_hitObject = null;
			spawnInfo.m_hitTerrain = null;
			spawnInfo.m_terrainNormalWU = Vector3.one;
			spawnInfo.m_terrainHeightWU = 0f;
			spawnInfo.m_terrainSlopeWU = 0f;
			spawnInfo.m_areaHitSlopeWU = 0f;
			spawnInfo.m_areaMinSlopeWU = 0f;
			spawnInfo.m_areaAvgSlopeWU = 0f;
			spawnInfo.m_areaMaxSlopeWU = 0f;
			locationWU.y = m_terrainHeight + 1000f;
			if (Physics.Raycast(locationWU, Vector3.down, out m_checkHitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				if (spawnInfo.m_spawner.IsDetailSpawner() && (m_checkHitInfo.collider is SphereCollider || m_checkHitInfo.collider is CapsuleCollider) && m_checkHitInfo.collider.name == "_GaiaCollider_Grass")
				{
					Vector3 point = m_checkHitInfo.point;
					locationWU.y = point.y - 0.01f;
					if (!Physics.Raycast(locationWU, Vector3.down, out m_checkHitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
					{
						return false;
					}
				}
				spawnInfo.m_hitLocationWU = m_checkHitInfo.point;
				spawnInfo.m_hitDistanceWU = Vector3.Distance(base.transform.position, spawnInfo.m_hitLocationWU);
				spawnInfo.m_hitNormal = m_checkHitInfo.normal;
				spawnInfo.m_hitObject = m_checkHitInfo.transform;
				if (m_spawnerShape == GaiaConstants.SpawnerShape.Box)
				{
					if (!m_spawnerBounds.Contains(spawnInfo.m_hitLocationWU))
					{
						return false;
					}
				}
				else if (spawnInfo.m_hitDistanceWU > m_spawnRange)
				{
					return false;
				}
				spawnInfo.m_outOfBounds = false;
				Terrain terrain;
				if (m_checkHitInfo.collider is TerrainCollider)
				{
					terrain = m_checkHitInfo.transform.GetComponent<Terrain>();
					spawnInfo.m_wasVirginTerrain = true;
				}
				else
				{
					terrain = TerrainHelper.GetTerrain(m_checkHitInfo.point);
				}
				if (terrain != null)
				{
					spawnInfo.m_hitTerrain = terrain;
					spawnInfo.m_terrainHeightWU = terrain.SampleHeight(m_checkHitInfo.point);
					Vector3 vector = terrain.transform.InverseTransformPoint(m_checkHitInfo.point);
					Vector3 size = terrain.terrainData.size;
					float x = Mathf.InverseLerp(0f, size.x, vector.x);
					Vector3 size2 = terrain.terrainData.size;
					float y = Mathf.InverseLerp(0f, size2.y, vector.y);
					Vector3 size3 = terrain.terrainData.size;
					Vector3 hitLocationNU = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, vector.z));
					spawnInfo.m_hitLocationNU = hitLocationNU;
					spawnInfo.m_terrainSlopeWU = terrain.terrainData.GetSteepness(hitLocationNU.x, hitLocationNU.z);
					spawnInfo.m_areaHitSlopeWU = (spawnInfo.m_areaMinSlopeWU = (spawnInfo.m_areaAvgSlopeWU = (spawnInfo.m_areaMaxSlopeWU = spawnInfo.m_terrainSlopeWU)));
					spawnInfo.m_terrainNormalWU = terrain.terrainData.GetInterpolatedNormal(hitLocationNU.x, hitLocationNU.z);
					if (spawnInfo.m_wasVirginTerrain && !Utils.Math_ApproximatelyEqual(spawnInfo.m_hitLocationWU.y, spawnInfo.m_terrainHeightWU, GaiaConstants.VirginTerrainCheckThreshold))
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
					if (m_textureMapCache != null && m_textureMapCache.Count > 0)
					{
						List<HeightMap> list = m_textureMapCache[terrain.GetInstanceID()];
						for (int i = 0; i < spawnInfo.m_textureStrengths.Length; i++)
						{
							spawnInfo.m_textureStrengths[i] = list[i][hitLocationNU.z, hitLocationNU.x];
						}
					}
					else
					{
						float[,,] alphamaps = terrain.terrainData.GetAlphamaps((int)(hitLocationNU.x * (float)(terrain.terrainData.alphamapWidth - 1)), (int)(hitLocationNU.z * (float)(terrain.terrainData.alphamapHeight - 1)), 1, 1);
						for (int j = 0; j < spawnInfo.m_textureStrengths.Length; j++)
						{
							spawnInfo.m_textureStrengths[j] = alphamaps[0, 0, j];
						}
					}
				}
				return true;
			}
			return false;
		}

		public bool CheckLocationBounds(ref SpawnInfo spawnInfo, float distance)
		{
			spawnInfo.m_areaHitSlopeWU = (spawnInfo.m_areaMinSlopeWU = (spawnInfo.m_areaAvgSlopeWU = (spawnInfo.m_areaMaxSlopeWU = spawnInfo.m_terrainSlopeWU)));
			if (spawnInfo.m_areaHitsWU == null)
			{
				spawnInfo.m_areaHitsWU = new Vector3[4];
			}
			spawnInfo.m_areaHitsWU[0] = new Vector3(spawnInfo.m_hitLocationWU.x + distance, spawnInfo.m_hitLocationWU.y + 3000f, spawnInfo.m_hitLocationWU.z);
			spawnInfo.m_areaHitsWU[1] = new Vector3(spawnInfo.m_hitLocationWU.x - distance, spawnInfo.m_hitLocationWU.y + 3000f, spawnInfo.m_hitLocationWU.z);
			spawnInfo.m_areaHitsWU[2] = new Vector3(spawnInfo.m_hitLocationWU.x, spawnInfo.m_hitLocationWU.y + 3000f, spawnInfo.m_hitLocationWU.z + distance);
			spawnInfo.m_areaHitsWU[3] = new Vector3(spawnInfo.m_hitLocationWU.x, spawnInfo.m_hitLocationWU.y + 3000f, spawnInfo.m_hitLocationWU.z - distance);
			Vector3 zero = Vector3.zero;
			Vector3 vector = Vector3.zero;
			float num = 0f;
			float num2 = 0f;
			if (!Physics.SphereCast(new Vector3(spawnInfo.m_hitLocationWU.x, spawnInfo.m_hitLocationWU.y + 3000f, spawnInfo.m_hitLocationWU.z), distance, Vector3.down, out RaycastHit hitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				return false;
			}
			Terrain component;
			if (spawnInfo.m_wasVirginTerrain)
			{
				if (hitInfo.collider is TerrainCollider)
				{
					component = hitInfo.transform.GetComponent<Terrain>();
					num = component.SampleHeight(hitInfo.point);
					Vector3 point = hitInfo.point;
					if (!Utils.Math_ApproximatelyEqual(point.y, num, GaiaConstants.VirginTerrainCheckThreshold))
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
				}
				else
				{
					spawnInfo.m_wasVirginTerrain = false;
				}
			}
			if (!Physics.Raycast(spawnInfo.m_areaHitsWU[0], Vector3.down, out hitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				return false;
			}
			spawnInfo.m_areaHitsWU[0] = hitInfo.point;
			component = hitInfo.transform.GetComponent<Terrain>();
			if (component == null)
			{
				component = TerrainHelper.GetTerrain(hitInfo.point);
			}
			if (component != null)
			{
				num = component.SampleHeight(hitInfo.point);
				zero = component.transform.InverseTransformPoint(hitInfo.point);
				Vector3 size = component.terrainData.size;
				float x = Mathf.InverseLerp(0f, size.x, zero.x);
				Vector3 size2 = component.terrainData.size;
				float y = Mathf.InverseLerp(0f, size2.y, zero.y);
				Vector3 size3 = component.terrainData.size;
				vector = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, zero.z));
				num2 = component.terrainData.GetSteepness(vector.x, vector.z);
				spawnInfo.m_areaAvgSlopeWU += num2;
				if (num2 > spawnInfo.m_areaMaxSlopeWU)
				{
					spawnInfo.m_areaMaxSlopeWU = num2;
				}
				if (num2 < spawnInfo.m_areaMinSlopeWU)
				{
					spawnInfo.m_areaMinSlopeWU = num2;
				}
				if (spawnInfo.m_wasVirginTerrain)
				{
					if (hitInfo.collider is TerrainCollider)
					{
						Vector3 point2 = hitInfo.point;
						if (!Utils.Math_ApproximatelyEqual(point2.y, num, GaiaConstants.VirginTerrainCheckThreshold))
						{
							spawnInfo.m_wasVirginTerrain = false;
						}
					}
					else
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
				}
			}
			if (!Physics.Raycast(spawnInfo.m_areaHitsWU[1], Vector3.down, out hitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				return false;
			}
			spawnInfo.m_areaHitsWU[1] = hitInfo.point;
			component = hitInfo.transform.GetComponent<Terrain>();
			if (component == null)
			{
				component = TerrainHelper.GetTerrain(hitInfo.point);
			}
			if (component != null)
			{
				num = component.SampleHeight(hitInfo.point);
				zero = component.transform.InverseTransformPoint(hitInfo.point);
				Vector3 size4 = component.terrainData.size;
				float x2 = Mathf.InverseLerp(0f, size4.x, zero.x);
				Vector3 size5 = component.terrainData.size;
				float y2 = Mathf.InverseLerp(0f, size5.y, zero.y);
				Vector3 size6 = component.terrainData.size;
				vector = new Vector3(x2, y2, Mathf.InverseLerp(0f, size6.z, zero.z));
				num2 = component.terrainData.GetSteepness(vector.x, vector.z);
				spawnInfo.m_areaAvgSlopeWU += num2;
				if (num2 > spawnInfo.m_areaMaxSlopeWU)
				{
					spawnInfo.m_areaMaxSlopeWU = num2;
				}
				if (num2 < spawnInfo.m_areaMinSlopeWU)
				{
					spawnInfo.m_areaMinSlopeWU = num2;
				}
				if (spawnInfo.m_wasVirginTerrain)
				{
					if (hitInfo.collider is TerrainCollider)
					{
						Vector3 point3 = hitInfo.point;
						if (!Utils.Math_ApproximatelyEqual(point3.y, num, GaiaConstants.VirginTerrainCheckThreshold))
						{
							spawnInfo.m_wasVirginTerrain = false;
						}
					}
					else
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
				}
			}
			if (!Physics.Raycast(spawnInfo.m_areaHitsWU[2], Vector3.down, out hitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				return false;
			}
			spawnInfo.m_areaHitsWU[2] = hitInfo.point;
			component = hitInfo.transform.GetComponent<Terrain>();
			if (component == null)
			{
				component = TerrainHelper.GetTerrain(hitInfo.point);
			}
			if (component != null)
			{
				num = component.SampleHeight(hitInfo.point);
				zero = component.transform.InverseTransformPoint(hitInfo.point);
				Vector3 size7 = component.terrainData.size;
				float x3 = Mathf.InverseLerp(0f, size7.x, zero.x);
				Vector3 size8 = component.terrainData.size;
				float y3 = Mathf.InverseLerp(0f, size8.y, zero.y);
				Vector3 size9 = component.terrainData.size;
				vector = new Vector3(x3, y3, Mathf.InverseLerp(0f, size9.z, zero.z));
				num2 = component.terrainData.GetSteepness(vector.x, vector.z);
				spawnInfo.m_areaAvgSlopeWU += num2;
				if (num2 > spawnInfo.m_areaMaxSlopeWU)
				{
					spawnInfo.m_areaMaxSlopeWU = num2;
				}
				if (num2 < spawnInfo.m_areaMinSlopeWU)
				{
					spawnInfo.m_areaMinSlopeWU = num2;
				}
				if (spawnInfo.m_wasVirginTerrain)
				{
					if (hitInfo.collider is TerrainCollider)
					{
						Vector3 point4 = hitInfo.point;
						if (!Utils.Math_ApproximatelyEqual(point4.y, num, GaiaConstants.VirginTerrainCheckThreshold))
						{
							spawnInfo.m_wasVirginTerrain = false;
						}
					}
					else
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
				}
			}
			if (!Physics.Raycast(spawnInfo.m_areaHitsWU[3], Vector3.down, out hitInfo, float.PositiveInfinity, m_spawnCollisionLayers))
			{
				return false;
			}
			spawnInfo.m_areaHitsWU[3] = hitInfo.point;
			component = hitInfo.transform.GetComponent<Terrain>();
			if (component == null)
			{
				component = TerrainHelper.GetTerrain(hitInfo.point);
			}
			if (component != null)
			{
				num = component.SampleHeight(hitInfo.point);
				zero = component.transform.InverseTransformPoint(hitInfo.point);
				Vector3 size10 = component.terrainData.size;
				float x4 = Mathf.InverseLerp(0f, size10.x, zero.x);
				Vector3 size11 = component.terrainData.size;
				float y4 = Mathf.InverseLerp(0f, size11.y, zero.y);
				Vector3 size12 = component.terrainData.size;
				vector = new Vector3(x4, y4, Mathf.InverseLerp(0f, size12.z, zero.z));
				num2 = component.terrainData.GetSteepness(vector.x, vector.z);
				spawnInfo.m_areaAvgSlopeWU += num2;
				if (num2 > spawnInfo.m_areaMaxSlopeWU)
				{
					spawnInfo.m_areaMaxSlopeWU = num2;
				}
				if (num2 < spawnInfo.m_areaMinSlopeWU)
				{
					spawnInfo.m_areaMinSlopeWU = num2;
				}
				if (spawnInfo.m_wasVirginTerrain)
				{
					if (hitInfo.collider is TerrainCollider)
					{
						Vector3 point5 = hitInfo.point;
						if (!Utils.Math_ApproximatelyEqual(point5.y, num, GaiaConstants.VirginTerrainCheckThreshold))
						{
							spawnInfo.m_wasVirginTerrain = false;
						}
					}
					else
					{
						spawnInfo.m_wasVirginTerrain = false;
					}
				}
			}
			spawnInfo.m_areaAvgSlopeWU /= 5f;
			float num3 = spawnInfo.m_areaHitsWU[0].y - spawnInfo.m_areaHitsWU[1].y;
			float num4 = spawnInfo.m_areaHitsWU[2].y - spawnInfo.m_areaHitsWU[3].y;
			spawnInfo.m_areaHitSlopeWU = Utils.Math_Clamp(0f, 90f, (float)Math.Sqrt(num3 * num3 + num4 * num4));
			return true;
		}

		public void UpdateCounters()
		{
			m_totalRuleCnt = 0;
			m_activeRuleCnt = 0;
			m_inactiveRuleCnt = 0;
			m_maxInstanceCnt = 0uL;
			m_activeInstanceCnt = 0uL;
			m_inactiveInstanceCnt = 0uL;
			m_totalInstanceCnt = 0uL;
			foreach (SpawnRule spawnerRule in m_spawnerRules)
			{
				m_totalRuleCnt++;
				if (spawnerRule.m_isActive)
				{
					m_activeRuleCnt++;
					m_maxInstanceCnt += spawnerRule.m_maxInstances;
					m_activeInstanceCnt += spawnerRule.m_activeInstanceCnt;
					m_inactiveInstanceCnt += spawnerRule.m_inactiveInstanceCnt;
					m_totalInstanceCnt += spawnerRule.m_activeInstanceCnt + spawnerRule.m_inactiveInstanceCnt;
				}
				else
				{
					m_inactiveRuleCnt++;
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (m_showGizmos)
			{
				if (m_spawnerShape == GaiaConstants.SpawnerShape.Sphere)
				{
					if (m_mode == GaiaConstants.OperationMode.RuntimeTriggeredInterval)
					{
						Gizmos.color = Color.yellow;
						Gizmos.DrawWireSphere(base.transform.position, m_triggerRange);
					}
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere(base.transform.position, m_spawnRange);
				}
				else
				{
					if (m_mode == GaiaConstants.OperationMode.RuntimeTriggeredInterval)
					{
						Gizmos.color = Color.yellow;
						Gizmos.DrawWireCube(base.transform.position, new Vector3(m_triggerRange * 2f, m_triggerRange * 2f, m_triggerRange * 2f));
					}
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(base.transform.position, new Vector3(m_spawnRange * 2f, m_spawnRange * 2f, m_spawnRange * 2f));
				}
				if (m_resources != null)
				{
					Bounds bounds = default(Bounds);
					if (TerrainHelper.GetTerrainBounds(base.transform.position, ref bounds))
					{
						Vector3 center = bounds.center;
						float x = center.x;
						float seaLevel = m_resources.m_seaLevel;
						Vector3 center2 = bounds.center;
						bounds.center = new Vector3(x, seaLevel, center2.z);
						Vector3 size = bounds.size;
						float x2 = size.x;
						Vector3 size2 = bounds.size;
						bounds.size = new Vector3(x2, 0.05f, size2.z);
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
			UpdateCounters();
		}

		public void CacheTextureMapsFromTerrain(int terrainID)
		{
			if (m_textureMapCache == null)
			{
				m_textureMapCache = new Dictionary<int, List<HeightMap>>();
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() == terrainID)
				{
					float[,,] alphamaps = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
					List<HeightMap> list = new List<HeightMap>();
					for (int j = 0; j < terrain.terrainData.alphamapLayers; j++)
					{
						HeightMap item = new HeightMap(alphamaps, j);
						list.Add(item);
					}
					m_textureMapCache[terrainID] = list;
					return;
				}
			}
			UnityEngine.Debug.LogError("Attempted to get textures on terrain that does not exist!");
		}

		public List<HeightMap> GetTextureMaps(int terrainID)
		{
			if (!m_textureMapCache.TryGetValue(terrainID, out List<HeightMap> value))
			{
				return null;
			}
			return value;
		}

		public void SaveTextureMapsToTerrain(int terrainID)
		{
			if (!m_textureMapCache.TryGetValue(terrainID, out List<HeightMap> value))
			{
				UnityEngine.Debug.LogError("Texture map list was not found for terrain ID : " + terrainID + " !");
				return;
			}
			if (value.Count <= 0)
			{
				UnityEngine.Debug.LogError("Texture map list was empty for terrain ID : " + terrainID + " !");
				return;
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() != terrainID)
				{
					continue;
				}
				if (value.Count != terrain.terrainData.alphamapLayers)
				{
					UnityEngine.Debug.LogError("Texture map prototype list does not match terrain prototype list for terrain ID : " + terrainID + " !");
					return;
				}
				float[,,] array = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, terrain.terrainData.alphamapLayers];
				for (int j = 0; j < terrain.terrainData.alphamapLayers; j++)
				{
					HeightMap heightMap = value[j];
					for (int k = 0; k < heightMap.Width(); k++)
					{
						for (int l = 0; l < heightMap.Depth(); l++)
						{
							array[k, l, j] = heightMap[k, l];
						}
					}
				}
				terrain.terrainData.SetAlphamaps(0, 0, array);
				return;
			}
			UnityEngine.Debug.LogError("Attempted to locate a terrain that does not exist!");
		}

		public void DeleteTextureMapCache()
		{
			m_textureMapCache = new Dictionary<int, List<HeightMap>>();
		}

		public void SetTextureMapsDirty()
		{
			m_textureMapsDirty = true;
		}

		public void CacheDetailMapsFromTerrain(int terrainID)
		{
			if (m_detailMapCache == null)
			{
				m_detailMapCache = new Dictionary<int, List<HeightMap>>();
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() == terrainID)
				{
					List<HeightMap> list = new List<HeightMap>();
					for (int j = 0; j < terrain.terrainData.detailPrototypes.Length; j++)
					{
						HeightMap item = new HeightMap(terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, j));
						list.Add(item);
					}
					m_detailMapCache[terrainID] = list;
					return;
				}
			}
			UnityEngine.Debug.LogError("Attempted to get details on terrain that does not exist!");
		}

		public void SaveDetailMapsToTerrain(int terrainID)
		{
			if (!m_detailMapCache.TryGetValue(terrainID, out List<HeightMap> value))
			{
				UnityEngine.Debug.LogWarning(base.gameObject.name + "Detail map list was not found for terrain ID : " + terrainID + " !");
				return;
			}
			if (value.Count <= 0)
			{
				UnityEngine.Debug.LogWarning(base.gameObject.name + ": Detail map list was empty for terrain ID : " + terrainID + " !");
				return;
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() != terrainID)
				{
					continue;
				}
				if (value.Count != terrain.terrainData.detailPrototypes.Length)
				{
					UnityEngine.Debug.LogError("Detail map protoype list does not match terrain prototype list for terrain ID : " + terrainID + " !");
					return;
				}
				int[,] array = new int[value[0].Width(), value[0].Depth()];
				for (int j = 0; j < terrain.terrainData.detailPrototypes.Length; j++)
				{
					HeightMap heightMap = value[j];
					for (int k = 0; k < heightMap.Width(); k++)
					{
						for (int l = 0; l < heightMap.Depth(); l++)
						{
							array[k, l] = (int)heightMap[k, l];
						}
					}
					terrain.terrainData.SetDetailLayer(0, 0, j, array);
				}
				terrain.Flush();
				return;
			}
			UnityEngine.Debug.LogError("Attempted to locate a terrain that does not exist!");
		}

		public List<HeightMap> GetDetailMaps(int terrainID)
		{
			if (!m_detailMapCache.TryGetValue(terrainID, out List<HeightMap> value))
			{
				return null;
			}
			return value;
		}

		public HeightMap GetDetailMap(int terrainID, int detailIndex)
		{
			if (!m_detailMapCache.TryGetValue(terrainID, out List<HeightMap> value))
			{
				return null;
			}
			if (detailIndex >= 0 && detailIndex < value.Count)
			{
				return value[detailIndex];
			}
			return null;
		}

		public void DeleteDetailMapCache()
		{
			m_detailMapCache = new Dictionary<int, List<HeightMap>>();
		}

		public void AddToSession(GaiaOperation.OperationType opType, string opName)
		{
			GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
			if (sessionManager != null && !sessionManager.IsLocked())
			{
				GaiaOperation gaiaOperation = new GaiaOperation();
				gaiaOperation.m_description = opName;
				gaiaOperation.m_generatedByID = m_spawnerID;
				gaiaOperation.m_generatedByName = base.transform.name;
				gaiaOperation.m_generatedByType = GetType().ToString();
				gaiaOperation.m_isActive = true;
				gaiaOperation.m_operationDateTime = DateTime.Now.ToString();
				gaiaOperation.m_operationType = opType;
				gaiaOperation.m_operationDataJson = new string[1];
				gaiaOperation.m_operationDataJson[0] = SerialiseJson();
				sessionManager.AddOperation(gaiaOperation);
				sessionManager.AddResource(m_resources);
			}
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
			Spawner instance = this;
			fsSerializer.TryDeserialize(data, ref instance);
			instance.m_resources = (Utils.GetAsset(m_resourcesPath, typeof(GaiaResource)) as GaiaResource);
		}

		public void FlattenTerrain()
		{
			AddToSession(GaiaOperation.OperationType.FlattenTerrain, "Flattening terrain");
			GaiaWorldManager gaiaWorldManager = new GaiaWorldManager(Terrain.activeTerrains);
			gaiaWorldManager.FlattenWorld();
		}

		public void SmoothTerrain()
		{
			AddToSession(GaiaOperation.OperationType.SmoothTerrain, "Smoothing terrain");
			GaiaWorldManager gaiaWorldManager = new GaiaWorldManager(Terrain.activeTerrains);
			gaiaWorldManager.SmoothWorld();
		}

		public void ClearTrees()
		{
			AddToSession(GaiaOperation.OperationType.ClearTrees, "Clearing terrain trees");
			TerrainHelper.ClearTrees();
		}

		public void ClearDetails()
		{
			AddToSession(GaiaOperation.OperationType.ClearDetails, "Clearing terrain details");
			TerrainHelper.ClearDetails();
		}

		public void CacheHeightMapFromTerrain(int terrainID)
		{
			if (m_heightMapCache == null)
			{
				m_heightMapCache = new Dictionary<int, UnityHeightMap>();
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() == terrainID)
				{
					m_heightMapCache[terrainID] = new UnityHeightMap(terrain);
					return;
				}
			}
			UnityEngine.Debug.LogError("Attempted to get height maps on a terrain that does not exist!");
		}

		public UnityHeightMap GetHeightMap(int terrainID)
		{
			if (!m_heightMapCache.TryGetValue(terrainID, out UnityHeightMap value))
			{
				return null;
			}
			return value;
		}

		public void SaveHeightMapToTerrain(int terrainID)
		{
			if (!m_heightMapCache.TryGetValue(terrainID, out UnityHeightMap value))
			{
				UnityEngine.Debug.LogError("Heightmap was not found for terrain ID : " + terrainID + " !");
				return;
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				if (terrain.GetInstanceID() == terrainID)
				{
					value.SaveToTerrain(terrain);
					return;
				}
			}
			UnityEngine.Debug.LogError("Attempted to locate a terrain that does not exist!");
		}

		public void DeleteHeightMapCache()
		{
			m_heightMapCache = new Dictionary<int, UnityHeightMap>();
		}

		public void SetHeightMapsDirty()
		{
			m_heightMapDirty = true;
		}

		public void CacheStamps(List<string> stampList)
		{
			if (m_stampCache == null)
			{
				m_stampCache = new Dictionary<string, HeightMap>();
			}
			for (int i = 0; i < stampList.Count; i++)
			{
			}
		}

		private void CacheTaggedGameObjectsFromScene(List<string> tagList)
		{
			m_taggedGameObjectCache = new Dictionary<string, Quadtree<GameObject>>();
			Vector3 position = Terrain.activeTerrain.transform.position;
			float x = position.x;
			Vector3 position2 = Terrain.activeTerrain.transform.position;
			float z = position2.z;
			Vector3 size = Terrain.activeTerrain.terrainData.size;
			float x2 = size.x;
			Vector3 size2 = Terrain.activeTerrain.terrainData.size;
			Rect boundaries = new Rect(x, z, x2, size2.z);
			Vector2 vector = default(Vector2);
			for (int i = 0; i < tagList.Count; i++)
			{
				string text = tagList[i].Trim();
				bool flag = false;
				if (!string.IsNullOrEmpty(text))
				{
					flag = true;
				}
				if (!flag)
				{
					continue;
				}
				Quadtree<GameObject> value = null;
				if (!m_taggedGameObjectCache.TryGetValue(text, out value))
				{
					value = new Quadtree<GameObject>(boundaries);
					m_taggedGameObjectCache.Add(text, value);
				}
				GameObject[] array = GameObject.FindGameObjectsWithTag(text);
				foreach (GameObject gameObject in array)
				{
					Vector3 position3 = gameObject.transform.position;
					float x3 = position3.x;
					Vector3 position4 = gameObject.transform.position;
					vector = new Vector2(x3, position4.z);
					if (boundaries.Contains(vector))
					{
						value.Insert(vector, gameObject);
					}
				}
			}
		}

		private void DeleteTagCache()
		{
			m_taggedGameObjectCache = null;
		}

		public List<GameObject> GetNearbyObjects(List<string> tagList, Rect area)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < tagList.Count; i++)
			{
				Quadtree<GameObject> value = null;
				string key = tagList[i];
				if (m_taggedGameObjectCache.TryGetValue(key, out value))
				{
					IEnumerable<GameObject> enumerable = value.Find(area);
					foreach (GameObject item in enumerable)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		public GameObject GetClosestObject(List<string> tagList, Rect area)
		{
			float num = float.MaxValue;
			GameObject result = null;
			for (int i = 0; i < tagList.Count; i++)
			{
				Quadtree<GameObject> value = null;
				string key = tagList[i];
				if (m_taggedGameObjectCache.TryGetValue(key, out value))
				{
					IEnumerable<GameObject> enumerable = value.Find(area);
					foreach (GameObject item in enumerable)
					{
						Vector2 center = area.center;
						Vector3 position = item.transform.position;
						float x = position.x;
						Vector3 position2 = item.transform.position;
						float num2 = Vector2.Distance(center, new Vector2(x, position2.z));
						if (num2 < num)
						{
							num = num2;
							result = item;
						}
					}
				}
			}
			return result;
		}

		public GameObject GetClosestObject(string tag, Rect area)
		{
			float num = float.MaxValue;
			GameObject result = null;
			Quadtree<GameObject> value = null;
			if (m_taggedGameObjectCache.TryGetValue(tag, out value))
			{
				IEnumerable<GameObject> enumerable = value.Find(area);
				{
					foreach (GameObject item in enumerable)
					{
						Vector2 center = area.center;
						Vector3 position = item.transform.position;
						float x = position.x;
						Vector3 position2 = item.transform.position;
						float num2 = Vector2.Distance(center, new Vector2(x, position2.z));
						if (num2 < num)
						{
							num = num2;
							result = item;
						}
					}
					return result;
				}
			}
			return result;
		}

		public void ResetRandomGenertor()
		{
			m_rndGenerator = new XorshiftPlus(m_seed);
		}

		public int GetRandomInt(int min, int max)
		{
			return m_rndGenerator.Next(min, max);
		}

		public float GetRandomFloat(float min, float max)
		{
			return m_rndGenerator.Next(min, max);
		}

		public Vector3 GetRandomV3(float range)
		{
			return m_rndGenerator.NextVector(0f - range, range);
		}
	}
}
