using System;
using System.IO;
using UnityEngine;

namespace Gaia
{
	public class Scanner : MonoBehaviour
	{
		[Tooltip("The name of the stamp as it will be stored in the project. Initally based on the file name.")]
		public string m_featureName = $"{DateTime.Now}";

		[Tooltip("The type of stamp, also controls which directory the stamp will be loaded into.")]
		public GaiaConstants.FeatureType m_featureType = GaiaConstants.FeatureType.Mountains;

		[Range(0f, 1f)]
		[Tooltip("Base level for this stamp, used by stamper to cut off all heights below the base. It is the highest point of the stamp around its edges.")]
		public float m_baseLevel;

		[HideInInspector]
		[Range(0.1f, 1f)]
		[Tooltip("Scan resolution in meters. Leave this at smaller values for higher quality scans.")]
		public float m_scanResolution = 0.1f;

		[Tooltip("The material that will be used to display the scan preview. This is just for visualisation and will not affect the scan.")]
		public Material m_previewMaterial;

		private HeightMap m_scanMap;

		private Bounds m_scanBounds;

		private int m_scanWidth = 1;

		private int m_scanDepth = 1;

		private int m_scanHeight = 500;

		private Vector3 m_groundOffset = Vector3.zero;

		private Vector3 m_groundSize = Vector3.zero;

		private void OnEnable()
		{
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			meshFilter.hideFlags = HideFlags.HideInInspector;
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			meshRenderer.hideFlags = HideFlags.HideInInspector;
		}

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
		}

		public void Reset()
		{
			m_featureName = string.Empty;
			m_scanMap = null;
			m_scanBounds = new Bounds(base.transform.position, Vector3.one * 10f);
			m_scanWidth = (m_scanDepth = (m_scanHeight = 0));
			m_scanResolution = 0.1f;
			m_baseLevel = 0f;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
		}

		public void LoadRawFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				UnityEngine.Debug.LogError("Must supply a valid path. Raw load Aborted!");
			}
			Reset();
			m_featureName = Path.GetFileNameWithoutExtension(path);
			m_scanMap = new HeightMap();
			m_scanMap.LoadFromRawFile(path);
			if (!m_scanMap.HasData())
			{
				UnityEngine.Debug.LogError("Unable to load raw file. Raw load aborted.");
				return;
			}
			m_scanWidth = m_scanMap.Width();
			m_scanDepth = m_scanMap.Depth();
			m_scanHeight = m_scanWidth / 2;
			m_scanResolution = 0.1f;
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanWidth * m_scanResolution * 0.4f, (float)m_scanDepth * m_scanResolution));
			m_baseLevel = m_scanMap.GetBaseLevel();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
				meshFilter.hideFlags = HideFlags.HideInInspector;
			}
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				meshRenderer.hideFlags = HideFlags.HideInInspector;
			}
			meshFilter.mesh = Utils.CreateMesh(m_scanMap.Heights(), m_scanBounds.size);
			if (m_previewMaterial != null)
			{
				m_previewMaterial.hideFlags = HideFlags.HideInInspector;
				meshRenderer.sharedMaterial = m_previewMaterial;
			}
		}

		public void LoadTextureFile(Texture2D texture)
		{
			if (texture == null)
			{
				UnityEngine.Debug.LogError("Must supply a valid texture! Texture load aborted.");
				return;
			}
			Reset();
			m_featureName = texture.name;
			m_scanMap = new UnityHeightMap(texture);
			if (!m_scanMap.HasData())
			{
				UnityEngine.Debug.LogError("Unable to load Texture file. Texture load aborted.");
				return;
			}
			m_scanWidth = m_scanMap.Width();
			m_scanDepth = m_scanMap.Depth();
			m_scanHeight = m_scanWidth / 2;
			m_scanResolution = 0.1f;
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanWidth * m_scanResolution * 0.4f, (float)m_scanDepth * m_scanResolution));
			m_baseLevel = m_scanMap.GetBaseLevel();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
				meshFilter.hideFlags = HideFlags.HideInInspector;
			}
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				meshRenderer.hideFlags = HideFlags.HideInInspector;
			}
			meshFilter.mesh = Utils.CreateMesh(m_scanMap.Heights(), m_scanBounds.size);
			if (m_previewMaterial != null)
			{
				m_previewMaterial.hideFlags = HideFlags.HideInInspector;
				meshRenderer.sharedMaterial = m_previewMaterial;
			}
		}

		public void LoadTerain(Terrain terrain)
		{
			if (terrain == null)
			{
				UnityEngine.Debug.LogError("Must supply a valid terrain! Terrain load aborted.");
				return;
			}
			Reset();
			m_featureName = terrain.name;
			m_scanMap = new UnityHeightMap(terrain);
			if (!m_scanMap.HasData())
			{
				UnityEngine.Debug.LogError("Unable to load terrain file. Terrain load aborted.");
				return;
			}
			m_scanMap.Flip();
			m_scanWidth = m_scanMap.Width();
			m_scanDepth = m_scanMap.Depth();
			Vector3 size = terrain.terrainData.size;
			m_scanHeight = (int)size.y;
			m_scanResolution = 0.1f;
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanWidth * m_scanResolution * 0.4f, (float)m_scanDepth * m_scanResolution));
			m_baseLevel = m_scanMap.GetBaseLevel();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
				meshFilter.hideFlags = HideFlags.HideInInspector;
			}
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				meshRenderer.hideFlags = HideFlags.HideInInspector;
			}
			meshFilter.mesh = Utils.CreateMesh(m_scanMap.Heights(), m_scanBounds.size);
			if (m_previewMaterial != null)
			{
				m_previewMaterial.hideFlags = HideFlags.HideInInspector;
				meshRenderer.sharedMaterial = m_previewMaterial;
			}
		}

		public void LoadGameObject(GameObject go)
		{
			if (go == null)
			{
				UnityEngine.Debug.LogError("Must supply a valid gamem object! GameObject load aborted.");
				return;
			}
			Reset();
			m_featureName = go.name;
			GameObject gameObject = UnityEngine.Object.Instantiate(go);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			Collider[] componentsInChildren = gameObject.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider obj in array)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			Transform[] componentsInChildren2 = gameObject.GetComponentsInChildren<Transform>();
			Transform[] array2 = componentsInChildren2;
			foreach (Transform transform in array2)
			{
				if (transform.gameObject.activeSelf)
				{
					transform.gameObject.AddComponent<MeshCollider>();
				}
			}
			m_scanBounds.center = gameObject.transform.position;
			m_scanBounds.size = Vector3.zero;
			MeshCollider[] componentsInChildren3 = gameObject.GetComponentsInChildren<MeshCollider>();
			foreach (MeshCollider meshCollider in componentsInChildren3)
			{
				m_scanBounds.Encapsulate(meshCollider.bounds);
			}
			Vector3 size = m_scanBounds.size;
			m_scanWidth = (int)Mathf.Ceil(size.x * (1f / m_scanResolution));
			Vector3 size2 = m_scanBounds.size;
			m_scanHeight = (int)Mathf.Ceil(size2.y * (1f / m_scanResolution));
			Vector3 size3 = m_scanBounds.size;
			m_scanDepth = (int)Mathf.Ceil(size3.z * (1f / m_scanResolution));
			m_scanMap = new HeightMap(m_scanWidth, m_scanDepth);
			Vector3 min = m_scanBounds.min;
			Vector3 vector = min;
			Vector3 max = m_scanBounds.max;
			vector.y = max.y;
			RaycastHit hitInfo = default(RaycastHit);
			for (int l = 0; l < m_scanWidth; l++)
			{
				vector.x = min.x + m_scanResolution * (float)l;
				for (int m = 0; m < m_scanDepth; m++)
				{
					vector.z = min.z + m_scanResolution * (float)m;
					Vector3 origin = vector;
					Vector3 down = Vector3.down;
					Vector3 size4 = m_scanBounds.size;
					if (Physics.Raycast(origin, down, out hitInfo, size4.y))
					{
						HeightMap scanMap = m_scanMap;
						int x = l;
						int z = m;
						float distance = hitInfo.distance;
						Vector3 size5 = m_scanBounds.size;
						scanMap[x, z] = 1f - distance / size5.y;
					}
				}
			}
			UnityEngine.Object.DestroyImmediate(gameObject);
			if (!m_scanMap.HasData())
			{
				UnityEngine.Debug.LogError("Unable to scan GameObject. GameObject load aborted.");
				return;
			}
			m_scanBounds = new Bounds(base.transform.position, new Vector3((float)m_scanWidth * m_scanResolution, (float)m_scanWidth * m_scanResolution * 0.4f, (float)m_scanDepth * m_scanResolution));
			m_baseLevel = m_scanMap.GetBaseLevel();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
				meshFilter.hideFlags = HideFlags.HideInInspector;
			}
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
				meshRenderer.hideFlags = HideFlags.HideInInspector;
			}
			meshFilter.mesh = Utils.CreateMesh(m_scanMap.Heights(), m_scanBounds.size);
			if (m_previewMaterial != null)
			{
				m_previewMaterial.hideFlags = HideFlags.HideInInspector;
				meshRenderer.sharedMaterial = m_previewMaterial;
			}
		}

		public string SaveScan()
		{
			if (m_scanMap == null || !m_scanMap.HasData())
			{
				UnityEngine.Debug.LogWarning("Cant save scan as none has been loaded");
				return null;
			}
			string gaiaAssetPath = Utils.GetGaiaAssetPath(m_featureType, m_featureName);
			Utils.CompressToSingleChannelFileImage(m_scanMap.Heights(), gaiaAssetPath, TextureFormat.RGBA32, exportPNG: false);
			gaiaAssetPath = Utils.GetGaiaStampAssetPath(m_featureType, m_featureName);
			gaiaAssetPath += ".bytes";
			float[] array = new float[5]
			{
				m_scanWidth,
				m_scanDepth,
				m_scanHeight,
				m_scanResolution,
				m_baseLevel
			};
			byte[] array2 = new byte[array.Length * 4];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			m_scanMap.SetMetaData(array2);
			m_scanMap.SaveToBinaryFile(gaiaAssetPath);
			return gaiaAssetPath;
		}

		private void UpdateScanner()
		{
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			m_scanBounds.center = base.transform.position;
		}

		private void OnDrawGizmosSelected()
		{
			UpdateScanner();
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(m_scanBounds.center, m_scanBounds.size);
			m_groundOffset = m_scanBounds.center;
			ref Vector3 groundOffset = ref m_groundOffset;
			Vector3 min = m_scanBounds.min;
			float y = min.y;
			Vector3 max = m_scanBounds.max;
			float y2 = max.y;
			Vector3 min2 = m_scanBounds.min;
			groundOffset.y = y + (y2 - min2.y) * m_baseLevel;
			m_groundSize = m_scanBounds.size;
			m_groundSize.y = 0.1f;
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(m_groundOffset, m_groundSize);
		}
	}
}
