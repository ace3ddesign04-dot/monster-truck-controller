using ExitGames.Client.Photon;
using Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : PunBehaviour
{
	public static MultiplayerManager Instance;

	public static List<GameObject> CurrentPlayers = new List<GameObject>();

	public static PhotonView[] CurrentPlayerViews;

	public static List<string> RoomsAttempted = new List<string>();

	public static string ServerVersion = "2.6";

	[HideInInspector]
	public bool TotallyReady;

	private float playersRefreshRate = 10f;

	private float nextRefreshTime;

	[HideInInspector]
	public int traileringRequesterViewID;

	public MultiplayerManager()
	{
		Instance = this;
	}

	private void Awake()
	{
		if (Object.FindObjectsOfType(GetType()).Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(this);
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.sendRate = 60;
		PhotonNetwork.sendRateOnSerialize = 60;
	}

	private void Update()
	{
		if (PhotonNetwork.inRoom && Time.time > nextRefreshTime)
		{
			RefreshCurrentPlayers();
			nextRefreshTime = Time.time + playersRefreshRate;
		}
	}

	public static void RefreshCurrentPlayers()
	{
		CurrentPlayerViews = PhotonNetwork.networkingPeer.photonViewList.Values.ToArray();
		CurrentPlayers.Clear();
		if (CurrentPlayerViews == null)
		{
			return;
		}
		PhotonView[] currentPlayerViews = CurrentPlayerViews;
		foreach (PhotonView photonView in currentPlayerViews)
		{
			if (!photonView.isMine)
			{
				CurrentPlayers.Add(photonView.gameObject);
			}
		}
	}

	public static void Connect()
	{
		if (PhotonNetwork.connectionState != ConnectionState.Connected && PhotonNetwork.connectionState != ConnectionState.Connecting)
		{
			PhotonNetwork.ConnectUsingSettings(ServerVersion);
		}
	}

	public static RoomInfo FindPrivateRoom(string password)
	{
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		foreach (RoomInfo roomInfo in roomList)
		{
			if (roomInfo.CustomProperties["Password"] != null && roomInfo.CustomProperties["Password"].ToString() == password && roomInfo.PlayerCount < roomInfo.MaxPlayers)
			{
				return roomInfo;
			}
		}
		return null;
	}

	public static RoomInfo[] GetAllTrailRaceRooms()
	{
		List<RoomInfo> list = new List<RoomInfo>();
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		foreach (RoomInfo roomInfo in roomList)
		{
			if ((GameType)roomInfo.CustomProperties["GameType"] == GameType.TrailRace && (roomInfo.CustomProperties["Password"] == null || roomInfo.CustomProperties["Password"] == string.Empty) && roomInfo.CustomProperties["TrailID"] != null && roomInfo.PlayerCount < 2 && roomInfo.IsOpen)
			{
				list.Add(roomInfo);
			}
		}
		return list.ToArray();
	}

	public static void JoinRoom()
	{
		StatsData statsData = GameState.LoadStatsData();
		PhotonNetwork.player.SetCustomProperties(new Hashtable
		{
			{
				"IsMember",
				statsData.IsMember.ToString()
			},
			{
				"XP",
				statsData.XP.ToString()
			},
			{
				"DisplayName",
				GameState.playerName
			}
		});
		switch (GameState.GameType)
		{
		case GameType.FreeRoam:
		case GameType.CaptureTheFlag:
			if (GameState.Password == null || GameState.Password == string.Empty)
			{
				JoinPublicFreeRoam();
			}
			else
			{
				JoinPrivateFreeRoam();
			}
			break;
		case GameType.TrailRace:
			if (GameState.Password == null || GameState.Password == string.Empty)
			{
				JoinPublicTrailRace();
			}
			else
			{
				JoinPrivateTrailRace();
			}
			break;
		}
	}

	public static void JoinPublicFreeRoam()
	{
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		bool flag = false;
		RoomInfo[] array = roomList;
		foreach (RoomInfo roomInfo in array)
		{
			bool flag2 = true;
			if (roomInfo.CustomProperties["Password"] != null && roomInfo.customProperties["Password"] != string.Empty)
			{
				flag2 = false;
			}
			if (roomInfo.CustomProperties["Scene"].ToString() != GameState.SceneName)
			{
				flag2 = false;
			}
			if (roomInfo.CustomProperties["CustomMapID"] != null && roomInfo.CustomProperties["CustomMapID"].ToString() != GameState.mapToDownload)
			{
				flag2 = false;
			}
			if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
			{
				flag2 = false;
			}
			if (roomInfo.CustomProperties["GameType"] == null || (GameType)roomInfo.CustomProperties["GameType"] != GameState.GameType)
			{
				flag2 = false;
			}
			if (RoomsAttempted.Contains(roomInfo.Name))
			{
				flag2 = false;
			}
			if (flag2)
			{
				RoomsAttempted.Add(roomInfo.Name);
				flag = true;
				PhotonNetwork.JoinRoom(roomInfo.Name);
				break;
			}
		}
		if (!flag)
		{
			RoomOptions roomOptions = GetRoomOptions();
			PhotonNetwork.CreateRoom(GameState.RoomName, roomOptions, TypedLobby.Default);
		}
	}

	public static void JoinPrivateFreeRoam()
	{
		RoomOptions roomOptions = GetRoomOptions();
		PhotonNetwork.JoinOrCreateRoom(GameState.RoomName, roomOptions, TypedLobby.Default);
	}

	public static void JoinPublicTrailRace()
	{
		RoomOptions roomOptions = GetRoomOptions();
		PhotonNetwork.JoinOrCreateRoom(GameState.RoomName, roomOptions, TypedLobby.Default);
	}

	public static void JoinPrivateTrailRace()
	{
		RoomOptions roomOptions = GetRoomOptions();
		PhotonNetwork.JoinOrCreateRoom(GameState.RoomName, roomOptions, TypedLobby.Default);
	}

	private static RoomOptions GetRoomOptions()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = GameState.MaxPlayers;
		roomOptions.CustomRoomProperties = GetCustomProperties();
		roomOptions.CustomRoomPropertiesForLobby = GetCustomPropertiesForLobby();
		return roomOptions;
	}

	private static Hashtable GetCustomProperties()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Scene", GameState.SceneName);
		hashtable.Add("GameType", GameState.GameType);
		hashtable.Add("Password", GameState.Password);
		hashtable.Add("TrailID", GameState.TrailID);
		hashtable.Add("HostPlayerName", GameState.playerName);
		hashtable.Add("TrailRaceBet", GameState.TrailRaceBet);
		hashtable.Add("CustomMapID", GameState.mapToDownload);
		return hashtable;
	}

	private static string[] GetCustomPropertiesForLobby()
	{
		string[] result = null;
		switch (GameState.GameType)
		{
		case GameType.FreeRoam:
		case GameType.CaptureTheFlag:
			result = new string[4]
			{
				"Scene",
				"GameType",
				"Password",
				"CustomMapID"
			};
			break;
		case GameType.TrailRace:
			result = new string[7]
			{
				"Scene",
				"GameType",
				"Password",
				"TrailID",
				"HostPlayerName",
				"TrailRaceBet",
				"CustomMapID"
			};
			break;
		}
		return result;
	}

	public static void LeaveRoom()
	{
		if (PhotonNetwork.inRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedRoom()
	{
		RoomsAttempted = new List<string>();
		if (PhotonNetwork.room.CustomProperties["CustomMapID"] != null)
		{
			GameState.mapToDownload = PhotonNetwork.room.CustomProperties["CustomMapID"].ToString();
		}
		PhotonNetwork.LoadLevel(GameState.SceneName);
	}

	public override void OnJoinedLobby()
	{
		TotallyReady = false;
		if (GameState.WaitingForRoom)
		{
			JoinRoom();
		}
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		RefreshCurrentPlayers();
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		RefreshCurrentPlayers();
		if (GameState.GameMode == GameMode.Multiplayer && GameState.GameType == GameType.TrailRace)
		{
			TrailRaceManager.Instance.OnOtherPlayerDisconnected();
		}
	}

	public override void OnLeftRoom()
	{
		if (SceneManager.GetActiveScene().name.ToLower() != "menu")
		{
			SceneManager.LoadScene("Menu");
		}
	}

	public override void OnCreatedRoom()
	{
		if (PhotonNetwork.room != null)
		{
			UnityEngine.Debug.Log("Created room: " + PhotonNetwork.room.Name);
			RoomsAttempted = new List<string>();
		}
	}

	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		JoinRoom();
	}
}
