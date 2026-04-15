using CustomVP;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TrailerController : MonoBehaviour
{
	private CarUIControl _ui;

	public TrailerWheelCollider[] wc;

	public Transform[] wheels;

	public LineRenderer[] straps;

	public Transform[] strapsMounts;

	public Collider supportCollider;

	public Vector3 connectPoint;

	public Vector3 center;

	private Rigidbody _rb;

	[HideInInspector]
	public bool connected;

	private ConfigurableJoint joint;

	[HideInInspector]
	public bool multiplayerTrailer;

	[HideInInspector]
	public PhotonView playerView;

	public bool mpConnected;

	public GameObject mpCarOnMe;

	private VehicleDataManager loadedVehicle;

	private WheelComponent[] loadedVehicleWheelColliders;

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

	private VehicleDataManager dataManager
	{
		get
		{
			if (VehicleLoader.Instance != null)
			{
				return VehicleLoader.Instance.playerDataManager;
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

	private CarUIControl ui
	{
		get
		{
			if (_ui == null)
			{
				_ui = CarUIControl.Instance;
			}
			return _ui;
		}
	}

	public Rigidbody rb
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

	private void Start()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(base.transform.TransformPoint(connectPoint), 0.05f);
		Gizmos.DrawSphere(base.transform.TransformPoint(center), 0.05f);
	}

	public void ConnectToCar()
	{
		if (connected)
		{
			Detach();
		}
		rb.isKinematic = true;
		AlignByVehicle();
		if (loadedVehicle != null)
		{
			loadedVehicle.GetComponent<Rigidbody>().isKinematic = true;
			loadedVehicle.AlignOnTrailer(this);
		}
		joint = base.gameObject.AddComponent<ConfigurableJoint>();
		joint.connectedBody = carController.GetComponent<Rigidbody>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.angularXMotion = ConfigurableJointMotion.Limited;
		joint.angularYMotion = ConfigurableJointMotion.Limited;
		joint.angularZMotion = ConfigurableJointMotion.Limited;
		joint.anchor = connectPoint;
		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = carController.transform.InverseTransformPoint(partsSwitcher.RearWinchPoint.position);
		SoftJointLimit lowAngularXLimit = joint.lowAngularXLimit;
		lowAngularXLimit.limit = -30f;
		joint.lowAngularXLimit = lowAngularXLimit;
		SoftJointLimit highAngularXLimit = joint.highAngularXLimit;
		highAngularXLimit.limit = 30f;
		joint.highAngularXLimit = highAngularXLimit;
		SoftJointLimit angularYLimit = joint.angularYLimit;
		angularYLimit.limit = 60f;
		joint.angularYLimit = angularYLimit;
		SoftJointLimit angularZLimit = joint.angularZLimit;
		angularZLimit.limit = 30f;
		joint.angularZLimit = angularZLimit;
		supportCollider.gameObject.SetActive(value: false);
		connected = true;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.isKinematic = false;
		if (loadedVehicle != null)
		{
			loadedVehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
			loadedVehicle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			loadedVehicle.GetComponent<Rigidbody>().isKinematic = false;
		}
		if (ui != null)
		{
			ui.detachTrailerButton.SetActive(value: true);
		}
		if (PhotonNetwork.inRoom)
		{
			photonTransformView.ChangeTrailerMpConnectedState(connected);
		}
	}

	public void AlignByVehicle()
	{
		Transform transform = base.transform;
		Vector3 position = partsSwitcher.RearWinchPoint.position;
		Transform transform2 = partsSwitcher.transform;
		Vector3 a = connectPoint;
		Vector3 localScale = base.transform.localScale;
		transform.position = position - transform2.TransformVector(a * localScale.x);
		base.transform.rotation = carController.transform.rotation;
	}

	public void VehicleLoadedOnMe(GameObject vehicle)
	{
		loadedVehicle = vehicle.GetComponent<VehicleDataManager>();
		loadedVehicleWheelColliders = loadedVehicle.GetComponentsInChildren<WheelComponent>();
	}

	public void Detach()
	{
		if (joint != null)
		{
			UnityEngine.Object.DestroyImmediate(joint);
		}
		connected = false;
		supportCollider.gameObject.SetActive(value: true);
		if (ui != null)
		{
			ui.detachTrailerButton.SetActive(value: false);
		}
		if (PhotonNetwork.inRoom)
		{
			photonTransformView.ChangeTrailerMpConnectedState(connected);
		}
	}

	public void Attach()
	{
		if (!(carController == null))
		{
			ConnectToCar();
		}
	}

	private void Update()
	{
		if (multiplayerTrailer)
		{
			if (playerView == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			return;
		}
		for (int i = 0; i < straps.Length; i++)
		{
			if (loadedVehicle != null)
			{
				straps[i].SetPositions(new Vector3[2]
				{
					strapsMounts[i].position,
					loadedVehicleWheelColliders[i].GetVisualWheelPosition()
				});
			}
			straps[i].gameObject.SetActive(loadedVehicle != null);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || CrossPlatformInputManager.GetButtonUp("DetachTrailer"))
		{
			Detach();
		}
		if (CrossPlatformInputManager.GetButtonUp("AttachTrailer"))
		{
			Attach();
		}
		if (joint == null && connected)
		{
			Detach();
		}
		for (int j = 0; j < wheels.Length; j++)
		{
			wheels[j].transform.position = wc[j].GetVisualWheelPosition();
			float num = (j % 2 != 0) ? 1 : (-1);
			wheels[j].Rotate(wheels[j].right, wc[j].perFrameRotation * num, Space.World);
			if (connected)
			{
				wc[j].currentBrakeTorque = carController.currentBrakeTorque;
			}
			else
			{
				wc[j].currentBrakeTorque = 1000f;
			}
		}
		if (!(ui != null) || !(partsSwitcher != null))
		{
			return;
		}
		Vector3 a = Vector3.zero;
		if (partsSwitcher.RearWinchPoint != null)
		{
			a = partsSwitcher.RearWinchPoint.position;
		}
		a.y = 0f;
		Vector3 b = base.transform.TransformPoint(connectPoint);
		b.y = 0f;
		GameObject attachTrailerButton = ui.attachTrailerButton;
		int active;
		if (Vector3.Distance(a, b) < 1f && !connected && dataManager.vehicleType != VehicleType.ATV)
		{
			Vector3 up = base.transform.up;
			if (up.y > 0f && !WinchManager.Instance.BeingWinchTarget && !WinchManager.Instance.WinchMode)
			{
				active = ((!carController.loadedOnOtherPlayerTrailer) ? 1 : 0);
				goto IL_02ba;
			}
		}
		active = 0;
		goto IL_02ba;
		IL_02ba:
		attachTrailerButton.SetActive((byte)active != 0);
		GameObject gameObject = ui.swapVehiclesButton.gameObject;
		int active2;
		if (Vector3.Distance(base.transform.position, partsSwitcher.transform.position) < 10f && !connected && loadedVehicle != null)
		{
			Vector3 up2 = base.transform.up;
			if (up2.y > 0f && !WinchManager.Instance.BeingWinchTarget && !WinchManager.Instance.WinchMode)
			{
				active2 = ((!carController.loadedOnOtherPlayerTrailer) ? 1 : 0);
				goto IL_0362;
			}
		}
		active2 = 0;
		goto IL_0362;
		IL_0362:
		gameObject.SetActive((byte)active2 != 0);
	}
}
