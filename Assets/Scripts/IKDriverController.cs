using CustomVP;
using RootMotion.FinalIK;
using System.Collections;
using UnityEngine;

public class IKDriverController : MonoBehaviour
{
	public enum DriverMode
	{
		Truck,
		ATV,
		Bike
	}

	public DriverMode driverMode;

	public float LeanPower = 1f;

	public float LeanSpeed = 1f;

	public float MaxShoulderLongitudinalOffset;

	public float MaxShoulderLateralOffset;

	public float MaxShoulderVerticalOffset;

	public float KnockOutForce = 3000f;

	public Transform DriverShouldersHolder;

	public Transform DriverBody;

	public Transform LeftShoulder;

	public Transform RightShoulder;

	public Transform DriverLookTarget;

	public float HipsMaxOffset = 0.3f;

	public float MaxLookOffset;

	private Transform DefaultDriverParent;

	private Vector3 DefaultDriverPos;

	private Quaternion DefaultDriverRot;

	private Vector3 WantedShouldersPos;

	private Vector3 refVel = Vector3.one;

	private Rigidbody PelvisRigidbody;

	private RagdollUtility ragdollUtility;

	private FullBodyBipedIK DriverIKComponent;

	private CarController carController;

	private CameraController camController;

	private MotorcycleAssistant motorcycleAssistant;

	private Rigidbody rb;

	private PhotonTransformView photonTransformView;

	private GameObject Driver;

	private GameObject Hands;

	[HideInInspector]
	public bool KnockedOut;

	private bool TouchingGround;

	private LayerMask driverCollisionLayermask;

	public Transform RightLegEffector;

	public Transform RightLeg_StandTarget;

	public Transform RightLeg_RideTarget;

	public Transform RightLeg_TurnTarget;

	public Transform LeftLegEffector;

	public Transform LeftLeg_RideTarget;

	public Transform LeftLeg_TurnTarget;

	private Vector3 RightLegTargetPos;

	private Vector3 LeftLegTargetPos;

	private Quaternion RightLegTargetRot;

	private Quaternion LeftLegTargetRot;

	private Collider frontBumpCollider;

	private Collider rearBumpCollider;

	private float KnockdownTimeout;

	private Collider[] RagdollColliders;

	private void Awake()
	{
		DriverIKComponent = GetComponentInChildren<FullBodyBipedIK>(includeInactive: true);
		if (DriverIKComponent != null)
		{
			Driver = DriverIKComponent.gameObject;
		}
		LimbIK componentInChildren = GetComponentInChildren<LimbIK>(includeInactive: true);
		if (componentInChildren != null)
		{
			Hands = componentInChildren.gameObject;
		}
		if (Driver != null)
		{
			DefaultDriverParent = Driver.transform.parent;
			DefaultDriverPos = Driver.transform.localPosition;
			DefaultDriverRot = Driver.transform.localRotation;
			PelvisRigidbody = Driver.GetComponentInChildren<Rigidbody>();
			ragdollUtility = Driver.GetComponent<RagdollUtility>();
		}
		carController = GetComponent<CarController>();
		motorcycleAssistant = GetComponent<MotorcycleAssistant>();
		photonTransformView = GetComponent<PhotonTransformView>();
		rb = GetComponent<Rigidbody>();
		int layer = base.gameObject.layer;
		int num = layer;
		RagdollUtility componentInChildren2 = GetComponentInChildren<RagdollUtility>();
		if (componentInChildren2 != null)
		{
			num = componentInChildren2.gameObject.layer;
			RagdollColliders = componentInChildren2.GetComponentsInChildren<Collider>(includeInactive: true);
			for (int i = 0; i < RagdollColliders.Length; i++)
			{
				RagdollColliders[i].enabled = false;
				if (carController != null)
				{
					for (int j = 0; j < carController.BodyColliders.Length; j++)
					{
						Physics.IgnoreCollision(RagdollColliders[i], carController.BodyColliders[j], ignore: true);
					}
				}
			}
		}
		driverCollisionLayermask = ~((1 << layer) | (1 << num));
	}

	private void Start()
	{
		camController = CameraController.Instance;
	}

	private void FixedUpdate()
	{
		if (driverMode == DriverMode.ATV || driverMode == DriverMode.Bike)
		{
			CheckHeadCollision();
		}
	}

	private void Update()
	{
		DoDriver();
		KnockdownTimeout += Time.deltaTime;
		if (motorcycleAssistant != null && (frontBumpCollider == null || rearBumpCollider == null))
		{
			AssignBumpColliders();
		}
	}

	private void AssignBumpColliders()
	{
		frontBumpCollider = motorcycleAssistant.FrontWC.GetComponentInChildren<Collider>();
		rearBumpCollider = motorcycleAssistant.RearWC.GetComponentInChildren<Collider>();
	}

	private void CheckHeadCollision()
	{
		int layerMask = -67108865;
		if (Physics.CheckSphere(DriverShouldersHolder.position, 0.2f, layerMask))
		{
			DoKnockOut(Vector3.zero);
		}
	}

	private void DoDriver()
	{
		if (!(DriverShouldersHolder == null))
		{
			switch (driverMode)
			{
			case DriverMode.Truck:
				DoTruck();
				break;
			case DriverMode.ATV:
				DoATV();
				break;
			case DriverMode.Bike:
				DoBike();
				break;
			}
		}
	}

	private void DoATV()
	{
		Vector3 target = -carController.acceleration / 50f * LeanPower;
		WantedShouldersPos = Vector3.SmoothDamp(WantedShouldersPos, target, ref refVel, 0.2f);
		WantedShouldersPos.y = Mathf.Clamp(WantedShouldersPos.y, 0f - MaxShoulderVerticalOffset, MaxShoulderVerticalOffset);
		WantedShouldersPos.x = (WantedShouldersPos.z = 0f);
		DriverShouldersHolder.localPosition = Vector3.MoveTowards(DriverShouldersHolder.localPosition, WantedShouldersPos, Time.deltaTime * LeanSpeed);
		if (carController != null)
		{
			if (LeftShoulder != null && RightShoulder != null)
			{
				LeftShoulder.localPosition = new Vector3(0f - Mathf.LerpUnclamped(MaxShoulderLateralOffset, 0f, carController.Steering / carController.maxSteeringAngle + 1f), 0f, Mathf.Lerp(0f, MaxShoulderLongitudinalOffset, Mathf.Clamp(carController.Steering, 0f, carController.maxSteeringAngle) / carController.maxSteeringAngle));
				RightShoulder.localPosition = new Vector3(0f - Mathf.LerpUnclamped(MaxShoulderLateralOffset, 0f, carController.Steering / carController.maxSteeringAngle + 1f), 0f, Mathf.Lerp(0f, MaxShoulderLongitudinalOffset, (0f - Mathf.Clamp(carController.Steering, 0f - carController.maxSteeringAngle, 0f)) / carController.maxSteeringAngle));
			}
			if (DriverBody != null)
			{
				DriverBody.localPosition = new Vector3(Mathf.LerpUnclamped(0f - HipsMaxOffset, 0f, carController.Steering / carController.maxSteeringAngle + 1f), 0f, 0f);
			}
		}
	}

	private void DoTruck()
	{
		Vector3 target = -carController.acceleration / 50f * LeanPower;
		WantedShouldersPos = Vector3.SmoothDamp(WantedShouldersPos, target, ref refVel, 0.2f);
		WantedShouldersPos.x = Mathf.Clamp(WantedShouldersPos.x, 0f - MaxShoulderLateralOffset, MaxShoulderLateralOffset);
		WantedShouldersPos.y = Mathf.Clamp(WantedShouldersPos.y, 0f - MaxShoulderVerticalOffset, MaxShoulderVerticalOffset);
		WantedShouldersPos.z = Mathf.Clamp(WantedShouldersPos.z, 0f - MaxShoulderLongitudinalOffset, MaxShoulderLongitudinalOffset);
		DriverShouldersHolder.localPosition = Vector3.MoveTowards(DriverShouldersHolder.localPosition, WantedShouldersPos, Time.deltaTime * LeanSpeed);
		if (carController != null)
		{
			DriverLookTarget.localPosition = new Vector3(Mathf.LerpUnclamped(0f - MaxLookOffset, 0f, carController.Steering / carController.maxSteeringAngle + 1f), 0f, 0f);
		}
	}

	private void DoBike()
	{
		Vector3 target = -carController.acceleration / 50f * LeanPower;
		WantedShouldersPos = Vector3.SmoothDamp(WantedShouldersPos, target, ref refVel, 0.2f);
		WantedShouldersPos.y = Mathf.Clamp(WantedShouldersPos.y, 0f - MaxShoulderVerticalOffset, MaxShoulderVerticalOffset);
		WantedShouldersPos.x = (WantedShouldersPos.z = 0f);
		DriverShouldersHolder.localPosition = Vector3.MoveTowards(DriverShouldersHolder.localPosition, WantedShouldersPos, Time.deltaTime * LeanSpeed);
		if (!(motorcycleAssistant == null))
		{
			float f = 0f;
			if (rb != null)
			{
				Vector3 vector = base.transform.InverseTransformDirection(rb.velocity);
				f = vector.z * 3.6f;
			}
			if (Mathf.Abs(f) < 2f && motorcycleAssistant.IsBikeGrounded)
			{
				RightLegTargetPos = RightLeg_StandTarget.position;
				RightLegTargetRot = RightLeg_StandTarget.rotation;
			}
			else
			{
				RightLegTargetPos = Vector3.Lerp(RightLeg_RideTarget.position, RightLeg_TurnTarget.position, (0f - motorcycleAssistant.lean) / motorcycleAssistant.MaxLean);
				RightLegTargetRot = Quaternion.Lerp(RightLeg_RideTarget.rotation, RightLeg_TurnTarget.rotation, (0f - motorcycleAssistant.lean) / motorcycleAssistant.MaxLean);
			}
			LeftLegTargetPos = Vector3.Lerp(LeftLeg_RideTarget.position, LeftLeg_TurnTarget.position, motorcycleAssistant.lean / motorcycleAssistant.MaxLean);
			LeftLegTargetRot = Quaternion.Lerp(LeftLeg_RideTarget.rotation, LeftLeg_TurnTarget.rotation, motorcycleAssistant.lean / motorcycleAssistant.MaxLean);
			RightLegEffector.position = Vector3.MoveTowards(RightLegEffector.position, RightLegTargetPos, Time.deltaTime * 2f);
			RightLegEffector.rotation = Quaternion.RotateTowards(RightLegEffector.rotation, RightLegTargetRot, Time.deltaTime * 100f);
			LeftLegEffector.position = Vector3.MoveTowards(LeftLegEffector.position, LeftLegTargetPos, Time.deltaTime * 2f);
			LeftLegEffector.rotation = Quaternion.RotateTowards(LeftLegEffector.rotation, LeftLegTargetRot, Time.deltaTime * 100f);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (carController == null || driverMode == DriverMode.Truck || col.collider.transform.root.gameObject.GetPhotonView() != null)
		{
			return;
		}
		if (col.impulse.magnitude > KnockOutForce)
		{
			if (Vector3.Angle(-base.transform.forward, col.impulse) < 40f)
			{
				DoKnockOut(-col.relativeVelocity / 5f + Vector3.up * col.relativeVelocity.magnitude / 5f);
			}
			if (Vector3.Angle(-base.transform.up, col.impulse) < 30f)
			{
				DoKnockOut(Vector3.zero);
			}
		}
		if (motorcycleAssistant != null)
		{
			ContactPoint[] contacts = col.contacts;
			for (int i = 0; i < contacts.Length; i++)
			{
				ContactPoint contactPoint = contacts[i];
				if (contactPoint.thisCollider.Equals(frontBumpCollider) || contactPoint.thisCollider.Equals(rearBumpCollider))
				{
					if (Vector3.Angle(base.transform.up, contactPoint.normal) > 60f && Vector3.Angle(base.transform.forward, contactPoint.normal) > 45f)
					{
						DoKnockOut(Vector3.zero);
					}
					break;
				}
			}
		}
		TouchingGround = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		TouchingGround = false;
	}

	public void DoKnockOut(Vector3 force)
	{
		StartCoroutine(TurnToRagdoll(force));
	}

	public IEnumerator TurnToRagdoll(Vector3 force)
	{
		if (!DriverIKComponent.enabled || !DriverIKComponent.gameObject.activeSelf || KnockdownTimeout < 5f)
		{
			yield break;
		}
		for (int i = 0; i < RagdollColliders.Length; i++)
		{
			RagdollColliders[i].enabled = true;
		}
		if (GameState.GameMode == GameMode.Multiplayer)
		{
			photonTransformView.RiderKnockOut(force);
		}
		KnockdownTimeout = 0f;
		bool DriverWasDisabled = !Driver.activeSelf;
		if (DriverWasDisabled)
		{
			ToggleDriver(ShowDriver: true, ShowHands: false);
		}
		KnockedOut = true;
		DriverIKComponent.enabled = false;
		Driver.transform.parent = null;
		ragdollUtility.EnableRagdoll();
		if (carController != null)
		{
			carController.vehicleIsActive = false;
		}
		yield return new WaitForSeconds(0.01f);
		PelvisRigidbody.AddForce(force * 3000f);
		if (carController != null && camController.cameraMode != CameraController.CameraMode.Cinematic)
		{
			camController.SetRagdollCamera();
			camController.Ragdoll = PelvisRigidbody.transform;
		}
		yield return new WaitForSeconds(3f);
		GetDriverBack();
		if (carController != null)
		{
			carController.FlipCar();
			if (camController.cameraMode != CameraController.CameraMode.Cinematic)
			{
				camController.GetCameraBack();
			}
			carController.vehicleIsActive = true;
		}
		if (DriverWasDisabled)
		{
			ToggleDriver(ShowDriver: false, ShowHands: true);
		}
		KnockedOut = false;
		for (int j = 0; j < RagdollColliders.Length; j++)
		{
			RagdollColliders[j].enabled = false;
		}
	}

	private void GetDriverBack()
	{
		DriverIKComponent.enabled = true;
		if (DefaultDriverParent != null)
		{
			Driver.transform.parent = DefaultDriverParent;
		}
		ragdollUtility.DisableRagdoll();
		Driver.transform.localPosition = DefaultDriverPos;
		Driver.transform.localRotation = DefaultDriverRot;
	}

	public void ToggleDriver(bool ShowDriver, bool ShowHands)
	{
		if (!(Driver == null) && !(Hands == null))
		{
			Driver.SetActive(ShowDriver);
			Hands.SetActive(ShowHands);
		}
	}
}
