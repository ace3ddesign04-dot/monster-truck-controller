using CustomVP;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WinchManager : MonoBehaviour
{
	public static WinchManager Instance;

	private Projector WinchZoneProjector;

	private CableType myCableType;

	private GameObject LandAnchor;

	private Transform MyCurrentWinchPoint;

	private int WinchTargetLayer = 8;

	private float WinchRadius = 20f;

	private List<WinchTarget> WinchTargets = new List<WinchTarget>();

	private List<WinchTarget> AvailableWinchTargets;

	private int SelectedTargetIndex;

	private Transform CurrentWinchTarget;

	private PhotonView CarAttachedToUs;

	private Transform WinchOfCarAttachedToUs;

	private PhotonView CarWeWantToAttachTo;

	private string CarThatSentWinchRequest;

	private float MinCableLength;

	private LineRenderer lineRenderer;

	public string MyCableID;

	public List<WinchCable> OtherPlayersCables;

	[HideInInspector]
	public bool WinchMode;

	[HideInInspector]
	public bool WinchAttached;

	[HideInInspector]
	public bool BeingWinchTarget;

	[HideInInspector]
	public bool WaitingForResponse;

	[HideInInspector]
	public bool LandAnchorMode;

	[HideInInspector]
	public bool TouchMoved;

	[HideInInspector]
	public bool WinchTowing;

	private float ropeDamping = 5000f;

	private float ropeForce = 3000f;

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

	public WinchManager()
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
		CreateWinchTargets();
		OtherPlayersCables = new List<WinchCable>();
		lineRenderer = CreateLineRenderer("Player cable line renderer");
	}

	private void Update()
	{
		if (carController != null)
		{
			DoWinch();
			carController.DontPreventFromSliding = (WinchMode || BeingWinchTarget);
			if (OtherPlayersCables != null)
			{
				foreach (WinchCable otherPlayersCable in OtherPlayersCables)
				{
					if (otherPlayersCable.IsCarMissing())
					{
						OnCableDestroyed(otherPlayersCable.CableID);
					}
					else
					{
						otherPlayersCable.UpdateCable();
					}
				}
			}
		}
	}

	private LineRenderer CreateLineRenderer(string name)
	{
		GameObject gameObject = new GameObject(name);
		LineRenderer lineRenderer = new LineRenderer();
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = (Resources.Load("Materials/WinchRope", typeof(Material)) as Material);
		lineRenderer.useWorldSpace = true;
		lineRenderer.positionCount = 2;
		lineRenderer.textureMode = LineTextureMode.Tile;
		lineRenderer.widthMultiplier = 0.1f;
		return lineRenderer;
	}

	private void TurnToLandAnchor()
	{
		CarUIControl.Instance.SwitchWinchTargetSelector(Show: false);
		CarUIControl.Instance.ShowNotification("Tap on ground", blinking: true);
		LandAnchorMode = true;
		CameraController.Instance.cameraMode = CameraController.CameraMode.Free;
		ShowHideWinchTargets(Show: false);
	}

	private void CheckLandAnchorTap(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo) && Vector3.Distance(carController.transform.position, hitInfo.point) < WinchRadius && hitInfo.collider.GetType() == typeof(TerrainCollider))
		{
			LandAnchor = (UnityEngine.Object.Instantiate(Resources.Load("Other/LandAnchor")) as GameObject);
			LandAnchor.transform.position = hitInfo.point + Vector3.up * 0.25f;
			LandAnchor.transform.LookAt(carController.transform, Vector3.up);
			AttachWinch(LandAnchor.transform);
		}
	}

	private void DoWinch()
	{
		if (CrossPlatformInputManager.GetButtonDown("ToggleWinch"))
		{
			ToggleWinch();
		}
		if (CrossPlatformInputManager.GetButtonDown("LeftArrow"))
		{
			SwitchToLeftTarget();
		}
		if (CrossPlatformInputManager.GetButtonDown("RightArrow"))
		{
			SwitchToRightTarget();
		}
		if (CrossPlatformInputManager.GetButtonDown("Attach"))
		{
			AttachWinch();
		}
		if (CrossPlatformInputManager.GetButtonDown("SendWinchRequest"))
		{
			SendWinchRequest();
		}
		if (CrossPlatformInputManager.GetButtonDown("AcceptWinchRequest"))
		{
			AcceptWinchRequest();
		}
		if (CrossPlatformInputManager.GetButtonDown("DeclineWinchRequest"))
		{
			DeclineWinchRequest();
		}
		if (CrossPlatformInputManager.GetButtonDown("Detach"))
		{
			DetachAttachedCar();
		}
		if (CrossPlatformInputManager.GetButtonUp("LandAnchor"))
		{
			TurnToLandAnchor();
		}
		if (WinchAttached)
		{
			WinchTowing = CrossPlatformInputManager.GetButton("TowWinch");
		}
		if (LandAnchorMode && UnityEngine.Input.touchCount == 1)
		{
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				TouchMoved = true;
			}
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended && !TouchMoved)
			{
				CheckLandAnchorTap(UnityEngine.Input.GetTouch(0).position);
			}
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				TouchMoved = false;
			}
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
			{
				TouchMoved = false;
			}
		}
		if (WinchMode && !WinchAttached)
		{
			carController.ExtremeBraking = 1f;
		}
		else
		{
			carController.ExtremeBraking = 0f;
		}
		if (WinchAttached)
		{
			if (CurrentWinchTarget == null)
			{
				ToggleWinch();
			}
			ApplyTug(CurrentWinchTarget.position, partsSwitcher.FrontWinchPoint.position);
			if (Time.frameCount % 2 == 0)
			{
				UpdateMyWinchCable();
			}
			if (WinchTowing)
			{
				ApplyTug(CurrentWinchTarget.position, partsSwitcher.FrontWinchPoint.position, ManualTowing: true);
			}
		}
		if (BeingWinchTarget)
		{
			if (CarAttachedToUs == null)
			{
				StopBeingWinchTarget();
			}
			ApplyTug(WinchOfCarAttachedToUs.position, MyCurrentWinchPoint.position);
		}
	}

	private void ApplyTug(Vector3 targetPos, Vector3 winchPos, bool ManualTowing = false)
	{
		float num = Vector3.Distance(winchPos, targetPos) - MinCableLength;
		if ((myCableType == CableType.CarToCar && num <= 0f && !ManualTowing) || Vector3.Distance(winchPos, targetPos) < 2f)
		{
			return;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		float t = Mathf.InverseLerp(10f, 0f, Mathf.Abs(carController.Speed));
		Vector3 normalized = (targetPos - winchPos).normalized;
		Vector3 normalized2 = Vector3.ProjectOnPlane(normalized, carController.transform.up).normalized;
		Vector3 vector = Vector3.Lerp(normalized2, normalized, t);
		Vector3 vector2 = Vector3.zero;
		Vector3 b = ropeDamping * -Vector3.Project(playerRigidbody.velocity, vector);
		if (ManualTowing && num == 0f)
		{
			b = Vector3.zero;
		}
		float num2 = ManualTowing ? 1 : 0;
		switch (myCableType)
		{
		case CableType.CarToCar:
		{
			Vector3 vector3 = Vector3.zero;
			for (int i = 0; i < carController.wheels.Count; i++)
			{
				vector3 += carController.wheels[i].wc.wheelCollider.LongForce;
			}
			vector2 = vector * num * ropeForce + vector3 + b + num2 * vector * ropeForce * 5f;
			break;
		}
		case CableType.CarToStatic:
		{
			float d = Mathf.InverseLerp(10f, 0f, carController.Speed);
			vector2 = vector * ropeForce * (Mathf.Clamp01(carController.Throttle) + num + num2) * 2f * Mathf.Max(1f, num2 * 3f);
			vector2 = ((num != 0f) ? (vector2 + b) : (vector2 * d));
			break;
		}
		}
		UnityEngine.Debug.DrawRay(winchPos, vector2, Color.magenta);
		playerRigidbody.AddForceAtPosition(vector2 * Time.timeScale, winchPos, ForceMode.Force);
	}

	private void SendWinchRequest()
	{
		if (GameState.GameType != GameType.TrailRace)
		{
			CarWeWantToAttachTo = AvailableWinchTargets[SelectedTargetIndex].transform.root.GetComponent<PhotonView>();
			photonTransformView.SendWinchRequest(CarWeWantToAttachTo);
			WaitingForResponse = true;
			CarUIControl.Instance.ToggleAttachButton(DynamicTarget: false, Show: false);
			CarUIControl.Instance.ShowNotification("Waiting for response...", blinking: true);
		}
	}

	public void OnWinchRequestAccepted(PhotonView AcceptingCar)
	{
		if (WaitingForResponse && !(CarWeWantToAttachTo != AcceptingCar))
		{
			AttachWinch();
		}
	}

	public void OnWinchRequestDeclined()
	{
		CarUIControl.Instance.ShowNotification("Other player declined winch request", blinking: false);
		if (WinchMode)
		{
			ToggleWinch();
		}
	}

	public void GetWinchRequest(string RequestingCarID)
	{
		CarThatSentWinchRequest = RequestingCarID;
	}

	private void AcceptWinchRequest()
	{
		if (WinchMode)
		{
			ToggleWinch();
		}
		PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
		int num = 0;
		PhotonView photonView;
		while (true)
		{
			if (num < array.Length)
			{
				photonView = array[num];
				if (photonView.ownerId == int.Parse(CarThatSentWinchRequest))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		photonTransformView.SendWinchAcceptation(photonView);
	}

	public void OnOtherCarAttachedToUs(PhotonView AttachingCar)
	{
		CarAttachedToUs = AttachingCar;
		WinchOfCarAttachedToUs = GetClosestTransform(CarAttachedToUs.GetComponent<BodyPartsSwitcher>().FrontWinchPoint, CarAttachedToUs.GetComponent<BodyPartsSwitcher>().RearWinchPoint, base.transform.position);
		MinCableLength = Vector3.Distance(partsSwitcher.FrontWinchPoint.position, WinchOfCarAttachedToUs.position);
		BeingWinchTarget = true;
		CarUIControl.Instance.SwitchDetachButton(Show: true);
		myCableType = CableType.CarToCar;
		MyCurrentWinchPoint = GetClosestTransform(partsSwitcher.FrontWinchPoint, partsSwitcher.RearWinchPoint, CarAttachedToUs.transform.position);
	}

	public void OnOtherCarDetachedFromUs()
	{
		StopBeingWinchTarget();
	}

	public void DeclineWinchRequest()
	{
		PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
		foreach (PhotonView photonView in array)
		{
			if (photonView.ownerId == int.Parse(CarThatSentWinchRequest))
			{
				photonTransformView.SendWinchDeclination(photonView);
				break;
			}
		}
		StopBeingWinchTarget();
	}

	private void DetachAttachedCar()
	{
		photonTransformView.SendWinchDeclination(CarAttachedToUs);
		StopBeingWinchTarget();
	}

	private void UpdateMyWinchCable()
	{
		lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, partsSwitcher.FrontWinchPoint.position);
		lineRenderer.SetPosition(1, CurrentWinchTarget.position);
	}

	public void OnDynamicCableCreated(string CableID, PhotonView car1, PhotonView car2)
	{
		WinchCable winchCable = new WinchCable();
		winchCable.CableID = CableID;
		winchCable.t1 = car1.GetComponent<BodyPartsSwitcher>().FrontWinchPoint.transform;
		BodyPartsSwitcher component = car2.GetComponent<BodyPartsSwitcher>();
		winchCable.t2 = GetClosestTransform(component.FrontWinchPoint, component.RearWinchPoint, car1.transform.position);
		winchCable.cableType = CableType.CarToCar;
		winchCable.lineRenderer = CreateLineRenderer("Cable:" + CableID);
		OtherPlayersCables.Add(winchCable);
	}

	public void OnStaticCableCreated(string CableID, PhotonView car, Vector3 Target)
	{
		WinchCable winchCable = new WinchCable();
		winchCable.CableID = CableID;
		winchCable.Car = car.GetComponent<BodyPartsSwitcher>().Winch.transform;
		winchCable.CarTargetPos = Target;
		winchCable.cableType = CableType.CarToStatic;
		winchCable.lineRenderer = CreateLineRenderer("Cable:" + CableID);
		OtherPlayersCables.Add(winchCable);
	}

	public void OnCableDestroyed(string CableID)
	{
		if (OtherPlayersCables.Count != 0)
		{
			WinchCable winchCable = OtherPlayersCables.Find((WinchCable cable) => cable.CableID == CableID);
			if (winchCable != null)
			{
				UnityEngine.Object.Destroy(winchCable.lineRenderer.gameObject);
				OtherPlayersCables.Remove(winchCable);
			}
		}
	}

	public void StopBeingWinchTarget()
	{
		BeingWinchTarget = false;
		CarUIControl.Instance.SwitchDetachButton(Show: false);
	}

	public void ToggleWinch()
	{
		if (BeingWinchTarget || carController.loadedOnOtherPlayerTrailer)
		{
			return;
		}
		WinchMode = !WinchMode;
		CarUIControl.Instance.ToggleCarControls(!WinchMode);
		CarUIControl.Instance.ToggleCarExtras(!WinchMode);
		ShowHideWinchTargets(WinchMode);
		ShowHideWinchZoneProjector(WinchMode);
		CarUIControl.Instance.SwitchWinchTargetSelector(WinchMode);
		if (WinchMode)
		{
			CameraController.Instance.SetWinchCamera();
			AvailableWinchTargets = new List<WinchTarget>();
			foreach (WinchTarget winchTarget2 in WinchTargets)
			{
				if (winchTarget2 != null && winchTarget2.gameObject != null && winchTarget2.gameObject.activeSelf && Vector3.Distance(carController.transform.position, winchTarget2.transform.position) < WinchRadius)
				{
					AvailableWinchTargets.Add(winchTarget2);
				}
			}
			if (AvailableWinchTargets.Count > 0)
			{
				CameraController.Instance.SelectedWinchTarget = AvailableWinchTargets[0].transform;
				WinchTarget winchTarget = AvailableWinchTargets[0];
				CarUIControl.Instance.ToggleAttachButton(winchTarget.DynamicTarget, Show: true);
			}
			CarUIControl.Instance.ShowNotification((AvailableWinchTargets.Count <= 0) ? "No winch targets available" : "Choose winch target", blinking: true);
			SelectedTargetIndex = 0;
			return;
		}
		WinchAttached = false;
		lineRenderer.enabled = false;
		LandAnchorMode = false;
		CarUIControl.Instance.ToggleAttachButton(DynamicTarget: false, Show: false);
		CarUIControl.Instance.HideNotification();
		CarUIControl.Instance.SwitchWinchTowButton(Show: false);
		CameraController.Instance.GetCameraBack();
		WaitingForResponse = false;
		CameraController.Instance.SelectedWinchTarget = null;
		WinchTowing = false;
		if (LandAnchor != null)
		{
			UnityEngine.Object.Destroy(LandAnchor);
		}
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			photonTransformView.SendCableDestroyingEvent(MyCableID);
		}
		if (CarWeWantToAttachTo != null)
		{
			photonTransformView.SendWinchDetachEvent(CarWeWantToAttachTo);
			CarWeWantToAttachTo = null;
		}
	}

	public void AttachWinch(Transform landAnchor = null)
	{
		Transform transform = null;
		Transform frontWinchPoint = partsSwitcher.FrontWinchPoint;
		myCableType = CableType.CarToStatic;
		if (AvailableWinchTargets.Count > 0)
		{
			transform = AvailableWinchTargets[SelectedTargetIndex].transform;
		}
		if (CarWeWantToAttachTo != null)
		{
			BodyPartsSwitcher component = CarWeWantToAttachTo.GetComponent<BodyPartsSwitcher>();
			transform = GetClosestTransform(component.FrontWinchPoint, component.RearWinchPoint, carController.transform.position);
			myCableType = CableType.CarToCar;
		}
		if (landAnchor != null)
		{
			transform = landAnchor;
		}
		CurrentWinchTarget = transform;
		ShowHideWinchTargets(Show: false);
		ShowHideWinchZoneProjector(Show: false);
		MinCableLength = Vector3.Distance(transform.position, frontWinchPoint.position);
		WinchAttached = true;
		LandAnchorMode = false;
		CarUIControl.Instance.HideNotification();
		CarUIControl.Instance.ToggleAttachButton(DynamicTarget: false, Show: false);
		CarUIControl.Instance.ToggleCarControls(Show: true);
		CarUIControl.Instance.ToggleCarExtras(Show: true);
		CarUIControl.Instance.SwitchWinchTowButton(Show: true);
		CarUIControl.Instance.SwitchWinchTargetSelector(Show: false);
		CameraController.Instance.SetSideCamera();
		CameraController.Instance.GetCameraBack();
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			MyCableID = GenerateRandomID();
			if (CarWeWantToAttachTo != null)
			{
				photonTransformView.SendWinchAttachEvent(CarWeWantToAttachTo);
				photonTransformView.SendDynamicCableCreationEvent(MyCableID, CarWeWantToAttachTo.ownerId.ToString());
			}
			else
			{
				photonTransformView.SendStaticCableCreationEvent(MyCableID, transform.position);
			}
		}
	}

	private Transform GetClosestTransform(Transform t1, Transform t2, Vector3 origin)
	{
		if (Vector3.Distance(origin, t1.position) < Vector3.Distance(origin, t2.position))
		{
			return t1;
		}
		return t2;
	}

	public void SwitchToLeftTarget()
	{
		if (AvailableWinchTargets.Count > 1)
		{
			if (SelectedTargetIndex > 0)
			{
				SelectedTargetIndex--;
			}
			else
			{
				SelectedTargetIndex = AvailableWinchTargets.Count - 1;
			}
			WinchTarget winchTarget = AvailableWinchTargets[SelectedTargetIndex];
			CarUIControl.Instance.ToggleAttachButton(winchTarget.DynamicTarget, Show: true);
			CameraController.Instance.SelectedWinchTarget = AvailableWinchTargets[SelectedTargetIndex].transform;
		}
	}

	public void SwitchToRightTarget()
	{
		if (AvailableWinchTargets.Count > 1)
		{
			if (SelectedTargetIndex == AvailableWinchTargets.Count - 1)
			{
				SelectedTargetIndex = 0;
			}
			else
			{
				SelectedTargetIndex++;
			}
			WinchTarget winchTarget = AvailableWinchTargets[SelectedTargetIndex];
			CarUIControl.Instance.ToggleAttachButton(winchTarget.DynamicTarget, Show: true);
			CameraController.Instance.SelectedWinchTarget = AvailableWinchTargets[SelectedTargetIndex].transform;
		}
	}

	private void ShowHideWinchTargets(bool Show)
	{
		foreach (WinchTarget winchTarget in WinchTargets)
		{
			if (winchTarget != null && (!(winchTarget.tView != null) || ((!(winchTarget.tView.trailer != null) || !winchTarget.tView.trailer.mpConnected || !Show) && !winchTarget.tView.onOtherPlayerTrailer)))
			{
				Vector3 position = winchTarget.transform.position;
				Vector3 position2 = carController.transform.position;
				position.y = position2.y;
				winchTarget.spriteRenderer.color = Color.green;
				winchTarget.gameObject.SetActive(Vector3.Distance(carController.transform.position, position) < WinchRadius && Show);
				winchTarget.transform.LookAt(carController.transform);
			}
		}
	}

	private void ShowHideWinchZoneProjector(bool Show)
	{
		WinchZoneProjector.gameObject.SetActive(Show);
		WinchZoneProjector.orthographicSize = WinchRadius * 2f;
		WinchZoneProjector.transform.position = carController.transform.position + Vector3.up * 10f;
	}

	public void AddWinchTarget(Vector3 Pos, Transform Parent, bool DynamicTarget)
	{
		WinchTarget component = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Other/WinchTarget"), Pos, Quaternion.identity, Parent)).GetComponent<WinchTarget>();
		component.transform.localScale = Vector3.one;
		component.gameObject.SetActive(value: false);
		component.DynamicTarget = DynamicTarget;
		WinchTargets.Add(component);
	}

	private void CreateWinchTargets()
	{
		WinchTargets = new List<WinchTarget>();
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		Terrain[] activeTerrains = Terrain.activeTerrains;
		foreach (Terrain terrain in activeTerrains)
		{
			if (!terrain || !terrain.terrainData)
				return;

			TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
			for (int j = 0; j < treeInstances.Length; j++)
			{
				TreeInstance treeInstance = treeInstances[j];
				Vector3 position = treeInstance.position;
				float x = position.x;
				Vector3 size = terrain.terrainData.size;
				position.x = x * size.x;
				float y = position.y;
				Vector3 size2 = terrain.terrainData.size;
				position.y = y * size2.y;
				float z = position.z;
				Vector3 size3 = terrain.terrainData.size;
				position.z = z * size3.z;
				AddWinchTarget(position + terrain.GetPosition() + Vector3.up, terrain.transform, DynamicTarget: false);
			}
		}
		WinchZoneProjector = (UnityEngine.Object.Instantiate(Resources.Load("Other/WinchZoneProjector", typeof(GameObject))) as GameObject).GetComponent<Projector>();
		WinchZoneProjector.gameObject.SetActive(value: false);
	}

	private string GenerateRandomID()
	{
		string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string text2 = string.Empty;
		for (int i = 0; i < 5; i++)
		{
			text2 += text[Random.Range(0, text.Length)];
		}
		return text2;
	}
}
