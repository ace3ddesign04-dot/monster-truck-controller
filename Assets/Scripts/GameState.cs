using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class GameState
{
	public static string CurrentVehicleID = string.Empty;

	public static GameMode GameMode;

	public static GameType GameType;

	public static string RoomName;

	public static string Password = string.Empty;

	public static string SceneName;

	public static bool WaitingForRoom;

	public static bool FramerateWarning;

	public static int SelectedGarageVehicleID;

	public static bool FailedToJoin;

	public static string PlayerName;

	public static bool JustOpenedGame = true;

	public static int TrailID;

	public static int TrailRaceBet;

	public static string mapToDownload;

	public static bool unloadUnusedResourcesInGame;

	public static float resourcesUnloadRate = 120f;

	public static byte MaxPlayers
	{
		get
		{
			if (GameType == GameType.TrailRace)
			{
				return 2;
			}
			return 5;
		}
	}

	public static string playerName => (!DataStore.GetBool("UseFBName")) ? DataStore.GetString("GeneratedName") : PlayerName;

	public static void Populate(string vehicleId, string sceneName, GameMode gameMode, GameType gameType, string password)
	{
		Clear();
		CurrentVehicleID = vehicleId;
		GameMode = gameMode;
		GameType = gameType;
		Password = password;
		SceneName = sceneName;
		RoomName = Utility.RandomDigits(10);
	}

	public static void Clear()
	{
		CurrentVehicleID = null;
		GameMode = GameMode.None;
		GameType = GameType.None;
		Password = null;
		RoomName = null;
		SceneName = null;
		WaitingForRoom = false;
		TrailRaceBet = 0;
		TrailID = 0;
		mapToDownload = string.Empty;
	}

	public static void SaveStatsData(StatsData newData)
	{
		string value = XmlSerialization.SerializeData<StatsData>(newData);
		DataStore.SetString("Stats", value);
		DataStore.SetString("SH", Utility.MD5(Utility.MD5(Utility.MD5(newData.Dump()))));
	}

	public static StatsData LoadStatsData()
	{
		string @string = DataStore.GetString("Stats", string.Empty);
		StatsData statsData = new StatsData();
		if (@string != string.Empty && @string != null)
		{
			statsData = (StatsData)XmlSerialization.DeserializeData<StatsData>(@string);
			string string2 = DataStore.GetString("SH", string.Empty);
			string b = Utility.MD5(Utility.MD5(Utility.MD5(statsData.Dump())));
			if (string2 != b)
			{
				statsData = new StatsData();
			}
		}
		return statsData;
	}

	public static void SetMembership(bool isMember)
	{
		StatsData statsData = LoadStatsData();
		statsData.IsMember = isMember;
		try
		{
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				if (isMember)
				{
					AddUserVirtualCurrencyRequest addUserVirtualCurrencyRequest = new AddUserVirtualCurrencyRequest();
					addUserVirtualCurrencyRequest.Amount = 1;
					addUserVirtualCurrencyRequest.VirtualCurrency = "MB";
					PlayFabClientAPI.AddUserVirtualCurrency(addUserVirtualCurrencyRequest, delegate
					{
						UnityEngine.Debug.Log("Saved currency to cloud");
					}, delegate(PlayFabError errorCallback)
					{
						UnityEngine.Debug.Log("Could not set currency: " + errorCallback.GenerateErrorReport());
					});
				}
				else
				{
					SubtractUserVirtualCurrencyRequest subtractUserVirtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest();
					subtractUserVirtualCurrencyRequest.Amount = 0;
					subtractUserVirtualCurrencyRequest.VirtualCurrency = "MB";
					PlayFabClientAPI.SubtractUserVirtualCurrency(subtractUserVirtualCurrencyRequest, delegate
					{
						UnityEngine.Debug.Log("Saved currency to cloud");
					}, delegate(PlayFabError errorCallback)
					{
						UnityEngine.Debug.Log("Could not set currency: " + errorCallback.GenerateErrorReport());
					});
				}
			}
		}
		catch
		{
			UnityEngine.Debug.Log("Couldn't make cloud backup!");
		}
		SaveStatsData(statsData);
	}

	public static void AddXP(int amount)
	{
		StatsData statsData = LoadStatsData();
		if (statsData == null)
		{
			statsData = new StatsData();
		}
		statsData.XP += amount;
		SaveStatsData(statsData);
	}

	public static void AddCurrency(int amount, Currency currency)
	{
		StatsData statsData = LoadStatsData();
		if (statsData == null)
		{
			statsData = new StatsData();
		}
		if (currency == Currency.Money)
		{
			statsData.Money += amount;
		}
		if (currency == Currency.Gold)
		{
			statsData.Gold += amount;
		}
		try
		{
			if (PlayFabClientAPI.IsClientLoggedIn())
			{
				UnityEngine.Debug.Log("Client is logged in.. Backup up currency to cloud");
				AddUserVirtualCurrencyRequest addUserVirtualCurrencyRequest = new AddUserVirtualCurrencyRequest();
				addUserVirtualCurrencyRequest.Amount = amount;
				addUserVirtualCurrencyRequest.VirtualCurrency = ((currency != Currency.Gold) ? "CC" : "GC");
				PlayFabClientAPI.AddUserVirtualCurrency(addUserVirtualCurrencyRequest, delegate
				{
					UnityEngine.Debug.Log("Saved currency to cloud");
				}, delegate(PlayFabError errorCallback)
				{
					UnityEngine.Debug.Log("Could not set currency: " + errorCallback.GenerateErrorReport());
				});
			}
		}
		catch
		{
			UnityEngine.Debug.Log("Couldn't make cloud backup!");
		}
		SaveStatsData(statsData);
	}

	public static void SubtractCurrency(int amount, Currency currency)
	{
		StatsData statsData = LoadStatsData();
		if (statsData == null)
		{
			statsData = new StatsData();
		}
		if (currency == Currency.Money)
		{
			statsData.Money -= amount;
		}
		if (currency == Currency.Gold)
		{
			statsData.Gold -= amount;
		}
		SaveStatsData(statsData);
	}
}
