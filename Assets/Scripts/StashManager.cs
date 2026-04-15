using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StashManager : MonoBehaviour
{
	public RacingManager RacingManager;

	public int ActiveCrates = 4;

	public int VehicleChance;

	public int LargeChance = 20;

	public int MediumChance = 40;

	public int BoostCardChance = 20;

	public int MinAmount = 5;

	public int MaxAmount = 50;

	public int MinLockboxStashInterval = 600;

	public int MaxLockboxStashInterval = 1200;

	public int MinLockboxStashLockTimer = 60;

	public int MaxLockboxStashLockTimer = 120;

	public int LockboxDelayIfBusy = 60;

	public GameObject CurrentLockbox;

	public StashCrate CurrentLockboxData;

	public bool LockboxActive;

	public GameObject StashMessage;

	public GameObject LockboxEnabledMessage;

	public GameObject LockboxTimer;

	public Text MissingPartFoundText;

	public GameObject FoundStashMoneyAndGold;

	public GameObject FoundStashMissingPart;

	public Text GoldAmount;

	public Text MoneyAmount;

	public Text LockboxTimeLeft;

	public GameObject LockboxLabel;

	public GameObject StashCrateLabel;

	public GameObject FieldFindLabel;

	private float lastLockboxStashShown;

	private float nextLockboxStashShown;

	private CameraController camController;

	private CarUIControl carUIControl;

	private bool madeVehicle;

	public static StashManager Instance;

	private StashCrate[] crates = new StashCrate[0];

	public StashManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		camController = UnityEngine.Object.FindObjectOfType<CameraController>();
		carUIControl = UnityEngine.Object.FindObjectOfType<CarUIControl>();
		GameObject gameObject = GameObject.Find("Crates");
		crates = UnityEngine.Object.FindObjectsOfType<StashCrate>();
		RacingManager = UnityEngine.Object.FindObjectOfType<RacingManager>();
		UnityEngine.Debug.Log("Got " + crates.Length + " crates!");
		RefreshCrates();
		if (StashMessage == null || GoldAmount == null || MoneyAmount == null)
		{
			UnityEngine.Debug.LogError("STASH MANAGER IS NOT SETUP!");
		}
		Instance = this;
		nextLockboxStashShown = Time.time + (float)UnityEngine.Random.Range(MinLockboxStashInterval, MaxLockboxStashInterval);
		StashMessage.SetActive(value: false);
	}

	public void CloseStashMessage()
	{
		Time.timeScale = 1f;
		StashMessage.SetActive(value: false);
		LockboxEnabledMessage.SetActive(value: false);
		if (CurrentLockboxData != null)
		{
			LockboxTimer.SetActive(value: true);
		}
		camController.GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;
	}

	public void RefreshCrates()
	{
		for (int i = 0; i < crates.Length; i++)
		{
			crates[i].gameObject.SetActive(value: false);
		}
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			base.enabled = false;
			return;
		}
		int num = DataStore.LastFoundFieldFind();
		string text = string.Empty;
		if (num > 0)
		{
			text = FieldFind.FieldFindNames[num - 1];
		}
		if (text != string.Empty && num > 0 && !Utility.OwnsVehicle(text))
		{
			UnityEngine.Debug.Log("NEED TO MAKE PARTS BOXES!!");
			string @string = DataStore.GetString("FoundPartsFF" + num, string.Empty);
			string[] array = @string.Split(',');
			List<int> list = new List<int>();
			for (int j = 0; j <= 9; j++)
			{
				bool flag = false;
				string[] array2 = array;
				foreach (string a in array2)
				{
					if (a == j.ToString())
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(j);
				}
			}
			for (int l = 0; l < 3 && l < list.Count; l++)
			{
				UnityEngine.Debug.Log("Making parts box!");
				StashCrate stashCrate = EnableCrate(CrateSize.Large);
				if (stashCrate != null)
				{
					UnityEngine.Debug.Log("Got a box!");
					StashContent stashContent = new StashContent();
					stashContent.MissingPart = (CratePartType)list[l];
					stashCrate.Content = stashContent;
					stashCrate.SetAsMissingParts();
				}
			}
		}
		for (int m = 0; m < ActiveCrates; m++)
		{
			if (!EnableCrate(CrateSizeNeeded()))
			{
				EnableCrate(CrateSize.Small);
			}
		}
		num = DataStore.LastFoundFieldFind();
		text = string.Empty;
		bool flag2 = false;
		if (num > 0)
		{
			text = FieldFind.FieldFindNames[num - 1];
			flag2 = Utility.OwnsVehicle(text);
		}
		if (!flag2 && num != 0)
		{
			return;
		}
		for (int n = 0; n < crates.Length; n++)
		{
			if (crates[n].Size == CrateSize.Vehicle)
			{
				crates[n].gameObject.SetActive(crates[n].FieldFindID == num + 1);
			}
		}
	}

	public CrateSize CrateSizeNeeded(bool allowVehicles = true, bool allowSmallCrates = true)
	{
		int num = UnityEngine.Random.Range(1, 101);
		if (num <= VehicleChance && !madeVehicle && allowVehicles)
		{
			return CrateSize.Vehicle;
		}
		if (num <= LargeChance)
		{
			return CrateSize.Large;
		}
		if (num <= MediumChance)
		{
			return CrateSize.Medium;
		}
		if (!allowSmallCrates)
		{
			return (UnityEngine.Random.Range(1, 3) == 1) ? CrateSize.Medium : CrateSize.Large;
		}
		return CrateSize.Small;
	}

	public void FoundStashCrate(StashCrate crate)
	{
		if (crate.Size == CrateSize.Vehicle)
		{
			CarUIControl.Instance.ShowMessage("Great find! We'll ship this back home for you and place it in your yard.");
			DataStore.SetBool("FoundFieldFind" + crate.FieldFindID, value: true);
			crate.gameObject.SetActive(value: false);
			return;
		}
		if (crate.Content != null && crate.IsMissingParts)
		{
			FoundStashMissingPart.SetActive(value: true);
			FoundStashMoneyAndGold.SetActive(value: false);
			Dictionary<CratePartType, string> dictionary = StashContent.CratePartTypeList();
			MissingPartFoundText.text = dictionary[crate.Content.MissingPart];
			crate.gameObject.SetActive(value: false);
			StashMessage.SetActive(value: true);
			int num = DataStore.LastFoundFieldFind();
			string @string = DataStore.GetString("FoundPartsFF" + num, string.Empty);
			DataStore.SetString("FoundPartsFF" + num, @string + "," + (int)crate.Content.MissingPart);
			if (Utility.FoundAllParts(num.ToString()))
			{
				MissingPartFoundText.text += "\r\n\r\nYou've found them all!!";
			}
			return;
		}
		StashContent stashContent = GetStashContent(crate);
		StatsData statsData = GameState.LoadStatsData();
		if (statsData != null)
		{
			LockboxLabel.SetActive(value: false);
			StashCrateLabel.SetActive(value: false);
			statsData.Gold += Utility.AdjustedWinnings(stashContent.GoldAmount);
			statsData.Money += Utility.AdjustedWinnings(stashContent.CashAmount);
			statsData.XP += Utility.AdjustedWinnings(10);
			GameState.SaveStatsData(statsData);
			statsData = GameState.LoadStatsData();
			CarUIControl.Instance.ShowMessage("Good find! We've given you 10XP" + ((!statsData.IsMember) ? string.Empty : "x3"));
			FoundStashMissingPart.SetActive(value: false);
			FoundStashMoneyAndGold.SetActive(value: true);
			StashMessage.SetActive(value: true);
			Time.timeScale = 0f;
			GoldAmount.text = stashContent.GoldAmount.ToString() + ((!statsData.IsMember) ? string.Empty : "x3");
			if (stashContent.CashAmount > 0)
			{
				MoneyAmount.text = stashContent.CashAmount.ToString("$0,0") + ((!statsData.IsMember) ? string.Empty : "x3");
			}
			else
			{
				MoneyAmount.text = "$0";
			}
			if (crate.TimeLeft > 0f)
			{
				UnityEngine.Debug.Log("This was a lockbox!");
				LockboxDisabled(expired: false);
				LockboxLabel.SetActive(value: true);
			}
			else
			{
				StashCrateLabel.SetActive(value: true);
			}
		}
		if (stashContent.BoostCard != null)
		{
			UnityEngine.Debug.Log("Boost Card: " + stashContent.BoostCard.Type + " x" + stashContent.BoostCard.MultiplyAmount + " for " + stashContent.BoostCard.Duration + " seconds.");
		}
		camController.GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;
		crate.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (RacingManager != null && (RacingManager.IsPlayerBusy || !RacingManager.gameObject.activeInHierarchy) && nextLockboxStashShown != -1f && Time.time + (float)LockboxDelayIfBusy > nextLockboxStashShown)
		{
			nextLockboxStashShown = Time.time + (float)LockboxDelayIfBusy;
		}
		if (Time.frameCount % 2 == 0 && CurrentLockboxData != null)
		{
			if (CurrentLockboxData.TimeLeft < 10f)
			{
				carUIControl.LockboxCountdown(CurrentLockboxData.TimeLeft);
			}
			LockboxTimeLeft.text = (int)CurrentLockboxData.TimeLeft + " seconds";
		}
		if (nextLockboxStashShown != -1f && Time.time > nextLockboxStashShown && !LockboxActive)
		{
			UnityEngine.Debug.Log("Making lockbox");
			StashCrate stashCrate = EnableCrate(CrateSizeNeeded(allowVehicles: false, allowSmallCrates: false));
			if (stashCrate != null)
			{
				stashCrate.SetLockTimer(UnityEngine.Random.Range(MinLockboxStashLockTimer, MaxLockboxStashLockTimer));
				lastLockboxStashShown = Time.time;
				LockboxActive = true;
				CurrentLockbox = stashCrate.gameObject;
				CurrentLockboxData = stashCrate;
				UnityEngine.Debug.Log("Lockbox activated!");
				nextLockboxStashShown = -1f;
				carUIControl.DirectionalArrow.gameObject.SetActive(value: true);
				carUIControl.DirectionalArrowTarget = stashCrate.transform;
				camController.GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;
				LockboxEnabledMessage.SetActive(value: true);
				Time.timeScale = 0f;
			}
			else
			{
				nextLockboxStashShown = Time.time + (float)UnityEngine.Random.Range(MinLockboxStashInterval, MaxLockboxStashInterval);
				UnityEngine.Debug.Log("Couldn't make lockbox");
			}
		}
	}

	public void LockboxDisabled(bool expired)
	{
		if (expired)
		{
			camController.Shake();
			carUIControl.LockboxBomb();
		}
		carUIControl.DirectionalArrow.gameObject.SetActive(value: false);
		carUIControl.DirectionalArrowTarget = null;
		CurrentLockbox = null;
		CurrentLockboxData = null;
		LockboxActive = false;
		LockboxTimer.SetActive(value: false);
		nextLockboxStashShown = Time.time + (float)UnityEngine.Random.Range(MinLockboxStashInterval, MaxLockboxStashInterval);
		carUIControl.TickingSound.Stop();
	}

	public StashContent GetStashContent(StashCrate crate)
	{
		StashContent stashContent = new StashContent();
		if (crate.Size >= CrateSize.Small)
		{
			stashContent.CashAmount = UnityEngine.Random.Range(MinAmount, MaxAmount) * 100;
		}
		if (crate.Size >= CrateSize.Medium)
		{
			stashContent.GoldAmount = UnityEngine.Random.Range(MinAmount, MaxAmount);
		}
		return stashContent;
	}

	public StashCrate EnableCrate(CrateSize size)
	{
		StashCrate stashCrate = null;
		List<StashCrate> list = new List<StashCrate>();
		StashCrate[] array = crates;
		foreach (StashCrate stashCrate2 in array)
		{
			if (!stashCrate2.gameObject.activeInHierarchy && stashCrate2.Size == size)
			{
				list.Add(stashCrate2);
			}
		}
		if (list.Count > 0)
		{
			stashCrate = list[Random.Range(0, list.Count)];
			if (stashCrate != null)
			{
				stashCrate.gameObject.SetActive(value: true);
			}
		}
		return stashCrate;
	}
}
