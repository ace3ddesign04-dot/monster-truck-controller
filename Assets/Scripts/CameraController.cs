using CustomVP;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraController : MonoBehaviour
{
	public enum CameraMode
	{
		Free,
		Follow,
		Side,
		FirstPerson,
		Ragdoll,
		WinchTargetSelection,
		Photo,
		Cinematic
	}

	public static CameraController Instance;

	[HideInInspector]
	public Transform forcedTarget;

	public CameraMode cameraMode;

	private CameraMode cameraModeBeforeWinchMode;

	private CameraMode cameraModeBeforeRagdollMode;

	[Header("Start settings")]
	public float XStart;

	public float YStart;

	public float DistanceStart = 5f;

	[Header("Common settings")]
	public float RotationDamping = 2f;

	public float HeightDamping = 2f;

	public float ShakeAmplitude = 0.5f;

	public float YMax = 70f;

	public KeyCode SlowMoToggleButton;

	[Header("Free")]
	public float SwipeSpeed = 1f;

	public float ScrollSpeed = 1f;

	public float MinDistance = 3f;

	public float MaxDistance = 10f;

	[Header("Side")]
	public float SideXAngle = 60f;

	[Header("First Person")]
	public float FirstPersonDamping = 10f;

	[Header("WinchTargetSelection")]
	public float WinchTargetSelectionHeight = 3f;

	public float CameraMovingSpeed = 1f;

	[HideInInspector]
	public Transform SelectedWinchTarget;

	[HideInInspector]
	public Transform Ragdoll;

	private float DistanceCam;

	[HideInInspector]
	public float DistanceCamTarget;

	private float CurrentXAngle;

	private float CurrentYAngle;

	private float ShakeAmount;

	private float AngleX;

	private float AngleY;

	private float TargetYAngle;

	private float desiredYAngle;

	private bool Swiping;

	private bool SlowMo;

	private bool CameraDislocated;

	public bool ForceRearView;

	private Vector3 CinematicCameraPoint;

	private float HeightAboveGround;

	private float _height;

	private Vector3 movingSpeed;

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

	private IKDriverController driver
	{
		get
		{
			if (VehicleLoader.Instance != null)
			{
				return VehicleLoader.Instance.playerDriver;
			}
			return null;
		}
	}

	private Transform target
	{
		get
		{
			if (forcedTarget != null)
			{
				return forcedTarget;
			}
			if (carController != null)
			{
				return carController.transform;
			}
			if (MenuManager.Instance != null)
			{
				return MenuManager.Instance.CameraTarget;
			}
			return null;
		}
	}

	public CameraController()
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
		DistanceCamTarget = DistanceStart;
		AngleX = XStart;
		TargetYAngle = YStart;
	}

	public void Shake()
	{
		ShakeAmount = 1f;
	}

	private void ToggleDriver(bool Show)
	{
		if (!(driver == null))
		{
			driver.ToggleDriver(Show, !Show);
		}
	}

	public string SwitchCamera()
	{
		if (driver != null && driver.KnockedOut)
		{
			return null;
		}
		ToggleDriver(Show: true);
		if (cameraMode == CameraMode.Follow)
		{
			cameraMode = CameraMode.Free;
			return "Free camera";
		}
		if (cameraMode == CameraMode.Free)
		{
			cameraMode = CameraMode.FirstPerson;
			ToggleDriver(Show: false);
			return "First Person";
		}
		if (cameraMode == CameraMode.FirstPerson)
		{
			cameraMode = CameraMode.Cinematic;
			GenerateCinematicCameraPoint();
			return "Cinematic Camera";
		}
		cameraMode = CameraMode.Follow;
		return "Follow Camera";
	}

	private void GenerateCinematicCameraPoint()
	{
		CinematicCameraPoint = target.position + UnityEngine.Random.insideUnitSphere * 40f;
		CinematicCameraPoint.y += 100f;
		HeightAboveGround = UnityEngine.Random.Range(1, 10);
		if (Physics.Raycast(CinematicCameraPoint, Vector3.down, out RaycastHit hitInfo))
		{
			ref Vector3 cinematicCameraPoint = ref CinematicCameraPoint;
			Vector3 point = hitInfo.point;
			cinematicCameraPoint.y = point.y + HeightAboveGround;
		}
	}

	public void SetCameraPos(float X, float Y, float Distance)
	{
		AngleX = X;
		TargetYAngle = Y;
		DistanceCamTarget = Distance;
	}

	public void SetWinchCamera()
	{
		cameraModeBeforeWinchMode = cameraMode;
		cameraMode = CameraMode.WinchTargetSelection;
	}

	private void OnDisable()
	{
		cameraMode = CameraMode.Follow;
	}

	public void SetRagdollCamera()
	{
		cameraModeBeforeRagdollMode = cameraMode;
		cameraMode = CameraMode.Ragdoll;
	}

	public void SetSideCamera()
	{
		cameraMode = CameraMode.Side;
	}

	public void GetCameraBack()
	{
		if (cameraMode == CameraMode.WinchTargetSelection)
		{
			cameraMode = cameraModeBeforeWinchMode;
		}
		if (cameraMode == CameraMode.Side)
		{
			cameraMode = cameraModeBeforeWinchMode;
		}
		if (cameraMode == CameraMode.Ragdoll)
		{
			cameraMode = cameraModeBeforeRagdollMode;
		}
		SelectedWinchTarget = null;
	}

	private void ToggleSlowMo()
	{
		SlowMo = !SlowMo;
		Time.timeScale = ((!SlowMo) ? 1f : 0.3f);
	}

	private void LateUpdate()
	{
		if (target == null)
		{
			return;
		}
		if (CrossPlatformInputManager.GetButtonDown("Swipe"))
		{
			Swiping = true;
		}
		if (UnityEngine.Input.touchCount == 0)
		{
			Swiping = false;
		}
		ShakeAmount = Mathf.MoveTowards(ShakeAmount, 0f, Time.deltaTime * 4f);
		if (UnityEngine.Input.GetKeyDown(SlowMoToggleButton))
		{
			ToggleSlowMo();
		}
		switch (cameraMode)
		{
		case CameraMode.Ragdoll:
			break;
		case CameraMode.Free:
			DoFreeNavigation();
			DistanceCamTarget = Mathf.Clamp(DistanceCamTarget - UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 3f, MinDistance, MaxDistance);
			DoSphereCam();
			break;
		case CameraMode.Photo:
			DoFreeNavigation();
			DistanceCamTarget = Mathf.Clamp(DistanceCamTarget - UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 3f, MinDistance, MaxDistance * 2f);
			DoSphereCam();
			break;
		case CameraMode.Follow:
			if (!(carController == null))
			{
				if (carController.Speed >= 0f)
				{
					AngleX = 0f;
				}
				if (ForceRearView || (carController.Speed < -10f && carController.WheelsOffTheGround == 0))
				{
					AngleX = 180f;
				}
				desiredYAngle = carController.FollowYAngle;
				DistanceCamTarget = carController.FollowDistance;
				bool flag = Physics.CheckSphere(base.transform.position, 0.7f);
				if (Mathf.Abs(TargetYAngle - desiredYAngle) > 3f && !flag)
				{
					TargetYAngle = Mathf.MoveTowards(TargetYAngle, desiredYAngle, Time.deltaTime * 50f);
				}
				DoSphereCam();
			}
			break;
		case CameraMode.Side:
			if (!(carController == null))
			{
				AngleX = SideXAngle;
				desiredYAngle = carController.FollowYAngle;
				bool flag = Physics.CheckSphere(base.transform.position, 0.7f);
				if (Mathf.Abs(TargetYAngle - desiredYAngle) > 3f && !flag)
				{
					TargetYAngle = Mathf.MoveTowards(TargetYAngle, desiredYAngle, Time.deltaTime * 50f);
				}
				DistanceCamTarget = carController.FollowDistance;
				DoSphereCam();
			}
			break;
		case CameraMode.FirstPerson:
			if (!(carController == null))
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, target.transform.rotation, FirstPersonDamping * Time.deltaTime);
				base.transform.position = carController.FirstPersonPoint.position;
			}
			break;
		case CameraMode.WinchTargetSelection:
			base.transform.position = Vector3.Lerp(base.transform.position, target.transform.position + Vector3.up * WinchTargetSelectionHeight, Time.deltaTime * CameraMovingSpeed);
			if (SelectedWinchTarget != null)
			{
				Quaternion b = base.transform.rotation;
				if (SelectedWinchTarget != null)
				{
					b = Quaternion.LookRotation(SelectedWinchTarget.position - base.transform.position);
				}
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 2f);
			}
			break;
		case CameraMode.Cinematic:
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, CinematicCameraPoint + UnityEngine.Random.insideUnitSphere * 5f, ref movingSpeed, Time.deltaTime * 100f);
			_height = Mathf.MoveTowards(_height, HeightAboveGround, Time.deltaTime);
			if (Physics.Raycast(base.transform.position + Vector3.up * 10f, Vector3.down, out RaycastHit hitInfo) && hitInfo.collider.GetType() == typeof(TerrainCollider))
			{
				base.transform.position = hitInfo.point + Vector3.up * _height;
			}
			if (Physics.Raycast(base.transform.position, target.position - base.transform.position, out hitInfo) && hitInfo.collider.transform.root != target && hitInfo.collider.GetType() == typeof(TerrainCollider))
			{
				GenerateCinematicCameraPoint();
			}
			base.transform.LookAt(target);
			Vector3 position = target.position;
			float x = CinematicCameraPoint.x;
			Vector3 position2 = target.position;
			if (Vector3.Distance(position, new Vector3(x, position2.y, CinematicCameraPoint.z)) > 30f)
			{
				GenerateCinematicCameraPoint();
			}
			break;
		}
		}
	}

	private void DoFreeNavigation()
	{
		if (Swiping)
		{
			if (UnityEngine.Input.touchCount == 1)
			{
				float angleX = AngleX;
				Vector2 deltaPosition = UnityEngine.Input.GetTouch(0).deltaPosition;
				AngleX = angleX + deltaPosition.x / 10f * SwipeSpeed;
				Vector2 deltaPosition2 = UnityEngine.Input.GetTouch(0).deltaPosition;
				float num = deltaPosition2.y / 10f * SwipeSpeed;
				if (!CameraDislocated || (CameraDislocated && num < 0f))
				{
					TargetYAngle -= num;
				}
				desiredYAngle = TargetYAngle;
			}
			if (UnityEngine.Input.touchCount == 2)
			{
				Vector2 a = UnityEngine.Input.GetTouch(0).position - UnityEngine.Input.GetTouch(0).deltaPosition;
				Vector2 b = UnityEngine.Input.GetTouch(1).position - UnityEngine.Input.GetTouch(1).deltaPosition;
				float magnitude = (a - b).magnitude;
				float magnitude2 = (UnityEngine.Input.GetTouch(0).position - UnityEngine.Input.GetTouch(1).position).magnitude;
				float num2 = magnitude - magnitude2;
				DistanceCamTarget += num2 * Time.deltaTime / 2f * ScrollSpeed;
			}
		}
		bool flag = Physics.CheckSphere(base.transform.position, 0.7f);
		if ((Mathf.Abs(TargetYAngle - desiredYAngle) < 3f && !flag) || desiredYAngle == 0f)
		{
			desiredYAngle = TargetYAngle;
		}
		if (Mathf.Abs(TargetYAngle - desiredYAngle) > 3f && !flag)
		{
			TargetYAngle = Mathf.MoveTowards(TargetYAngle, desiredYAngle, Time.deltaTime * 50f);
		}
	}

	private void FixedUpdate()
	{
		if (!(target == null) && cameraMode == CameraMode.Ragdoll)
		{
			Vector3 b = Ragdoll.position - target.transform.forward * 2f + Vector3.up * 2f;
			base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * 10f);
			base.transform.LookAt(Ragdoll, base.transform.up);
		}
	}

	private void DoSphereCam()
	{
		TargetYAngle = Mathf.Clamp(TargetYAngle, -45f, YMax);
		AngleY = TargetYAngle;
		DistanceCam = Mathf.Lerp(DistanceCam, DistanceCamTarget, 10f * Time.deltaTime);
		bool flag = false;
		if (carController != null && carController.WheelsOffTheGround < carController.wheels.Count)
		{
			flag = true;
		}
		Vector3 eulerAngles = target.transform.eulerAngles;
		float y = eulerAngles.y;
		if (flag)
		{
			CurrentXAngle = Mathf.LerpAngle(CurrentXAngle, y, RotationDamping * Time.deltaTime);
		}
		float num;
		if (flag)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			num = eulerAngles2.x;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		if (AngleX == 180f)
		{
			num2 = 0f - num2;
		}
		if (cameraMode == CameraMode.Follow)
		{
			CurrentYAngle = Mathf.LerpAngle(CurrentYAngle, num2, HeightDamping * Time.deltaTime);
		}
		Vector3 b = UnityEngine.Random.onUnitSphere * ShakeAmount * ShakeAmplitude;
		Quaternion rotation = Quaternion.Euler(CurrentYAngle + AngleY, CurrentXAngle + AngleX, 0f);
		Vector3 position = target.transform.position - rotation * Vector3.forward * DistanceCam;
		int num3 = 0;
		while (Physics.CheckSphere(position, 0.5f) && num3 < 20)
		{
			num3++;
			TargetYAngle += 1f;
			AngleY = TargetYAngle;
			rotation = Quaternion.Euler(CurrentYAngle + AngleY, CurrentXAngle + AngleX, 0f);
			position = target.transform.position - rotation * Vector3.forward * DistanceCam;
		}
		Quaternion rotation2 = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(CurrentYAngle + AngleY, CurrentXAngle + AngleX, 0f), Time.deltaTime * 10f);
		Vector3 position2 = target.transform.position - rotation2 * Vector3.forward * DistanceCam + b;
		CameraDislocated = (AngleY != TargetYAngle);
		base.transform.position = position2;
		base.transform.rotation = rotation2;
	}
}
