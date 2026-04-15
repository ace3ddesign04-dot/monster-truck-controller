using CustomVP;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EngineController : MonoBehaviour
{
	public Pulley[] pulleys;

	public Pulley ThrottleValve;

	public bool Turbo;

	public bool PurchasedTurbo;

	[HideInInspector]
	public bool Diesel;

	private SurfaceManager surfaceManager;

	private CarController carController;

	private EngineSoundProcessor engineSoundProcessor;

	private float revvingSpeed;

	private float Speed;

	private Vector3 lastPos;

	private float TimeSinceGearSwitching;

	private bool CarGrounded;

	private bool Skidding;

	private bool SkiddingReallyMuch;

	private float throttle;

	private float yInput;

	private Vector3 ThrottleValveDefaultRotation;

	[HideInInspector]
	public bool NeutralGear;

	[HideInInspector]
	public bool ReverseGear;

	[HideInInspector]
	public float RPM;

	[HideInInspector]
	public int Gear;

	[HideInInspector]
	public float TopGear = 9f;

	public float FakeRPMTarget;

	private float smoothThrottle;

	public float[] Gears
	{
		get
		{
			if (carController != null)
			{
				return carController.GearRatios;
			}
			return GearsManager.DefaultGears;
		}
	}

	public int MaxGear
	{
		get
		{
			if (carController != null)
			{
				return carController.MaxGear;
			}
			return 5;
		}
	}

	public float minRpm => 800f;

	public float maxRpm => 6000f;

	public float gearDownRpm => 3000f;

	public float underThrottleGearDownRpm => 3500f;

	public float gearUpRpm => 5500f;

	public float throttleRpmBoost => 500f;

	public void SetDiesel(bool isDiesel)
	{
		Diesel = isDiesel;
	}

	private bool MovingBack()
	{
		if (carController == null)
		{
			return false;
		}
		for (int i = 0; i < carController.wheels.Count; i++)
		{
			if (carController.wheels[i].wc.rpm > 0f)
			{
				return false;
			}
		}
		return true;
	}

	private void Awake()
	{
		surfaceManager = SurfaceManager.Instance;
		carController = GetComponent<CarController>();
		if (ThrottleValve != null && ThrottleValve.t != null)
		{
			ThrottleValveDefaultRotation = ThrottleValve.t.localEulerAngles;
		}
	}

	private void Update()
	{
		if (engineSoundProcessor == null)
		{
			LoadEngineSounds();
		}
		CalculateSpeed();
		SimulateEngine();
		DoGearShifting();
		DoEngineSounds();
		RotatePulleys();
	}

	private void OnDisable()
	{
		if (engineSoundProcessor != null)
		{
			engineSoundProcessor.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (engineSoundProcessor != null)
		{
			engineSoundProcessor.enabled = true;
		}
	}

	private void LoadEngineSounds()
	{
		if (carController == null || carController.vehicleDataManager == null)
		{
			return;
		}
		VehicleData vehicleData = carController.vehicleDataManager.GetVehicleData();
		if (Diesel)
		{
			engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/DieselTruck"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			Turbo = true;
			engineSoundProcessor.Turbo = true;
		}
		else if (carController.vehicleDataManager.vehicleType == VehicleType.Truck)
		{
			if (carController.EngineBlockStage >= 2)
			{
				engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/GasTruckBigBlock"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			}
			else
			{
				engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/GasTruck"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			}
			Turbo = PurchasedTurbo;
			engineSoundProcessor.Turbo = PurchasedTurbo;
		}
		else if (carController.vehicleDataManager.vehicleType == VehicleType.SideBySide)
		{
			engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/SideBySide"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			Turbo = false;
			engineSoundProcessor.Turbo = false;
		}
		else if (carController.vehicleDataManager.vehicleType == VehicleType.Crawler)
		{
			engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/Crawler"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			Turbo = false;
			engineSoundProcessor.Turbo = false;
		}
		else
		{
			engineSoundProcessor = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/ATV"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<EngineSoundProcessor>();
			Turbo = false;
			engineSoundProcessor.Turbo = false;
		}
	}

	private void RotatePulleys()
	{
		if (pulleys != null)
		{
			Pulley[] array = pulleys;
			foreach (Pulley pulley in array)
			{
				pulley.t.Rotate(pulley.Rotation + pulley.Rotation * smoothThrottle);
			}
		}
		if (ThrottleValve != null && ThrottleValve.t != null)
		{
			ThrottleValve.t.localEulerAngles = Vector3.Lerp(ThrottleValveDefaultRotation, ThrottleValveDefaultRotation + ThrottleValve.Rotation, smoothThrottle);
		}
	}

	private void CalculateSpeed()
	{
		if (carController != null)
		{
			Speed = Mathf.Abs(carController.Speed);
			return;
		}
		Vector3 vector = base.transform.position - lastPos;
		float target = Vector3.ProjectOnPlane(vector, base.transform.up).magnitude / Time.deltaTime * 2.33f;
		Speed = Mathf.MoveTowards(Speed, target, Time.deltaTime * 20f);
		Speed = Mathf.Abs(Speed);
		lastPos = base.transform.position;
	}

	private void SimulateEngine()
	{
		throttle = ((!(carController != null)) ? 0f : Mathf.Abs(carController.Throttle));
		if (FakeRPMTarget > 1000f)
		{
			throttle = 1f;
		}
		smoothThrottle = Mathf.MoveTowards(smoothThrottle, throttle, Time.deltaTime * 10f);
		float num = 0f;
		float num2 = 0f;
		Skidding = false;
		SkiddingReallyMuch = false;
		CarGrounded = true;
		if (carController == null)
		{
			num2 = minRpm + Speed * 5f * Gears[Gear] * TopGear;
		}
		else
		{
			yInput = UnityEngine.Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical");
			float num3 = throttleRpmBoost;
			float num4 = (!carController.LowGear) ? 1f : carController.LowGearRatio;
			CarGrounded = carController.Grounded();
			if (!carController.vehicleIsActive)
			{
				throttle = yInput;
				num3 = maxRpm - minRpm;
			}
			if (NeutralGear)
			{
				num3 = maxRpm - minRpm;
			}
			int num5 = 0;
			for (int i = 0; i < carController.wheels.Count; i++)
			{
				if (carController.wheels[i].wc.wheelCollider != null)
				{
					carController.wheels[i].wc.wheelCollider.rpmLimit = 275f / carController.wheels[i].wc.wheelRadius / Gears[Gear] / TopGear / num4;
					if (carController.wheels[i].power)
					{
						num += carController.wheels[i].wc.rpm * carController.wheels[i].wc.wheelRadius;
						num5++;
					}
				}
			}
			num /= (float)num5;
			num2 = minRpm + Mathf.Abs(num * 2f) * Gears[Gear] * TopGear * num4 + num3 * throttle;
			num2 = Mathf.Clamp(num2, minRpm, maxRpm);
			Skidding = (Mathf.Abs(num / 2.5f / Speed) > 2f && CarGrounded);
			if (carController.wheels.Count == 2)
			{
				Skidding = false;
			}
			SkiddingReallyMuch = (Mathf.Abs(num / 2.5f / Speed) > 4f && CarGrounded);
		}
		if (FakeRPMTarget > 0f)
		{
			num2 = FakeRPMTarget;
		}
		float num6 = NeutralGear ? 40 : ((!(RPM < num2)) ? 5 : 25);
		RPM = Mathf.SmoothDamp(RPM, num2, ref revvingSpeed, Time.deltaTime * num6);
	}

	private void DoGearShifting()
	{
		if (FakeRPMTarget > 0f)
		{
			return;
		}
		int gear = Gear;
		if (carController != null && carController.transmissionType == TransmissionType.Manual)
		{
			if (CrossPlatformInputManager.GetButtonDown("ShiftUp") && Gear + 1 < MaxGear)
			{
				ShiftGear(Up: true);
			}
			if (CrossPlatformInputManager.GetButtonDown("ShiftDown"))
			{
				ShiftGear(Up: false);
			}
		}
		bool flag = false;
		if (carController == null)
		{
			flag = true;
		}
		if (carController != null && carController.transmissionType == TransmissionType.AT)
		{
			flag = true;
		}
		if (flag)
		{
			if (RPM > gearUpRpm && !Skidding && TimeSinceGearSwitching > 0.5f && CarGrounded && Gear + 1 < MaxGear)
			{
				ShiftGear(Up: true);
			}
			if ((RPM < gearDownRpm || SkiddingReallyMuch || (throttle > 0.5f && RPM < underThrottleGearDownRpm)) && Gear > 0)
			{
				ShiftGear(Up: false);
			}
			if (carController != null && (carController.LowGear || MovingBack()) && Gear > 0)
			{
				ShiftGear(Up: false);
			}
		}
		TimeSinceGearSwitching += Time.deltaTime;
		if (gear != Gear)
		{
			if (TimeSinceGearSwitching > 1f && engineSoundProcessor != null)
			{
				engineSoundProcessor.GearShift();
			}
			TimeSinceGearSwitching = 0f;
		}
	}

	private void ShiftGear(bool Up)
	{
		switch (Up)
		{
		case true:
			if (ReverseGear)
			{
				ReverseGear = false;
				NeutralGear = true;
			}
			else if (NeutralGear)
			{
				NeutralGear = false;
			}
			else
			{
				Gear++;
			}
			break;
		case false:
			if (!NeutralGear)
			{
				if (Gear > 0)
				{
					Gear--;
				}
				else if (Gear == 0 && !ReverseGear)
				{
					NeutralGear = true;
				}
			}
			else if (!ReverseGear)
			{
				ReverseGear = true;
				NeutralGear = false;
			}
			break;
		}
		int currentGear = Gear + 1;
		if (NeutralGear)
		{
			currentGear = -1;
		}
		if (ReverseGear)
		{
			currentGear = -2;
		}
		if (carController != null)
		{
			CarUIControl.Instance.SetCurrentGear(currentGear);
		}
	}

	private void DoEngineSounds()
	{
		if (!(engineSoundProcessor == null))
		{
			float target = smoothThrottle;
			if (TimeSinceGearSwitching < 0.3f && !Skidding && FakeRPMTarget == 0f)
			{
				target = 0f;
			}
			engineSoundProcessor.RevLimiterAllowed = (Skidding || NeutralGear || !CarGrounded || FakeRPMTarget > 0f);
			engineSoundProcessor.RPM = RPM;
			engineSoundProcessor.load = Mathf.MoveTowards(engineSoundProcessor.load, target, Time.deltaTime * 50f);
			engineSoundProcessor.Turbo = Turbo;
		}
	}
}
