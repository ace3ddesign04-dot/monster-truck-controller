using CustomVP;
using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Transform View")]
public class PhotonTransformView : Photon.MonoBehaviour, IPunObservable
{
	private PhotonTransformViewPositionControl m_PositionControl;

	private PhotonTransformViewRotationControl m_RotationControl;

	public int vehicleStatusInterval = 4;

	private float steeringAngle;

	private PhotonView m_PhotonView;

	private PhotonRigidbodyView rigidbodyView;

	private Rigidbody _rb;

	private CarController carController;

	private CarUIControl carUIControl;

	private LightsController lightsController;

	private SuspensionController suspensionController;

	private BodyPartsSwitcher partsSwitcher;

	private VehicleDataManager vehicleDataManager;

	private CaptureTheFlagManager captureTheFlagManager;

	private IKDriverController driver;

	private bool haveSentVehicleData;

	[HideInInspector]
	public float lastSteeringAngle;

	private float lastWheelsRPM;

	private float smoothWheelsRPM;

	public bool m_firstTake = true;

	[HideInInspector]
	public TrailerController trailer;

	[HideInInspector]
	public GameObject carOnTrailer;

	private Vector3 trailerPos;

	private Quaternion trailerRot;

	private Vector3 wantedPosOnTrailer;

	[HideInInspector]
	public bool onOtherPlayerTrailer;

	[HideInInspector]
	public TrailerController trailerImOn;

	private Rigidbody rb
	{
		get
		{
			if (_rb == null)
			{
				_rb = GetComponent<Rigidbody>();
			}
			return _rb;
		}
	}

	private void Awake()
	{
		if (!PhotonNetwork.inRoom || SceneManager.GetActiveScene().name.ToLower() == "menu")
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		m_firstTake = true;
		m_PhotonView = GetComponent<PhotonView>();
		m_PositionControl = new PhotonTransformViewPositionControl();
		m_RotationControl = new PhotonTransformViewRotationControl();
		rigidbodyView = GetComponent<PhotonRigidbodyView>();
		carController = GetComponent<CarController>();
		suspensionController = GetComponent<SuspensionController>();
		partsSwitcher = GetComponent<BodyPartsSwitcher>();
		vehicleDataManager = GetComponent<VehicleDataManager>();
		lightsController = GetComponent<LightsController>();
		if (PhotonNetwork.inRoom && !base.photonView.isMine && SceneManager.GetActiveScene().name.ToLower() != "menu")
		{
			if (suspensionController != null)
			{
				suspensionController.TurnToMultiplayerCar();
			}
			MultiplayerManager.RefreshCurrentPlayers();
		}
		if (WinchManager.Instance != null && !base.photonView.isMine && GameState.GameMode == GameMode.Multiplayer && (vehicleDataManager.vehicleType == VehicleType.Crawler || vehicleDataManager.vehicleType == VehicleType.Truck || vehicleDataManager.vehicleType == VehicleType.SideBySide))
		{
			WinchManager.Instance.AddWinchTarget(base.transform.position, base.transform, DynamicTarget: true);
		}
		captureTheFlagManager = CaptureTheFlagManager.Instance;
	}

	private void Update()
	{
		if (m_PhotonView != null && m_PhotonView.isMine && Time.frameCount % vehicleStatusInterval == 0)
		{
			base.photonView.RPC("UpdateVehicleStatus", PhotonTargets.Others, new VehicleStatus(carController.Steering, partsSwitcher.Dirtiness, partsSwitcher.MudWetness, carController.AverageRPM).Serialize());
		}
		if (m_PhotonView != null && m_PhotonView.isMine && PhotonNetwork.inRoom && !haveSentVehicleData)
		{
			string @string = DataStore.GetString(GameState.CurrentVehicleID);
			base.photonView.RPC("UpdateVehicleData", PhotonTargets.OthersBuffered, @string);
			haveSentVehicleData = true;
		}
		if (captureTheFlagManager != null)
		{
			if (!captureTheFlagManager.GameInProgress && GameState.GameType == GameType.CaptureTheFlag && !captureTheFlagManager.GameOver && PhotonNetwork.playerList.Length == captureTheFlagManager.PlayerCount)
			{
				captureTheFlagManager.GameOn();
			}
			else if (GameState.GameMode == GameMode.Multiplayer && GameState.GameType == GameType.CaptureTheFlag && !captureTheFlagManager.GameInProgress && !captureTheFlagManager.GameOver)
			{
				captureTheFlagManager.GameWaiting();
			}
		}
		if (!(m_PhotonView == null) && !m_PhotonView.isMine && PhotonNetwork.connected)
		{
			steeringAngle = Mathf.Lerp(steeringAngle, lastSteeringAngle, Time.deltaTime * 5f);
			smoothWheelsRPM = Mathf.Lerp(smoothWheelsRPM, lastWheelsRPM, Time.deltaTime * 5f);
			if (suspensionController != null && Time.timeScale != 0f)
			{
				suspensionController.UpdateSuspensions(steeringAngle, smoothWheelsRPM);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!(m_PhotonView == null) && !m_PhotonView.isMine && PhotonNetwork.connected && !onOtherPlayerTrailer)
		{
			UpdatePosition();
			UpdateRotation();
			UpdateRigidbody();
		}
	}

	private void UpdatePosition()
	{
		base.transform.localPosition = m_PositionControl.UpdatePosition(base.transform.localPosition);
		if (trailer != null)
		{
			trailer.transform.position = Vector3.Lerp(trailer.transform.position, trailerPos, Time.deltaTime * 4f);
			if (Vector3.Distance(trailer.transform.position, trailerPos) > 15f)
			{
				trailer.transform.position = trailerPos;
			}
		}
	}

	private void UpdateRotation()
	{
		base.transform.localRotation = m_RotationControl.GetRotation(base.transform.localRotation);
		if (trailer != null)
		{
			trailer.transform.rotation = Quaternion.Lerp(trailer.transform.rotation, trailerRot, Time.deltaTime * 3f);
		}
	}

	private void UpdateRigidbody()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		if (!(rb == null) && !(rigidbodyView == null))
		{
			rb.velocity = rigidbodyView.Velocity;
			rb.angularVelocity = rigidbodyView.AngularVelocity;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		m_PositionControl.OnPhotonSerializeView(base.transform.localPosition, stream, info);
		m_RotationControl.OnPhotonSerializeView(base.transform.localRotation, stream, info);
		if (stream.isWriting)
		{
			if (carController.myTrailer != null)
			{
				stream.SendNext(carController.myTrailer.transform.position);
				stream.SendNext(carController.myTrailer.transform.rotation);
				if (VehicleLoader.Instance.carOnTrailer != null)
				{
					Vector3 vector = carController.myTrailer.transform.InverseTransformPoint(VehicleLoader.Instance.carOnTrailer.transform.position);
					stream.SendNext(vector.y);
				}
			}
			if (carController.loadedOnOtherPlayerTrailer)
			{
				Vector3 vector2 = carController.ownerOfTrailer.trailer.transform.InverseTransformPoint(base.transform.position);
				stream.SendNext(vector2.y);
			}
		}
		if (stream.isReading)
		{
			if (trailer != null)
			{
				trailerPos = (Vector3)stream.ReceiveNext();
				trailerRot = (Quaternion)stream.ReceiveNext();
				if (carOnTrailer != null)
				{
					float y = (float)stream.ReceiveNext();
					Vector3 localPosition = carOnTrailer.transform.localPosition;
					localPosition.y = y;
					carOnTrailer.transform.localPosition = localPosition;
				}
			}
			if (onOtherPlayerTrailer)
			{
				float y2 = (float)stream.ReceiveNext();
				Vector3 localPosition2 = wantedPosOnTrailer;
				localPosition2.y = y2;
				base.transform.localPosition = localPosition2;
				base.transform.localRotation = Quaternion.identity;
			}
		}
		if (stream.isReading && m_firstTake)
		{
			m_firstTake = false;
			base.transform.localPosition = m_PositionControl.m_NetworkPosition;
			base.transform.localRotation = m_RotationControl.m_NetworkRotation;
		}
	}

	public void SendTraileringRequest(PhotonView trailerOwner)
	{
		base.photonView.RPC("SendTraileringRequestRPC", PhotonPlayer.Find(trailerOwner.ownerId), base.photonView.viewID);
	}

	[PunRPC]
	private void SendTraileringRequestRPC(int requesterViewID)
	{
		CarUIControl.Instance.traileringRequestWindow.SetActive(value: true);
		MultiplayerManager.Instance.traileringRequesterViewID = requesterViewID;
	}

	public void AcceptTraileringRequest()
	{
		CarUIControl.Instance.traileringRequestWindow.SetActive(value: false);
		PhotonView photonView = PhotonView.Find(MultiplayerManager.Instance.traileringRequesterViewID);
		if (photonView != null)
		{
			base.photonView.RPC("AcceptTraileringRequestRPC", PhotonPlayer.Find(photonView.ownerId));
		}
		MultiplayerManager.Instance.traileringRequesterViewID = -1;
	}

	[PunRPC]
	private void AcceptTraileringRequestRPC()
	{
		VehicleLoader.Instance.playerCarController.OnLoadOnTrailerResponseAccepted(base.photonView);
	}

	public void DeclineTraierlingRequest()
	{
		CarUIControl.Instance.traileringRequestWindow.SetActive(value: false);
		PhotonView photonView = PhotonView.Find(MultiplayerManager.Instance.traileringRequesterViewID);
		if (photonView != null)
		{
			base.photonView.RPC("DeclineTraileringRequestRPC", PhotonPlayer.Find(photonView.ownerId));
		}
		MultiplayerManager.Instance.traileringRequesterViewID = -1;
	}

	[PunRPC]
	private void DeclineTraileringRequestRPC()
	{
		VehicleLoader.Instance.playerCarController.OnLoadOnTrailerResponseDeclined(base.photonView);
	}

	public void TellEveryoneImOnTrailer(int pViewID)
	{
		base.photonView.RPC("TellEveryoneImOnTrailerRPC", PhotonTargets.OthersBuffered, pViewID);
	}

	[PunRPC]
	private void TellEveryoneImOnTrailerRPC(int pViewID)
	{
		PhotonView photonView = PhotonView.Find(pViewID);
		if (!(photonView == null))
		{
			if (!photonView.isMine)
			{
				wantedPosOnTrailer = vehicleDataManager.AlignOnTrailer(photonView.tView.trailer);
				base.transform.parent = photonView.tView.trailer.transform;
				photonView.tView.trailer.mpCarOnMe = base.gameObject;
				trailerImOn = photonView.tView.trailer;
			}
			else
			{
				vehicleDataManager.AlignOnTrailer(photonView.tView.carController.myTrailer);
				base.transform.parent = photonView.tView.carController.myTrailer.transform;
				photonView.tView.carController.myTrailer.GetComponent<Rigidbody>().mass = 600f;
				photonView.tView.carController.myTrailer.mpCarOnMe = base.gameObject;
				trailerImOn = photonView.tView.carController.myTrailer;
			}
			rb.interpolation = RigidbodyInterpolation.None;
			rb.isKinematic = true;
			onOtherPlayerTrailer = true;
		}
	}

	public void TellEveryoneImOuttaTrailer(int pViewID)
	{
		base.photonView.RPC("TellEveryoneImOuttaTrailerRPC", PhotonTargets.OthersBuffered, pViewID);
	}

	[PunRPC]
	private void TellEveryoneImOuttaTrailerRPC(int pViewID)
	{
		PhotonView photonView = PhotonView.Find(pViewID);
		if (!(photonView == null))
		{
			if (photonView.isMine)
			{
				photonView.tView.carController.myTrailer.GetComponent<Rigidbody>().mass = 200f;
				photonView.tView.carController.myTrailer.mpCarOnMe = null;
			}
			else
			{
				photonView.tView.trailer.mpCarOnMe = null;
			}
			trailerImOn = null;
			base.transform.parent = null;
			onOtherPlayerTrailer = false;
			rb.isKinematic = false;
			rb.interpolation = RigidbodyInterpolation.Interpolate;
		}
	}

	public void SpawnTrailer(string trailerName)
	{
		base.photonView.RPC("SpawnTrailerRpc", PhotonTargets.OthersBuffered, trailerName);
	}

	[PunRPC]
	private void SpawnTrailerRpc(string trailerName)
	{
		trailer = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Vehicles/" + trailerName))).GetComponent<TrailerController>();
		trailer.GetComponent<Rigidbody>().isKinematic = true;
		UnityEngine.Object.Destroy(trailer.GetComponent<VehicleDataManager>());
		TrailerWheelCollider[] componentsInChildren = trailer.GetComponentsInChildren<TrailerWheelCollider>();
		foreach (TrailerWheelCollider obj in componentsInChildren)
		{
			UnityEngine.Object.Destroy(obj);
		}
		trailer.multiplayerTrailer = true;
		trailer.playerView = base.photonView;
	}

	public void ChangeTrailerMpConnectedState(bool mpConnected)
	{
		base.photonView.RPC("ChangeTrailerMpConnectedStateRPC", PhotonTargets.OthersBuffered, mpConnected);
	}

	[PunRPC]
	private void ChangeTrailerMpConnectedStateRPC(bool mpConnected)
	{
		UnityEngine.Debug.LogError("Changing mpConnected state: " + mpConnected + " : " + base.gameObject.name);
		if (trailer != null)
		{
			trailer.mpConnected = mpConnected;
		}
	}

	public void SpawnTraileredCar(string xmlData)
	{
		base.photonView.RPC("SpawnTraileredCarRPC", PhotonTargets.OthersBuffered, xmlData);
	}

	[PunRPC]
	private void SpawnTraileredCarRPC(string xmlData)
	{
		VehicleData vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(xmlData);
		carOnTrailer = (UnityEngine.Object.Instantiate(Resources.Load("Vehicles/" + vehicleData.VehicleName, typeof(GameObject))) as GameObject);
		carOnTrailer.name = vehicleData.VehicleName;
		IKDriverController component = carOnTrailer.GetComponent<IKDriverController>();
		if (component != null)
		{
			component.ToggleDriver(ShowDriver: false, ShowHands: false);
			component.enabled = false;
		}
		VehicleDataManager component2 = carOnTrailer.GetComponent<VehicleDataManager>();
		component2.LoadVehicleDataFromString(xmlData);
		carOnTrailer.GetComponent<BodyPartsSwitcher>().MergeBodyParts();
		carOnTrailer.GetComponent<BodyPartsSwitcher>().UpdateColor(Merge: false);
		component2.AlignOnTrailer(trailer.GetComponent<TrailerController>());
		carOnTrailer.transform.parent = trailer.transform;
		carOnTrailer.GetComponent<SuspensionController>().multiplayerTraileredCar = true;
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<Rigidbody>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<CarController>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<CarEffects>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<BodyPartsSwitcher>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<PhotonTransformView>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<PhotonView>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<IKDriverController>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<LightsController>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<RammingChecker>());
		UnityEngine.Object.DestroyImmediate(carOnTrailer.GetComponent<EngineController>());
		WheelComponent[] componentsInChildren = carOnTrailer.GetComponentsInChildren<WheelComponent>();
		foreach (WheelComponent wheelComponent in componentsInChildren)
		{
			UnityEngine.Object.Destroy(wheelComponent.gameObject);
		}
	}

	public void SendDisableMyCollidersEvent(PhotonPlayer player)
	{
		base.photonView.RPC("DisableMyColliders", player);
	}

	[PunRPC]
	private void DisableMyColliders()
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			collider.enabled = false;
		}
	}

	public void SendLightsChangingEvent(float LightState)
	{
		base.photonView.RPC("UpdateLights", PhotonTargets.Others, LightState.ToString());
	}

	[PunRPC]
	private void UpdateLights(string LightsState)
	{
		if (lightsController != null)
		{
			lightsController.LightsState = int.Parse(LightsState);
		}
	}

	public void SendChatMessage(string msg)
	{
		base.photonView.RPC("ReceiveChatMessage", PhotonTargets.All, msg);
	}

	[PunRPC]
	public void ReceiveChatMessage(string msg)
	{
		if (!(ChatBox.Instance == null))
		{
			ChatBox.Instance.ReceiveChatMessage(msg);
		}
	}

	public void SendWinchRequest(PhotonView targetCar)
	{
		base.photonView.RPC("GetWinchRequest", PhotonPlayer.Find(targetCar.ownerId), base.photonView.ownerId.ToString());
	}

	[PunRPC]
	public void GetWinchRequest(string requesterId)
	{
		PhotonView photonView = FindPhotonViewByID(requesterId);
		if (!(photonView == null))
		{
			string text = "Player " + photonView.owner.CustomProperties["DisplayName"].ToString() + " wants to attach winch to your vehicle";
			CarUIControl.Instance.ShowWinchRequestWindow(text);
			WinchManager.Instance.GetWinchRequest(requesterId);
		}
	}

	public void SendWinchAcceptation(PhotonView requesterCar)
	{
		base.photonView.RPC("WinchRequestAccepted", PhotonPlayer.Find(requesterCar.ownerId), base.photonView.ownerId.ToString());
	}

	[PunRPC]
	public void WinchRequestAccepted(string AcceptingCarID)
	{
		PhotonView photonView = FindPhotonViewByID(AcceptingCarID);
		if (!(photonView == null))
		{
			WinchManager.Instance.OnWinchRequestAccepted(photonView);
		}
	}

	public void SendWinchDeclination(PhotonView requesterCar)
	{
		base.photonView.RPC("WinchRequestDeclined", PhotonPlayer.Find(requesterCar.ownerId));
	}

	[PunRPC]
	public void WinchRequestDeclined()
	{
		WinchManager.Instance.OnWinchRequestDeclined();
	}

	public void SendWinchAttachEvent(PhotonView TargetCar)
	{
		base.photonView.RPC("OtherCarAttachedToUs", PhotonPlayer.Find(TargetCar.ownerId), base.photonView.ownerId.ToString());
	}

	[PunRPC]
	public void OtherCarAttachedToUs(string AttachedCarID)
	{
		PhotonView photonView = FindPhotonViewByID(AttachedCarID);
		if (!(photonView == null))
		{
			WinchManager.Instance.OnOtherCarAttachedToUs(photonView);
		}
	}

	public void SendDynamicCableCreationEvent(string CableID, string TargetCarID)
	{
		string text = CableID + "|" + base.photonView.ownerId + "|" + TargetCarID;
		base.photonView.RPC("DynamicCableCreated", PhotonTargets.Others, text);
	}

	[PunRPC]
	public void DynamicCableCreated(string xmlData)
	{
		string[] array = xmlData.Split('|');
		PhotonView car = FindPhotonViewByID(array[1]);
		PhotonView car2 = FindPhotonViewByID(array[2]);
		WinchManager.Instance.OnDynamicCableCreated(array[0], car, car2);
	}

	public void SendStaticCableCreationEvent(string CableID, Vector3 TargetPos)
	{
		string text = TargetPos.x.ToString() + "?" + TargetPos.y.ToString() + "?" + TargetPos.z.ToString();
		string text2 = CableID + "|" + base.photonView.ownerId + "|" + text;
		base.photonView.RPC("StaticCableCreated", PhotonTargets.Others, text2);
	}

	[PunRPC]
	public void StaticCableCreated(string xmlData)
	{
		string[] array = xmlData.Split('|');
		string[] array2 = array[2].Split('?');
		Vector3 target = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
		PhotonView car = FindPhotonViewByID(array[1]);
		WinchManager.Instance.OnStaticCableCreated(array[0], car, target);
	}

	public void SendCableDestroyingEvent(string CableID)
	{
		base.photonView.RPC("CableDestroyed", PhotonTargets.Others, CableID);
	}

	[PunRPC]
	public void CableDestroyed(string CableID)
	{
		WinchManager.Instance.OnCableDestroyed(CableID);
	}

	public void SendWinchDetachEvent(PhotonView targetCar)
	{
		base.photonView.RPC("OtherCarDetachedFromUs", PhotonPlayer.Find(targetCar.ownerId));
	}

	[PunRPC]
	public void OtherCarDetachedFromUs()
	{
		WinchManager.Instance.OnOtherCarDetachedFromUs();
	}

	public void RiderKnockOut(Vector3 force)
	{
		base.photonView.RPC("OtherPlayerKnockedOut", PhotonTargets.Others, force);
	}

	[PunRPC]
	public void OtherPlayerKnockedOut(Vector3 force)
	{
		GetComponent<IKDriverController>().DoKnockOut(force);
	}

	public void ImReadyToRace()
	{
		base.photonView.RPC("OtherPlayerReadyToRace", PhotonTargets.Others);
	}

	[PunRPC]
	public void OtherPlayerReadyToRace()
	{
		TrailRaceManager.Instance.OnOtherPlayerReady();
	}

	public void ImTotallyLoaded()
	{
		base.photonView.RPC("OtherPlayerTotallyLoaded", PhotonTargets.OthersBuffered, (!DataStore.GetBool("UseFBName")) ? DataStore.GetString("GeneratedName") : GameState.PlayerName);
	}

	[PunRPC]
	public void OtherPlayerTotallyLoaded(string name)
	{
		TrailRaceManager.Instance.OnOtherPlayerTotallyLoaded(name);
	}

	public void iFinishedTrailRace(float raceTime)
	{
		base.photonView.RPC("OpponentFinishedTrailRace", PhotonTargets.Others, raceTime);
	}

	[PunRPC]
	private void OpponentFinishedTrailRace(float raceTime)
	{
		TrailRaceManager.Instance.OnOtherPlayerFinished(raceTime);
	}

	public void SendRestartOffering()
	{
		base.photonView.RPC("RestartOffered", PhotonTargets.Others);
	}

	[PunRPC]
	private void RestartOffered()
	{
		TrailRaceManager.Instance.OnRestartOfferingReceived();
	}

	public void SendRestartAcceptation()
	{
		base.photonView.RPC("RestartAccepted", PhotonTargets.Others);
	}

	[PunRPC]
	private void RestartAccepted()
	{
		TrailRaceManager.Instance.OnRestartAccepted();
	}

	private PhotonView FindPhotonViewByID(string ID)
	{
		foreach (KeyValuePair<int, PhotonView> photonView in PhotonNetwork.networkingPeer.photonViewList)
		{
			if (photonView.Value.ownerId.ToString() == ID)
			{
				return photonView.Value;
			}
		}
		return null;
	}

	public void SendGameOverReport()
	{
		base.photonView.RPC("ReportGameOver", PhotonTargets.AllBuffered);
	}

	public void SendFlagCapturedBlue(int flagID)
	{
		base.photonView.RPC("SetFlagCapturedBlue", PhotonTargets.AllBuffered, flagID);
	}

	public void SendFlagCapturedRed(int flagID)
	{
		base.photonView.RPC("SetFlagCapturedRed", PhotonTargets.AllBuffered, flagID);
	}

	public void SendCTFGameOn()
	{
		base.photonView.RPC("CTFGameOn", PhotonTargets.AllBuffered);
	}

	[PunRPC]
	public void SetFlagCapturedBlue(int flagID)
	{
		captureTheFlagManager.SetFlagCaptured(flagID, PunTeams.Team.blue);
	}

	[PunRPC]
	public void SetFlagCapturedRed(int flagID)
	{
		captureTheFlagManager.SetFlagCaptured(flagID, PunTeams.Team.red);
	}

	[PunRPC]
	public void CTFGameOn()
	{
		captureTheFlagManager.GameOn();
	}

	[PunRPC]
	public void ReportGameOver()
	{
		captureTheFlagManager.ReportGameOver();
	}

	[PunRPC]
	private void UpdateVehicleData(string xmlData)
	{
		if (!base.photonView.isMine)
		{
			SuspensionController component = GetComponent<SuspensionController>();
			if (component != null)
			{
				component.TurnToMultiplayerCar();
			}
			vehicleDataManager = GetComponent<VehicleDataManager>();
			partsSwitcher = GetComponent<BodyPartsSwitcher>();
			try
			{
				if (vehicleDataManager != null)
				{
					vehicleDataManager.LoadVehicleDataFromString(xmlData);
					partsSwitcher.MergeBodyParts();
					partsSwitcher.UpdateColor(Merge: false);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Could not load vehicle data from string RPC: " + ex.Message);
			}
		}
	}

	[PunRPC]
	private void UpdateVehicleStatus(string status)
	{
		if (!base.photonView.isMine)
		{
			VehicleStatus vehicleStatus = VehicleStatus.DeSerialize(status);
			if (partsSwitcher != null)
			{
				partsSwitcher.Dirtiness = vehicleStatus.Dirtiness;
				partsSwitcher.MudWetness = vehicleStatus.Wetness;
				partsSwitcher.UpdateDirtiness();
			}
			lastSteeringAngle = vehicleStatus.SteeringAngle;
			lastWheelsRPM = vehicleStatus.WheelsRPM;
		}
	}
}
