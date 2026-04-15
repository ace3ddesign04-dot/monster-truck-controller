using System.Collections;
using System.Collections.Generic;
using CustomVP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingManager : MonoBehaviour
{
    private CarUIControl carUIControl;

    private bool InRace;

    private bool AnyEventIsNear;

    public Route ActiveRoute;

    private Route[] AllRoutes;

    private int CurrentCheckpoint;

    private int CurrentLap;

    private float StartHealth;

    private int FlippedOverCount;

    public float RaceTime;

    private int WinchUsedTimes;

    private bool WinchUsingCounted;

    private bool AllCheckpointsPassed;

    private bool RaceTimerEnabled;

    private bool ShowingEventLobby;

    private AudioSource CheckpointSound;

    private float CheckpointDetectionDistance = 10f;

    private float EventsDetectionDistance = 10f;

    private Coroutine countdownRoutine;

    public string SerializedString;

    public OnRouteCompleteDelegate OnRouteCompleteHandler;

    private Route ClosestRoute;

    private Route CurrentLobbyRoute;

    public static RacingManager Instance;

    private CarController carController
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerCarController;
            }

            return null;
        }
    }

    private BodyPartsSwitcher partsSwitcher
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerPartsSwitcher;
            }

            return null;
        }
    }

    private Rigidbody playerRigidbody
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerRigidbody;
            }

            return null;
        }
    }

    private PhotonTransformView photonTransformView
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerTView;
            }

            return null;
        }
    }

    private VehicleDataManager dataManager
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerDataManager;
            }

            return null;
        }
    }

    public bool IsPlayerBusy => InRace || AnyEventIsNear;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AllRoutes = FindObjectsOfType<Route>();
        carUIControl = CarUIControl.Instance;
        CheckpointSound = Instantiate(Resources.Load("Sounds/Checkpoint") as GameObject).GetComponent<AudioSource>();
        CheckpointSound.transform.parent = transform;
        HideAllCheckpoints();
    }

    public void CollidedWithCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint.ID == CurrentCheckpoint)
        {
            NextCheckpoint();
        }
    }

    public void SaveVehicleData()
    {
        if (dataManager != null)
        {
            dataManager.SaveVehicleData();
        }
    }

    public void GetToGarage()
    {
        SaveVehicleData();
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (carController == null)
        {
            return;
        }

        if (!InRace && !StashManager.Instance.LockboxActive && Time.frameCount % 10 == 0 &&
            GameState.GameType != GameType.TrailRace)
        {
            CheckClosestEvents();
        }

        if (InRace)
        {
            CheckFinish();
            if (RaceTimerEnabled)
            {
                RaceTime += Time.deltaTime;
            }

            carUIControl.UpdateTimer(RaceTime);
        }
    }

    private void HideAllCheckpoints()
    {
        Route[] allRoutes = AllRoutes;
        foreach (Route route in allRoutes)
        {
            route.HideShowAllCheckpoints(Show: false);
            route.HideShowStartAndFinish(Show: false);
        }
    }

    public void HideShowEventMarks(bool Show)
    {
        Route[] allRoutes = AllRoutes;
        foreach (Route route in allRoutes)
        {
            route.HideShowEventMark(Show);
        }
    }

    public void StartRace()
    {
        InRace = true;
        RaceTimerEnabled = false;
        WinchUsedTimes = 0;
        RaceTime = 0f;
        AllCheckpointsPassed = false;
        CurrentLap = 1;
        CurrentCheckpoint = 0;
        StartHealth = carController.CarHealth;
        FlippedOverCount = 0;
        carUIControl.HideEventLobby();
        carUIControl.HideShowRaceUI(Show: true, ShowCancelButton: true);
        carUIControl.UpdateWinchUsedText(WinchUsedTimes);
        carUIControl.UpdateLapText(CurrentLap, ActiveRoute.LapsNumber);
        carUIControl.CurrentCheckpoint = ActiveRoute.SpawnedCheckpoints[0].transform;
        carController.m_Rigidbody.velocity = Vector3.zero;
        carController.transform.position = ActiveRoute.Waypoints[0].position + Vector3.up * 2f;
        carController.transform.LookAt(ActiveRoute.Waypoints[1]);
        ActiveRoute.HideShowStartAndFinish(Show: true);
        ActiveRoute.ShowCheckpoint(0);
        HideShowEventMarks(Show: false);
        countdownRoutine = StartCoroutine(Countdown());
    }

    public void CancelRace()
    {
        RaceTimerEnabled = false;
        carController.vehicleIsActive = true;
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }

        carUIControl.HideShowRaceUI(Show: false, ShowCancelButton: false);
        carUIControl.HideShowCountdown(Show: false);
        ActiveRoute.HideShowStartAndFinish(Show: false);
        ActiveRoute.HideShowAllCheckpoints(Show: false);
        InRace = false;
        HideShowEventMarks(Show: true);
    }

    public void Continue()
    {
        carController.vehicleIsActive = true;
        carUIControl.FinishInfo.SetActive(value: false);
        carUIControl.HideShowRaceUI(Show: false, ShowCancelButton: false);
        ActiveRoute.Completed = true;
        ActiveRoute.HideShowStartAndFinish(Show: false);
        ActiveRoute.HideShowAllCheckpoints(Show: false);
        InRace = false;
        HideShowEventMarks(Show: true);
    }

    public void FlippedOver()
    {
        if (InRace)
        {
            FlippedOverCount++;
        }
    }

    private IEnumerator Countdown()
    {
        carUIControl.HideShowCountdown(Show: true);
        carController.vehicleIsActive = false;
        for (int c = 3; c > 0; c--)
        {
            carUIControl.ShowCountdownText(c);
            yield return new WaitForSeconds(1f);
        }

        carUIControl.HideShowCountdown(Show: false);
        carController.vehicleIsActive = true;
        RaceTimerEnabled = true;
    }

    private void Finish()
    {
        carController.vehicleIsActive = false;
        RaceTimerEnabled = false;
        CheckpointSound.Play();
        if (dataManager.vehicleType == VehicleType.Any)
        {
            Debug.Log("VEHICLE TYPE IS NOT SET FOR THE CURRENT VEHICLE!");
            return;
        }

        int num = Mathf.RoundToInt(RaceTime * 100f);
        ActiveRoute.SubmitTime(num);
        RoutePayment routePayment = ActiveRoute.GetRoutePayment(dataManager.vehicleType, num, WinchUsedTimes,
            FlippedOverCount, StartHealth - carController.CarHealth);
        StatsData statsData = GameState.LoadStatsData();
        int amount = Utility.AdjustedWinnings(routePayment.Cash);
        int amount2 = Utility.AdjustedWinnings(routePayment.XP);
        int num2 = Utility.AdjustedWinnings(routePayment.Gold) +
                   Utility.AdjustedWinnings(routePayment.TrailblazerGoldBonus);
        Debug.Log("TRAILBLAZER BONUS!" + routePayment.TrailblazerGoldBonus);
        GameState.AddCurrency(amount, Currency.Money);
        if (num2 > 0)
        {
            GameState.AddCurrency(num2, Currency.Gold);
        }

        GameState.AddXP(amount2);
        int num3 = 0;
        string s = ActiveRoute.RouteName.Replace("Route", string.Empty);
        num3 = int.Parse(s);
        string text = ActiveRoute.GetRecordKeeper();
        if (text == string.Empty)
        {
            text = "Undefined";
        }

        if (num < ActiveRoute.RouteRecord)
        {
            text = GameState.playerName;
        }

        carUIControl.HideShowFinishWindow(Show: true, routePayment, num, ActiveRoute.RouteRecord, num3, text);
        if (OnRouteCompleteHandler != null)
        {
            OnRouteCompleteHandler(ActiveRoute, dataManager.vehicleType, routePayment);
        }
    }

    private void NewLap()
    {
        CurrentLap++;
        CurrentCheckpoint = 0;
        AllCheckpointsPassed = false;
        ActiveRoute.ShowCheckpoint(CurrentCheckpoint);
        carUIControl.UpdateLapText(CurrentLap, ActiveRoute.LapsNumber);
        carUIControl.ShowNotification("New lap", blinking: false);
        carUIControl.CurrentCheckpoint = ActiveRoute.SpawnedCheckpoints[CurrentCheckpoint].transform;
        CheckpointSound.Play();
    }

    private void CheckClosestEvents()
    {
        AnyEventIsNear = false;
        List<Route> list = new List<Route>();
        Route route = null;
        Route[] allRoutes = AllRoutes;
        foreach (Route route2 in allRoutes)
        {
            if (Vector3.Distance(carController.transform.position, route2.transform.position) < EventsDetectionDistance)
            {
                list.Add(route2);
            }
        }

        if (list.Count > 0)
        {
            float num = 99999f;
            for (int j = 0; j < list.Count; j++)
            {
                float num2 = Vector3.Distance(carController.transform.position, list[j].transform.position);
                if (num2 < num)
                {
                    num = num2;
                    route = list[j];
                }
            }

            if (route != CurrentLobbyRoute)
            {
                ShowingEventLobby = true;
                AnyEventIsNear = true;
                ActiveRoute = route;
                CurrentLobbyRoute = route;
                carUIControl.ShowEventLobby(route);
                route.RefreshRecord();
            }
        }
        else if (ShowingEventLobby)
        {
            Debug.Log("Close routes: " + list.Count);
            ShowingEventLobby = false;
            Debug.Log("HH");
            carUIControl.HideEventLobby();
            route = null;
            ActiveRoute = null;
            CurrentLobbyRoute = null;
        }
    }

    private void NextCheckpoint()
    {
        CurrentCheckpoint++;
        if (CurrentCheckpoint < ActiveRoute.SpawnedCheckpoints.Count)
        {
            carUIControl.CurrentCheckpoint = ActiveRoute.SpawnedCheckpoints[CurrentCheckpoint].transform;
        }
        else
        {
            AllCheckpointsPassed = true;
            if (ActiveRoute.SpawnedFinish != null)
            {
                carUIControl.CurrentCheckpoint = ActiveRoute.SpawnedFinish.transform;
            }
            else
            {
                carUIControl.CurrentCheckpoint = ActiveRoute.SpawnedStart.transform;
            }
        }

        ActiveRoute.ShowCheckpoint(CurrentCheckpoint);
        carUIControl.ShowNotification("Checkpoint", blinking: false);
        CheckpointSound.Play();
    }

    private void CheckCheckpoints()
    {
        if (CurrentCheckpoint < ActiveRoute.SpawnedCheckpoints.Count)
        {
            Vector3 position = carController.transform.position;
            Vector3 position2 = ActiveRoute.SpawnedCheckpoints[CurrentCheckpoint].transform.position;
            position2.y = position.y;
            if (Vector3.Distance(position, position2) < CheckpointDetectionDistance && !AllCheckpointsPassed)
            {
                NextCheckpoint();
            }
        }
    }

    private void CheckFinish()
    {
        switch (ActiveRoute.Circuit)
        {
            case false:
                if (Vector3.Distance(carController.transform.position, ActiveRoute.SpawnedFinish.transform.position) <
                    CheckpointDetectionDistance && AllCheckpointsPassed && RaceTimerEnabled)
                {
                    Finish();
                }

                break;
            case true:
                if (Vector3.Distance(carController.transform.position, ActiveRoute.SpawnedStart.transform.position) <
                    CheckpointDetectionDistance && AllCheckpointsPassed && RaceTimerEnabled)
                {
                    if (CurrentLap == ActiveRoute.LapsNumber)
                    {
                        Finish();
                    }
                    else
                    {
                        NewLap();
                    }
                }

                break;
        }
    }

    private RoutesData GetRoutesData()
    {
        RoutesData routesData = new RoutesData();
        routesData.RoutesCompleteness = new bool[AllRoutes.Length];
        routesData.RoutesNames = new string[AllRoutes.Length];
        for (int i = 0; i < AllRoutes.Length; i++)
        {
            routesData.RoutesNames[i] = AllRoutes[i].name;
            routesData.RoutesCompleteness[i] = AllRoutes[i].Completed;
        }

        return routesData;
    }

    private void SetRoutesData(RoutesData rData)
    {
        for (int i = 0; i < rData.RoutesNames.Length; i++)
        {
            Route[] allRoutes = AllRoutes;
            foreach (Route route in allRoutes)
            {
                if (route.name == rData.RoutesNames[i])
                {
                    route.Completed = rData.RoutesCompleteness[i];
                }
            }
        }

        HideShowEventMarks(Show: true);
    }

    [ContextMenu("Export data")]
    public string ExportData()
    {
        string empty = string.Empty;
        RoutesData routesData = GetRoutesData();
        return SerializedString = XmlSerialization.SerializeData<RoutesData>(routesData);
    }

    public void ImportData(string XmlString)
    {
        RoutesData routesData = new RoutesData();
        routesData = (RoutesData)XmlSerialization.DeserializeData<RoutesData>(XmlString);
        SetRoutesData(routesData);
    }

    [ContextMenu("Import test data")]
    public void ImportTestData()
    {
        ImportData(SerializedString);
    }
}