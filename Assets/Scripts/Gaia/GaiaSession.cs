using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class GaiaSession : ScriptableObject, ISerializationCallbackReceiver
	{
		[TextArea(1, 1)]
		public string m_name = $"Session {DateTime.Now:yyyyMMdd-HHmmss}";

		[TextArea(3, 5)]
		public string m_description = string.Empty;

		[HideInInspector]
		public Texture2D m_previewImage;

		public string m_dateCreated = DateTime.Now.ToString();

		public int m_terrainWidth;

		public int m_terrainDepth;

		public int m_terrainHeight;

		public float m_seaLevel = 30f;

		public bool m_isLocked;

		[HideInInspector]
		public ScriptableObjectWrapper m_defaults;

		[HideInInspector]
		public Dictionary<string, ScriptableObjectWrapper> m_resources = new Dictionary<string, ScriptableObjectWrapper>();

		[HideInInspector]
		public List<string> m_resourcesKeys = new List<string>();

		[HideInInspector]
		public List<ScriptableObjectWrapper> m_resourcesValues = new List<ScriptableObjectWrapper>();

		[HideInInspector]
		public byte[] m_previewImageBytes = new byte[0];

		[HideInInspector]
		public int m_previewImageWidth;

		[HideInInspector]
		public int m_previewImageHeight;

		[HideInInspector]
		public List<GaiaOperation> m_operations = new List<GaiaOperation>();

		public void OnBeforeSerialize()
		{
			m_resourcesKeys.Clear();
			m_resourcesValues.Clear();
			foreach (KeyValuePair<string, ScriptableObjectWrapper> resource in m_resources)
			{
				m_resourcesKeys.Add(resource.Key);
				m_resourcesValues.Add(resource.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			m_resources = new Dictionary<string, ScriptableObjectWrapper>();
			for (int i = 0; i != Math.Min(m_resourcesKeys.Count, m_resourcesValues.Count); i++)
			{
				m_resources.Add(m_resourcesKeys[i], m_resourcesValues[i]);
			}
		}

		public string GetSessionFileName()
		{
			return Utils.FixFileName(m_name);
		}

		public Texture2D GetPreviewImage()
		{
			if (m_previewImageBytes.GetLength(0) == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(m_previewImageWidth, m_previewImageHeight, TextureFormat.ARGB32, mipChain: false);
			texture2D.LoadRawTextureData(m_previewImageBytes);
			texture2D.Apply();
			texture2D.name = m_name;
			return texture2D;
		}
	}
}
