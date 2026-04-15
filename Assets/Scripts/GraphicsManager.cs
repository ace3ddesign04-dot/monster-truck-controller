using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
	public bool m_showFPS = true;

	public bool m_showGUI = true;

	public bool m_autoQualityManagement = true;

	public float m_autoCheckInterval = 10f;

	public int m_targetFrameRate = 60;

	public int m_frameRateRange = 10;

	public QualityLevel m_maxAutoLevel = QualityLevel.High;

	public QualityLevel m_minAutoLevel = QualityLevel.Low;

	public int m_fontSize = 25;

	private float m_autoCheckTimeLeft;

	private int m_currentQuality;

	private int m_maxQuality = 1;

	private string[] m_qualityNames;

	private bool m_hasDowngraded;

	private float m_fpsUpdateInterval = 0.5f;

	private float m_fpsAccum;

	private int m_fpsFrames;

	private float m_fpsTimeLeft;

	private float m_fpsValue;

	private string m_fpsValueStr = string.Empty;

	private GUIStyle m_fpsStyle = new GUIStyle();

	private GUIStyle m_fpsShadowStyle = new GUIStyle();

	private Rect m_fpsLocation = new Rect(5f, 5f, 100f, 25f);

	private Rect m_fpsShadowLocation = new Rect(6f, 6f, 100f, 25f);

	private void Start()
	{
		Application.targetFrameRate = m_targetFrameRate;
		m_maxQuality = QualitySettings.names.Length - 1;
		m_qualityNames = QualitySettings.names;
		m_currentQuality = QualitySettings.GetQualityLevel();
		m_autoCheckTimeLeft = m_autoCheckInterval;
		UnityEngine.Debug.Log("Current quality is " + m_qualityNames[m_currentQuality]);
		m_fpsStyle.fontSize = m_fontSize;
		m_fpsShadowStyle.fontSize = m_fontSize;
		m_fpsTimeLeft = m_fpsUpdateInterval;
		m_fpsLocation = new Rect(Screen.width - 250, 5f, 100f, 25f);
		m_fpsShadowLocation = new Rect(Screen.width - 250 + 1, 6f, 100f, 25f);
		UpdateQuality();
	}

	private void OnGUI()
	{
		if ((!m_showFPS && !m_showGUI) || !m_showGUI)
		{
			return;
		}
		GUILayout.BeginVertical();
		for (int i = 0; i <= m_maxQuality; i++)
		{
			if (GUILayout.Button(m_qualityNames[i]))
			{
				QualitySettings.SetQualityLevel(i, applyExpensiveChanges: true);
				m_currentQuality = i;
				m_autoQualityManagement = false;
			}
		}
		GUILayout.EndVertical();
	}

	private void Update()
	{
		m_fpsTimeLeft -= Time.deltaTime;
		m_fpsAccum += Time.timeScale / Time.deltaTime;
		m_fpsFrames++;
		if (m_fpsTimeLeft <= 0f)
		{
			m_fpsValue = m_fpsAccum / (float)m_fpsFrames;
			if (m_fpsValue < 30f)
			{
				m_fpsStyle.normal.textColor = Color.yellow;
			}
			else if (m_fpsValue < 10f)
			{
				m_fpsStyle.normal.textColor = Color.red;
			}
			else
			{
				m_fpsStyle.normal.textColor = Color.green;
			}
			m_fpsValueStr = $"{m_fpsValue:f0} {m_qualityNames[m_currentQuality].Substring(0, 3)}";
			m_fpsTimeLeft = m_fpsUpdateInterval;
			m_fpsAccum = 0f;
			m_fpsFrames = 0;
		}
		if (!m_autoQualityManagement)
		{
			return;
		}
		m_autoCheckTimeLeft -= Time.deltaTime;
		if (m_autoCheckTimeLeft < 0f)
		{
			if (m_fpsValue + (float)m_frameRateRange >= (float)m_targetFrameRate && m_currentQuality < (int)m_maxAutoLevel && !m_hasDowngraded)
			{
				IncreaseQuality();
			}
			else if (m_fpsValue <= (float)(m_targetFrameRate - m_frameRateRange) && m_currentQuality > (int)m_minAutoLevel)
			{
				DecreaseQuality();
			}
			m_autoCheckTimeLeft = m_autoCheckInterval;
		}
	}

	private void UpdateQuality()
	{
		UnityEngine.Debug.Log("Changing quality to " + m_qualityNames[m_currentQuality]);
		switch (m_currentQuality)
		{
		case 0:
			Terrain.activeTerrain.heightmapPixelError = 120f;
			Terrain.activeTerrain.heightmapMaximumLOD = 1;
			Terrain.activeTerrain.basemapDistance = 70f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 100f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 100f;
			}
			break;
		case 1:
			Terrain.activeTerrain.heightmapPixelError = 80f;
			Terrain.activeTerrain.heightmapMaximumLOD = 1;
			Terrain.activeTerrain.basemapDistance = 80f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 120f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 120f;
			}
			break;
		case 2:
			Terrain.activeTerrain.heightmapPixelError = 40f;
			Terrain.activeTerrain.heightmapMaximumLOD = 0;
			Terrain.activeTerrain.basemapDistance = 90f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 300f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 300f;
			}
			break;
		case 3:
			Terrain.activeTerrain.heightmapPixelError = 40f;
			Terrain.activeTerrain.heightmapMaximumLOD = 0;
			Terrain.activeTerrain.basemapDistance = 100f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 300f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 300f;
			}
			break;
		case 4:
			Terrain.activeTerrain.heightmapPixelError = 20f;
			Terrain.activeTerrain.heightmapMaximumLOD = 0;
			Terrain.activeTerrain.basemapDistance = 200f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 500f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 500f;
			}
			break;
		case 5:
			Terrain.activeTerrain.heightmapPixelError = 10f;
			Terrain.activeTerrain.heightmapMaximumLOD = 0;
			Terrain.activeTerrain.basemapDistance = 300f;
			RenderSettings.fogStartDistance = 35f;
			RenderSettings.fogEndDistance = 600f;
			if (Camera.main != null)
			{
				Camera.main.farClipPlane = 600f;
			}
			break;
		}
		QualitySettings.SetQualityLevel(m_currentQuality, applyExpensiveChanges: true);
	}

	private bool IncreaseQuality()
	{
		if (m_currentQuality < m_maxQuality)
		{
			m_currentQuality++;
		}
		return true;
	}

	private bool DecreaseQuality()
	{
		if (m_currentQuality > 0)
		{
			m_currentQuality--;
			m_hasDowngraded = true;
			GameState.FramerateWarning = true;
		}
		return true;
	}
}
