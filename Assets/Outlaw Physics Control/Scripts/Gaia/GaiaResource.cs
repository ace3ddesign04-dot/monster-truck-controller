using Gaia.FullSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class GaiaResource : ScriptableObject
	{
		[Tooltip("Unique identifier for these resources.")]
		[HideInInspector]
		public string m_resourcesID = Guid.NewGuid().ToString();

		[Tooltip("Resource name")]
		public string m_name = "Gaia Resource";

		[Tooltip("The absolute height of the sea or water table in meters. All spawn criteria heights are calculated relative to this. This can also be thought of as the water level. This value is sourced from the defaults file, and managed on a session by session basis.")]
		public float m_seaLevel = 100f;

		[Tooltip("The beach height in meters. Beaches are spawned at sea level and are extended for this height above sea level. This is used when creating default spawn rules in order to create a beach in the zone between water and land.")]
		public float m_beachHeight = 5f;

		[Tooltip("Terrain height.")]
		public float m_terrainHeight = 1000f;

		[Tooltip("Texture prototypes and fitness criteria.")]
		public ResourceProtoTexture[] m_texturePrototypes = new ResourceProtoTexture[0];

		[Tooltip("Detail prototypes, dna and fitness criteria.")]
		public ResourceProtoDetail[] m_detailPrototypes = new ResourceProtoDetail[0];

		[Tooltip("Tree prototypes, dna and fitness criteria.")]
		public ResourceProtoTree[] m_treePrototypes = new ResourceProtoTree[0];

		[Tooltip("Game object prototypes, dna and fitness criteria.")]
		public ResourceProtoGameObject[] m_gameObjectPrototypes = new ResourceProtoGameObject[0];

		public bool SetAssetAssociations()
		{
			int num = 0;
			bool result = false;
			for (num = 0; num < m_texturePrototypes.GetLength(0); num++)
			{
				ResourceProtoTexture resourceProtoTexture = m_texturePrototypes[num];
				if (resourceProtoTexture.SetAssetAssociations())
				{
					result = true;
				}
			}
			for (num = 0; num < m_detailPrototypes.GetLength(0); num++)
			{
				ResourceProtoDetail resourceProtoDetail = m_detailPrototypes[num];
				if (resourceProtoDetail.SetAssetAssociations())
				{
					result = true;
				}
			}
			for (num = 0; num < m_treePrototypes.GetLength(0); num++)
			{
				ResourceProtoTree resourceProtoTree = m_treePrototypes[num];
				if (resourceProtoTree.SetAssetAssociations())
				{
					result = true;
				}
			}
			for (num = 0; num < m_gameObjectPrototypes.GetLength(0); num++)
			{
				ResourceProtoGameObject resourceProtoGameObject = m_gameObjectPrototypes[num];
				if (resourceProtoGameObject.SetAssetAssociations())
				{
					result = true;
				}
			}
			return result;
		}

		public bool AssociateAssets()
		{
			int num = 0;
			bool result = false;
			for (num = 0; num < m_texturePrototypes.GetLength(0); num++)
			{
				ResourceProtoTexture resourceProtoTexture = m_texturePrototypes[num];
				if (resourceProtoTexture.AssociateAssets())
				{
					result = true;
				}
			}
			for (num = 0; num < m_detailPrototypes.GetLength(0); num++)
			{
				ResourceProtoDetail resourceProtoDetail = m_detailPrototypes[num];
				if (resourceProtoDetail.AssociateAssets())
				{
					result = true;
				}
			}
			for (num = 0; num < m_treePrototypes.GetLength(0); num++)
			{
				ResourceProtoTree resourceProtoTree = m_treePrototypes[num];
				if (resourceProtoTree.AssociateAssets())
				{
					result = true;
				}
			}
			for (num = 0; num < m_gameObjectPrototypes.GetLength(0); num++)
			{
				ResourceProtoGameObject resourceProtoGameObject = m_gameObjectPrototypes[num];
				if (resourceProtoGameObject.AssociateAssets())
				{
					result = true;
				}
			}
			return result;
		}

		public void DeletePrototypes()
		{
			m_texturePrototypes = new ResourceProtoTexture[0];
			m_detailPrototypes = new ResourceProtoDetail[0];
			m_treePrototypes = new ResourceProtoTree[0];
			m_gameObjectPrototypes = new ResourceProtoGameObject[0];
		}

		public bool PrototypesMissingFromTerrain()
		{
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain == null)
			{
				UnityEngine.Debug.LogWarning("Could not check assets in terrain as no terrain has been supplied.");
				return false;
			}
			int num = 0;
			for (num = 0; num < m_texturePrototypes.GetLength(0); num++)
			{
				if (PrototypeMissingFromTerrain(GaiaConstants.SpawnerResourceType.TerrainTexture, num))
				{
					return true;
				}
			}
			for (num = 0; num < m_detailPrototypes.GetLength(0); num++)
			{
				if (PrototypeMissingFromTerrain(GaiaConstants.SpawnerResourceType.TerrainDetail, num))
				{
					return true;
				}
			}
			for (num = 0; num < m_treePrototypes.GetLength(0); num++)
			{
				if (PrototypeMissingFromTerrain(GaiaConstants.SpawnerResourceType.TerrainTree, num))
				{
					return true;
				}
			}
			for (num = 0; num < m_gameObjectPrototypes.GetLength(0); num++)
			{
				if (PrototypeMissingFromTerrain(GaiaConstants.SpawnerResourceType.GameObject, num))
				{
					return true;
				}
			}
			return false;
		}

		public bool PrototypeMissingFromTerrain(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			if (PrototypeIdxInTerrain(resourceType, resourceIdx) == -1)
			{
				return true;
			}
			return false;
		}

		public int PrototypeIdxInTerrain(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			int result = -1;
			int num = 0;
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain == null)
			{
				return result;
			}
			if (ResourceIdxOutOfBounds(resourceType, resourceIdx))
			{
				return result;
			}
			if (!ResourceIsInUnity(resourceType, resourceIdx))
			{
				return result;
			}
			switch (resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
			{
				ResourceProtoTexture resourceProtoTexture = m_texturePrototypes[resourceIdx];
				SplatPrototype[] splatPrototypes = activeTerrain.terrainData.splatPrototypes;
				foreach (SplatPrototype splatPrototype in splatPrototypes)
				{
					if (Utils.IsSameTexture(resourceProtoTexture.m_texture, splatPrototype.texture))
					{
						return num;
					}
					num++;
				}
				return result;
			}
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
			{
				ResourceProtoDetail resourceProtoDetail = m_detailPrototypes[resourceIdx];
				DetailPrototype[] detailPrototypes = activeTerrain.terrainData.detailPrototypes;
				foreach (DetailPrototype detailPrototype in detailPrototypes)
				{
					if (resourceProtoDetail.m_renderMode == detailPrototype.renderMode)
					{
						if (Utils.IsSameTexture(resourceProtoDetail.m_detailTexture, detailPrototype.prototypeTexture))
						{
							return num;
						}
						if (Utils.IsSameGameObject(resourceProtoDetail.m_detailProtoype, detailPrototype.prototype))
						{
							return num;
						}
					}
					num++;
				}
				return result;
			}
			case GaiaConstants.SpawnerResourceType.TerrainTree:
			{
				ResourceProtoTree resourceProtoTree = m_treePrototypes[resourceIdx];
				TreePrototype[] treePrototypes = activeTerrain.terrainData.treePrototypes;
				foreach (TreePrototype treePrototype in treePrototypes)
				{
					if (Utils.IsSameGameObject(resourceProtoTree.m_desktopPrefab, treePrototype.prefab))
					{
						return num;
					}
					num++;
				}
				return result;
			}
			case GaiaConstants.SpawnerResourceType.GameObject:
				return resourceIdx;
			default:
				return result;
			}
		}

		public bool ResourceIdxOutOfBounds(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			switch (resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
				if (resourceIdx < 0 || resourceIdx >= m_texturePrototypes.GetLength(0))
				{
					return true;
				}
				return false;
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
				if (resourceIdx < 0 || resourceIdx >= m_detailPrototypes.GetLength(0))
				{
					return true;
				}
				return false;
			case GaiaConstants.SpawnerResourceType.TerrainTree:
				if (resourceIdx < 0 || resourceIdx >= m_treePrototypes.GetLength(0))
				{
					return true;
				}
				return false;
			case GaiaConstants.SpawnerResourceType.GameObject:
				if (resourceIdx < 0 || resourceIdx >= m_gameObjectPrototypes.GetLength(0))
				{
					return true;
				}
				return false;
			default:
				return true;
			}
		}

		public bool ResourceIsInUnity(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			if (ResourceIdxOutOfBounds(resourceType, resourceIdx))
			{
				return false;
			}
			switch (resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
			{
				ResourceProtoTexture resourceProtoTexture = m_texturePrototypes[resourceIdx];
				if (resourceProtoTexture.m_texture == null)
				{
					return false;
				}
				return true;
			}
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
			{
				ResourceProtoDetail resourceProtoDetail = m_detailPrototypes[resourceIdx];
				if (resourceProtoDetail.m_detailTexture == null && resourceProtoDetail.m_detailProtoype == null)
				{
					return false;
				}
				return true;
			}
			case GaiaConstants.SpawnerResourceType.TerrainTree:
			{
				ResourceProtoTree resourceProtoTree = m_treePrototypes[resourceIdx];
				if (resourceProtoTree.m_desktopPrefab == null)
				{
					return false;
				}
				return true;
			}
			case GaiaConstants.SpawnerResourceType.GameObject:
			{
				ResourceProtoGameObject resourceProtoGameObject = m_gameObjectPrototypes[resourceIdx];
				if (resourceProtoGameObject.m_instances[0] == null)
				{
					return false;
				}
				return true;
			}
			default:
				return false;
			}
		}

		public void UpdatePrototypesFromTerrain()
		{
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain == null)
			{
				UnityEngine.Debug.LogWarning("Can not update prototypes from the terrain as there is no terrain currently active in this scene.");
				return;
			}
			Dictionary<string, string> names = new Dictionary<string, string>();
			Vector3 size = activeTerrain.terrainData.size;
			m_terrainHeight = size.y;
			List<ResourceProtoTexture> list = new List<ResourceProtoTexture>(m_texturePrototypes);
			while (list.Count > activeTerrain.terrainData.splatPrototypes.Length)
			{
				list.RemoveAt(list.Count - 1);
			}
			int i;
			for (i = 0; i < activeTerrain.terrainData.splatPrototypes.Length; i++)
			{
				SplatPrototype splatPrototype = activeTerrain.terrainData.splatPrototypes[i];
				ResourceProtoTexture resourceProtoTexture;
				if (i < list.Count)
				{
					resourceProtoTexture = list[i];
				}
				else
				{
					resourceProtoTexture = new ResourceProtoTexture();
					list.Add(resourceProtoTexture);
				}
				resourceProtoTexture.m_name = GetUniqueName(splatPrototype.texture.name, ref names);
				resourceProtoTexture.m_texture = splatPrototype.texture;
				resourceProtoTexture.m_normal = splatPrototype.normalMap;
				ResourceProtoTexture resourceProtoTexture2 = resourceProtoTexture;
				Vector2 tileOffset = splatPrototype.tileOffset;
				resourceProtoTexture2.m_offsetX = tileOffset.x;
				ResourceProtoTexture resourceProtoTexture3 = resourceProtoTexture;
				Vector2 tileOffset2 = splatPrototype.tileOffset;
				resourceProtoTexture3.m_offsetY = tileOffset2.y;
				ResourceProtoTexture resourceProtoTexture4 = resourceProtoTexture;
				Vector2 tileSize = splatPrototype.tileSize;
				resourceProtoTexture4.m_sizeX = tileSize.x;
				ResourceProtoTexture resourceProtoTexture5 = resourceProtoTexture;
				Vector2 tileSize2 = splatPrototype.tileSize;
				resourceProtoTexture5.m_sizeY = tileSize2.y;
				resourceProtoTexture.m_metalic = splatPrototype.metallic;
				resourceProtoTexture.m_smoothness = splatPrototype.smoothness;
				if (resourceProtoTexture.m_spawnCriteria.Length == 0)
				{
					resourceProtoTexture.m_spawnCriteria = new SpawnCritera[1];
					SpawnCritera spawnCritera = new SpawnCritera();
					spawnCritera.m_isActive = true;
					spawnCritera.m_virginTerrain = false;
					spawnCritera.m_checkType = GaiaConstants.SpawnerLocationCheckType.PointCheck;
					switch (i)
					{
					case 0:
						spawnCritera.m_checkHeight = true;
						spawnCritera.m_minHeight = m_seaLevel * -1f;
						spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
						spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkSlope = false;
						spawnCritera.m_minSlope = 0f;
						spawnCritera.m_maxSlope = 90f;
						spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
						spawnCritera.m_checkProximity = false;
						spawnCritera.m_checkTexture = false;
						break;
					case 1:
						spawnCritera.m_checkHeight = true;
						spawnCritera.m_minHeight = 1f;
						spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
						spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.01f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkSlope = false;
						spawnCritera.m_minSlope = 0f;
						spawnCritera.m_maxSlope = 90f;
						spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
						spawnCritera.m_checkProximity = false;
						spawnCritera.m_checkTexture = false;
						break;
					case 2:
						spawnCritera.m_checkHeight = true;
						spawnCritera.m_minHeight = 2f;
						spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
						spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.02f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkSlope = true;
						spawnCritera.m_minSlope = 0f;
						spawnCritera.m_maxSlope = 90f;
						spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.1f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkProximity = false;
						spawnCritera.m_checkTexture = false;
						break;
					case 3:
						spawnCritera.m_checkHeight = false;
						spawnCritera.m_minHeight = m_seaLevel * -1f;
						spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
						spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkSlope = true;
						spawnCritera.m_minSlope = 15f;
						spawnCritera.m_maxSlope = 90f;
						spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.2f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkProximity = false;
						spawnCritera.m_checkTexture = false;
						break;
					default:
						spawnCritera.m_isActive = false;
						spawnCritera.m_checkHeight = false;
						spawnCritera.m_minHeight = UnityEngine.Random.Range(m_beachHeight - m_beachHeight / 4f, m_beachHeight * 2f);
						spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
						spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
						spawnCritera.m_checkSlope = false;
						spawnCritera.m_minSlope = 0f;
						spawnCritera.m_maxSlope = 90f;
						spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
						spawnCritera.m_checkProximity = false;
						spawnCritera.m_checkTexture = false;
						break;
					}
					resourceProtoTexture.m_spawnCriteria[0] = spawnCritera;
				}
			}
			m_texturePrototypes = list.ToArray();
			i = 0;
			names.Clear();
			List<ResourceProtoDetail> list2 = new List<ResourceProtoDetail>(m_detailPrototypes);
			while (list2.Count > activeTerrain.terrainData.detailPrototypes.Length)
			{
				list2.RemoveAt(list2.Count - 1);
			}
			for (i = 0; i < activeTerrain.terrainData.detailPrototypes.Length; i++)
			{
				DetailPrototype detailPrototype = activeTerrain.terrainData.detailPrototypes[i];
				ResourceProtoDetail resourceProtoDetail;
				if (i < list2.Count)
				{
					resourceProtoDetail = list2[i];
				}
				else
				{
					resourceProtoDetail = new ResourceProtoDetail();
					list2.Add(resourceProtoDetail);
				}
				resourceProtoDetail.m_renderMode = detailPrototype.renderMode;
				if (detailPrototype.prototype != null)
				{
					resourceProtoDetail.m_name = GetUniqueName(detailPrototype.prototype.name, ref names);
					resourceProtoDetail.m_detailProtoype = detailPrototype.prototype;
				}
				else
				{
					resourceProtoDetail.m_name = GetUniqueName(detailPrototype.prototypeTexture.name, ref names);
					resourceProtoDetail.m_detailTexture = detailPrototype.prototypeTexture;
				}
				resourceProtoDetail.m_dryColour = detailPrototype.dryColor;
				resourceProtoDetail.m_healthyColour = detailPrototype.healthyColor;
				resourceProtoDetail.m_maxHeight = detailPrototype.maxHeight;
				resourceProtoDetail.m_maxWidth = detailPrototype.maxWidth;
				resourceProtoDetail.m_minHeight = detailPrototype.minHeight;
				resourceProtoDetail.m_minWidth = detailPrototype.minWidth;
				resourceProtoDetail.m_noiseSpread = detailPrototype.noiseSpread;
				resourceProtoDetail.m_bendFactor = detailPrototype.bendFactor;
				if (resourceProtoDetail.m_dna == null)
				{
					resourceProtoDetail.m_dna = new ResourceProtoDNA();
				}
				resourceProtoDetail.m_dna.m_rndScaleInfluence = false;
				resourceProtoDetail.m_dna.Update(i, resourceProtoDetail.m_maxWidth, resourceProtoDetail.m_maxHeight, 0.1f, 1f);
				if (resourceProtoDetail.m_spawnCriteria.Length == 0)
				{
					resourceProtoDetail.m_spawnCriteria = new SpawnCritera[1];
					SpawnCritera spawnCritera = new SpawnCritera();
					spawnCritera.m_isActive = true;
					spawnCritera.m_virginTerrain = true;
					spawnCritera.m_checkType = GaiaConstants.SpawnerLocationCheckType.PointCheck;
					spawnCritera.m_checkHeight = true;
					spawnCritera.m_minHeight = UnityEngine.Random.Range(m_beachHeight * 0.25f, m_beachHeight);
					spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
					spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.05f, 1f), new Keyframe(1f, 0f));
					spawnCritera.m_checkSlope = true;
					spawnCritera.m_minSlope = 0f;
					spawnCritera.m_maxSlope = UnityEngine.Random.Range(25f, 40f);
					spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.05f, 1f), new Keyframe(1f, 0f));
					spawnCritera.m_checkProximity = false;
					spawnCritera.m_checkTexture = false;
					resourceProtoDetail.m_spawnCriteria[0] = spawnCritera;
				}
			}
			m_detailPrototypes = list2.ToArray();
			i = 0;
			names.Clear();
			List<ResourceProtoTree> list3 = new List<ResourceProtoTree>(m_treePrototypes);
			while (list3.Count > activeTerrain.terrainData.treePrototypes.Length)
			{
				list3.RemoveAt(list3.Count - 1);
			}
			for (i = 0; i < activeTerrain.terrainData.treePrototypes.Length; i++)
			{
				TreePrototype treePrototype = activeTerrain.terrainData.treePrototypes[i];
				ResourceProtoTree resourceProtoTree;
				if (i < list3.Count)
				{
					resourceProtoTree = list3[i];
				}
				else
				{
					resourceProtoTree = new ResourceProtoTree();
					list3.Add(resourceProtoTree);
				}
				resourceProtoTree.m_name = GetUniqueName(treePrototype.prefab.name, ref names);
				resourceProtoTree.m_desktopPrefab = (resourceProtoTree.m_mobilePrefab = treePrototype.prefab);
				resourceProtoTree.m_bendFactor = treePrototype.bendFactor;
				if (resourceProtoTree.m_dna == null)
				{
					resourceProtoTree.m_dna = new ResourceProtoDNA();
					resourceProtoTree.m_dna.Update(i);
				}
				UpdateDNA(treePrototype.prefab, ref resourceProtoTree.m_dna);
				resourceProtoTree.m_dna.m_boundsRadius = resourceProtoTree.m_dna.m_width * 0.25f;
				resourceProtoTree.m_dna.m_seedThrow = resourceProtoTree.m_dna.m_height * 1.5f;
				if (resourceProtoTree.m_spawnCriteria.Length == 0)
				{
					resourceProtoTree.m_spawnCriteria = new SpawnCritera[1];
					SpawnCritera spawnCritera = new SpawnCritera();
					spawnCritera.m_isActive = true;
					spawnCritera.m_virginTerrain = true;
					spawnCritera.m_checkType = GaiaConstants.SpawnerLocationCheckType.PointCheck;
					spawnCritera.m_checkHeight = true;
					spawnCritera.m_minHeight = 0f;
					spawnCritera.m_maxHeight = m_terrainHeight - m_seaLevel;
					spawnCritera.m_heightFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
					spawnCritera.m_checkSlope = true;
					spawnCritera.m_minSlope = 0f;
					spawnCritera.m_maxSlope = UnityEngine.Random.Range(25f, 40f);
					spawnCritera.m_slopeFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
					spawnCritera.m_checkProximity = false;
					spawnCritera.m_checkTexture = false;
					resourceProtoTree.m_spawnCriteria[0] = spawnCritera;
				}
			}
			m_treePrototypes = list3.ToArray();
			SetAssetAssociations();
		}

		private string GetUniqueName(string name, ref Dictionary<string, string> names)
		{
			int num = 0;
			string text = name;
			while (names.ContainsKey(text))
			{
				text = name + " " + num.ToString();
				num++;
			}
			names.Add(text, text);
			return text;
		}

		private void UpdateDNA(GameObject prefab, ref ResourceProtoDNA dna)
		{
			if (prefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
				Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					bounds.Encapsulate(renderer.bounds);
				}
				Collider[] componentsInChildren2 = gameObject.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsInChildren2)
				{
					bounds.Encapsulate(collider.bounds);
				}
				UnityEngine.Object.DestroyImmediate(gameObject);
				ResourceProtoDNA obj = dna;
				int protoIdx = dna.m_protoIdx;
				Vector3 size = bounds.size;
				float x = size.x;
				Vector3 size2 = bounds.size;
				obj.Update(protoIdx, x, size2.y);
			}
		}

		public void ChangeHeight(float oldHeight, float newHeight)
		{
			float num = oldHeight - m_seaLevel;
			float maxHeight = newHeight - m_seaLevel;
			for (int i = 0; i < m_texturePrototypes.Length; i++)
			{
				SpawnCritera[] spawnCriteria = m_texturePrototypes[i].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_maxHeight == num)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int k = 0; k < m_detailPrototypes.Length; k++)
			{
				SpawnCritera[] spawnCriteria = m_detailPrototypes[k].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_maxHeight == num)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int m = 0; m < m_treePrototypes.Length; m++)
			{
				SpawnCritera[] spawnCriteria = m_treePrototypes[m].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_maxHeight == num)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int num2 = 0; num2 < m_gameObjectPrototypes.Length; num2++)
			{
				SpawnCritera[] spawnCriteria = m_gameObjectPrototypes[num2].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_maxHeight == num)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
		}

		public void ChangeSeaLevel(float newSeaLevel)
		{
			if (newSeaLevel != m_seaLevel)
			{
				ChangeSeaLevel(m_seaLevel, newSeaLevel);
			}
		}

		public void ChangeSeaLevel(float oldSeaLevel, float newSeaLevel)
		{
			float num = oldSeaLevel * -1f;
			float minHeight = newSeaLevel * -1f;
			float num2 = m_terrainHeight - oldSeaLevel;
			float maxHeight = m_terrainHeight - newSeaLevel;
			for (int i = 0; i < m_texturePrototypes.Length; i++)
			{
				SpawnCritera[] spawnCriteria = m_texturePrototypes[i].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_minHeight == num)
					{
						spawnCritera.m_minHeight = minHeight;
					}
					if (spawnCritera.m_maxHeight == num2)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int k = 0; k < m_detailPrototypes.Length; k++)
			{
				SpawnCritera[] spawnCriteria = m_detailPrototypes[k].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_minHeight == num)
					{
						spawnCritera.m_minHeight = minHeight;
					}
					if (spawnCritera.m_maxHeight == num2)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int m = 0; m < m_treePrototypes.Length; m++)
			{
				SpawnCritera[] spawnCriteria = m_treePrototypes[m].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_minHeight == num)
					{
						spawnCritera.m_minHeight = minHeight;
					}
					if (spawnCritera.m_maxHeight == num2)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			for (int num3 = 0; num3 < m_gameObjectPrototypes.Length; num3++)
			{
				SpawnCritera[] spawnCriteria = m_gameObjectPrototypes[num3].m_spawnCriteria;
				foreach (SpawnCritera spawnCritera in spawnCriteria)
				{
					if (spawnCritera.m_minHeight == num)
					{
						spawnCritera.m_minHeight = minHeight;
					}
					if (spawnCritera.m_maxHeight == num2)
					{
						spawnCritera.m_maxHeight = maxHeight;
					}
				}
			}
			m_seaLevel = newSeaLevel;
		}

		public void ApplyPrototypesToTerrain()
		{
			AssociateAssets();
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain in activeTerrains)
			{
				ApplyPrototypesToTerrain(terrain);
			}
		}

		public void ApplyPrototypesToTerrain(Terrain terrain)
		{
			if (terrain == null)
			{
				UnityEngine.Debug.LogWarning("Can not apply assets to terrain no terrain has been supplied.");
				return;
			}
			List<SplatPrototype> list = new List<SplatPrototype>();
			ResourceProtoTexture[] texturePrototypes = m_texturePrototypes;
			foreach (ResourceProtoTexture resourceProtoTexture in texturePrototypes)
			{
				if (resourceProtoTexture.m_texture != null)
				{
					SplatPrototype splatPrototype = new SplatPrototype();
					splatPrototype.normalMap = resourceProtoTexture.m_normal;
					splatPrototype.tileOffset = new Vector2(resourceProtoTexture.m_offsetX, resourceProtoTexture.m_offsetY);
					splatPrototype.tileSize = new Vector2(resourceProtoTexture.m_sizeX, resourceProtoTexture.m_sizeY);
					splatPrototype.texture = resourceProtoTexture.m_texture;
					list.Add(splatPrototype);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Unable to find resource for " + resourceProtoTexture.m_name + "... ignoring.");
				}
			}
			terrain.terrainData.splatPrototypes = list.ToArray();
			List<DetailPrototype> list2 = new List<DetailPrototype>();
			ResourceProtoDetail[] detailPrototypes = m_detailPrototypes;
			foreach (ResourceProtoDetail resourceProtoDetail in detailPrototypes)
			{
				if (resourceProtoDetail.m_detailProtoype != null || resourceProtoDetail.m_detailTexture != null)
				{
					DetailPrototype detailPrototype = new DetailPrototype();
					detailPrototype.renderMode = resourceProtoDetail.m_renderMode;
					if (resourceProtoDetail.m_detailProtoype != null)
					{
						detailPrototype.usePrototypeMesh = true;
						detailPrototype.prototype = resourceProtoDetail.m_detailProtoype;
					}
					else
					{
						detailPrototype.usePrototypeMesh = false;
						detailPrototype.prototypeTexture = resourceProtoDetail.m_detailTexture;
					}
					detailPrototype.dryColor = resourceProtoDetail.m_dryColour;
					detailPrototype.healthyColor = resourceProtoDetail.m_healthyColour;
					detailPrototype.maxHeight = resourceProtoDetail.m_maxHeight;
					detailPrototype.maxWidth = resourceProtoDetail.m_maxWidth;
					detailPrototype.minHeight = resourceProtoDetail.m_minHeight;
					detailPrototype.minWidth = resourceProtoDetail.m_minWidth;
					detailPrototype.noiseSpread = resourceProtoDetail.m_noiseSpread;
					detailPrototype.bendFactor = resourceProtoDetail.m_bendFactor;
					list2.Add(detailPrototype);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Unable to find resource for " + resourceProtoDetail.m_name + "... ignoring.");
				}
			}
			terrain.terrainData.detailPrototypes = list2.ToArray();
			List<TreePrototype> list3 = new List<TreePrototype>();
			ResourceProtoTree[] treePrototypes = m_treePrototypes;
			foreach (ResourceProtoTree resourceProtoTree in treePrototypes)
			{
				if (resourceProtoTree.m_desktopPrefab != null)
				{
					TreePrototype treePrototype = new TreePrototype();
					treePrototype.bendFactor = resourceProtoTree.m_bendFactor;
					treePrototype.prefab = resourceProtoTree.m_desktopPrefab;
					list3.Add(treePrototype);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Unable to find resource for " + resourceProtoTree.m_name + "... ignoring.");
				}
			}
			terrain.terrainData.treePrototypes = list3.ToArray();
			terrain.Flush();
		}

		public void AddMissingPrototypesToTerrain()
		{
			AssociateAssets();
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain in activeTerrains)
			{
				AddMissingPrototypesToTerrain(terrain);
			}
		}

		public void AddMissingPrototypesToTerrain(Terrain terrain)
		{
			if (terrain == null)
			{
				UnityEngine.Debug.LogWarning("Can not add resources to the terrain as no terrain has been supplied.");
				return;
			}
			bool flag = false;
			List<SplatPrototype> list = new List<SplatPrototype>(terrain.terrainData.splatPrototypes);
			ResourceProtoTexture[] texturePrototypes = m_texturePrototypes;
			foreach (ResourceProtoTexture resourceProtoTexture in texturePrototypes)
			{
				flag = false;
				foreach (SplatPrototype item in list)
				{
					if (Utils.IsSameTexture(item.texture, resourceProtoTexture.m_texture))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					SplatPrototype splatPrototype = new SplatPrototype();
					splatPrototype.normalMap = resourceProtoTexture.m_normal;
					splatPrototype.tileOffset = new Vector2(resourceProtoTexture.m_offsetX, resourceProtoTexture.m_offsetY);
					splatPrototype.tileSize = new Vector2(resourceProtoTexture.m_sizeX, resourceProtoTexture.m_sizeY);
					splatPrototype.texture = resourceProtoTexture.m_texture;
					list.Add(splatPrototype);
				}
			}
			terrain.terrainData.splatPrototypes = list.ToArray();
			List<DetailPrototype> list2 = new List<DetailPrototype>(terrain.terrainData.detailPrototypes);
			ResourceProtoDetail[] detailPrototypes = m_detailPrototypes;
			foreach (ResourceProtoDetail resourceProtoDetail in detailPrototypes)
			{
				flag = false;
				foreach (DetailPrototype item2 in list2)
				{
					if (item2.renderMode == resourceProtoDetail.m_renderMode)
					{
						if (Utils.IsSameTexture(item2.prototypeTexture, resourceProtoDetail.m_detailTexture))
						{
							flag = true;
						}
						if (Utils.IsSameGameObject(item2.prototype, resourceProtoDetail.m_detailProtoype))
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					DetailPrototype detailPrototype = new DetailPrototype();
					detailPrototype.renderMode = resourceProtoDetail.m_renderMode;
					if (resourceProtoDetail.m_detailProtoype != null)
					{
						detailPrototype.usePrototypeMesh = true;
						detailPrototype.prototype = resourceProtoDetail.m_detailProtoype;
					}
					else
					{
						detailPrototype.usePrototypeMesh = false;
						detailPrototype.prototypeTexture = resourceProtoDetail.m_detailTexture;
					}
					detailPrototype.dryColor = resourceProtoDetail.m_dryColour;
					detailPrototype.healthyColor = resourceProtoDetail.m_healthyColour;
					detailPrototype.maxHeight = resourceProtoDetail.m_maxHeight;
					detailPrototype.maxWidth = resourceProtoDetail.m_maxWidth;
					detailPrototype.minHeight = resourceProtoDetail.m_minHeight;
					detailPrototype.minWidth = resourceProtoDetail.m_minWidth;
					detailPrototype.noiseSpread = resourceProtoDetail.m_noiseSpread;
					detailPrototype.bendFactor = resourceProtoDetail.m_bendFactor;
					list2.Add(detailPrototype);
				}
			}
			terrain.terrainData.detailPrototypes = list2.ToArray();
			List<TreePrototype> list3 = new List<TreePrototype>(terrain.terrainData.treePrototypes);
			ResourceProtoTree[] treePrototypes = m_treePrototypes;
			foreach (ResourceProtoTree resourceProtoTree in treePrototypes)
			{
				flag = false;
				foreach (TreePrototype item3 in list3)
				{
					if (Utils.IsSameGameObject(item3.prefab, resourceProtoTree.m_desktopPrefab))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					TreePrototype treePrototype = new TreePrototype();
					treePrototype.bendFactor = resourceProtoTree.m_bendFactor;
					treePrototype.prefab = resourceProtoTree.m_desktopPrefab;
					list3.Add(treePrototype);
				}
			}
			terrain.terrainData.treePrototypes = list3.ToArray();
			terrain.Flush();
		}

		public void AddPrototypeToTerrain(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx)
		{
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain in activeTerrains)
			{
				AddPrototypeToTerrain(resourceType, resourceIdx, terrain);
			}
		}

		public void AddPrototypeToTerrain(GaiaConstants.SpawnerResourceType resourceType, int resourceIdx, Terrain terrain)
		{
			if (ResourceIdxOutOfBounds(resourceType, resourceIdx) || !PrototypeMissingFromTerrain(resourceType, resourceIdx))
			{
				return;
			}
			switch (resourceType)
			{
			case GaiaConstants.SpawnerResourceType.TerrainTexture:
			{
				ResourceProtoTexture resourceProtoTexture = m_texturePrototypes[resourceIdx];
				List<SplatPrototype> list3 = new List<SplatPrototype>(terrain.terrainData.splatPrototypes);
				SplatPrototype splatPrototype = new SplatPrototype();
				splatPrototype.normalMap = resourceProtoTexture.m_normal;
				splatPrototype.tileOffset = new Vector2(resourceProtoTexture.m_offsetX, resourceProtoTexture.m_offsetY);
				splatPrototype.tileSize = new Vector2(resourceProtoTexture.m_sizeX, resourceProtoTexture.m_sizeY);
				splatPrototype.texture = resourceProtoTexture.m_texture;
				list3.Add(splatPrototype);
				terrain.terrainData.splatPrototypes = list3.ToArray();
				break;
			}
			case GaiaConstants.SpawnerResourceType.TerrainDetail:
			{
				ResourceProtoDetail resourceProtoDetail = m_detailPrototypes[resourceIdx];
				List<DetailPrototype> list2 = new List<DetailPrototype>(terrain.terrainData.detailPrototypes);
				DetailPrototype detailPrototype = new DetailPrototype();
				detailPrototype.renderMode = resourceProtoDetail.m_renderMode;
				if (resourceProtoDetail.m_detailProtoype != null)
				{
					detailPrototype.usePrototypeMesh = true;
					detailPrototype.prototype = resourceProtoDetail.m_detailProtoype;
				}
				else
				{
					detailPrototype.usePrototypeMesh = false;
					detailPrototype.prototypeTexture = resourceProtoDetail.m_detailTexture;
				}
				detailPrototype.dryColor = resourceProtoDetail.m_dryColour;
				detailPrototype.healthyColor = resourceProtoDetail.m_healthyColour;
				detailPrototype.maxHeight = resourceProtoDetail.m_maxHeight;
				detailPrototype.maxWidth = resourceProtoDetail.m_maxWidth;
				detailPrototype.minHeight = resourceProtoDetail.m_minHeight;
				detailPrototype.minWidth = resourceProtoDetail.m_minWidth;
				detailPrototype.noiseSpread = resourceProtoDetail.m_noiseSpread;
				detailPrototype.bendFactor = resourceProtoDetail.m_bendFactor;
				list2.Add(detailPrototype);
				terrain.terrainData.detailPrototypes = list2.ToArray();
				break;
			}
			case GaiaConstants.SpawnerResourceType.TerrainTree:
			{
				ResourceProtoTree resourceProtoTree = m_treePrototypes[resourceIdx];
				List<TreePrototype> list = new List<TreePrototype>(terrain.terrainData.treePrototypes);
				TreePrototype treePrototype = new TreePrototype();
				treePrototype.bendFactor = resourceProtoTree.m_bendFactor;
				treePrototype.prefab = resourceProtoTree.m_desktopPrefab;
				list.Add(treePrototype);
				terrain.terrainData.treePrototypes = list.ToArray();
				break;
			}
			}
			terrain.Flush();
		}

		public bool ChecksTextures()
		{
			for (int i = 0; i < m_texturePrototypes.Length; i++)
			{
				if (m_texturePrototypes[i].ChecksTextures())
				{
					return true;
				}
			}
			for (int i = 0; i < m_detailPrototypes.Length; i++)
			{
				if (m_detailPrototypes[i].ChecksTextures())
				{
					return true;
				}
			}
			for (int i = 0; i < m_treePrototypes.Length; i++)
			{
				if (m_treePrototypes[i].ChecksTextures())
				{
					return true;
				}
			}
			for (int i = 0; i < m_gameObjectPrototypes.Length; i++)
			{
				if (m_gameObjectPrototypes[i].ChecksTextures())
				{
					return true;
				}
			}
			return false;
		}

		public bool ChecksProximity()
		{
			for (int i = 0; i < m_texturePrototypes.Length; i++)
			{
				if (m_texturePrototypes[i].ChecksProximity())
				{
					return true;
				}
			}
			for (int i = 0; i < m_detailPrototypes.Length; i++)
			{
				if (m_detailPrototypes[i].ChecksProximity())
				{
					return true;
				}
			}
			for (int i = 0; i < m_treePrototypes.Length; i++)
			{
				if (m_treePrototypes[i].ChecksProximity())
				{
					return true;
				}
			}
			for (int i = 0; i < m_gameObjectPrototypes.Length; i++)
			{
				if (m_gameObjectPrototypes[i].ChecksProximity())
				{
					return true;
				}
			}
			return false;
		}

		public void AddGameObject(GameObject prefab)
		{
			if (prefab == null)
			{
				UnityEngine.Debug.LogWarning("Can't add null game object");
			}
		}

		public void AddGameObject(List<GameObject> prototypes)
		{
			if (prototypes == null || prototypes.Count < 1)
			{
				UnityEngine.Debug.LogWarning("Can't add null or empty prototypes list");
			}
		}

		public GameObject CreateCoverageTextureSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Coverage Texture Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.All;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.EveryLocation;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationIncrement = 1f;
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain != null)
			{
				Spawner spawner = component;
				Vector3 size = activeTerrain.terrainData.size;
				float x = size.x;
				Vector3 size2 = activeTerrain.terrainData.size;
				spawner.m_locationIncrement = Mathf.Clamp(Mathf.Min(x, size2.z) / (float)activeTerrain.terrainData.alphamapWidth, 0.05f, 1f);
			}
			for (int i = 0; i < m_texturePrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_texturePrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.TerrainTexture;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = 0f;
				spawnRule.m_failureRate = 0f;
				spawnRule.m_maxInstances = (ulong)(range * 2f * (range * 2f));
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_ignoreMaxInstances = true;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
				if (i == 2)
				{
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 1.5f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = UnityEngine.Random.Range(0, 5000);
					spawnRule.m_noiseStrength = 1f;
					spawnRule.m_noiseZoom = 150f;
				}
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateCoverageDetailSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Coverage Detail Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.All;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.EveryLocationJittered;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationIncrement = 1.2f;
			for (int i = 0; i < m_detailPrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_detailPrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.TerrainDetail;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = UnityEngine.Random.Range(0.2f, 0.5f);
				spawnRule.m_failureRate = UnityEngine.Random.Range(0.7f, 0.95f);
				spawnRule.m_maxInstances = (ulong)(range * 2f * (range * 2f));
				spawnRule.m_ignoreMaxInstances = true;
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				switch (i)
				{
				case 0:
					spawnRule.m_minViableFitness = 0.1f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = true;
					break;
				case 1:
					spawnRule.m_minViableFitness = 0.1f;
					spawnRule.m_failureRate = 0.8f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = true;
					break;
				case 2:
					spawnRule.m_minViableFitness = 0.4f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = false;
					break;
				case 3:
					spawnRule.m_minViableFitness = 0.2f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 30f;
					spawnRule.m_noiseInvert = true;
					break;
				case 4:
					spawnRule.m_minViableFitness = 0.5f;
					spawnRule.m_failureRate = 0.65f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 13390f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = true;
					break;
				case 5:
					spawnRule.m_minViableFitness = 0.4f;
					spawnRule.m_failureRate = 0.3f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 13390f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 30f;
					spawnRule.m_noiseInvert = false;
					break;
				case 6:
					spawnRule.m_minViableFitness = 0.5f;
					spawnRule.m_failureRate = 0.9f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 1.5f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 6886f;
					spawnRule.m_noiseStrength = 1f;
					spawnRule.m_noiseZoom = 90f;
					spawnRule.m_noiseInvert = false;
					break;
				default:
					spawnRule.m_isActive = false;
					break;
				}
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateOrFindGaia()
		{
			GameObject gameObject = GameObject.Find("Gaia");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia");
			}
			return gameObject;
		}

		public GameObject CreateOrFindSessionManager()
		{
			GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
			ChangeSeaLevel(sessionManager.m_session.m_seaLevel);
			return sessionManager.gameObject;
		}

		public GameObject CreateClusteredDetailSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Clustered Detail Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.Random;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.RandomLocationClustered;
			component.m_locationChecksPerInt = UnityEngine.Random.Range((int)range * 7, (int)range * 10);
			component.m_maxRandomClusterSize = UnityEngine.Random.Range(10, 100);
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationIncrement = 1.5f;
			for (int i = 0; i < m_detailPrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_detailPrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.TerrainDetail;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = UnityEngine.Random.Range(0.3f, 0.6f);
				spawnRule.m_failureRate = UnityEngine.Random.Range(0.1f, 0.3f);
				spawnRule.m_maxInstances = (ulong)(range * 2f * (range * 2f));
				spawnRule.m_ignoreMaxInstances = true;
				spawnRule.m_isActive = false;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
				if (i > 2)
				{
					spawnRule.m_isActive = true;
				}
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateClusteredTreeSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Clustered Tree Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.Random;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.RandomLocationClustered;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationChecksPerInt = (int)range * 5;
			component.m_maxRandomClusterSize = 30;
			for (int i = 0; i < m_treePrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_treePrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.TerrainTree;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = 0.25f;
				spawnRule.m_failureRate = 0f;
				spawnRule.m_maxInstances = (ulong)(range * range / 5f);
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
				switch (i)
				{
				case 0:
					spawnRule.m_minViableFitness = 0.2f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = false;
					break;
				case 1:
					spawnRule.m_minViableFitness = 0.2f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = true;
					break;
				}
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateCoverageTreeSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Coverage Tree Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.Random;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.EveryLocationJittered;
			component.m_locationIncrement = 45f;
			component.m_maxJitteredLocationOffsetPct = 0.85f;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationChecksPerInt = (int)range * 5;
			for (int i = 0; i < m_treePrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_treePrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.TerrainTree;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = 0.25f;
				spawnRule.m_failureRate = 0f;
				spawnRule.m_maxInstances = (ulong)(range * range / 5f);
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
				switch (i)
				{
				case 0:
					spawnRule.m_minViableFitness = 0.2f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = false;
					break;
				case 1:
					spawnRule.m_minViableFitness = 0.2f;
					spawnRule.m_failureRate = 0f;
					spawnRule.m_noiseMask = GaiaConstants.NoiseType.Perlin;
					spawnRule.m_noiseMaskFrequency = 1f;
					spawnRule.m_noiseMaskLacunarity = 2f;
					spawnRule.m_noiseMaskOctaves = 8;
					spawnRule.m_noiseMaskPersistence = 0.25f;
					spawnRule.m_noiseMaskSeed = 0f;
					spawnRule.m_noiseStrength = 1.5f;
					spawnRule.m_noiseZoom = 50f;
					spawnRule.m_noiseInvert = true;
					break;
				}
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateCoverageGameObjectSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Coverage GameObject Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.Random;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.EveryLocationJittered;
			component.m_locationIncrement = 45f;
			component.m_maxJitteredLocationOffsetPct = 0.85f;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationChecksPerInt = (int)range * 5;
			for (int i = 0; i < m_gameObjectPrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_gameObjectPrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.GameObject;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = 0.25f;
				spawnRule.m_failureRate = 0f;
				spawnRule.m_maxInstances = (ulong)(range * range / 5f);
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public GameObject CreateClusteredGameObjectSpawner(float range)
		{
			CreateOrFindSessionManager();
			GameObject gameObject = CreateOrFindGaia();
			GameObject gameObject2 = new GameObject("Clustered GameObject Spawner");
			gameObject2.AddComponent<Spawner>();
			Spawner component = gameObject2.GetComponent<Spawner>();
			component.m_resources = this;
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			component.m_resources = this;
			component.m_mode = GaiaConstants.OperationMode.DesignTime;
			component.m_spawnerShape = GaiaConstants.SpawnerShape.Box;
			component.m_rndGenerator = new XorshiftPlus(component.m_seed);
			component.m_spawnRange = range;
			component.m_spawnFitnessAttenuator = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
			component.m_spawnRuleSelector = GaiaConstants.SpawnerRuleSelector.Random;
			component.m_spawnLocationAlgorithm = GaiaConstants.SpawnerLocation.RandomLocationClustered;
			component.m_spawnCollisionLayers = TerrainHelper.GetActiveTerrainLayer();
			component.m_locationChecksPerInt = 2000;
			component.m_maxRandomClusterSize = 20;
			for (int i = 0; i < m_gameObjectPrototypes.Length; i++)
			{
				SpawnRule spawnRule = new SpawnRule();
				spawnRule.m_name = m_gameObjectPrototypes[i].m_name;
				spawnRule.m_resourceType = GaiaConstants.SpawnerResourceType.GameObject;
				spawnRule.m_resourceIdx = i;
				spawnRule.m_minViableFitness = 0.25f;
				spawnRule.m_failureRate = 0f;
				spawnRule.m_maxInstances = (ulong)range * 2;
				spawnRule.m_isActive = true;
				spawnRule.m_isFoldedOut = false;
				spawnRule.m_useExtendedSpawn = false;
				component.m_activeRuleCnt++;
				component.m_spawnerRules.Add(spawnRule);
			}
			SpawnerGroup.SpawnerInstance spawnerInstance = new SpawnerGroup.SpawnerInstance();
			spawnerInstance.m_name = gameObject2.name;
			spawnerInstance.m_interationsPerSpawn = 1;
			spawnerInstance.m_spawner = component;
			return gameObject2;
		}

		public void ExportTexture()
		{
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				Terrain terrain = Terrain.activeTerrains[i];
				int alphamapWidth = terrain.terrainData.alphamapWidth;
				int alphamapHeight = terrain.terrainData.alphamapHeight;
				int alphamapLayers = terrain.terrainData.alphamapLayers;
				Texture2D texture2D = new Texture2D(alphamapWidth, alphamapHeight, TextureFormat.ARGB32, mipChain: false);
				float[,,] alphamaps = terrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
				for (int j = 0; j < alphamapWidth; j++)
				{
					for (int k = 0; k < alphamapHeight; k++)
					{
						float num3;
						float num2;
						float num;
						float num4 = num3 = (num2 = (num = 0f));
						for (int l = 0; l < alphamapLayers; l++)
						{
							Color pixel = m_texturePrototypes[l].m_texture.GetPixel(j % ((int)m_texturePrototypes[l].m_sizeX / m_texturePrototypes[l].m_texture.width), k % ((int)m_texturePrototypes[l].m_sizeY / m_texturePrototypes[l].m_texture.height));
							num4 += alphamaps[j, k, l] * pixel.r;
							num3 += alphamaps[j, k, l] * pixel.g;
							num2 += alphamaps[j, k, l] * pixel.b;
							num += alphamaps[j, k, l] * pixel.a;
						}
						texture2D.SetPixel(j, k, new Color(num4, num3, num2, num));
					}
				}
				Utils.ExportPNG(terrain.name + " - Export", texture2D);
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
			UnityEngine.Debug.LogError("Attempted to export textures on terrain that does not exist!");
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
			GaiaResource instance = this;
			fsSerializer.TryDeserialize(data, ref instance);
		}
	}
}
