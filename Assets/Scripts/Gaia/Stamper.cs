using Gaia.FullSerializer;
using System;
using System.Collections;
using UnityEngine;

namespace Gaia
{
	[ExecuteInEditMode]
	public class Stamper : MonoBehaviour
	{
		public string m_stampID = Guid.NewGuid().ToString();

		public Texture2D m_stampPreviewImage;

		public float m_x;

		public float m_y = 50f;

		public float m_z;

		public float m_width = 10f;

		public float m_height = 10f;

		public float m_rotation;

		public bool m_stickBaseToGround = true;

		[fsIgnore]
		public GaiaResource m_resources;

		[fsIgnore]
		public float m_seaLevel;

		public string m_resourcesPath;

		public bool m_invertStamp;

		public bool m_normaliseStamp;

		public float m_baseLevel;

		public bool m_drawStampBase = true;

		public GaiaConstants.FeatureOperation m_stampOperation;

		public int m_smoothIterations;

		public float m_blendStrength = 0.5f;

		public float m_stencilHeight = 1f;

		public AnimationCurve m_heightModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public AnimationCurve m_distanceMask = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		public GaiaConstants.ImageFitnessFilterMode m_areaMaskMode;

		public Texture2D m_imageMask;

		public bool m_imageMaskInvert;

		public bool m_imageMaskNormalise;

		public bool m_imageMaskFlip;

		public int m_imageMaskSmoothIterations = 3;

		[fsIgnore]
		public HeightMap m_imageMaskHM;

		public float m_noiseMaskSeed;

		public int m_noiseMaskOctaves = 8;

		public float m_noiseMaskPersistence = 0.25f;

		public float m_noiseMaskFrequency = 1f;

		public float m_noiseMaskLacunarity = 1.5f;

		public float m_noiseZoom = 10f;

		public bool m_alwaysShow;

		public bool m_showBase = true;

		public bool m_showSeaLevel = true;

		public bool m_showRulers;

		public bool m_showTerrainHelper;

		[fsIgnore]
		public Color m_gizmoColour = new Color(1f, 0.6f, 0f, 1f);

		[fsIgnore]
		public IEnumerator m_updateCoroutine;

		[fsIgnore]
		public float m_updateTimeAllowed = 71f / (678f * (float)Math.PI);

		[fsIgnore]
		public float m_stampProgress;

		[fsIgnore]
		public bool m_stampComplete = true;

		[fsIgnore]
		public bool m_cancelStamp;

		[fsIgnore]
		public Material m_previewMaterial;

		private int m_featureID;

		private int m_scanWidth;

		private int m_scanDepth;

		private int m_scanHeight;

		private float m_scanResolution = 0.1f;

		private Bounds m_scanBounds;

		private UnityHeightMap m_stampHM;

		private GaiaWorldManager m_undoMgr;

		private GaiaWorldManager m_redoMgr;

		private MeshFilter m_previewFilter;

		private MeshRenderer m_previewRenderer;

		public void LoadStamp()
		{
			m_featureID = -1;
			m_scanBounds = new Bounds(base.transform.position, Vector3.one * 10f);
			if (m_stampPreviewImage == null)
			{
				UnityEngine.Debug.LogWarning("Can't load feature - texture not set");
				return;
			}
			m_featureID = m_stampPreviewImage.GetInstanceID();
			if (!Utils.CheckValidGaiaStampPath(m_stampPreviewImage))
			{
				UnityEngine.Debug.LogError("The file provided is not a valid stamp. You need to drag the stamp preview from one of the directories underneath your Gaia Stamps directory.");
				m_featureID = -1;
				m_stampPreviewImage = null;
				return;
			}
			string gaiaStampPath = Utils.GetGaiaStampPath(m_stampPreviewImage);
			m_stampHM = new UnityHeightMap(gaiaStampPath);
			if (!m_stampHM.HasData())
			{
				m_featureID = -1;
				m_stampPreviewImage = null;
				UnityEngine.Debug.LogError("Was unable to load " + gaiaStampPath);
				return;
			}
			float[] array = new float[5];
			Buffer.BlockCopy(m_stampHM.GetMetaData(), 0, array, 0, array.Length * 4);
			m_scanWidth = (int)array[0];
			m_scanDepth = (int)array[1];
			m_scanHeight = (int)array[2];
			m_scanResolution = array[3];
			m_baseLevel = array[4];
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution * m_width, (float)m_scanHeight * m_scanResolution * m_height, (float)m_scanDepth * m_scanResolution * m_width));
			if (m_invertStamp)
			{
				m_stampHM.Invert();
			}
			if (m_normaliseStamp)
			{
				m_stampHM.Normalise();
			}
			GeneratePreviewMesh();
		}

		public void LoadStamp(string imagePreviewPath)
		{
			LoadStamp();
		}

		public bool LoadRuntimeStamp(TextAsset stamp)
		{
			m_stampHM = new UnityHeightMap(stamp);
			if (!m_stampHM.HasData())
			{
				m_featureID = -1;
				m_stampPreviewImage = null;
				UnityEngine.Debug.LogError("Was unable to load textasset stamp");
				return false;
			}
			float[] array = new float[5];
			Buffer.BlockCopy(m_stampHM.GetMetaData(), 0, array, 0, array.Length * 4);
			m_scanWidth = (int)array[0];
			m_scanDepth = (int)array[1];
			m_scanHeight = (int)array[2];
			m_scanResolution = array[3];
			m_baseLevel = array[4];
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution * m_width, (float)m_scanHeight * m_scanResolution * m_height, (float)m_scanDepth * m_scanResolution * m_width));
			if (m_invertStamp)
			{
				m_stampHM.Invert();
			}
			if (m_normaliseStamp)
			{
				m_stampHM.Normalise();
			}
			return true;
		}

		public void InvertStamp()
		{
			m_stampHM.Invert();
			GeneratePreviewMesh();
		}

		public void NormaliseStamp()
		{
			m_stampHM.Normalise();
			GeneratePreviewMesh();
		}

		public void Stamp()
		{
			m_cancelStamp = false;
			m_stampComplete = false;
			m_stampProgress = 0f;
			AddToSession(GaiaOperation.OperationType.Stamp, "Stamping " + m_stampPreviewImage.name);
			StartCoroutine(ApplyStamp());
		}

		public void CancelStamp()
		{
			m_cancelStamp = true;
		}

		public bool IsStamping()
		{
			return !m_stampComplete;
		}

		public void UpdateStamp()
		{
			if (m_stickBaseToGround)
			{
				AlignToGround();
			}
			base.transform.position = new Vector3(m_x, m_y, m_z);
			base.transform.localScale = new Vector3(m_width, m_height, m_width);
			base.transform.localRotation = Quaternion.AngleAxis(m_rotation, Vector3.up);
			m_scanBounds.center = base.transform.position;
			m_scanBounds.size = new Vector3((float)m_scanWidth * m_scanResolution * m_width, (float)m_scanHeight * m_scanResolution * m_height, (float)m_scanDepth * m_scanResolution * m_width);
			if (m_stampHM != null)
			{
				m_stampHM.SetBoundsWU(m_scanBounds);
			}
			base.transform.hasChanged = false;
		}

		public void AlignToGround()
		{
			if (m_stampHM != null && m_stampHM.HasData())
			{
				float num = 0f;
				Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
				if (terrain == null)
				{
					terrain = Terrain.activeTerrain;
				}
				if (terrain != null)
				{
					Vector3 position = terrain.transform.position;
					num = position.y;
				}
				m_scanBounds.center = base.transform.position;
				m_scanBounds.size = new Vector3((float)m_scanWidth * m_scanResolution * m_width, (float)m_scanHeight * m_scanResolution * m_height, (float)m_scanDepth * m_scanResolution * m_width);
				if (terrain == null)
				{
					float num2 = num;
					Vector3 extents = m_scanBounds.extents;
					m_y = num2 + extents.y;
					return;
				}
				Vector3 min = m_scanBounds.min;
				float y = min.y;
				Vector3 size = m_scanBounds.size;
				float num3 = y + size.y * m_baseLevel;
				Vector3 center = m_scanBounds.center;
				m_y = center.y - (num3 - num);
			}
		}

		public bool GetHeightRange(ref float baseLevel, ref float minHeight, ref float maxHeight)
		{
			if (m_stampHM == null || !m_stampHM.HasData())
			{
				return false;
			}
			baseLevel = m_baseLevel;
			m_stampHM.GetHeightRange(ref minHeight, ref maxHeight);
			return true;
		}

		public void FitToTerrain()
		{
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain == null)
			{
				terrain = TerrainHelper.GetActiveTerrain();
			}
			if (terrain == null)
			{
				return;
			}
			Bounds bounds = default(Bounds);
			if (TerrainHelper.GetTerrainBounds(terrain, ref bounds))
			{
				Vector3 size = bounds.size;
				m_height = size.y / 100f * 2f;
				if (m_stampHM != null && m_stampHM.HasData())
				{
					Vector3 size2 = bounds.size;
					m_width = size2.x / (float)m_stampHM.Width() * 10f;
				}
				else
				{
					m_width = m_height;
				}
				m_height *= 0.25f;
				Vector3 center = bounds.center;
				m_x = center.x;
				Vector3 center2 = bounds.center;
				m_y = center2.y;
				Vector3 center3 = bounds.center;
				m_z = center3.z;
				m_rotation = 0f;
			}
			if (m_stickBaseToGround)
			{
				AlignToGround();
			}
		}

		public bool IsFitToTerrain()
		{
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain == null)
			{
				terrain = Terrain.activeTerrain;
			}
			if (terrain == null || m_stampHM == null || !m_stampHM.HasData())
			{
				UnityEngine.Debug.LogError("Could not check if fit to terrain - no terrain present");
				return false;
			}
			Bounds bounds = default(Bounds);
			if (TerrainHelper.GetTerrainBounds(terrain, ref bounds))
			{
				Vector3 size = bounds.size;
				float num = size.x / (float)m_stampHM.Width() * 10f;
				Vector3 center = bounds.center;
				float x = center.x;
				Vector3 center2 = bounds.center;
				float z = center2.z;
				float num2 = 0f;
				if (num != m_width || x != m_x || z != m_z || num2 != m_rotation)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public void AddToSession(GaiaOperation.OperationType opType, string opName)
		{
			GaiaSessionManager sessionManager = GaiaSessionManager.GetSessionManager();
			if (sessionManager != null && !sessionManager.IsLocked())
			{
				GaiaOperation gaiaOperation = new GaiaOperation();
				gaiaOperation.m_description = opName;
				gaiaOperation.m_generatedByID = m_stampID;
				gaiaOperation.m_generatedByName = base.transform.name;
				gaiaOperation.m_generatedByType = GetType().ToString();
				gaiaOperation.m_isActive = true;
				gaiaOperation.m_operationDateTime = DateTime.Now.ToString();
				gaiaOperation.m_operationType = opType;
				if (opType == GaiaOperation.OperationType.Stamp)
				{
					gaiaOperation.m_operationDataJson = new string[1];
					gaiaOperation.m_operationDataJson[0] = SerialiseJson();
				}
				else
				{
					gaiaOperation.m_operationDataJson = new string[0];
				}
				sessionManager.AddOperation(gaiaOperation);
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
			Stamper instance = this;
			fsSerializer.TryDeserialize(data, ref instance);
			instance.LoadStamp();
			instance.UpdateStamp();
		}

		public void FlattenTerrain()
		{
			AddToSession(GaiaOperation.OperationType.FlattenTerrain, "Flattening terrain");
			m_undoMgr = new GaiaWorldManager(Terrain.activeTerrains);
			m_undoMgr.LoadFromWorld();
			m_redoMgr = new GaiaWorldManager(Terrain.activeTerrains);
			m_redoMgr.FlattenWorld();
			m_redoMgr = null;
		}

		public void SmoothTerrain()
		{
			AddToSession(GaiaOperation.OperationType.SmoothTerrain, "Smoothing terrain");
			m_undoMgr = new GaiaWorldManager(Terrain.activeTerrains);
			m_undoMgr.LoadFromWorld();
			m_redoMgr = new GaiaWorldManager(Terrain.activeTerrains);
			m_redoMgr.SmoothWorld();
			m_redoMgr = null;
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

		public bool CanPreview()
		{
			return m_previewRenderer != null;
		}

		public bool CurrentPreviewState()
		{
			if (m_previewRenderer != null)
			{
				return m_previewRenderer.enabled;
			}
			return false;
		}

		public void ShowPreview()
		{
			if (m_previewRenderer != null)
			{
				m_previewRenderer.enabled = true;
			}
		}

		public void HidePreview()
		{
			if (m_previewRenderer != null)
			{
				m_previewRenderer.enabled = false;
			}
		}

		public void TogglePreview()
		{
			if (m_previewRenderer != null)
			{
				m_previewRenderer.enabled = !m_previewRenderer.enabled;
			}
		}

		public bool CanUndo()
		{
			if (m_undoMgr == null)
			{
				return false;
			}
			return true;
		}

		public void CreateUndo()
		{
			m_undoMgr = new GaiaWorldManager(Terrain.activeTerrains);
			m_undoMgr.LoadFromWorld();
			m_redoMgr = null;
		}

		public void Undo()
		{
			if (m_undoMgr != null)
			{
				AddToSession(GaiaOperation.OperationType.StampUndo, "Undoing stamp");
				m_redoMgr = new GaiaWorldManager(Terrain.activeTerrains);
				m_redoMgr.LoadFromWorld();
				m_undoMgr.SaveToWorld(forceWrite: true);
			}
		}

		public bool CanRedo()
		{
			if (m_redoMgr == null)
			{
				return false;
			}
			return true;
		}

		public void Redo()
		{
			if (m_redoMgr != null)
			{
				AddToSession(GaiaOperation.OperationType.StampRedo, "Redoing stamp");
				m_redoMgr.SaveToWorld(forceWrite: true);
				m_redoMgr = null;
			}
		}

		private void OnEnable()
		{
			if (m_stampPreviewImage != null)
			{
				LoadStamp();
			}
			if (Application.isPlaying)
			{
				HidePreview();
			}
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

		private void OnDrawGizmosSelected()
		{
			DrawGizmos(isSelected: true);
		}

		private void OnDrawGizmos()
		{
			DrawGizmos(isSelected: false);
		}

		private void DrawGizmos(bool isSelected)
		{
			if (m_stampPreviewImage == null)
			{
				return;
			}
			if (base.transform.hasChanged)
			{
				Vector3 position = base.transform.position;
				m_x = position.x;
				Vector3 position2 = base.transform.position;
				m_y = position2.y;
				Vector3 position3 = base.transform.position;
				m_z = position3.z;
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				m_rotation = localEulerAngles.y;
				Vector3 localScale = base.transform.localScale;
				if (localScale.x == m_width)
				{
					Vector3 localScale2 = base.transform.localScale;
					if (localScale2.z == m_width)
					{
						goto IL_0186;
					}
				}
				Vector3 localScale3 = base.transform.localScale;
				float num = Mathf.Abs(localScale3.x - m_width);
				Vector3 localScale4 = base.transform.localScale;
				float num2 = Mathf.Abs(localScale4.z - m_width);
				if (num > num2)
				{
					Vector3 localScale5 = base.transform.localScale;
					if (localScale5.x > 0f)
					{
						Vector3 localScale6 = base.transform.localScale;
						m_width = localScale6.x;
					}
				}
				else
				{
					Vector3 localScale7 = base.transform.localScale;
					if (localScale7.z > 0f)
					{
						Vector3 localScale8 = base.transform.localScale;
						m_width = localScale8.z;
					}
				}
				goto IL_0186;
			}
			goto IL_01e3;
			IL_0186:
			Vector3 localScale9 = base.transform.localScale;
			if (localScale9.y != m_height)
			{
				Vector3 localScale10 = base.transform.localScale;
				if (localScale10.y > 0f)
				{
					Vector3 localScale11 = base.transform.localScale;
					m_height = localScale11.y;
				}
			}
			UpdateStamp();
			goto IL_01e3;
			IL_01e3:
			if (!isSelected && !m_alwaysShow)
			{
				return;
			}
			if (m_showBase)
			{
				Bounds bounds = default(Bounds);
				if (TerrainHelper.GetTerrainBounds(base.transform.position, ref bounds))
				{
					Vector3 center = bounds.center;
					float x = center.x;
					Vector3 min = m_scanBounds.min;
					float y = min.y;
					Vector3 size = m_scanBounds.size;
					float y2 = y + size.y * m_baseLevel;
					Vector3 center2 = bounds.center;
					bounds.center = new Vector3(x, y2, center2.z);
					Vector3 size2 = bounds.size;
					float x2 = size2.x;
					Vector3 size3 = bounds.size;
					bounds.size = new Vector3(x2, 0.05f, size3.z);
					Color yellow = Color.yellow;
					float r = yellow.r;
					Color yellow2 = Color.yellow;
					float g = yellow2.g;
					Color yellow3 = Color.yellow;
					float b = yellow3.b;
					Color yellow4 = Color.yellow;
					Gizmos.color = new Color(r, g, b, yellow4.a / 2f);
					Gizmos.DrawCube(bounds.center, bounds.size);
				}
			}
			if (m_resources != null)
			{
				m_seaLevel = m_resources.m_seaLevel;
			}
			if (m_showSeaLevel)
			{
				Bounds bounds2 = default(Bounds);
				if (TerrainHelper.GetTerrainBounds(base.transform.position, ref bounds2))
				{
					Vector3 center3 = bounds2.center;
					float x3 = center3.x;
					float seaLevel = m_seaLevel;
					Vector3 center4 = bounds2.center;
					bounds2.center = new Vector3(x3, seaLevel, center4.z);
					Vector3 size4 = bounds2.size;
					float x4 = size4.x;
					Vector3 size5 = bounds2.size;
					bounds2.size = new Vector3(x4, 0.05f, size5.z);
					if (isSelected)
					{
						Color blue = Color.blue;
						float r2 = blue.r;
						Color blue2 = Color.blue;
						float g2 = blue2.g;
						Color blue3 = Color.blue;
						float b2 = blue3.b;
						Color blue4 = Color.blue;
						Gizmos.color = new Color(r2, g2, b2, blue4.a / 2f);
						Gizmos.DrawCube(bounds2.center, bounds2.size);
					}
					else
					{
						Color blue5 = Color.blue;
						float r3 = blue5.r;
						Color blue6 = Color.blue;
						float g3 = blue6.g;
						Color blue7 = Color.blue;
						float b3 = blue7.b;
						Color blue8 = Color.blue;
						Gizmos.color = new Color(r3, g3, b3, blue8.a / 4f);
						Gizmos.DrawCube(bounds2.center, bounds2.size);
					}
				}
			}
			if (m_showRulers)
			{
				DrawRulers();
			}
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Vector3 size6 = new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanHeight * m_scanResolution, (float)m_scanDepth * m_scanResolution);
			Gizmos.color = new Color(m_gizmoColour.r, m_gizmoColour.g, m_gizmoColour.b, m_gizmoColour.a / 2f);
			Gizmos.DrawWireCube(Vector3.zero, size6);
			Gizmos.matrix = matrix;
			Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
			if (terrain != null)
			{
				Gizmos.color = Color.white;
				Bounds bounds3 = default(Bounds);
				TerrainHelper.GetTerrainBounds(terrain, ref bounds3);
				Gizmos.DrawWireCube(bounds3.center, bounds3.size);
			}
		}

		private void DrawRulers()
		{
		}

		public IEnumerator ApplyStamp()
		{
			UpdateStamp();
			GaiaWorldManager mgr = new GaiaWorldManager(Terrain.activeTerrains);
			mgr.LoadFromWorld();
			if (mgr.TileCount == 0)
			{
				UnityEngine.Debug.LogError("Can not stamp without a terrain present!");
				m_stampProgress = 0f;
				m_stampComplete = true;
				m_updateCoroutine = null;
				yield break;
			}
			CreateUndo();
			if (m_areaMaskMode != 0 && !LoadImageMask())
			{
				m_stampProgress = 0f;
				m_stampComplete = true;
				m_updateCoroutine = null;
				yield break;
			}
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 rotation = new Vector3(0f, eulerAngles.y, 0f);
			Vector3 eulerAngles2 = base.transform.localRotation.eulerAngles;
			Vector3 negRotation = new Vector3(0f, eulerAngles2.y * -1f, 0f);
			Bounds origSmBoundsWU = m_stampHM.GetBoundsWU();
			Bounds newSmBoundsWU = new Bounds
			{
				center = origSmBoundsWU.center
			};
			Vector3 min = origSmBoundsWU.min;
			float x2 = min.x;
			Vector3 center = origSmBoundsWU.center;
			float y = center.y;
			Vector3 min2 = origSmBoundsWU.min;
			newSmBoundsWU.Encapsulate(RotatePointAroundPivot(new Vector3(x2, y, min2.z), origSmBoundsWU.center, rotation));
			Vector3 min3 = origSmBoundsWU.min;
			float x3 = min3.x;
			Vector3 center2 = origSmBoundsWU.center;
			float y2 = center2.y;
			Vector3 max = origSmBoundsWU.max;
			newSmBoundsWU.Encapsulate(RotatePointAroundPivot(new Vector3(x3, y2, max.z), origSmBoundsWU.center, rotation));
			Vector3 max2 = origSmBoundsWU.max;
			float x4 = max2.x;
			Vector3 center3 = origSmBoundsWU.center;
			float y3 = center3.y;
			Vector3 min4 = origSmBoundsWU.min;
			newSmBoundsWU.Encapsulate(RotatePointAroundPivot(new Vector3(x4, y3, min4.z), origSmBoundsWU.center, rotation));
			Vector3 max3 = origSmBoundsWU.max;
			float x5 = max3.x;
			Vector3 center4 = origSmBoundsWU.center;
			float y4 = center4.y;
			Vector3 max4 = origSmBoundsWU.max;
			newSmBoundsWU.Encapsulate(RotatePointAroundPivot(new Vector3(x5, y4, max4.z), origSmBoundsWU.center, rotation));
			Vector3 newSmSizeTU = mgr.Ceil(mgr.WUtoTU(newSmBoundsWU.size));
			Vector3 pivot = new Vector3(0.5f, 0f, 0.5f);
			int newSmMaxX = (int)newSmSizeTU.x;
			int newSmMaxZ = (int)newSmSizeTU.z;
			float newSmXtoNU = 1f / newSmSizeTU.x;
			float newSmZtoNU = 1f / newSmSizeTU.z;
			Vector3 size = newSmBoundsWU.size;
			float x6 = size.x;
			Vector3 size2 = origSmBoundsWU.size;
			float xNewSMtoOrigSMScale = x6 / size2.x;
			Vector3 size3 = newSmBoundsWU.size;
			float x7 = size3.x;
			Vector3 size4 = origSmBoundsWU.size;
			float zNewSMtoOrigSMScale = x7 / size4.z;
			Vector3 size5 = origSmBoundsWU.size;
			float x8 = size5.x;
			Vector3 size6 = newSmBoundsWU.size;
			float num = x8 - size6.x;
			Vector3 size7 = origSmBoundsWU.size;
			float scaleOffsetX = 0.5f * (num / size7.x);
			Vector3 size8 = origSmBoundsWU.size;
			float z2 = size8.z;
			Vector3 size9 = newSmBoundsWU.size;
			float num2 = z2 - size9.x;
			Vector3 size10 = origSmBoundsWU.size;
			float scaleOffsetZ = 0.5f * (num2 / size10.z);
			float currentTime = Time.realtimeSinceStartup;
			float accumulatedTime = 0f;
			int currChecks = 0;
			int totalChecks = newSmMaxX * newSmMaxZ;
			Vector3 globalCentreTU = mgr.WUtoTU(base.transform.position);
			Vector3 globalOffsetTU = globalCentreTU - newSmSizeTU * 0.5f;
			Vector3 globalPositionTU = Vector3.one;
			Vector3 size11 = origSmBoundsWU.size;
			float y5 = size11.y;
			Vector3 size12 = mgr.WorldBoundsWU.size;
			float smToOrigHeightConversion = y5 / size12.y;
			Vector3 min5 = origSmBoundsWU.min;
			float y6 = min5.y;
			Vector3 min6 = mgr.WorldBoundsWU.min;
			float num3 = y6 - min6.y;
			Vector3 size13 = mgr.WorldBoundsWU.size;
			float smHeightOffset = num3 / size13.y;
			float stencilHeight = m_stencilHeight;
			Vector3 size14 = mgr.WorldBoundsWU.size;
			float stencilHeightNU = stencilHeight / size14.y;
			for (int x = 0; x < newSmMaxX; x++)
			{
				float newSmXNU = (float)x * newSmXtoNU;
				for (int z = 0; z < newSmMaxZ; z++)
				{
					float newSmZNU = (float)z * newSmZtoNU;
					int num4;
					currChecks = (num4 = currChecks) + 1;
					m_stampProgress = (float)num4 / (float)totalChecks;
					float newTime = Time.realtimeSinceStartup;
					float stepTime = newTime - currentTime;
					currentTime = newTime;
					accumulatedTime += stepTime;
					if (accumulatedTime > m_updateTimeAllowed)
					{
						accumulatedTime = 0f;
						yield return null;
					}
					if (m_cancelStamp)
					{
						break;
					}
					globalPositionTU.x = (float)z + globalOffsetTU.z;
					globalPositionTU.y = globalCentreTU.y;
					globalPositionTU.z = (float)x + globalOffsetTU.x;
					if (!mgr.InBoundsTU(globalPositionTU))
					{
						continue;
					}
					Vector3 position2 = new Vector3(newSmXNU, 0f, newSmZNU);
					position2 = RotatePointAroundPivot(position2, pivot, negRotation);
					float origSmXNU = position2.x * xNewSMtoOrigSMScale + scaleOffsetX;
					float origSmZNU = position2.z * zNewSMtoOrigSMScale + scaleOffsetZ;
					if (!(origSmXNU < 0f) && !(origSmXNU > 1f) && !(origSmZNU < 0f) && !(origSmZNU > 1f))
					{
						float distance = Utils.Math_Distance(origSmXNU, origSmZNU, pivot.x, pivot.z) * 2f;
						float strength = m_distanceMask.Evaluate(distance);
						if (m_areaMaskMode != 0 && m_imageMaskHM != null)
						{
							strength *= m_imageMaskHM[origSmXNU, origSmZNU];
						}
						float smHeightRaw = m_heightModifier.Evaluate(m_stampHM[origSmXNU, origSmZNU]);
						float smHeightAdj = (m_stampOperation == GaiaConstants.FeatureOperation.StencilHeight) ? smHeightRaw : (smHeightOffset + smHeightRaw * smToOrigHeightConversion);
						float terrainHeight = mgr.GetHeightTU(globalPositionTU);
						mgr.SetHeightTU(height: Mathf.Clamp01(CalculateHeight(terrainHeight, smHeightRaw, smHeightAdj, stencilHeightNU, strength)), positionTU: globalPositionTU);
					}
				}
			}
			if (!m_cancelStamp)
			{
				mgr.SaveToWorld();
			}
			else
			{
				m_undoMgr = null;
				m_redoMgr = null;
			}
			m_stampProgress = 0f;
			m_stampComplete = true;
			m_updateCoroutine = null;
		}

		private void GeneratePreviewMesh()
		{
			if (m_previewMaterial == null)
			{
				m_previewMaterial = new Material(Shader.Find("Diffuse"));
				m_previewMaterial.color = Color.white;
				if (Terrain.activeTerrain != null && Terrain.activeTerrain.terrainData.splatPrototypes.Length > 0)
				{
					Texture2D texture2D = (Terrain.activeTerrain.terrainData.splatPrototypes.Length != 4) ? Terrain.activeTerrain.terrainData.splatPrototypes[0].texture : Terrain.activeTerrain.terrainData.splatPrototypes[3].texture;
					Utils.MakeTextureReadable(texture2D);
					Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, TextureFormat.ARGB32, mipChain: true);
					texture2D2.SetPixels32(texture2D.GetPixels32());
					texture2D2.wrapMode = TextureWrapMode.Repeat;
					texture2D2.Apply();
					m_previewMaterial.mainTexture = texture2D2;
					m_previewMaterial.mainTextureScale = new Vector2(30f, 30f);
				}
				m_previewMaterial.hideFlags = HideFlags.HideInInspector;
				m_previewMaterial.name = "StamperMaterial";
			}
			m_previewFilter = GetComponent<MeshFilter>();
			if (m_previewFilter == null)
			{
				base.gameObject.AddComponent<MeshFilter>();
				m_previewFilter = GetComponent<MeshFilter>();
				m_previewFilter.hideFlags = HideFlags.HideInInspector;
			}
			m_previewRenderer = GetComponent<MeshRenderer>();
			if (m_previewRenderer == null)
			{
				base.gameObject.AddComponent<MeshRenderer>();
				m_previewRenderer = GetComponent<MeshRenderer>();
				m_previewRenderer.hideFlags = HideFlags.HideInInspector;
			}
			m_previewRenderer.sharedMaterial = m_previewMaterial;
			Vector3 targetSize = new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanHeight * m_scanResolution, (float)m_scanDepth * m_scanResolution);
			m_previewFilter.mesh = Utils.CreateMesh(m_stampHM.Heights(), targetSize);
		}

		private bool LoadImageMask()
		{
			m_imageMaskHM = null;
			if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.None)
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
			else if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.PerlinNoise || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.RidgedNoise || m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.BillowNoise)
			{
				int num = 2048;
				int num2 = 2048;
				Terrain terrain = TerrainHelper.GetTerrain(base.transform.position);
				if (terrain == null)
				{
					terrain = Terrain.activeTerrain;
				}
				if (terrain != null)
				{
					num = terrain.terrainData.heightmapResolution;
					num2 = terrain.terrainData.heightmapResolution;
				}
				m_imageMaskHM = new HeightMap(num, num2);
				FractalGenerator fractalGenerator = new FractalGenerator();
				fractalGenerator.Seed = m_noiseMaskSeed;
				fractalGenerator.Octaves = m_noiseMaskOctaves;
				fractalGenerator.Persistence = m_noiseMaskPersistence;
				fractalGenerator.Frequency = m_noiseMaskFrequency;
				fractalGenerator.Lacunarity = m_noiseMaskLacunarity;
				if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.PerlinNoise)
				{
					fractalGenerator.FractalType = FractalGenerator.Fractals.Perlin;
				}
				else if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.RidgedNoise)
				{
					fractalGenerator.FractalType = FractalGenerator.Fractals.RidgeMulti;
				}
				else if (m_areaMaskMode == GaiaConstants.ImageFitnessFilterMode.BillowNoise)
				{
					fractalGenerator.FractalType = FractalGenerator.Fractals.Billow;
				}
				float num3 = 1f / m_noiseZoom;
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < num2; l++)
					{
						m_imageMaskHM[k, l] = fractalGenerator.GetValue((float)k * num3, (float)l * num3);
					}
				}
			}
			else
			{
				Terrain terrain2 = TerrainHelper.GetTerrain(base.transform.position);
				if (terrain2 == null)
				{
					terrain2 = Terrain.activeTerrain;
				}
				if (terrain2 == null)
				{
					UnityEngine.Debug.LogError("You requested an terrain texture mask but there is no terrain.");
					return false;
				}
				switch (m_areaMaskMode)
				{
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture0:
					if (terrain2.terrainData.splatPrototypes.Length < 1)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 0 but there is no active texture in slot 0.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 0);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture1:
					if (terrain2.terrainData.splatPrototypes.Length < 2)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 1 but there is no active texture in slot 1.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 1);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture2:
					if (terrain2.terrainData.splatPrototypes.Length < 3)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 2 but there is no active texture in slot 2.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 2);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture3:
					if (terrain2.terrainData.splatPrototypes.Length < 4)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 3 but there is no active texture in slot 3.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 3);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture4:
					if (terrain2.terrainData.splatPrototypes.Length < 5)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 4 but there is no active texture in slot 4.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 4);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture5:
					if (terrain2.terrainData.splatPrototypes.Length < 6)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 5 but there is no active texture in slot 5.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 5);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture6:
					if (terrain2.terrainData.splatPrototypes.Length < 7)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 6 but there is no active texture in slot 6.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 6);
					break;
				case GaiaConstants.ImageFitnessFilterMode.TerrainTexture7:
					if (terrain2.terrainData.splatPrototypes.Length < 8)
					{
						UnityEngine.Debug.LogError("You requested an terrain texture mask 7 but there is no active texture in slot 7.");
						return false;
					}
					m_imageMaskHM = new HeightMap(terrain2.terrainData.GetAlphamaps(0, 0, terrain2.terrainData.alphamapWidth, terrain2.terrainData.alphamapHeight), 7);
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

		private float CalculateHeight(float terrainHeight, float smHeightRaw, float smHeightAdj, float stencilHeightNU, float strength)
		{
			float num = 0f;
			float num2 = 0f;
			if (!m_drawStampBase && smHeightRaw < m_baseLevel)
			{
				return terrainHeight;
			}
			switch (m_stampOperation)
			{
			case GaiaConstants.FeatureOperation.RaiseHeight:
				if (smHeightAdj > terrainHeight)
				{
					num2 = (smHeightAdj - terrainHeight) * strength;
					terrainHeight += num2;
				}
				break;
			case GaiaConstants.FeatureOperation.BlendHeight:
				num = m_blendStrength * smHeightAdj + (1f - m_blendStrength) * terrainHeight;
				num2 = (num - terrainHeight) * strength;
				terrainHeight += num2;
				break;
			case GaiaConstants.FeatureOperation.DifferenceHeight:
				num = Mathf.Abs(smHeightAdj - terrainHeight);
				num2 = (num - terrainHeight) * strength;
				terrainHeight += num2;
				break;
			case GaiaConstants.FeatureOperation.StencilHeight:
				num = terrainHeight + smHeightAdj * stencilHeightNU;
				num2 = (num - terrainHeight) * strength;
				terrainHeight += num2;
				break;
			case GaiaConstants.FeatureOperation.LowerHeight:
				if (smHeightAdj < terrainHeight)
				{
					num2 = (terrainHeight - smHeightAdj) * strength;
					terrainHeight -= num2;
				}
				break;
			}
			return terrainHeight;
		}

		private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 point2 = point - pivot;
			point2 = Quaternion.Euler(angle) * point2;
			point = point2 + pivot;
			return point;
		}
	}
}
