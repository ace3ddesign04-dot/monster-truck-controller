using System;
using UnityEngine;

namespace Gaia
{
	public class RuntimeStamper : MonoBehaviour
	{
		public bool m_showGUI = true;

		public bool m_showDebug = true;

		public string m_stampAddress = "Gaia/Stamps/RuggedHills 1810 4";

		public string m_currentProgress = string.Empty;

		private Rect m_currentPosition;

		private Stamper m_stamper;

		private void Awake()
		{
			m_currentPosition.height = 20f;
			m_currentPosition.width = 300f;
		}

		private void Start()
		{
			CreateStamper();
		}

		private void LateUpdate()
		{
			m_currentPosition.center = new Vector2((float)Screen.width / 2f, (float)Screen.height - 20f);
			if (m_stamper != null)
			{
				m_currentProgress = string.Format("Stamp progress: " + m_stamper.m_stampProgress.ToString());
			}
		}

		private void OnGUI()
		{
			if (m_showGUI && m_showGUI)
			{
				GUI.Label(m_currentPosition, m_currentProgress);
			}
		}

		private void CreateStamper()
		{
			string stampAddress = m_stampAddress;
			stampAddress = stampAddress.Replace("\\", "/");
			TextAsset textAsset = Resources.Load<TextAsset>(stampAddress);
			if (textAsset == null)
			{
				m_currentProgress = "Failed to load stamp at " + stampAddress;
				if (m_showDebug)
				{
					UnityEngine.Debug.Log(m_currentProgress);
				}
				return;
			}
			m_currentProgress = "Loaded stamp at " + textAsset.name;
			if (m_showDebug)
			{
				if (m_showDebug)
				{
					UnityEngine.Debug.Log(m_currentProgress);
				}
				GameObject gameObject = new GameObject("Runtime Stamper");
				m_stamper = gameObject.AddComponent<Stamper>();
				if (m_stamper.LoadRuntimeStamp(textAsset))
				{
					m_currentProgress = "Loaded Stamp";
					m_stamper.FlattenTerrain();
					m_stamper.FitToTerrain();
					m_stamper.m_height = 6f;
					m_stamper.m_distanceMask = AnimationCurve.Linear(0f, 1f, 1f, 0f);
					m_stamper.m_rotation = 0f;
					m_stamper.m_stickBaseToGround = true;
					m_stamper.m_updateTimeAllowed = 71f / (339f * (float)Math.PI);
					m_stamper.UpdateStamp();
					m_stamper.Stamp();
				}
				else
				{
					m_currentProgress = "Failed to load stamp";
				}
				if (m_showDebug)
				{
					UnityEngine.Debug.Log(m_currentProgress);
				}
			}
		}
	}
}
