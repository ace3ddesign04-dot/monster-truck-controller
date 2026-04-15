using UnityEngine;

public class RouteManager : MonoBehaviour
{
	public int RefreshInterval = 20;

	public string mapName;

	public static RouteManager Instance;

	private void Start()
	{
		Instance = this;
		if (mapName == string.Empty || mapName == "SET_ME_TO_MAP_NAME")
		{
			UnityEngine.Debug.LogError("RouteManager needs to know the current map's name!");
		}
		Route[] componentsInChildren = GetComponentsInChildren<Route>();
		Route[] array = componentsInChildren;
		foreach (Route route in array)
		{
			if (route.MultiplayerTrail && GameState.GameType != GameType.TrailRace)
			{
				route.gameObject.SetActive(value: false);
			}
			route.Manager = this;
		}
		if (GameState.GameType == GameType.CaptureTheFlag)
		{
			Route[] array2 = componentsInChildren;
			foreach (Route route2 in array2)
			{
				route2.gameObject.SetActive(value: false);
				route2.Manager = this;
			}
		}
	}
}
