using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeNa
{
	public class Spawner : MonoBehaviour
	{
		public string m_parentName = "GeNa Spawner";

		public bool m_mergeSpawns = true;

		public List<Prototype> m_spawnPrototypes = new List<Prototype>();

		public Vector3 m_spawnOriginLocation = Vector3.zero;

		public Vector3 m_spawnOriginNormal = Vector3.up;

		public int m_spawnOriginObjectID = int.MinValue;

		public bool m_spawnOriginIsTerrain;

		public Bounds m_spawnOriginBounds = default(Bounds);

		public Transform m_spawnOriginGroundTransform;

		public Constants.LocationAlgorithm m_spawnAlgorithm = Constants.LocationAlgorithm.Organic;

		public long m_minInstances = 1L;

		public long m_maxInstances = 1L;

		public long m_instancesSpawned;

		public float m_seedThrowRange = 5f;

		public float m_seedThrowJitter = 1f;

		public float m_maxSpawnRange = 50f;

		public Constants.SpawnRangeShape m_spawnRangeShape;

		public Constants.VirginCheckType m_critVirginCheckType = Constants.VirginCheckType.Point;

		public float m_critBoundsBorder;

		public bool m_critCheckHeight = true;

		public float m_critMinSpawnHeight = 50f;

		public float m_critHeightVariance = 30f;

		public bool m_critCheckSlope = true;

		public float m_critSlopeVariance = 30f;

		public bool m_critCheckTextures;

		public float m_critTextureStrength;

		public float m_critTextureVariance = 0.1f;

		public int m_critSelectedTextureIdx;

		public string m_critSelectedTextureName = string.Empty;

		public int m_critMaxSelectedTexture = 1;

		public bool m_critCheckMask;

		public Constants.MaskType m_critMaskType;

		public Fractal m_critMaskFractal = new Fractal();

		public float m_critMaskFractalMidpoint = 0.5f;

		public float m_critMaskFractalRange = 0.5f;

		public Texture2D m_critMaskImage;

		public Heightmap m_critMaskImageData;

		public Heightmap m_critMaskAlphaData;

		public bool m_critMaskInvert;

		private Vector3 m_critSpawnCentre = Vector3.zero;

		private float m_critMinHeight;

		private float m_critMaxHeight = 1000f;

		private float m_critMinSlope;

		private float m_critMaxSlope = 90f;

		private float m_critMinTextureStrength;

		private float m_critMaxTextureStrength = 1f;

		private float m_critMaskFractalMin;

		private float m_critMaskFractalMax = 1f;

		public Constants.RotationAlgorithm m_rotationAlgorithm;

		public float m_rotationYOffsetXX;

		public float m_minRotationY;

		public float m_maxRotationY = 360f;

		public bool m_sameScale = true;

		public bool m_scaleToNearestInt = true;

		public float m_minScaleX = 0.7f;

		public float m_maxScaleX = 1.3f;

		public float m_minScaleY = 1f;

		public float m_maxScaleY = 1f;

		public float m_minScaleZ = 1f;

		public float m_maxScaleZ = 1f;

		public bool m_useGravity;

		public bool m_enableRotationDragUpdate;

		public bool m_autoOptimise = true;

		public float m_maxSizeToOptimize = 10f;

		public float m_minProbeGroupDistance = 100f;

		public float m_minProbeDistance = 15f;

		public bool m_autoProbe = true;

		public Gravity m_gravity;

		public bool m_advUseLargeRanges;

		public bool m_advShowMouseOverHelp = true;

		public bool m_advShowDetailedHelp = true;

		public bool m_advForcePlaceAtClickLocation = true;

		public bool m_advAddColliderToSpawnedPrefabs;

		public bool m_showSpawnCriteria;

		public bool m_showPlacementCriteria;

		public bool m_showPrototypes;

		public bool m_showGizmos;

		public bool m_showAdvancedSettings;

		public bool m_needsVisualisationUpdate = true;

		public int m_maxVisualisationDimensions = 50;

		private float[,] m_fitnessArray = new float[1, 1];

		public float m_metersPerScan = 1f;

		public float m_metersPerScanVisualisation = 4f;

		private TreeManager m_treeManager = new TreeManager();

		private ProbeManager m_probeManager = new ProbeManager();

		private GameObject m_probeParent;

		public GameObject m_lastSpawnedObject;

		public List<GameObject> m_prefabUndoList = new List<GameObject>();

		private List<Spawner> m_childSpawners = new List<Spawner>();

		private DateTime m_lastUpdated = DateTime.MinValue;

		public GenaDefaults m_defaults;

		public int m_randomSeed = 1000;

		public XorshiftPlus m_randomGenerator = new XorshiftPlus(1000);

		public void SetDefaults()
		{
			if (!(m_defaults != null))
			{
				if (m_defaults == null)
				{
					m_defaults = ScriptableObject.CreateInstance<GenaDefaults>();
				}
				m_advShowDetailedHelp = m_defaults.m_showDetailedHelp;
				m_advShowMouseOverHelp = m_defaults.m_showTooltips;
				m_autoProbe = m_defaults.m_autoLightProbe;
				m_minProbeGroupDistance = m_defaults.m_minProbeGroupDistance;
				m_minProbeDistance = m_defaults.m_minProbeDistance;
				m_autoOptimise = m_defaults.m_autoOptimize;
				m_maxSizeToOptimize = m_defaults.m_maxOptimizeSize;
				m_randomSeed = UnityEngine.Random.Range(100, 999999);
				m_randomGenerator = new XorshiftPlus(m_randomSeed);
			}
		}

		public void SetSpawnOriginAndUpdateRanges(Transform groundObject, Vector3 location, Vector3 normal)
		{
			m_spawnOriginLocation = location;
			m_spawnOriginNormal = normal;
			m_spawnOriginBounds = new Bounds(location, new Vector3(m_maxSpawnRange, 5000f, m_maxSpawnRange));
			m_spawnOriginGroundTransform = groundObject;
			if (groundObject != null)
			{
				m_spawnOriginObjectID = groundObject.GetInstanceID();
				if (groundObject.GetComponent<Terrain>() != null)
				{
					m_spawnOriginIsTerrain = true;
				}
				else
				{
					m_spawnOriginIsTerrain = false;
				}
			}
			else
			{
				m_spawnOriginObjectID = int.MinValue;
				m_spawnOriginIsTerrain = false;
			}
			if (m_critCheckTextures)
			{
				Terrain terrain = GetTerrain(m_spawnOriginLocation);
				if (terrain != null)
				{
					Vector3 vector = terrain.transform.InverseTransformPoint(m_spawnOriginLocation);
					Vector3 size = terrain.terrainData.size;
					float x = Mathf.InverseLerp(0f, size.x, vector.x);
					Vector3 size2 = terrain.terrainData.size;
					float y = Mathf.InverseLerp(0f, size2.y, vector.y);
					Vector3 size3 = terrain.terrainData.size;
					Vector3 vector2 = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, vector.z));
					float[,,] alphamaps = terrain.terrainData.GetAlphamaps((int)(vector2.x * (float)(terrain.terrainData.alphamapWidth - 1)), (int)(vector2.z * (float)(terrain.terrainData.alphamapHeight - 1)), 1, 1);
					m_critMaxSelectedTexture = alphamaps.GetLength(2) - 1;
					float num = 0f;
					for (int i = 0; i <= m_critMaxSelectedTexture; i++)
					{
						if (alphamaps[0, 0, i] > num)
						{
							num = (m_critTextureStrength = alphamaps[0, 0, i]);
							m_critSelectedTextureIdx = i;
						}
					}
					m_critSelectedTextureName = terrain.terrainData.splatPrototypes[m_critSelectedTextureIdx].texture.name;
				}
				else
				{
					m_critSelectedTextureName = "Missing terrain";
				}
			}
			if (!Application.isPlaying)
			{
				m_treeManager.LoadTreesFromTerrain();
			}
			if (m_critCheckMask && m_critMaskType == Constants.MaskType.Image && m_critMaskImage != null)
			{
				int width = m_critMaskImage.width;
				int height = m_critMaskImage.height;
				m_critMaskImageData = new Heightmap(width, height);
				m_critMaskAlphaData = new Heightmap(width, height);
				for (int j = 0; j < width; j++)
				{
					for (int k = 0; k < height; k++)
					{
						Color pixel = m_critMaskImage.GetPixel(j, k);
						m_critMaskImageData[j, k] = pixel.r * 2.55E+08f + pixel.g * 255000f + pixel.b * 255f;
						m_critMaskAlphaData[j, k] = pixel.a;
					}
				}
			}
			UpdateTargetSpawnerRanges();
			UpdateChildSpawners();
			foreach (Spawner childSpawner in m_childSpawners)
			{
				if (childSpawner != null)
				{
					childSpawner.SetSpawnOriginAndUpdateRanges(groundObject, location, normal);
				}
			}
		}

		public void UpdateTargetSpawnerRanges()
		{
			m_critMinHeight = m_spawnOriginLocation.y - m_critHeightVariance / 2f;
			if (m_critMinHeight < m_critMinSpawnHeight)
			{
				m_critMinHeight = m_critMinSpawnHeight;
			}
			m_critMaxHeight = m_spawnOriginLocation.y + m_critHeightVariance / 2f;
			if (m_critMaxHeight < m_critMinHeight)
			{
				m_critMaxHeight = m_critMinHeight;
			}
			float num = Vector3.Angle(Vector3.up, m_spawnOriginNormal);
			m_critMinSlope = Mathf.Clamp(num - m_critSlopeVariance / 2f, 0f, 90f);
			m_critMaxSlope = Mathf.Clamp(num + m_critSlopeVariance / 2f, 0f, 90f);
			m_critMinTextureStrength = Mathf.Clamp01(m_critTextureStrength - m_critTextureVariance / 2f);
			m_critMaxTextureStrength = Mathf.Clamp01(m_critTextureStrength + m_critTextureVariance / 2f);
			m_critMaskFractalMin = Mathf.Clamp01(m_critMaskFractalMidpoint - m_critMaskFractalRange / 2f);
			m_critMaskFractalMax = Mathf.Clamp01(m_critMaskFractalMidpoint + m_critMaskFractalRange / 2f);
			m_needsVisualisationUpdate = true;
		}

		private void SetSpawnOrigin(Vector3 location)
		{
			m_spawnOriginLocation = location;
			if (m_spawnOriginIsTerrain)
			{
				Terrain terrain = GetTerrain(location);
				if (terrain != null)
				{
					m_spawnOriginLocation.y = terrain.SampleHeight(location);
				}
			}
			m_spawnOriginBounds = new Bounds(location, new Vector3(m_maxSpawnRange, 5000f, m_maxSpawnRange));
		}

		private void UpdateSpawnerVisualisation()
		{
			m_needsVisualisationUpdate = false;
			Vector3 zero = Vector3.zero;
			float num = m_maxSpawnRange / 2f;
			Vector3 vector = m_spawnOriginLocation + Vector3.one * num;
			Vector3 hitLocation = Vector3.zero;
			Vector3 hitNormal = Vector3.zero;
			float hitAlpha = 1f;
			Vector3 a = Vector3.zero;
			List<Prototype> list = new List<Prototype>();
			foreach (Prototype spawnPrototype in m_spawnPrototypes)
			{
				if (spawnPrototype.m_active)
				{
					list.Add(spawnPrototype);
				}
			}
			int num2 = (int)m_maxSpawnRange + 1;
			if (num2 > m_maxVisualisationDimensions)
			{
				num2 = m_maxVisualisationDimensions + 1;
			}
			float num3 = m_maxSpawnRange / ((float)num2 - 1f);
			if (num2 != m_fitnessArray.GetLength(0))
			{
				m_fitnessArray = new float[num2, num2];
			}
			int i;
			for (i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					m_fitnessArray[i, j] = float.MinValue;
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			if (m_critVirginCheckType == Constants.VirginCheckType.Bounds)
			{
				float num4 = 0f;
				float num5 = 0f;
				Vector3 zero2 = Vector3.zero;
				for (int k = 0; k < m_spawnPrototypes.Count; k++)
				{
					zero2 = m_spawnPrototypes[k].m_extents;
					zero2 += new Vector3((m_critBoundsBorder + m_spawnPrototypes[k].m_boundsBorder) * zero2.x, (m_critBoundsBorder + m_spawnPrototypes[k].m_boundsBorder) * zero2.y, (m_critBoundsBorder + m_spawnPrototypes[k].m_boundsBorder) * zero2.z);
					num4 = zero2.x * zero2.z;
					if (num5 == 0f)
					{
						num5 = num4;
						a = zero2;
					}
					else if (num4 < num5)
					{
						num5 = num4;
						a = zero2;
					}
				}
			}
			DateTime now = DateTime.Now;
			a += new Vector3(m_critBoundsBorder * a.x, m_critBoundsBorder * a.y, m_critBoundsBorder * a.z);
			Prototype selectedPrototype = null;
			float rotation = 0f;
			if (ApproximatelyEqual(m_minRotationY, m_maxRotationY))
			{
				rotation = m_minRotationY;
			}
			i = 0;
			zero.x = m_spawnOriginLocation.x - num;
			while (zero.x < vector.x)
			{
				int j = 0;
				zero.z = m_spawnOriginLocation.z - num;
				while (zero.z < vector.z)
				{
					if (CheckLocationForSpawn(zero, rotation, list, out selectedPrototype, out hitLocation, out hitNormal, out hitAlpha))
					{
						m_fitnessArray[i, j] = hitLocation.y;
						if (m_critVirginCheckType == Constants.VirginCheckType.Bounds)
						{
							if (CheckBoundedLocationForSpawn(hitLocation, rotation, null, a, visualising: true))
							{
								m_fitnessArray[i, j] = hitLocation.y;
							}
							else
							{
								m_fitnessArray[i, j] = float.MinValue;
							}
						}
					}
					j++;
					zero.z += num3;
				}
				i++;
				zero.x += num3;
			}
			if ((DateTime.Now - now).TotalMilliseconds > 200.0)
			{
				double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
				m_maxVisualisationDimensions = (int)((double)(float)m_maxVisualisationDimensions * (200.0 / totalMilliseconds));
				if (m_maxVisualisationDimensions < 1)
				{
					m_maxVisualisationDimensions = 1;
				}
			}
		}

		private void UpdateChildSpawners()
		{
			m_childSpawners.Clear();
			Spawner spawner = null;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					spawner = transform.gameObject.GetComponent<Spawner>();
					if (spawner != null)
					{
						m_childSpawners.Add(spawner);
						spawner.m_showGizmos = false;
						spawner.UpdateChildSpawners();
					}
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
		}

		public void SpawnGlobally()
		{
			float num = float.NaN;
			float num2 = float.NaN;
			float num3 = float.NaN;
			float num4 = float.NaN;
			float num5 = float.NaN;
			if (m_spawnOriginIsTerrain)
			{
				Terrain[] activeTerrains = Terrain.activeTerrains;
				foreach (Terrain terrain in activeTerrains)
				{
					if (float.IsNaN(num2))
					{
						Vector3 position = terrain.transform.position;
						num2 = position.y;
						Vector3 position2 = terrain.transform.position;
						num = position2.x;
						Vector3 position3 = terrain.transform.position;
						num3 = position3.z;
						float num6 = num;
						Vector3 size = terrain.terrainData.size;
						num4 = num6 + size.x;
						float num7 = num3;
						Vector3 size2 = terrain.terrainData.size;
						num5 = num7 + size2.z;
						continue;
					}
					Vector3 position4 = terrain.transform.position;
					if (position4.x < num)
					{
						Vector3 position5 = terrain.transform.position;
						num = position5.x;
					}
					Vector3 position6 = terrain.transform.position;
					if (position6.z < num3)
					{
						Vector3 position7 = terrain.transform.position;
						num3 = position7.z;
					}
					Vector3 position8 = terrain.transform.position;
					float x = position8.x;
					Vector3 size3 = terrain.terrainData.size;
					if (x + size3.x > num4)
					{
						Vector3 position9 = terrain.transform.position;
						float x2 = position9.x;
						Vector3 size4 = terrain.terrainData.size;
						num4 = x2 + size4.x;
					}
					Vector3 position10 = terrain.transform.position;
					float z = position10.z;
					Vector3 size5 = terrain.terrainData.size;
					if (z + size5.z > num5)
					{
						Vector3 position11 = terrain.transform.position;
						float z2 = position11.z;
						Vector3 size6 = terrain.terrainData.size;
						num5 = z2 + size6.z;
					}
				}
			}
			else if (m_spawnOriginGroundTransform != null)
			{
				Bounds bounds = default(Bounds);
				if (GetObjectBounds(m_spawnOriginGroundTransform.gameObject, ref bounds))
				{
					Vector3 min = bounds.min;
					num = min.x;
					Vector3 position12 = m_spawnOriginGroundTransform.position;
					num2 = position12.y;
					Vector3 min2 = bounds.min;
					num3 = min2.z;
					Vector3 max = bounds.max;
					num4 = max.x;
					Vector3 max2 = bounds.max;
					num5 = max2.z;
				}
			}
			Vector3 spawnOriginLocation = m_spawnOriginLocation;
			if (!Application.isPlaying)
			{
				m_treeManager.LoadTreesFromTerrain();
				if (m_autoProbe)
				{
					m_probeManager.LoadProbesFromScene();
					if (m_probeParent == null)
					{
						m_probeParent = GameObject.Find("GeNa Light Probes");
						if (m_probeParent == null)
						{
							m_probeParent = new GameObject("GeNa Light Probes");
						}
					}
				}
			}
			bool flag = false;
			Vector3 location = new Vector3(num, num2, num3);
			for (float num8 = num + JitterAsPct(m_maxSpawnRange, 0.25f); num8 < num4; num8 += JitterAsPct(m_maxSpawnRange, 0.25f))
			{
				for (float num9 = num3 + JitterAsPct(m_maxSpawnRange, 0.25f); num9 < num5; num9 += JitterAsPct(m_maxSpawnRange, 0.25f))
				{
					location.x = JitterAround(num8, m_maxSpawnRange);
					location.z = JitterAround(num9, m_maxSpawnRange);
					Spawn(location, subSpawn: true);
				}
				if (flag)
				{
					break;
				}
			}
			SetSpawnOrigin(spawnOriginLocation);
		}

		public void Spawn(Vector3 location, float rotation, bool subSpawn)
		{
			m_minRotationY = (m_maxRotationY = rotation);
			Spawn(location, subSpawn);
		}

		public void Spawn(Vector3 location, bool subSpawn)
		{
			if (m_spawnPrototypes.Count == 0)
			{
				UnityEngine.Debug.LogWarning("No prototypes to spawn.");
				return;
			}
			SetSpawnOrigin(location);
			float num = m_maxSpawnRange / 2f;
			Vector3 vector = location;
			Vector3 hitLocation = location;
			float hitAlpha = 1f;
			Vector3 vector2 = new Vector3(0f - num, 0f, 0f - num);
			Vector3 vector3 = new Vector3(num, 0f, num);
			Vector3 b = vector2;
			Vector3 vector4 = vector2;
			vector4.x -= m_seedThrowRange;
			Vector3 one = Vector3.one;
			Vector3 zero = Vector3.zero;
			Vector3 hitNormal = m_spawnOriginNormal;
			Prototype prototype = null;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = m_randomGenerator.Next((int)m_minInstances, (int)m_maxInstances);
			long num5 = num4 * 20;
			int num6 = 0;
			float rotation = m_randomGenerator.Next(m_minRotationY, m_maxRotationY);
			GameObject gameObject = null;
			List<Vector3> list = new List<Vector3>();
			List<Prototype> list2 = new List<Prototype>();
			List<GameObject> list3 = new List<GameObject>();
			foreach (Prototype spawnPrototype in m_spawnPrototypes)
			{
				if (spawnPrototype.m_active)
				{
					list2.Add(spawnPrototype);
				}
			}
			if (list2.Count == 0)
			{
				UnityEngine.Debug.LogWarning("No active prototypes to spawn.");
				return;
			}
			float defaultContactOffset = Physics.defaultContactOffset;
			int defaultSolverIterations = Physics.defaultSolverIterations;
			if (!Application.isPlaying)
			{
				Physics.defaultContactOffset = 0.003f;
				Physics.defaultSolverIterations = 25;
			}
			if (!subSpawn && !Application.isPlaying)
			{
				m_treeManager.LoadTreesFromTerrain();
				if (m_autoProbe)
				{
					m_probeManager.LoadProbesFromScene();
					if (m_probeParent == null)
					{
						m_probeParent = GameObject.Find("GeNa Light Probes");
						if (m_probeParent == null)
						{
							m_probeParent = new GameObject("GeNa Light Probes");
						}
					}
				}
			}
			if (m_spawnAlgorithm == Constants.LocationAlgorithm.Every)
			{
				num5 = (long)(m_maxSpawnRange / m_seedThrowRange + 1f);
				num5 *= num5;
			}
			for (; num3 < num4 && num2 < num5; num2++)
			{
				gameObject = null;
				prototype = list2[m_randomGenerator.Next(0, list2.Count - 1)];
				rotation = m_randomGenerator.Next(m_minRotationY, m_maxRotationY);
				if (m_lastSpawnedObject != null && m_rotationAlgorithm != 0)
				{
					if (m_rotationAlgorithm == Constants.RotationAlgorithm.LastSpawnClosest)
					{
						Collider[] componentsInChildren = m_lastSpawnedObject.GetComponentsInChildren<Collider>();
						if (componentsInChildren.Length > 0)
						{
							Vector3 a = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
							Vector3 zero2 = Vector3.zero;
							for (int i = 0; i < componentsInChildren.Length; i++)
							{
								zero2 = componentsInChildren[i].ClosestPointOnBounds(vector);
								if (Vector3.Distance(zero2, vector) < Vector3.Distance(a, vector))
								{
									a = zero2;
								}
							}
							Vector3 eulerAngles = Quaternion.LookRotation(a - vector).eulerAngles;
							rotation = eulerAngles.y;
						}
						else
						{
							Vector3 eulerAngles2 = Quaternion.LookRotation(m_lastSpawnedObject.transform.position - vector).eulerAngles;
							rotation = eulerAngles2.y;
						}
					}
					else
					{
						Vector3 eulerAngles3 = Quaternion.LookRotation(m_lastSpawnedObject.transform.position - vector).eulerAngles;
						rotation = eulerAngles3.y;
					}
				}
				if (!m_sameScale)
				{
					one = new Vector3(m_randomGenerator.Next(m_minScaleX, m_maxScaleX), m_randomGenerator.Next(m_minScaleY, m_maxScaleY), m_randomGenerator.Next(m_minScaleZ, m_maxScaleZ));
				}
				else
				{
					float num7 = m_randomGenerator.Next(m_minScaleX, m_maxScaleX);
					one = new Vector3(num7, num7, num7);
				}
				if (num3 == 0 && !subSpawn && m_advForcePlaceAtClickLocation)
				{
					if (PaintPrototype(prototype, vector, hitNormal, hitAlpha, one, rotation, spawnAtLeastOneResource: true, out gameObject))
					{
						num3++;
					}
					list.Add(vector);
				}
				else
				{
					if (m_spawnAlgorithm != Constants.LocationAlgorithm.Every)
					{
						b = new Vector3(m_randomGenerator.Next(0f - m_seedThrowRange, m_seedThrowRange), 0f, m_randomGenerator.Next(0f - m_seedThrowRange, m_seedThrowRange));
					}
					else
					{
						if (vector4.x < vector3.x)
						{
							vector4.x += m_seedThrowRange;
						}
						else
						{
							vector4.x = vector2.x;
							vector4.z += m_seedThrowRange;
							if (vector4.z > vector3.z)
							{
								num2 = num5;
								continue;
							}
						}
						b.x = vector4.x + m_seedThrowJitter * m_randomGenerator.Next(0f - m_seedThrowRange, m_seedThrowRange);
						b.z = vector4.z + m_seedThrowJitter * m_randomGenerator.Next(0f - m_seedThrowRange, m_seedThrowRange);
					}
					if (m_spawnAlgorithm == Constants.LocationAlgorithm.LastSpawn)
					{
						vector = ((list.Count <= 0) ? (location + b) : (list[list.Count - 1] + b));
					}
					else if (m_spawnAlgorithm == Constants.LocationAlgorithm.Organic)
					{
						if (list.Count > 0)
						{
							vector = list[num6++] + b;
							if (num6 >= list.Count)
							{
								num6 = 0;
							}
						}
						else
						{
							vector = location + b;
						}
					}
					else
					{
						vector = location + b;
					}
					if (m_lastSpawnedObject != null && m_rotationAlgorithm != 0)
					{
						if (m_rotationAlgorithm == Constants.RotationAlgorithm.LastSpawnClosest)
						{
							Collider[] componentsInChildren2 = m_lastSpawnedObject.GetComponentsInChildren<Collider>();
							if (componentsInChildren2.Length > 0)
							{
								Vector3 a2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
								Vector3 zero3 = Vector3.zero;
								for (int j = 0; j < componentsInChildren2.Length; j++)
								{
									zero3 = componentsInChildren2[j].ClosestPointOnBounds(vector);
									if (Vector3.Distance(zero3, vector) < Vector3.Distance(a2, vector))
									{
										a2 = zero3;
									}
								}
								Vector3 eulerAngles4 = Quaternion.LookRotation(a2 - vector).eulerAngles;
								rotation = eulerAngles4.y + m_randomGenerator.Next(m_minRotationY, m_maxRotationY);
							}
							else
							{
								Vector3 eulerAngles5 = Quaternion.LookRotation(m_lastSpawnedObject.transform.position - vector).eulerAngles;
								rotation = eulerAngles5.y + m_randomGenerator.Next(m_minRotationY, m_maxRotationY);
							}
						}
						else
						{
							Vector3 eulerAngles6 = Quaternion.LookRotation(m_lastSpawnedObject.transform.position - vector).eulerAngles;
							rotation = eulerAngles6.y + m_randomGenerator.Next(m_minRotationY, m_maxRotationY);
						}
					}
					if (CheckLocationForSpawn(vector, rotation, list2, out prototype, out hitLocation, out hitNormal, out hitAlpha))
					{
						if (m_critVirginCheckType != Constants.VirginCheckType.Bounds)
						{
							if (PaintPrototype(prototype, hitLocation, hitNormal, hitAlpha, one, rotation, spawnAtLeastOneResource: false, out gameObject))
							{
								num3++;
								list.Add(hitLocation);
							}
						}
						else
						{
							zero = Vector3.Scale(prototype.m_extents, one);
							zero += new Vector3((m_critBoundsBorder + prototype.m_boundsBorder) * prototype.m_extents.x, (m_critBoundsBorder + prototype.m_boundsBorder) * prototype.m_extents.y, (m_critBoundsBorder + prototype.m_boundsBorder) * prototype.m_extents.z);
							vector = hitLocation;
							if (CheckBoundedLocationForSpawn(vector, rotation, prototype, zero, visualising: false) && PaintPrototype(prototype, hitLocation, hitNormal, hitAlpha, one, rotation, spawnAtLeastOneResource: false, out gameObject))
							{
								num3++;
								list.Add(hitLocation);
							}
						}
					}
				}
				if (gameObject != null)
				{
					list3.Add(gameObject);
					m_lastSpawnedObject = gameObject;
				}
			}
			if (list3.Count > 0)
			{
				GameObject gameObject2 = null;
				if (m_mergeSpawns)
				{
					gameObject2 = GameObject.Find(m_parentName);
				}
				if (gameObject2 == null)
				{
					gameObject2 = new GameObject(m_parentName);
					gameObject2.transform.position = location;
				}
				for (int k = 0; k < list3.Count; k++)
				{
					list3[k].transform.parent = gameObject2.transform;
				}
			}
			if (!Application.isPlaying)
			{
				Physics.defaultContactOffset = defaultContactOffset;
				Physics.defaultSolverIterations = defaultSolverIterations;
			}
			foreach (Spawner childSpawner in m_childSpawners)
			{
				if (childSpawner != null && childSpawner.gameObject.activeInHierarchy)
				{
					childSpawner.Spawn(location, rotation, subSpawn: true);
				}
			}
		}

		public void LoadLightProbes()
		{
		}

		private bool PaintPrototype(Prototype prototype, Vector3 location, Vector3 normal, float alpha, Vector3 scaleFactor, float rotation, bool spawnAtLeastOneResource, out GameObject spawnedInstance)
		{
			spawnedInstance = null;
			if (prototype == null)
			{
				UnityEngine.Debug.Log("Missing prototype - aborting paint");
				return false;
			}
			List<GameObject> list = new List<GameObject>();
			List<Gravity.GravityInstance> list2 = new List<Gravity.GravityInstance>();
			Terrain terrain = null;
			Vector3 vector = location;
			Vector3 toDirection = normal;
			Vector3 zero = Vector3.zero;
			Vector3 position = Vector3.zero;
			bool flag = false;
			rotation += prototype.m_forwardRotation;
			if (!spawnAtLeastOneResource && m_critMaskType == Constants.MaskType.Image)
			{
				if (prototype.m_invertMaskedAlpha)
				{
					alpha = 1f - alpha;
				}
				if (prototype.m_successOnMaskedAlpha && m_randomGenerator.Next() > alpha)
				{
					return false;
				}
				if (prototype.m_scaleOnMaskedAlpha)
				{
					float num = prototype.m_scaleOnMaskedAlphaMin + (prototype.m_scaleOnMaskedAlphaMax - prototype.m_scaleOnMaskedAlphaMin) * alpha;
					if (ApproximatelyEqual(num, 0f))
					{
						return false;
					}
					scaleFactor *= num;
				}
			}
			foreach (Resource resource in prototype.m_resources)
			{
				if (spawnAtLeastOneResource)
				{
					if (list.Count > 0 && m_randomGenerator.Next() > resource.m_successRate)
					{
						continue;
					}
				}
				else if (m_randomGenerator.Next() > resource.m_successRate)
				{
					continue;
				}
				vector = location + new Vector3(m_randomGenerator.Next(resource.m_minOffset.x, resource.m_maxOffset.x), m_randomGenerator.Next(resource.m_minOffset.y, resource.m_maxOffset.y), m_randomGenerator.Next(resource.m_minOffset.z, resource.m_maxOffset.z));
				vector = RotatePointAroundPivot(vector, location, new Vector3(0f, rotation, 0f));
				if (m_spawnOriginIsTerrain)
				{
					terrain = GetTerrain(vector);
					if (terrain != null)
					{
						float num2 = terrain.SampleHeight(vector);
						Vector3 position2 = terrain.transform.position;
						vector.y = num2 + position2.y + m_randomGenerator.Next(resource.m_minOffset.y, resource.m_maxOffset.y);
						zero = terrain.transform.InverseTransformPoint(vector);
						Vector3 size = terrain.terrainData.size;
						float x = Mathf.InverseLerp(0f, size.x, zero.x);
						Vector3 size2 = terrain.terrainData.size;
						float y = Mathf.InverseLerp(0f, size2.y, zero.y);
						Vector3 size3 = terrain.terrainData.size;
						position = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, zero.z));
						toDirection = terrain.terrainData.GetInterpolatedNormal(position.x, position.z);
					}
				}
				else
				{
					vector.y += m_randomGenerator.Next(resource.m_minOffset.y, resource.m_maxOffset.y);
					toDirection = normal;
				}
				if (resource.m_resourceType == Constants.ResourceType.Prefab)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(resource.m_prefab);
					gameObject.name = "_Sp_" + resource.m_name;
					if (resource.m_conformToSlope)
					{
						gameObject.name = "_Sp_" + resource.m_name + " C";
					}
					gameObject.transform.position = vector;
					if (m_scaleToNearestInt)
					{
						gameObject.transform.localScale = ScaleToNearestInt(Vector3.Scale(resource.m_baseScale, scaleFactor));
					}
					else
					{
						gameObject.transform.localScale = Vector3.Scale(resource.m_baseScale, scaleFactor);
					}
					gameObject.transform.rotation = Quaternion.Euler(m_randomGenerator.Next(resource.m_minRotation.x, resource.m_maxRotation.x), m_randomGenerator.Next(resource.m_minRotation.y + rotation, resource.m_maxRotation.y + rotation), m_randomGenerator.Next(resource.m_minRotation.z, resource.m_maxRotation.z));
					if (resource.m_conformToSlope)
					{
						gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection) * gameObject.transform.rotation;
					}
					if (m_gravity != null)
					{
						if (!resource.m_hasRootCollider)
						{
							BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
							boxCollider.center = resource.m_baseColliderCenter;
							if (resource.m_baseColliderUseConstScale)
							{
								boxCollider.size = resource.m_baseColliderScale * resource.m_baseColliderConstScaleAmount;
							}
							else
							{
								boxCollider.size = resource.m_baseColliderScale;
							}
						}
						if (!resource.m_hasRigidBody)
						{
							gameObject.AddComponent<Rigidbody>();
						}
						Gravity.GravityInstance gravityInstance = new Gravity.GravityInstance();
						gravityInstance.m_resource = resource;
						gravityInstance.m_instance = gameObject;
						gravityInstance.m_startPosition = gameObject.transform.position;
						gravityInstance.m_startRotation = gameObject.transform.rotation.eulerAngles;
						list2.Add(gravityInstance);
					}
					else
					{
						AutoOptimiseGameObject(resource, gameObject);
						AutoProbeGameObject(resource, gameObject);
					}
					list.Add(gameObject);
					resource.m_instancesSpawned++;
					flag = true;
				}
				else if (resource.m_resourceType == Constants.ResourceType.TerrainTree)
				{
					if (terrain != null && resource.m_terrainProtoIdx < terrain.terrainData.treePrototypes.Length)
					{
						TreeInstance instance = default(TreeInstance);
						instance.prototypeIndex = resource.m_terrainProtoIdx;
						instance.position = position;
						if (m_scaleToNearestInt)
						{
							instance.widthScale = Mathf.Ceil(scaleFactor.x);
							instance.heightScale = Mathf.Ceil(scaleFactor.y);
						}
						else
						{
							instance.widthScale = scaleFactor.x;
							instance.heightScale = scaleFactor.y;
						}
						rotation += m_randomGenerator.Next(resource.m_minRotation.y + rotation, resource.m_maxRotation.y + rotation);
						instance.rotation = rotation * ((float)Math.PI / 180f);
						instance.color = Color.white;
						instance.lightmapColor = Color.white;
						terrain.AddTreeInstance(instance);
						m_treeManager.AddTree(vector, instance.prototypeIndex);
						resource.m_instancesSpawned++;
						flag = true;
					}
				}
				else if (terrain != null && resource.m_terrainProtoIdx < terrain.terrainData.detailPrototypes.Length)
				{
					int xBase = (int)(position.x * (float)(terrain.terrainData.detailWidth - 1));
					int yBase = (int)(position.z * (float)(terrain.terrainData.detailHeight - 1));
					terrain.terrainData.SetDetailLayer(xBase, yBase, resource.m_terrainProtoIdx, new int[1, 1]
					{
						{
							(int)m_randomGenerator.Next(resource.m_minScale.x * 16f, resource.m_maxScale.x * 16f)
						}
					});
					resource.m_instancesSpawned++;
					flag = true;
				}
			}
			if (m_gravity != null)
			{
				m_gravity.AddInstances(list2);
			}
			if (list.Count == 1)
			{
				spawnedInstance = list[0];
				m_prefabUndoList.Add(spawnedInstance);
			}
			else if (list.Count > 1)
			{
				GameObject gameObject = new GameObject(prototype.m_name);
				gameObject.transform.position = location;
				foreach (GameObject item in list)
				{
					item.transform.parent = gameObject.transform;
				}
				if (m_advAddColliderToSpawnedPrefabs)
				{
					SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
					Vector3 vector2 = Vector3.Scale(prototype.m_extents, scaleFactor);
					sphereCollider.radius = Mathf.Max(vector2.x, vector2.z);
					gameObject.AddComponent<DisableColliderOnAwake>();
				}
				spawnedInstance = gameObject;
				m_prefabUndoList.Add(spawnedInstance);
			}
			if (flag)
			{
				prototype.m_instancesSpawned++;
				m_instancesSpawned++;
			}
			return flag;
		}

		private bool CanOptimiseGameObject(Resource resource, GameObject go)
		{
			if (!m_autoOptimise)
			{
				return false;
			}
			if (resource.m_flagForceOptimise)
			{
				return true;
			}
			if (!resource.m_flagCanBeOptimised)
			{
				return false;
			}
			Vector3 vector = Vector3.Scale(resource.m_baseSize, go.transform.localScale);
			if (vector.x < m_maxSizeToOptimize && vector.y < m_maxSizeToOptimize && vector.z < m_maxSizeToOptimize)
			{
				return true;
			}
			return false;
		}

		private void OptimiseGameObject(Resource resource, GameObject go)
		{
		}

		public void AutoOptimiseGameObject(Resource resource, GameObject go)
		{
		}

		private bool CanProbeGameObject(Resource resource, GameObject go)
		{
			if (!m_autoProbe)
			{
				return false;
			}
			if (Application.isPlaying)
			{
				return false;
			}
			return true;
		}

		private void ProbeGameObject(Resource resource, GameObject go)
		{
		}

		private LightProbeGroup GetOrCreateNearestProbeGroup(Vector3 position, out bool canAddNewProbes)
		{
			List<LightProbeGroup> probeGroups = m_probeManager.GetProbeGroups(position, m_minProbeDistance);
			if (probeGroups.Count != 0)
			{
				canAddNewProbes = false;
				return probeGroups[0];
			}
			canAddNewProbes = true;
			probeGroups = m_probeManager.GetProbeGroups(position, m_minProbeGroupDistance);
			if (probeGroups.Count != 0)
			{
				return probeGroups[0];
			}
			GameObject gameObject = new GameObject($"Light Probe Group {position.x:0}x {position.z:0}z");
			gameObject.transform.position = position;
			if (m_probeParent == null)
			{
				m_probeParent = GameObject.Find("GeNa Light Probes");
				if (m_probeParent == null)
				{
					m_probeParent = new GameObject("GeNa Light Probes");
				}
			}
			gameObject.transform.parent = m_probeParent.transform;
			LightProbeGroup lightProbeGroup = gameObject.AddComponent<LightProbeGroup>();
			//lightProbeGroup.probePositions = new Vector3[0];
			return lightProbeGroup;
		}

		public void AutoProbeGameObject(Resource resource, GameObject go)
		{
		}

		private bool CheckLocationForSpawn(Vector3 location, float rotation, List<Prototype> prototypes, out Prototype selectedPrototype, out Vector3 hitLocation, out Vector3 hitNormal, out float hitAlpha)
		{
			selectedPrototype = null;
			hitLocation = location;
			hitNormal = Vector3.up;
			hitAlpha = 0f;
			if (prototypes.Count > 0)
			{
				selectedPrototype = prototypes[m_randomGenerator.Next(0, prototypes.Count - 1)];
				Ray ray = new Ray(new Vector3(location.x, location.y + 10000f, location.z), Vector3.down);
				if (Physics.Raycast(ray, out RaycastHit hitInfo, 20000f))
				{
					hitLocation = hitInfo.point;
					hitNormal = hitInfo.normal;
					if (m_spawnRangeShape == Constants.SpawnRangeShape.Circle)
					{
						if (Vector3.Distance(m_spawnOriginLocation, hitLocation) > m_maxSpawnRange / 2f)
						{
							return false;
						}
					}
					else if (!m_spawnOriginBounds.Contains(hitLocation))
					{
						return false;
					}
					if (m_critCheckHeight && (hitLocation.y < m_critMinHeight || hitLocation.y > m_critMaxHeight))
					{
						return false;
					}
					if (m_critCheckSlope)
					{
						float num = Vector3.Angle(Vector3.up, hitNormal);
						if (num < m_critMinSlope || num > m_critMaxSlope)
						{
							return false;
						}
					}
					if (m_critCheckMask)
					{
						if (m_critMaskType != Constants.MaskType.Image)
						{
							float normalisedValue = m_critMaskFractal.GetNormalisedValue(100000f + hitLocation.x, 100000f + hitLocation.z);
							if (m_critMaskInvert)
							{
								if (normalisedValue >= m_critMaskFractalMin && normalisedValue <= m_critMaskFractalMax)
								{
									return false;
								}
							}
							else if (normalisedValue < m_critMaskFractalMin || normalisedValue > m_critMaskFractalMax)
							{
								return false;
							}
						}
						else if (m_critMaskImageData != null && m_critMaskImageData.HasData())
						{
							Vector3 vector = RotatePointAroundPivot(hitLocation, m_spawnOriginLocation, new Vector3(0f, 180f - rotation, 0f));
							float num2 = (m_spawnOriginLocation.x - vector.x) / m_maxSpawnRange + 0.5f;
							float num3 = (m_spawnOriginLocation.z - vector.z) / m_maxSpawnRange + 0.5f;
							if (num2 < 0f || num2 >= 1f || num3 < 0f || num3 > 1f)
							{
								return false;
							}
							hitAlpha = m_critMaskAlphaData[num2, num3];
							float num4 = m_critMaskImageData[num2, num3];
							Color c = default(Color);
							c.b = num4 % 1000f;
							num4 -= c.b;
							num4 /= 1000f;
							c.b /= 255f;
							c.g = num4 % 1000f;
							num4 -= c.g;
							num4 /= 1000f;
							c.g /= 255f;
							c.r = num4;
							c.r /= 255f;
							List<Prototype> list = new List<Prototype>();
							for (int i = 0; i < prototypes.Count; i++)
							{
								Prototype prototype = prototypes[i];
								if (RGBDifference(c, prototype.m_imageFilterColour) < (1f - prototype.m_imageFilterFuzzyMatch) * 100f)
								{
									list.Add(prototype);
								}
							}
							if (list.Count == 0)
							{
								selectedPrototype = null;
								return false;
							}
							int num5 = 0;
							while (num5 < list.Count)
							{
								if (list[num5].m_successOnMaskedAlpha)
								{
									if (!list[num5].m_invertMaskedAlpha)
									{
										if (ApproximatelyEqual(hitAlpha, 0f))
										{
											list.RemoveAt(num5);
											continue;
										}
									}
									else if (ApproximatelyEqual(1f - hitAlpha, 0f))
									{
										list.RemoveAt(num5);
										continue;
									}
								}
								num5++;
							}
							if (list.Count == 0)
							{
								selectedPrototype = null;
								return false;
							}
							selectedPrototype = list[m_randomGenerator.Next(0, list.Count - 1)];
						}
					}
					Terrain terrain = null;
					if (hitInfo.collider is TerrainCollider)
					{
						terrain = hitInfo.transform.GetComponent<Terrain>();
					}
					if (m_critVirginCheckType != 0)
					{
						if (m_spawnOriginIsTerrain)
						{
							if (terrain == null)
							{
								return false;
							}
							if (m_treeManager.Count(hitLocation, 0.5f) > 0)
							{
								return false;
							}
						}
						else if (hitInfo.transform.GetInstanceID() != m_spawnOriginObjectID)
						{
							return false;
						}
					}
					if (m_critCheckTextures && terrain != null)
					{
						Vector3 vector2 = terrain.transform.InverseTransformPoint(hitLocation);
						Vector3 size = terrain.terrainData.size;
						float x = Mathf.InverseLerp(0f, size.x, vector2.x);
						Vector3 size2 = terrain.terrainData.size;
						float y = Mathf.InverseLerp(0f, size2.y, vector2.y);
						Vector3 size3 = terrain.terrainData.size;
						Vector3 vector3 = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, vector2.z));
						float[,,] alphamaps = terrain.terrainData.GetAlphamaps((int)(vector3.x * (float)(terrain.terrainData.alphamapWidth - 1)), (int)(vector3.z * (float)(terrain.terrainData.alphamapHeight - 1)), 1, 1);
						if (alphamaps.GetLength(2) - 1 < m_critSelectedTextureIdx)
						{
							return false;
						}
						if (alphamaps[0, 0, m_critSelectedTextureIdx] < m_critMinTextureStrength || alphamaps[0, 0, m_critSelectedTextureIdx] > m_critMaxTextureStrength)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			return false;
		}

		private bool CheckBoundedLocationForSpawn(Vector3 location, float rotation, Prototype prototype, Vector3 extents, bool visualising)
		{
			if (m_spawnOriginIsTerrain && m_treeManager.Count(location, Mathf.Max(extents.x, extents.z)) > 0)
			{
				return false;
			}
			float num = m_metersPerScan;
			float num2 = m_metersPerScan;
			if (visualising)
			{
				num = m_metersPerScanVisualisation;
				num2 = m_metersPerScanVisualisation;
			}
			Vector3 origin = new Vector3(location.x - extents.x, location.y + 10000f, location.z - extents.z);
			Vector3 vector = new Vector3(location.x + extents.x, location.y + 10000f, location.z + extents.z);
			Terrain terrain = null;
			origin.x = location.x - extents.x;
			Vector3 vector4 = default(Vector3);
			while (origin.x < vector.x)
			{
				origin.z = location.z - extents.z;
				while (origin.z < vector.z)
				{
					Ray ray = new Ray(origin, Vector3.down);
					if (Physics.Raycast(ray, out RaycastHit hitInfo, 20000f))
					{
						if (m_critCheckHeight)
						{
							Vector3 point = hitInfo.point;
							if (!(point.y < m_critMinHeight))
							{
								Vector3 point2 = hitInfo.point;
								if (!(point2.y > m_critMaxHeight))
								{
									goto IL_0155;
								}
							}
							return false;
						}
						goto IL_0155;
					}
					return false;
					IL_0155:
					if (m_critCheckSlope)
					{
						float num3 = Vector3.Angle(Vector3.up, hitInfo.normal);
						if (num3 < m_critMinSlope || num3 > m_critMaxSlope)
						{
							return false;
						}
					}
					terrain = null;
					if (hitInfo.collider is TerrainCollider)
					{
						terrain = hitInfo.transform.GetComponent<Terrain>();
					}
					if (m_spawnOriginIsTerrain)
					{
						if (terrain == null)
						{
							return false;
						}
					}
					else if (hitInfo.transform.GetInstanceID() != m_spawnOriginObjectID)
					{
						return false;
					}
					if (m_critCheckMask && m_critMaskType == Constants.MaskType.Image && prototype != null && prototype.m_constrainWithinMaskedBounds && m_critMaskImageData != null && m_critMaskImageData.HasData())
					{
						Vector3 vector2 = RotatePointAroundPivot(hitInfo.point, m_spawnOriginLocation, new Vector3(0f, 180f - rotation, 0f));
						float num4 = (m_spawnOriginLocation.x - vector2.x) / m_maxSpawnRange + 0.5f;
						float num5 = (m_spawnOriginLocation.z - vector2.z) / m_maxSpawnRange + 0.5f;
						if (num4 < 0f || num4 >= 1f || num5 < 0f || num5 > 1f)
						{
							return false;
						}
						float num6 = m_critMaskImageData[num4, num5];
						Color c = default(Color);
						c.b = num6 % 1000f;
						num6 -= c.b;
						num6 /= 1000f;
						c.b /= 255f;
						c.g = num6 % 1000f;
						num6 -= c.g;
						num6 /= 1000f;
						c.g /= 255f;
						c.r = num6;
						c.r /= 255f;
						if (RGBDifference(c, prototype.m_imageFilterColour) > (1f - prototype.m_imageFilterFuzzyMatch) * 100f)
						{
							return false;
						}
						if (prototype.m_successOnMaskedAlpha)
						{
							float num7 = m_critMaskAlphaData[num4, num5];
							if (!prototype.m_invertMaskedAlpha)
							{
								if (ApproximatelyEqual(num7, 0f))
								{
									return false;
								}
							}
							else if (ApproximatelyEqual(1f - num7, 0f))
							{
								return false;
							}
						}
					}
					if (m_critCheckTextures && terrain != null && UnityEngine.Random.Range(0, 5) == 1)
					{
						Vector3 vector3 = terrain.transform.InverseTransformPoint(hitInfo.point);
						Vector3 size = terrain.terrainData.size;
						float x = Mathf.InverseLerp(0f, size.x, vector3.x);
						Vector3 size2 = terrain.terrainData.size;
						float y = Mathf.InverseLerp(0f, size2.y, vector3.y);
						Vector3 size3 = terrain.terrainData.size;
						vector4 = new Vector3(x, y, Mathf.InverseLerp(0f, size3.z, vector3.z));
						float[,,] alphamaps = terrain.terrainData.GetAlphamaps((int)(vector4.x * (float)(terrain.terrainData.alphamapWidth - 1)), (int)(vector4.z * (float)(terrain.terrainData.alphamapHeight - 1)), 1, 1);
						if (alphamaps.GetLength(2) - 1 < m_critSelectedTextureIdx)
						{
							return false;
						}
						if (alphamaps[0, 0, m_critSelectedTextureIdx] < m_critMinTextureStrength || alphamaps[0, 0, m_critSelectedTextureIdx] > m_critMaxTextureStrength)
						{
							return false;
						}
					}
					origin.z += num2;
				}
				origin.x += num;
			}
			return true;
		}

		private void LateUpdate()
		{
			if (m_gravity != null && (DateTime.Now - m_lastUpdated).TotalSeconds > 2.0)
			{
				m_lastUpdated = DateTime.Now;
				m_gravity.UpdateInstances();
			}
		}

		public void UnspawnAll()
		{
			m_randomGenerator.Reset();
			foreach (GameObject prefabUndo in m_prefabUndoList)
			{
				UnityEngine.Object.DestroyImmediate(prefabUndo);
			}
			m_prefabUndoList.Clear();
			foreach (Prototype spawnPrototype in m_spawnPrototypes)
			{
				switch (spawnPrototype.m_resourceType)
				{
				case Constants.ResourceType.TerrainTree:
					UnspawnTree(spawnPrototype);
					break;
				case Constants.ResourceType.TerrainGrass:
					UnspawnGrass(spawnPrototype);
					break;
				}
				spawnPrototype.m_instancesSpawned = 0L;
				foreach (Resource resource in spawnPrototype.m_resources)
				{
					resource.m_instancesSpawned = 0L;
				}
			}
			m_instancesSpawned = 0L;
			foreach (Spawner childSpawner in m_childSpawners)
			{
				if (childSpawner != null && childSpawner.gameObject.activeInHierarchy)
				{
					childSpawner.UnspawnAll();
				}
			}
		}

		public void UnspawnGameObject(Prototype proto)
		{
			if (proto.m_resourceType == Constants.ResourceType.Prefab)
			{
			}
		}

		public void UnspawnGrass(Prototype proto)
		{
			if (proto.m_resourceType == Constants.ResourceType.TerrainGrass)
			{
				Resource resource = proto.m_resources[0];
				Terrain[] activeTerrains = Terrain.activeTerrains;
				foreach (Terrain terrain in activeTerrains)
				{
					terrain.terrainData.SetDetailLayer(0, 0, resource.m_terrainProtoIdx, new int[terrain.terrainData.detailWidth, terrain.terrainData.detailWidth]);
				}
				proto.m_instancesSpawned -= resource.m_instancesSpawned;
				m_instancesSpawned -= resource.m_instancesSpawned;
				resource.m_instancesSpawned = 0L;
			}
		}

		public void UnspawnTree(Prototype proto)
		{
			if (proto.m_resourceType != Constants.ResourceType.TerrainTree)
			{
				return;
			}
			List<TreeInstance> list = new List<TreeInstance>();
			Resource resource = proto.m_resources[0];
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain in activeTerrains)
			{
				for (int j = 0; j < terrain.terrainData.treeInstances.Length; j++)
				{
					TreeInstance item = terrain.terrainData.treeInstances[j];
					if (item.prototypeIndex != resource.m_terrainProtoIdx)
					{
						list.Add(item);
						continue;
					}
					resource.m_instancesSpawned--;
					proto.m_instancesSpawned--;
					m_instancesSpawned--;
				}
				terrain.terrainData.treeInstances = list.ToArray();
				list.Clear();
			}
		}

		private float JitterAsPct(float value, float percent)
		{
			return m_randomGenerator.Next(Mathf.Clamp01(percent) * value, value);
		}

		private float JitterAround(float value, float delta)
		{
			return m_randomGenerator.Next(value - delta, value + delta);
		}

		private Vector3 ScaleToNearestInt(Vector3 sourceScale)
		{
			float x = sourceScale.x;
			float y = sourceScale.y;
			float z = sourceScale.z;
			x = ((!(x - Mathf.Floor(x) < Mathf.Ceil(x) - x)) ? Mathf.Ceil(x) : Mathf.Floor(x));
			if (x < 1f)
			{
				x = 1f;
			}
			y = ((!(y - Mathf.Floor(y) < Mathf.Ceil(y) - y)) ? Mathf.Ceil(y) : Mathf.Floor(y));
			if (y < 1f)
			{
				y = 1f;
			}
			z = ((!(z - Mathf.Floor(z) < Mathf.Ceil(z) - z)) ? Mathf.Ceil(z) : Mathf.Floor(z));
			if (z < 1f)
			{
				z = 1f;
			}
			return new Vector3(x, y, z);
		}

		private void CombineMeshes(GameObject go)
		{
			Vector3 position = go.transform.position;
			go.transform.position = Vector3.zero;
			MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
			CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				array[i].mesh = componentsInChildren[i].sharedMesh;
				array[i].transform = componentsInChildren[i].transform.localToWorldMatrix;
				componentsInChildren[i].gameObject.SetActive(value: false);
			}
			if (go.transform.GetComponent<MeshFilter>() == null)
			{
				go.AddComponent<MeshFilter>();
			}
			go.transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
			go.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
			go.transform.gameObject.SetActive(value: true);
			go.transform.position = position;
			go.AddComponent<MeshCollider>();
		}

		private Terrain GetTerrain(Vector3 location)
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain != null)
			{
				vector = activeTerrain.GetPosition();
				vector2 = vector + activeTerrain.terrainData.size;
				if (location.x >= vector.x && location.x <= vector2.x && location.z >= vector.z && location.z <= vector2.z)
				{
					return activeTerrain;
				}
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				activeTerrain = Terrain.activeTerrains[i];
				vector = activeTerrain.GetPosition();
				vector2 = vector + activeTerrain.terrainData.size;
				if (location.x >= vector.x && location.x <= vector2.x && location.z >= vector.z && location.z <= vector2.z)
				{
					return activeTerrain;
				}
			}
			return null;
		}

		private bool GetTerrainBounds(Vector3 location, ref Bounds bounds)
		{
			Terrain terrain = GetTerrain(location);
			if (terrain == null)
			{
				return false;
			}
			bounds.center = terrain.transform.position;
			bounds.size = terrain.terrainData.size;
			bounds.center += bounds.extents;
			return true;
		}

		private bool GetObjectBounds(GameObject go, ref Bounds bounds)
		{
			if (go == null)
			{
				return false;
			}
			bounds.center = go.transform.position;
			bounds.size = Vector3.zero;
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				bounds.Encapsulate(renderer.bounds);
			}
			Collider[] componentsInChildren2 = go.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren2)
			{
				bounds.Encapsulate(collider.bounds);
			}
			return true;
		}

		public static bool ApproximatelyEqual(float a, float b, float delta = float.Epsilon)
		{
			if (a == b || Mathf.Abs(a - b) < delta)
			{
				return true;
			}
			return false;
		}

		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 point2 = point - pivot;
			point2 = Quaternion.Euler(angle) * point2;
			point = point2 + pivot;
			return point;
		}

		private float RGBDifference(Color c1, Color c2)
		{
			if (ApproximatelyEqual(c1.r, c2.r) && ApproximatelyEqual(c1.g, c2.g) && ApproximatelyEqual(c1.b, c2.b))
			{
				return 0f;
			}
			Vector3 vector = RGBtoLAB(c1);
			Vector3 vector2 = RGBtoLAB(c2);
			float num = 0f;
			num += Mathf.Pow(vector.x - vector2.x, 2f);
			num += Mathf.Pow(vector.y - vector2.y, 2f);
			num += Mathf.Pow(vector.z - vector2.z, 2f);
			return Mathf.Max(Mathf.Min(Mathf.Sqrt(num), 100f), 0f);
		}

		private Vector3 RGBtoLAB(Color c)
		{
			return XYZtoLAB(RGBtoXYZ(c));
		}

		private Vector3 RGBtoXYZ(Color c)
		{
			float r = c.r;
			float g = c.g;
			float b = c.b;
			r = ((!(r > 0.04045f)) ? (r / 12.92f) : Mathf.Pow((r + 0.055f) / 1.055f, 2.4f));
			g = ((!(g > 0.04045f)) ? (g / 12.92f) : Mathf.Pow((g + 0.055f) / 1.055f, 2.4f));
			b = ((!(b > 0.04045f)) ? (b / 12.92f) : Mathf.Pow((b + 0.055f) / 1.055f, 2.4f));
			r *= 100f;
			g *= 100f;
			b *= 100f;
			float x = r * 0.4124f + g * 0.3576f + b * 0.1805f;
			float y = r * 0.2126f + g * 0.7152f + b * 0.0722f;
			float z = r * 0.0193f + g * 0.1192f + b * 0.9505f;
			return new Vector3(x, y, z);
		}

		private Vector3 XYZtoLAB(Vector3 c)
		{
			float num = 100f;
			float num2 = 108.883f;
			float num3 = 95.047f;
			float num4 = c.y / num;
			float num5 = c.z / num2;
			float num6 = c.x / num3;
			num6 = ((!(num6 > 0.008856f)) ? (7.787f * num6 + 0.137931034f) : Mathf.Pow(num6, 0.333333343f));
			num4 = ((!((double)num4 > 0.008856)) ? (7.787f * num4 + 0.137931034f) : Mathf.Pow(num4, 0.333333343f));
			num5 = ((!(num5 > 0.008856f)) ? (7.787f * num5 + 0.137931034f) : Mathf.Pow(num5, 0.333333343f));
			float x = 116f * num4 - 16f;
			float y = 500f * (num6 - num4);
			float z = 200f * (num4 - num5);
			return new Vector3(x, y, z);
		}

		private void OnDrawGizmosSelected()
		{
			if (!m_showGizmos || m_spawnOriginLocation == Vector3.zero)
			{
				return;
			}
			float num = m_maxSpawnRange / 2f;
			int num2 = (int)m_maxSpawnRange + 1;
			if (num2 > m_maxVisualisationDimensions)
			{
				num2 = m_maxVisualisationDimensions + 1;
			}
			float num3 = m_maxSpawnRange / ((float)num2 - 1f);
			if (num2 != m_fitnessArray.GetLength(0))
			{
				m_needsVisualisationUpdate = true;
			}
			if (m_needsVisualisationUpdate)
			{
				UpdateSpawnerVisualisation();
			}
			Vector3 zero = Vector3.zero;
			Vector3 vector = m_spawnOriginLocation + Vector3.one * num;
			Gizmos.color = Color.green;
			int num4 = 0;
			zero.x = m_spawnOriginLocation.x - num;
			while (zero.x < vector.x)
			{
				int num5 = 0;
				zero.z = m_spawnOriginLocation.z - num;
				while (zero.z < vector.z)
				{
					zero.y = m_fitnessArray[num4, num5];
					if (zero.y > float.MinValue)
					{
						Gizmos.DrawSphere(zero, num3 / 4f);
					}
					num5++;
					zero.z += num3;
				}
				num4++;
				zero.x += num3;
			}
			if (m_critCheckHeight)
			{
				Bounds bounds = default(Bounds);
				if (GetTerrainBounds(m_critSpawnCentre, ref bounds))
				{
					Vector3 center = bounds.center;
					float x = center.x;
					float critMinSpawnHeight = m_critMinSpawnHeight;
					Vector3 center2 = bounds.center;
					bounds.center = new Vector3(x, critMinSpawnHeight, center2.z);
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
					Gizmos.color = new Color(r, g, b, blue4.a / 2f);
					Gizmos.DrawCube(bounds.center, bounds.size);
				}
			}
		}
	}
}
