using UnityEngine;

namespace MeshCombineStudio
{
	public class FPSmeter : MonoBehaviour
	{
		public float updateInterval = 0.5f;

		private float lastInterval;

		private int frames;

		public static float fps;

		public bool showFPS = true;

		private float timeNow;

		private void OnGUI()
		{
			if (showFPS)
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(Screen.width - 75, 10f, 150f, 20f), "FPS " + (Mathf.Round(fps * 100f) / 100f).ToString("F0"));
				GUI.color = Color.white;
			}
		}

		private void Update()
		{
			timeNow = Time.realtimeSinceStartup;
			frames++;
			if (timeNow > lastInterval + updateInterval)
			{
				fps = (float)frames / (timeNow - lastInterval);
				frames = 0;
				lastInterval = timeNow;
			}
		}
	}
}
