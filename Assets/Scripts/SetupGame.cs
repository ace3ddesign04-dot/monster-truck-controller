using UnityEngine;

public class SetupGame : MonoBehaviour
{
	public int minimumResolution = 1920;

	private void OnEnable()
	{
		UnityEngine.Debug.Log("Setup game running");
		Application.targetFrameRate = 60;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		if (Screen.width >= minimumResolution)
		{
			Screen.SetResolution(Screen.width / 2, Screen.height / 2, fullscreen: true);
		}
		Screen.sleepTimeout = -1;
	}
}
