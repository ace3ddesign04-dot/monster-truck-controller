using CustomVP;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VehicleLoader : MonoBehaviour
{
	private RacingManager racingManager;

	public Transform[] SpawnPoints;

	public bool levelEditor;

	public bool customMap;

	public static VehicleLoader Instance;

	[HideInInspector]
	public GameObject playerVehicle;

	[HideInInspector]
	public CarController playerCarController;

	[HideInInspector]
	public BodyPartsSwitcher playerPartsSwitcher;

	[HideInInspector]
	public SuspensionController playerSuspensionController;

	[HideInInspector]
	public Rigidbody playerRigidbody;

	[HideInInspector]
	public PhotonTransformView playerTView;

	[HideInInspector]
	public IKDriverController playerDriver;

	[HideInInspector]
	public VehicleDataManager playerDataManager;

	[HideInInspector]
	public EngineController playerEngine;

	[HideInInspector]
	public GameObject carOnTrailer;

	[HideInInspector]
	public GameObject trailer;

	private float swapTimer;

	private float nextResourcesUnloadTime;

	[HideInInspector]
	public bool loadVehicle = true;

	private bool loadingComplete;

	private bool teamSetupComplete;

	private CarUIControl ui => CarUIControl.Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		if (CarUIControl.Instance != null)
		{
			ShowLoading();
		}
		UnityEngine.Debug.Log("Starting vehicle loader");
		racingManager = UnityEngine.Object.FindObjectOfType<RacingManager>();
		nextResourcesUnloadTime = Time.time + GameState.resourcesUnloadRate;
	}

	public void Update()
	{
		if (GameState.unloadUnusedResourcesInGame && !levelEditor && Time.time > nextResourcesUnloadTime)
		{
			UnityEngine.Debug.LogError("Resources were unloaded");
			nextResourcesUnloadTime = Time.time + GameState.resourcesUnloadRate;
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
		if (loadVehicle)
		{
			PhotonNetwork.player.SetTeam(PunTeams.Team.none);
			LoadVehicle(swapping: false);
			if (CarUIControl.Instance != null)
			{
				Invoke("HideLoading", 0.1f);
			}
		}
		if (loadingComplete && !teamSetupComplete && !customMap && !levelEditor)
		{
			Invoke("SetupTeam", 0.1f);
		}
		if (CrossPlatformInputManager.GetButtonUp("SwapVehicles"))
		{
			SwapTrailerVehicles();
		}
		if (!levelEditor || (levelEditor && playerVehicle.activeSelf))
		{
			if (swapTimer > 0f)
			{
				swapTimer -= Time.deltaTime;
			}
			else
			{
				swapTimer = 0f;
			}
			ui.UpdateSwapButtonText((int)swapTimer);
		}
	}

	public void HideLoading()
	{
		CarUIControl.Instance.LoadingScreen.SetActive(value: false);
	}

	public void ShowLoading()
	{
		CarUIControl.Instance.LoadingScreen.SetActive(value: true);
	}

	public void SetupTeam()
	{
		if (teamSetupComplete)
		{
			return;
		}
		if (GameState.GameType == GameType.CaptureTheFlag)
		{
			UnityEngine.Debug.Log("Blue players: " + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count);
			UnityEngine.Debug.Log("Red players: " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count);
			if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count >= PunTeams.PlayersPerTeam[PunTeams.Team.red].Count)
			{
				UnityEngine.Debug.Log("Setting team to red");
				PhotonNetwork.player.SetTeam(PunTeams.Team.red);
			}
			else
			{
				UnityEngine.Debug.Log("Setting team to blue");
				PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
			}
		}
		if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count > 0 || PunTeams.PlayersPerTeam[PunTeams.Team.red].Count > 0)
		{
			teamSetupComplete = true;
		}
	}

	public void LoadVehicle(bool swapping)
	{
		if (SpawnPoints.Length == 0)
		{
			UnityEngine.Debug.LogError("Assign spawn point(s)");
			return;
		}
		string text = GameState.CurrentVehicleID;
		if (text == string.Empty)
		{
			string @string = DataStore.GetString("VehiclesList");
			if (@string != string.Empty)
			{
				SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
				text = savedVehiclesList.VehicleIDs[0];
			}
		}
		string string2 = DataStore.GetString(text);
		VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(string2);
		Transform availableSpawnPoint = GetAvailableSpawnPoint();
		Vector3 position = availableSpawnPoint.position;
		Quaternion rotation = availableSpawnPoint.rotation;
		if (swapping)
		{
			position = playerVehicle.transform.position;
			rotation = playerVehicle.transform.rotation;
		}
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			if (swapping)
			{
				PhotonNetwork.Destroy(playerVehicle.GetPhotonView());
			}
			playerVehicle = PhotonNetwork.Instantiate("Vehicles/" + vehicleData.VehicleName, position, rotation, 0);
		}
		else
		{
			if (swapping)
			{
				UnityEngine.Object.DestroyImmediate(playerVehicle);
			}
			playerVehicle = (UnityEngine.Object.Instantiate(Resources.Load("Vehicles/" + vehicleData.VehicleName, typeof(GameObject)), position, rotation) as GameObject);
		}
		playerVehicle.name = vehicleData.VehicleName;
		CachePlayerComponents();
		Utility.AlignVehicleByGround(playerVehicle.transform);
		if (playerDriver != null)
		{
			playerDriver.ToggleDriver(ShowDriver: true, ShowHands: false);
		}
		playerDataManager.VehicleID = text;
		playerDataManager.LoadVehicleData();
		playerPartsSwitcher.UpdateColor(Merge: true);
		playerPartsSwitcher.UpdateDirtiness();
		UpdateUiAccordingToCar();
		bool flag = false;
		if (GameState.GameType == GameType.CaptureTheFlag || GameState.GameType == GameType.TrailRace || playerDataManager.vehicleType == VehicleType.Bike)
		{
			flag = true;
		}
		if (playerDataManager.vehicleType == VehicleType.ATV && !swapping)
		{
			flag = true;
		}
		if (levelEditor)
		{
			flag = true;
		}
		if (!flag)
		{
			string text2 = Utility.EqiuppedTrailer();
			if (Utility.DoesTruckExist(text2))
			{
				Vector3 position2 = Vector3.zero;
				Quaternion rotation2 = Quaternion.identity;
				if (swapping)
				{
					position2 = trailer.transform.position;
					rotation2 = trailer.transform.rotation;
					UnityEngine.Object.DestroyImmediate(trailer);
				}
				string string3 = DataStore.GetString(text2);
				VehicleData vehicleData2 = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(string3);
				trailer = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Vehicles/" + vehicleData2.VehicleName), position2, rotation2);
				TrailerController component = trailer.GetComponent<TrailerController>();
				if (!swapping)
				{
					component.ConnectToCar();
				}
				playerCarController.myTrailer = component;
				if (GameState.GameMode == GameMode.Multiplayer)
				{
					playerTView.SpawnTrailer(vehicleData2.VehicleName);
				}
			}
			string string4 = DataStore.GetString("VehicleOnTrailer");
			if (Utility.DoesTruckExist(string4))
			{
				if (swapping)
				{
					UnityEngine.Object.DestroyImmediate(carOnTrailer);
				}
				string string5 = DataStore.GetString(string4);
				VehicleData vehicleData3 = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(string5);
				carOnTrailer = (UnityEngine.Object.Instantiate(Resources.Load("Vehicles/" + vehicleData3.VehicleName, typeof(GameObject))) as GameObject);
				carOnTrailer.name = vehicleData3.VehicleName;
				IKDriverController component2 = carOnTrailer.GetComponent<IKDriverController>();
				if (component2 != null)
				{
					component2.ToggleDriver(ShowDriver: false, ShowHands: false);
					component2.enabled = false;
				}
				VehicleDataManager component3 = carOnTrailer.GetComponent<VehicleDataManager>();
				component3.VehicleID = string4;
				component3.LoadVehicleData();
				carOnTrailer.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: true);
				carOnTrailer.GetComponent<BodyPartsSwitcher>().UpdateDirtiness();
				carOnTrailer.GetComponent<CarController>().UpdateEngineModel();
				carOnTrailer.GetComponent<CarController>().SetCalculatedCOM();
				if (carOnTrailer.GetComponentInChildren<EngineSoundProcessor>() != null)
				{
					carOnTrailer.GetComponentInChildren<EngineSoundProcessor>().enabled = false;
				}
				component3.LoadOnTrailer(trailer.GetComponent<TrailerController>());
				if (GameState.GameMode == GameMode.Multiplayer)
				{
					playerTView.SpawnTraileredCar(string5);
				}
			}
		}
		if (GameState.GameType == GameType.TrailRace)
		{
			TrailRaceManager.Instance.InitializeRace();
			TrailRaceManager.Instance.ImTotallyLoaded();
		}
		if (MultiplayerManager.Instance != null)
		{
			MultiplayerManager.Instance.TotallyReady = true;
			MultiplayerManager.RefreshCurrentPlayers();
		}
		loadVehicle = false;
		loadingComplete = true;
		if (levelEditor)
		{
			playerVehicle.SetActive(value: false);
		}
	}

	public void UpdateUiAccordingToCar()
	{
		if (CarUIControl.Instance != null)
		{
			CarUIControl.Instance.ToggleEbrake(playerCarController.Ebrake == 1);
			CarUIControl.Instance.ToggleGearShifter(playerCarController.transmissionType == TransmissionType.Manual);
			CarUIControl.Instance.SwitchWinchToggleButton(playerPartsSwitcher.WinchInstalled);
			CarUIControl.Instance.ToggleRepairButton(playerPartsSwitcher.RepairPackInstalled);
		}
	}

	private void CachePlayerComponents()
	{
		playerCarController = playerVehicle.GetComponent<CarController>();
		playerPartsSwitcher = playerVehicle.GetComponent<BodyPartsSwitcher>();
		playerSuspensionController = playerVehicle.GetComponent<SuspensionController>();
		playerRigidbody = playerVehicle.GetComponent<Rigidbody>();
		playerTView = playerVehicle.GetComponent<PhotonTransformView>();
		playerDriver = playerVehicle.GetComponent<IKDriverController>();
		playerEngine = playerVehicle.GetComponent<EngineController>();
		playerDataManager = playerVehicle.GetComponent<VehicleDataManager>();
	}

	[ContextMenu("Swap")]
	public void SwapTrailerVehicles()
	{
		swapTimer = 10f;
		string vehicleID = playerDataManager.VehicleID;
		string text = GameState.CurrentVehicleID = carOnTrailer.GetComponent<VehicleDataManager>().VehicleID;
		DataStore.SetString("VehicleOnTrailer", vehicleID);
		LoadVehicle(swapping: true);
	}

	public Transform GetAvailableSpawnPoint()
	{
		PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < SpawnPoints.Length; i++)
		{
			bool flag = true;
			PhotonView[] array2 = array;
			foreach (PhotonView photonView in array2)
			{
				if (Vector3.Distance(photonView.transform.position, SpawnPoints[i].position) < 10f)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(SpawnPoints[i]);
			}
		}
		if (list.Count > 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
	}
}
