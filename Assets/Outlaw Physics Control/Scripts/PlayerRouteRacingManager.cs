using CustomVP;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRouteRacingManager : MonoBehaviour
{
	private CarUIControl carUIControl;

	public float routeDetectionRadius;

	public float checkpointDetectionRadius;

	public bool inRace;

	public bool raceTimerEnabled;

	private PlayerRoute[] routes;

	private int currentCheckpointID;

	private float raceTime;

	private PlayerRoute currentRoute;

	private PlayerRoute lastRoute;

	private PlayerRoute tempRoute;

	private Coroutine countdownCor;

	private Coroutine loadRouteInfoCor;

	private bool initialized;

	public static PlayerRouteRacingManager Instance;

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

	private Transform CurrentCheckpoint => currentRoute.checkpoints[currentCheckpointID];

	private void Awake()
	{
		Instance = this;
	}

	public void Initialize()
	{
		routes = LevelEditorTools.RoutesParent.GetComponentsInChildren<PlayerRoute>();
		carUIControl = CarUIControl.Instance;
		initialized = true;
	}

	private void Update()
	{
		if (!initialized || playerVehicle == null)
		{
			return;
		}
		if (!inRace)
		{
			CheckClosestRoutes();
		}
		else
		{
			CheckCheckpoints();
			if (raceTimerEnabled)
			{
				raceTime += Time.deltaTime;
				carUIControl.UpdateTimer(raceTime);
			}
		}
		if (currentRoute != lastRoute && currentRoute != null && loadRouteInfoCor == null)
		{
			loadRouteInfoCor = StartCoroutine(LoadRouteRecordCor(currentRoute));
		}
		lastRoute = currentRoute;
		if (currentRoute == null)
		{
			carUIControl.HidePlayerRouteInfo();
		}
	}

	private IEnumerator LoadRouteRecordCor(PlayerRoute route)
	{
		for (int i = 0; i < 10; i++)
		{
			WWW query = new WWW("https://keereedev.000webhostapp.com/RoutesHandler.php?record&routeID=" + route.routeID);
			yield return query;
			if (query.error == null)
			{
				OnRouteRecordLoaded(query.text, route);
				break;
			}
		}
		loadRouteInfoCor = null;
	}

	private void OnRouteRecordLoaded(string rawData, PlayerRoute route)
	{
		UnityEngine.Debug.Log("Route record loaded:" + rawData);
		float num = 0f;
		float bronzeTime = 0f;
		float silverTime = 0f;
		float goldTime = 0f;
		string routeRecordKeeper = GameState.playerName;
		string[] array = rawData.Split('/');
		if (array.Length != 0)
		{
			num = float.Parse(array[1]);
			routeRecordKeeper = array[0];
			bronzeTime = num * 2f;
			silverTime = num + num * 0.4f;
			goldTime = num + num * 0.2f;
		}
		route.routeRecord = num;
		route.routeRecordKeeper = routeRecordKeeper;
		float num2 = 1f;
		float num3 = route.RouteLength();
		if (num3 > 0f)
		{
			num2 = Mathf.InverseLerp(0f, 500f, num3);
		}
		float finishReward = Mathf.RoundToInt(PlayerRoute.completionMoney * num2);
		float bronzeReward = Mathf.RoundToInt(PlayerRoute.bronzeMoney * num2);
		float silverReward = Mathf.RoundToInt(PlayerRoute.silverMoney * num2);
		float goldReward = Mathf.RoundToInt(PlayerRoute.goldMoney * num2);
		carUIControl.DisplayPlayerRouteValues(bronzeTime, silverTime, goldTime, finishReward, bronzeReward, silverReward, goldReward, num);
	}

	private void OnDisable()
	{
		if (initialized)
		{
			CancelRace();
		}
	}

	public void GetToGarage()
	{
		if (playerVehicle != null)
		{
			playerVehicle.GetComponent<VehicleDataManager>().SaveVehicleData();
		}
		SceneManager.LoadScene("Menu");
	}

	private void CheckClosestRoutes()
	{
		PlayerRoute[] array = routes;
		foreach (PlayerRoute playerRoute in array)
		{
			if (Vector3.Distance(playerVehicle.transform.position, playerRoute.checkpoints[0].position) < routeDetectionRadius)
			{
				currentRoute = playerRoute;
				carUIControl.DisplayPlayerRouteInfo();
				return;
			}
		}
		currentRoute = null;
	}

	private void CheckCheckpoints()
	{
		if (Vector3.Distance(playerVehicle.transform.position, CurrentCheckpoint.position) < checkpointDetectionRadius)
		{
			if (currentCheckpointID < currentRoute.checkpoints.Count - 1)
			{
				NextCheckpoint();
			}
			else if (currentCheckpointID == currentRoute.checkpoints.Count - 1)
			{
				Finish();
			}
		}
	}

	private void NextCheckpoint()
	{
		currentCheckpointID++;
		carUIControl.CurrentCheckpoint = CurrentCheckpoint;
		carUIControl.ShowNotification("Checkpoint", blinking: false);
	}

	private void Finish()
	{
		if (!raceTimerEnabled)
		{
			return;
		}
		raceTimerEnabled = false;
		currentRoute.ToggleCheckpoints(on: false);
		playerVehicle.GetComponent<CarController>().vehicleIsActive = false;
		carUIControl.DisplayPlayerRouteFinish();
		bool flag = currentRoute.routeRecord == 0f || raceTime < currentRoute.routeRecord;
		float record = (!flag) ? currentRoute.routeRecord : raceTime;
		string keeper = (!flag) ? currentRoute.routeRecordKeeper : GameState.playerName;
		string awardLevel = "Gold";
		float xp = PlayerRoute.goldXP;
		float num = PlayerRoute.goldMoney;
		float num2 = PlayerRoute.goldGolds;
		if (currentRoute.routeRecord != 0f)
		{
			float num3 = currentRoute.routeRecord * 2f;
			float num4 = currentRoute.routeRecord + currentRoute.routeRecord * 0.4f;
			float num5 = currentRoute.routeRecord + currentRoute.routeRecord * 0.2f;
			if (raceTime < num3)
			{
				awardLevel = "Bronze";
				xp = PlayerRoute.bronzeXP;
				num = PlayerRoute.bronzeMoney;
				num2 = PlayerRoute.bronzeGolds;
			}
			if (raceTime < num4)
			{
				awardLevel = "Silver";
				xp = PlayerRoute.silverXP;
				num = PlayerRoute.silverMoney;
				num2 = PlayerRoute.silverGolds;
			}
			if (raceTime < num5)
			{
				awardLevel = "Gold";
				xp = PlayerRoute.goldXP;
				num = PlayerRoute.goldMoney;
				num2 = PlayerRoute.goldGolds;
			}
		}
		float num6 = 1f;
		float num7 = currentRoute.RouteLength();
		if (num7 > 0f)
		{
			num6 = Mathf.InverseLerp(0f, 500f, num7);
		}
		num *= num6;
		num = Mathf.RoundToInt(num);
		num2 = (int)(num2 * num6);
		carUIControl.ShowPlayerRouteFinish(raceTime, record, keeper, awardLevel, xp, num2, num);
		GameState.AddCurrency((int)num, Currency.Money);
		GameState.AddCurrency((int)num2, Currency.Gold);
		StartCoroutine(SubmitTimeCor());
	}

	private IEnumerator SubmitTimeCor()
	{
		int i = 0;
		WWW query;
		while (true)
		{
			if (i < 10)
			{
				query = new WWW("https://keereedev.000webhostapp.com/RoutesHandler.php?submit&playerID=" + GameState.playerName + "&routeID=" + currentRoute.routeID + "&time=" + raceTime);
				yield return query;
				if (query.error == null)
				{
					break;
				}
				i++;
				continue;
			}
			yield break;
		}
		OnTimeSubmitted(query.text);
	}

	private void OnTimeSubmitted(string info)
	{
	}

	public void Continue()
	{
		carUIControl.HidePlayerRouteFinish();
		playerVehicle.GetComponent<CarController>().vehicleIsActive = true;
		carUIControl.HideShowRaceUI(Show: false, ShowCancelButton: false);
		inRace = false;
	}

	public void StartRace()
	{
		carUIControl.HidePlayerRouteInfo();
		carUIControl.HideShowRaceUI(Show: true, ShowCancelButton: true);
		raceTime = 0f;
		currentCheckpointID = 1;
		inRace = true;
		raceTimerEnabled = false;
		carUIControl.CurrentCheckpoint = CurrentCheckpoint;
		carUIControl.UpdateTimer(raceTime);
		currentRoute.BakeRoute();
		currentRoute.ToggleCheckpoints(on: true);
		playerVehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
		playerVehicle.transform.position = currentRoute.checkpoints[0].position;
		playerVehicle.transform.LookAt(CurrentCheckpoint);
		Utility.AlignVehicleByGround(playerVehicle.transform);
		countdownCor = StartCoroutine(CountdownCor());
	}

	public void CancelRace()
	{
		inRace = false;
		carUIControl.HideShowRaceUI(Show: false, ShowCancelButton: false);
		if (currentRoute != null)
		{
			currentRoute.ToggleCheckpoints(on: false);
		}
		if (countdownCor != null)
		{
			StopCoroutine(countdownCor);
		}
		playerVehicle.GetComponent<CarController>().vehicleIsActive = true;
		carUIControl.HideShowCountdown(Show: false);
		carUIControl.HidePlayerRouteFinish();
	}

	private IEnumerator CountdownCor()
	{
		carUIControl.HideShowCountdown(Show: true);
		playerVehicle.GetComponent<CarController>().vehicleIsActive = false;
		for (int c = 3; c > 0; c--)
		{
			carUIControl.ShowCountdownText(c);
			yield return new WaitForSeconds(1f);
		}
		carUIControl.HideShowCountdown(Show: false);
		playerVehicle.GetComponent<CarController>().vehicleIsActive = true;
		raceTimerEnabled = true;
		countdownCor = null;
	}
}
