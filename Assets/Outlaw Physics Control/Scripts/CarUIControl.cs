using AGS_MonsterTruckControl;
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

	public AGS_MTC_CarController carController;

	public AGS_MTC_EngineController engine;

	private AGS_MTC_CameraController camController => AGS_MTC_CameraController.Instance;

	private void Awake()
	{
		Instance = this;
		if (SceneManager.GetActiveScene().name == "CustomMap")
		{
			LoadingScreenLabel.text = "Level editor builds your level, it may take some time. Wait...";
		}
	}


	private void Start()
	{
		Color color = NotificationText.color;
		color.a = 0f;
		NotificationText.color = color;
		ToggleCarControls(Show: true);
		ToggleCarExtras(Show: true);
		HideEventLobby();
		HideShowCountdown(Show: false);
		SwitchFlipButton(Show: false);
		RepairWindow.SetActive(value: false);
		
		GameObject[] touchAccelerators = TouchAccelerators;
		foreach (GameObject gameObject in touchAccelerators)
		{
			gameObject.SetActive(true);
		}
		GameObject[] slideAccelerators = SlideAccelerators;
		foreach (GameObject gameObject2 in slideAccelerators)
		{
			gameObject2.SetActive(false);
		}
		ArrowControls.SetActive(value: false);
		SteeringWheelControls.SetActive(value: false);
		TiltControls.SetActive(value: false);
		controlType = 0;
		ToggleCarControls(Show: true);
		if (Map != null)
		{
			Map.SetActive(value: false);
		}
		
		AudioListener.volume = 1f;
		PlayerInformationTemplate = Resources.Load<GameObject>("UI/PlayerInfoPanel");
		
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
		Start();
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
		
		NotificationText.color = color;
		if (engine != null && carController != null)
		{
			DoGauges();
		}
		if (CurrentCheckpoint != null)
		{
			DoCheckpointArrow();
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

	public void ToggleRepairButton(bool Show)
	{
		RepairButton.SetActive(Show);
	}

	public void SwitchFlipButton(bool Show)
	{
		FlipButton.SetActive(Show);
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

	public void ToggleRearView()
	{
		camController.ForceRearView = !camController.ForceRearView;
	}

	public void CalculateRepairCost()
	{
		int num = (int)(1000f * (100f - carController.CarHealth) / 100f);
		RepairCostText.text = "Repair vehicle for $" + num + "?";
	}

	public void ShowPause()
	{
		AudioListener.volume = 0f;
		Time.timeScale = 0f;
		PausePanel.SetActive(value: true);
	}

	
	public void Unpause(bool exiting = true)
	{
		Time.timeScale = 1f;
		LoadingScreen.SetActive(exiting);
		LoadingScreenLabel.text = "Going back home!";
		PausePanel.SetActive(value: false);
		CaptureTheFlagGameOverMessage.SetActive(value: false);
		
		if (exiting)
		{
			SceneManager.LoadScene("Menu");
		}
	}

	public void HideEventLobby()
	{
		EventLobby.SetActive(value: false);
	}
	
	public void ToggleCarControls(bool Show)
	{
		ArrowControls.SetActive(Show && controlType == ControlType.Arrow);
		TiltControls.SetActive(Show && controlType == ControlType.Tilt);
		SteeringWheelControls.SetActive(Show && controlType == ControlType.SteeringWheel);
		CarControlsEnabled = Show;
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

	public void HideShowOfferRestartButton(bool Show)
	{
		OfferRestartButton.SetActive(Show);
	}

	public void HideShowRestartOffering(bool Show)
	{
		RestartOfferingWindow.SetActive(Show);
	}

	public void ToggleCarExtras(bool Show)
	{
		CarExtras.SetActive(Show);
	}

	public void ShowNotification(string text, bool blinking)
	{
		NotificationText.text = text;
		Color color = NotificationText.color;
		color.a = 1f;
		NotificationText.color = color;
	}

	public void FlipCar()
	{
		carController.FlipCar();
	}

	public void RespawnCar()
	{
		carController.RespawnCar();
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
