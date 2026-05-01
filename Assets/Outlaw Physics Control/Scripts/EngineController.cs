using AGS_MonsterTruckControl;
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

    private AGS_MTC_CarController _orcTruckController;

    public ORC_EngineSounds orcEngineSounds;

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

    public float[] Gears {
        get {
            if (_orcTruckController != null) {
                return _orcTruckController.GearRatios;
            }
            return GearsManager.DefaultGears;
        }
    }

    public int MaxGear {
        get {
            if (_orcTruckController != null) {
                return _orcTruckController.MaxGear;
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

    public void SetDiesel(bool isDiesel) {
        Diesel = isDiesel;
    }

    private bool MovingBack() {
        if (_orcTruckController == null) {
            return false;
        }
        for (int i = 0; i < _orcTruckController.wheels.Count; i++) {
            if (_orcTruckController.wheels[i].wc.rpm > 0f) {
                return false;
            }
        }
        return true;
    }

    private void Awake() {
        _orcTruckController = GetComponent<AGS_MTC_CarController>();
        if (ThrottleValve != null && ThrottleValve.t != null) {
            ThrottleValveDefaultRotation = ThrottleValve.t.localEulerAngles;
        }
    }

    private void Update() {
        if (orcEngineSounds == null) {
            LoadEngineSounds();
        }
        CalculateSpeed();
        SimulateEngine();
        DoGearShifting();
        DoEngineSounds();
        RotatePulleys();
    }

    private void OnDisable() {
        if (orcEngineSounds != null) {
            orcEngineSounds.enabled = false;
        }
    }

    private void OnEnable() {
        if (orcEngineSounds != null) {
            orcEngineSounds.enabled = true;
        }
    }

    private void LoadEngineSounds() {
        if (Diesel) {
            orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/DieselTruck"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            Turbo = true;
            orcEngineSounds.Turbo = true;
        }
        else if (_orcTruckController.vehicleType == AGS_MTC_VehicleType.Truck) {
            if (_orcTruckController.EngineBlockStage >= 2) {
                orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/GasTruckBigBlock"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            }
            else {
                orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/GasTruck"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            }
            Turbo = PurchasedTurbo;
            orcEngineSounds.Turbo = PurchasedTurbo;
        }
        else if (_orcTruckController.vehicleType == AGS_MTC_VehicleType.SideBySide) {
            orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/SideBySide"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            Turbo = false;
            orcEngineSounds.Turbo = false;
        }
        else if (_orcTruckController.vehicleType == AGS_MTC_VehicleType.Crawler) {
            orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/Crawler"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            Turbo = false;
            orcEngineSounds.Turbo = false;
        }
        else {
            orcEngineSounds = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Sounds/ATV"), base.transform.position, Quaternion.identity, base.transform)).GetComponent<ORC_EngineSounds>();
            Turbo = false;
            orcEngineSounds.Turbo = false;
        }
    }

    private void RotatePulleys() {
        if (pulleys != null) {
            Pulley[] array = pulleys;
            foreach (Pulley pulley in array) {
                pulley.t.Rotate(pulley.Rotation + pulley.Rotation * smoothThrottle);
            }
        }
        if (ThrottleValve != null && ThrottleValve.t != null) {
            ThrottleValve.t.localEulerAngles = Vector3.Lerp(ThrottleValveDefaultRotation, ThrottleValveDefaultRotation + ThrottleValve.Rotation, smoothThrottle);
        }
    }

    private void CalculateSpeed() {
        if (_orcTruckController != null) {
            Speed = Mathf.Abs(_orcTruckController.Speed);
            return;
        }
        Vector3 vector = base.transform.position - lastPos;
        float target = Vector3.ProjectOnPlane(vector, base.transform.up).magnitude / Time.deltaTime * 2.33f;
        Speed = Mathf.MoveTowards(Speed, target, Time.deltaTime * 20f);
        Speed = Mathf.Abs(Speed);
        lastPos = base.transform.position;
    }

    private void SimulateEngine() {
        throttle = ((!(_orcTruckController != null)) ? 0f : Mathf.Abs(_orcTruckController.Throttle));
        if (FakeRPMTarget > 1000f) {
            throttle = 1f;
        }
        smoothThrottle = Mathf.MoveTowards(smoothThrottle, throttle, Time.deltaTime * 10f);
        float num = 0f;
        float num2 = 0f;
        Skidding = false;
        SkiddingReallyMuch = false;
        CarGrounded = true;
        if (_orcTruckController == null) {
            num2 = minRpm + Speed * 5f * Gears[Gear] * TopGear;
        }
        else {
            yInput = UnityEngine.Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical");
            float num3 = throttleRpmBoost;
            float num4 = (!_orcTruckController.LowGear) ? 1f : _orcTruckController.LowGearRatio;
            CarGrounded = _orcTruckController.Grounded();
            if (!_orcTruckController.vehicleIsActive) {
                throttle = yInput;
                num3 = maxRpm - minRpm;
            }
            if (NeutralGear) {
                num3 = maxRpm - minRpm;
            }
            int num5 = 0;
            for (int i = 0; i < _orcTruckController.wheels.Count; i++) {
                if (_orcTruckController.wheels[i].wc.wheelCollider != null) {
                    _orcTruckController.wheels[i].wc.wheelCollider.rpmLimit = 275f / _orcTruckController.wheels[i].wc.wheelRadius / Gears[Gear] / TopGear / num4;
                    if (_orcTruckController.wheels[i].power) {
                        num += _orcTruckController.wheels[i].wc.rpm * _orcTruckController.wheels[i].wc.wheelRadius;
                        num5++;
                    }
                }
            }
            num /= (float)num5;
            num2 = minRpm + Mathf.Abs(num * 2f) * Gears[Gear] * TopGear * num4 + num3 * throttle;
            num2 = Mathf.Clamp(num2, minRpm, maxRpm);
            Skidding = (Mathf.Abs(num / 2.5f / Speed) > 2f && CarGrounded);
            if (_orcTruckController.wheels.Count == 2) {
                Skidding = false;
            }
            SkiddingReallyMuch = (Mathf.Abs(num / 2.5f / Speed) > 4f && CarGrounded);
        }
        if (FakeRPMTarget > 0f) {
            num2 = FakeRPMTarget;
        }
        float num6 = NeutralGear ? 40 : ((!(RPM < num2)) ? 5 : 25);
        RPM = Mathf.SmoothDamp(RPM, num2, ref revvingSpeed, Time.deltaTime * num6);
    }

    private void DoGearShifting() {
        if (FakeRPMTarget > 0f) {
            return;
        }
        int gear = Gear;
        if (_orcTruckController != null && _orcTruckController.transmissionType == AGS_MTC_TransmissionType.Manual) {
            if (CrossPlatformInputManager.GetButtonDown("ShiftUp") && Gear + 1 < MaxGear) {
                ShiftGear(Up: true);
            }
            if (CrossPlatformInputManager.GetButtonDown("ShiftDown")) {
                ShiftGear(Up: false);
            }
        }
        bool flag = false;
        if (_orcTruckController == null) {
            flag = true;
        }
        if (_orcTruckController != null && _orcTruckController.transmissionType == AGS_MTC_TransmissionType.AT) {
            flag = true;
        }
        if (flag) {
            if (RPM > gearUpRpm && !Skidding && TimeSinceGearSwitching > 0.5f && CarGrounded && Gear + 1 < MaxGear) {
                ShiftGear(Up: true);
            }
            if ((RPM < gearDownRpm || SkiddingReallyMuch || (throttle > 0.5f && RPM < underThrottleGearDownRpm)) && Gear > 0) {
                ShiftGear(Up: false);
            }
            if (_orcTruckController != null && (_orcTruckController.LowGear || MovingBack()) && Gear > 0) {
                ShiftGear(Up: false);
            }
        }
        TimeSinceGearSwitching += Time.deltaTime;
        if (gear != Gear) {
            if (TimeSinceGearSwitching > 1f && orcEngineSounds != null) {
                orcEngineSounds.GearShift();
            }
            TimeSinceGearSwitching = 0f;
        }
    }

    private void ShiftGear(bool Up) {
        switch (Up) {
            case true:
                if (ReverseGear) {
                    ReverseGear = false;
                    NeutralGear = true;
                }
                else if (NeutralGear) {
                    NeutralGear = false;
                }
                else {
                    Gear++;
                }
                break;
            case false:
                if (!NeutralGear) {
                    if (Gear > 0) {
                        Gear--;
                    }
                    else if (Gear == 0 && !ReverseGear) {
                        NeutralGear = true;
                    }
                }
                else if (!ReverseGear) {
                    ReverseGear = true;
                    NeutralGear = false;
                }
                break;
        }
        int currentGear = Gear + 1;
        if (NeutralGear) {
            currentGear = -1;
        }
        if (ReverseGear) {
            currentGear = -2;
        }
    }

    private void DoEngineSounds() {
        if (!(orcEngineSounds == null)) {
            float target = smoothThrottle;
            if (TimeSinceGearSwitching < 0.3f && !Skidding && FakeRPMTarget == 0f) {
                target = 0f;
            }
            orcEngineSounds.RevLimiterAllowed = (Skidding || NeutralGear || !CarGrounded || FakeRPMTarget > 0f);
            orcEngineSounds.RPM = RPM;
            orcEngineSounds.load = Mathf.MoveTowards(orcEngineSounds.load, target, Time.deltaTime * 50f);
            orcEngineSounds.Turbo = Turbo;
        }
    }
}
