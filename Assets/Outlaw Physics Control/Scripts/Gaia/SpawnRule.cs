using Gaia.FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class SpawnRule
	{
		public string m_name;

		public bool m_useExtendedSpawn;

		public float m_minViableFitness = 0.25f;

		public float m_failureRate;

		public ulong m_maxInstances = 40000000uL;

		public bool m_ignoreMaxInstances;

		public float m_minDirection;

		public float m_maxDirection = 359.9f;

		public GaiaConstants.SpawnerResourceType m_resourceType;

		public int m_resourceIdx;

		[fsIgnore]
		public int m_resourceIdxPhysical;

		[fsIgnore]
		public Transform m_spawnParent;

		[fsIgnore]
		public string m_colliderName = "_GaiaCollider_Grass";

		public GaiaConstants.NoiseType m_noiseMask;

		public float m_noiseMaskSeed;

		public int m_noiseMaskOctaves = 8;

		public float m_noiseMaskPersistence = 0.25f;

		public float m_noiseMaskFrequency = 1f;

		public float m_noiseMaskLacunarity = 1.5f;

		public float m_noiseZoom = 50f;

		public float m_noiseStrength = 1f;

		public bool m_noiseInvert;

		private FractalGenerator m_noiseGenerator;

		public bool m_isActive = true;

		public bool m_isFoldedOut;

		public ulong m_currInstanceCnt;

		public ulong m_activeInstanceCnt;

		public ulong m_inactiveInstanceCnt;

		public void Initialise(Spawner spawner)
		{
			if (!m_isActive)
			{
				return;
			}
			if (spawner.m_resources.ResourceIdxOutOfBounds(m_resourceType, m_resourceIdx))
			{
				UnityEngine.Debug.Log($"Warning: {spawner.gameObject.name} - {m_name} :: Disabling rule {m_resourceType} idx {m_resourceIdx}, index out of bounds");
				m_isActive = false;
				return;
			}
			if (!spawner.m_resources.ResourceIsInUnity(m_resourceType, m_resourceIdx))
			{
				UnityEngine.Debug.Log($"Warning: {spawner.gameObject.name} - {m_name} :: Disabling rule {m_resourceType} idx {m_resourceIdx}, resource missing from unity");
				m_isActive = false;
				return;
			}
			m_resourceIdxPhysical = spawner.m_resources.PrototypeIdxInTerrain(m_resourceType, m_resourceIdx);
			if (m_resourceIdxPhysical < 0 && Application.isPlaying)
			{
				UnityEngine.Debug.Log($"Warning: {spawner.gameObject.name} - {m_name} :: Disabling rule as resource is physically missing");
				m_isActive = false;
				return;
			}
			if (m_noiseGenerator == null)
			{
				m_noiseGenerator = new FractalGenerator();
			}
			m_noiseGenerator.Frequency = m_noiseMaskFrequency;
			m_noiseGenerator.Lacunarity = m_noiseMaskLacunarity;
			m_noiseGenerator.Octaves = m_noiseMaskOctaves;
			m_noiseGenerator.Persistence = m_noiseMaskPersistence;
			m_noiseGenerator.Seed = m_noiseMaskSeed;
			switch (m_noiseMask)
			{
			case GaiaConstants.NoiseType.Billow:
				m_noiseGenerator.FractalType = FractalGenerator.Fractals.Billow;
				break;
			case GaiaConstants.NoiseType.Ridged:
				m_noiseGenerator.FractalType = FractalGenerator.Fractals.RidgeMulti;
				break;
			default:
				m_noiseGenerator.FractalType = FractalGenerator.Fractals.Perlin;
				break;
			}
			if (m_resourceType == GaiaConstants.SpawnerResourceType.GameObject)
			{
				if (spawner.m_goParentGameObject == null)
				{
					Transform transform = spawner.transform.Find("Spawned_GameObjects");
					if (transform == null)
					{
						spawner.m_goParentGameObject = new GameObject("Spawned_GameObjects");
					}
					else
					{
						spawner.m_goParentGameObject = transform.gameObject;
					}
				}
				Transform transform2 = spawner.m_goParentGameObject.transform.Find(m_name);
				if (transform2 == null)
				{
					GameObject gameObject = new GameObject(m_name);
					gameObject.transform.parent = spawner.m_goParentGameObject.transform;
					transform2 = gameObject.transform;
				}
				m_spawnParent = transform2;
			}
			if (m_resourceType == GaiaConstants.SpawnerResourceType.GameObject)
			{
				if (spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_dna.m_extParam.ToLower().Contains("nograss"))
				{
					m_colliderName = "_GaiaCollider_NoGrass";
				}
				else
				{
					m_colliderName = "_GaiaCollider_Grass";
				}
			}
			if (m_resourceType == GaiaConstants.SpawnerResourceType.TerrainTree)
			{
				if (spawner.m_resources.m_treePrototypes[m_resourceIdx].m_dna.m_extParam.ToLower().Contains("nograss"))
				{
					m_colliderName = "_GaiaCollider_NoGrass";
				}
				else
				{
					m_colliderName = "_GaiaCollider_Grass";
				}
			}
			SpawnRuleExtension[] array = null;
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				array = spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				array = spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				array = spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.GameObject:
				array = spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			}
			if (array != null)
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					array[i].Initialise();
				}
			}
		}

		public bool ResourceIsInUnity(Spawner spawner)
		{
			if (!spawner.m_resources.ResourceIsInUnity(m_resourceType, m_resourceIdx))
			{
				return false;
			}
			return true;
		}

		public bool ResourceIsLoadedInTerrain(Spawner spawner)
		{
			m_resourceIdxPhysical = spawner.m_resources.PrototypeIdxInTerrain(m_resourceType, m_resourceIdx);
			if (m_resourceIdxPhysical < 0)
			{
				return false;
			}
			return true;
		}

		public void AddResourceToTerrain(Spawner spawner)
		{
			spawner.m_resources.AddPrototypeToTerrain(m_resourceType, m_resourceIdx);
		}

		public float GetFitness(ref SpawnInfo spawnInfo)
		{
			if (!m_isActive)
			{
				return 0f;
			}
			SpawnCritera[] array = null;
			SpawnRuleExtension[] array2 = null;
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				array = spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnCriteria;
				array2 = spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				array = spawnInfo.m_spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnCriteria;
				array2 = spawnInfo.m_spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				array = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnCriteria;
				array2 = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.GameObject:
				array = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnCriteria;
				array2 = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			}
			if (array == null || array.Length == 0)
			{
				return 0f;
			}
			float num = 1f;
			if (m_noiseMask != 0 && m_noiseGenerator != null)
			{
				num = (m_noiseInvert ? Mathf.Clamp01(1f - m_noiseGenerator.GetNormalisedValue(100000f + spawnInfo.m_hitLocationWU.x * (1f / m_noiseZoom), 100000f + spawnInfo.m_hitLocationWU.z * (1f / m_noiseZoom)) * m_noiseStrength) : Mathf.Clamp01(m_noiseGenerator.GetNormalisedValue(100000f + spawnInfo.m_hitLocationWU.x * (1f / m_noiseZoom), 100000f + spawnInfo.m_hitLocationWU.z * (1f / m_noiseZoom)) * m_noiseStrength));
			}
			float num2 = float.MinValue;
			float num3 = 0f;
			foreach (SpawnCritera spawnCritera in array)
			{
				if (spawnCritera.m_checkType == GaiaConstants.SpawnerLocationCheckType.BoundedAreaCheck && !spawnInfo.m_spawner.CheckLocationBounds(ref spawnInfo, GetMaxScaledRadius(ref spawnInfo)))
				{
					return 0f;
				}
				num3 = spawnCritera.GetFitness(ref spawnInfo) * num;
				if (array2 != null)
				{
					for (int j = 0; j < array2.GetLength(0); j++)
					{
						SpawnRuleExtension spawnRuleExtension = array2[j];
						if (spawnRuleExtension != null)
						{
							num3 = spawnRuleExtension.GetFitness(num3, ref spawnInfo);
						}
					}
				}
				if (num3 > num2)
				{
					num2 = num3;
					if (num2 >= 1f)
					{
						return num2;
					}
				}
			}
			if (num2 == float.MinValue)
			{
				return 0f;
			}
			return num2;
		}

		public float GetRadius(ref SpawnInfo spawnInfo)
		{
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				return 1f;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				return spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.m_boundsRadius;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				return spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_dna.m_boundsRadius;
			case GaiaConstants.SpawnerResourceType.GameObject:
				return spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_dna.m_boundsRadius;
			default:
				return 0f;
			}
		}

		public float GetMaxScaledRadius(ref SpawnInfo spawnInfo)
		{
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				return 1f;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				return spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.m_maxScale;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				return spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_dna.m_maxScale;
			case GaiaConstants.SpawnerResourceType.GameObject:
				return spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_dna.m_boundsRadius * spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_dna.m_maxScale;
			default:
				return 0f;
			}
		}

		public float GetSeedThrowRange(ref SpawnInfo spawnInfo)
		{
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				return 1f;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				return spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.m_seedThrow;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				return spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_dna.m_seedThrow;
			case GaiaConstants.SpawnerResourceType.GameObject:
				return spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_dna.m_seedThrow;
			default:
				return 0f;
			}
		}

		public void Spawn(ref SpawnInfo spawnInfo)
		{
			if (!m_isActive || (!m_ignoreMaxInstances && m_activeInstanceCnt > m_maxInstances))
			{
				return;
			}
			m_activeInstanceCnt++;
			SpawnRuleExtension spawnRuleExtension = null;
			SpawnRuleExtension[] array = null;
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				array = spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				array = spawnInfo.m_spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				array = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			case GaiaConstants.SpawnerResourceType.GameObject:
				array = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnExtensions;
				break;
			}
			if (m_resourceType != GaiaConstants.SpawnerResourceType.GameObject)
			{
				spawnInfo.m_spawnRotationY = spawnInfo.m_spawner.GetRandomFloat(0f, 359.9f);
			}
			else
			{
				spawnInfo.m_spawnRotationY = spawnInfo.m_spawner.GetRandomFloat(m_minDirection, m_maxDirection);
			}
			bool flag = false;
			if (array != null)
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					spawnRuleExtension = array[i];
					if (spawnRuleExtension != null && spawnRuleExtension.OverridesSpawn(this, ref spawnInfo))
					{
						flag = true;
						spawnRuleExtension.Spawn(this, ref spawnInfo);
					}
				}
			}
			if (flag)
			{
				return;
			}
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
			{
				if (!(spawnInfo.m_fitness > spawnInfo.m_textureStrengths[m_resourceIdxPhysical]))
				{
					break;
				}
				float num9 = spawnInfo.m_fitness - spawnInfo.m_textureStrengths[m_resourceIdxPhysical];
				float num10 = 1f - spawnInfo.m_textureStrengths[m_resourceIdxPhysical];
				float num11 = 0f;
				if (num10 != 0f)
				{
					num11 = 1f - num9 / num10;
				}
				for (int l = 0; l < spawnInfo.m_textureStrengths.Length; l++)
				{
					if (l == m_resourceIdx)
					{
						spawnInfo.m_textureStrengths[l] = spawnInfo.m_fitness;
					}
					else
					{
						spawnInfo.m_textureStrengths[l] *= num11;
					}
				}
				spawnInfo.m_spawner.SetTextureMapsDirty();
				break;
			}
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
			{
				HeightMap detailMap = spawnInfo.m_spawner.GetDetailMap(spawnInfo.m_hitTerrain.GetInstanceID(), m_resourceIdxPhysical);
				int num8 = 1;
				num8 = ((!spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.m_rndScaleInfluence) ? ((int)Mathf.Clamp(15f * spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.GetScale(spawnInfo.m_fitness), 1f, 15f)) : ((int)Mathf.Clamp(15f * spawnInfo.m_spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_dna.GetScale(spawnInfo.m_fitness, spawnInfo.m_spawner.GetRandomFloat(0f, 1f)), 1f, 15f)));
				if (detailMap == null)
				{
					int xBase = (int)(spawnInfo.m_hitLocationNU.x * (float)spawnInfo.m_hitTerrain.terrainData.detailWidth);
					int yBase = (int)(spawnInfo.m_hitLocationNU.z * (float)spawnInfo.m_hitTerrain.terrainData.detailHeight);
					int[,] detailLayer = spawnInfo.m_hitTerrain.terrainData.GetDetailLayer(xBase, yBase, 1, 1, m_resourceIdxPhysical);
					if (detailLayer[0, 0] < num8)
					{
						detailLayer[0, 0] = num8;
						spawnInfo.m_hitTerrain.terrainData.SetDetailLayer(xBase, yBase, m_resourceIdxPhysical, detailLayer);
					}
				}
				else if (detailMap[spawnInfo.m_hitLocationNU.z, spawnInfo.m_hitLocationNU.x] < (float)num8)
				{
					detailMap[spawnInfo.m_hitLocationNU.z, spawnInfo.m_hitLocationNU.x] = num8;
				}
				break;
			}
			case GaiaConstants.SpawnerResourceType.TerrainTree:
			{
				ResourceProtoTree resourceProtoTree = spawnInfo.m_spawner.m_resources.m_treePrototypes[m_resourceIdx];
				TreeInstance instance = default(TreeInstance);
				instance.prototypeIndex = m_resourceIdxPhysical;
				instance.position = spawnInfo.m_hitLocationNU;
				if (resourceProtoTree.m_dna.m_rndScaleInfluence)
				{
					instance.widthScale = resourceProtoTree.m_dna.GetScale(spawnInfo.m_fitness, spawnInfo.m_spawner.GetRandomFloat(0f, 1f));
				}
				else
				{
					instance.widthScale = resourceProtoTree.m_dna.GetScale(spawnInfo.m_fitness);
				}
				instance.heightScale = instance.widthScale;
				instance.rotation = spawnInfo.m_spawnRotationY * ((float)Math.PI / 180f);
				instance.color = resourceProtoTree.m_healthyColour;
				instance.lightmapColor = Color.white;
				spawnInfo.m_hitTerrain.AddTreeInstance(instance);
				float num7 = resourceProtoTree.m_dna.m_boundsRadius * instance.widthScale;
				GameObject gameObject4 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				gameObject4.name = m_colliderName;
				gameObject4.transform.position = new Vector3(spawnInfo.m_hitLocationWU.x, spawnInfo.m_hitLocationWU.y + num7, spawnInfo.m_hitLocationWU.z);
				gameObject4.GetComponent<MeshRenderer>().enabled = false;
				gameObject4.transform.localScale = new Vector3(num7, num7, num7);
				gameObject4.AddComponent<CapsuleCollider>();
				gameObject4.layer = spawnInfo.m_spawner.m_spawnColliderLayer;
				if (spawnInfo.m_spawner.m_areaBoundsColliderCache == null)
				{
					spawnInfo.m_spawner.m_areaBoundsColliderCache = new GameObject("Bounds_ColliderCache");
					spawnInfo.m_spawner.m_areaBoundsColliderCache.transform.parent = spawnInfo.m_spawner.transform;
				}
				gameObject4.transform.parent = spawnInfo.m_spawner.m_areaBoundsColliderCache.transform;
				break;
			}
			case GaiaConstants.SpawnerResourceType.GameObject:
			{
				ResourceProtoGameObject resourceProtoGameObject = spawnInfo.m_spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx];
				float num = 1f;
				float num2 = 1f;
				float num3 = 0f;
				num = ((!resourceProtoGameObject.m_dna.m_rndScaleInfluence) ? resourceProtoGameObject.m_dna.GetScale(spawnInfo.m_fitness) : resourceProtoGameObject.m_dna.GetScale(spawnInfo.m_fitness, spawnInfo.m_spawner.GetRandomFloat(0f, 1f)));
				int num4 = 0;
				float num5 = resourceProtoGameObject.m_dna.m_boundsRadius * num;
				Vector3 localScale = new Vector3(num, num, num);
				Vector3 vector = spawnInfo.m_hitLocationWU;
				SpawnInfo spawnInfo2 = new SpawnInfo();
				spawnInfo2.m_spawner = spawnInfo.m_spawner;
				spawnInfo2.m_textureStrengths = new float[Terrain.activeTerrain.terrainData.alphamapLayers];
				for (int j = 0; j < resourceProtoGameObject.m_instances.Length; j++)
				{
					ResourceProtoGameObjectInstance resourceProtoGameObjectInstance = resourceProtoGameObject.m_instances[j];
					num4 = spawnInfo.m_spawner.GetRandomInt(resourceProtoGameObjectInstance.m_minInstances, resourceProtoGameObjectInstance.m_maxInstances);
					for (int k = 0; k < num4; k++)
					{
						if (!(spawnInfo.m_spawner.GetRandomFloat(0f, 1f) >= resourceProtoGameObjectInstance.m_failureRate))
						{
							continue;
						}
						vector = spawnInfo.m_hitLocationWU;
						vector.x += spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minSpawnOffsetX, resourceProtoGameObjectInstance.m_maxSpawnOffsetX) * num;
						vector.z += spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minSpawnOffsetZ, resourceProtoGameObjectInstance.m_maxSpawnOffsetZ) * num;
						vector = Utils.RotatePointAroundPivot(vector, spawnInfo.m_hitLocationWU, new Vector3(0f, spawnInfo.m_spawnRotationY, 0f));
						vector.y += 500f;
						if (spawnInfo.m_spawner.CheckLocation(vector, ref spawnInfo2) && (!resourceProtoGameObjectInstance.m_virginTerrain || spawnInfo2.m_wasVirginTerrain))
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(resourceProtoGameObjectInstance.m_desktopPrefab);
							gameObject.name = "_Sp_" + gameObject.name;
							vector = spawnInfo2.m_hitLocationWU;
							vector.y = spawnInfo2.m_terrainHeightWU;
							vector.y += spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minSpawnOffsetY, resourceProtoGameObjectInstance.m_maxSpawnOffsetY) * num;
							gameObject.transform.position = vector;
							if (resourceProtoGameObjectInstance.m_useParentScale)
							{
								num2 = num;
								gameObject.transform.localScale = localScale;
							}
							else
							{
								num3 = Vector3.Distance(spawnInfo.m_hitLocationWU, spawnInfo2.m_hitLocationWU);
								num2 = resourceProtoGameObjectInstance.m_minScale + resourceProtoGameObjectInstance.m_scaleByDistance.Evaluate(num3 / num5) * spawnInfo.m_spawner.GetRandomFloat(0f, resourceProtoGameObjectInstance.m_maxScale - resourceProtoGameObjectInstance.m_minScale);
								gameObject.transform.localScale = new Vector3(num2, num2, num2);
							}
							gameObject.transform.rotation = Quaternion.Euler(new Vector3(spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minRotationOffsetX, resourceProtoGameObjectInstance.m_maxRotationOffsetX), spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minRotationOffsetY + spawnInfo.m_spawnRotationY, resourceProtoGameObjectInstance.m_maxRotationOffsetY + spawnInfo.m_spawnRotationY), spawnInfo.m_spawner.GetRandomFloat(resourceProtoGameObjectInstance.m_minRotationOffsetZ, resourceProtoGameObjectInstance.m_maxRotationOffsetZ)));
							if (resourceProtoGameObject.m_instances[j].m_rotateToSlope)
							{
								gameObject.transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, spawnInfo2.m_terrainNormalWU) * gameObject.transform.rotation;
							}
							if (m_spawnParent != null)
							{
								gameObject.transform.parent = m_spawnParent;
							}
							GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
							if (resourceProtoGameObjectInstance.m_extParam.ToLower().Contains("nograss"))
							{
								gameObject2.name = "_GaiaCollider_NoGrass";
							}
							else
							{
								gameObject2.name = "_GaiaCollider_Grass";
							}
							gameObject2.transform.position = vector;
							gameObject2.GetComponent<MeshRenderer>().enabled = false;
							float num6 = resourceProtoGameObjectInstance.m_localBounds * num2;
							gameObject2.transform.localScale = new Vector3(num6, num6, num6);
							gameObject2.AddComponent<SphereCollider>();
							gameObject2.layer = spawnInfo.m_spawner.m_spawnColliderLayer;
							if (spawnInfo.m_spawner.m_areaBoundsColliderCache == null)
							{
								spawnInfo.m_spawner.m_areaBoundsColliderCache = new GameObject("Bounds_ColliderCache");
								spawnInfo.m_spawner.m_areaBoundsColliderCache.transform.parent = spawnInfo.m_spawner.transform;
							}
							gameObject2.transform.parent = spawnInfo.m_spawner.m_areaBoundsColliderCache.transform;
						}
					}
				}
				GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject3.name = m_colliderName;
				gameObject3.transform.position = spawnInfo.m_hitLocationWU;
				gameObject3.GetComponent<MeshRenderer>().enabled = false;
				gameObject3.transform.localScale = new Vector3(num5, num5, num5);
				gameObject3.AddComponent<SphereCollider>();
				gameObject3.layer = spawnInfo.m_spawner.m_spawnColliderLayer;
				if (spawnInfo.m_spawner.m_areaBoundsColliderCache == null)
				{
					spawnInfo.m_spawner.m_areaBoundsColliderCache = new GameObject("Bounds_ColliderCache");
					spawnInfo.m_spawner.m_areaBoundsColliderCache.transform.parent = spawnInfo.m_spawner.transform;
				}
				gameObject3.transform.parent = spawnInfo.m_spawner.m_areaBoundsColliderCache.transform;
				break;
			}
			}
			if (array == null)
			{
				return;
			}
			for (int i = 0; i < array.GetLength(0); i++)
			{
				spawnRuleExtension = array[i];
				if (spawnRuleExtension != null)
				{
					spawnRuleExtension.PostSpawn(this, ref spawnInfo);
				}
			}
		}

		public bool CacheHeightMaps(Spawner spawner)
		{
			if (!m_isActive)
			{
				return false;
			}
			return false;
		}

		public bool CacheTextures(Spawner spawner)
		{
			if (!m_isActive)
			{
				return false;
			}
			if (m_resourceType == GaiaConstants.SpawnerResourceType.TerrainTexture)
			{
				return true;
			}
			SpawnRuleExtension spawnRuleExtension = null;
			SpawnRuleExtension[] array = null;
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				array = spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int k = 0; k < array.GetLength(0); k++)
					{
						spawnRuleExtension = array[k];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsTextures())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_texturePrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				array = spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int l = 0; l < array.GetLength(0); l++)
					{
						spawnRuleExtension = array[l];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsTextures())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_detailPrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				array = spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int j = 0; j < array.GetLength(0); j++)
					{
						spawnRuleExtension = array[j];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsTextures())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_treePrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.GameObject:
				array = spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int i = 0; i < array.GetLength(0); i++)
					{
						spawnRuleExtension = array[i];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsTextures())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].ChecksTextures();
			default:
				return false;
			}
		}

		public bool CacheDetails(Spawner spawner)
		{
			if (!m_isActive)
			{
				return false;
			}
			if (m_resourceType == GaiaConstants.SpawnerResourceType.TerrainDetail)
			{
				return true;
			}
			SpawnRuleExtension spawnRuleExtension = null;
			SpawnRuleExtension[] array = null;
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				array = spawner.m_resources.m_texturePrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int k = 0; k < array.GetLength(0); k++)
					{
						spawnRuleExtension = array[k];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsDetails())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_texturePrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				array = spawner.m_resources.m_detailPrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int l = 0; l < array.GetLength(0); l++)
					{
						spawnRuleExtension = array[l];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsDetails())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_detailPrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				array = spawner.m_resources.m_treePrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int j = 0; j < array.GetLength(0); j++)
					{
						spawnRuleExtension = array[j];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsDetails())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_treePrototypes[m_resourceIdx].ChecksTextures();
			case GaiaConstants.SpawnerResourceType.GameObject:
				array = spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].m_spawnExtensions;
				if (array != null && array.GetLength(0) > 0)
				{
					for (int i = 0; i < array.GetLength(0); i++)
					{
						spawnRuleExtension = array[i];
						if (spawnRuleExtension != null && spawnRuleExtension.AffectsDetails())
						{
							return true;
						}
					}
				}
				return spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].ChecksTextures();
			default:
				return false;
			}
		}

		public bool CacheProximity(Spawner spawner)
		{
			if (!m_isActive)
			{
				return false;
			}
			switch (m_resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				return spawner.m_resources.m_texturePrototypes[m_resourceIdx].ChecksProximity();
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				return spawner.m_resources.m_detailPrototypes[m_resourceIdx].ChecksProximity();
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				return spawner.m_resources.m_treePrototypes[m_resourceIdx].ChecksProximity();
			case GaiaConstants.SpawnerResourceType.GameObject:
				return spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].ChecksProximity();
			default:
				return false;
			}
		}

		public void AddProximityTags(Spawner spawner, ref List<string> tagList)
		{
			if (m_isActive)
			{
				switch (m_resourceType)
				{
				case GaiaConstants.SpawnerResourceType.TerrainTexture:
					spawner.m_resources.m_texturePrototypes[m_resourceIdx].AddTags(ref tagList);
					break;
				case GaiaConstants.SpawnerResourceType.TerrainDetail:
					spawner.m_resources.m_detailPrototypes[m_resourceIdx].AddTags(ref tagList);
					break;
				case GaiaConstants.SpawnerResourceType.TerrainTree:
					spawner.m_resources.m_treePrototypes[m_resourceIdx].AddTags(ref tagList);
					break;
				case GaiaConstants.SpawnerResourceType.GameObject:
					spawner.m_resources.m_gameObjectPrototypes[m_resourceIdx].AddTags(ref tagList);
					break;
				}
			}
		}
	}
}
