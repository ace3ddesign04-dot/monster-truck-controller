using System;
using UnityEngine;
using UnityEngine.UI;

public class CaptureTheFlagManager : MonoBehaviour
{
	private int _PlayerCount = 4;

	public Color NeutralColor = Color.green;

	public FlagPoint[] FlagPoints;

	public GameObject CTFIndicators;

	public Image[] FlagIndicators;

	public Image PlayerTeamIndicator;

	public int PlayersReportingGameOver;

	public GameObject playersWaitingMessage;

	public bool SentGameOver;

	public bool GameOver;

	public bool GameInProgress;

	public PhotonView photonView;

	public PhotonTransformView transformView;

	public CarUIControl carUIControl;

	public PunTeams.Team winningTeam;

	public PunTeams.Team myTeam;

	public static CaptureTheFlagManager Instance;

	public int PlayerCount => _PlayerCount;

	public CaptureTheFlagManager()
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
		FlagPoints = Utility.FindObjectsOfTypeAll<FlagPoint>().ToArray();
		if (FlagPoints != null && FlagPoints.Length > 0)
		{
			Array.Sort(FlagPoints, (FlagPoint fp1, FlagPoint fp2) => fp1.gameObject.name.CompareTo(fp2.gameObject.name));
		}
		for (int i = 0; i < FlagPoints.Length; i++)
		{
			FlagPoints[i].FlagPointID = i;
		}
		if (GameState.GameType != GameType.CaptureTheFlag)
		{
			CTFIndicators.SetActive(value: false);
			for (int j = 0; j < FlagPoints.Length; j++)
			{
				FlagPoints[j].gameObject.SetActive(value: false);
			}
		}
		else
		{
			CTFIndicators.SetActive(value: true);
			for (int k = 0; k < FlagPoints.Length; k++)
			{
				FlagPoints[k].gameObject.SetActive(value: true);
			}
		}
	}

	public void GameOn()
	{
		if (!(photonView == null) && !GameInProgress && MultiplayerManager.Instance.TotallyReady)
		{
			playersWaitingMessage.SetActive(value: false);
			carUIControl.GameOn();
			GameInProgress = true;
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;
		}
	}

	public void GameWaiting()
	{
		playersWaitingMessage.SetActive(value: true);
	}

	private void LoadView()
	{
		PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
		PhotonView[] array2 = array;
		int num = 0;
		PhotonView photonView;
		while (true)
		{
			if (num < array2.Length)
			{
				photonView = array2[num];
				UnityEngine.Debug.Log("Looking for view.. Current: " + photonView.viewID);
				if (photonView.isMine)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		UnityEngine.Debug.Log("Found our view!");
		myTeam = PhotonNetwork.player.GetTeam();
		UnityEngine.Debug.Log("Assigned our team as: " + myTeam);
		this.photonView = photonView;
		transformView = photonView.gameObject.GetComponent<PhotonTransformView>();
		carUIControl = UnityEngine.Object.FindObjectOfType<CarUIControl>();
		photonView.gameObject.GetComponent<VehicleDataManager>().Team = myTeam;
		carUIControl.ShowTeam(myTeam);
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
		{
			PlayerTeamIndicator.color = Color.blue;
		}
		else
		{
			PlayerTeamIndicator.color = Color.red;
		}
		Image[] flagIndicators = FlagIndicators;
		foreach (Image image in flagIndicators)
		{
			image.color = Color.green;
		}
	}

	private void Update()
	{
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.none)
		{
			return;
		}
		if (photonView == null && GameState.GameType == GameType.CaptureTheFlag && MultiplayerManager.Instance.TotallyReady)
		{
			LoadView();
		}
		else
		{
			if (GameOver || GameState.GameType != GameType.CaptureTheFlag || !MultiplayerManager.Instance.TotallyReady)
			{
				return;
			}
			bool flag = true;
			Color color = Color.clear;
			FlagPoint[] flagPoints = FlagPoints;
			foreach (FlagPoint flagPoint in flagPoints)
			{
				if (flagPoint.CurrentColor != Color.red && flagPoint.CurrentColor != Color.blue)
				{
					flag = false;
				}
				else if (color != Color.clear && flagPoint.CurrentColor != color)
				{
					flag = false;
				}
				else if (flagPoint.CurrentColor == Color.green)
				{
					flag = false;
				}
				if (color == Color.clear)
				{
					color = flagPoint.CurrentColor;
				}
				if (!flag)
				{
					break;
				}
			}
			if (flag && !SentGameOver && GameInProgress)
			{
				winningTeam = ((color == Color.red) ? PunTeams.Team.red : PunTeams.Team.blue);
				transformView.SendGameOverReport();
				SentGameOver = true;
				UnityEngine.Debug.Log("SENT GAME OVER - WAITING FOR OTHERS - " + winningTeam.ToString());
			}
			if (SentGameOver && (PlayersReportingGameOver == PhotonNetwork.playerList.Length || PlayersReportingGameOver > 1))
			{
				UnityEngine.Debug.Log("GAME IS OVER!");
				GameOver = true;
				GameInProgress = false;
				carUIControl.CaptureTheFlagGameOver(winningTeam, PhotonNetwork.player.GetTeam());
			}
		}
	}

	public void ReportGameOver()
	{
		UnityEngine.Debug.Log("Someone reported game over!");
		PlayersReportingGameOver++;
	}

	public void SetFlagCaptured(int flagID, PunTeams.Team team)
	{
		FlagPoint[] flagPoints = FlagPoints;
		foreach (FlagPoint flagPoint in flagPoints)
		{
			if (flagPoint.FlagPointID == flagID)
			{
				flagPoint.SwitchColor((team != PunTeams.Team.blue) ? Color.red : Color.blue);
			}
		}
		if (flagID < FlagIndicators.Length)
		{
			FlagIndicators[flagID].color = ((team != PunTeams.Team.blue) ? Color.red : Color.blue);
		}
	}
}
