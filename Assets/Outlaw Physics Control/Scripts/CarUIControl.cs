using CustomVP;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarUIControl : MonoBehaviour
{
	public enum ControlType
	{
		Arrow,
		Tilt,
		SteeringWheel
	}

	public ControlType controlType;

	public AudioSource ToggleSound;

	public AudioSource TickingSound;

	public AudioSource BombSound;

	[Range(0f, 1f)]
	private float ToggleSoundVolume = 0.1f;

	public Text NotificationText;

	public GameObject[] GearButtons;

	public GameObject[] DiffLockButtons;

	public GameObject[] DriveButtons;

	public GameObject ChatBox;

	public GameObject WinchControls;

	public GameObject ArrowControls;

	public GameObject TiltControls;

	public GameObject SteeringWheelControls;

	public GameObject CarExtras;

	public GameObject FlipButton;

	public GameObject PausePanel;

	public GameObject LoadingScreen;

	public GameObject MessageBox;

	public GameObject ReadyToRaceWindow;

	public GameObject RepairButton;

	public Text RepairCostText;

	public GameObject LandAnchorButton;

	public GameObject RepairWindow;

	public Text MessageText;

	public Text LoadingScreenLabel;

	public GameObject CaptureTheFlagGameOverMessage;

	public Text CaptureTheFlagGameOverText;

	public GameObject PasswordPanel;

	public Text MapPassword;

	public AudioSource Ding;

	public GameObject MultiplayerLabelsButton;

	public GameObject OtherPlayerDisconnectedWindow;

	public GameObject RammingWindow;

	public GameObject[] TouchAccelerators;

	public GameObject[] SlideAccelerators;

	public GameObject[] EbrakeButtons;

	public GameObject[] GearShifters;

	public Image DirectionalArrow;

	public Transform DirectionalArrowTarget;

	public Text RammingPlayerName;

	public Text swapButtonText;

	public List<Image> DirectionalArrows = new List<Image>();

	public List<Image> DirectionalArrowsPool = new List<Image>();

	public Image LockboxArrow;

	public List<Text> PlayerNames = new List<Text>();

	[Header("Gauges")]
	public GameObject MainGauge;

	public Text SpeedText;

	public Text[] GearTexts;

	[Space(10f)]
	public float TachoMinAngle;

	public float TachoMaxAngle;

	public float MinRevs;

	public float MaxRevs;

	[Space(10f)]
	public RectTransform RevsPointer;

	[Space(10f)]
	public float ThermometerMinAngle;

	public float ThermometerMaxAngle;

	public RectTransform TemperatureArrow;

	[Space(10f)]
	public Image HPBar;

	[Space(10f)]
	public RectTransform InclinometerHolder;

	public RectTransform InclinometerBG;

	public float InclinometerMaxY;

	public float InclinometerMaxZ;

	private bool CarControlsEnabled;

	public GameObject HideGaugeImage;

	[Header("Winch")]
	public GameObject ToggleButton;

	public GameObject TowButton;

	public GameObject LeftArrowButton;

	public GameObject RightArrowButton;

	public GameObject AttachButton;

	public GameObject SendWinchRequestButton;

	public GameObject WinchRequestWindow;

	public Text WinchRequestText;

	public GameObject DetachAttachedCarButton;

	private bool notificationBlinking;

	[Header("Trailers")]
	public GameObject loadOnOtherPlayerTrailerButton;

	public GameObject unloadFromOtherPlayerTrailerButton;

	public GameObject detachTrailerButton;

	public GameObject attachTrailerButton;

	public Button swapVehiclesButton;

	public GameObject waitingForLoadOnTrailerResponseWindow;

	public GameObject traileringRequestWindow;

	[Header("Race controls")]
	public GameObject EventLobby;

	public GameObject InRaceUI;

	public GameObject FinishInfo;

	public GameObject Countdown;

	public Text RaceTimeText;

	public Text WinchUsedText;

	public Text FlipsText;

	public Text FinishText;

	public Text LapText;

	public Text RecordTime;

	public Text[] AwardTimes;

	public Text[] AwardAmounts;

	public Text TrailRaceResultText;

	public Text TrailRaceOpponentTimeText;

	public Text TrailRaceMyTimeText;

	public Text RecordKeeperText;

	public Text TrailIDText;

	public Text RecordDateText;

	public Text FinishTime;

	public Text RecordTimeFinished;

	public Text AwardLevel;

	public Text MoneyWon;

	public Text GoldWon;

	public Text XPWon;

	public Text RaceBetText;

	public Text OpponentLeftRewardText;

	public Text PlayerName;

	public Text OpponentName;

	public Text DateText;

	public GameObject RaceCancelButton;

	public GameObject TrailblazerLabel;

	public GameObject TrailRaceFinishWindow;

	public GameObject OfferRestartButton;

	public GameObject RestartOfferingWindow;

	public GameObject WaitForOtherPlayerButton;

	public GameObject SpectateButton;

	public GameObject LeftSideArrow;

	public GameObject RightSideArrow;

	public Image CheckpointArrow;

	public Text CountdownText;

	public GameObject Map;

	public NavigationMap MapScript;

	[HideInInspector]
	public Transform CurrentCheckpoint;

	public static CarUIControl Instance;

	[Header("Player routes")]
	public GameObject playerRouteFinishWindow;

	public Text playerRouteTime;

	public Text playerRouteRecord;

	public Text playerRouteRecordKeeper;

	public Text playerRouteAwardLevel;

	public Text playerRouteXP;

	public Text playerRouteGolds;

	public Text playerRouteMoney;

	public GameObject[] PlayerInformationBoxes = new GameObject[0];

	public GameObject[] PlayerInformationBoxesPool = new GameObject[10];

	private GameObject PlayerInformationTemplate;

	private bool ShowMultiplayerLabels = true;

	public Font font;

	public CarController carController;
	//private CarController carController
	//{
	//	get
	//	{
	//		if (VehicleLoader.Instance != null)
	//		{
	//			return VehicleLoader.Instance.playerCarController;
	//		}
	//		return null;
	//	}
	//}

	public EngineController engine;
	//private EngineController engine
	//{
	//	get
	//	{
	//		if (VehicleLoader.Instance != null)
	//		{
	//			return VehicleLoader.Instance.playerEngine;
	//		}
	//		return null;
	//	}
	//}

	private RacingManager racingManager => RacingManager.Instance;

	private CameraController camController => CameraController.Instance;

	private void Awake()
	{
		Instance = this;
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			LoadingScreenLabel.text = "Joining other Outlaws...";
		}
		else
		{
			LoadingScreenLabel.text = string.Empty;
		}
		if (SceneManager.GetActiveScene().name == "CustomMap")
		{
			LoadingScreenLabel.text = "Level editor builds your level, it may take some time. Wait...";
		}
		ChatBox.SetActive(GameState.GameMode == GameMode.Multiplayer);
	}

	private void Start()
	{
		MapScript = Map.GetComponent<NavigationMap>();
		Color color = NotificationText.color;
		color.a = 0f;
		NotificationText.color = color;
		ToggleCarControls(Show: true);
		ToggleCarExtras(Show: true);
		ToggleWinchControls(Show: true);
		HideEventLobby();
		HideShowCountdown(Show: false);
		HideShowRaceUI(Show: false, ShowCancelButton: false);
		SwitchWinchTowButton(Show: false);
		SwitchWinchTargetSelector(Show: false);
		SwitchFlipButton(Show: false);
		HideWinchRequestWindow();
		ToggleAttachButton(DynamicTarget: false, Show: false);
		SwitchDetachButton(Show: false);
		PasswordPanel.SetActive(value: false);
		ToggleReadyToRaceWindow(Show: false);
		HideShoTrailRaceFinishWindow(Show: false, string.Empty, 0f, 0f, string.Empty, string.Empty);
		HideShowOtherPlayerDisconnectedWindow(Show: false);
		HidePlayerRouteFinish();
		detachTrailerButton.SetActive(value: false);
		attachTrailerButton.SetActive(value: false);
		swapVehiclesButton.gameObject.SetActive(value: false);
		loadOnOtherPlayerTrailerButton.SetActive(value: false);
		unloadFromOtherPlayerTrailerButton.SetActive(value: false);
		traileringRequestWindow.SetActive(value: false);
		waitingForLoadOnTrailerResponseWindow.SetActive(value: false);
		RepairWindow.SetActive(value: false);
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			if (GameState.GameType == GameType.TrailRace)
			{
				RaceBetText.gameObject.SetActive(value: true);
				string text = "$" + GameState.TrailRaceBet.ToString();
				if (text == "$0")
				{
					text = "FREE RACE";
				}
				RaceBetText.text = "Race bet: " + text;
			}
			if (GameState.Password != null && GameState.Password != string.Empty)
			{
				PasswordPanel.SetActive(value: true);
				MapPassword.text = "Map password: " + GameState.Password;
			}
		}
		GameObject[] touchAccelerators = TouchAccelerators;
		foreach (GameObject gameObject in touchAccelerators)
		{
			gameObject.SetActive(!DataStore.GetBool("SlideAccelerator"));
		}
		GameObject[] slideAccelerators = SlideAccelerators;
		foreach (GameObject gameObject2 in slideAccelerators)
		{
			gameObject2.SetActive(DataStore.GetBool("SlideAccelerator"));
		}
		ArrowControls.SetActive(value: false);
		SteeringWheelControls.SetActive(value: false);
		TiltControls.SetActive(value: false);
		controlType = (ControlType)DataStore.GetInt("ControlsType", 0);
		ToggleCarControls(Show: true);
		if (Map != null)
		{
			Map.SetActive(value: false);
		}
		if (DataStore.GetInt("GameSound", 1) == 0)
		{
			AudioListener.volume = 0f;
		}
		else
		{
			AudioListener.volume = 1f;
		}
		PlayerInformationTemplate = Resources.Load<GameObject>("UI/PlayerInfoPanel");
		if (GameState.GameMode != GameMode.Multiplayer)
		{
			MultiplayerLabelsButton.SetActive(value: false);
		}
		else
		{
			MultiplayerLabelsButton.SetActive(value: true);
		}
		for (int k = 0; k < 10; k++)
		{
			PlayerInformationBoxesPool[k] = UnityEngine.Object.Instantiate(PlayerInformationTemplate, base.transform);
			PlayerInformationBoxesPool[k].SetActive(value: false);
		}
		for (int l = 0; l < 10; l++)
		{
			DirectionalArrowsPool.Add(UnityEngine.Object.Instantiate(DirectionalArrow, DirectionalArrow.transform.parent));
			DirectionalArrowsPool[l].gameObject.SetActive(value: false);
		}
	}

	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			ToggleCarControls(CarControlsEnabled);
		}
	}

	public void ToggleMultiplayerLabels()
	{
		ShowMultiplayerLabels = !ShowMultiplayerLabels;
	}

	public void ToggleMap()
	{
		Map.SetActive(!Map.activeSelf);
	}

	public void ToggleEbrake(bool Show)
	{
		GameObject[] ebrakeButtons = EbrakeButtons;
		foreach (GameObject gameObject in ebrakeButtons)
		{
			gameObject.SetActive(Show);
		}
	}

	public void ToggleGearShifter(bool Show)
	{
		GameObject[] gearShifters = GearShifters;
		foreach (GameObject gameObject in gearShifters)
		{
			gameObject.SetActive(Show);
		}
		UnityEngine.Debug.Log("TOGGLING GEAR SHIFTER:" + Show);
	}

	private void Update()
	{
		Color color = NotificationText.color;
		color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
		if (notificationBlinking && color.a == 0f)
		{
			color.a = 1f;
		}
		NotificationText.color = color;
		if (engine != null && carController != null)
		{
			DoGauges();
		}
		if (CurrentCheckpoint != null)
		{
			DoCheckpointArrow();
		}
		if (StashManager.Instance != null)
		{
			if (StashManager.Instance.LockboxActive)
			{
				if (LockboxArrow == null)
				{
					LockboxArrow = UnityEngine.Object.Instantiate(DirectionalArrow, DirectionalArrow.transform.parent);
					LockboxArrow.gameObject.SetActive(value: true);
					LockboxArrow.color = new Color(1f, 0f, 0f, 0.8f);
				}
				if (LockboxArrow != null)
				{
					DoDirectionalArrow(LockboxArrow, StashManager.Instance.CurrentLockbox.transform);
					LockboxArrow.gameObject.SetActive(value: true);
				}
			}
			else if (LockboxArrow != null && LockboxArrow.gameObject.activeInHierarchy)
			{
				LockboxArrow.gameObject.SetActive(value: false);
			}
		}
		if (GameState.GameMode == GameMode.Multiplayer && MultiplayerManager.CurrentPlayers != null && MapScript != null)
		{
			if (!ShowMultiplayerLabels)
			{
				for (int i = 0; i < PlayerInformationBoxes.Length; i++)
				{
					if (PlayerInformationBoxes[i] != null && PlayerInformationBoxes[i].activeInHierarchy)
					{
						PlayerInformationBoxes[i].SetActive(value: false);
					}
				}
			}
			else
			{
				if (PlayerInformationBoxes.Length != MultiplayerManager.CurrentPlayers.Count)
				{
					for (int j = 0; j < PlayerInformationBoxes.Length; j++)
					{
						if (PlayerInformationBoxes[j] != null)
						{
							PlayerInformationBoxes[j].SetActive(value: false);
						}
					}
					PlayerInformationBoxes = new GameObject[MultiplayerManager.CurrentPlayers.Count];
					for (int k = 0; k < MultiplayerManager.CurrentPlayers.Count; k++)
					{
						PlayerInformationBoxes[k] = PlayerInformationBoxesPool[k];
						PlayerInformationBoxes[k].GetComponent<PlayerInfoUI>().Populate("------", 0, isMember: false);
						if (MultiplayerManager.CurrentPlayerViews[k] != null)
						{
							string name = "------";
							int xp = 0;
							bool isMember = false;
							if (MultiplayerManager.CurrentPlayerViews != null && MultiplayerManager.CurrentPlayerViews.Length > k && MultiplayerManager.CurrentPlayerViews[k] != null && MultiplayerManager.CurrentPlayerViews[k].owner != null && MultiplayerManager.CurrentPlayerViews[k].owner.CustomProperties != null)
							{
								Hashtable customProperties = MultiplayerManager.CurrentPlayerViews[k].owner.CustomProperties;
								if (customProperties.ContainsKey("XP"))
								{
									xp = int.Parse(customProperties["XP"].ToString());
								}
								if (customProperties.ContainsKey("IsMember"))
								{
									isMember = bool.Parse(customProperties["IsMember"].ToString());
								}
								if (customProperties.ContainsKey("DisplayName"))
								{
									name = customProperties["DisplayName"].ToString();
								}
							}
							PlayerInformationBoxes[k].GetComponent<PlayerInfoUI>().Populate(name, xp, isMember);
						}
						else
						{
							PlayerInformationBoxes[k].SetActive(value: false);
						}
					}
				}
				for (int l = 0; l < PlayerInformationBoxes.Length && l < MultiplayerManager.CurrentPlayers.Count; l++)
				{
					if (MultiplayerManager.CurrentPlayers[l] != null)
					{
						Vector3 vector = Camera.main.WorldToScreenPoint(MultiplayerManager.CurrentPlayers[l].transform.position + Vector3.up * 1.3f);
						if (PlayerInformationBoxes[l] != null && vector.z > 0f && carController != null && Vector3.Distance(MultiplayerManager.CurrentPlayers[l].transform.position, carController.transform.position) < 15f)
						{
							PlayerInformationBoxes[l].transform.position = new Vector3(vector.x, vector.y, 0f);
							PlayerInformationBoxes[l].SetActive(value: true);
						}
						else if (PlayerInformationBoxes[l] != null)
						{
							PlayerInformationBoxes[l].SetActive(value: false);
						}
					}
					else if (PlayerInformationBoxes[l] != null)
					{
						PlayerInformationBoxes[l].SetActive(value: false);
					}
				}
			}
			if (MapScript.OtherCars.Length != MultiplayerManager.CurrentPlayers.Count)
			{
				UnityEngine.Debug.Log("Count didn't match, reinitializing array");
				MapScript.OtherCars = new Transform[MultiplayerManager.CurrentPlayers.Count];
			}
			else
			{
				List<Transform> list = new List<Transform>();
				for (int m = 0; m < MultiplayerManager.CurrentPlayers.Count; m++)
				{
					bool flag = false;
					for (int n = 0; n < MapScript.OtherCars.Length; n++)
					{
						if (MultiplayerManager.CurrentPlayers[m] != null && MapScript.OtherCars[n] == MultiplayerManager.CurrentPlayers[m].transform)
						{
							flag = true;
						}
					}
					if (!flag && MultiplayerManager.CurrentPlayers[m] != null)
					{
						list.Add(MultiplayerManager.CurrentPlayers[m].transform);
					}
				}
				if (list.Count > 0)
				{
					for (int num = 0; num < MultiplayerManager.CurrentPlayers.Count; num++)
					{
						if (MultiplayerManager.CurrentPlayers[num] != null)
						{
							MapScript.OtherCars[num] = MultiplayerManager.CurrentPlayers[num].transform;
						}
					}
				}
			}
		}
		if (GameState.GameMode == GameMode.Multiplayer && MultiplayerManager.CurrentPlayers != null && MultiplayerManager.CurrentPlayers.Count > 0 && MultiplayerManager.CurrentPlayers[0] != null)
		{
			if (MultiplayerManager.CurrentPlayers.Count != DirectionalArrows.Count)
			{
				for (int num2 = 0; num2 < DirectionalArrows.Count; num2++)
				{
					if (DirectionalArrows[num2] != null)
					{
						DirectionalArrows[num2].gameObject.SetActive(value: false);
					}
				}
				DirectionalArrows.Clear();
				for (int num3 = 0; num3 < MultiplayerManager.CurrentPlayers.Count; num3++)
				{
					DirectionalArrows.Add(DirectionalArrowsPool[num3]);
					DirectionalArrows[DirectionalArrows.Count - 1].gameObject.SetActive(value: true);
					DirectionalArrows[DirectionalArrows.Count - 1].color = new Color(0f, 1f, 0f, 0.8f);
				}
			}
			for (int num4 = 0; num4 < DirectionalArrows.Count && num4 < MultiplayerManager.CurrentPlayers.Count; num4++)
			{
				if (MultiplayerManager.CurrentPlayers[num4] != null)
				{
					DoDirectionalArrow(DirectionalArrows[num4], MultiplayerManager.CurrentPlayers[num4].transform);
				}
				else if (DirectionalArrows[num4] != null)
				{
					DirectionalArrows[num4].gameObject.SetActive(value: false);
				}
			}
		}
		else
		{
			DirectionalArrow.gameObject.SetActive(value: false);
			foreach (Image directionalArrow in DirectionalArrows)
			{
				if (directionalArrow != null)
				{
					directionalArrow.gameObject.SetActive(value: false);
				}
			}
		}
	}

	public void ShowMessage(string text)
	{
		MessageBox.SetActive(value: true);
		MessageText.text = text;
	}

	public void HideMessage()
	{
		MessageBox.SetActive(value: false);
	}

	private void DoGauges()
	{
		SpeedText.text = Mathf.Abs((int)carController.Speed).ToString();
		RevsPointer.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(TachoMinAngle, TachoMaxAngle, Mathf.InverseLerp(MinRevs, MaxRevs, Mathf.Abs(engine.RPM))));
		HPBar.fillAmount = carController.CarHealth / 100f;
		InclinometerBG.localPosition = new Vector3(0f, Mathf.LerpUnclamped(-30f, 0f, carController.LongTilt + 1f), 0f);
		InclinometerHolder.eulerAngles = new Vector3(0f, 0f, Mathf.LerpUnclamped(-90f, 0f, carController.LatTilt + 1f));
	}

	public void LockboxCountdown(float timeLeft)
	{
		if (!TickingSound.isPlaying)
		{
			TickingSound.Play();
		}
		TickingSound.pitch = 1f + Mathf.Clamp(1f / timeLeft, 0.01f, 4f);
	}

	public void LockboxBomb()
	{
		BombSound.Play();
	}

	private void DoCheckpointArrow()
	{
		Vector3 vector = carController.transform.InverseTransformDirection(CurrentCheckpoint.position - carController.transform.position);
		float z = Mathf.Atan2(0f - vector.x, vector.z) * 57.29578f;
		CheckpointArrow.rectTransform.eulerAngles = new Vector3(0f, 0f, z);
		Vector3 vector2 = Camera.main.WorldToViewportPoint(CurrentCheckpoint.position);
		LeftSideArrow.SetActive(vector2.x < 0f);
		RightSideArrow.SetActive(vector2.x > 1f);
	}

	[ContextMenu("Fix fonts")]
	private void FixFonts()
	{
		Text[] componentsInChildren = GetComponentsInChildren<Text>(includeInactive: true);
		foreach (Text text in componentsInChildren)
		{
			text.font = font;
		}
	}

	private void DoDirectionalArrow(Image arrow, Transform target)
	{
		Vector3 vector = Camera.main.transform.InverseTransformDirection(target.position - Camera.main.transform.position);
		float z = Mathf.Atan2(0f - vector.x, vector.z) * 57.29578f;
		arrow.rectTransform.eulerAngles = new Vector3(0f, 0f, z);
		arrow.gameObject.SetActive(value: true);
	}

	public void SetCurrentGear(int number)
	{
		Text[] gearTexts = GearTexts;
		foreach (Text text in gearTexts)
		{
			text.text = number.ToString();
			if (number == -1)
			{
				text.text = "N";
			}
			if (number == -2)
			{
				text.text = "R";
			}
		}
	}

	public void SetupGearButton(int SelectedPosition)
	{
		for (int i = 0; i < GearButtons.Length; i++)
		{
			GearButtons[i].SetActive(i == SelectedPosition);
		}
	}

	public void SetupDiffLockButton(int SelectedPosition)
	{
		for (int i = 0; i < DiffLockButtons.Length; i++)
		{
			DiffLockButtons[i].SetActive(i == SelectedPosition);
		}
	}

	public void SetupDriveButton(int SelectedPosition)
	{
		for (int i = 0; i < DriveButtons.Length; i++)
		{
			DriveButtons[i].SetActive(i == SelectedPosition);
		}
	}

	public void HideAllDrivetrainOptions()
	{
		for (int i = 0; i < GearButtons.Length; i++)
		{
			GearButtons[i].SetActive(value: false);
		}
		for (int j = 0; j < DiffLockButtons.Length; j++)
		{
			DiffLockButtons[j].SetActive(value: false);
		}
		for (int k = 0; k < DriveButtons.Length; k++)
		{
			DriveButtons[k].SetActive(value: false);
		}
	}

	public void SwitchWinchTargetSelector(bool Show)
	{
		LeftArrowButton.SetActive(Show);
		RightArrowButton.SetActive(Show);
		LandAnchorButton.SetActive(Show);
	}

	public void ToggleAttachButton(bool DynamicTarget, bool Show)
	{
		AttachButton.SetActive(Show && !DynamicTarget);
		SendWinchRequestButton.SetActive(Show && DynamicTarget);
	}

	public void ShowWinchRequestWindow(string text)
	{
		WinchRequestWindow.SetActive(value: true);
		WinchRequestText.text = text;
	}

	private void HideWinchRequestWindow()
	{
		WinchRequestWindow.SetActive(value: false);
	}

	public void SwitchWinchToggleButton(bool Show)
	{
		ToggleButton.SetActive(Show);
	}

	public void ToggleRepairButton(bool Show)
	{
		RepairButton.SetActive(Show);
	}

	public void SwitchWinchTowButton(bool Show)
	{
		TowButton.SetActive(Show);
	}

	public void SwitchFlipButton(bool Show)
	{
		FlipButton.SetActive(Show);
	}

	public void SwitchDetachButton(bool Show)
	{
		DetachAttachedCarButton.SetActive(Show);
	}

	public void UpdateTimer(float Seconds)
	{
		RaceTimeText.text = $"{Mathf.Floor(Seconds / 60f):0}:{Mathf.Floor(Seconds) % 60f:00}:{Mathf.Floor(Seconds * 10f % 10f):0}";
	}

	public void UpdateSwapButtonText(int swapTimer)
	{
		swapButtonText.text = "Swap vehicles";
		if (swapTimer > 0)
		{
			Text text = swapButtonText;
			string text2 = text.text;
			text.text = text2 + " (" + swapTimer + ")";
		}
		swapVehiclesButton.interactable = (swapTimer == 0);
	}

	public void UpdateThermometer(float TemperatureRatio)
	{
		TemperatureArrow.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(ThermometerMinAngle, ThermometerMaxAngle, TemperatureRatio));
	}

	public void UpdateWinchUsedText(int WinchUsedTimes)
	{
		WinchUsedText.text = "Winch used " + WinchUsedTimes + " times";
	}

	public void UpdateLapText(int Lap, int LapsNumber)
	{
		LapText.text = "Lap " + Lap + "/" + LapsNumber;
	}

	public void ShowCountdownText(int Seconds)
	{
		CountdownText.text = Seconds.ToString();
	}

	public void HideShowCountdown(bool Show)
	{
		Countdown.SetActive(Show);
	}

	public void HideShowOtherPlayerDisconnectedWindow(bool Show)
	{
		OtherPlayerDisconnectedWindow.SetActive(Show);
		OpponentLeftRewardText.text = "Your opponent left!";
	}

	public void ToggleReadyToRaceWindow(bool Show)
	{
		ReadyToRaceWindow.SetActive(Show);
	}

	public void HideShowRaceUI(bool Show, bool ShowCancelButton)
	{
		InRaceUI.SetActive(Show);
		RaceCancelButton.SetActive(ShowCancelButton);
	}

	public void ShowRammingWindow(string playerName)
	{
		RammingPlayerName.text = "Looks like player " + playerName + " is ramming you. Block collisions with him?";
		RammingWindow.SetActive(value: true);
	}

	public void ToggleRearView()
	{
		camController.ForceRearView = !camController.ForceRearView;
	}

	public void RepairVehicle()
	{
		StatsData statsData = GameState.LoadStatsData();
		int num = (int)(1000f * (100f - carController.CarHealth) / 100f);
		if (statsData.Money > num)
		{
			carController.CarHealth = 100f;
			GameState.SubtractCurrency(num, Currency.Money);
		}
		else if (statsData.Gold > Utility.CashToGold(num))
		{
			carController.CarHealth = 100f;
			GameState.SubtractCurrency(Utility.CashToGold(num), Currency.Gold);
		}
		else
		{
			ShowNotification("Not enough money!", blinking: false);
		}
	}

	public void CalculateRepairCost()
	{
		int num = (int)(1000f * (100f - carController.CarHealth) / 100f);
		RepairCostText.text = "Repair vehicle for $" + num + "?";
	}

	public void ShowEventLobby(Route route)
	{
		EventLobby.SetActive(value: true);
		RecordTime.text = "--Loading--";
		Text[] awardTimes = AwardTimes;
		foreach (Text text in awardTimes)
		{
			text.text = "----";
		}
		Text[] awardAmounts = AwardAmounts;
		foreach (Text text2 in awardAmounts)
		{
			text2.text = "----";
		}
	}

	public void DisplayEventInfo()
	{
		long @long = DataStore.GetLong(RouteManager.Instance.mapName + RacingManager.Instance.ActiveRoute.RouteName + carController.vehicleDataManager.vehicleType.ToString());
		TimeSpan timeSpan = TimeSpan.FromSeconds((float)@long / 100f);
		RecordTime.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.TotalMilliseconds / 10.0:00}";
		if (@long == -1 || @long == 0)
		{
			RouteGoal routeGoal = RouteGoal.Default(@long, RacingManager.Instance.ActiveRoute, carController.vehicleDataManager.vehicleType);
			RecordTime.text = "----";
			AwardTimes[0].text = "Finish";
			AwardAmounts[0].text = routeGoal.BaseCashPayment.ToString("$0,0");
			return;
		}
		RouteGoal routeGoal2 = null;
		routeGoal2 = ((RacingManager.Instance.ActiveRoute.RouteGoals.Count <= 0) ? RouteGoal.Default(@long, RacingManager.Instance.ActiveRoute, carController.vehicleDataManager.vehicleType) : RacingManager.Instance.ActiveRoute.RouteGoals[0]);
		for (int i = 0; i < 4; i++)
		{
			RouteGoalLimit limits = routeGoal2.GetLimits((AwardLevel)i);
			TimeSpan timeSpan2 = TimeSpan.FromSeconds((float)limits.TimeLimit / 100f);
			if (i == 0)
			{
				AwardTimes[i].text = "Finish";
			}
			else
			{
				AwardTimes[i].text = $"{timeSpan2.Minutes:D2}:{timeSpan2.Seconds:D2}.{timeSpan2.Milliseconds / 10:00}";
			}
			AwardAmounts[i].text = (routeGoal2.BaseCashPayment + routeGoal2.LevelUpCashIncrement * i).ToString("$0,0");
		}
	}

	public void ShowPause()
	{
		AudioListener.volume = 0f;
		Time.timeScale = 0f;
		PausePanel.SetActive(value: true);
	}

	public void ShowTeam(PunTeams.Team myTeam)
	{
		ShowNotification("You are on team " + ((myTeam != PunTeams.Team.blue) ? "RED" : "BLUE") + "(" + PhotonNetwork.playerList.Length + ")", blinking: false);
	}

	public void Unpause(bool exiting = true)
	{
		if (exiting && PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
		if (exiting)
		{
			AudioListener.volume = 0f;
		}
		else if (DataStore.GetInt("GameSound", 1) == 1)
		{
			AudioListener.volume = 1f;
		}
		Time.timeScale = 1f;
		LoadingScreen.SetActive(exiting);
		LoadingScreenLabel.text = "Going back home!";
		PausePanel.SetActive(value: false);
		CaptureTheFlagGameOverMessage.SetActive(value: false);
		if (racingManager != null)
		{
			racingManager.SaveVehicleData();
		}
		if (exiting)
		{
			SceneManager.LoadScene("Menu");
		}
	}

	public void CaptureTheFlagGameOver(PunTeams.Team winningTeam, PunTeams.Team myTeam)
	{
		Camera.main.GetComponent<AudioListener>().enabled = false;
		Time.timeScale = 0f;
		CaptureTheFlagGameOverText.text = ((winningTeam != PunTeams.Team.blue) ? "RED" : "BLUE") + " WON!";
		CaptureTheFlagGameOverMessage.SetActive(value: true);
	}

	public void HideEventLobby()
	{
		EventLobby.SetActive(value: false);
	}

	public void HideShowFinishWindow(bool Show, RoutePayment payment, long finishTime, long recordTime, int trailID, string recordKeeper)
	{
		FinishInfo.SetActive(Show);
		StatsData statsData = GameState.LoadStatsData();
		TimeSpan timeSpan = TimeSpan.FromSeconds((float)finishTime / 100f);
		TimeSpan timeSpan2 = TimeSpan.FromSeconds((float)recordTime / 100f);
		FinishTime.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds / 10:00}";
		RecordTimeFinished.text = $"{timeSpan2.Minutes:D2}:{timeSpan2.Seconds:D2}.{timeSpan2.Milliseconds / 10:00}";
		TrailIDText.text = trailID.ToString();
		RecordKeeperText.text = recordKeeper;
		AwardLevel.text = payment.AwardLevelString();
		MoneyWon.text = payment.Cash + ((!statsData.IsMember) ? string.Empty : "x3");
		GoldWon.text = payment.Gold + payment.TrailblazerGoldBonus + ((!statsData.IsMember) ? string.Empty : "x3");
		XPWon.text = payment.XP + ((!statsData.IsMember) ? string.Empty : "x3");
		TrailblazerLabel.SetActive(payment.Trailblazer);
		if (payment.Trailblazer)
		{
			ShowMessage("TRAILBLAZER! You were among the first to complete the route!\r\nGold Bonus: " + payment.TrailblazerGoldBonus.ToString() + ((!statsData.IsMember) ? string.Empty : "x3"));
		}
	}

	public void ToggleCarControls(bool Show)
	{
		ArrowControls.SetActive(Show && controlType == ControlType.Arrow);
		TiltControls.SetActive(Show && controlType == ControlType.Tilt);
		SteeringWheelControls.SetActive(Show && controlType == ControlType.SteeringWheel);
		CarControlsEnabled = Show;
	}

	public void HideShoTrailRaceFinishWindow(bool Show, string ResultText, float raceTime, float opponentTime, string playerName, string opponentName)
	{
		TrailRaceFinishWindow.SetActive(Show);
		TrailRaceResultText.text = ResultText;
		PlayerName.text = playerName;
		OpponentName.text = opponentName;
		DateText.text = DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + " " + Trails.GetByID(GameState.TrailID).TrailName;
		TimeSpan timeSpan = TimeSpan.FromSeconds(raceTime);
		TrailRaceMyTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds / 10:00}";
		if (raceTime == -1f)
		{
			TrailRaceMyTimeText.text = "Disqualified";
		}
		TimeSpan timeSpan2 = TimeSpan.FromSeconds(opponentTime);
		if (opponentTime == 0f)
		{
			TrailRaceOpponentTimeText.text = "-----";
		}
		else if (opponentTime == -1f)
		{
			TrailRaceOpponentTimeText.text = "Disqualified";
		}
		else
		{
			TrailRaceOpponentTimeText.text = $"{timeSpan2.Minutes:D2}:{timeSpan2.Seconds:D2}.{timeSpan2.Milliseconds / 10:00}";
		}
	}

	public void HidePlayerRouteInfo()
	{
		EventLobby.SetActive(value: false);
	}

	public void DisplayPlayerRouteInfo()
	{
		if (!EventLobby.activeSelf)
		{
			EventLobby.SetActive(value: true);
			RecordTime.text = "--Loading--";
			Text[] awardTimes = AwardTimes;
			foreach (Text text in awardTimes)
			{
				text.text = "----";
			}
			Text[] awardAmounts = AwardAmounts;
			foreach (Text text2 in awardAmounts)
			{
				text2.text = "----";
			}
		}
	}

	public void DisplayPlayerRouteValues(float bronzeTime, float silverTime, float goldTime, float finishReward, float bronzeReward, float silverReward, float goldReward, float record)
	{
		if (bronzeTime != 0f)
		{
			AwardTimes[1].text = ConvertTime(bronzeTime);
		}
		if (silverTime != 0f)
		{
			AwardTimes[2].text = ConvertTime(silverTime);
		}
		if (goldTime != 0f)
		{
			AwardTimes[3].text = ConvertTime(goldTime);
		}
		AwardAmounts[0].text = finishReward.ToString();
		AwardAmounts[1].text = bronzeReward.ToString();
		AwardAmounts[2].text = silverReward.ToString();
		AwardAmounts[3].text = goldReward.ToString();
		RecordTime.text = ConvertTime(record);
		if (record == 0f)
		{
			RecordTime.text = "Not set";
		}
	}

	public void ShowPlayerRouteFinish(float time, float record, string keeper, string awardLevel, float xp, float golds, float money)
	{
		if (!(playerRouteFinishWindow == null))
		{
			playerRouteFinishWindow.SetActive(value: true);
			playerRouteTime.text = ConvertTime(time);
			playerRouteRecord.text = ConvertTime(record);
			playerRouteRecordKeeper.text = keeper;
			playerRouteAwardLevel.text = awardLevel;
			playerRouteXP.text = xp.ToString();
			playerRouteGolds.text = golds.ToString();
			playerRouteMoney.text = money.ToString();
		}
	}

	private string ConvertTime(float rawSeconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(rawSeconds);
		return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds / 10:00}";
	}

	public void DisplayPlayerRouteFinish()
	{
		if (!(playerRouteFinishWindow == null))
		{
			playerRouteFinishWindow.SetActive(value: true);
		}
	}

	public void HidePlayerRouteFinish()
	{
		if (!(playerRouteFinishWindow == null))
		{
			playerRouteFinishWindow.SetActive(value: false);
		}
	}

	public void HideShoTrailRaceFinishWindow(bool Show)
	{
		TrailRaceFinishWindow.SetActive(Show);
	}

	public void HideShowOfferRestartButton(bool Show)
	{
		OfferRestartButton.SetActive(Show);
	}

	public void HideShowRestartOffering(bool Show)
	{
		RestartOfferingWindow.SetActive(Show);
	}

	public void HideShowWaitForOtherPlayerButton(bool Show)
	{
		WaitForOtherPlayerButton.SetActive(Show);
	}

	public void HideShowSpectateButton(bool Show)
	{
		SpectateButton.SetActive(Show);
	}

	public void ToggleCarExtras(bool Show)
	{
		CarExtras.SetActive(Show);
	}

	public void ToggleWinchControls(bool Show)
	{
		WinchControls.SetActive(Show);
	}

	public void ShowNotification(string text, bool blinking)
	{
		notificationBlinking = blinking;
		NotificationText.text = text;
		Color color = NotificationText.color;
		color.a = 1f;
		NotificationText.color = color;
	}

	public void HideNotification()
	{
		notificationBlinking = false;
	}

	public void FlipCar()
	{
		carController.FlipCar();
	}

	public void RespawnCar()
	{
		if (GameState.GameType != GameType.TrailRace)
		{
			carController.RespawnCar();
		}
	}

	public void StartRace()
	{
		if (racingManager != null)
		{
			racingManager.StartRace();
		}
		if (PlayerRouteRacingManager.Instance != null)
		{
			PlayerRouteRacingManager.Instance.StartRace();
		}
	}

	public void CancelRace()
	{
		if (racingManager != null)
		{
			racingManager.CancelRace();
		}
		if (PlayerRouteRacingManager.Instance != null)
		{
			PlayerRouteRacingManager.Instance.CancelRace();
		}
	}

	public void Continue()
	{
		if (racingManager != null)
		{
			racingManager.Continue();
		}
		if (PlayerRouteRacingManager.Instance != null)
		{
			PlayerRouteRacingManager.Instance.Continue();
		}
	}

	public void GameOn()
	{
		Ding.Play();
	}

	public void ExtrasChanged(string TextToDisplay)
	{
		if (camController != null)
		{
			camController.Shake();
		}
		ToggleSound.volume = ToggleSoundVolume;
		ToggleSound.Play();
		ShowNotification(TextToDisplay, blinking: false);
	}

	public void SwitchCamera()
	{
		if (!(camController == null))
		{
			ShowNotification(camController.SwitchCamera(), blinking: false);
		}
	}

	public void ToggleGauge()
	{
		MainGauge.SetActive(!MainGauge.activeInHierarchy);
		Transform transform = HideGaugeImage.transform;
		Vector3 eulerAngles = HideGaugeImage.transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(0f, 0f, 0f - eulerAngles.z);
	}
}
