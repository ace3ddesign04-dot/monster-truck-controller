using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
	public GameObject levelEditorComponents;

	public GameObject gameplayComponents;

	public GameObject playMapButton;

	public GameObject editMapButton;

	private LevelEditor levelEditor;

	public CanvasGroup warningPanel;

	public Text warningText;

	private float warningAlpha;

	private Transform[] spawnPoints;

	private GameObject playerVehicle
	{
		get
		{
			if (VehicleLoader.Instance != null)
			{
				return VehicleLoader.Instance.playerVehicle;
			}
			return null;
		}
	}

	private void Start()
	{
		levelEditor = LevelEditor.Instance;
		warningPanel.alpha = 0f;
		SwitchMode(editing: true);
	}

	public void SwitchMode(bool editing)
	{
		if (levelEditor == null)
		{
			return;
		}
		if (!editing)
		{
			spawnPoints = levelEditor.GetSpawnPoints();
			if (spawnPoints.Length == 0)
			{
				ShowWarning("You need at least one spawn point!");
				return;
			}
		}
		if (spawnPoints != null)
		{
			Transform[] array = spawnPoints;
			foreach (Transform transform in array)
			{
				transform.gameObject.SetActive(editing);
			}
		}
		levelEditorComponents.SetActive(editing);
		gameplayComponents.SetActive(!editing);
		playMapButton.SetActive(editing);
		editMapButton.SetActive(!editing);
		if (editing)
		{
			levelEditor.CancelPreBakeProps();
		}
		else
		{
			VehicleLoader.Instance.UpdateUiAccordingToCar();
			levelEditor.PreBakeProps();
			levelEditor.terCollider.enabled = false;
			levelEditor.terCollider.enabled = true;
		}
		if (playerVehicle != null)
		{
			playerVehicle.SetActive(!editing);
			playerVehicle.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			playerVehicle.transform.rotation = spawnPoints[Random.Range(0, spawnPoints.Length)].rotation;
			playerVehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
			Utility.AlignVehicleByGround(playerVehicle.transform);
		}
		levelEditor.ChangeLevelCreationStep(LevelCreationStep.None);
		if (!editing)
		{
			PlayerRouteRacingManager.Instance.Initialize();
			levelEditor.CacheSplatMaps();
			LevelEditorTools.ApplyMudStamps(levelEditor.mudStamps, Terrain.activeTerrain);
			SurfaceManager.Instance.CreateMudTerrains(levelEditor.mudStamps);
			LevelEditorTools.ToggleMudIndicators(enable: false);
		}
		else
		{
			LevelEditorTools.ToggleMudIndicators(enable: true);
			if (SurfaceManager.Instance != null)
			{
				SurfaceManager.Instance.RemoveMudTerrains(levelEditor.mudStamps);
			}
			levelEditor.RestoreSplatMaps();
		}
	}

	private void ShowWarning(string text)
	{
		warningText.text = text;
		warningAlpha = 2f;
	}

	private void Update()
	{
		warningAlpha -= Time.deltaTime;
		warningPanel.alpha = warningAlpha;
	}
}
