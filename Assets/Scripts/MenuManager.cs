using CustomVP;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public enum EngineTuningItem
	{
		FuelRatio,
		TimingRatio
	}

	public static MenuManager Instance;

	private MenuState menuState;

	private VehicleType SelectedVehicleType;

	[Header("Prices")]
	public int WashPrice;

	public int FullRepairPrice;

	[Header("Stats")]
	public int Money;

	public int Gold;

	public int XP;

	[Header("UI elements")]
	public CanvasGroup FadeScreen;

	public Text MoneyText;

	public Text GoldText;

	public Text XPText;

	public Text VehicleTypeText;

	public Text MessageText;

	public GameObject MessageBox;

	public Button messageOkButton;

	public GameObject FieldFindBox;

	public Text FieldFindParts1;

	public Text FieldFindParts2;

	public GameObject communityMapsButton;

	private bool storeCallbackTimerCounting;

	private float storeCallbackTimer;

	private const float storeCallbackTimeoutTime = 180f;

	public GameObject YesNoCloudDataBox;

	public GameObject FacebookLoginWarning;

	public GameObject equipTrailerWarning;

	[Header("Private Hosting Screen")]
	public Text CurrentPassword;

	[Header("PIN Entry Screen")]
	public Text CurrentPIN;

	[Header("Camera settings")]
	public Transform CameraTarget;

	public CameraPosition[] cameraPositions;

	private Vector3 cameraTargetPos;

	[Header("Truck selector screen")]
	public Text TruckPriceMoney;

	public Text TruckPriceGold;

	public Text TruckPriceCash;

	public GameObject BuyForGoldButton;

	public GameObject BuyForMoneyButton;

	public GameObject BuyForCashButton;

	public GameObject MembersOnlyPanel;

	public GameObject ExclusivePanel;

	public GameObject PremiumPanel;

	public GameObject MembersAndEveryoneElseAfterDatePanel;

	public Text DaysLeftText;

	public GameObject truckSellWindow;

	public Text truckSellText;

	[Header("Trail race")]
	public Text NoRaceAvailableText;

	private RoomInfo[] TrailRaceRooms;

	private List<GameObject> InstantiatedRoomBars;

	[Space(10f)]
	public GameObject[] StreetTrucks;

	public GameObject[] ATVs;

	public GameObject[] SideBySides;

	public GameObject[] Crawlers;

	public GameObject[] TrophyTrucks;

	public GameObject[] Bikes;

	public GameObject[] TurnKeyVehicles;

	public GameObject[] trailers;

	private GameObject[] SelectedArray;

	private List<VehicleData> StoredVehicles;

	private GameObject LoadedVehicleInSelector;

	private int SelectedTruckIDInSelector;

	private GameObject loadedVehicleOnTrailer;

	[Header("Customize")]
	public Text PartCostText;

	public Text GroupNameText;

	public Text TotalModsCostText;

	public Text TotalModsCostGoldText;

	public Text WashCostText;

	public Text RimSideText;

	public Text RimCostText;

	public Text TireSideText;

	public Text TireCostText;

	public Image WrapPreviewImage;

	public GameObject BodyPartColorMenu;

	public GameObject BodyPartColorBar;

	public GameObject RimsColorBar;

	public GameObject BeadlockColorBar;

	public CUIColorPicker BodyColorPicker;

	public CUIColorPicker WrapColorPicker;

	public Slider XOffsetSlider;

	public Slider YOffsetSlider;

	public Slider XTillingSlider;

	public Slider YTillingSlider;

	public Slider TransparencySlider;

	private GameObject LoadedVehicleInCustomization;

	public GameObject PurchaseModsButton;

	public GameObject PurchaseModsButtonGold;

	public GameObject ModConfirmation;

	public GameObject BuyGlossyPaintButton;

	public GameObject SetGlossyPaintButton;

	public Text CurrentPaintTypeText;

	private PartGroup SelectedPartGroup;

	private int SelectedPartGroupID;

	private int SelectedPartID;

	private WheelsControls SelectedWheelsControls;

	private int SelectedRimID;

	private int SelectedTireID;

	private Side currentSide;

	private int SelectedWrap;

	private Color WrapColor;

	private Vector4 WrapCoords;

	private Vector2 ColorHandlerCoords;

	private bool WrapCoordsSlidersInitialized;

	public Text WrapLayerCountText;

	public Text WrapLayerCostText;

	public GameObject WrapGoldBars;

	public Button ApplyLayerButton;

	private bool LightsOn;

	private PartGroup[] partGroupsBeforeEnteringCustomization;

	private WheelsControls FrontWheelsBeforeEnteringCustomization;

	private WheelsControls RearWheelsBeforeEnteringCustomization;

	private Color FrontRimsColorBeforeEngeringCustomizaiton;

	private Color FrontBeadlockColorBeforeEnteringCustomization;

	private Color RearRimsColorBeforeEngeringCustomizaiton;

	private Color RearBeadlockColorBeforeEnteringCustomization;

	[Header("Power")]
	public GameObject StatsPanel;

	public GameObject DescriptionPanel;

	public GameObject DieselDescriptionPanel;

	public Text DescriptionText;

	public Text UpgradeCostText;

	public Text UpgradeCostGoldText;

	public Text UpgradeCostDieselText;

	public Text StageText;

	public Image StageIcon;

	public Text TypeText;

	public Image TypeImage;

	public Text RepairCostText;

	public GameObject RepairIcon;

	public GameObject WashButton;

	public GameObject unequipTrailerButton;

	public GameObject equipTrailerButton;

	public GameObject loadCarOnTrailerButton;

	public GameObject unloadCarsFromTrailerButton;

	public Sprite EngineBlockImage;

	public Sprite HeadsImage;

	public Sprite GripImage;

	public Sprite ValvetrainImage;

	public Sprite WeightImage;

	public Sprite DieselImage;

	public Sprite TitanImage;

	public Sprite GearboxImage;

	public Sprite EbrakeImage;

	public Sprite TankTracksImage;

	public Sprite TurboImage;

	public Sprite BlowerImage;

	public Sprite Stage1Icon;

	public Sprite Stage2Icon;

	public Sprite Stage3Icon;

	public Sprite Stage4Icon;

	public Sprite Stage5Icon;

	public Image PowerBar;

	public Image GripBar;

	public Image WeightBar;

	public Image DurabilityBar;

	public Button UpgradeButton;

	public Button UpgradeButtonGold;

	public Button UpgradeButtonDiesel;

	public Button UninstallButton;

	private PowerPartType SelectedPowerPartType;

	private PowerPartType SelectedSubPowerPartType;

	private int CurrentPowerPartStage;

	[Header("Drivetrain")]
	public Text SuspensionSideText;

	public Text SuspensionCostText;

	public Text SuspensionCostGoldText;

	public Text SuspensionNameText;

	public Text SuspensionNameInUpgradeBarText;

	public Text SuspensionDescriptionText;

	public Text SuspensionUpgradeCostText;

	public Text SuspensionUpgradeCostGoldText;

	public Text SuspensionStageInUpgradeBarText;

	public Text GearingUpgradeCostText;

	public Text GearingStageInUpgradeBarText;

	public Text WheelsSideText;

	public Text WheelsStageText;

	public Text WheelsUpgradeCostText;

	public Text WheelsUpgradeCostGoldText;

	public Button WheelsUpgradeButton;

	public Button WheelsUpgradeButtonGold;

	public GameObject GearingTutorialWindow;

	private SuspensionValue SelectedSuspensionValue;

	public Button SuspensionUpgradeButton;

	public Button SuspensionUpgradeButtonGold;

	public Button GearingUpgradeButton;

	public Button FirstAdjustmentButton;

	public Button FirstGearButton;

	private List<Button> LoadedAdjustmentButtons;

	private int SelectedGear;

	public AdjustmentSlider SuspensionAdjustmentSlider;

	public AdjustmentSlider WheelsAdjustmentSlider;

	public AdjustmentSlider GearingAdjustmentSlider;

	private int SelectedSuspensionID;

	private Suspension SelectedSuspension;

	[Header("Dyno")]
	public Button BuyTuningPackButton;

	public Button BuyPerfectSetupButton;

	public Text MaxHPText;

	public Text MaxTQText;

	public Text AvgHPText;

	public Text AvgTQText;

	public Text DynoRunsLeftText;

	public GameObject DynoTutorialWindow;

	public AdjustmentSlider EngineTuningSlider;

	public EngineTuningItem selectedEngineTuningItem;

	[Header("Transforms")]
	public Transform TruckSelectorSpawnPoint;

	public Transform[] GarageVehiclePoints;

	//[Header("Logo")]
	//public GameObject Logo;

	[Header("Menus holders")]
	public GameObject MainMenu;

	public GameObject IAPMenu;

	public GameObject TruckTypeSelector;

	public GameObject StorageArea;

	public GameObject TruckSelector;

	public GameObject CustomizeCategorySelector;

	public GameObject CustomizeBodyParts;

	public GameObject CustomizePaint;

	public GameObject CustomizeWraps;

	public GameObject CustomizeRims;

	public GameObject CustomizeTires;

	public GameObject Drivetrain;

	public GameObject SwitchSuspension;

	public GameObject TuneSuspension;

	public GameObject TuneWheels;

	public GameObject TestSuspension;

	public GameObject TuneGearing;

	public GameObject Dyno;

	public GameObject BuyingDynoRuns;

	public GameObject DynoResult;

	public GameObject Power;

	public GameObject communityMapsScreen;

	public GameObject PlayMenu;

	public GameObject MapMenu;

	public GameObject SceneLoading;

	public Text SceneLoadingText;

	public GameObject MultiplayerGameType;

	public GameObject PrivateMultiplayer;

	public GameObject MultiplayerPrivateButton;

	public GameObject FramerateWarning;

	public GameObject PINEntryScreen;

	public GameObject TrailRaceLobby;

	public GameObject TrailSelectorScreen;

	public GameObject TrailRaceBetScreen;

	public GameObject FirstTrailRaceElement;

	public GameObject DesertLockPanel;

	public GameObject StuntParkLockPabel;

	public GameObject RockParkLockPanel;

	public Text DesertLockXPText;

	public Text StuntParkLockXPText;

	public Text RockParkLockXPText;

	public int UnlockDesertXP;

	public int UnlockStuntParkXP;

	public int UnlockRockParkXP;

	public VehicleDataManager CurrentVehicle;

	private BodyPartsSwitcher CurrentPartsSwitcher;

	private CarController CurrentCarController;

	private SuspensionController CurrentSuspensionController;

	private List<VehicleDataManager> LoadedVehiclesInGarage;

	private int SelectedVehicleInGarageID;

	[Header("Side bar")]
	public RectTransform Sidebar;

	public GameObject SettingsTab;

	public GameObject DefaultTab;

	public Text GraphicsLevel;

	public Text MusicStatus;

	public Text SoundStatus;

	public Text ControlsType;

	public Text AcceleratorType;

	public Text TrailName;

	public Text TrailNameHint;

	private bool SideBarExpanded;

	public GameObject[] StaticFieldFinds;

	private static StoreListener storeListener;

	public GameObject MembershipButton;

	public GameObject AlreadyMemberText;

	private bool enableCloudSave = true;

	[Header("Cloud Stuff")]
	public Text LocalGoldLabel;

	public Text LocalMoneyLabel;

	public Text LocalVehiclesLabel;

	public Text CloudGoldLabel;

	public Text CloudMoneyLabel;

	public Text CloudVehiclesLabel;

	public Text TimeStamp;

	private TouchScreenKeyboard keyboard;

	private float sellingTruckCost;

	private int LogoTaps;

	private string url = string.Empty;

	public MenuManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		Instance = this;
		StatsData statsData = GameState.LoadStatsData();
		if (DataStore.GetLong("UpdateOpenedOn1") == 0)
		{
			DataStore.SetLong("UpdateOpenedOn1", DateTime.Now.Ticks);
		}
		if (DataStore.GetLong("UpdateOpenedOn2") == 0)
		{
			DataStore.SetLong("UpdateOpenedOn2", DateTime.Now.Ticks);
		}
		if (!DataStore.GetBool("Opened"))
		{
			if (statsData == null)
			{
				statsData = new StatsData();
			}
			statsData.Money = 30000;
			statsData.Gold = 0;
			statsData.XP = 0;
			GameState.SaveStatsData(statsData);
			DataStore.SetBool("Opened", value: true);
			ShowMessage("We've given you $30,000 to start - visit the dealership!\r\n\r\nHint: That's enough for a truck and a quad!");
		}
		else if (DataStore.GetLong("ShownUpdate25Message") == 0)
		{
			ShowMessage("Welcome to V2.5! You can now purchase trailers, generate your own maps, share them with others, and there's a new crawler! Have fun!");
			DataStore.SetLong("ShownUpdate25Message", 1L);
		}
		if (DataStore.GetString("GeneratedName", string.Empty) == string.Empty)
		{
			DataStore.SetString("GeneratedName", Utility.GenerateName());
		}
		UpdateStats();
		if (SelectedVehicleInGarageID != 0)
		{
			GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
		}
		else
		{
			SelectedVehicleInGarageID = GameState.SelectedGarageVehicleID;
		}
		SetCameraTarget(GarageVehiclePoints[SelectedVehicleInGarageID].position, Instantly: true);
		SettingsTab.SetActive(value: false);
		DefaultTab.SetActive(value: true);
		BodyPartColorBar.SetActive(value: false);
		RimsColorBar.SetActive(value: false);
		BeadlockColorBar.SetActive(value: false);
		ApplySettings();
		if (GameState.FramerateWarning)
		{
			GameState.FramerateWarning = false;
			if (!DataStore.GetBool("IgnoreFramerateWarnings"))
			{
				ShowFramerateWarning();
			}
		}
		int num = DataStore.LastFoundFieldFind() - 1;
		UnityEngine.Debug.Log("Last field find found: " + num);
		if (num >= 0)
		{
			string name = FieldFind.FieldFindNames[num];
			if (!Utility.OwnsVehicle(name) && Utility.FoundAllParts((num + 1).ToString()))
			{
				BuyFieldFindParts(spendMoney: false);
			}
		}
		for (int i = 0; i < StaticFieldFinds.Length; i++)
		{
			string text = FieldFind.FieldFindNames[i];
			StaticFieldFinds[i].SetActive(i == num && !Utility.OwnsVehicle(text));
			UnityEngine.Debug.Log(text + " set active? " + (i == num && !Utility.OwnsVehicle(text)).ToString());
		}
		MultiplayerManager.Connect();
		if (PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
		if (PhotonNetwork.connectedAndReady && !PhotonNetwork.insideLobby)
		{
			PhotonNetwork.JoinLobby();
		}
		PlayFabSettings.TitleId = "433F";
		FB.Init(OnFacebookInitialized);
		MembershipButton.SetActive(!statsData.IsMember);
		AlreadyMemberText.SetActive(statsData.IsMember);
		if (GameState.FailedToJoin)
		{
			ShowMessage("Something went wrong joining the room. Please try again.");
		}
		GameState.FailedToJoin = false;
		if (!GameState.JustOpenedGame)
		{
			DataStore.CloudSave();
		}
		GameState.JustOpenedGame = false;
		LoadMenu(MenuState.MainMenu, ThroughFade: false, FromMainMenu: true);
		LoadSpecificGameSettings();
	}

	private void LoadSpecificGameSettings()
	{
		StartCoroutine(LoadSpecificGameSettingsCor());
	}

	private IEnumerator LoadSpecificGameSettingsCor()
	{
		WWW w = new WWW("https://keereedev.000webhostapp.com/gameSettings.cfg");
		yield return w;
		string[] lines = w.text.Split('\n');
		bool useResourcesUnloading = bool.TryParse(lines[0], out useResourcesUnloading);
		float unloadResourcesRate = float.Parse(lines[1]);
		GameState.unloadUnusedResourcesInGame = useResourcesUnloading;
		GameState.resourcesUnloadRate = unloadResourcesRate;
	}

	public void Awake()
	{
		if (storeListener == null)
		{
			storeListener = new StoreListener();
			storeListener.InitializeIAP();
		}
		else
		{
			UnityEngine.Debug.Log("Already initialized");
		}
	}

	public void OpenURL(string url)
	{
		Application.OpenURL(url);
	}

	public void LogoTapped()
	{
		UnityEngine.Debug.Log("Tapped");
		LogoTaps++;
		if (LogoTaps == 4)
		{
			LoadMenu(MenuState.EnterPIN, ThroughFade: true, FromMainMenu: false);
			LogoTaps = 0;
		}
	}

	public void ConfirmPIN()
	{
		UnityEngine.Debug.Log("PIN WAS: " + CurrentPIN.text);
		url = "http://www.racerslog.net/NoLimit/Remote/RemoteOO.aspx?password=" + CurrentPIN.text;
		StartCoroutine(MakeWebRequest());
		LoadMainMenu(FromMainMenu: false);
	}

	public void HidePINEntry()
	{
		PINEntryScreen.SetActive(value: false);
	}

	private IEnumerator MakeWebRequest()
	{
		UnityEngine.Debug.Log("Making request");
		WWW www = new WWW(url);
		yield return www;
		string unlockString = www.text.Trim();
		int gold = 0;
		int cash = 0;
		int level = 0;
		int ads = 0;
		int membership = 0;
		string[] data = unlockString.Split(',');
		int.TryParse(data[0], out gold);
		int.TryParse(data[1], out cash);
		int.TryParse(data[2], out level);
		int.TryParse(data[3], out ads);
		int.TryParse(data[4], out membership);
		if (gold > 0)
		{
			GameState.AddCurrency(gold, Currency.Gold);
		}
		if (cash > 0)
		{
			GameState.AddCurrency(cash, Currency.Money);
		}
		if (membership > 0)
		{
			GameState.SetMembership(isMember: true);
		}
		if (cash > 0 || gold > 0 || membership > 0)
		{
			ShowMessage("Success!");
		}
		else
		{
			ShowMessage("Invalid PIN!");
		}
		UnityEngine.Debug.Log("Response: " + www.text);
		UpdateStats();
	}

	private void Update()
	{
		CameraTarget.position = Vector3.Lerp(CameraTarget.position, cameraTargetPos, Time.deltaTime * 10f);
		Vector3 b = new Vector3((!SideBarExpanded) ? (-600) : (-200), 0f, 0f);
		Sidebar.localPosition = Vector3.Lerp(Sidebar.localPosition, b, Time.deltaTime * 10f);
		if (enableCloudSave)
		{
			DataStore.CloudSave();
		}
		if (keyboard != null && keyboard.done)
		{
			ChangeTrailName();
			keyboard = null;
		}
		if (storeCallbackTimerCounting)
		{
			storeCallbackTimer += Time.deltaTime;
			if (storeCallbackTimer > 180f)
			{
				ShowMessage("Purhase timeout");
				storeCallbackTimerCounting = false;
			}
		}
	}

	private void OnApplicationQuit()
	{
		DataStore.CloudSave();
	}

	public void PrivateRace()
	{
		LoadMenu(MenuState.PrivateMultiplayer, ThroughFade: true, FromMainMenu: false);
	}

	public void SelectTrail(int id)
	{
		GameState.TrailID = id;
		Trail trail = Trails.trails.Find((Trail t) => t.id == id);
		GameState.SceneName = trail.MapName;
		GameState.GameType = GameType.TrailRace;
		StartGame(string.Empty);
	}

	public void AddPasswordNumber(int number)
	{
		if (CurrentPassword.text.IndexOf("-") != -1)
		{
			CurrentPassword.text = string.Empty;
		}
		if (CurrentPassword.text.Length < 4)
		{
			CurrentPassword.text += number.ToString();
		}
	}

	public void RemovePasswordNumber()
	{
		if (CurrentPassword.text.Length > 0)
		{
			CurrentPassword.text = CurrentPassword.text.Substring(0, CurrentPassword.text.Length - 1);
		}
		if (CurrentPassword.text.Length == 0)
		{
			CurrentPassword.text = "----";
		}
	}

	public void AddPINNumber(int number)
	{
		if (CurrentPIN.text.IndexOf("-") != -1)
		{
			CurrentPIN.text = string.Empty;
		}
		if (CurrentPIN.text.Length < 6)
		{
			CurrentPIN.text += number.ToString();
		}
	}

	public void RemovePINNumber()
	{
		if (CurrentPIN.text.Length > 0)
		{
			CurrentPIN.text = CurrentPIN.text.Substring(0, CurrentPIN.text.Length - 1);
		}
		if (CurrentPIN.text.Length == 0)
		{
			CurrentPIN.text = "------";
		}
	}

	public void JoinPrivateNow()
	{
		if (CurrentPassword.text.Length != 4)
		{
			ShowMessage("Password must be 4 digits.");
			return;
		}
		RoomInfo roomInfo = MultiplayerManager.FindPrivateRoom(CurrentPassword.text);
		if (roomInfo != null)
		{
			HideWaiting();
			GameState.Password = CurrentPassword.text;
			GameState.GameType = (GameType)roomInfo.CustomProperties["GameType"];
			GameState.RoomName = roomInfo.Name;
			GameState.SceneName = roomInfo.CustomProperties["Scene"].ToString();
			if (roomInfo.CustomProperties["TrailID"] != null)
			{
				GameState.TrailID = (int)roomInfo.CustomProperties["TrailID"];
			}
			if (roomInfo.CustomProperties["TrailRaceBet"] != null)
			{
				GameState.TrailRaceBet = (int)roomInfo.CustomProperties["TrailRaceBet"];
			}
			StartGame(string.Empty);
		}
		else
		{
			HideWaiting();
			ShowMessage("Couldn't find that private map. If it was just created you might need to try again in a moment.");
		}
	}

	public void HostNow()
	{
		string password = Utility.RandomDigits(4);
		GameState.RoomName = Utility.RandomDigits(10);
		GoPlaying(isMultiplayer: true, password);
	}

	public void HostTrailRace()
	{
		GameState.Populate(CurrentVehicle.VehicleID, null, GameMode.Multiplayer, GameType.TrailRace, string.Empty);
		SetRaceBet(0);
	}

	public void ShowFramerateWarning()
	{
		FramerateWarning.SetActive(value: true);
	}

	public void HideFramerateWarning()
	{
		FramerateWarning.SetActive(value: false);
	}

	public void IgnoreFramerateWarning(bool never)
	{
		DataStore.SetBool("IgnoreFramerateWarnings", never);
		HideFramerateWarning();
		if (never)
		{
			ShowMessage("Ok, remember: You can change the graphics quality in settings any time you want!");
		}
	}

	public void AcceptFramerateWarning()
	{
		int num = DataStore.GetInt("GraphicsLevel", 2);
		if (num > 0)
		{
			num--;
		}
		DataStore.SetInt("GraphicsLevel", num);
		ApplySettings();
		HideFramerateWarning();
		ShowMessage("Graphics level lowered!");
	}

	public void EditTrailName()
	{
		keyboard = TouchScreenKeyboard.Open(DataStore.GetString("GeneratedName"), TouchScreenKeyboardType.Default);
	}

	private void ChangeTrailName()
	{
		if (keyboard.text != string.Empty)
		{
			string text = keyboard.text;
			text = Utility.CleanBadWords(text);
			DataStore.SetString("GeneratedName", text);
			ApplySettings();
		}
	}

	public void ToggleControlsType()
	{
		int @int = DataStore.GetInt("ControlsType");
		@int = ((@int < 2) ? (@int + 1) : 0);
		DataStore.SetInt("ControlsType", @int);
		ApplySettings();
	}

	public void ToggleTrailName()
	{
		if (AccessToken.CurrentAccessToken == null)
		{
			DataStore.SetBool("UseFBName", value: false);
		}
		else
		{
			DataStore.SetBool("UseFBName", !DataStore.GetBool("UseFBName"));
		}
		ApplySettings();
	}

	public void ToggleSound()
	{
		int value = (DataStore.GetInt("GameSound") == 0) ? 1 : 0;
		DataStore.SetInt("GameSound", value);
		ApplySettings();
	}

	public void ToggleMusic()
	{
		int value = (DataStore.GetInt("BackgroundMusic") == 0) ? 1 : 0;
		DataStore.SetInt("BackgroundMusic", value);
		ApplySettings();
	}

	public void ToggleAccelerator()
	{
		bool @bool = DataStore.GetBool("SlideAccelerator");
		@bool = !@bool;
		DataStore.SetBool("SlideAccelerator", @bool);
		ApplySettings();
	}

	public void ToggleGraphicsLevel()
	{
		int @int = DataStore.GetInt("GraphicsLevel", 2);
		@int = ((@int < 4) ? (@int + 1) : 0);
		DataStore.SetInt("GraphicsLevel", @int);
		ApplySettings();
	}

	public void ApplySettings()
	{
		QualitySettings.SetQualityLevel(DataStore.GetInt("GraphicsLevel", 2), applyExpensiveChanges: true);
		switch (QualitySettings.GetQualityLevel())
		{
		case 0:
			GraphicsLevel.text = "Very Low";
			break;
		case 1:
			GraphicsLevel.text = "Low";
			break;
		case 2:
			GraphicsLevel.text = "Medium";
			break;
		case 3:
			GraphicsLevel.text = "High";
			break;
		case 4:
			GraphicsLevel.text = "Very High";
			break;
		case 5:
			GraphicsLevel.text = "Ultra";
			break;
		}
		SoundStatus.text = ((DataStore.GetInt("GameSound", 1) != 0) ? "On" : "Off");
		MusicStatus.text = ((DataStore.GetInt("BackgroundMusic", 1) != 0) ? "On" : "Off");
		switch (DataStore.GetInt("ControlsType", 0))
		{
		case 0:
			ControlsType.text = "Arrows";
			break;
		case 2:
			ControlsType.text = "Wheel";
			break;
		case 1:
			ControlsType.text = "Tilt";
			break;
		}
		if (DataStore.GetBool("SlideAccelerator"))
		{
			AcceleratorType.text = "Slide";
		}
		else
		{
			AcceleratorType.text = "Touch";
		}
		if (DataStore.GetInt("BackgroundMusic", 1) == 0)
		{
			AudioListener.volume = 0f;
		}
		else
		{
			AudioListener.volume = 1f;
		}
		UnityEngine.Debug.Log(DataStore.GetString("GeneratedName"));
		if (AccessToken.CurrentAccessToken == null)
		{
			DataStore.SetBool("UseFBName", value: false);
		}
		if (DataStore.GetBool("UseFBName"))
		{
			TrailName.text = GameState.PlayerName;
			TrailNameHint.text = "Tap to use generated name";
		}
		else
		{
			if (AccessToken.CurrentAccessToken == null)
			{
				TrailNameHint.text = "Tap the \"F\" to login to FB";
			}
			else
			{
				TrailNameHint.text = "Tap to use Facebook name";
			}
			TrailName.text = DataStore.GetString("GeneratedName");
		}
		PhotonNetwork.player.NickName = TrailName.text;
	}

	public void PurchaseProduct(string productName)
	{
		storeListener.PurchaseIAP(productName);
	}

	public void RestorePurchases()
	{
		storeListener.RestoreIAP();
	}

	public void LoadMainMenu(bool FromMainMenu)
	{
		LoadMenu(MenuState.MainMenu, ThroughFade: true, FromMainMenu);
	}

	public void LoadVehicleTypeSelector(bool FromMainMenu)
	{
		LoadMenu(MenuState.TruckTypeSelector, ThroughFade: true, FromMainMenu);
	}

	public void LoadVehicleSelector(bool FromMainMenu)
	{
		LoadMenu(MenuState.TruckSelector, ThroughFade: true, FromMainMenu);
	}

	public void LoadStorageArea(bool FromMainMenu)
	{
		LoadMenu(MenuState.StorageArea, ThroughFade: true, FromMainMenu);
	}

	public void LoadCustomizeCategorySelector(bool FromMainMenu)
	{
		if (CurrentVehicle != null && CurrentPartsSwitcher != null)
		{
			LoadMenu(MenuState.CustomizeCategorySelector, ThroughFade: true, FromMainMenu);
		}
		else
		{
			ShowMessage("No vehicle!");
		}
	}

	public void LoadDrivetrain(bool FromMainMenu)
	{
		if (CurrentVehicle != null && CurrentSuspensionController != null)
		{
			LoadMenu(MenuState.Drivetrain, ThroughFade: true, FromMainMenu);
		}
		else
		{
			ShowMessage("No vehicle!");
		}
	}

	public void LoadPower(bool FromMainMenu)
	{
		if (CurrentVehicle != null && CurrentCarController != null)
		{
			LoadMenu(MenuState.Power, ThroughFade: true, FromMainMenu);
		}
		else
		{
			ShowMessage("No vehicle!");
		}
	}

	public void LoadIAPMenu()
	{
		IAPMenu.SetActive(value: true);
		//Logo.SetActive(value: false);
	}

	public void ToggleLights()
	{
		LightsOn = !LightsOn;
		LightsController[] array = UnityEngine.Object.FindObjectsOfType<LightsController>();
		foreach (LightsController lightsController in array)
		{
			lightsController.LightsOn = LightsOn;
		}
	}

	public void HideIAPMenu()
	{
		IAPMenu.SetActive(value: false);
		if (menuState == MenuState.MainMenu)
		{
			//Logo.SetActive(value: true);
		}
	}

	public void LoadCommunityMapsMenu()
	{
		LoadMenu(MenuState.CommunityMaps, ThroughFade: true, FromMainMenu: true);
	}

	public void LoadPlay(bool FromMainMenu)
	{
		if (CurrentVehicle != null && CurrentCarController != null)
		{
			LoadMenu(MenuState.Play, ThroughFade: true, FromMainMenu);
		}
		else if (CurrentVehicle != null)
		{
			ShowMessage("Choose a vehicle, you can't drive this!");
		}
		else
		{
			ShowMessage("No vehicle!");
		}
	}

	public void Play(bool isMultiplayer)
	{
		GoPlaying(isMultiplayer);
	}

	public void BuyMap(string mapName)
	{
		if (ProcessPurchase(Currency.Gold, 50))
		{
			DataStore.SetBool(mapName + "Unlocked", value: true);
			LoadMenu(MenuState.Map, ThroughFade: false, FromMainMenu: false);
		}
		else
		{
			ShowMessage("You don't have enough gold. To continue, you can purchase gold, or do more races to earn more XP!");
			LoadIAPMenu();
		}
	}

	public void StartGame(string mapName = "")
	{
		if (mapName != string.Empty)
		{
			GameState.SceneName = mapName;
		}
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			StartCoroutine(StartMultiplayer());
		}
		else
		{
			LoadScene(mapName);
		}
	}

	private IEnumerator StartMultiplayer()
	{
		StatsData sData = GameState.LoadStatsData();
		if (GameState.SceneName == "MapDesertNG" && sData.XP < UnlockDesertXP && !DataStore.GetBool(GameState.SceneName + "Unlocked") && !sData.IsMember)
		{
			BuyMap("MapDesertNG");
			yield break;
		}
		if (GameState.SceneName == "StuntPark" && sData.XP < UnlockStuntParkXP && !DataStore.GetBool(GameState.SceneName + "Unlocked") && !sData.IsMember)
		{
			BuyMap("StuntPark");
			yield break;
		}
		if (GameState.SceneName == "RockParkNG" && sData.XP < UnlockRockParkXP && !DataStore.GetBool(GameState.SceneName + "Unlocked") && !sData.IsMember)
		{
			BuyMap("RockParkNG");
			yield break;
		}
		SceneLoadingText.text = "Loading trailer...";
		SceneLoading.SetActive(value: true);
		if (!PhotonNetwork.connectedAndReady || !PhotonNetwork.insideLobby || PhotonNetwork.inRoom)
		{
			int num = 0;
			if (!PhotonNetwork.connectedAndReady)
			{
				num = 1;
			}
			if (!PhotonNetwork.insideLobby)
			{
				num = 2;
			}
			if (PhotonNetwork.inRoom)
			{
				num = 3;
			}
			ShowMessage("Multiplayer isn't ready yet. Please try again in a moment. Make sure you have Internet access! (" + num.ToString() + ")");
			SceneLoading.SetActive(value: false);
		}
		else
		{
			MultiplayerManager.JoinRoom();
			for (float time = 0f; time < 10f; time += 1f)
			{
				yield return new WaitForSeconds(1f);
			}
			if (!PhotonNetwork.inRoom)
			{
				ShowMessage("Can't connect to the room. Try again");
			}
		}
	}

	public void LoadScene(string sceneName)
	{
		StatsData statsData = GameState.LoadStatsData();
		if (sceneName == "MapDesertNG" && statsData.XP < UnlockDesertXP && !DataStore.GetBool(sceneName + "Unlocked") && !statsData.IsMember)
		{
			BuyMap("MapDesertNG");
			return;
		}
		if (sceneName == "StuntPark" && statsData.XP < UnlockStuntParkXP && !DataStore.GetBool(sceneName + "Unlocked") && !statsData.IsMember)
		{
			BuyMap("StuntPark");
			return;
		}
		if (sceneName == "RockParkNG" && statsData.XP < UnlockRockParkXP && !DataStore.GetBool(sceneName + "Unlocked") && !statsData.IsMember)
		{
			BuyMap("RockParkNG");
			return;
		}
		SceneLoadingText.text = "Loading trailer...";
		SceneLoading.SetActive(value: true);
		SceneManager.LoadScene(sceneName);
	}

	public void SelectTrailRaceRoom(GameObject roomElement)
	{
		int num = -1;
		for (int i = 0; i < roomElement.transform.parent.childCount; i++)
		{
			if (roomElement.transform.parent.GetChild(i).gameObject == roomElement)
			{
				num = i;
				break;
			}
		}
		RoomInfo roomInfo = null;
		string name = TrailRaceRooms[num].Name;
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		foreach (RoomInfo roomInfo2 in roomList)
		{
			if (roomInfo2.Name == name)
			{
				roomInfo = roomInfo2;
				break;
			}
		}
		if (roomInfo != null)
		{
			if (roomInfo.PlayerCount > 1 || !roomInfo.IsOpen)
			{
				ShowMessage("Sorry, this game is full already!");
				CreateTrailRaceRoomsList(Randomize: false);
			}
			else if (roomInfo.CustomProperties["TrailID"] != null)
			{
				GameState.TrailID = (int)roomInfo.CustomProperties["TrailID"];
				HideWaiting();
				GameState.RoomName = roomInfo.Name;
				GameState.GameType = GameType.TrailRace;
				GameState.SceneName = roomInfo.CustomProperties["Scene"].ToString();
				StartGame(string.Empty);
			}
			else
			{
				ShowMessage("Oops! Something went wrong. Try again");
			}
		}
		else
		{
			ShowMessage("Sorry, this game no longer exists!");
			CreateTrailRaceRoomsList(Randomize: false);
		}
	}

	public void Repair()
	{
		RepairVehicle(Currency.Money);
	}

	public void EquipTrailer()
	{
		equipTrailerWarning.SetActive(value: true);
	}

	public void SelectStreetTrucks()
	{
		SelectedVehicleType = VehicleType.Truck;
	}

	public void SelectATVs()
	{
		SelectedVehicleType = VehicleType.ATV;
	}

	public void SelectSideBySides()
	{
		SelectedVehicleType = VehicleType.SideBySide;
	}

	public void SelectCrawlers()
	{
		SelectedVehicleType = VehicleType.Crawler;
	}

	public void SelectBikes()
	{
		SelectedVehicleType = VehicleType.Bike;
	}

	public void SelectTrailers()
	{
		SelectedVehicleType = VehicleType.Trailer;
	}

	public void SelectStorageVehicles()
	{
		SelectedVehicleType = VehicleType.Any;
	}

	public void SelectTurnKeyVehicles()
	{
		SelectedVehicleType = VehicleType.TurnKey;
	}

	public void NextVehicleInSelector(bool isStorage)
	{
		SelectedTruckIDInSelector++;
		UpdateVehicleSelector(isStorage);
	}

	public void PrevVehicleInSelector(bool isStorage)
	{
		SelectedTruckIDInSelector--;
		UpdateVehicleSelector(isStorage);
	}

	public void BuyVehicleForMoney()
	{
		BuyVehicle(Currency.Money);
	}

	public void BuyVehicleForGold()
	{
		BuyVehicle(Currency.Gold);
	}

	public void BuyVehicleForCash()
	{
		BuyVehicleForRealCash();
	}

	public void Wash()
	{
		WashVehicle(Currency.Money);
	}

	public void Randomize()
	{
		RandomizeVehicle();
	}

	public void Stock()
	{
		SetStock();
	}

	public void BuyAllModsForMoney()
	{
		BuyAllMods(Currency.Money);
	}

	public void BuyAllModsForGold()
	{
		BuyAllMods(Currency.Gold);
	}

	public void Customize_BodyParts(bool FromMainMenu)
	{
		if (CurrentPartsSwitcher.partGroups.Length > 0)
		{
			LoadMenu(MenuState.CustomizeBodyParts, ThroughFade: false, FromMainMenu);
		}
		else
		{
			ShowMessage("No mods available");
		}
	}

	public void Customize_Paint(bool FromMainMenu)
	{
		LoadMenu(MenuState.CustomizePaint, ThroughFade: false, FromMainMenu);
	}

	public void Customize_Rims(bool FromMainMenu)
	{
		if (CurrentSuspensionController.FrontWheelsControls.TankTracks)
		{
			ShowMessage("Uninstall the tracks first!");
		}
		else
		{
			LoadMenu(MenuState.CustomizeRims, ThroughFade: false, FromMainMenu);
		}
	}

	public void Customize_Tires(bool FromMainMenu)
	{
		if (CurrentSuspensionController.FrontWheelsControls.TankTracks)
		{
			ShowMessage("Uninstall the tracks first!");
		}
		else
		{
			LoadMenu(MenuState.CustomizeTires, ThroughFade: false, FromMainMenu);
		}
	}

	public void Customize_Wraps(bool FromMainMenu)
	{
		LoadMenu(MenuState.CustomizeWraps, ThroughFade: false, FromMainMenu);
	}

	public void NextBodyPart()
	{
		ChangePart(SelectedPartID + 1);
	}

	public void PrevBodyPart()
	{
		ChangePart(SelectedPartID - 1);
	}

	public void NextBodyPartGroup()
	{
		ChangePartGroup(SelectedPartGroupID + 1);
	}

	public void PrevBodyPartGroup()
	{
		ChangePartGroup(SelectedPartGroupID - 1);
	}

	public void NextRim()
	{
		ChangeRim(SelectedRimID + 1);
	}

	public void PrevRim()
	{
		ChangeRim(SelectedRimID - 1);
	}

	public void NextRimSide()
	{
		ChangeRimSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void PrevRimSide()
	{
		ChangeRimSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void NextTire()
	{
		ChangeTire(SelectedTireID + 1);
	}

	public void PrevTire()
	{
		ChangeTire(SelectedTireID - 1);
	}

	public void NextTireSide()
	{
		ChangeTireSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void PrevTireSide()
	{
		ChangeTireSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void ChooseColor(Image image)
	{
		SetVehicleColorInCustomization(image.color);
		ToggleBodyColorPicker(Show: false);
	}

	public void ChooseCustomBodyColor()
	{
		SetVehicleColorInCustomization(BodyColorPicker.Color);
	}

	public void ChooseCustomWrapColor()
	{
		WrapColor = WrapColorPicker.Color;
		UpdateWrap();
	}

	public void ToggleBodyColorPicker(bool Show)
	{
		BodyColorPicker.gameObject.SetActive(Show);
		BodyColorPicker.Color = CurrentPartsSwitcher.BodyColor;
		BodyColorPicker.ColorChangedCallback = ChooseCustomBodyColor;
	}

	public void ChooseRimColor(Image image)
	{
		SetRimColor(image.color);
	}

	public void ChooseBedlockColor(Image image)
	{
		SetBeadlockColor(image.color);
	}

	public void ChooseBodyPartColor(Image image)
	{
		SetBodyPartColor(image.color);
	}

	public void SetMattePaint()
	{
		ChangeGlossiness(Glossy: false);
	}

	public void SetGlossy()
	{
		ChangeGlossiness(Glossy: true);
	}

	public void BuyGlossy()
	{
		BuyGlossyPaint(Currency.Gold);
	}

	public void ToggleBodyColorBar()
	{
		BodyPartColorBar.SetActive(!BodyPartColorBar.activeSelf);
	}

	public void ToggleRimsColorBar()
	{
		RimsColorBar.SetActive(!RimsColorBar.activeSelf);
		BeadlockColorBar.SetActive(value: false);
	}

	public void ToggleBeadlockColorBar()
	{
		BeadlockColorBar.SetActive(!BeadlockColorBar.activeSelf);
		RimsColorBar.SetActive(value: false);
	}

	public void NextWrap()
	{
		LoadWrap(SelectedWrap + 1);
	}

	public void PrevWrap()
	{
		LoadWrap(SelectedWrap - 1);
	}

	public void OffsetChanged()
	{
		WrapOffsetChanged();
	}

	public void Power_EnginePower()
	{
		LoadPowerSubtypeScreen(PowerPartType.EnginePower);
	}

	public void Power_EngineBlock()
	{
		LoadPowerSubtypeScreen(PowerPartType.EngineBlock);
	}

	public void Power_Head()
	{
		LoadPowerSubtypeScreen(PowerPartType.Head);
	}

	public void Power_Valvetrain()
	{
		LoadPowerSubtypeScreen(PowerPartType.Valvetrain);
	}

	public void Power_Grip()
	{
		LoadPowerSubtypeScreen(PowerPartType.Grip);
	}

	public void Power_Weight()
	{
		LoadPowerSubtypeScreen(PowerPartType.Weight);
	}

	public void Power_Durability()
	{
		LoadPowerSubtypeScreen(PowerPartType.Durability);
	}

	public void Power_Diesel()
	{
		LoadPowerSubtypeScreen(PowerPartType.Diesel);
	}

	public void Power_Gearbox()
	{
		LoadPowerSubtypeScreen(PowerPartType.Gearbox);
	}

	public void Power_Ebrake()
	{
		LoadPowerSubtypeScreen(PowerPartType.Ebrake);
	}

	public void Power_TankTracks()
	{
		LoadPowerSubtypeScreen(PowerPartType.TankTracks);
	}

	public void Power_Turbo()
	{
		LoadPowerSubtypeScreen(PowerPartType.Turbo);
	}

	public void Power_Blower()
	{
		LoadPowerSubtypeScreen(PowerPartType.Blower);
	}

	public void Power_Uninstall()
	{
		UninstallPowerPart();
	}

	public void UpgradePower()
	{
		UpgradePowerPart(Currency.Money);
	}

	public void UpgradePowerGold()
	{
		UpgradePowerPart(Currency.Gold);
	}

	public void Drivetrain_SwitchSuspension()
	{
		LoadMenu(MenuState.SwitchSuspension, ThroughFade: true, FromMainMenu: false);
	}

	public void Drivetrain_TuneSuspension()
	{
		LoadMenu(MenuState.TuneSuspension, ThroughFade: true, FromMainMenu: false);
	}

	public void Drivetrain_TuneWheels()
	{
		if (CurrentSuspensionController.FrontWheelsControls.TankTracks)
		{
			ShowMessage("Uninstall the tracks first!");
		}
		else
		{
			LoadMenu(MenuState.TuneWheels, ThroughFade: true, FromMainMenu: false);
		}
	}

	public void Drivetrain_TestSuspension()
	{
		LoadMenu(MenuState.TestSuspension, ThroughFade: true, FromMainMenu: false);
	}

	public void Drivetrain_TuneGearing()
	{
		LoadMenu(MenuState.TuneGearing, ThroughFade: true, FromMainMenu: false);
	}

	public void Drivetrain_DynoTest()
	{
		LoadMenu(MenuState.Dyno, ThroughFade: true, FromMainMenu: false);
	}

	public void Drivetrain_BuyDynoRuns()
	{
		LoadMenu(MenuState.BuyingDynoRuns, ThroughFade: false, FromMainMenu: false);
	}

	public void Drivetrain_DynoResult()
	{
		LoadMenu(MenuState.DynoResult, ThroughFade: true, FromMainMenu: false);
	}

	public void NextSuspension()
	{
		ChangeSuspension(SelectedSuspensionID + 1);
	}

	public void PrevSuspension()
	{
		ChangeSuspension(SelectedSuspensionID - 1);
	}

	public void NextSuspensionSide()
	{
		ChangeSuspensionSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void PrevSuspensionSide()
	{
		ChangeSuspensionSide((currentSide == Side.Front) ? Side.Rear : Side.Front);
	}

	public void SetFrontSuspensionAdjustmentSide()
	{
		ChangeSuspensionAdjustmentsSide(Side.Front);
	}

	public void SetRearSuspensionAdjustmentSide()
	{
		ChangeSuspensionAdjustmentsSide(Side.Rear);
	}

	public void SetFrontWheelsSide()
	{
		ChangeWheelsSide(Side.Front);
	}

	public void SetRearWheelsSide()
	{
		ChangeWheelsSide(Side.Rear);
	}

	public void OnSuspensionAdjustmentChanged()
	{
		SuspensionAdjustmentChanged();
	}

	public void OnGearValueChanged()
	{
		GearValueChanged();
	}

	public void InstallSuspension()
	{
		InstallChosenSuspension(Currency.Money);
	}

	public void InstallSuspensionGold()
	{
		InstallChosenSuspension(Currency.Gold);
	}

	public void UpgradeSuspension()
	{
		UpgradeSelectedSuspension(Currency.Money);
	}

	public void UpgradeSuspensionGold()
	{
		UpgradeSelectedSuspension(Currency.Gold);
	}

	public void UpgradeGearing()
	{
		UpgradeGearingStage();
	}

	public void UpgradeWheels()
	{
		UpgradeSelectedWheels(Currency.Money);
	}

	public void UpgradeWheelsGold()
	{
		UpgradeSelectedWheels(Currency.Gold);
	}

	public void SaveVehicle()
	{
		CurrentVehicle.SaveVehicleData();
	}

	public void Wheels_SelectRimSizeAdjustment()
	{
		SelectRimSizeAdjustment();
	}

	public void Wheels_SelectWheelsRadiusAdjustment()
	{
		SelectWheelsRadiusAdjustment();
	}

	public void Wheels_SelelectWheelsWidthAdjustment()
	{
		SelectWheelsWidthAdjustment();
	}

	public void OnWheelsAdjustmentChanged()
	{
		WheelsAdjustmentChanged();
	}

	public void SetDefaulGears()
	{
		CurrentCarController.GearRatios = GearsManager.DefaultGears;
		CurrentCarController.LowGearRatio = GearsManager.DefaultLowGear;
		UpdateGearingSlider();
	}

	public void SetRaceBet(int bet)
	{
		StatsData statsData = GameState.LoadStatsData();
		if (statsData.Money < bet)
		{
			ShowMessage("Not enough money!");
			return;
		}
		GameState.TrailRaceBet = bet;
		LoadMenu(MenuState.TrailSelectorScreen, ThroughFade: false, FromMainMenu: false);
	}

	public void OpenSettingsTab()
	{
		OpenSettings();
	}

	private void OpenSettings()
	{
		SettingsTab.SetActive(value: true);
		DefaultTab.SetActive(value: false);
	}

	public void SwitchSideBar()
	{
		SideBarExpanded = !SideBarExpanded;
		TimeStamp.text = DateTime.Now.ToString();
	}

	public void BackToGarageFromCustomize()
	{
		if (PurchaseModsButton.activeInHierarchy)
		{
			ShowModConfirmation();
		}
		else
		{
			LoadMainMenu(FromMainMenu: false);
		}
	}

	private void ShowModConfirmation()
	{
		ModConfirmation.SetActive(value: true);
	}

	private void WashVehicle(Currency currency)
	{
		int num = 300;
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			CurrentPartsSwitcher.WashVehicle();
			WashButton.SetActive(value: false);
		}
		else if (currency == Currency.Money && Utility.CashToGold(300) <= GameState.LoadStatsData().Gold)
		{
			WashVehicle(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	public void DoneWashing()
	{
		UpdateSideButtons();
	}

	private void RandomizeVehicle()
	{
		CurrentSuspensionController.SetRandomWheels();
		CurrentPartsSwitcher.SetRandomModification();
		PurchaseModsButton.SetActive(value: true);
		UpdatePurchaseModsButton();
	}

	private void SetStock()
	{
		CurrentSuspensionController.SetStockWheels();
		CurrentPartsSwitcher.SetStockModification();
		PurchaseModsButton.SetActive(value: true);
		UpdatePurchaseModsButton();
	}

	private void SetVehicleColorInCustomization(Color color)
	{
		CurrentPartsSwitcher.BodyColor = color;
		CurrentPartsSwitcher.UpdateColor(Merge: false);
	}

	private void BuyAllMods(Currency currency)
	{
		int num = TotalModsCost();
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			PartGroup[] partGroups = CurrentPartsSwitcher.partGroups;
			foreach (PartGroup partGroup in partGroups)
			{
				if (partGroup.Parts[partGroup.InstalledPart] != null && !CurrentVehicle.PurchasedPartsList.Contains(partGroup.Parts[partGroup.InstalledPart].name))
				{
					CurrentVehicle.PurchasedPartsList.Add(partGroup.Parts[partGroup.InstalledPart].name);
				}
			}
			int intValue = CurrentSuspensionController.FrontWheelsControls.Rim.IntValue;
			int intValue2 = CurrentSuspensionController.RearWheelsControls.Rim.IntValue;
			int intValue3 = CurrentSuspensionController.FrontWheelsControls.Tire.IntValue;
			int intValue4 = CurrentSuspensionController.RearWheelsControls.Tire.IntValue;
			if (!CurrentVehicle.PurchasedPartsList.Contains("Rim" + intValue.ToString()))
			{
				CurrentVehicle.PurchasedPartsList.Add("Rim" + intValue.ToString());
			}
			if (!CurrentVehicle.PurchasedPartsList.Contains("Rim" + intValue2.ToString()))
			{
				CurrentVehicle.PurchasedPartsList.Add("Rim" + intValue2.ToString());
			}
			if (!CurrentVehicle.PurchasedPartsList.Contains("Tire" + intValue3.ToString()))
			{
				CurrentVehicle.PurchasedPartsList.Add("Tire" + intValue3.ToString());
			}
			if (!CurrentVehicle.PurchasedPartsList.Contains("Tire" + intValue4.ToString()))
			{
				CurrentVehicle.PurchasedPartsList.Add("Tire" + intValue4.ToString());
			}
			CurrentVehicle.SaveVehicleData();
			LoadMenu(MenuState.MainMenu, ThroughFade: true, FromMainMenu: false);
		}
		else if (currency == Currency.Money && Utility.CashToGold(num) <= GameState.LoadStatsData().Gold)
		{
			BuyAllMods(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void ChangePartGroup(int newGroupID)
	{
		SelectedPartGroupID = newGroupID;
		if (SelectedPartGroupID > CurrentPartsSwitcher.partGroups.Length - 1)
		{
			SelectedPartGroupID = 0;
		}
		if (SelectedPartGroupID < 0)
		{
			SelectedPartGroupID = CurrentPartsSwitcher.partGroups.Length - 1;
		}
		SelectedPartGroup = CurrentPartsSwitcher.partGroups[SelectedPartGroupID];
		SelectedPartID = SelectedPartGroup.InstalledPart;
		if (SelectedPartGroup.partType == PartType.Headlights && !LightsOn)
		{
			ToggleLights();
		}
		if (SelectedPartGroup.partType != PartType.Headlights && LightsOn)
		{
			ToggleLights();
		}
		BodyPartColorMenu.SetActive(SelectedPartGroup.Paintable);
		CameraPosition[] array = cameraPositions;
		foreach (CameraPosition cameraPosition in array)
		{
			if (cameraPosition.bodyPartType == CurrentPartsSwitcher.partGroups[SelectedPartGroupID].partType)
			{
				CameraController.Instance.SetCameraPos(cameraPosition.XAngle, cameraPosition.YAngle, cameraPosition.Distance);
			}
		}
		GroupNameText.text = SelectedPartGroup.GroupName;
		ChangePart(SelectedPartID);
	}

	private void ChangePart(int newPartID)
	{
		SelectedPartID = newPartID;
		if (SelectedPartID > SelectedPartGroup.Parts.Length - 1)
		{
			SelectedPartID = 0;
		}
		if (SelectedPartID < 0)
		{
			SelectedPartID = SelectedPartGroup.Parts.Length - 1;
		}
		if (SelectedPartGroup.Parts[SelectedPartID] == null)
		{
			PartCostText.text = "Owned";
			CurrentPartsSwitcher.InstallBodyPart(SelectedPartGroup, SelectedPartID);
			return;
		}
		string name = SelectedPartGroup.Parts[SelectedPartID].name;
		PartType partType = SelectedPartGroup.partType;
		BodyPart part = VehicleParts.GetPart(CurrentVehicle.vehicleType, partType, name);
		bool flag = CurrentVehicle.PurchasedPartsList.Contains(name);
		PartCostText.text = ((!flag && part.partCost != 0) ? ("$" + part.partCost.ToString()) : "Owned");
		CurrentPartsSwitcher.InstallBodyPart(SelectedPartGroup, SelectedPartID);
		CurrentPartsSwitcher.UpdateDirtiness();
		CurrentPartsSwitcher.UpdateColor(Merge: false);
	}

	private int TotalModsCost()
	{
		int num = 0;
		PartGroup[] array = partGroupsBeforeEnteringCustomization;
		foreach (PartGroup partGroup in array)
		{
			PartGroup[] partGroups = CurrentPartsSwitcher.partGroups;
			foreach (PartGroup partGroup2 in partGroups)
			{
				if (partGroup.GroupName == partGroup2.GroupName && partGroup.InstalledPart != partGroup2.InstalledPart && partGroup2.Parts[partGroup2.InstalledPart] != null && !CurrentVehicle.PurchasedPartsList.Contains(partGroup2.Parts[partGroup2.InstalledPart].name))
				{
					BodyPart part = VehicleParts.GetPart(CurrentVehicle.vehicleType, partGroup2.partType, partGroup2.Parts[partGroup2.InstalledPart].name);
					if (part != null)
					{
						num += part.partCost;
					}
				}
			}
		}
		PartGroup[] array2 = partGroupsBeforeEnteringCustomization;
		foreach (PartGroup partGroup3 in array2)
		{
			PartGroup[] partGroups2 = CurrentPartsSwitcher.partGroups;
			foreach (PartGroup partGroup4 in partGroups2)
			{
				if (partGroup4.GroupName == partGroup3.GroupName && partGroup4.color != partGroup3.color)
				{
					BodyPart part2 = VehicleParts.GetPart(VehicleType.Any, PartType.Other, "BodyPartPaint");
					if (part2 != null)
					{
						num += part2.partCost;
					}
				}
			}
		}
		SuspensionValue[] allValues = CurrentSuspensionController.FrontWheelsControls.GetAllValues();
		foreach (SuspensionValue suspensionValue in allValues)
		{
			SuspensionValue[] allValues2 = FrontWheelsBeforeEnteringCustomization.GetAllValues();
			foreach (SuspensionValue suspensionValue2 in allValues2)
			{
				if (suspensionValue.ValueName == suspensionValue2.ValueName && suspensionValue.valueType == ValueType.Int && !CurrentVehicle.PurchasedPartsList.Contains(suspensionValue.ValueName + suspensionValue.IntValue))
				{
					string partName = suspensionValue.ValueName + suspensionValue.IntValue.ToString();
					BodyPart part3 = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, partName);
					if (part3 != null)
					{
						num += part3.partCost;
					}
				}
			}
		}
		if (FrontRimsColorBeforeEngeringCustomizaiton != CurrentPartsSwitcher.FRimsColor)
		{
			BodyPart part4 = VehicleParts.GetPart(VehicleType.Any, PartType.Other, "RimPaint");
			if (part4 != null)
			{
				num += part4.partCost;
			}
		}
		if (FrontBeadlockColorBeforeEnteringCustomization != CurrentPartsSwitcher.FBeadlocksColor)
		{
			BodyPart part5 = VehicleParts.GetPart(VehicleType.Any, PartType.Other, "BeadlockPaint");
			if (part5 != null)
			{
				num += part5.partCost;
			}
		}
		SuspensionValue[] allValues3 = CurrentSuspensionController.RearWheelsControls.GetAllValues();
		foreach (SuspensionValue suspensionValue3 in allValues3)
		{
			SuspensionValue[] allValues4 = RearWheelsBeforeEnteringCustomization.GetAllValues();
			foreach (SuspensionValue suspensionValue4 in allValues4)
			{
				if (!(suspensionValue3.ValueName == suspensionValue4.ValueName) || suspensionValue3.valueType != ValueType.Int || CurrentVehicle.PurchasedPartsList.Contains(suspensionValue3.ValueName + suspensionValue3.IntValue))
				{
					continue;
				}
				SuspensionValue[] allValues5 = CurrentSuspensionController.FrontWheelsControls.GetAllValues();
				foreach (SuspensionValue suspensionValue5 in allValues5)
				{
					if (suspensionValue3.ValueName == suspensionValue5.ValueName && suspensionValue3.IntValue != suspensionValue5.IntValue)
					{
						string partName2 = suspensionValue3.ValueName + suspensionValue3.IntValue.ToString();
						BodyPart part6 = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, partName2);
						if (part6 != null)
						{
							num += part6.partCost;
						}
					}
				}
			}
		}
		if (RearRimsColorBeforeEngeringCustomizaiton != CurrentPartsSwitcher.RRimsColor)
		{
			BodyPart part7 = VehicleParts.GetPart(VehicleType.Any, PartType.Other, "RimPaint");
			if (part7 != null)
			{
				num += part7.partCost;
			}
		}
		if (RearBeadlockColorBeforeEnteringCustomization != CurrentPartsSwitcher.RBeadlocksColor)
		{
			BodyPart part8 = VehicleParts.GetPart(VehicleType.Any, PartType.Other, "BeadlockPaint");
			if (part8 != null)
			{
				num += part8.partCost;
			}
		}
		return num;
	}

	private void UpdatePurchaseModsButton()
	{
		int num = TotalModsCost();
		if (num == 0)
		{
			TotalModsCostText.text = "FREE";
		}
		else
		{
			TotalModsCostText.text = "$" + num.ToString();
		}
		if (num == 0)
		{
			PurchaseModsButtonGold.SetActive(value: false);
			TotalModsCostGoldText.text = "0";
		}
		else
		{
			PurchaseModsButtonGold.SetActive(value: true);
			TotalModsCostGoldText.text = Utility.CashToGold(num).ToString();
		}
	}

	private void ChangeRimSide(Side side)
	{
		SelectedWheelsControls = ((side != 0) ? CurrentSuspensionController.RearWheelsControls : CurrentSuspensionController.FrontWheelsControls);
		SelectedRimID = SelectedWheelsControls.Rim.IntValue;
		if (side == Side.Front)
		{
			SetCameraTargetWithoutOffset(CurrentSuspensionController.CurrentFrontSuspension.transform.position, Instantly: false);
		}
		else
		{
			SetCameraTargetWithoutOffset(CurrentSuspensionController.CurrentRearSuspension.transform.position, Instantly: false);
		}
		CameraController.Instance.SetCameraPos(90f, 0f, 3f);
		BeadlockColorBar.SetActive(value: false);
		RimsColorBar.SetActive(value: false);
		currentSide = side;
		RimSideText.text = side.ToString();
		ChangeRim(SelectedRimID);
	}

	private void ChangeRim(int RimID)
	{
		SuspensionControlLimit limit = SuspensionControlLimits.getLimit((currentSide != 0) ? CurrentSuspensionController.CurrentRearSuspension.gameObject.name : CurrentSuspensionController.CurrentFrontSuspension.gameObject.name, "Rim");
		if (RimID > limit.iMax)
		{
			RimID = 0;
		}
		if (RimID < 0)
		{
			RimID = limit.iMax;
		}
		SelectedRimID = RimID;
		string text = "Rim" + RimID.ToString();
		BodyPart part = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, text);
		bool flag = CurrentVehicle.PurchasedPartsList.Contains(text);
		RimCostText.text = ((!flag && part.partCost != 0) ? ("$" + part.partCost.ToString()) : "Owned");
		SelectedWheelsControls.Rim.IntValue = RimID;
		CurrentSuspensionController.LoadWheels();
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void SetRimColor(Color color)
	{
		if (currentSide == Side.Front)
		{
			CurrentPartsSwitcher.FRimsColor = color;
		}
		else
		{
			CurrentPartsSwitcher.RRimsColor = color;
		}
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void SetBeadlockColor(Color color)
	{
		if (currentSide == Side.Front)
		{
			CurrentPartsSwitcher.FBeadlocksColor = color;
		}
		else
		{
			CurrentPartsSwitcher.RBeadlocksColor = color;
		}
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void SetBodyPartColor(Color color)
	{
		SelectedPartGroup.color = color;
		SelectedPartGroup.PaintPart();
	}

	private void UpdatePaintTypeButtons()
	{
		BuyGlossyPaintButton.SetActive(!CurrentPartsSwitcher.GlossyPaintPurchased);
		SetGlossyPaintButton.SetActive(CurrentPartsSwitcher.GlossyPaintPurchased);
		CurrentPaintTypeText.text = ((!CurrentPartsSwitcher.GlossyPaint) ? "Matte" : "Glossy");
	}

	private void BuyGlossyPaint(Currency currency)
	{
		int num = 10000;
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			ChangeGlossiness(Glossy: true);
			CurrentPartsSwitcher.GlossyPaintPurchased = true;
			CurrentVehicle.SaveOnlyGlossinessData();
			UpdatePaintTypeButtons();
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void ChangeGlossiness(bool Glossy)
	{
		CurrentPartsSwitcher.GlossyPaint = Glossy;
		CurrentPartsSwitcher.UpdateColor(Merge: false);
		CurrentVehicle.SaveOnlyGlossinessData();
		UpdatePaintTypeButtons();
	}

	private void ChangeTireSide(Side side)
	{
		SelectedWheelsControls = ((side != 0) ? CurrentSuspensionController.RearWheelsControls : CurrentSuspensionController.FrontWheelsControls);
		SelectedTireID = SelectedWheelsControls.Tire.IntValue;
		if (side == Side.Front)
		{
			SetCameraTargetWithoutOffset(CurrentSuspensionController.CurrentFrontSuspension.transform.position, Instantly: false);
		}
		else
		{
			SetCameraTargetWithoutOffset(CurrentSuspensionController.CurrentRearSuspension.transform.position, Instantly: false);
		}
		CameraController.Instance.SetCameraPos(90f, 0f, 3f);
		currentSide = side;
		TireSideText.text = side.ToString();
		ChangeTire(SelectedTireID);
	}

	private void ChangeTire(int TireID)
	{
		SuspensionControlLimit limit = SuspensionControlLimits.getLimit((currentSide != 0) ? CurrentSuspensionController.CurrentRearSuspension.gameObject.name : CurrentSuspensionController.CurrentFrontSuspension.gameObject.name, "Tire");
		if (TireID > limit.iMax)
		{
			TireID = 0;
		}
		if (TireID < 0)
		{
			TireID = limit.iMax;
		}
		SelectedTireID = TireID;
		string text = "Tire" + TireID.ToString();
		BodyPart part = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, text);
		bool flag = CurrentVehicle.PurchasedPartsList.Contains(text);
		TireCostText.text = ((!flag && part.partCost != 0) ? ("$" + part.partCost.ToString()) : "Owned");
		SelectedWheelsControls.Tire.IntValue = TireID;
		CurrentSuspensionController.LoadWheels();
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void ChangeSuspensionSide(Side side)
	{
		SelectedSuspension = ((side != 0) ? CurrentSuspensionController.CurrentRearSuspension : CurrentSuspensionController.CurrentFrontSuspension);
		SelectedSuspensionID = ((side != 0) ? CurrentSuspensionController.rearSuspension : CurrentSuspensionController.frontSuspension);
		currentSide = side;
		SuspensionSideText.text = side.ToString();
		ChangeSuspension(SelectedSuspensionID);
		CameraController.Instance.SetCameraPos((side != 0) ? 180 : 0, -20f, 4f);
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void ChangeSuspension(int SuspensionID)
	{
		List<Suspension> list = (currentSide != 0) ? CurrentSuspensionController.RearSuspensions : CurrentSuspensionController.FrontSuspensions;
		if (SuspensionID > list.Count - 1)
		{
			SuspensionID = 0;
		}
		if (SuspensionID < 0)
		{
			SuspensionID = list.Count - 1;
		}
		SelectedSuspensionID = SuspensionID;
		if (currentSide == Side.Front)
		{
			CurrentSuspensionController.SetFrontSuspension(SuspensionID);
			SelectedSuspension = CurrentSuspensionController.CurrentFrontSuspension;
		}
		else
		{
			CurrentSuspensionController.SetRearSuspension(SuspensionID);
			SelectedSuspension = CurrentSuspensionController.CurrentRearSuspension;
		}
		SuspensionPart suspension = Suspensions.GetSuspension(CurrentVehicle.vehicleType, SelectedSuspension.gameObject.name);
		SuspensionCostText.text = ((!CurrentVehicle.PurchasedPartsList.Contains(SelectedSuspension.gameObject.name) && suspension.partCost != 0) ? ("$" + suspension.partCost) : "Owned");
		SuspensionCostGoldText.text = ((!CurrentVehicle.PurchasedPartsList.Contains(SelectedSuspension.gameObject.name) && suspension.partCost != 0) ? Utility.CashToGold(suspension.partCost).ToString() : "--");
		SuspensionDescriptionText.text = suspension.partDescription;
		SuspensionNameText.text = suspension.displayedName;
		CurrentPartsSwitcher.GenerateRimsTexture();
	}

	private void InstallChosenSuspension(Currency currency)
	{
		SuspensionPart suspension = Suspensions.GetSuspension(CurrentVehicle.vehicleType, SelectedSuspension.gameObject.name);
		int num = (!CurrentVehicle.PurchasedPartsList.Contains(SelectedSuspension.gameObject.name)) ? suspension.partCost : 0;
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			if (!CurrentVehicle.PurchasedPartsList.Contains(SelectedSuspension.gameObject.name))
			{
				CurrentVehicle.PurchasedPartsList.Add(SelectedSuspension.gameObject.name);
			}
			CurrentVehicle.SaveVehicleData();
			ShowMessage(suspension.displayedName + " installed");
			ChangeSuspension(SelectedSuspensionID);
		}
		else if (currency == Currency.Money && (float)num / 100f <= (float)GameState.LoadStatsData().Gold)
		{
			InstallChosenSuspension(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void ChangeSuspensionAdjustmentsSide(Side side)
	{
		CameraController.Instance.SetCameraPos((side != 0) ? 180 : 0, -20f, 4f);
		currentSide = side;
		SelectedSuspension = ((side != 0) ? CurrentSuspensionController.CurrentRearSuspension : CurrentSuspensionController.CurrentFrontSuspension);
		SuspensionPart suspension = Suspensions.GetSuspension(CurrentVehicle.vehicleType, SelectedSuspension.gameObject.name);
		SuspensionNameInUpgradeBarText.text = suspension.displayedName;
		UpdateSuspensionUpgradeButton();
		BuildSuspensionAdjustmentsList();
		SuspensionAdjustmentSlider.gameObject.SetActive(value: false);
	}

	private void BuildSuspensionAdjustmentsList()
	{
		ClearAdjustments();
		SuspensionValue[] controlValues = SelectedSuspension.GetControlValues();
		LoadedAdjustmentButtons.Add(FirstAdjustmentButton);
		bool flag = false;
		for (int i = 0; i < controlValues.Length; i++)
		{
			SuspensionControlLimit limit = SuspensionControlLimits.getLimit(SelectedSuspension.gameObject.name, controlValues[i].ValueName);
			if (limit != null && limit.ModifiableByPlayer)
			{
				if (!flag)
				{
					int firstID = i;
					FirstAdjustmentButton.onClick.RemoveAllListeners();
					FirstAdjustmentButton.onClick.AddListener(delegate
					{
						SelectSuspensionAdjustment(firstID);
					});
					FirstAdjustmentButton.GetComponentInChildren<Text>().text = controlValues[i].ValueName;
					flag = true;
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(FirstAdjustmentButton.gameObject);
					gameObject.transform.parent = FirstAdjustmentButton.transform.parent;
					gameObject.transform.localScale = FirstAdjustmentButton.transform.localScale;
					Button component = gameObject.GetComponent<Button>();
					int ID = i;
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate
					{
						SelectSuspensionAdjustment(ID);
					});
					component.GetComponentInChildren<Text>().text = controlValues[i].ValueName;
					LoadedAdjustmentButtons.Add(component);
				}
			}
		}
	}

	private void BuildGearingList()
	{
		ClearAdjustments();
		LoadedAdjustmentButtons.Add(FirstGearButton);
		GearingTutorialWindow.SetActive(value: false);
		UpdateGearingUpgradeButton();
		GearingAdjustmentSlider.gameObject.SetActive(value: false);
		bool flag = false;
		for (int i = 0; i < CurrentCarController.MaxGear; i++)
		{
			if (!flag)
			{
				int firstID = i;
				FirstGearButton.onClick.RemoveAllListeners();
				FirstGearButton.onClick.AddListener(delegate
				{
					SelectGear(firstID);
				});
				FirstGearButton.GetComponentInChildren<Text>().text = GetGearName(firstID);
				flag = true;
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(FirstGearButton.gameObject);
				gameObject.transform.parent = FirstGearButton.transform.parent;
				gameObject.transform.localScale = FirstGearButton.transform.localScale;
				Button component = gameObject.GetComponent<Button>();
				int ID = i;
				component.onClick.RemoveAllListeners();
				component.onClick.AddListener(delegate
				{
					SelectGear(ID);
				});
				component.GetComponentInChildren<Text>().text = GetGearName(i);
				LoadedAdjustmentButtons.Add(component);
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(FirstGearButton.gameObject);
		gameObject2.transform.parent = FirstGearButton.transform.parent;
		gameObject2.transform.localScale = FirstGearButton.transform.localScale;
		Button component2 = gameObject2.GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(delegate
		{
			SelectGear(-1);
		});
		component2.GetComponentInChildren<Text>().text = "Low gear";
		LoadedAdjustmentButtons.Add(component2);
	}

	private void SetupAdjustmentSlider(AdjustmentSlider slider, SuspensionValue value, int Stage)
	{
		if (value != null)
		{
			SuspensionControlLimit limit = SuspensionControlLimits.getLimit(SelectedSuspension.gameObject.name, value.ValueName);
			switch (value.valueType)
			{
			case ValueType.Float:
			{
				float maxClamp = limit.fDef + (limit.fMax - limit.fDef) / 5f * (float)(Stage + 1);
				float minClamp = limit.fDef - (limit.fDef - limit.fMin) / 5f * (float)(Stage + 1);
				slider.SetupFloatValue(value.ValueName, limit.fMin, limit.fMax, minClamp, maxClamp, value.FloatValue);
				break;
			}
			case ValueType.Int:
			{
				int iMin = limit.iMin;
				int iMax = limit.iMax;
				slider.SetupIntValue(value.ValueName, limit.iMin, limit.iMax, iMax, iMin, value.IntValue);
				break;
			}
			}
		}
	}

	private void ClearAdjustments()
	{
		if (LoadedAdjustmentButtons != null)
		{
			for (int i = 1; i < LoadedAdjustmentButtons.Count; i++)
			{
				UnityEngine.Object.Destroy(LoadedAdjustmentButtons[i].gameObject);
			}
		}
		LoadedAdjustmentButtons = new List<Button>();
	}

	private string GetGearName(int GearID)
	{
		string result = string.Empty;
		switch (GearID)
		{
		case -1:
			result = "Low gear";
			break;
		case 0:
			result = "1st gear";
			break;
		case 1:
			result = "2nd gear";
			break;
		case 2:
			result = "3rd gear";
			break;
		case 3:
			result = "4th gear";
			break;
		case 4:
			result = "5th gear";
			break;
		}
		return result;
	}

	private void SelectGear(int ID)
	{
		SelectedGear = ID;
		GearingAdjustmentSlider.gameObject.SetActive(value: true);
		UpdateGearingSlider();
	}

	private void UpdateGearingSlider()
	{
		string gearName = GetGearName(SelectedGear);
		float defaultGear = GearsManager.GetDefaultGear(SelectedGear);
		float minLimit = GearsManager.GetMinLimit(SelectedGear);
		float maxLimit = GearsManager.GetMaxLimit(SelectedGear);
		float maxClamp = defaultGear + (maxLimit - defaultGear) / 5f * (float)(CurrentCarController.GearingStage + 1);
		float minClamp = defaultGear - (defaultGear - minLimit) / 5f * (float)(CurrentCarController.GearingStage + 1);
		float num = 0f;
		num = ((SelectedGear >= 0) ? CurrentCarController.GearRatios[SelectedGear] : CurrentCarController.LowGearRatio);
		GearingAdjustmentSlider.SetupFloatValue(gearName, minLimit, maxLimit, minClamp, maxClamp, num);
	}

	private void SelectSuspensionAdjustment(int ID)
	{
		SelectedSuspensionValue = SelectedSuspension.GetControlValues()[ID];
		SetupAdjustmentSlider(SuspensionAdjustmentSlider, SelectedSuspensionValue, SelectedSuspension.UpgradeStage);
		SuspensionAdjustmentSlider.gameObject.SetActive(value: true);
	}

	private void SuspensionAdjustmentChanged()
	{
		SelectedSuspensionValue.FloatValue = SuspensionAdjustmentSlider.slider.value;
		SelectedSuspensionValue.IntValue = (int)SuspensionAdjustmentSlider.slider.value;
		SelectedSuspension.OnValidate();
	}

	private void GearValueChanged()
	{
		if (SelectedGear >= 0)
		{
			CurrentCarController.GearRatios[SelectedGear] = GearingAdjustmentSlider.slider.value;
		}
		else
		{
			CurrentCarController.LowGearRatio = GearingAdjustmentSlider.slider.value;
		}
	}

	private void UpgradeSelectedSuspension(Currency currency)
	{
		int stage = SelectedSuspension.UpgradeStage + 1;
		int num = Suspensions.GetSuspensionUpgrade(SelectedSuspension.gameObject.name, stage).upgradeCost;
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			SelectedSuspension.UpgradeStage++;
			CurrentVehicle.SaveVehicleData();
			UpdateSuspensionUpgradeButton();
			SetupAdjustmentSlider(SuspensionAdjustmentSlider, SelectedSuspensionValue, stage);
		}
		else if (currency == Currency.Money && (float)num / 100f <= (float)GameState.LoadStatsData().Gold)
		{
			UpgradeSelectedSuspension(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void UpgradeGearingStage()
	{
		int stage = CurrentCarController.GearingStage + 1;
		int partCost = PowerParts.GetPart(VehicleType.Any, PowerPartType.Gearing, stage).partCost;
		partCost = Utility.CashToGold(partCost);
		if (ProcessPurchase(Currency.Gold, partCost))
		{
			CurrentCarController.GearingStage++;
			CurrentVehicle.SaveVehicleData();
			UpdateGearingUpgradeButton();
			UpdateGearingSlider();
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void UpdateSuspensionUpgradeButton()
	{
		int upgradeStage = SelectedSuspension.UpgradeStage;
		SuspensionUpgrade suspensionUpgrade = Suspensions.GetSuspensionUpgrade(SelectedSuspension.gameObject.name, upgradeStage + 1);
		SuspensionUpgradeButton.interactable = (upgradeStage < 4);
		SuspensionUpgradeButtonGold.interactable = (upgradeStage < 4);
		SuspensionUpgradeCostText.text = ((upgradeStage >= 4) ? "MAX" : ("$" + suspensionUpgrade.upgradeCost));
		SuspensionUpgradeCostGoldText.text = ((upgradeStage >= 4) ? "--" : Utility.CashToGold(suspensionUpgrade.upgradeCost).ToString());
		SuspensionStageInUpgradeBarText.text = "Stage " + (upgradeStage + 1).ToString();
	}

	private void UpdateGearingUpgradeButton()
	{
		int gearingStage = CurrentCarController.GearingStage;
		PowerPart part = PowerParts.GetPart(VehicleType.Any, PowerPartType.Gearing, gearingStage + 1);
		GearingUpgradeButton.interactable = (gearingStage < 4);
		GearingUpgradeCostText.text = ((gearingStage >= 4) ? "--" : Utility.CashToGold(part.partCost).ToString());
		GearingStageInUpgradeBarText.text = "Stage " + (gearingStage + 1).ToString();
	}

	private void UpdateWheelsUpgradeButton()
	{
		int stage = SelectedWheelsControls.Stage;
		WheelsUpgrade wheelsUpgrade = Suspensions.GetWheelsUpgrade(stage + 1);
		WheelsUpgradeButton.interactable = (stage < 4);
		WheelsUpgradeButtonGold.interactable = (stage < 4);
		WheelsUpgradeCostText.text = ((stage >= 4) ? "MAX" : ("$" + wheelsUpgrade.upgradeCost));
		WheelsUpgradeCostGoldText.text = ((stage >= 4) ? "--" : Utility.CashToGold(wheelsUpgrade.upgradeCost).ToString());
		WheelsStageText.text = "Stage " + (stage + 1).ToString();
	}

	private void UpgradeSelectedWheels(Currency currency)
	{
		int stage = SelectedWheelsControls.Stage + 1;
		int num = Suspensions.GetWheelsUpgrade(stage).upgradeCost;
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (ProcessPurchase(currency, num))
		{
			SelectedWheelsControls.Stage++;
			CurrentVehicle.SaveVehicleData();
			UpdateWheelsUpgradeButton();
			SetupAdjustmentSlider(WheelsAdjustmentSlider, SelectedSuspensionValue, stage);
		}
		else if (currency == Currency.Money && (float)num / 100f <= (float)GameState.LoadStatsData().Gold)
		{
			UpgradeSelectedWheels(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void ChangeWheelsSide(Side side)
	{
		currentSide = side;
		SelectedWheelsControls = ((side != 0) ? CurrentSuspensionController.RearWheelsControls : CurrentSuspensionController.FrontWheelsControls);
		SelectedSuspension = ((side != 0) ? CurrentSuspensionController.CurrentRearSuspension : CurrentSuspensionController.CurrentFrontSuspension);
		WheelsSideText.text = ((side != 0) ? "Rear wheels" : "Front wheels");
		UpdateWheelsUpgradeButton();
		WheelsAdjustmentSlider.gameObject.SetActive(value: false);
	}

	private void SelectRimSizeAdjustment()
	{
		SelectedSuspensionValue = SelectedWheelsControls.RimSize;
		SetupAdjustmentSlider(WheelsAdjustmentSlider, SelectedWheelsControls.RimSize, SelectedWheelsControls.Stage);
		WheelsAdjustmentSlider.gameObject.SetActive(value: true);
	}

	private void SelectWheelsRadiusAdjustment()
	{
		SelectedSuspensionValue = SelectedWheelsControls.WheelsRadius;
		SetupAdjustmentSlider(WheelsAdjustmentSlider, SelectedWheelsControls.WheelsRadius, SelectedWheelsControls.Stage);
		WheelsAdjustmentSlider.gameObject.SetActive(value: true);
	}

	private void SelectWheelsWidthAdjustment()
	{
		SelectedSuspensionValue = SelectedWheelsControls.WheelsWidth;
		SetupAdjustmentSlider(WheelsAdjustmentSlider, SelectedWheelsControls.WheelsWidth, SelectedWheelsControls.Stage);
		WheelsAdjustmentSlider.gameObject.SetActive(value: true);
	}

	private void WheelsAdjustmentChanged()
	{
		SelectedSuspensionValue.FloatValue = WheelsAdjustmentSlider.slider.value;
		CurrentSuspensionController.DoWheelsSize();
	}

	private void UninstallPowerPart()
	{
		switch (SelectedPowerPartType)
		{
		case PowerPartType.Blower:
			CurrentCarController.BlowerStage = 0;
			LoadPowerSubtypeScreen(SelectedPowerPartType);
			break;
		case PowerPartType.Turbo:
			CurrentCarController.TurboStage = 0;
			LoadPowerSubtypeScreen(SelectedPowerPartType);
			break;
		}
		CurrentCarController.UpdateEngineModel();
		CurrentVehicle.SaveVehicleData();
	}

	private void LoadPowerSubtypeScreen(PowerPartType partType)
	{
		if (partType == PowerPartType.Diesel && CurrentVehicle.vehicleType != VehicleType.Truck)
		{
			ShowMessage("Can't diesel swap this kind of vehicle!");
			return;
		}
		if (partType == PowerPartType.TankTracks && CurrentVehicle.vehicleType != VehicleType.Truck && CurrentVehicle.vehicleType != VehicleType.Crawler)
		{
			ShowMessage("Can't install tracks on this kind of vehicle!");
			return;
		}
		DescriptionPanel.SetActive(value: true);
		UninstallButton.gameObject.SetActive(value: false);
		SelectedPowerPartType = partType;
		int num = 4;
		switch (partType)
		{
		case PowerPartType.EnginePower:
			TypeText.text = "Engine power";
			TypeImage.sprite = EngineBlockImage;
			CurrentPowerPartStage = CurrentCarController.EngineBlockStage;
			partType = PowerPartType.EngineBlock;
			if (CurrentCarController.EngineBlockStage >= 4)
			{
				CurrentPowerPartStage = CurrentCarController.HeadStage;
				partType = PowerPartType.Head;
			}
			if (CurrentCarController.HeadStage >= 4)
			{
				CurrentPowerPartStage = CurrentCarController.ValvetrainStage;
				partType = PowerPartType.Valvetrain;
			}
			break;
		case PowerPartType.EngineBlock:
			TypeText.text = "Engine block";
			TypeImage.sprite = EngineBlockImage;
			CurrentPowerPartStage = CurrentCarController.EngineBlockStage;
			break;
		case PowerPartType.Head:
			TypeText.text = "Head";
			TypeImage.sprite = HeadsImage;
			CurrentPowerPartStage = CurrentCarController.HeadStage;
			break;
		case PowerPartType.Valvetrain:
			TypeText.text = "Valvetrain";
			TypeImage.sprite = ValvetrainImage;
			CurrentPowerPartStage = CurrentCarController.ValvetrainStage;
			break;
		case PowerPartType.Grip:
			TypeText.text = "Grip";
			TypeImage.sprite = GripImage;
			CurrentPowerPartStage = CurrentCarController.GripStage;
			break;
		case PowerPartType.Weight:
			TypeText.text = "Weight";
			TypeImage.sprite = WeightImage;
			CurrentPowerPartStage = CurrentCarController.WeightStage;
			break;
		case PowerPartType.Diesel:
			TypeText.text = "Diesel Swap";
			TypeImage.sprite = DieselImage;
			CurrentPowerPartStage = CurrentCarController.DieselStage;
			break;
		case PowerPartType.Durability:
			TypeText.text = "Durability";
			TypeImage.sprite = TitanImage;
			CurrentPowerPartStage = CurrentCarController.DurabilityStage;
			break;
		case PowerPartType.Gearbox:
			TypeText.text = "Gearbox type";
			TypeImage.sprite = GearboxImage;
			CurrentPowerPartStage = (int)CurrentCarController.transmissionType;
			num = 1;
			break;
		case PowerPartType.Ebrake:
			TypeText.text = "E-brake";
			TypeImage.sprite = EbrakeImage;
			CurrentPowerPartStage = CurrentCarController.Ebrake;
			num = 1;
			break;
		case PowerPartType.TankTracks:
			TypeText.text = "Tracks";
			TypeImage.sprite = TankTracksImage;
			CurrentPowerPartStage = (CurrentSuspensionController.FrontWheelsControls.TankTracks ? 1 : 0);
			num = 1;
			break;
		case PowerPartType.Blower:
			TypeText.text = "Blower";
			TypeImage.sprite = BlowerImage;
			CurrentPowerPartStage = CurrentCarController.BlowerStage;
			if (CurrentCarController.BlowerStage > 0 && CurrentCarController.BlowerStage == CurrentCarController.PurchasedBlowerStage)
			{
				UninstallButton.gameObject.SetActive(value: true);
			}
			break;
		case PowerPartType.Turbo:
			TypeText.text = "Turbo";
			TypeImage.sprite = TurboImage;
			CurrentPowerPartStage = CurrentCarController.TurboStage;
			if (CurrentCarController.TurboStage > 0 && CurrentCarController.TurboStage == CurrentCarController.PurchasedTurboStage)
			{
				UninstallButton.gameObject.SetActive(value: true);
			}
			break;
		}
		if (partType == PowerPartType.Diesel && CurrentPowerPartStage < 3)
		{
			CurrentPowerPartStage = 3;
		}
		UpdatePowerStats();
		if (CurrentPowerPartStage < num)
		{
			DescriptionText.text = PowerParts.GetPart(CurrentVehicle.vehicleType, partType, CurrentPowerPartStage).Description;
			StageText.text = "Stage " + (CurrentPowerPartStage + 1).ToString();
		}
		else
		{
			DescriptionText.text = PowerParts.GetPart(CurrentVehicle.vehicleType, partType, CurrentPowerPartStage).Description;
			StageText.text = "Stage " + CurrentPowerPartStage.ToString();
		}
		if (partType == PowerPartType.Diesel)
		{
			StageText.text = ((CurrentCarController.DieselStage != 4) ? "Gas" : "Diesel");
		}
		if (partType == PowerPartType.TankTracks)
		{
			StageText.text = ((!CurrentSuspensionController.FrontWheelsControls.TankTracks) ? "Not installed" : "Installed");
		}
		if (partType == PowerPartType.Gearbox)
		{
			if (CurrentCarController.transmissionType == TransmissionType.AT)
			{
				StageText.text = "Stage - A/T";
			}
			else
			{
				StageText.text = "Stage - Manual";
			}
		}
		if (partType == PowerPartType.Ebrake)
		{
			if (CurrentCarController.Ebrake == 0)
			{
				StageText.text = "Stage - Not installed";
			}
			else
			{
				StageText.text = "Stage - Installed";
			}
		}
		switch (CurrentPowerPartStage)
		{
		case -1:
			StageIcon.sprite = Stage1Icon;
			break;
		case 0:
			StageIcon.sprite = Stage2Icon;
			break;
		case 1:
			StageIcon.sprite = Stage3Icon;
			break;
		case 2:
			StageIcon.sprite = Stage4Icon;
			break;
		case 3:
			StageIcon.sprite = Stage5Icon;
			break;
		}
		UpgradeButton.interactable = (CurrentPowerPartStage < num || partType == PowerPartType.Gearbox || partType == PowerPartType.Diesel);
		UpgradeButtonGold.interactable = (CurrentPowerPartStage < num);
		if (partType == PowerPartType.Gearbox && CurrentCarController.ManualTransmissionPurchased)
		{
			UpgradeButtonGold.interactable = false;
		}
		if (partType == PowerPartType.Diesel && CurrentCarController.DieselPurchased)
		{
			UpgradeButtonGold.interactable = false;
		}
		if (partType == PowerPartType.TankTracks)
		{
			UpgradeButtonGold.interactable = !CurrentCarController.TankTracksPurchased;
			UpgradeButton.interactable = CurrentCarController.TankTracksPurchased;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (CurrentPowerPartStage == num)
		{
			empty = "MAXED OUT!";
			empty2 = "--";
			UpgradeCostDieselText.text = "MAXED OUT!";
		}
		else
		{
			int partCost = PowerParts.GetPart(CurrentVehicle.vehicleType, partType, CurrentPowerPartStage + 1).partCost;
			empty = partCost.ToString("$0,0");
			empty2 = Utility.CashToGold(partCost).ToString();
		}
		if (partType == PowerPartType.TankTracks)
		{
			empty = "Gold Only";
		}
		if (partType == PowerPartType.Gearbox && CurrentCarController.ManualTransmissionPurchased)
		{
			empty2 = "--";
			empty = ((CurrentCarController.transmissionType != TransmissionType.Manual) ? "Install Manual" : "Back to A/T");
		}
		if (partType == PowerPartType.TankTracks && CurrentCarController.TankTracksPurchased)
		{
			empty2 = "--";
			empty = ((!CurrentSuspensionController.FrontWheelsControls.TankTracks) ? "Install tracks" : "Back to wheels");
		}
		if (partType == PowerPartType.Diesel && CurrentCarController.DieselPurchased)
		{
			empty2 = "--";
			empty = ((CurrentCarController.DieselStage != 4) ? "Swap to diesel" : "Back to gas");
		}
		if (partType == PowerPartType.Ebrake && CurrentCarController.Ebrake == 1)
		{
			empty = "Installed";
		}
		if (partType == PowerPartType.Blower && CurrentCarController.PurchasedBlowerStage > CurrentCarController.BlowerStage)
		{
			empty = "Install";
			empty2 = "0";
			UpgradeButtonGold.interactable = false;
		}
		if (partType == PowerPartType.Turbo && CurrentCarController.PurchasedTurboStage > CurrentCarController.TurboStage)
		{
			empty = "Install";
			empty2 = "0";
			UpgradeButtonGold.interactable = false;
		}
		UpgradeCostText.text = empty;
		UpgradeCostGoldText.text = empty2;
		SelectedSubPowerPartType = partType;
	}

	private void UpgradePowerPart(Currency currency)
	{
		PowerPart part = PowerParts.GetPart(CurrentVehicle.vehicleType, SelectedSubPowerPartType, CurrentPowerPartStage + 1);
		if (CurrentCarController.DieselStage == 4)
		{
			CurrentCarController.DieselPurchased = true;
		}
		int num = 0;
		if (part != null)
		{
			num = part.partCost;
		}
		else
		{
			UnityEngine.Debug.Log("Part was null: " + CurrentVehicle.vehicleType + " : " + SelectedSubPowerPartType + " : " + (CurrentPowerPartStage + 1).ToString());
		}
		UnityEngine.Debug.Log("Cost: " + num);
		if (currency == Currency.Gold)
		{
			num = Utility.CashToGold(num);
		}
		if (SelectedSubPowerPartType == PowerPartType.Gearbox && CurrentCarController.ManualTransmissionPurchased)
		{
			num = 0;
		}
		if (SelectedSubPowerPartType == PowerPartType.TankTracks && CurrentCarController.TankTracksPurchased)
		{
			num = 0;
		}
		if (SelectedSubPowerPartType == PowerPartType.Diesel && CurrentCarController.DieselPurchased)
		{
			num = 0;
		}
		if (SelectedSubPowerPartType == PowerPartType.Blower && CurrentCarController.BlowerStage == 0 && CurrentCarController.PurchasedBlowerStage > 0)
		{
			num = 0;
		}
		if (SelectedSubPowerPartType == PowerPartType.Turbo && CurrentCarController.TurboStage == 0 && CurrentCarController.PurchasedTurboStage > 0)
		{
			num = 0;
		}
		if (ProcessPurchase(currency, num))
		{
			switch (SelectedSubPowerPartType)
			{
			case PowerPartType.EnginePower:
				if (CurrentCarController.EngineBlockStage < 4)
				{
					CurrentCarController.EngineBlockStage++;
				}
				else if (CurrentCarController.HeadStage < 4)
				{
					CurrentCarController.HeadStage++;
				}
				else if (CurrentCarController.ValvetrainStage < 4)
				{
					CurrentCarController.ValvetrainStage++;
				}
				break;
			case PowerPartType.EngineBlock:
				CurrentCarController.EngineBlockStage++;
				break;
			case PowerPartType.Head:
				CurrentCarController.HeadStage++;
				break;
			case PowerPartType.Valvetrain:
				CurrentCarController.ValvetrainStage++;
				break;
			case PowerPartType.Grip:
				CurrentCarController.GripStage++;
				break;
			case PowerPartType.Weight:
				CurrentCarController.WeightStage++;
				break;
			case PowerPartType.Durability:
				CurrentCarController.DurabilityStage++;
				break;
			case PowerPartType.Diesel:
				if (CurrentCarController.BlowerStage > 0)
				{
					ShowMessage("Uninstall blower first!");
					return;
				}
				if (CurrentCarController.DieselStage == 3)
				{
					CurrentCarController.DieselStage = 4;
					CurrentCarController.DieselPurchased = true;
				}
				else
				{
					CurrentCarController.DieselStage = 3;
				}
				break;
			case PowerPartType.Gearbox:
				if (CurrentCarController.transmissionType == TransmissionType.AT)
				{
					CurrentCarController.transmissionType = TransmissionType.Manual;
					CurrentCarController.ManualTransmissionPurchased = true;
				}
				else
				{
					CurrentCarController.transmissionType = TransmissionType.AT;
				}
				break;
			case PowerPartType.Ebrake:
				CurrentCarController.Ebrake = 1;
				break;
			case PowerPartType.TankTracks:
				if (!CurrentSuspensionController.FrontWheelsControls.TankTracks)
				{
					CurrentSuspensionController.FrontWheelsControls.TankTracks = true;
					CurrentSuspensionController.RearWheelsControls.TankTracks = true;
					CurrentCarController.TankTracksPurchased = true;
				}
				else
				{
					CurrentSuspensionController.FrontWheelsControls.TankTracks = false;
					CurrentSuspensionController.RearWheelsControls.TankTracks = false;
				}
				CurrentSuspensionController.LoadWheels();
				CurrentPartsSwitcher.GenerateRimsTexture();
				break;
			case PowerPartType.Blower:
				if (CurrentCarController.DieselStage == 4)
				{
					ShowMessage("Uninstall diesel first!");
					return;
				}
				if (CurrentCarController.TurboStage > 0)
				{
					ShowMessage("Uninstall turbo first!");
					return;
				}
				if (CurrentCarController.PurchasedBlowerStage > CurrentCarController.BlowerStage)
				{
					CurrentCarController.BlowerStage = CurrentCarController.PurchasedBlowerStage;
					break;
				}
				CurrentCarController.BlowerStage++;
				CurrentCarController.PurchasedBlowerStage = CurrentCarController.BlowerStage;
				break;
			case PowerPartType.Turbo:
				if (CurrentCarController.BlowerStage > 0)
				{
					ShowMessage("Uninstall blower first!");
					return;
				}
				if (CurrentCarController.PurchasedTurboStage > CurrentCarController.TurboStage)
				{
					CurrentCarController.TurboStage = CurrentCarController.PurchasedTurboStage;
					break;
				}
				CurrentCarController.TurboStage++;
				CurrentCarController.PurchasedTurboStage = CurrentCarController.TurboStage;
				break;
			}
			CurrentCarController.UpdateEngineModel();
			CurrentVehicle.SaveVehicleData();
			LoadPowerSubtypeScreen(SelectedPowerPartType);
		}
		else if (currency == Currency.Money && Utility.CashToGold(num) <= GameState.LoadStatsData().Gold)
		{
			UpgradePowerPart(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	private void UpdatePowerStats()
	{
		float fillAmount = ((float)CurrentCarController.EngineBlockStage + (float)CurrentCarController.HeadStage + (float)CurrentCarController.ValvetrainStage + 3f) / 15f;
		PowerBar.fillAmount = fillAmount;
		float fillAmount2 = ((float)CurrentCarController.GripStage + 1f) / 5f;
		GripBar.fillAmount = fillAmount2;
		float fillAmount3 = ((float)CurrentCarController.WeightStage + 1f) / 5f;
		WeightBar.fillAmount = fillAmount3;
		float fillAmount4 = ((float)CurrentCarController.DurabilityStage + 1f) / 5f;
		DurabilityBar.fillAmount = fillAmount4;
	}

	private void RepairVehicle(Currency currency)
	{
		if (!(CurrentVehicle == null))
		{
			int num = (int)((float)FullRepairPrice * (100f - CurrentCarController.CarHealth) / 100f);
			if (currency == Currency.Gold)
			{
				num = Mathf.Max(1, num / 100);
			}
			if (ProcessPurchase(currency, num))
			{
				CurrentCarController.CarHealth = 100f;
				UpdateSideButtons();
				CurrentVehicle.SaveVehicleData();
			}
			else if (currency == Currency.Money && Utility.CashToGold(num) <= GameState.LoadStatsData().Gold)
			{
				RepairVehicle(Currency.Gold);
			}
			else
			{
				ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
				LoadIAPMenu();
			}
		}
	}

	private void LoadWrap(int WrapID)
	{
		if (WrapID > 16)
		{
			WrapID = 0;
		}
		if (WrapID < 0)
		{
			WrapID = 16;
		}
		SelectedWrap = WrapID;
		WrapLayerCountText.text = "Layers: " + CurrentPartsSwitcher.WrapLayers.Count + "/5";
		WrapLayerCostText.text = ((CurrentPartsSwitcher.WrapLayers.Count != 0 && CurrentPartsSwitcher.WrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count && CurrentPartsSwitcher.SessionWrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count) ? "Apply   20" : "Apply");
		WrapGoldBars.SetActive(CurrentPartsSwitcher.WrapLayers.Count != 0 && CurrentPartsSwitcher.WrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count && CurrentPartsSwitcher.SessionWrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count);
		ApplyLayerButton.interactable = (WrapID > 0 && CurrentPartsSwitcher.WrapLayers.Count < 5);
		UpdateWrap();
		UpdateWrapPreview();
	}

	public void WrapOffsetChanged()
	{
		if (WrapCoordsSlidersInitialized)
		{
			WrapCoords = new Vector4(XOffsetSlider.value, YOffsetSlider.value, XTillingSlider.value, YTillingSlider.value);
			UpdateWrap();
		}
	}

	private void UpdateWrap()
	{
		WrapColor.a = TransparencySlider.value;
		if (WrapCoords == Vector4.zero)
		{
			WrapCoords = new Vector4(0f, 0f, 1f, 1f);
		}
		CurrentPartsSwitcher.ChangeCurrentWrap(SelectedWrap, WrapColor, WrapCoords);
	}

	private void UpdateWrapPreview()
	{
		Sprite x = Resources.Load<Sprite>("Wraps/WrapPreview" + SelectedWrap);
		if (x != null)
		{
			WrapPreviewImage.sprite = Resources.Load<Sprite>("Wraps/WrapPreview" + SelectedWrap);
		}
	}

	public void ApplyWrap()
	{
		if (SelectedWrap != 0)
		{
			int num = (CurrentPartsSwitcher.WrapLayers.Count != 0 && CurrentPartsSwitcher.WrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count && CurrentPartsSwitcher.SessionWrapLayerCount <= CurrentPartsSwitcher.WrapLayers.Count) ? 20 : 0;
			UnityEngine.Debug.Log("Layer Applying: " + CurrentPartsSwitcher.WrapLayers.Count);
			UnityEngine.Debug.Log("Layers Bought: " + CurrentPartsSwitcher.WrapLayerCount);
			UnityEngine.Debug.Log("Session Layers: " + CurrentPartsSwitcher.SessionWrapLayerCount);
			if (num == 0)
			{
				UnityEngine.Debug.Log("Free layer!");
			}
			if (ProcessPurchase(Currency.Gold, num))
			{
				CurrentPartsSwitcher.BakeWrap();
				ClearLayer();
			}
			else
			{
				ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
				LoadIAPMenu();
			}
		}
	}

	public void ClearLayer()
	{
		WrapCoords = new Vector4(0f, 0f, 1f, 1f);
		XOffsetSlider.value = 0f;
		YOffsetSlider.value = 0f;
		XTillingSlider.value = 1f;
		YTillingSlider.value = 1f;
		TransparencySlider.value = 1f;
		WrapColor = Color.white;
		LoadWrap(0);
	}

	public void RemoveAllLayers()
	{
		ClearLayer();
		CurrentPartsSwitcher.ClearWraps();
		LoadWrap(0);
	}

	private void LoadMenu(MenuState state, bool ThroughFade, bool FromMainMenu)
	{
		StartCoroutine(LoadMenuCoroutine(state, ThroughFade, FromMainMenu));
	}

	private IEnumerator LoadMenuCoroutine(MenuState state, bool ThroughFade, bool FromMainMenu)
	{
		if (ThroughFade)
		{
			if (GameState.unloadUnusedResourcesInGame)
			{
				Resources.UnloadUnusedAssets();
			}
			for (float f2 = 0f; f2 <= 1.1f; f2 += 0.1f)
			{
				FadeScreen.alpha = f2;
				yield return null;
			}
		}
		FadeScreen.alpha = 1f;
		//Logo.SetActive(value: false);
		Sidebar.gameObject.SetActive(value: false);
		menuState = state;
		UpdateScreen();
		switch (menuState)
		{
		case MenuState.Play:
			Resources.UnloadUnusedAssets();
			StatsPanel.SetActive(value: true);
			GameState.Clear();
			if (!PhotonNetwork.insideLobby)
			{
				MultiplayerManager.Connect();
			}
			break;
		case MenuState.Map:
		{
			StatsData statsData = GameState.LoadStatsData();
			StatsPanel.SetActive(value: true);
			DesertLockXPText.text = UnlockDesertXP.ToString() + "XP";
			DesertLockPanel.SetActive(statsData.XP < UnlockDesertXP && !DataStore.GetBool("MapDesertNGUnlocked") && !statsData.IsMember);
			RockParkLockXPText.text = UnlockRockParkXP.ToString() + "XP";
			RockParkLockPanel.SetActive(statsData.XP < UnlockRockParkXP && !DataStore.GetBool("RockParkNGUnlocked") && !statsData.IsMember);
			StuntParkLockXPText.text = UnlockStuntParkXP.ToString() + "XP";
			StuntParkLockPabel.SetActive(statsData.XP < UnlockStuntParkXP && !DataStore.GetBool("StuntParkUnlocked") && !statsData.IsMember);
			break;
		}
		case MenuState.MultiplayerGameType:
			if (GameState.Password != null && GameState.Password != string.Empty)
			{
				MultiplayerPrivateButton.SetActive(value: false);
			}
			else
			{
				MultiplayerPrivateButton.SetActive(value: true);
			}
			StatsPanel.SetActive(value: true);
			break;
		case MenuState.PrivateMultiplayer:
			UnityEngine.Debug.Log("Private Multiplayer!");
			StatsPanel.SetActive(value: true);
			break;
		case MenuState.TruckSelector:
			SetCameraTarget(TruckSelectorSpawnPoint.position, Instantly: true);
			UnloadVehiclesInGarage();
			SelectedTruckIDInSelector = 0;
			switch (SelectedVehicleType)
			{
			case VehicleType.ATV:
				SelectedArray = ATVs;
				break;
			case VehicleType.Crawler:
				SelectedArray = Crawlers;
				break;
			case VehicleType.SideBySide:
				SelectedArray = SideBySides;
				break;
			case VehicleType.Truck:
				SelectedArray = StreetTrucks;
				break;
			case VehicleType.Bike:
				SelectedArray = Bikes;
				break;
			case VehicleType.TurnKey:
				SelectedArray = TurnKeyVehicles;
				break;
			case VehicleType.Trailer:
				SelectedArray = trailers;
				break;
			}
			UpdateVehicleSelector();
			break;
		case MenuState.TrailRaceLobby:
			CreateTrailRaceRoomsList();
			break;
		case MenuState.StorageArea:
		{
			SetCameraTarget(TruckSelectorSpawnPoint.position, Instantly: true);
			UnloadVehiclesInGarage();
			SelectedTruckIDInSelector = 0;
			string[] storageVehiclesIDs = GetStorageVehiclesIDs();
			StoredVehicles = new List<VehicleData>();
			for (int i = 0; i < storageVehiclesIDs.Length; i++)
			{
				string @string = DataStore.GetString(storageVehiclesIDs[i]);
				VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(@string);
				vehicleData.SavedID = storageVehiclesIDs[i];
				StoredVehicles.Add(vehicleData);
			}
			UpdateVehicleSelector(isStorage: true);
			break;
		}
		case MenuState.MainMenu:
			Sidebar.gameObject.SetActive(value: true);
			LoadVehiclesInGarage();
			if (SelectedVehicleInGarageID == 0)
			{
				SelectedVehicleInGarageID = GameState.SelectedGarageVehicleID;
			}
			if (SelectedVehicleInGarageID >= LoadedVehiclesInGarage.Count)
			{
				SelectedVehicleInGarageID = LoadedVehiclesInGarage.Count - 1;
			}
			if (LoadedVehiclesInGarage != null && LoadedVehiclesInGarage.Count > 0)
			{
				ChangeCurrentVehicle(LoadedVehiclesInGarage[SelectedVehicleInGarageID], InstantCameraMove: true);
			}
			else
			{
				SetCameraTarget(GarageVehiclePoints[0].position, Instantly: true);
			}
			UpdateSideButtons();
			//Logo.SetActive(value: true);
			break;
		case MenuState.CustomizeCategorySelector:
			if (FromMainMenu)
			{
				ModConfirmation.SetActive(value: false);
				GetCurrentCarToCustomizationPoint();
				PartGroup[] partGroups = CurrentPartsSwitcher.partGroups;
				partGroupsBeforeEnteringCustomization = new PartGroup[partGroups.Length];
				for (int j = 0; j < partGroups.Length; j++)
				{
					partGroupsBeforeEnteringCustomization[j] = partGroups[j].DeepCopy();
				}
				FrontWheelsBeforeEnteringCustomization = CurrentSuspensionController.FrontWheelsControls.DeepCopy();
				RearWheelsBeforeEnteringCustomization = CurrentSuspensionController.RearWheelsControls.DeepCopy();
				FrontRimsColorBeforeEngeringCustomizaiton = CurrentPartsSwitcher.FRimsColor;
				FrontBeadlockColorBeforeEnteringCustomization = CurrentPartsSwitcher.FBeadlocksColor;
				RearRimsColorBeforeEngeringCustomizaiton = CurrentPartsSwitcher.RRimsColor;
				RearBeadlockColorBeforeEnteringCustomization = CurrentPartsSwitcher.RBeadlocksColor;
				WashCostText.text = "$" + WashPrice.ToString();
			}
			ToggleBodyColorPicker(Show: false);
			SetCameraTarget(TruckSelectorSpawnPoint.position, Instantly: true);
			PurchaseModsButton.SetActive(!FromMainMenu);
			PurchaseModsButtonGold.SetActive(!FromMainMenu);
			UpdatePurchaseModsButton();
			UpdateCameraSettings();
			if (LightsOn)
			{
				ToggleLights();
			}
			ClearLayer();
			break;
		case MenuState.CustomizeBodyParts:
			ChangePartGroup(0);
			break;
		case MenuState.CustomizeRims:
			ChangeRimSide(Side.Front);
			break;
		case MenuState.CustomizeTires:
			ChangeTireSide(Side.Front);
			break;
		case MenuState.Power:
			if (FromMainMenu)
			{
				DescriptionPanel.SetActive(value: false);
				StatsPanel.SetActive(value: true);
				UpdatePowerStats();
				GetCurrentCarToCustomizationPoint();
				CameraController.Instance.GetComponent<Camera>().fieldOfView = 69f;
			}
			break;
		case MenuState.Drivetrain:
			UpdateCameraSettings();
			ClearAdjustments();
			GetCurrentCarToCustomizationPoint();
			break;
		case MenuState.SwitchSuspension:
			ChangeSuspensionSide(Side.Front);
			break;
		case MenuState.TuneSuspension:
			ChangeSuspensionAdjustmentsSide(Side.Front);
			break;
		case MenuState.TuneWheels:
			ChangeWheelsSide(Side.Front);
			break;
		case MenuState.TuneGearing:
			BuildGearingList();
			break;
		case MenuState.Dyno:
			if (DynoRoomController.Instance != null)
			{
				DynoRoomController.Instance.InitializeDyno(CurrentVehicle.gameObject);
				Utility.AlignVehicleByGround(CurrentVehicle.transform);
				SetCameraTarget(DynoRoomController.Instance.CarPos.position, Instantly: true);
				CameraController.Instance.SetCameraPos(90f, 5f, 5f);
				UpdateCameraSettings();
			}
			BuyTuningPackButton.interactable = !CurrentCarController.TuningEnginePurchased;
			BuyPerfectSetupButton.interactable = !CurrentCarController.PerfectSetupPurchased;
			DynoRunsLeftText.text = GameState.LoadStatsData().DynoRuns.ToString();
			EngineTuningSlider.gameObject.SetActive(value: false);
			DynoTutorialWindow.SetActive(value: false);
			break;
		case MenuState.TestSuspension:
			if (SuspensionTestRoomController.Instance != null)
			{
				SuspensionTestRoomController.Instance.InitializeSuspensionTest(CurrentVehicle.gameObject);
				SetCameraTarget(SuspensionTestRoomController.Instance.CarPositionPoint.position, Instantly: true);
				CameraController.Instance.SetCameraPos(160f, 5f, 5f);
				UpdateCameraSettings();
			}
			break;
		case MenuState.CustomizePaint:
			UpdatePaintTypeButtons();
			break;
		case MenuState.CustomizeWraps:
			WrapCoordsSlidersInitialized = false;
			SelectedWrap = CurrentPartsSwitcher.AppliedWrapID;
			WrapColor = CurrentPartsSwitcher.WrapColor;
			WrapCoords = CurrentPartsSwitcher.WrapCoords;
			ClearLayer();
			WrapColorPicker.ColorChangedCallback = ChooseCustomWrapColor;
			WrapColorPicker.Color = Color.white;
			LoadWrap(SelectedWrap);
			WrapCoordsSlidersInitialized = true;
			break;
		}
		if (ThroughFade)
		{
			for (float f = 1f; f >= 0f; f -= 0.1f)
			{
				FadeScreen.alpha = f;
				yield return null;
			}
		}
		FadeScreen.alpha = 0f;
	}

	public void CreateTrailRaceRoomsList(bool Randomize = true)
	{
		if (InstantiatedRoomBars != null && InstantiatedRoomBars.Count > 0)
		{
			int count = InstantiatedRoomBars.Count;
			for (int i = 0; i < count; i++)
			{
				UnityEngine.Object.Destroy(InstantiatedRoomBars[i]);
			}
		}
		InstantiatedRoomBars = new List<GameObject>();
		TrailRaceRooms = MultiplayerManager.GetAllTrailRaceRooms();
		if (Randomize)
		{
			RandomizeArray(TrailRaceRooms);
		}
		FirstTrailRaceElement.SetActive(TrailRaceRooms.Length > 0);
		NoRaceAvailableText.gameObject.SetActive(TrailRaceRooms.Length == 0);
		if (TrailRaceRooms.Length > 0)
		{
			string str = "  ";
			str += TrailRaceRooms[0].CustomProperties["HostPlayerName"].ToString();
			str += "   -   ";
			string str2 = "Trail";
			if (TrailRaceRooms[0].CustomProperties["TrailID"] != null)
			{
				str2 = Trails.trails[(int)TrailRaceRooms[0].CustomProperties["TrailID"]].TrailName;
			}
			str += str2;
			FirstTrailRaceElement.GetComponentInChildren<Text>().text = str;
		}
		if (TrailRaceRooms.Length <= 1)
		{
			return;
		}
		for (int j = 1; j < TrailRaceRooms.Length; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(FirstTrailRaceElement);
			gameObject.transform.parent = FirstTrailRaceElement.transform.parent;
			gameObject.transform.localScale = FirstTrailRaceElement.transform.localScale;
			string str3 = "  ";
			str3 += TrailRaceRooms[j].CustomProperties["HostPlayerName"].ToString();
			str3 += "   -   ";
			string str4 = "Trail";
			if (TrailRaceRooms[j].CustomProperties["TrailID"] != null)
			{
				str4 = Trails.trails[(int)TrailRaceRooms[j].CustomProperties["TrailID"]].TrailName;
			}
			str3 += str4;
			gameObject.GetComponentInChildren<Text>().text = str3;
			InstantiatedRoomBars.Add(gameObject);
		}
	}

	private void RandomizeArray<T>(T[] ar)
	{
		for (int i = 0; i < ar.Length; i++)
		{
			T val = ar[i];
			int num = UnityEngine.Random.Range(i, ar.Length);
			ar[i] = ar[num];
			ar[num] = val;
		}
	}

	private void UpdateSideButtons()
	{
		WashButton.SetActive(value: false);
		RepairIcon.SetActive(value: false);
		equipTrailerButton.SetActive(value: false);
		bool flag = Utility.EqiuppedTrailer() != string.Empty;
		bool flag2 = DataStore.GetString("VehicleOnTrailer") != string.Empty;
		unequipTrailerButton.SetActive(flag);
		loadCarOnTrailerButton.SetActive(flag && CurrentVehicle.vehicleType != VehicleType.Trailer && CurrentVehicle.vehicleType != VehicleType.Bike);
		unloadCarsFromTrailerButton.SetActive(flag && CurrentVehicle.vehicleType == VehicleType.Trailer && flag2);
		if (CurrentVehicle != null && CurrentPartsSwitcher != null)
		{
			WashButton.SetActive(CurrentPartsSwitcher.Dirtiness > 0f && !CurrentPartsSwitcher.Washing);
			CarController component = CurrentVehicle.GetComponent<CarController>();
			int a = (int)((float)FullRepairPrice * (100f - component.CarHealth) / 100f);
			StatsData statsData = GameState.LoadStatsData();
			a = Mathf.Min(a, statsData.Money);
			if (statsData.Money == 0)
			{
				CurrentCarController.CarHealth = 100f;
				CurrentVehicle.SaveVehicleData();
			}
			RepairCostText.text = "$" + a;
			RepairIcon.SetActive(a > 0);
		}
		if (CurrentVehicle != null && CurrentVehicle.vehicleType == VehicleType.Trailer && !CurrentVehicle.equipped)
		{
			equipTrailerButton.SetActive(value: true);
		}
	}

	public void PickMultiplayerGameType(int gameType)
	{
		GameState.GameType = (GameType)gameType;
		if (gameType != 3)
		{
			LoadMenu(MenuState.Map, ThroughFade: false, FromMainMenu: false);
		}
		else if (GameState.Password != null && GameState.Password != string.Empty)
		{
			LoadMenu(MenuState.TrailSelectorScreen, ThroughFade: false, FromMainMenu: false);
		}
		else
		{
			LoadMenu(MenuState.TrailRaceLobby, ThroughFade: false, FromMainMenu: false);
		}
	}

	public void GoPlaying(bool isMultiplayer)
	{
		GoPlaying(isMultiplayer, null);
	}

	public void GoPlaying(bool isMultiplayer, string password)
	{
		if (CurrentVehicle == null)
		{
			return;
		}
		if (isMultiplayer && !PhotonNetwork.connectedAndReady)
		{
			ShowMessage("You're not connected yet. Try again...");
			return;
		}
		GameState.Populate(CurrentVehicle.VehicleID, null, isMultiplayer ? GameMode.Multiplayer : GameMode.SinglePlayer, GameType.FreeRoam, password);
		if (isMultiplayer)
		{
			LoadMenu(MenuState.MultiplayerGameType, ThroughFade: false, FromMainMenu: false);
		}
		else
		{
			LoadMenu(MenuState.Map, ThroughFade: false, FromMainMenu: false);
		}
	}

	public void PickMap()
	{
		LoadMenu(MenuState.Map, ThroughFade: false, FromMainMenu: false);
	}

	public void ShowWaiting(string message)
	{
		SceneLoadingText.text = message;
		SceneLoading.SetActive(value: true);
	}

	public void HideWaiting()
	{
		SceneLoading.SetActive(value: false);
	}

	private void LoadCurrentComponents(GameObject currentVehicleObject)
	{
		CurrentVehicle = currentVehicleObject.GetComponent<VehicleDataManager>();
		CurrentCarController = currentVehicleObject.GetComponent<CarController>();
		CurrentPartsSwitcher = currentVehicleObject.GetComponent<BodyPartsSwitcher>();
		CurrentSuspensionController = currentVehicleObject.GetComponent<SuspensionController>();
	}

	private void GetCurrentCarToCustomizationPoint()
	{
		if (LoadedVehicleInSelector != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInSelector);
		}
		if (LoadedVehicleInCustomization != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInCustomization);
		}
		if (loadedVehicleOnTrailer != null)
		{
			UnityEngine.Object.Destroy(loadedVehicleOnTrailer);
		}
		string name = CurrentVehicle.name;
		string vehicleID = CurrentVehicle.VehicleID;
		UnloadVehiclesInGarage();
		LoadedVehicleInCustomization = LoadVehicle(name, TruckSelectorSpawnPoint, forDealership: false, string.Empty);
		LoadCurrentComponents(LoadedVehicleInCustomization);
		CurrentVehicle.VehicleID = vehicleID;
		CurrentVehicle.LoadVehicleData();
		CurrentPartsSwitcher.UpdateColor(Merge: false);
		CurrentPartsSwitcher.UpdateDirtiness();
		SetCameraTarget(TruckSelectorSpawnPoint.position, Instantly: true);
		StartCoroutine(PreventVehicleJumpingOnLoad(LoadedVehicleInCustomization));
		CameraController.Instance.SetCameraPos(30f, 15f, (CurrentCarController.GarageMaxDistance + CurrentCarController.GarageMinDistance) / 2f);
	}

	public void UpdateScreen()
	{
		MainMenu.SetActive(menuState == MenuState.MainMenu);
		TruckTypeSelector.SetActive(menuState == MenuState.TruckTypeSelector);
		StorageArea.SetActive(menuState == MenuState.StorageArea);
		TruckSelector.SetActive(menuState == MenuState.TruckSelector);
		CustomizeCategorySelector.SetActive(menuState == MenuState.CustomizeCategorySelector);
		CustomizeBodyParts.SetActive(menuState == MenuState.CustomizeBodyParts);
		CustomizePaint.SetActive(menuState == MenuState.CustomizePaint);
		CustomizeRims.SetActive(menuState == MenuState.CustomizeRims);
		CustomizeTires.SetActive(menuState == MenuState.CustomizeTires);
		CustomizeWraps.SetActive(menuState == MenuState.CustomizeWraps);
		Drivetrain.SetActive(menuState == MenuState.Drivetrain);
		Power.SetActive(menuState == MenuState.Power);
		SwitchSuspension.SetActive(menuState == MenuState.SwitchSuspension);
		TuneSuspension.SetActive(menuState == MenuState.TuneSuspension);
		TuneWheels.SetActive(menuState == MenuState.TuneWheels);
		TestSuspension.SetActive(menuState == MenuState.TestSuspension);
		TuneGearing.SetActive(menuState == MenuState.TuneGearing);
		Dyno.SetActive(menuState == MenuState.Dyno);
		BuyingDynoRuns.SetActive(menuState == MenuState.BuyingDynoRuns);
		DynoResult.SetActive(menuState == MenuState.DynoResult);
		PlayMenu.SetActive(menuState == MenuState.Play);
		MapMenu.SetActive(menuState == MenuState.Map);
		MultiplayerGameType.SetActive(menuState == MenuState.MultiplayerGameType);
		PrivateMultiplayer.SetActive(menuState == MenuState.PrivateMultiplayer);
		PINEntryScreen.SetActive(menuState == MenuState.EnterPIN);
		TrailRaceLobby.SetActive(menuState == MenuState.TrailRaceLobby);
		TrailSelectorScreen.SetActive(menuState == MenuState.TrailSelectorScreen);
		TrailRaceBetScreen.SetActive(menuState == MenuState.TrailRaceBetScreen);
		communityMapsScreen.SetActive(menuState == MenuState.CommunityMaps);
		communityMapsButton.SetActive(GameState.GameType != GameType.CaptureTheFlag);
		UpdateStats();
	}

	public void EquipTrailerConfirmed()
	{
		CurrentVehicle.equipped = true;
		CurrentVehicle.SaveVehicleData();
		UpdateSideButtons();
		equipTrailerWarning.SetActive(value: false);
		LoadMainMenu(FromMainMenu: true);
	}

	public void UnequipTrailers()
	{
		string @string = DataStore.GetString("VehiclesList");
		if (!(@string == string.Empty))
		{
			SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
			string[] array = savedVehiclesList.VehicleIDs.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				string string2 = DataStore.GetString(array[i]);
				VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(string2);
				vehicleData.equippedTrailer = false;
				string value = XmlSerialization.SerializeData<VehicleData>(vehicleData);
				DataStore.SetString(array[i], value);
			}
			UnloadCarsFromTrailer();
		}
	}

	public void LoadCarOnTrailer()
	{
		DataStore.SetString("VehicleOnTrailer", CurrentVehicle.VehicleID);
		LoadMainMenu(FromMainMenu: true);
	}

	public void UnloadCarsFromTrailer()
	{
		DataStore.SetString("VehicleOnTrailer", string.Empty);
		LoadMainMenu(FromMainMenu: true);
	}

	private void UpdateStats()
	{
		StatsData statsData = GameState.LoadStatsData();
		if (statsData == null)
		{
			statsData = new StatsData();
		}
		Gold = statsData.Gold;
		Money = statsData.Money;
		XP = statsData.XP;
		MoneyText.text = statsData.Money.ToString();
		GoldText.text = statsData.Gold.ToString();
		XPText.text = statsData.XP.ToString() + "XP";
		VehicleTypeText.text = ((!(CurrentVehicle != null)) ? "No truck" : CurrentVehicle.vehicleType.ToString());
		MembershipButton.SetActive(!statsData.IsMember);
		AlreadyMemberText.SetActive(statsData.IsMember);
	}

	private GameObject LoadVehicle(string PrefabName, Transform AtPoint, bool forDealership = false, string vehicleID = "")
	{
		string path = "Vehicles/" + PrefabName;
		if (SelectedArray == TurnKeyVehicles && menuState != 0)
		{
			path = "TurnKeyVehicles/" + PrefabName;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
		gameObject.name = PrefabName;
		gameObject.transform.position = AtPoint.position;
		gameObject.transform.rotation = AtPoint.rotation;
		UnityEngine.Debug.DrawRay(AtPoint.position, Vector3.up * 5f, Color.magenta, 10f);
		VehicleDataManager component = gameObject.GetComponent<VehicleDataManager>();
		bool flag = component.vehicleType == VehicleType.Trailer;
		if (gameObject.GetComponent<VehicleDataManager>().vehicleType != VehicleType.Bike)
		{
			gameObject.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)10;
		}
		if (!flag)
		{
			SuspensionController component2 = gameObject.GetComponent<SuspensionController>();
			CarController component3 = gameObject.GetComponent<CarController>();
			BodyPartsSwitcher component4 = gameObject.GetComponent<BodyPartsSwitcher>();
			if (SelectedArray != TurnKeyVehicles)
			{
				component2.SetStockSuspensionsValues();
			}
			IKDriverController component5 = gameObject.GetComponent<IKDriverController>();
			if (component5 != null)
			{
				component5.ToggleDriver(ShowDriver: false, ShowHands: false);
				component5.enabled = false;
			}
			gameObject.GetComponent<CarController>().vehicleIsActive = false;
			gameObject.GetComponent<CarEffects>().enabled = false;
			if (gameObject.GetComponent<LightsController>() != null)
			{
				gameObject.GetComponent<LightsController>().LightsOn = LightsOn;
			}
			if (gameObject.GetComponentInChildren<EngineSoundProcessor>() != null)
			{
				gameObject.GetComponentInChildren<EngineSoundProcessor>().enabled = false;
			}
			gameObject.GetComponent<EngineController>().enabled = false;
			component3.UpdateEngineModel();
		}
		else if (!forDealership && Utility.EqiuppedTrailer() != string.Empty && Utility.EqiuppedTrailer() == vehicleID)
		{
			string @string = DataStore.GetString("VehicleOnTrailer");
			if (Utility.DoesTruckExist(@string))
			{
				string string2 = DataStore.GetString(@string);
				VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(string2);
				string path2 = "Vehicles/" + vehicleData.VehicleName;
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(path2, typeof(GameObject))) as GameObject;
				gameObject2.name = vehicleData.VehicleName;
				IKDriverController component6 = gameObject2.GetComponent<IKDriverController>();
				if (component6 != null)
				{
					component6.ToggleDriver(ShowDriver: false, ShowHands: false);
					component6.enabled = false;
				}
				VehicleDataManager component7 = gameObject2.GetComponent<VehicleDataManager>();
				component7.VehicleID = @string;
				component7.LoadVehicleData();
				gameObject2.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: true);
				gameObject2.GetComponent<BodyPartsSwitcher>().UpdateDirtiness();
				gameObject2.GetComponent<CarController>().UpdateEngineModel();
				gameObject2.GetComponent<CarController>().SetCalculatedCOM();
				if (gameObject2.GetComponentInChildren<EngineSoundProcessor>() != null)
				{
					gameObject2.GetComponentInChildren<EngineSoundProcessor>().enabled = false;
				}
				component7.LoadOnTrailer(gameObject.GetComponent<TrailerController>());
				loadedVehicleOnTrailer = gameObject2;
			}
		}
		return gameObject;
	}

	private IEnumerator PreventVehicleJumpingOnLoad(GameObject vehicle)
	{
		Utility.AlignVehicleByGround(vehicle.transform);
		yield return null;
	}

	private void LoadVehiclesInGarage()
	{
		UnloadVehiclesInGarage();
		LoadedVehiclesInGarage = new List<VehicleDataManager>();
		string[] savedVehiclesIDs = GetSavedVehiclesIDs();
		if (savedVehiclesIDs == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		while (num < GarageVehiclePoints.Length && num2 < savedVehiclesIDs.Length)
		{
			if (!(DataStore.GetString("VehicleOnTrailer") == savedVehiclesIDs[num2]))
			{
				string @string = DataStore.GetString(savedVehiclesIDs[num2]);
				VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(@string);
				GameObject gameObject = LoadVehicle(vehicleData.VehicleName, GarageVehiclePoints[num], forDealership: false, savedVehiclesIDs[num2]);
				VehicleDataManager component = gameObject.GetComponent<VehicleDataManager>();
				component.VehicleID = savedVehiclesIDs[num2];
				component.LoadVehicleData();
				if (component.vehicleType != VehicleType.Trailer)
				{
					StartCoroutine(PreventVehicleJumpingOnLoad(gameObject));
					gameObject.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: true);
					gameObject.GetComponent<BodyPartsSwitcher>().UpdateDirtiness();
				}
				LoadedVehiclesInGarage.Add(component);
				component.GaragePlaceID = num;
				num++;
			}
			num2++;
		}
	}

	private void UnloadVehiclesInGarage()
	{
		if (LoadedVehiclesInGarage != null)
		{
			for (int i = 0; i < LoadedVehiclesInGarage.Count; i++)
			{
				if (LoadedVehiclesInGarage[i].gameObject != null)
				{
					UnityEngine.Object.Destroy(LoadedVehiclesInGarage[i].gameObject);
				}
			}
		}
		LoadedVehiclesInGarage = null;
		if (loadedVehicleOnTrailer != null)
		{
			UnityEngine.Object.Destroy(loadedVehicleOnTrailer);
		}
		if (LoadedVehicleInSelector != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInSelector);
		}
		if (LoadedVehicleInCustomization != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInCustomization);
		}
	}

	public void ChangeCurrentVehicle(VehicleDataManager vehicle, bool InstantCameraMove)
	{
		if (menuState == MenuState.MainMenu && !(CurrentVehicle == vehicle))
		{
			LoadCurrentComponents(vehicle.gameObject);
			SelectedVehicleInGarageID = vehicle.GaragePlaceID;
			SetCameraTarget(GarageVehiclePoints[SelectedVehicleInGarageID].position, InstantCameraMove);
			GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
			UpdateCameraSettings(CurrentVehicle.gameObject);
			UpdateSideButtons();
			UpdateStats();
		}
	}

	public void ChangeCurrentVehicle(string VehicleID, bool InstantCameraMove)
	{
		if (menuState == MenuState.MainMenu)
		{
			if (LoadedVehiclesInGarage != null)
			{
				foreach (VehicleDataManager item in LoadedVehiclesInGarage)
				{
					if (item.VehicleID == VehicleID)
					{
						LoadCurrentComponents(item.gameObject);
						SelectedVehicleInGarageID = item.GaragePlaceID;
						SetCameraTarget(GarageVehiclePoints[SelectedVehicleInGarageID].position, InstantCameraMove);
						GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
						UpdateCameraSettings(CurrentVehicle.gameObject);
					}
				}
			}
			UpdateStats();
		}
	}

	public void ShowStorage()
	{
		string[] savedVehiclesIDs = GetSavedVehiclesIDs();
		if (savedVehiclesIDs.Length > GarageVehiclePoints.Length)
		{
			LoadStorageArea(FromMainMenu: true);
		}
		else
		{
			ShowMessage("You don't have any vehicles in storage!");
		}
	}

	public void MoveOutOfStorage()
	{
		VehicleDataManager component = LoadedVehicleInSelector.GetComponent<VehicleDataManager>();
		AddVehicleToSavedVehiclesList(component, forceToFront: true);
		SaveVehicleData(component);
		UnityEngine.Object.Destroy(LoadedVehicleInSelector);
		if (loadedVehicleOnTrailer != null)
		{
			UnityEngine.Object.Destroy(loadedVehicleOnTrailer);
		}
		LoadMenu(MenuState.MainMenu, ThroughFade: true, FromMainMenu: false);
		SelectedVehicleInGarageID = 0;
		GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
	}

	private void UpdateVehicleSelector(bool isStorage = false)
	{
		if (!isStorage)
		{
			if (SelectedTruckIDInSelector < 0)
			{
				SelectedTruckIDInSelector = SelectedArray.Length - 1;
			}
			if (SelectedTruckIDInSelector > SelectedArray.Length - 1)
			{
				SelectedTruckIDInSelector = 0;
			}
		}
		else
		{
			if (SelectedTruckIDInSelector < 0)
			{
				SelectedTruckIDInSelector = StoredVehicles.Count - 1;
			}
			if (SelectedTruckIDInSelector > StoredVehicles.Count - 1)
			{
				SelectedTruckIDInSelector = 0;
			}
		}
		if (LoadedVehicleInCustomization != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInCustomization);
		}
		if (LoadedVehicleInSelector != null)
		{
			UnityEngine.Object.Destroy(LoadedVehicleInSelector);
		}
		if (loadedVehicleOnTrailer != null)
		{
			UnityEngine.Object.Destroy(loadedVehicleOnTrailer);
		}
		if (!isStorage)
		{
			bool flag = SelectedArray == trailers;
			LoadedVehicleInSelector = LoadVehicle(SelectedArray[SelectedTruckIDInSelector].name, TruckSelectorSpawnPoint, forDealership: true, string.Empty);
			if (!flag)
			{
				StartCoroutine(PreventVehicleJumpingOnLoad(LoadedVehicleInSelector));
				if (SelectedArray != TurnKeyVehicles)
				{
					LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>().SetStockModification();
				}
				LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>().WashVehicle();
				LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: false);
			}
			VehicleDataManager component = LoadedVehicleInSelector.GetComponent<VehicleDataManager>();
			TruckPriceMoney.text = component.MoneyPrice.ToString();
			TruckPriceGold.text = component.GoldPrice.ToString();
			TruckPriceCash.text = "$" + component.CashPrice.ToString();
			if (component.IsAvailable)
			{
				BuyForGoldButton.SetActive(value: true);
				BuyForMoneyButton.SetActive(value: true);
				BuyForCashButton.SetActive(value: false);
				MembersOnlyPanel.SetActive(value: false);
				PremiumPanel.SetActive(value: false);
				ExclusivePanel.SetActive(value: false);
				MembersAndEveryoneElseAfterDatePanel.SetActive(value: false);
			}
			else if (component.vehicleAvailability == Availability.MembersAndEveryoneAfterDate)
			{
				BuyForGoldButton.SetActive(value: false);
				BuyForMoneyButton.SetActive(value: false);
				BuyForCashButton.SetActive(value: true);
				MembersOnlyPanel.SetActive(value: false);
				ExclusivePanel.SetActive(value: false);
				MembersAndEveryoneElseAfterDatePanel.SetActive(value: true);
				PremiumPanel.SetActive(value: false);
				int days = component.TimeLeft.Days;
				int hours = component.TimeLeft.Hours;
				DaysLeftText.text = days + " days " + hours + " hours ";
			}
			else if (component.vehicleAvailability == Availability.MembersOnly)
			{
				BuyForGoldButton.SetActive(value: false);
				BuyForMoneyButton.SetActive(value: false);
				BuyForCashButton.SetActive(value: false);
				MembersOnlyPanel.SetActive(value: true);
				ExclusivePanel.SetActive(value: true);
				PremiumPanel.SetActive(value: false);
				MembersAndEveryoneElseAfterDatePanel.SetActive(value: false);
			}
			if (SelectedArray == TurnKeyVehicles)
			{
				BuyForGoldButton.SetActive(value: false);
				BuyForMoneyButton.SetActive(value: false);
				BuyForCashButton.SetActive(value: true);
				PremiumPanel.SetActive(value: true);
			}
			UpdateCameraSettings(LoadedVehicleInSelector);
		}
		else
		{
			LoadedVehicleInSelector = LoadVehicle(StoredVehicles[SelectedTruckIDInSelector].VehicleName, TruckSelectorSpawnPoint, forDealership: false, string.Empty);
			VehicleDataManager component2 = LoadedVehicleInSelector.GetComponent<VehicleDataManager>();
			component2.VehicleID = StoredVehicles[SelectedTruckIDInSelector].SavedID;
			component2.LoadVehicleData();
			if (component2.vehicleType != VehicleType.Trailer)
			{
				StartCoroutine(PreventVehicleJumpingOnLoad(LoadedVehicleInSelector));
				LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: true);
				LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>().UpdateDirtiness();
			}
			UpdateCameraSettings(LoadedVehicleInSelector);
		}
	}

	private void BuyVehicleForRealCash()
	{
		ShowMessage("Waiting for response from store...", okButtonEnabled: false);
		storeCallbackTimerCounting = true;
		storeCallbackTimer = 0f;
		if (SelectedArray == TurnKeyVehicles)
		{
			PurchaseProduct("com.battlecreek.offroadoutlaws.premiumvehiclepurchase");
		}
		else
		{
			PurchaseProduct("com.battlecreek.offroadoutlaws.timedvehiclepurchase");
		}
	}

	public void StopStoreCallbackTimer()
	{
		storeCallbackTimerCounting = false;
	}

	public void BuyVehicle(Currency currency, bool iapPurchase = false)
	{
		bool flag = SelectedArray == TurnKeyVehicles;
		bool flag2 = SelectedArray == trailers;
		VehicleDataManager component = LoadedVehicleInSelector.GetComponent<VehicleDataManager>();
		int num = 0;
		num = ((currency != 0) ? component.GoldPrice : component.MoneyPrice);
		UnityEngine.Debug.Log("Buying: " + currency);
		UnityEngine.Debug.Log("Price: " + num);
		if (ProcessPurchase(currency, num) || iapPurchase)
		{
			if (flag)
			{
				BodyPartsSwitcher component2 = LoadedVehicleInSelector.GetComponent<BodyPartsSwitcher>();
				SuspensionController component3 = LoadedVehicleInSelector.GetComponent<SuspensionController>();
				PartGroup[] partGroups = component2.partGroups;
				foreach (PartGroup partGroup in partGroups)
				{
					if (partGroup.Parts[partGroup.InstalledPart] != null && !component.PurchasedPartsList.Contains(partGroup.Parts[partGroup.InstalledPart].name))
					{
						component.PurchasedPartsList.Add(partGroup.Parts[partGroup.InstalledPart].name);
					}
				}
				int intValue = component3.FrontWheelsControls.Rim.IntValue;
				int intValue2 = component3.RearWheelsControls.Rim.IntValue;
				int intValue3 = component3.FrontWheelsControls.Tire.IntValue;
				int intValue4 = component3.RearWheelsControls.Tire.IntValue;
				if (!component.PurchasedPartsList.Contains("Rim" + intValue.ToString()))
				{
					component.PurchasedPartsList.Add("Rim" + intValue.ToString());
				}
				if (!component.PurchasedPartsList.Contains("Rim" + intValue2.ToString()))
				{
					component.PurchasedPartsList.Add("Rim" + intValue2.ToString());
				}
				if (!component.PurchasedPartsList.Contains("Tire" + intValue3.ToString()))
				{
					component.PurchasedPartsList.Add("Tire" + intValue3.ToString());
				}
				if (!component.PurchasedPartsList.Contains("Tire" + intValue4.ToString()))
				{
					component.PurchasedPartsList.Add("Tire" + intValue4.ToString());
				}
				component3.CurrentFrontSuspension.UpgradeStage = 4;
				component3.CurrentRearSuspension.UpgradeStage = 4;
			}
			component.Bought = true;
			component.VehicleID = GenerateRandomID();
			string vehicleID = component.VehicleID;
			SaveVehicleData(component);
			UnityEngine.Object.Destroy(LoadedVehicleInSelector);
			LoadMenu(MenuState.MainMenu, ThroughFade: true, FromMainMenu: false);
			SelectedVehicleInGarageID = 0;
			GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
		}
		else if (currency == Currency.Money && Utility.CashToGold(num) <= GameState.LoadStatsData().Gold)
		{
			BuyVehicle(Currency.Gold);
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more money!");
			LoadIAPMenu();
		}
	}

	public void SellVehicle()
	{
		if (CurrentVehicle == null)
		{
			return;
		}
		float num = CurrentVehicle.MoneyPrice;
		if (CurrentVehicle.vehicleType != VehicleType.Trailer)
		{
			PartGroup[] partGroups = CurrentPartsSwitcher.partGroups;
			foreach (PartGroup partGroup in partGroups)
			{
				GameObject[] parts = partGroup.Parts;
				foreach (GameObject gameObject in parts)
				{
					if (gameObject != null && CurrentVehicle.PurchasedPartsList.Contains(gameObject.name))
					{
						num += (float)VehicleParts.GetPart(CurrentVehicle.vehicleType, partGroup.partType, gameObject.name).partCost;
					}
				}
			}
			if (CurrentPartsSwitcher.GlossyPaintPurchased)
			{
				num += 10000f;
			}
			SuspensionControlLimit limit = SuspensionControlLimits.getLimit((currentSide != 0) ? CurrentSuspensionController.CurrentRearSuspension.gameObject.name : CurrentSuspensionController.CurrentFrontSuspension.gameObject.name, "Rim");
			for (int k = 0; k < limit.iMax; k++)
			{
				string text = "Rim" + k;
				BodyPart part = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, text);
				if (CurrentVehicle.PurchasedPartsList.Contains(text))
				{
					num += (float)part.partCost;
				}
			}
			SuspensionControlLimit limit2 = SuspensionControlLimits.getLimit((currentSide != 0) ? CurrentSuspensionController.CurrentRearSuspension.gameObject.name : CurrentSuspensionController.CurrentFrontSuspension.gameObject.name, "Tire");
			for (int l = 0; l < limit2.iMax; l++)
			{
				string text2 = "Tire" + l;
				BodyPart part2 = VehicleParts.GetPart(CurrentVehicle.vehicleType, PartType.Wheel, text2);
				if (CurrentVehicle.PurchasedPartsList.Contains(text2))
				{
					num += (float)part2.partCost;
				}
			}
			for (int m = 0; m < 5; m++)
			{
				PowerPart part3 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.EngineBlock, m);
				if (CurrentCarController.EngineBlockStage >= m)
				{
					num += (float)part3.partCost;
				}
			}
			for (int n = 0; n < 5; n++)
			{
				PowerPart part4 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Head, n);
				if (CurrentCarController.HeadStage >= n)
				{
					num += (float)part4.partCost;
				}
			}
			for (int num2 = 0; num2 < 5; num2++)
			{
				PowerPart part5 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Grip, num2);
				if (CurrentCarController.GripStage >= num2)
				{
					num += (float)part5.partCost;
				}
			}
			for (int num3 = 0; num3 < 5; num3++)
			{
				PowerPart part6 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Valvetrain, num3);
				if (CurrentCarController.ValvetrainStage >= num3)
				{
					num += (float)part6.partCost;
				}
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				PowerPart part7 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Weight, num4);
				if (CurrentCarController.WeightStage >= num4)
				{
					num += (float)part7.partCost;
				}
			}
			for (int num5 = 0; num5 < 5; num5++)
			{
				PowerPart part8 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Durability, num5);
				if (CurrentCarController.DurabilityStage >= num5)
				{
					num += (float)part8.partCost;
				}
			}
			for (int num6 = 0; num6 < 5; num6++)
			{
				PowerPart part9 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Gearing, num6);
				if (CurrentCarController.GearingStage >= num6)
				{
					num += (float)part9.partCost;
				}
			}
			for (int num7 = 0; num7 < 5; num7++)
			{
				PowerPart part10 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Turbo, num7);
				if (CurrentCarController.PurchasedTurboStage >= num7)
				{
					num += (float)part10.partCost;
				}
			}
			for (int num8 = 0; num8 < 5; num8++)
			{
				PowerPart part11 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Blower, num8);
				if (CurrentCarController.PurchasedBlowerStage >= num8)
				{
					num += (float)part11.partCost;
				}
			}
			PowerPart part12 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Gearbox, 1);
			if (CurrentCarController.ManualTransmissionPurchased)
			{
				num += (float)part12.partCost;
			}
			PowerPart part13 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.TankTracks, 1);
			if (CurrentCarController.TankTracksPurchased)
			{
				num += (float)part13.partCost;
			}
			PowerPart part14 = PowerParts.GetPart(CurrentVehicle.vehicleType, PowerPartType.Ebrake, 1);
			if (CurrentCarController.Ebrake == 1)
			{
				num += (float)part14.partCost;
			}
			foreach (Suspension frontSuspension in CurrentSuspensionController.FrontSuspensions)
			{
				if (CurrentVehicle.PurchasedPartsList.Contains(frontSuspension.name))
				{
					SuspensionPart suspension = Suspensions.GetSuspension(CurrentVehicle.vehicleType, frontSuspension.name);
					num += (float)suspension.partCost;
					for (int num9 = 0; num9 < 5; num9++)
					{
						SuspensionUpgrade suspensionUpgrade = Suspensions.GetSuspensionUpgrade(frontSuspension.name, num9);
						if (frontSuspension.UpgradeStage >= num9)
						{
							num += (float)suspensionUpgrade.upgradeCost;
						}
					}
				}
			}
			for (int num10 = 0; num10 < 5; num10++)
			{
				WheelsUpgrade wheelsUpgrade = Suspensions.GetWheelsUpgrade(num10);
				if (CurrentSuspensionController.FrontWheelsControls.Stage >= num10)
				{
					num += (float)wheelsUpgrade.upgradeCost;
				}
				if (CurrentSuspensionController.RearWheelsControls.Stage >= num10)
				{
					num += (float)wheelsUpgrade.upgradeCost;
				}
			}
		}
		num /= 2f;
		sellingTruckCost = (int)num;
		truckSellText.text = "Do you really want to sell this vehicle for $" + sellingTruckCost + "? This action can't be undone!";
		truckSellWindow.SetActive(value: true);
	}

	public void ConfirmSellingTruck()
	{
		if (CurrentVehicle.vehicleType == VehicleType.Trailer && Utility.EqiuppedTrailer() == CurrentVehicle.VehicleID)
		{
			UnloadCarsFromTrailer();
			UnequipTrailers();
		}
		DataStore.DeleteKey(CurrentVehicle.VehicleID);
		if (DataStore.HasKey("VehiclesList"))
		{
			string @string = DataStore.GetString("VehiclesList");
			SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
			if (savedVehiclesList.VehicleIDs.Contains(CurrentVehicle.VehicleID))
			{
				savedVehiclesList.VehicleIDs.Remove(CurrentVehicle.VehicleID);
			}
			@string = XmlSerialization.SerializeData<SavedVehiclesList>(savedVehiclesList);
			DataStore.SetString("VehiclesList", @string);
		}
		truckSellWindow.SetActive(value: false);
		GameState.AddCurrency((int)sellingTruckCost, Currency.Money);
		LoadMainMenu(FromMainMenu: true);
	}

	private void OnFacebookInitialized()
	{
		UnityEngine.Debug.Log("Facebook ready...");
		if (AccessToken.CurrentAccessToken != null)
		{
			ApplySettings();
			DataStore.SetBool("LinkedFB", value: true);
			UnityEngine.Debug.Log("User is logged in with Facebook.. logging into PlayFab with that id");
			LoginWithFacebookRequest loginWithFacebookRequest = new LoginWithFacebookRequest();
			loginWithFacebookRequest.CreateAccount = true;
			loginWithFacebookRequest.AccessToken = AccessToken.CurrentAccessToken.TokenString;
			PlayFabClientAPI.LoginWithFacebook(loginWithFacebookRequest, OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
			FB.API("/me?fields=name", HttpMethod.GET, GotFBName);
		}
		else
		{
			UnityEngine.Debug.Log("User is not logged in with Facebook.. logging in with playfab");
			if (PlayFabLogin.Instance != null)
			{
				PlayFabLogin.Instance.Login();
			}
			if (DataStore.GetBool("LinkedFB"))
			{
				FacebookLoginWarning.SetActive(value: true);
			}
		}
	}

	public void LoginWithFaceBook()
	{
		UnityEngine.Debug.Log("Trying to login with FB");
		FacebookLoginWarning.SetActive(value: false);
		if (!FB.IsLoggedIn)
		{
			ShowWaiting("Logging in...");
			FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
		}
		else
		{
			ShowMessage("You're already logged in.");
		}
	}

	public void CloseFacebookLoginWarning()
	{
		DataStore.SetBool("LinkedFB", value: false);
		FacebookLoginWarning.SetActive(value: false);
	}

	public void LikeUs()
	{
		Application.OpenURL("http://facebook.com/OffroadOutlawsGame");
	}

	private void OnFacebookLoggedIn(ILoginResult result)
	{
		if (result == null || string.IsNullOrEmpty(result.Error))
		{
			UnityEngine.Debug.Log("Facebook Auth Complete! Logging into/Linking PlayFab w/FB...");
			if (result == null || result.AccessToken == null || result.AccessToken.TokenString == null)
			{
				UnityEngine.Debug.Log("No access token!");
				HideWaiting();
				ShowMessage("Couldn't link your Facebook account.");
			}
			else
			{
				FB.API("/me?fields=name", HttpMethod.GET, GotFBName);
				LoginWithFacebookRequest loginWithFacebookRequest = new LoginWithFacebookRequest();
				loginWithFacebookRequest.CreateAccount = true;
				loginWithFacebookRequest.AccessToken = AccessToken.CurrentAccessToken.TokenString;
				PlayFabClientAPI.LoginWithFacebook(loginWithFacebookRequest, OnPlayfabFacebookAuthCompleteGetCloud, OnPlayfabFacebookAuthFailed);
			}
		}
		else
		{
			ShowMessage("Facebook Login Failed: " + result.Error + "\n");
			HideWaiting();
		}
	}

	private void GotFBName(IGraphResult result)
	{
		if (result != null && result.ResultDictionary != null && result.ResultDictionary["name"] != null)
		{
			string text = result.ResultDictionary["name"].ToString();
			UnityEngine.Debug.Log("Facebook Name: " + text);
			GameState.PlayerName = text;
			PhotonNetwork.playerName = text;
			DataStore.SetBool("UseFBName", value: true);
		}
		ApplySettings();
	}

	public void ShowImportCloudDataBox(int localGold, int cloudGold, int localMoney, int cloudMoney, int localVehicles, int cloudVehicles)
	{
		LocalGoldLabel.text = "Gold: " + localGold.ToString();
		LocalMoneyLabel.text = "Money: " + localMoney.ToString();
		LocalVehiclesLabel.text = "Vehicles: " + localVehicles.ToString();
		CloudGoldLabel.text = "Gold: " + cloudGold.ToString();
		CloudMoneyLabel.text = "Money: " + cloudMoney.ToString();
		CloudVehiclesLabel.text = "Vehicles: " + cloudVehicles.ToString();
		YesNoCloudDataBox.SetActive(value: true);
	}

	public void UseCloudData()
	{
		ShowWaiting("Downloading cloud data...");
		YesNoCloudDataBox.SetActive(value: false);
		DataStore.ImportCloudData();
	}

	public void UseLocalData()
	{
		DataStore.disableCloudSave = false;
		YesNoCloudDataBox.SetActive(value: false);
		HideWaiting();
	}

	private void OnPlayfabFacebookAuthCompleteGetCloud(PlayFab.ClientModels.LoginResult result)
	{
		LinkAndroidDeviceIDRequest linkAndroidDeviceIDRequest = new LinkAndroidDeviceIDRequest();
		linkAndroidDeviceIDRequest.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;
		linkAndroidDeviceIDRequest.ForceLink = true;
		PlayFabClientAPI.LinkAndroidDeviceID(linkAndroidDeviceIDRequest, delegate
		{
			HideWaiting();
			DataStore.disableCloudSave = true;
			DataStore.SetBool("LinkedFB", value: true);
			DataStore.DownloadCloudData();
		}, delegate(PlayFabError error)
		{
			UnityEngine.Debug.Log(error.ErrorMessage);
			ShowMessage("Couldn't link your Facebook account.");
			HideWaiting();
		});
		UnityEngine.Debug.Log("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
	}

	private void OnPlayfabFacebookAuthComplete(PlayFab.ClientModels.LoginResult result)
	{
		LinkAndroidDeviceIDRequest linkAndroidDeviceIDRequest = new LinkAndroidDeviceIDRequest();
		linkAndroidDeviceIDRequest.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;
		linkAndroidDeviceIDRequest.ForceLink = true;
		PlayFabClientAPI.LinkAndroidDeviceID(linkAndroidDeviceIDRequest, delegate
		{
			HideWaiting();
			DataStore.disableCloudSave = true;
			DataStore.SetBool("LinkedFB", value: true);
		}, delegate(PlayFabError error)
		{
			UnityEngine.Debug.Log(error.ErrorMessage);
			ShowMessage("Couldn't link your Facebook account.");
			HideWaiting();
		});
		UnityEngine.Debug.Log("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
	}

	private void OnPlayfabFacebookAuthFailed(PlayFabError error)
	{
		UnityEngine.Debug.Log("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport());
	}

	public void CloudRestoreComplete(bool reloadScene = true)
	{
		FacebookLoginWarning.SetActive(value: false);
		MessageBox.SetActive(value: false);
		HideWaiting();
		if (reloadScene)
		{
			SceneManager.LoadScene("Menu");
		}
	}

	private void SaveVehicleData(VehicleDataManager vehicleDataManager)
	{
		AddVehicleToSavedVehiclesList(vehicleDataManager);
		vehicleDataManager.SaveVehicleData();
	}

	public void RemoveAllSaveData()
	{
		DataStore.Clear();
	}

	private void AddVehicleToSavedVehiclesList(VehicleDataManager vehicleDataManager, bool forceToFront = false)
	{
		if (DataStore.HasKey("VehiclesList"))
		{
			string @string = DataStore.GetString("VehiclesList");
			SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
			if (!savedVehiclesList.VehicleIDs.Contains(vehicleDataManager.VehicleID))
			{
				savedVehiclesList.VehicleIDs.Insert(0, vehicleDataManager.VehicleID);
			}
			else if (forceToFront)
			{
				savedVehiclesList.VehicleIDs.Remove(vehicleDataManager.VehicleID);
				savedVehiclesList.VehicleIDs.Insert(0, vehicleDataManager.VehicleID);
			}
			@string = XmlSerialization.SerializeData<SavedVehiclesList>(savedVehiclesList);
			DataStore.SetString("VehiclesList", @string);
		}
		else
		{
			SavedVehiclesList savedVehiclesList2 = new SavedVehiclesList();
			savedVehiclesList2.VehicleIDs = new List<string>();
			savedVehiclesList2.VehicleIDs.Add(vehicleDataManager.VehicleID);
			string value = XmlSerialization.SerializeData<SavedVehiclesList>(savedVehiclesList2);
			DataStore.SetString("VehiclesList", value);
		}
	}

	public void DynoFinished(float maxHP, float avgHP, float maxTQ, float avgTQ)
	{
		Drivetrain_DynoResult();
		MaxHPText.text = ((int)maxHP).ToString();
		AvgHPText.text = ((int)avgHP).ToString();
		MaxTQText.text = ((int)maxTQ).ToString();
		AvgTQText.text = ((int)avgTQ).ToString();
	}

	public void BuyTuningPack()
	{
		int amount = 100;
		if (ProcessPurchase(Currency.Gold, amount))
		{
			CurrentCarController.TuningEnginePurchased = true;
			SaveVehicle();
			BuyTuningPackButton.interactable = false;
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more!");
			LoadIAPMenu();
		}
	}

	public void BuyPerfectSetup()
	{
		int amount = 100;
		if (ProcessPurchase(Currency.Gold, amount))
		{
			CurrentCarController.PerfectSetupPurchased = true;
			SetPerfectSetup();
			SaveVehicle();
			BuyPerfectSetupButton.interactable = false;
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more!");
			LoadIAPMenu();
		}
	}

	public void SetPerfectSetup()
	{
		CurrentCarController.FuelRatio = CurrentCarController.PerfectFuelRatio;
		CurrentCarController.TimingRatio = CurrentCarController.PerfectTimingRatio;
		EngineTuningSlider.gameObject.SetActive(value: false);
	}

	public void BuyDynoRuns(int amount)
	{
		int amount2 = 100;
		if (amount == 50)
		{
			amount2 = 200;
		}
		if (amount == 100)
		{
			amount2 = 300;
		}
		if (ProcessPurchase(Currency.Gold, amount2))
		{
			StatsData statsData = GameState.LoadStatsData();
			statsData.DynoRuns += amount;
			GameState.SaveStatsData(statsData);
			Drivetrain_DynoTest();
		}
		else
		{
			ShowMessage("You don't have enough. To continue, you can purchase gold, or do more races to earn more!");
			LoadIAPMenu();
		}
	}

	public void RunDynoTest()
	{
		if (GameState.LoadStatsData().DynoRuns <= 0)
		{
			Drivetrain_BuyDynoRuns();
			return;
		}
		Dyno.SetActive(value: false);
		DynoRoomController.Instance.StartDyno();
		StatsData statsData = GameState.LoadStatsData();
		statsData.DynoRuns--;
		GameState.SaveStatsData(statsData);
	}

	public void SelectEngineTuningItem(int ID)
	{
		if (CurrentCarController.PerfectSetupPurchased)
		{
			ShowMessage("You already have perfect setup, no need to tune anymore!");
			return;
		}
		if (!CurrentCarController.TuningEnginePurchased)
		{
			ShowMessage("Purchase Tuning pack first!");
			return;
		}
		EngineTuningSlider.gameObject.SetActive(value: true);
		selectedEngineTuningItem = (EngineTuningItem)ID;
		string valueName = string.Empty;
		float currentValue = 0f;
		switch (selectedEngineTuningItem)
		{
		case EngineTuningItem.FuelRatio:
			valueName = "Fuel ratio";
			currentValue = CurrentCarController.FuelRatio;
			break;
		case EngineTuningItem.TimingRatio:
			valueName = "Timing ratio";
			currentValue = CurrentCarController.TimingRatio;
			break;
		}
		EngineTuningSlider.SetupFloatValue(valueName, -10f, 10f, -10f, 10f, currentValue);
	}

	public void EngineTuningChanged()
	{
		switch (selectedEngineTuningItem)
		{
		case EngineTuningItem.FuelRatio:
			CurrentCarController.FuelRatio = EngineTuningSlider.slider.value;
			break;
		case EngineTuningItem.TimingRatio:
			CurrentCarController.TimingRatio = EngineTuningSlider.slider.value;
			break;
		}
	}

	private bool ProcessPurchase(Currency currency, int amount)
	{
		StatsData statsData = GameState.LoadStatsData();
		bool flag = false;
		switch (currency)
		{
		case Currency.Gold:
			flag = (statsData.Gold >= amount);
			if (flag)
			{
				GameState.SubtractCurrency(amount, currency);
			}
			break;
		case Currency.Money:
			flag = (statsData.Money >= amount);
			if (flag)
			{
				GameState.SubtractCurrency(amount, currency);
			}
			break;
		}
		UpdateScreen();
		return flag;
	}

	public string[] GetSavedVehiclesIDs()
	{
		string @string = DataStore.GetString("VehiclesList");
		if (@string == string.Empty)
		{
			return null;
		}
		SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
		return savedVehiclesList.VehicleIDs.ToArray();
	}

	private string[] GetStorageVehiclesIDs()
	{
		string @string = DataStore.GetString("VehiclesList");
		if (@string == string.Empty)
		{
			return null;
		}
		SavedVehiclesList savedVehiclesList = (SavedVehiclesList)XmlSerialization.DeserializeData<SavedVehiclesList>(@string);
		int num = 0;
		int num2 = 0;
		while (num < GarageVehiclePoints.Length)
		{
			savedVehiclesList.VehicleIDs.Remove(savedVehiclesList.VehicleIDs[0]);
			if (DataStore.GetString("VehicleOnTrailer") != savedVehiclesList.VehicleIDs[0])
			{
				num++;
			}
			num2++;
		}
		return savedVehiclesList.VehicleIDs.ToArray();
	}

	private string GenerateRandomID()
	{
		string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string text2 = "Vehicle_";
		for (int i = 0; i < 5; i++)
		{
			text2 += text[UnityEngine.Random.Range(0, text.Length)];
		}
		return text2;
	}

	private void SetCameraTarget(Vector3 To, bool Instantly)
	{
		cameraTargetPos = To + Vector3.down * 0.5f;
		if (Instantly)
		{
			CameraTarget.position = To;
		}
	}

	private void SetCameraTargetWithoutOffset(Vector3 To, bool Instantly)
	{
		cameraTargetPos = To;
		if (Instantly)
		{
			CameraTarget.position = To;
		}
	}

	private void UpdateCameraSettings(GameObject vehicle = null)
	{
		if (vehicle != null)
		{
			CarController component = vehicle.GetComponent<CarController>();
			if (component != null)
			{
				CameraController.Instance.MaxDistance = component.GarageMaxDistance;
				CameraController.Instance.MinDistance = component.GarageMinDistance;
				CameraController.Instance.DistanceCamTarget = component.GarageMinDistance + (component.GarageMinDistance + component.GarageMaxDistance) / 2f;
			}
			else
			{
				CameraController.Instance.MaxDistance = 6f;
				CameraController.Instance.MinDistance = 3f;
			}
		}
		if (menuState == MenuState.MainMenu)
		{
			CameraController.Instance.GetComponent<Camera>().fieldOfView = 50f;
		}
		else
		{
			CameraController.Instance.GetComponent<Camera>().fieldOfView = 69f;
		}
	}

	public void HideMessage()
	{
		MessageBox.SetActive(value: false);
	}

	public void ShowMessage(string message, bool okButtonEnabled = true)
	{
		MessageBox.SetActive(value: true);
		MessageText.text = message;
		messageOkButton.interactable = okButtonEnabled;
	}

	public void ShowFieldFindMessage()
	{
		string @string = DataStore.GetString("FoundPartsFF" + DataStore.CurrentFieldFind().ToString(), string.Empty);
		string[] array = @string.Split(',');
		Dictionary<CratePartType, string> dictionary = StashContent.CratePartTypeList();
		List<int> list = new List<int>();
		for (int i = 0; i <= 9; i++)
		{
			bool flag = false;
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text != null && text != string.Empty && text == i.ToString())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(i);
			}
		}
		FieldFindParts1.text = string.Empty;
		FieldFindParts2.text = string.Empty;
		for (int k = 0; k < list.Count; k++)
		{
			if (k < 5)
			{
				Text fieldFindParts = FieldFindParts1;
				fieldFindParts.text = fieldFindParts.text + "- " + dictionary[(CratePartType)list[k]] + "\r\n";
			}
			else
			{
				Text fieldFindParts2 = FieldFindParts2;
				fieldFindParts2.text = fieldFindParts2.text + "- " + dictionary[(CratePartType)list[k]] + "\r\n";
			}
		}
		FieldFindBox.SetActive(value: true);
	}

	public void BuyFieldFindParts(bool spendMoney = true)
	{
		StatsData statsData = GameState.LoadStatsData();
		if (statsData.Gold >= 500 || !spendMoney)
		{
			statsData.Gold -= 500;
			int num = DataStore.LastFoundFieldFind() - 1;
			string prefabName = FieldFind.FieldFindNames[num];
			GameObject gameObject = LoadVehicle(prefabName, TruckSelectorSpawnPoint, forDealership: false, string.Empty);
			StartCoroutine(PreventVehicleJumpingOnLoad(gameObject));
			VehicleDataManager component = gameObject.GetComponent<VehicleDataManager>();
			component.Bought = true;
			component.VehicleID = GenerateRandomID();
			string vehicleID = component.VehicleID;
			SaveVehicleData(component);
			UnityEngine.Object.Destroy(LoadedVehicleInSelector);
			LoadMenu(MenuState.MainMenu, ThroughFade: true, FromMainMenu: false);
			SelectedVehicleInGarageID = 0;
			GameState.SelectedGarageVehicleID = SelectedVehicleInGarageID;
			if (spendMoney)
			{
				GameState.SaveStatsData(statsData);
			}
			UnityEngine.Object.Destroy(gameObject);
			FieldFindBox.SetActive(value: false);
			StaticFieldFinds[num].SetActive(value: false);
		}
		else
		{
			FieldFindBox.SetActive(value: false);
			ShowMessage("You don't have enough gold. To continue, you can purchase gold.");
			LoadIAPMenu();
		}
	}

	public void HideFieldFindMessage()
	{
		FieldFindBox.SetActive(value: false);
	}

	private IEnumerator ShowMessageCor(string message)
	{
		Color color = MessageText.color;
		if (!(color.a > 0f))
		{
			MessageBox.SetActive(value: true);
			MessageText.text = message;
			MessageText.color = Color.white;
			yield return new WaitForSeconds(1f);
			for (float f = 1f; f >= 0f; f -= 0.1f)
			{
				MessageText.color = new Color(1f, 1f, 1f, f);
				yield return null;
			}
			MessageText.color = Color.clear;
		}
	}
}
