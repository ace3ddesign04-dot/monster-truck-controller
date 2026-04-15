using CustomVP;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class TrailRaceManager : MonoBehaviour
{
	public enum TrailRaceState
	{
		Connected,
		WaitingForOtherPlayer,
		WaitingForReadiness,
		Countdown,
		Racing
	}

	public enum CompetitionState
	{
		None,
		Lost,
		Won,
		Disqualified
	}

	public static TrailRaceManager Instance;

	[HideInInspector]
	public TrailRaceState trailRaceState;

	[HideInInspector]
	public CompetitionState competitionState;

	public bool Initialized;

	private Route currentRoute;

	private int countdownTime = 3;

	private bool OtherPlayerReady;

	private bool ImReady;

	private bool CurrentTrailFound;

	private bool OtherPlayerTotallyLoaded;

	private bool ShowMessageContiniously;

	private string Message;

	private bool RaceStarted;

	private float raceTimer;

	private int CurrentCheckpoint;

	private bool AllCheckpointsPassed;

	private float OpponentTime;

	private bool OpponentFinished;

	private bool iFinished;

	private bool IAmHost;

	private LightSet myLightSet;

	private LightSet opponentLightSet;

	private string OpponentName;

	private Coroutine Timer;

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

	private string RaceResultText
	{
		get
		{
			switch (competitionState)
			{
			case CompetitionState.Disqualified:
				return "You're disqualified!";
			case CompetitionState.Lost:
				return "You lost!";
			case CompetitionState.Won:
				return "You won!";
			default:
				return string.Empty;
			}
		}
	}

	public TrailRaceManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Instance = this;
		if (GameState.GameType != GameType.TrailRace)
		{
			base.enabled = false;
			return;
		}
		SetTrailRaceState(TrailRaceState.Connected);
		IAmHost = (PhotonNetwork.room.PlayerCount < 2);
	}

	public void OnOtherPlayerTotallyLoaded(string name)
	{
		UnityEngine.Debug.Log("Other player loaded");
		OtherPlayerTotallyLoaded = true;
		PhotonNetwork.room.IsOpen = false;
		OpponentName = name;
		if (opponentLightSet != null)
		{
			opponentLightSet.SetLightState(LightState.PreStage);
		}
		CarUIControl.Instance.HideShowRestartOffering(Show: false);
		CarUIControl.Instance.HideShowOfferRestartButton(Show: true);
		CarUIControl.Instance.HideShowWaitForOtherPlayerButton(Show: false);
		CarUIControl.Instance.HideShowSpectateButton(Show: true);
	}

	public void OnOtherPlayerDisconnected()
	{
		CarUIControl.Instance.HideShowRestartOffering(Show: false);
		CarUIControl.Instance.HideShowOfferRestartButton(Show: false);
		CarUIControl.Instance.HideShowWaitForOtherPlayerButton(Show: true);
		CarUIControl.Instance.HideShowSpectateButton(Show: false);
		CameraController.Instance.forcedTarget = null;
		CameraController.Instance.cameraMode = CameraController.CameraMode.Follow;
		OtherPlayerTotallyLoaded = false;
		if (trailRaceState == TrailRaceState.WaitingForReadiness)
		{
			InitializeRace();
			return;
		}
		if (!iFinished)
		{
			CarUIControl.Instance.HideShowOtherPlayerDisconnectedWindow(Show: true);
			ToggleUI(show: false);
			carController.vehicleIsActive = false;
		}
		else
		{
			if (competitionState == CompetitionState.None)
			{
				competitionState = CompetitionState.Won;
			}
			if (OpponentTime == 0f)
			{
				OpponentTime = -1f;
			}
			CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: true, RaceResultText, raceTimer, OpponentTime, GameState.playerName, OpponentName);
			CarUIControl.Instance.HideShowSpectateButton(Show: false);
			CarUIControl.Instance.HideShowOfferRestartButton(Show: false);
		}
		if (Timer != null)
		{
			StopCoroutine(Timer);
			Timer = null;
		}
	}

	public void ImTotallyLoaded()
	{
		photonTransformView.ImTotallyLoaded();
	}

	private IEnumerator DoLosingPlayerTimer()
	{
		for (int i = 30; i > 0; i--)
		{
			CarUIControl.Instance.ShowNotification(i.ToString(), blinking: false);
			yield return new WaitForSeconds(1f);
		}
		LosingPlayerTimerOver();
	}

	private void LosingPlayerTimerOver()
	{
		carController.vehicleIsActive = false;
		RaceStarted = false;
		iFinished = true;
		competitionState = CompetitionState.Disqualified;
		CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: true, RaceResultText, -1f, OpponentTime, GameState.playerName, OpponentName);
		CarUIControl.Instance.HideShowOfferRestartButton(OpponentFinished);
		CarUIControl.Instance.HideShowSpectateButton(Show: false);
		raceTimer = -1f;
		photonTransformView.iFinishedTrailRace(raceTimer);
		Timer = null;
	}

	public void OnOtherPlayerFinished(float hisTime)
	{
		OpponentTime = hisTime;
		OpponentFinished = true;
		if (competitionState == CompetitionState.None && iFinished)
		{
			if (OpponentTime != -1f)
			{
				competitionState = ((!(raceTimer < OpponentTime)) ? CompetitionState.Lost : CompetitionState.Won);
			}
			else
			{
				competitionState = CompetitionState.Won;
			}
		}
		CarUIControl.Instance.HideShowSpectateButton(Show: false);
		if (iFinished)
		{
			CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: true, RaceResultText, raceTimer, OpponentTime, GameState.playerName, OpponentName);
		}
		else
		{
			CarUIControl.Instance.ShowNotification(OpponentName + " finished!", blinking: false);
			Timer = StartCoroutine(DoLosingPlayerTimer());
		}
		CarUIControl.Instance.HideShowOfferRestartButton(iFinished && OpponentFinished);
		CarUIControl.Instance.HideShowRestartOffering(Show: false);
	}

	public void InitializeRace()
	{
		ToggleUI(show: false);
		if (currentRoute == null)
		{
			int trailID = GameState.TrailID;
			string trailName = Trails.trails[trailID].TrailName;
			UnityEngine.Debug.Log("Looking for route: " + trailName);
			Route[] array = UnityEngine.Object.FindObjectsOfType<Route>();
			foreach (Route route in array)
			{
				if (route.MultiplayerTrail && route.MultiplayerName == trailName)
				{
					currentRoute = route;
					CurrentTrailFound = true;
					break;
				}
			}
		}
		CameraController.Instance.forcedTarget = null;
		CameraController.Instance.cameraMode = CameraController.CameraMode.Follow;
		CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: false);
		CarUIControl.Instance.HideShowOtherPlayerDisconnectedWindow(Show: false);
		CarUIControl.Instance.ToggleReadyToRaceWindow(Show: false);
		if (PhotonNetwork.room.PlayerCount < 2)
		{
			IAmHost = true;
		}
		PhotonNetwork.room.IsOpen = (PhotonNetwork.room.PlayerCount < 2);
		Transform transform = (!IAmHost) ? currentRoute.Player2StartPos : currentRoute.Player1StartPos;
		carController.transform.position = transform.position + Vector3.up * 2f;
		carController.transform.rotation = transform.rotation;
		carController.vehicleIsActive = false;
		playerRigidbody.velocity = Vector3.zero;
		playerRigidbody.angularVelocity = Vector3.zero;
		SetupLightTree();
		myLightSet.SetLightState(LightState.PreStage);
		opponentLightSet.SetLightState(LightState.ShutAll);
		if (Timer != null)
		{
			StopCoroutine(Timer);
			Timer = null;
		}
		competitionState = CompetitionState.None;
		OtherPlayerReady = false;
		ImReady = false;
		RaceStarted = false;
		raceTimer = 0f;
		CurrentCheckpoint = 0;
		AllCheckpointsPassed = false;
		OpponentTime = 0f;
		iFinished = false;
		OpponentFinished = false;
		SetTrailRaceState(TrailRaceState.WaitingForOtherPlayer);
		Initialized = true;
	}

	private void SetupLightTree()
	{
		if (PhotonNetwork.room.PlayerCount < 2)
		{
			IAmHost = true;
		}
		myLightSet = ((!IAmHost) ? currentRoute.lightTree.Player2Lights : currentRoute.lightTree.Player1Lights);
		opponentLightSet = ((!IAmHost) ? currentRoute.lightTree.Player1Lights : currentRoute.lightTree.Player2Lights);
	}

	public void CollidedWithCheckpoint(Checkpoint cp)
	{
		if (cp.ID == CurrentCheckpoint)
		{
			NextCheckpoint();
		}
	}

	private void NextCheckpoint()
	{
		CurrentCheckpoint++;
		if (CurrentCheckpoint < currentRoute.SpawnedCheckpoints.Count)
		{
			CarUIControl.Instance.CurrentCheckpoint = currentRoute.SpawnedCheckpoints[CurrentCheckpoint].transform;
		}
		else
		{
			AllCheckpointsPassed = true;
			if (currentRoute.SpawnedFinish != null)
			{
				CarUIControl.Instance.CurrentCheckpoint = currentRoute.SpawnedFinish.transform;
			}
			else
			{
				CarUIControl.Instance.CurrentCheckpoint = currentRoute.SpawnedStart.transform;
			}
		}
		currentRoute.ShowCheckpoint(CurrentCheckpoint);
		CarUIControl.Instance.ShowNotification("Checkpoint", blinking: false);
	}

	private void CheckFinish()
	{
		if (Vector3.Distance(carController.transform.position, currentRoute.SpawnedFinish.transform.position) < 5f && AllCheckpointsPassed && RaceStarted)
		{
			Finish();
		}
	}

	private void SetTrailRaceState(TrailRaceState state)
	{
		trailRaceState = state;
		ShowMessageContiniously = false;
		switch (trailRaceState)
		{
		case TrailRaceState.Connected:
			break;
		case TrailRaceState.WaitingForOtherPlayer:
			myLightSet.SetLightState(LightState.PreStage);
			ToggleUI(show: false);
			CarUIControl.Instance.ToggleCarExtras(Show: true);
			Message = "Waiting for other player...";
			ShowMessageContiniously = true;
			break;
		case TrailRaceState.WaitingForReadiness:
			CarUIControl.Instance.ToggleReadyToRaceWindow(Show: true);
			ToggleUI(show: false);
			CarUIControl.Instance.ToggleCarExtras(Show: true);
			break;
		case TrailRaceState.Countdown:
			ToggleUI(show: true);
			StartCoroutine(Countdown(countdownTime));
			CarUIControl.Instance.CurrentCheckpoint = currentRoute.SpawnedCheckpoints[0].transform;
			if (CurrentTrailFound)
			{
				currentRoute.HideShowEventMark(Show: false);
				currentRoute.HideShowAllCheckpoints(Show: true);
				currentRoute.HideShowStartAndFinish(Show: true);
			}
			RacingManager.Instance.HideShowEventMarks(Show: false);
			break;
		case TrailRaceState.Racing:
			RaceStarted = true;
			carController.vehicleIsActive = true;
			CarUIControl.Instance.ShowNotification("GO!!!", blinking: false);
			break;
		}
	}

	private void PlayerIsReady()
	{
		UnityEngine.Debug.Log("PlayerIsReady");
		ImReady = true;
		photonTransformView.ImReadyToRace();
		myLightSet.SetLightState(LightState.Stage);
		CarUIControl.Instance.ToggleReadyToRaceWindow(Show: false);
		CheckReadiness();
	}

	public void OnOtherPlayerReady()
	{
		UnityEngine.Debug.Log("OnOtherPlayerReady");
		OtherPlayerReady = true;
		CheckReadiness();
		if (opponentLightSet == null)
		{
			SetupLightTree();
		}
		opponentLightSet.SetLightState(LightState.Stage);
	}

	private void CheckReadiness()
	{
		UnityEngine.Debug.Log("CheckReadiness");
		if (ImReady && OtherPlayerReady)
		{
			SetTrailRaceState(TrailRaceState.Countdown);
		}
	}

	private IEnumerator Countdown(int time)
	{
		for (int i = time; i > 0; i--)
		{
			switch (i)
			{
			case 3:
				myLightSet.SetLightState(LightState.Countdown3);
				opponentLightSet.SetLightState(LightState.Countdown3);
				CarUIControl.Instance.ShowNotification("3", blinking: false);
				break;
			case 2:
				myLightSet.SetLightState(LightState.Countdown2);
				opponentLightSet.SetLightState(LightState.Countdown2);
				CarUIControl.Instance.ShowNotification("2", blinking: false);
				break;
			case 1:
				myLightSet.SetLightState(LightState.Countdown1);
				opponentLightSet.SetLightState(LightState.Countdown1);
				CarUIControl.Instance.ShowNotification("1", blinking: false);
				break;
			}
			yield return new WaitForSeconds(1f);
		}
		myLightSet.SetLightState(LightState.Start);
		opponentLightSet.SetLightState(LightState.Start);
		SetTrailRaceState(TrailRaceState.Racing);
	}

	private void ToggleUI(bool show)
	{
		if (!(CarUIControl.Instance == null))
		{
			CarUIControl.Instance.ToggleCarExtras(show);
			CarUIControl.Instance.ToggleCarControls(show);
			CarUIControl.Instance.ToggleWinchControls(show);
			CarUIControl.Instance.HideShowRaceUI(show, ShowCancelButton: false);
		}
	}

	private void Finish()
	{
		carController.vehicleIsActive = false;
		RaceStarted = false;
		iFinished = true;
		if (Timer != null)
		{
			StopCoroutine(Timer);
			Timer = null;
		}
		if (competitionState == CompetitionState.None)
		{
			if (OpponentFinished)
			{
				competitionState = ((!(raceTimer < OpponentTime)) ? CompetitionState.Lost : CompetitionState.Won);
			}
			else if (PhotonNetwork.room.PlayerCount < 2)
			{
				competitionState = CompetitionState.Won;
			}
		}
		string resultText = string.Empty;
		switch (competitionState)
		{
		case CompetitionState.None:
			resultText = "Waiting for opponent";
			break;
		case CompetitionState.Won:
			resultText = "You won!";
			break;
		case CompetitionState.Lost:
			resultText = "You lost!";
			break;
		}
		if (OpponentFinished && competitionState == CompetitionState.Won)
		{
			GameState.AddCurrency(GameState.TrailRaceBet * 2, Currency.Money);
		}
		CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: true, resultText, raceTimer, OpponentTime, GameState.playerName, OpponentName);
		CarUIControl.Instance.HideShowOfferRestartButton(iFinished && OpponentFinished && PhotonNetwork.room.PlayerCount == 2);
		CarUIControl.Instance.HideShowSpectateButton(!OpponentFinished);
		CarUIControl.Instance.HideShowRestartOffering(Show: false);
		photonTransformView.iFinishedTrailRace(raceTimer);
	}

	private void BackToGarage()
	{
		if (carController != null)
		{
			carController.GetComponent<VehicleDataManager>().SaveVehicleData();
		}
		SceneManager.LoadScene("Menu");
	}

	private void Spectate()
	{
		if (iFinished && OpponentFinished)
		{
			return;
		}
		PhotonTransformView[] array = UnityEngine.Object.FindObjectsOfType<PhotonTransformView>();
		int num = 0;
		PhotonTransformView photonTransformView;
		while (true)
		{
			if (num < array.Length)
			{
				photonTransformView = array[num];
				if (photonTransformView != this.photonTransformView)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		CameraController.Instance.forcedTarget = photonTransformView.transform;
		CameraController.Instance.cameraMode = CameraController.CameraMode.Free;
		CarUIControl.Instance.HideShoTrailRaceFinishWindow(Show: false);
		ToggleUI(show: false);
	}

	private void OfferRestart()
	{
		CarUIControl.Instance.HideShowOfferRestartButton(Show: false);
		photonTransformView.SendRestartOffering();
	}

	public void OnRestartOfferingReceived()
	{
		CarUIControl.Instance.HideShowRestartOffering(Show: true);
		CarUIControl.Instance.HideShowOfferRestartButton(Show: false);
	}

	private void AcceptRestart()
	{
		photonTransformView.SendRestartAcceptation();
		InitializeRace();
	}

	public void OnRestartAccepted()
	{
		InitializeRace();
	}

	private void ContinueRaceAlone()
	{
		CarUIControl.Instance.HideShowOtherPlayerDisconnectedWindow(Show: false);
		ToggleUI(show: true);
		carController.vehicleIsActive = true;
	}

	private void Update()
	{
		if (ShowMessageContiniously && CarUIControl.Instance != null)
		{
			CarUIControl.Instance.ShowNotification(Message, blinking: false);
		}
		if (trailRaceState == TrailRaceState.WaitingForOtherPlayer && PhotonNetwork.room.PlayerCount > 1 && MultiplayerManager.Instance.TotallyReady && OtherPlayerTotallyLoaded)
		{
			SetTrailRaceState(TrailRaceState.WaitingForReadiness);
		}
		if (CrossPlatformInputManager.GetButtonDown("ReadyToRace"))
		{
			PlayerIsReady();
		}
		if (CrossPlatformInputManager.GetButtonDown("BackToMenu"))
		{
			BackToGarage();
		}
		if (CrossPlatformInputManager.GetButtonDown("Spectate"))
		{
			Spectate();
		}
		if (CrossPlatformInputManager.GetButtonDown("OfferRestart"))
		{
			OfferRestart();
		}
		if (CrossPlatformInputManager.GetButtonDown("AcceptRestart"))
		{
			AcceptRestart();
		}
		if (CrossPlatformInputManager.GetButtonDown("WaitForAnotherPlayer"))
		{
			InitializeRace();
		}
		if (CrossPlatformInputManager.GetButtonDown("ContinueRaceAlone"))
		{
			ContinueRaceAlone();
		}
		if (RaceStarted)
		{
			raceTimer += Time.deltaTime;
			CarUIControl.Instance.UpdateTimer(raceTimer);
			CheckFinish();
		}
	}
}
