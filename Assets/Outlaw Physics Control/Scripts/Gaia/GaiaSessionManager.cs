using Gaia.FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gaia
{
	[ExecuteInEditMode]
	public class GaiaSessionManager : MonoBehaviour
	{
		public IEnumerator m_updateSessionCoroutine;

		public IEnumerator m_updateOperationCoroutine;

		private bool m_cancelPlayback;

		public GaiaSession m_session;

		public bool m_genShowRandomGenerator;

		public bool m_genShowTerrainHelper;

		public GaiaConstants.GeneratorBorderStyle m_genBorderStyle = GaiaConstants.GeneratorBorderStyle.Water;

		public int m_genNumStampsToGenerate = 10;

		public float m_genScaleWidth = 10f;

		public float m_genScaleHeight = 4f;

		public float m_genChanceOfHills = 0.7f;

		public float m_genChanceOfIslands;

		public float m_genChanceOfLakes;

		public float m_genChanceOfMesas = 0.1f;

		public float m_genChanceOfMountains = 0.1f;

		public float m_genChanceOfPlains;

		public float m_genChanceOfRivers = 0.1f;

		public float m_genChanceOfValleys;

		public float m_genChanceOfVillages;

		public float m_genChanceOfWaterfalls;

		[fsIgnore]
		public Stamper m_currentStamper;

		[fsIgnore]
		public Spawner m_currentSpawner;

		[fsIgnore]
		public DateTime m_lastUpdateDateTime = DateTime.Now;

		[fsIgnore]
		public ulong m_progress;

		private List<string> m_genHillStamps = new List<string>();

		private List<string> m_genIslandStamps = new List<string>();

		private List<string> m_genLakeStamps = new List<string>();

		private List<string> m_genMesaStamps = new List<string>();

		private List<string> m_genMountainStamps = new List<string>();

		private List<string> m_genPlainsStamps = new List<string>();

		private List<string> m_genRiverStamps = new List<string>();

		private List<string> m_genValleyStamps = new List<string>();

		private List<string> m_genVillageStamps = new List<string>();

		private List<string> m_genWaterfallStamps = new List<string>();

		public static GaiaSessionManager GetSessionManager(bool pickupExistingTerrain = false)
		{
			GameObject gameObject = GameObject.Find("Gaia");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia");
			}
			GaiaSessionManager gaiaSessionManager = null;
			GameObject gameObject2 = GameObject.Find("Session Manager");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("Session Manager");
				gaiaSessionManager = gameObject2.AddComponent<GaiaSessionManager>();
				gaiaSessionManager.CreateSession(pickupExistingTerrain);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.position = TerrainHelper.GetActiveTerrainCenter();
			}
			else
			{
				gaiaSessionManager = gameObject2.GetComponent<GaiaSessionManager>();
			}
			return gaiaSessionManager;
		}

		public bool IsLocked()
		{
			if (m_session == null)
			{
				CreateSession();
			}
			return m_session.m_isLocked;
		}

		public bool LockSession()
		{
			if (m_session == null)
			{
				CreateSession();
			}
			bool isLocked = m_session.m_isLocked;
			m_session.m_isLocked = true;
			if (!isLocked)
			{
				SaveSession();
			}
			return isLocked;
		}

		public bool UnLockSession()
		{
			if (m_session == null)
			{
				CreateSession();
			}
			bool isLocked = m_session.m_isLocked;
			m_session.m_isLocked = false;
			if (isLocked)
			{
				SaveSession();
			}
			return isLocked;
		}

		public void AddOperation(GaiaOperation operation)
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant add operation on locked session");
				return;
			}
			m_session.m_operations.Add(operation);
			SaveSession();
		}

		public GaiaOperation GetOperation(int operationIdx)
		{
			if (m_session == null)
			{
				CreateSession();
			}
			if (operationIdx < 0 || operationIdx >= m_session.m_operations.Count)
			{
				return null;
			}
			return m_session.m_operations[operationIdx];
		}

		public void RemoveOperation(int operationIdx)
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant remove operation on locked session");
			}
			else if (operationIdx >= 0 && operationIdx < m_session.m_operations.Count)
			{
				m_session.m_operations.RemoveAt(operationIdx);
				SaveSession();
			}
		}

		public void AddResource(GaiaResource resource)
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant add resource on locked session");
			}
			else if (resource != null && m_session.m_resources.ContainsKey(resource.m_resourcesID + resource.name))
			{
			}
		}

		public void AddDefaults(GaiaDefaults defaults)
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant add defaults on locked session");
			}
			else if (!(defaults != null))
			{
			}
		}

		public void AddPreviewImage(Texture2D image)
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant add preview on locked session");
				return;
			}
			m_session.m_previewImageWidth = image.width;
			m_session.m_previewImageHeight = image.height;
			m_session.m_previewImageBytes = image.GetRawTextureData();
			SaveSession();
		}

		public bool HasPreviewImage()
		{
			if (m_session.m_previewImageWidth > 0 && m_session.m_previewImageHeight > 0 && m_session.m_previewImageBytes.GetLength(0) > 0)
			{
				return true;
			}
			return false;
		}

		public void RemovePreviewImage()
		{
			if (IsLocked())
			{
				UnityEngine.Debug.Log("Cant remove preview on locked session");
				return;
			}
			m_session.m_previewImageWidth = 0;
			m_session.m_previewImageHeight = 0;
			m_session.m_previewImageBytes = new byte[0];
			SaveSession();
		}

		public Texture2D GetPreviewImage()
		{
			if (m_session.m_previewImageBytes.GetLength(0) == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(m_session.m_previewImageWidth, m_session.m_previewImageHeight, TextureFormat.ARGB32, mipChain: false);
			texture2D.LoadRawTextureData(m_session.m_previewImageBytes);
			texture2D.Apply();
			texture2D.name = m_session.m_name;
			return texture2D;
		}

		public void SaveSession()
		{
		}

		public void StartEditorUpdates()
		{
		}

		public void StopEditorUpdates()
		{
			m_currentSpawner = null;
			m_currentStamper = null;
			m_updateOperationCoroutine = null;
			m_updateSessionCoroutine = null;
		}

		private void EditorUpdate()
		{
			if (m_cancelPlayback)
			{
				if (m_currentSpawner != null)
				{
					m_currentSpawner.CancelSpawn();
				}
				if (m_currentStamper != null)
				{
					m_currentStamper.CancelStamp();
				}
				StopEditorUpdates();
			}
			else if (m_updateSessionCoroutine == null && m_updateOperationCoroutine == null)
			{
				StopEditorUpdates();
			}
			else if (m_updateOperationCoroutine != null)
			{
				m_updateOperationCoroutine.MoveNext();
			}
			else
			{
				m_updateSessionCoroutine.MoveNext();
			}
		}

		public GaiaSession CreateSession(bool pickupExistingTerrain = false)
		{
			m_session = ScriptableObject.CreateInstance<GaiaSession>();
			m_session.m_description = "Rocking out at Creativity Central! If you like Gaia please consider rating it :)";
			GaiaSettings gaiaSettings = Utils.GetGaiaSettings();
			if (gaiaSettings != null && gaiaSettings.m_currentDefaults != null)
			{
				m_session.m_seaLevel = gaiaSettings.m_currentDefaults.m_seaLevel;
			}
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain != null)
			{
				GaiaSession session = m_session;
				Vector3 size = activeTerrain.terrainData.size;
				session.m_terrainWidth = (int)size.x;
				GaiaSession session2 = m_session;
				Vector3 size2 = activeTerrain.terrainData.size;
				session2.m_terrainDepth = (int)size2.z;
				GaiaSession session3 = m_session;
				Vector3 size3 = activeTerrain.terrainData.size;
				session3.m_terrainHeight = (int)size3.y;
				if (pickupExistingTerrain)
				{
					GaiaDefaults gaiaDefaults = ScriptableObject.CreateInstance<GaiaDefaults>();
					gaiaDefaults.UpdateFromTerrain();
					GaiaResource gaiaResource = ScriptableObject.CreateInstance<GaiaResource>();
					gaiaResource.UpdatePrototypesFromTerrain();
					gaiaResource.ChangeSeaLevel(m_session.m_seaLevel);
					AddDefaults(gaiaDefaults);
					AddResource(gaiaResource);
					AddOperation(gaiaDefaults.GetTerrainCreationOperation(gaiaResource));
				}
			}
			else if (gaiaSettings != null && gaiaSettings.m_currentDefaults != null)
			{
				m_session.m_terrainWidth = gaiaSettings.m_currentDefaults.m_terrainSize;
				m_session.m_terrainDepth = gaiaSettings.m_currentDefaults.m_terrainHeight;
				m_session.m_terrainHeight = gaiaSettings.m_currentDefaults.m_terrainSize;
			}
			return m_session;
		}

		public void SetSeaLevel(float seaLevel)
		{
			m_session.m_seaLevel = seaLevel;
		}

		public float GetSeaLevel()
		{
			return m_session.m_seaLevel;
		}

		public void ResetSession()
		{
			if (m_session == null)
			{
				UnityEngine.Debug.LogError("Can not erase the session as there is no existing session!");
			}
			else if (m_session.m_isLocked)
			{
				UnityEngine.Debug.LogError("Can not erase the session as it is locked!");
			}
			else if (m_session.m_operations.Count > 1)
			{
				GaiaOperation gaiaOperation = m_session.m_operations[0];
				m_session.m_operations.Clear();
				if (gaiaOperation.m_operationType == GaiaOperation.OperationType.CreateTerrain)
				{
					AddOperation(gaiaOperation);
				}
			}
		}

		public void RandomiseStamps()
		{
			if (m_session == null)
			{
				UnityEngine.Debug.LogError("Can not randomise stamps as there is no existing session!");
				return;
			}
			if (m_session.m_isLocked)
			{
				UnityEngine.Debug.LogError("Can not randomise stamps as the existing session is locked!");
				return;
			}
			Terrain activeTerrain = TerrainHelper.GetActiveTerrain();
			if (activeTerrain == null)
			{
				GaiaSettings gaiaSettings = (GaiaSettings)Utils.GetAssetScriptableObject("GaiaSettings");
				if (gaiaSettings == null)
				{
					UnityEngine.Debug.LogError("Can not randomise stamps as we are missing the terrain and settings!");
					return;
				}
				GaiaDefaults currentDefaults = gaiaSettings.m_currentDefaults;
				GaiaResource currentResources = gaiaSettings.m_currentResources;
				if (currentDefaults == null || currentResources == null)
				{
					UnityEngine.Debug.LogError("Can not randomise stamps as we are missing the terrain defaults or resources!");
					return;
				}
				currentDefaults.CreateTerrain(currentResources);
			}
			Bounds bounds = default(Bounds);
			TerrainHelper.GetTerrainBounds(activeTerrain, ref bounds);
			GameObject gameObject = GameObject.Find("Gaia");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia");
			}
			Stamper stamper = null;
			GameObject gameObject2 = GameObject.Find("Stamper");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("Stamper");
				gameObject2.transform.parent = gameObject.transform;
				stamper = gameObject2.AddComponent<Stamper>();
			}
			else
			{
				stamper = gameObject2.GetComponent<Stamper>();
			}
			for (int i = 0; i < m_genNumStampsToGenerate; i++)
			{
				string empty = string.Empty;
				GaiaConstants.FeatureType featureType = GaiaConstants.FeatureType.Hills;
				stamper.LoadStamp(empty);
				stamper.FitToTerrain();
				stamper.HidePreview();
				if (i == 0)
				{
					float width = stamper.m_width;
					PositionStamp(bounds, stamper, featureType);
					stamper.m_rotation = 0f;
					stamper.m_x = 0f;
					stamper.m_z = 0f;
					stamper.m_width = width;
					if (m_genBorderStyle == GaiaConstants.GeneratorBorderStyle.Mountains)
					{
						stamper.m_distanceMask = new AnimationCurve(new Keyframe(1f, 1f), new Keyframe(1f, 1f));
						stamper.m_areaMaskMode = GaiaConstants.ImageFitnessFilterMode.ImageGreyScale;
						stamper.m_imageMask = (Utils.GetAsset("Island Mask 1.jpg", typeof(Texture2D)) as Texture2D);
						stamper.m_imageMaskNormalise = true;
						stamper.m_imageMaskInvert = true;
					}
					else
					{
						stamper.m_distanceMask = new AnimationCurve(new Keyframe(1f, 1f), new Keyframe(1f, 1f));
						stamper.m_areaMaskMode = GaiaConstants.ImageFitnessFilterMode.ImageGreyScale;
						stamper.m_imageMask = (Utils.GetAsset("Island Mask 1.jpg", typeof(Texture2D)) as Texture2D);
						stamper.m_imageMaskNormalise = true;
						stamper.m_imageMaskInvert = false;
					}
				}
				else
				{
					PositionStamp(bounds, stamper, featureType);
					float num = UnityEngine.Random.Range(0f, 1f);
					if (num < 0.1f)
					{
						stamper.m_stampOperation = GaiaConstants.FeatureOperation.LowerHeight;
						stamper.m_invertStamp = true;
					}
					else if (num < 0.35f)
					{
						stamper.m_stampOperation = GaiaConstants.FeatureOperation.StencilHeight;
						stamper.m_normaliseStamp = true;
						if (featureType == GaiaConstants.FeatureType.Rivers || featureType == GaiaConstants.FeatureType.Lakes)
						{
							stamper.m_invertStamp = true;
							stamper.m_stencilHeight = UnityEngine.Random.Range(-80f, -5f);
						}
						else if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
						{
							stamper.m_invertStamp = true;
							stamper.m_stencilHeight = UnityEngine.Random.Range(-80f, -5f);
						}
						else
						{
							stamper.m_invertStamp = false;
							stamper.m_stencilHeight = UnityEngine.Random.Range(5f, 80f);
						}
					}
					else
					{
						stamper.m_stampOperation = GaiaConstants.FeatureOperation.RaiseHeight;
						stamper.m_invertStamp = false;
					}
				}
				stamper.UpdateStamp();
				stamper.AddToSession(GaiaOperation.OperationType.Stamp, "Stamping " + stamper.m_stampPreviewImage.name);
			}
		}

		private void PositionStamp(Bounds bounds, Stamper stamper, GaiaConstants.FeatureType stampType)
		{
			float baseLevel = 0f;
			float minHeight = 0f;
			float maxHeight = 0f;
			float num = stamper.m_height * 4f;
			float num2 = 0f;
			if ((float)m_session.m_terrainHeight > 0f)
			{
				num2 = m_session.m_seaLevel / (float)m_session.m_terrainHeight;
			}
			if (stamper.GetHeightRange(ref baseLevel, ref minHeight, ref maxHeight))
			{
				stamper.m_stampOperation = GaiaConstants.FeatureOperation.RaiseHeight;
				stamper.m_invertStamp = false;
				stamper.m_normaliseStamp = false;
				stamper.m_rotation = UnityEngine.Random.Range(-179f, 179f);
				stamper.m_width = UnityEngine.Random.Range(0.7f, 1.3f) * m_genScaleWidth;
				stamper.m_height = UnityEngine.Random.Range(0.7f, 1.3f) * m_genScaleHeight;
				float num3 = stamper.m_height / num * (float)m_session.m_terrainHeight;
				float num4 = num3 / 2f;
				float num5 = num2 * (float)m_session.m_terrainHeight;
				stamper.m_stickBaseToGround = false;
				stamper.m_y = num4 + num5 - baseLevel * num3;
				float num6 = 1f;
				if (m_genBorderStyle == GaiaConstants.GeneratorBorderStyle.None)
				{
					Vector3 extents = bounds.extents;
					float min = 0f - extents.x;
					Vector3 extents2 = bounds.extents;
					stamper.m_x = UnityEngine.Random.Range(min, extents2.x);
					Vector3 extents3 = bounds.extents;
					float min2 = 0f - extents3.z;
					Vector3 extents4 = bounds.extents;
					stamper.m_z = UnityEngine.Random.Range(min2, extents4.z);
				}
				else
				{
					num6 = 0.65f;
					Vector3 extents5 = bounds.extents;
					float min3 = 0f - extents5.x * num6;
					Vector3 extents6 = bounds.extents;
					stamper.m_x = UnityEngine.Random.Range(min3, extents6.x * num6);
					Vector3 extents7 = bounds.extents;
					float min4 = 0f - extents7.z * num6;
					Vector3 extents8 = bounds.extents;
					stamper.m_z = UnityEngine.Random.Range(min4, extents8.z * num6);
				}
				stamper.m_distanceMask = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
				stamper.m_areaMaskMode = GaiaConstants.ImageFitnessFilterMode.None;
				stamper.m_imageMask = null;
			}
		}

		private GaiaConstants.FeatureType GetWeightedRandomFeatureType()
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			float num2 = m_genChanceOfHills + m_genChanceOfIslands + m_genChanceOfLakes + m_genChanceOfMesas + m_genChanceOfMountains + m_genChanceOfPlains + m_genChanceOfRivers + m_genChanceOfValleys + m_genChanceOfVillages + m_genChanceOfWaterfalls;
			if (num2 == 0f)
			{
				num2 = 1f;
			}
			float num3 = 0f;
			float num4 = 0f;
			num4 = num3 + m_genChanceOfHills / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Hills;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfIslands / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Islands;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfLakes / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Lakes;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfMesas / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Mesas;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfMountains / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Mountains;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfPlains / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Plains;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfRivers / num2;
			if (num >= num3 && num < num4)
			{
				return GaiaConstants.FeatureType.Rivers;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfValleys / num2;
			if (num >= num3 && num < num3)
			{
				return GaiaConstants.FeatureType.Valleys;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfVillages / num2;
			if (num >= num3 && num < num3)
			{
				return GaiaConstants.FeatureType.Villages;
			}
			num3 = num4;
			num4 = num3 + m_genChanceOfWaterfalls / num2;
			if (num >= num3 && num < num3)
			{
				return GaiaConstants.FeatureType.Waterfalls;
			}
			return (GaiaConstants.FeatureType)UnityEngine.Random.Range(2, 7);
		}

		public string GetRandomStampPath(GaiaConstants.FeatureType featureType)
		{
			switch (featureType)
			{
			case GaiaConstants.FeatureType.Adhoc:
				return string.Empty;
			case GaiaConstants.FeatureType.Bases:
				return string.Empty;
			case GaiaConstants.FeatureType.Hills:
				if (m_genHillStamps.Count == 0)
				{
					m_genHillStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Hills);
				}
				if (m_genHillStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genHillStamps[UnityEngine.Random.Range(0, m_genHillStamps.Count - 1)];
			case GaiaConstants.FeatureType.Islands:
				if (m_genIslandStamps.Count == 0)
				{
					m_genIslandStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Islands);
				}
				if (m_genIslandStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genIslandStamps[UnityEngine.Random.Range(0, m_genIslandStamps.Count - 1)];
			case GaiaConstants.FeatureType.Lakes:
				if (m_genLakeStamps.Count == 0)
				{
					m_genLakeStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Lakes);
				}
				if (m_genLakeStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genLakeStamps[UnityEngine.Random.Range(0, m_genLakeStamps.Count - 1)];
			case GaiaConstants.FeatureType.Mesas:
				if (m_genMesaStamps.Count == 0)
				{
					m_genMesaStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Mesas);
				}
				if (m_genMesaStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genMesaStamps[UnityEngine.Random.Range(0, m_genMesaStamps.Count - 1)];
			case GaiaConstants.FeatureType.Mountains:
				if (m_genMountainStamps.Count == 0)
				{
					m_genMountainStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Mountains);
				}
				if (m_genMountainStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genMountainStamps[UnityEngine.Random.Range(0, m_genMountainStamps.Count - 1)];
			case GaiaConstants.FeatureType.Plains:
				if (m_genPlainsStamps.Count == 0)
				{
					m_genPlainsStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Plains);
				}
				if (m_genPlainsStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genPlainsStamps[UnityEngine.Random.Range(0, m_genPlainsStamps.Count - 1)];
			case GaiaConstants.FeatureType.Rivers:
				if (m_genRiverStamps.Count == 0)
				{
					m_genRiverStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Rivers);
				}
				if (m_genRiverStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genRiverStamps[UnityEngine.Random.Range(0, m_genRiverStamps.Count - 1)];
			case GaiaConstants.FeatureType.Rocks:
				return string.Empty;
			case GaiaConstants.FeatureType.Valleys:
				if (m_genValleyStamps.Count == 0)
				{
					m_genValleyStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Valleys);
				}
				if (m_genValleyStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genValleyStamps[UnityEngine.Random.Range(0, m_genValleyStamps.Count - 1)];
			case GaiaConstants.FeatureType.Villages:
				if (m_genVillageStamps.Count == 0)
				{
					m_genVillageStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Villages);
				}
				if (m_genVillageStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genVillageStamps[UnityEngine.Random.Range(0, m_genVillageStamps.Count - 1)];
			case GaiaConstants.FeatureType.Waterfalls:
				if (m_genWaterfallStamps.Count == 0)
				{
					m_genWaterfallStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Waterfalls);
				}
				if (m_genWaterfallStamps.Count == 0)
				{
					return string.Empty;
				}
				return m_genWaterfallStamps[UnityEngine.Random.Range(0, m_genWaterfallStamps.Count - 1)];
			default:
				return string.Empty;
			}
		}

		public string GetRandomMountainFieldPath()
		{
			if (m_genMountainStamps.Count == 0)
			{
				m_genMountainStamps = Utils.GetGaiaStampsList(GaiaConstants.FeatureType.Mountains);
			}
			if (m_genMountainStamps.Count == 0)
			{
				return string.Empty;
			}
			int num = 0;
			int num2 = 0;
			for (num = 0; num < m_genMountainStamps.Count; num++)
			{
				string text = m_genMountainStamps[num];
				if (text.Contains("Field"))
				{
					num2++;
				}
			}
			int num3 = 0;
			int num4 = UnityEngine.Random.Range(0, num2 - 1);
			for (num = 0; num < m_genMountainStamps.Count; num++)
			{
				string text = m_genMountainStamps[num];
				if (text.Contains("Field"))
				{
					if (num3 == num4)
					{
						return text;
					}
					num3++;
				}
			}
			return string.Empty;
		}

		public GameObject Apply(int operationIdx)
		{
			if (operationIdx < 0 || operationIdx >= m_session.m_operations.Count)
			{
				UnityEngine.Debug.LogWarning($"Can not Apply operation because the index {operationIdx} is out of bounds.");
				return null;
			}
			GaiaOperation gaiaOperation = m_session.m_operations[operationIdx];
			GameObject gameObject = FindOrCreateObject(gaiaOperation);
			if (gameObject == null)
			{
				return gameObject;
			}
			Stamper component = gameObject.GetComponent<Stamper>();
			if (component != null && gaiaOperation.m_operationType == GaiaOperation.OperationType.Stamp)
			{
				component.DeSerialiseJson(gaiaOperation.m_operationDataJson[0]);
				component.m_resources = (Utils.GetAsset(ScriptableObjectWrapper.GetSessionedFileName(m_session.GetSessionFileName(), component.m_resourcesPath), typeof(GaiaResource)) as GaiaResource);
				if (component.m_resources == null)
				{
					ExportSessionResource(component.m_resourcesPath);
					component.m_resources = (Utils.GetAsset(ScriptableObjectWrapper.GetSessionedFileName(m_session.GetSessionFileName(), component.m_resourcesPath), typeof(GaiaResource)) as GaiaResource);
				}
				component.m_seaLevel = m_session.m_seaLevel;
			}
			Spawner component2 = gameObject.GetComponent<Spawner>();
			if (component2 != null && gaiaOperation.m_operationType == GaiaOperation.OperationType.Spawn)
			{
				component2.DeSerialiseJson(gaiaOperation.m_operationDataJson[0]);
				component2.m_resources = (Utils.GetAsset(ScriptableObjectWrapper.GetSessionedFileName(m_session.GetSessionFileName(), component2.m_resourcesPath), typeof(GaiaResource)) as GaiaResource);
				if (component2.m_resources == null)
				{
					ExportSessionResource(component2.m_resourcesPath);
					component2.m_resources = (Utils.GetAsset(ScriptableObjectWrapper.GetSessionedFileName(m_session.GetSessionFileName(), component2.m_resourcesPath), typeof(GaiaResource)) as GaiaResource);
				}
				if (component2.m_resources == null)
				{
					UnityEngine.Debug.LogError("Unable to get resources file for " + component2.name);
				}
				else
				{
					component2.AssociateAssets();
					int[] missingResources = component2.GetMissingResources();
					if (missingResources.GetLength(0) > 0)
					{
						component2.AddResourcesToTerrain(missingResources);
					}
					component2.m_resources.ChangeSeaLevel(m_session.m_seaLevel);
				}
			}
			return gameObject;
		}

		public void PlaySession()
		{
			m_cancelPlayback = false;
			ExportSessionResources();
			StartCoroutine(PlaySessionCoRoutine());
		}

		public IEnumerator PlaySessionCoRoutine()
		{
			m_progress = 0uL;
			if (Application.isPlaying)
			{
				for (int idx2 = 0; idx2 < m_session.m_operations.Count; idx2++)
				{
					if (!m_cancelPlayback && m_session.m_operations[idx2].m_isActive)
					{
						yield return StartCoroutine(PlayOperationCoRoutine(idx2));
					}
				}
			}
			else
			{
				for (int idx = 0; idx < m_session.m_operations.Count; idx++)
				{
					if (!m_cancelPlayback && m_session.m_operations[idx].m_isActive)
					{
						m_updateOperationCoroutine = PlayOperationCoRoutine(idx);
						yield return new WaitForSeconds(0.2f);
					}
				}
			}
			UnityEngine.Debug.Log("Finished playing session " + m_session.m_name);
			m_updateSessionCoroutine = null;
		}

		public void PlayOperation(int opIdx)
		{
			m_cancelPlayback = false;
			StartCoroutine(PlayOperationCoRoutine(opIdx));
		}

		public IEnumerator PlayOperationCoRoutine(int operationIdx)
		{
			if (operationIdx < 0 || operationIdx >= m_session.m_operations.Count)
			{
				UnityEngine.Debug.LogWarning($"Operation index {operationIdx} is out of bounds.");
				m_updateOperationCoroutine = null;
				yield break;
			}
			if (!m_session.m_operations[operationIdx].m_isActive)
			{
				UnityEngine.Debug.LogWarning($"Operation '{m_session.m_operations[operationIdx].m_description}' is not active. Ignoring.");
				m_updateOperationCoroutine = null;
				yield break;
			}
			bool lockState = m_session.m_isLocked;
			m_session.m_isLocked = true;
			GaiaOperation operation = m_session.m_operations[operationIdx];
			GameObject go = Apply(operationIdx);
			Stamper stamper = null;
			Spawner spawner = null;
			if (go != null)
			{
				stamper = go.GetComponent<Stamper>();
				spawner = go.GetComponent<Spawner>();
			}
			switch (operation.m_operationType)
			{
			case GaiaOperation.OperationType.CreateTerrain:
				if (TerrainHelper.GetActiveTerrainCount() == 0 && m_session.m_defaults != null && m_session.m_defaults.m_content.GetLength(0) <= 0)
				{
				}
				break;
			case GaiaOperation.OperationType.FlattenTerrain:
				if (stamper != null)
				{
					stamper.FlattenTerrain();
				}
				break;
			case GaiaOperation.OperationType.SmoothTerrain:
				if (stamper != null)
				{
					stamper.SmoothTerrain();
				}
				break;
			case GaiaOperation.OperationType.ClearDetails:
				if (stamper != null)
				{
					stamper.ClearDetails();
				}
				break;
			case GaiaOperation.OperationType.ClearTrees:
				if (stamper != null)
				{
					stamper.ClearTrees();
				}
				break;
			case GaiaOperation.OperationType.Stamp:
				if (!(stamper != null))
				{
					break;
				}
				m_currentStamper = stamper;
				m_currentSpawner = null;
				if (!Application.isPlaying)
				{
					stamper.HidePreview();
					stamper.Stamp();
					while (stamper.IsStamping())
					{
						if ((DateTime.Now - m_lastUpdateDateTime).Milliseconds > 250)
						{
							m_lastUpdateDateTime = DateTime.Now;
							m_progress++;
						}
						yield return new WaitForSeconds(0.2f);
					}
				}
				else
				{
					yield return StartCoroutine(stamper.ApplyStamp());
				}
				break;
			case GaiaOperation.OperationType.StampUndo:
				if (stamper != null)
				{
					stamper.Undo();
				}
				break;
			case GaiaOperation.OperationType.StampRedo:
				if (stamper != null)
				{
					stamper.Redo();
				}
				break;
			case GaiaOperation.OperationType.Spawn:
				if (!(spawner != null))
				{
					break;
				}
				m_currentStamper = null;
				m_currentSpawner = spawner;
				if (Application.isPlaying)
				{
					break;
				}
				spawner.RunSpawnerIteration();
				while (spawner.IsSpawning())
				{
					if ((DateTime.Now - m_lastUpdateDateTime).Milliseconds > 250)
					{
						m_lastUpdateDateTime = DateTime.Now;
						m_progress++;
					}
					yield return new WaitForSeconds(0.2f);
				}
				break;
			}
			m_session.m_isLocked = lockState;
			m_updateOperationCoroutine = null;
		}

		public void CancelPlayback()
		{
			m_cancelPlayback = true;
			if (m_currentStamper != null)
			{
				m_currentStamper.CancelStamp();
			}
			if (m_currentSpawner != null)
			{
				m_currentSpawner.CancelSpawn();
			}
		}

		public void ExportSessionResources()
		{
			string text = "Assets/GaiaSessions/";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			text = Path.Combine(text, Utils.FixFileName(m_session.m_name));
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (m_session.m_defaults != null && m_session.m_defaults.m_content.GetLength(0) > 0)
			{
				string path = Path.Combine(text, ScriptableObjectWrapper.GetSessionedFileName(m_session.m_name, m_session.m_defaults.m_fileName));
				Utils.WriteAllBytes(path, m_session.m_defaults.m_content);
			}
			foreach (KeyValuePair<string, ScriptableObjectWrapper> resource in m_session.m_resources)
			{
				string path2 = Path.Combine(text, ScriptableObjectWrapper.GetSessionedFileName(m_session.m_name, resource.Value.m_fileName));
				Utils.WriteAllBytes(path2, resource.Value.m_content);
			}
		}

		public void ExportSessionDefaults()
		{
			string text = "Assets/GaiaSessions/";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			text = Path.Combine(text, Utils.FixFileName(m_session.m_name));
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (m_session.m_defaults != null && m_session.m_defaults.m_content.GetLength(0) > 0)
			{
				string path = Path.Combine(text, ScriptableObjectWrapper.GetSessionedFileName(m_session.m_name, m_session.m_defaults.m_fileName));
				Utils.WriteAllBytes(path, m_session.m_defaults.m_content);
			}
		}

		public void ExportSessionResource(string resourcePath)
		{
			string text = "Assets/GaiaSessions/";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			text = Path.Combine(text, Utils.FixFileName(m_session.m_name));
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			foreach (KeyValuePair<string, ScriptableObjectWrapper> resource in m_session.m_resources)
			{
				if (Path.GetFileName(resourcePath).ToLower() == Path.GetFileName(resource.Value.m_fileName).ToLower())
				{
					string path = Path.Combine(text, ScriptableObjectWrapper.GetSessionedFileName(m_session.m_name, resource.Value.m_fileName));
					Utils.WriteAllBytes(path, resource.Value.m_content);
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (m_session != null)
			{
				Bounds bounds = default(Bounds);
				if (TerrainHelper.GetTerrainBounds(base.transform.position, ref bounds))
				{
					Gizmos.color = Color.white;
					Gizmos.DrawWireCube(bounds.center, bounds.size);
					Vector3 center = bounds.center;
					float x = center.x;
					float seaLevel = m_session.m_seaLevel;
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

		private GameObject FindOrCreateObject(GaiaOperation operation)
		{
			if (operation.m_generatedByType == "Gaia.Stamper")
			{
				Stamper[] array = UnityEngine.Object.FindObjectsOfType<Stamper>();
				for (int i = 0; i < array.GetLength(0); i++)
				{
					if (array[i].m_stampID == operation.m_generatedByID && array[i].name == operation.m_generatedByName)
					{
						return array[i].gameObject;
					}
				}
				return ShowStamper(operation.m_generatedByName, operation.m_generatedByID);
			}
			if (operation.m_generatedByType == "Gaia.Spawner")
			{
				Spawner[] array2 = UnityEngine.Object.FindObjectsOfType<Spawner>();
				for (int j = 0; j < array2.GetLength(0); j++)
				{
					if (array2[j].m_spawnerID == operation.m_generatedByID && array2[j].name == operation.m_generatedByName)
					{
						return array2[j].gameObject;
					}
				}
				return CreateSpawner(operation.m_generatedByName, operation.m_generatedByID);
			}
			return null;
		}

		private GameObject ShowStamper(string name, string id)
		{
			GameObject gameObject = GameObject.Find("Gaia");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia");
			}
			GameObject gameObject2 = GameObject.Find(name);
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject(name);
				gameObject2.transform.parent = gameObject.transform;
				Stamper stamper = gameObject2.AddComponent<Stamper>();
				stamper.m_stampID = id;
				stamper.HidePreview();
				stamper.m_seaLevel = m_session.m_seaLevel;
			}
			return gameObject2;
		}

		private GameObject CreateSpawner(string name, string id)
		{
			GameObject gameObject = GameObject.Find("Gaia");
			if (gameObject == null)
			{
				gameObject = new GameObject("Gaia");
			}
			GameObject gameObject2 = new GameObject(name);
			gameObject2.transform.parent = gameObject.transform;
			Spawner spawner = gameObject2.AddComponent<Spawner>();
			spawner.m_spawnerID = id;
			return gameObject2;
		}
	}
}
