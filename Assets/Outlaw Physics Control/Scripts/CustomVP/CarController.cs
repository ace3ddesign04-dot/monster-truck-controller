using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace CustomVP {
    public class CarController : MonoBehaviour {
        public bool vehicleIsActive;

        #region Side Self Right
        [Header("Side Self Right")]
        public bool sideSelfRightStuntActive = true;
        public float SideSelfRightMaxSpeed = 10f;
        public float SideSelfRightMinThrottle = 0.4f;
        public float SideSelfRightMinTilt = 0.75f;

        public float SideSelfRightPitchPhaseDuration = 1.0f;
        public float SideSelfRightPitchDirection = -1f;

        public float SideSelfRightPitchForce = 18f;
        public float SideSelfRightPitchDamping = 2f;
        public float SideSelfRightPitchMaxAssist = 16f;
        public float SideSelfRightPitchRollLockStrength = 24f;

        public float SideSelfRightRollAssist = 6f;
        public float SideSelfRightRollDamping = 2.5f;
        public float SideSelfRightMaxAssist = 10f;

        public float SideSelfRightRecoverWheelTorque = 220f;
        public float SideSelfRightRecoverBrakeMultiplier = 0.15f;

        [Range(0f, 1f)] public float SideSelfRightPitchPhaseWheelGrip = 0.05f;
        [Range(0f, 1f)] public float SideSelfRightAntirollMultiplier = 0.10f;

        public bool sideSelfRightActive;

        private float sideSelfRightTimer = 0f;
        private bool sideSelfRightLatched = false;
        private bool sideSelfRightLowGripApplied = false;
        private float sideSelfRightLockedRoll = 0f;

        private float GetCurrentLatTilt() {
            Vector3 flatRight = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            float latTilt = (transform.right - flatRight).y;

            if (transform.up.y < 0f)
                latTilt = -latTilt;

            return latTilt;
        }

        private bool CanStartSideSelfRight() {
            float currentLatTilt = GetCurrentLatTilt();

            if (!sideSelfRightStuntActive)
                return false;

            if (!TouchingGround && !Grounded())
                return false;

            if (Mathf.Abs(Speed) > SideSelfRightMaxSpeed)
                return false;

            if (Throttle < SideSelfRightMinThrottle)
                return false;

            if (Mathf.Abs(currentLatTilt) < SideSelfRightMinTilt)
                return false;

            return true;
        }

        private void SetSelfRightWheelGrip(float grip) {
            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc == null)
                    continue;

                wheels[i].wc.forwardFrictionCoefficient = grip;
                wheels[i].wc.sideFrictionCoefficient = grip;
                wheels[i].wc.UpdateFriction();
            }
        }

        private void RestoreSelfRightWheelGrip() {
            if (!sideSelfRightLowGripApplied)
                return;

            SetSelfRightWheelGrip(1f);
            sideSelfRightLowGripApplied = false;
        }
        private void DoSideSelfRight() {
            sideSelfRightActive = false;

            if (!sideSelfRightLatched) {
                if (!CanStartSideSelfRight()) {
                    sideSelfRightTimer = 0f;
                    RestoreSelfRightWheelGrip();
                    return;
                }

                sideSelfRightLatched = true;
                sideSelfRightTimer = 0f;
                sideSelfRightLockedRoll = Vector3.Dot(transform.right, Vector3.up);
            }

            if ((!TouchingGround && !Grounded()) ||
                Mathf.Abs(Speed) > SideSelfRightMaxSpeed ||
                Throttle < SideSelfRightMinThrottle) {
                sideSelfRightLatched = false;
                sideSelfRightTimer = 0f;
                RestoreSelfRightWheelGrip();
                return;
            }

            sideSelfRightActive = true;
            sideSelfRightTimer += Time.fixedDeltaTime;

            EnableRearSteer = false;
            EnableSideWheelieAssist = false;
            EnableSideWheelieCOMShift = false;

            sideWheelieCOMState = 0;
            sideWheelieReleaseTimer = 0f;
            sideWheelieIntentTimer = 0f;
            sideWheelieIntentDirection = 0;
            donutIntentTimer = 0f;

            float currentLatTilt = GetCurrentLatTilt();
            bool leftSideDown = currentLatTilt < 0f;
            bool pitchPhase = sideSelfRightTimer < SideSelfRightPitchPhaseDuration;

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float pitchRate = localAngularVelocity.x;
            float rollRate = localAngularVelocity.z;

            if (pitchPhase) {
                if (!sideSelfRightLowGripApplied) {
                    SetSelfRightWheelGrip(SideSelfRightPitchPhaseWheelGrip);
                    sideSelfRightLowGripApplied = true;
                }

                for (int i = 0; i < wheels.Count; i++) {
                    if (wheels[i].wc == null || wheels[i].wc.wheelCollider == null)
                        continue;

                    wheels[i].wc.MotorTorque = 0f;
                    wheels[i].wc.BrakeTorque = 0f;
                }

                float currentRoll = Vector3.Dot(transform.right, Vector3.up);

                float pitchAssist = SideSelfRightPitchDirection * SideSelfRightPitchForce - pitchRate * SideSelfRightPitchDamping;
                pitchAssist = Mathf.Clamp(pitchAssist, -SideSelfRightPitchMaxAssist, SideSelfRightPitchMaxAssist);

                float pitchT = Mathf.Clamp01(sideSelfRightTimer / SideSelfRightPitchPhaseDuration);

                // stay hard on the side first, then smoothly allow it to come onto two tires
                float unlockT = Mathf.InverseLerp(0.4f, 0.8f, pitchT);

                localAngularVelocity.z = Mathf.Lerp(0f, localAngularVelocity.z, unlockT);
                m_Rigidbody.angularVelocity = transform.TransformVector(localAngularVelocity);

                float rollLockAssist = (sideSelfRightLockedRoll - currentRoll) * SideSelfRightPitchRollLockStrength * (1f - unlockT);
                m_Rigidbody.AddRelativeTorque(pitchAssist, 0f, rollLockAssist, ForceMode.Acceleration);
                return;
            }

            RestoreSelfRightWheelGrip();

            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc == null || wheels[i].wc.wheelCollider == null)
                    continue;

                wheels[i].wc.MotorTorque = 0f;
                wheels[i].wc.BrakeTorque = 0f;

                bool isLeftWheel = (i == 0 || i == 2);
                bool isRecoverySideWheel = leftSideDown ? isLeftWheel : !isLeftWheel;

                if (!isRecoverySideWheel)
                    continue;

                wheels[i].wc.BrakeTorque = currentBrakeTorque * SideSelfRightRecoverBrakeMultiplier;
                wheels[i].wc.MotorTorque = SideSelfRightRecoverWheelTorque * Mathf.Sign(Throttle);
            }

            float currentRoll2 = Vector3.Dot(transform.right, Vector3.up);
            float rollAssist = (-currentRoll2) * SideSelfRightRollAssist
                               - rollRate * SideSelfRightRollDamping;
            rollAssist = Mathf.Clamp(
                rollAssist,
                -SideSelfRightMaxAssist,
                SideSelfRightMaxAssist
            );

            m_Rigidbody.AddRelativeTorque(0f, 0f, rollAssist, ForceMode.Acceleration);

            if (Mathf.Abs(currentRoll2) < 0.18f && Grounded()) {
                sideSelfRightLatched = false;
                sideSelfRightTimer = 0f;
                RestoreSelfRightWheelGrip();
            }
        }

        #endregion

        #region Donut Stunt
        [Header("Donut Assist")]
        public bool donutStuntActive = true;
        public float DonutMinSpeed = 4f;
        public float DonutMaxSpeed = 22f;
        public float DonutMinThrottle = 0.55f;
        public float DonutMinSteer = 0.55f;

        public float DonutTargetYawRate = 2.8f;
        public float DonutYawAssistForce = 18f;
        public float DonutYawDamping = 5f;
        public float DonutMaxAssist = 25f;

        public float DonutFrontSideGrip = 1.10f;
        public float DonutRearSideGrip = 0.55f;

        public float donutIntentTimer = 0;
        public float donutIntentTime = 3.0f;

        private bool UpdateDonutIntent() {
            if (!donutStuntActive || sideSelfRightActive) {
                return false;
            }

            if (Mathf.Abs(xInput) > 0.9f && Mathf.Abs(yInput) > 0.9f && !sideWheeliAssistEnabled) {
                donutIntentTimer += Time.deltaTime;
                if (donutIntentTimer > donutIntentTime) {
                    return true;
                }
            }
            else {
                donutIntentTimer = 0f;
            }
            return false;
        }

        private void ApplyDonutFriction(bool active) {
            if (!donutStuntActive || sideSelfRightActive)
                return;

            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc == null)
                    return;

                float targetSideGrip = 1f;

                if (active) {
                    targetSideGrip = (i <= 1) ? DonutFrontSideGrip : DonutRearSideGrip;
                }

                if (!Mathf.Approximately(wheels[i].wc.sideFrictionCoefficient, targetSideGrip)) {
                    wheels[i].wc.sideFrictionCoefficient = targetSideGrip;
                    wheels[i].wc.UpdateFriction();
                }
            }
        }

        private void DoDonutAssist() {
            if (!donutStuntActive || sideSelfRightActive)
                return;

            bool donutActive = UpdateDonutIntent();

            EnableSideWheelieAssist = !donutActive;
            EnableSideWheelieCOMShift = !donutActive;

            ApplyDonutFriction(donutActive);

            if (!donutActive)
                return;

            float steerDir = Mathf.Sign(xInput);

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float yawRate = localAngularVelocity.y;

            float targetYawRate = steerDir * DonutTargetYawRate;
            float yawError = targetYawRate - yawRate;

            float assist = yawError * DonutYawAssistForce - yawRate * DonutYawDamping;
            assist = Mathf.Clamp(assist, -DonutMaxAssist, DonutMaxAssist);

            m_Rigidbody.AddRelativeTorque(0f, assist, 0f, ForceMode.Acceleration);
        }
        #endregion

        #region Side Wheeling Stunt
        [Header("Side Wheelie COM Shift")]
        public bool sideWheeliStuntActive = true;
        public bool EnableSideWheelieCOMShift = true;
        public Transform leftWheeliCOM;
        public Transform rightWheeliCOM;

        //public float SideWheelieEnterCOMLerpSpeed = 8f;
        //public float SideWheelieExitCOMLerpSpeed = 5f;

        private int sideWheelieCOMState = 0; // -1 = left, 0 = base, 1 = right

        public float SideWheelieReleaseHoldTime = 1.2f;
        public float SideWheelieHoldMinTilt = 0.14f;
        public float SideWheelieHoldMinSpeed = 6f;
        public float SideWheelieHoldMinThrottle = 0.12f;
        private float sideWheelieReleaseTimer = 0f;


        [Header("Side Wheelie Intent")]
        public float SideWheelieIntentMinSpeed = 12f;
        public float SideWheelieIntentMinThrottle = 0.6f;
        public float SideWheelieIntentMinSteer = 0.8f;
        public float SideWheelieIntentHoldTime = 0.3f;

        private float sideWheelieIntentTimer = 0f;
        private int sideWheelieIntentDirection = 0; // -1 left, 1 right, 0 none

        [Header("Side Wheelie Assist")]
        public bool EnableSideWheelieAssist = true;
        public float SideWheelieTargetRoll = 0.35f;
        public float SideWheelieBalanceForce = 22f;
        public float SideWheelieDamping = 7f;
        public float SideWheelieMaxAssist = 20f;

        public bool sideWheeliAssistEnabled;

        private void UpdateSideWheelieIntent() {
            if (!sideWheeliStuntActive || sideSelfRightActive) {
                sideWheelieIntentTimer = 0f;
                sideWheelieIntentDirection = 0;
                return;
            }

            sideWheelieIntentDirection = 0;

            if (!Grounded()) {
                sideWheelieIntentTimer = 0f;
                return;
            }

            if (Mathf.Abs(Speed) < SideWheelieIntentMinSpeed || Throttle < SideWheelieIntentMinThrottle) {
                sideWheelieIntentTimer = 0f;
                return;
            }

            int desiredDirection = 0;

            if (xInput <= -SideWheelieIntentMinSteer)
                desiredDirection = 1;
            else if (xInput >= SideWheelieIntentMinSteer)
                desiredDirection = -1;

            if (desiredDirection == 0) {
                sideWheelieIntentTimer = 0f;
                return;
            }

            if (sideWheelieIntentDirection != desiredDirection) {
                sideWheelieIntentTimer += Time.fixedDeltaTime;
            }

            if (sideWheelieIntentTimer >= SideWheelieIntentHoldTime) {
                sideWheelieIntentDirection = desiredDirection;
            }
            else {
                sideWheelieIntentDirection = 0;
            }
        }
        private bool WantsToEnterLeftSideWheelie() {
            return sideWheelieIntentDirection == -1;
        }

        private bool WantsToEnterRightSideWheelie() {
            return sideWheelieIntentDirection == 1;
        }

        private bool IsStillInSideWheelie(int dir) {
            if (!sideWheeliStuntActive)
                return false;

            if (wheels.Count < 4)
                return false;

            if (!Grounded())
                return false;

            if (Mathf.Abs(Speed) < SideWheelieHoldMinSpeed)
                return false;

            if (Throttle < SideWheelieHoldMinThrottle)
                return false;

            bool leftGrounded = wheels[0].wc.IsGrounded || wheels[2].wc.IsGrounded;
            bool rightGrounded = wheels[1].wc.IsGrounded || wheels[3].wc.IsGrounded;

            float currentRoll = Vector3.Dot(transform.right, Vector3.up);

            if (dir == -1) {
                return (leftGrounded && !rightGrounded) || currentRoll > SideWheelieHoldMinTilt;
            }

            if (dir == 1) {
                return (rightGrounded && !leftGrounded) || currentRoll < -SideWheelieHoldMinTilt;
            }

            return false;
        }

        private void UpdateSideWheelieCOMState() {
            if (!sideWheeliStuntActive || !EnableSideWheelieCOMShift || !useManualCenterOfMass || comBase == null || leftWheeliCOM == null || rightWheeliCOM == null) {
                sideWheelieCOMState = 0;
                sideWheelieReleaseTimer = 0f;
                return;
            }

            if (sideWheelieCOMState == 0) {
                // Hard gate: do not enter side wheelie state below required entry speed/throttle
                if (Mathf.Abs(Speed) < SideWheelieIntentMinSpeed || Throttle < SideWheelieIntentMinThrottle) {
                    sideWheelieReleaseTimer = 0f;
                    return;
                }

                if (WantsToEnterLeftSideWheelie()) {
                    sideWheelieCOMState = -1;
                    sideWheelieReleaseTimer = SideWheelieReleaseHoldTime;
                    return;
                }

                if (WantsToEnterRightSideWheelie()) {
                    sideWheelieCOMState = 1;
                    sideWheelieReleaseTimer = SideWheelieReleaseHoldTime;
                    return;
                }

                return;
            }

            // Refresh timer while player still pushes the same side-wheelie direction
            if ((sideWheelieCOMState == -1 && WantsToEnterLeftSideWheelie()) ||
                (sideWheelieCOMState == 1 && WantsToEnterRightSideWheelie())) {
                sideWheelieReleaseTimer = SideWheelieReleaseHoldTime;
                return;
            }

            // Keep state alive while truck is still actually in the side wheelie
            if (IsStillInSideWheelie(sideWheelieCOMState)) {
                sideWheelieReleaseTimer = SideWheelieReleaseHoldTime;
                return;
            }

            // Grace period after releasing steering
            sideWheelieReleaseTimer -= Time.fixedDeltaTime;
            if (sideWheelieReleaseTimer > 0f)
                return;

            sideWheelieCOMState = 0;
            sideWheelieReleaseTimer = 0f;
        }

        private Vector3 GetTargetCenterOfMass() {
            Vector3 baseCOM = (comBase != null) ? comBase.localPosition : manualCenterOfMass;

            if (sideSelfRightActive)
                return baseCOM;

            if (!EnableSideWheelieCOMShift || !useManualCenterOfMass)
                return baseCOM;

            if (sideWheelieCOMState == -1 && leftWheeliCOM != null)
                return leftWheeliCOM.localPosition;

            if (sideWheelieCOMState == 1 && rightWheeliCOM != null)
                return rightWheeliCOM.localPosition;

            return baseCOM;
        }

        private void UpdateCenterOfMass() {
            if (!useManualCenterOfMass)
                return;

            if (comBase != null)
                manualCenterOfMass = comBase.localPosition;

            UpdateSideWheelieCOMState();

            Vector3 targetCOM = GetTargetCenterOfMass();
            SetCOM(Vector3.Lerp(m_Rigidbody.centerOfMass, targetCOM, Time.fixedDeltaTime * 8f));
        }

        private void DoSideWheelieAssist() {
            if (sideSelfRightActive) {
                sideWheeliAssistEnabled = false;
                return;
            }

            EnableRearSteer = true;

            if (!sideWheeliStuntActive || !EnableSideWheelieAssist || wheels.Count < 4) {
                sideWheeliAssistEnabled = false;
                return;
            }

            if (sideWheelieCOMState == 0) {
                sideWheeliAssistEnabled = false;
                return;
            }

            bool leftGrounded = wheels[0].wc.IsGrounded || wheels[2].wc.IsGrounded;
            bool rightGrounded = wheels[1].wc.IsGrounded || wheels[3].wc.IsGrounded;

            if (leftGrounded == rightGrounded) {
                sideWheeliAssistEnabled = false;
                return;
            }

            if (Mathf.Abs(Speed) < SideWheelieHoldMinSpeed) {
                sideWheeliAssistEnabled = false;
                return;
            }

            EnableRearSteer = false;
            float currentRoll = Vector3.Dot(transform.right, Vector3.up);
            float targetRoll = leftGrounded ? SideWheelieTargetRoll : -SideWheelieTargetRoll;

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float rollRate = localAngularVelocity.z;

            float error = targetRoll - currentRoll;
            float assist = error * SideWheelieBalanceForce - rollRate * SideWheelieDamping;
            assist = Mathf.Clamp(assist, -SideWheelieMaxAssist, SideWheelieMaxAssist);

            m_Rigidbody.AddRelativeTorque(0f, 0f, assist, ForceMode.Acceleration);

            sideWheeliAssistEnabled = true;
        }
        #endregion

        [HideInInspector] public Rigidbody m_Rigidbody;
        [Header("Mass / Center Of Mass")]
        public bool useManualCenterOfMass = false;
        public Vector3 manualCenterOfMass = Vector3.zero;
        public Transform comBase;

        private BodyPartsSwitcher bodyPartsSwitcher;

        private CarUIControl carUIControl;

        private EngineController engine;

        private IKDriverController Driver;

        [HideInInspector] public VehicleDataManager vehicleDataManager;

        private PhotonTransformView myTransformView;

        [HideInInspector] public TrailerController myTrailer;

        [Header("Setup")][SerializeField] public List<_Wheel> wheels;

        public Collider[] BodyColliders;

        public Transform SteeringWheel;

        public float SteeringWheelMaxAngle;

        public Transform Shadow;

        [Header("Camera settings")] public Transform FirstPersonPoint;

        public float FollowDistance = 4f;

        public float FollowYAngle = 20f;

        public float SideDistance = 4f;

        public float GarageMaxDistance = 6f;

        public float GarageMinDistance = 3f;

        [Header("Damaging")] public float CarHealth = 100f;

        public float MaximumHitDamageForce = 20000f;

        public float MaximumHitDamage = 10f;

        [Space(20f)] public Transform DamageWaterline;

        public float WaterDamage = 5f;

        [Space(20f)] public float OverheatDamage = 5f;

        public float MaxTemperature = 60f;

        public float DamageTemperature = 50f;

        public float LowGearTemperatureStep = 1f;

        public float DiffLockTemperatureStep = 1f;

        public float FullWDTemperatureStep = 0.5f;

        public float CoolingStep = 1f;

        private float DrivetrainTemperature;

        [Header("Differential locks")] public bool FrontDiffLock;

        public bool RearDiffLock;

        public bool InteraxleDiffLock;

        private float FrontDiffLockRatio = 1f;

        private float RearDiffLockRatio = 1f;

        private float InteraxleDiffLockRatio = 1f;

        [Space(10f)][Header("Handling")] public bool FWD;

        public bool RWD;

        public bool LowGear;

        public float maxSteeringAngle = 30f;

        public float BrakeTorque = 1000f;

        [HideInInspector] public float currentBrakeTorque;

        [Space(10f)] public float BaseTorque = 500f;

        public float BaseMaxSpeed = 70f;

        public float ModsMaxSpeedBoost = 1.3f;

        public float ModsAdditionalBoost = 1.1f;

        public AnimationCurve DynoCurve = new AnimationCurve(new Keyframe(0f, 0.2f), new Keyframe(1f, 1f));

        private float LeveledMaxTorque;

        private float LeveledMaxSpeed;

        [Space(10f)][Range(0f, 4f)] public int EngineBlockStage;

        [Range(0f, 4f)] public int HeadStage;

        [Range(0f, 4f)] public int ValvetrainStage;

        [Range(0f, 4f)] public int GripStage;

        [Range(0f, 4f)] public int WeightStage;

        [Range(0f, 4f)] public int DurabilityStage;

        [Range(0f, 4f)] public int GearingStage;

        [Range(0f, 4f)] public int TurboStage;

        [Range(0f, 4f)] public int BlowerStage;

        [Range(0f, 1f)] public int DieselStage = 3;

        public int PurchasedTurboStage;

        public int PurchasedBlowerStage;

        [HideInInspector] public bool ManualTransmissionPurchased;

        [HideInInspector] public bool DieselPurchased;

        [HideInInspector] public bool TankTracksPurchased;

        public TransmissionType transmissionType;

        public float[] GearRatios = GearsManager.DefaultGears;

        public float LowGearRatio = GearsManager.DefaultLowGear;

        public int MaxGear = 5;

        [Range(0f, 1f)] public int Ebrake;

        [Space(10f)][Header("Engine tuning")] public bool TuningEnginePurchased;

        public bool PerfectSetupPurchased;

        [Range(-10f, 10f)] public float FuelRatio;

        [Range(-10f, 10f)] public float TimingRatio;

        public float PerfectFuelRatio;

        public float PerfectTimingRatio;

        [Space(10f)]
        [Header("Stability")]
        [Range(0f, 10000f)] public float FrontLateralAntiroll = 10000f;
        [Range(0f, 10000f)] public float RearLateralAntiroll = 10000f;

        [Range(0f, 10000f)] public float LongitudinalAntiroll = 5000f;

        [Range(0f, 1f)] public float RollingResistance = 0.2f;

        [Range(0f, 1f)] public float SteerLimitOnSpeed = 0.5f;

        public bool PreventFromSideSliding = true;

        [Space(10f)][Range(0f, 50f)] public float SelfAlignForceX;

        [Range(0f, 50f)] public float SelfAlignForceZ;

        [Range(0f, 20f)] public float AlignSpeed = 5f;

        [Range(0f, 50f)] public float AirControlForce = 5f;

        public AnimationCurve AirForceCurve;

        [HideInInspector] public int WheelsOffTheGround;

        private Vector3 StartAngularVelocity;

        private float FlyingTime;

        public float LongTilt;

        public float LatTilt;

        private float AngleCounter;

        private Vector3 prevForward;

        private bool PassedVerticalState;

        private bool Passed90Degrees;

        private bool BackFlip;

        private bool TouchingGround;

        [Space(10f)][Header("Friction")] public float SurfaceManagerDataUpdateInterval = 1f;

        [HideInInspector] public int FrontInstalledTiresID;

        [HideInInspector] public int RearInstalledTiresID;

        private SurfaceManager surfaceManager;

        public FrictionSettings FrontFriction;

        public FrictionSettings RearFriction;

        public AnimationCurve frontSpringCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        public AnimationCurve rearSpringCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        private Vector3 lastVelocity;

        [HideInInspector] public Vector3 acceleration;

        [HideInInspector] public float Handbraking;

        [HideInInspector] public float Braking;

        [HideInInspector] public float ExtremeBraking;

        [HideInInspector] public float Throttle;

        [HideInInspector] public float Speed;

        [HideInInspector] public float AngularSpeed;

        [HideInInspector] public float Steering;

        [Header("Rear Steering")]
        public bool EnableRearSteer = true;
        [Range(0f, 1f)] public float RearSteerLowSpeedMultiplier = 0.35f;
        [Range(0f, 1f)] public float RearSteerHighSpeedMultiplier = 0.08f;
        public float RearSteerFadeOutSpeed = 40f;

        [HideInInspector] public float InverseSteerMultiplier;

        public float xInput;

        public float yInput;

        private float nextSurfaceManagerDataUpdateTime;

        [HideInInspector] public float FakeRPM;

        private bool HasSnorkel;

        private bool IsSlideThrottle;

        private Vector3 lowestPointOfCollider;

        [HideInInspector] public bool DontPreventFromSliding;

        [HideInInspector] public bool loadedOnOtherPlayerTrailer;

        [HideInInspector] public PhotonTransformView ownerOfTrailer;

        [HideInInspector] public PhotonTransformView ownerOfTrailerWeWantToLoadOn;

        private bool waitingForTrailerResponse;

        public float FinalTorquePercentage;

        public float CurrentTorque;

        public float AverageRPM {
            get {
                float num = 0f;
                for (int i = 0; i < wheels.Count; i++) {
                    num += wheels[i].wc.wheelCollider.perFrameRotation;
                }

                return num / wheels.Count;
            }
        }

        private void Start() {
            if (comBase != null) {
                manualCenterOfMass = comBase.localPosition;
            }

            carUIControl = FindObjectOfType<CarUIControl>();
            m_Rigidbody = GetComponent<Rigidbody>();
            bodyPartsSwitcher = GetComponent<BodyPartsSwitcher>();
            surfaceManager = FindObjectOfType<SurfaceManager>();
            engine = GetComponent<EngineController>();
            Driver = GetComponent<IKDriverController>();
            vehicleDataManager = GetComponent<VehicleDataManager>();
            myTransformView = GetComponent<PhotonTransformView>();
            lowestPointOfCollider = Vector3.zero;
            lowestPointOfCollider =
                BodyColliders[0].ClosestPoint(BodyColliders[0].transform.position - transform.up * 10f);
            if (BodyColliders.Length > 1) {
                for (int i = 0; i < BodyColliders.Length; i++) {
                    Vector3 vector = BodyColliders[i]
                        .ClosestPoint(BodyColliders[i].transform.position - transform.up * 10f);
                    if (vector.y < lowestPointOfCollider.y) {
                        lowestPointOfCollider = BodyColliders[i]
                            .ClosestPoint(BodyColliders[i].transform.position - transform.up * 10f);
                    }
                }
            }

            lowestPointOfCollider = transform.InverseTransformPoint(lowestPointOfCollider);
            SetCalculatedCOM();
            CarController[] array = FindObjectsOfType<CarController>();
            foreach (CarController carController in array) {
                if (carController != this && carController.enabled) {
                    enabled = false;
                    return;
                }
            }

            SetupFrictionValues();
            if (carUIControl != null) {
                int selectedPosition = (!LowGear) ? 1 : 0;
                carUIControl.SetupGearButton(selectedPosition);
                int selectedPosition2 = 0;
                if (RearDiffLock) {
                    selectedPosition2 = 1;
                }

                if (FrontDiffLock) {
                    selectedPosition2 = 2;
                }

                if (InteraxleDiffLock) {
                    selectedPosition2 = 3;
                }

                carUIControl.SetupDiffLockButton(selectedPosition2);
                int selectedPosition3 = 0;
                if (FWD) {
                    selectedPosition3 = 1;
                }

                if (FWD && RWD) {
                    selectedPosition3 = 2;
                }

                carUIControl.SetupDriveButton(selectedPosition3);
                if (vehicleDataManager.vehicleType == VehicleType.Bike) {
                    carUIControl.HideAllDrivetrainOptions();
                }
            }

            PartGroup partGroup = null;
            if (bodyPartsSwitcher != null && bodyPartsSwitcher.partGroups != null) {
                for (int k = 0; k < bodyPartsSwitcher.partGroups.Length; k++) {
                    if (bodyPartsSwitcher.partGroups[k].partType == PartType.Snorkel) {
                        partGroup = bodyPartsSwitcher.partGroups[k];
                        break;
                    }
                }

                if (partGroup != null && partGroup.InstalledPart > 0) {
                    HasSnorkel = true;
                }
            }

            if (wheels.Count > 2) {
                wheels[0].steer = (wheels[1].steer = true);

                wheels[2].steer = (wheels[3].steer = true);
                wheels[2].inverseSteer = (wheels[3].inverseSteer = true);
                wheels[2].handbrake = (wheels[3].handbrake = true);

                if (wheels.Count > 4) {
                    wheels[4].steer = (wheels[5].steer = true);
                    wheels[4].inverseSteer = (wheels[5].inverseSteer = true);
                    wheels[4].handbrake = (wheels[5].handbrake = true);
                }
            }

            if (wheels.Count == 2) {
                wheels[0].steer = true;
                wheels[1].handbrake = true;
            }

            OnValidate();
            if (engine != null) {
                engine.SetDiesel(DieselStage == 4);
                engine.PurchasedTurbo = (TurboStage > 0);
            }

            Collider[] bodyColliders = BodyColliders;
            foreach (Collider collider in bodyColliders) {
                collider.material = (PhysicMaterial)Resources.Load("Physics/TruckCollider");
                collider.gameObject.layer = 26;
            }

            IsSlideThrottle = DataStore.GetBool("SlideAccelerator");
            CarController[] array2 = FindObjectsOfType<CarController>();
            int num = 0;
            while (true) {
                if (num < array2.Length) {
                    CarController carController2 = array2[num];
                    if (carController2 != this && carController2.enabled) {
                        break;
                    }

                    num++;
                    continue;
                }

                return;
            }

            Debug.Log("There's another CarController found in the scene. " + name +
                      " is disabled. There must be only 1 CarController in a scene.");
            enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            Checkpoint component = other.GetComponent<Checkpoint>();
            if (!(component == null)) {
                if (GameState.GameType == GameType.TrailRace) {
                    TrailRaceManager.Instance.CollidedWithCheckpoint(component);
                }
                else {
                    RacingManager.Instance.CollidedWithCheckpoint(component);
                }
            }
        }

        private void OnDrawGizmos() {
            Color blue = Color.blue;
            blue.a = 0.3f;
            Gizmos.color = blue;
            if (DamageWaterline != null) {
                Gizmos.DrawCube(DamageWaterline.transform.position, new Vector3(3f, 0f, 3f));
            }
        }

        private void OnDisable() {
            foreach (_Wheel wheel in wheels) {
                wheel.wc.BrakeTorque = BrakeTorque;
                wheel.wc.MotorTorque = 0f;
            }
        }

        private void FixedUpdate() {
            Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
            Speed = localVelocity.z * 2.23f;
            AngularSpeed = m_Rigidbody.angularVelocity.magnitude;

            if (!vehicleIsActive) {
                foreach (_Wheel wheel in wheels) {
                    wheel.wc.MotorTorque = 0f;
                    wheel.wc.BrakeTorque = BrakeTorque;
                }

                acceleration = (m_Rigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
                acceleration = transform.InverseTransformVector(acceleration);
                lastVelocity = m_Rigidbody.velocity;
                return;
            }

            if (Time.time >= nextSurfaceManagerDataUpdateTime) {
                GetDataFromSurfaceManager();
            }

            DoCarHandling();
            DoSideSelfRight();

            UpdateSideWheelieIntent();
            UpdateCenterOfMass();

            if (PreventFromSideSliding) {
                PreventFromSideSlide();
            }

            DoAirForces();
            DoAntiroll();
            DoDonutAssist();
            DoSideWheelieAssist();

            acceleration = (m_Rigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
            acceleration = transform.InverseTransformVector(acceleration);
            lastVelocity = m_Rigidbody.velocity;
        }

        private void OnCollisionEnter(Collision collision) {
            TouchingGround = true;
            GotHit(collision);
        }

        private void OnCollisionStay(Collision collision) {
            TouchingGround = true;
        }

        private void OnCollisionExit(Collision collision) {
            TouchingGround = false;
        }

        private void SendLoadOnTrailerRequest(PhotonTransformView otherTView) {
            ownerOfTrailerWeWantToLoadOn = otherTView;
            waitingForTrailerResponse = true;
            carUIControl.waitingForLoadOnTrailerResponseWindow.SetActive(value: true);
            carUIControl.loadOnOtherPlayerTrailerButton.SetActive(value: false);
            carUIControl.ToggleCarExtras(Show: false);
            carUIControl.ToggleCarControls(Show: false);
            carUIControl.ToggleWinchControls(Show: false);
            vehicleIsActive = false;
            myTransformView.SendTraileringRequest(ownerOfTrailerWeWantToLoadOn.photonView);
        }

        public void OnLoadOnTrailerResponseDeclined(PhotonView sender) {
            if (ownerOfTrailerWeWantToLoadOn != null && sender.tView == ownerOfTrailerWeWantToLoadOn &&
                waitingForTrailerResponse) {
                CancelTrailerLoadWaiting();
            }
        }

        public void OnLoadOnTrailerResponseAccepted(PhotonView sender) {
            if (ownerOfTrailerWeWantToLoadOn != null && sender.tView == ownerOfTrailerWeWantToLoadOn) {
                LoadOnOtherTrailer(ownerOfTrailerWeWantToLoadOn);
            }

            if (waitingForTrailerResponse) {
                CancelTrailerLoadWaiting();
            }
        }

        private void CancelTrailerLoadWaiting() {
            ownerOfTrailerWeWantToLoadOn = null;
            waitingForTrailerResponse = false;
            carUIControl.waitingForLoadOnTrailerResponseWindow.SetActive(value: false);
            carUIControl.ToggleCarExtras(Show: true);
            carUIControl.ToggleCarControls(Show: true);
            carUIControl.ToggleWinchControls(Show: true);
            vehicleIsActive = true;
        }

        public void LoadOnOtherTrailer(PhotonTransformView trailerOwner) {
            vehicleDataManager.LoadOnTrailer(trailerOwner.trailer, turnToDummy: false);
            myTransformView.TellEveryoneImOnTrailer(trailerOwner.photonView.viewID);
            loadedOnOtherPlayerTrailer = true;
            m_Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        public void UnloadFromOtherTrailer() {
            ConfigurableJoint component = GetComponent<ConfigurableJoint>();
            if (component != null) {
                DestroyImmediate(component);
            }

            int pViewID = -1;
            if (ownerOfTrailer != null) {
                pViewID = ownerOfTrailer.photonView.viewID;
            }

            myTransformView.TellEveryoneImOuttaTrailer(pViewID);
            loadedOnOtherPlayerTrailer = false;
            m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void Update() {

            if (carUIControl != null) // && PhotonNetwork.inRoom)
            {
                carUIControl.loadOnOtherPlayerTrailerButton.SetActive(value: false);
                carUIControl.unloadFromOtherPlayerTrailerButton.SetActive(loadedOnOtherPlayerTrailer);

                if (vehicleDataManager.vehicleType != VehicleType.Bike && !loadedOnOtherPlayerTrailer &&
                    !waitingForTrailerResponse && !WinchManager.Instance.WinchMode &&
                    (myTrailer == null || (myTrailer != null && !myTrailer.connected))) {
                    PhotonView[] currentPlayerViews = MultiplayerManager.CurrentPlayerViews;
                    if (currentPlayerViews != null && currentPlayerViews.Length > 0) {
                        foreach (PhotonView photonView in currentPlayerViews) {
                            if (photonView != null && photonView.tView.trailer != null &&
                                photonView.tView.carOnTrailer == null && photonView.tView.trailer.mpCarOnMe == null &&
                                photonView.tView.trailer.mpConnected &&
                                Vector3.Distance(transform.position, photonView.tView.trailer.transform.position) < 8f) {
                                carUIControl.loadOnOtherPlayerTrailerButton.SetActive(value: true);
                                ownerOfTrailer = photonView.tView;
                            }
                        }
                    }
                }

                if (CrossPlatformInputManager.GetButtonUp("LoadOnOtherTrailer")) {
                    SendLoadOnTrailerRequest(ownerOfTrailer);
                }

                if (CrossPlatformInputManager.GetButtonUp("UnloadFromOtherTrailer")) {
                    UnloadFromOtherTrailer();
                }

                if (CrossPlatformInputManager.GetButtonUp("CancelTrailerLoadWaiting")) {
                    CancelTrailerLoadWaiting();
                }

                if (CrossPlatformInputManager.GetButtonUp("AcceptTrailering")) {
                    myTransformView.AcceptTraileringRequest();
                }

                if (CrossPlatformInputManager.GetButtonUp("DeclineTrailering")) {
                    myTransformView.DeclineTraierlingRequest();
                }

                if (loadedOnOtherPlayerTrailer && ownerOfTrailer == null) {
                    UnloadFromOtherTrailer();
                }

                if (waitingForTrailerResponse && ownerOfTrailerWeWantToLoadOn == null) {
                    CancelTrailerLoadWaiting();
                }
            }

            DoInput();

            if (InteraxleDiffLock && (!FrontDiffLock || !RearDiffLock)) {
                InteraxleDiffLock = false;
            }

            Debug.DrawRay(transform.TransformPoint(m_Rigidbody.centerOfMass), Vector3.up);
            WheelsOffTheGround = NotGroundedWheels();

            if (Shadow != null) {
                bool shouldShowShadow = QualitySettings.GetQualityLevel() <= 2;
                if (Shadow.gameObject.activeInHierarchy != shouldShowShadow) {
                    Shadow.gameObject.SetActive(shouldShowShadow);
                }

                if (shouldShowShadow) {
                    Vector3 downPoint = Shadow.position + Vector3.down;
                    Shadow.rotation = Quaternion.LookRotation(downPoint - Shadow.position, transform.forward);
                }
            }
        }

        public void OnValidate() {
            SetDiffLock();
            SetupFrictionValues();
            UpdateMotorPower();
            if (wheels.Count >= 4) {
                for (int i = 0; i < wheels.Count; i++) {
                    wheels[i].power = ((i <= 1) ? FWD : RWD);
                    if (wheels[i].wc != null && wheels[i].wc.wheelCollider != null) {
                        wheels[i].wc.wheelCollider.FakeRPM = FakeRPM;
                    }
                }
            }

            if (wheels.Count != 2) {
                return;
            }

            for (int j = 0; j < wheels.Count; j++) {
                wheels[j].power = (j == 1);
                if (wheels[j].wc != null && wheels[j].wc.wheelCollider != null) {
                    wheels[j].wc.wheelCollider.FakeRPM = FakeRPM;
                }
            }

            if (Application.isPlaying && m_Rigidbody != null && useManualCenterOfMass) {
                SetCOM(manualCenterOfMass);
            }
        }

        private void PreventFromSideSlide() {
            if (!DontPreventFromSliding && Mathf.Abs(m_Rigidbody.velocity.magnitude) < 0.5f && Throttle == 0f &&
                WheelsOffTheGround == 0 && !TouchingGround) {
                Vector3 velocity = m_Rigidbody.velocity;
                float x = velocity.x;
                Vector3 velocity2 = m_Rigidbody.velocity;
                Vector3 a = new Vector3(x, 0f, velocity2.z);
                m_Rigidbody.AddForce(-a * 100000f);
            }
        }

        public void FlipCar() {
            if (!loadedOnOtherPlayerTrailer) {
                transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                Utility.AlignVehicleByGround(transform);
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.angularVelocity = Vector3.zero;
                m_Rigidbody.isKinematic = true;
                if (myTrailer != null && myTrailer.connected) {
                    myTrailer.Detach();
                    myTrailer.Attach();
                    myTrailer.rb.isKinematic = true;
                }

                Invoke("UnfreezeCar", 0.5f);
                carUIControl.SwitchFlipButton(Show: false);
            }
        }

        private void RepairVehicle() {
            CarHealth = 100f;
        }

        public void RespawnCar() {
            if (!DontPreventFromSliding && !loadedOnOtherPlayerTrailer) {
                Transform availableSpawnPoint = VehicleLoader.Instance.GetAvailableSpawnPoint();
                transform.position = availableSpawnPoint.position;
                transform.rotation = availableSpawnPoint.rotation;
                Utility.AlignVehicleByGround(transform);
                if (RacingManager.Instance != null && RacingManager.Instance.IsPlayerBusy) {
                    RacingManager.Instance.CancelRace();
                }

                carUIControl.SwitchFlipButton(Show: false);
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.angularVelocity = Vector3.zero;
                m_Rigidbody.isKinematic = true;
                if (myTrailer != null) {
                    myTrailer.Detach();
                    myTrailer.Attach();
                    myTrailer.rb.isKinematic = true;
                }

                Invoke("UnfreezeCar", 0.5f);
            }
        }

        private void UnfreezeCar() {
            m_Rigidbody.isKinematic = false;
            if (myTrailer != null) {
                myTrailer.rb.isKinematic = false;
            }
        }

        public void SetCalculatedCOM() {
            if (m_Rigidbody == null) {
                m_Rigidbody = GetComponent<Rigidbody>();
            }

            if (useManualCenterOfMass) {
                SetCOM(manualCenterOfMass);
                return;
            }

            if (BodyColliders != null && BodyColliders.Length > 0) {
                SetCOM(CalculateCOMPosition());
            }
        }

        private void SetCOM(Vector3 comPos) {
            m_Rigidbody.centerOfMass = comPos;
        }

        private Vector3 CalculateCOMPosition() {
            if (BodyColliders == null) {
                return Vector3.zero;
            }

            if (BodyColliders.Length == 0) {
                return Vector3.zero;
            }

            Vector3 a = Vector3.zero;
            for (int i = 0; i < wheels.Count; i++) {
                a += wheels[i].wc.transform.position;
            }

            a /= wheels.Count;
            a = transform.InverseTransformPoint(a);
            return new Vector3(lowestPointOfCollider.x, lowestPointOfCollider.y, a.z);
        }

        private void DoAntiroll() {
            if (wheels.Count < 4)
                return;

            foreach (_Wheel wheel in wheels) {
                if (wheel.wc == null || wheel.wc.wheelCollider == null)
                    return;
            }

            float effectiveFrontLateralAntiroll = FrontLateralAntiroll;
            float effectiveRearLateralAntiroll = RearLateralAntiroll;
            float effectiveLongitudinalAntiroll = LongitudinalAntiroll;

            if (sideSelfRightActive) {
                if (sideSelfRightTimer < SideSelfRightPitchPhaseDuration)
                    return;

                effectiveFrontLateralAntiroll *= SideSelfRightAntirollMultiplier;
                effectiveRearLateralAntiroll *= SideSelfRightAntirollMultiplier;
                effectiveLongitudinalAntiroll *= SideSelfRightAntirollMultiplier;
            }

            float frontLeftCompression = wheels[0].wc.IsGrounded ? wheels[0].wc.Compression : 0f;
            float frontRightCompression = wheels[1].wc.IsGrounded ? wheels[1].wc.Compression : 0f;
            float frontAntirollForce = (frontLeftCompression - frontRightCompression) * effectiveFrontLateralAntiroll;

            if (wheels[0].wc.IsGrounded)
                m_Rigidbody.AddForceAtPosition(wheels[0].wc.transform.up * frontAntirollForce, wheels[0].wc.transform.position);

            if (wheels[1].wc.IsGrounded)
                m_Rigidbody.AddForceAtPosition(wheels[1].wc.transform.up * -frontAntirollForce, wheels[1].wc.transform.position);

            float rearLeftCompression = wheels[2].wc.IsGrounded ? wheels[2].wc.Compression : 0f;
            float rearRightCompression = wheels[3].wc.IsGrounded ? wheels[3].wc.Compression : 0f;
            float rearAntirollForce = (rearLeftCompression - rearRightCompression) * effectiveRearLateralAntiroll;

            if (wheels[2].wc.IsGrounded)
                m_Rigidbody.AddForceAtPosition(wheels[2].wc.transform.up * rearAntirollForce, wheels[2].wc.transform.position);

            if (wheels[3].wc.IsGrounded)
                m_Rigidbody.AddForceAtPosition(wheels[3].wc.transform.up * -rearAntirollForce, wheels[3].wc.transform.position);

            float frontAvgCompression = (frontLeftCompression + frontRightCompression) * 0.5f;
            float rearAvgCompression = (rearLeftCompression + rearRightCompression) * 0.5f;
            float longitudinalAntirollForce = (frontAvgCompression - rearAvgCompression) * effectiveLongitudinalAntiroll;

            Vector3 frontAxleCenter = (wheels[0].wc.transform.position + wheels[1].wc.transform.position) * 0.5f;
            Vector3 rearAxleCenter = (wheels[2].wc.transform.position + wheels[3].wc.transform.position) * 0.5f;

            float uprightFactor = Mathf.InverseLerp(0.75f, 1f, transform.up.y);

            m_Rigidbody.AddForceAtPosition(transform.up * longitudinalAntirollForce * uprightFactor, frontAxleCenter);
            m_Rigidbody.AddForceAtPosition(-transform.up * longitudinalAntirollForce * uprightFactor, rearAxleCenter);
        }

        private int NotGroundedWheels() {
            int num = 0;
            for (int i = 0; i < wheels.Count; i++) {
                if (!wheels[i].wc.IsGrounded) {
                    num++;
                }
            }

            return num;
        }

        public bool Grounded() {
            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc.IsGrounded) {
                    return true;
                }
            }

            return false;
        }

        private void DoAirForces() {
            bool flag = !Grounded();
            if (flag) {
                FlyingTime += Time.fixedDeltaTime;
            }
            else {
                StartAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
                FlyingTime = 0f;
            }

            if (TouchingGround) {
                StartAngularVelocity = Vector3.zero;
            }

            Vector3 vector = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            Vector3 vector2 = transform.forward - vector;
            LongTilt = vector2.y;
            Vector3 b = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            Vector3 vector3 = transform.right - b;
            LatTilt = vector3.y;
            Vector3 up = transform.up;
            if (up.y < 0f) {
                LatTilt = 0f - LatTilt;
            }

            float num = (!flag) ? 0f : AirForceCurve.Evaluate(FlyingTime);
            float x = StartAngularVelocity.x;
            float y = (AirControlForce == 0f || TouchingGround)
                ? StartAngularVelocity.y
                : (AirControlForce * xInput * num / 10f);
            float num2 = 0f - StartAngularVelocity.z;
            if (TouchingGround && yInput == 0f) {
                Vector3 vector4 = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
                x = vector4.x;
                Vector3 vector5 = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
                num2 = vector5.z;
            }

            Vector3 vector6 = new Vector3(x - AirControlForce * yInput * num / 10f, y, 0f - num2);
            Vector3 target = transform.TransformVector(vector6);
            if ((SelfAlignForceX > 0f || SelfAlignForceZ > 0f || AirControlForce > 0f) && flag) {
                m_Rigidbody.angularVelocity = Vector3.MoveTowards(m_Rigidbody.angularVelocity, target,
                    Time.fixedDeltaTime * AlignSpeed);
            }

            bool flag2 = false;
            if (Driver != null) {
                flag2 = Driver.KnockedOut;
            }

            if (flag && !flag2) {
                Vector3 up2 = transform.up;
                if (up2.y < 0f && !Passed90Degrees) {
                    Passed90Degrees = true;
                }

                BackFlip = (vector6.x < 0f);
                if (Vector3.Angle(Vector3.up, -transform.up) < 5f) {
                    PassedVerticalState = true;
                }

                if (Passed90Degrees && PassedVerticalState) {
                    Vector3 up3 = transform.up;
                    if (up3.y > 0f) {
                        Passed90Degrees = false;
                        PassedVerticalState = false;
                        carUIControl.ShowNotification((!BackFlip) ? "Frontflip!" : "Backflip!", blinking: false);
                        BackFlip = false;
                        AngleCounter = 0f;
                        prevForward = Vector3.zero;
                    }
                }

                if (Passed90Degrees || PassedVerticalState) {
                    AngleCounter = 0f;
                }
            }
            else {
                Passed90Degrees = false;
                PassedVerticalState = false;
                BackFlip = false;
            }

            if (flag && !flag2) {
                if (prevForward == Vector3.zero) {
                    prevForward = vector;
                }

                AngleCounter += Vector3.Angle(vector, prevForward) * Mathf.Sign(vector6.y);
                prevForward = vector;
                if (AngleCounter > 320f || AngleCounter < -320f) {
                    AngleCounter = 0f;
                    carUIControl.ShowNotification("Roll over!", blinking: false);
                }
            }
            else {
                AngleCounter = 0f;
                prevForward = Vector3.zero;
            }
        }

        private void UpdateFriction() {
            for (int i = 0; i < wheels.Count; i++) {
                float num = (100f + PowerParts
                    .GetPart(GetComponent<VehicleDataManager>().vehicleType, PowerPartType.Grip, GripStage)
                    .IncrementPercantage) / 100f;
                wheels[i].wc.surfaceFrictionCoefficient =
                    surfaceManager.GetTireFriction(i, (i <= 1) ? FrontInstalledTiresID : RearInstalledTiresID) * num;
                wheels[i].wc.UpdateFriction();
            }
        }

        public void SetZeroFriction() {
            foreach (_Wheel wheel in wheels) {
                wheel.wc.forwardFrictionCoefficient =
                    (wheel.wc.sideFrictionCoefficient = (wheel.wc.surfaceFrictionCoefficient = 0f));
                wheel.wc.UpdateFriction();
            }
        }

        public void SetDefaultFriction() {
            foreach (_Wheel wheel in wheels) {
                wheel.wc.forwardFrictionCoefficient =
                    (wheel.wc.sideFrictionCoefficient = (wheel.wc.surfaceFrictionCoefficient = 1f));
                wheel.wc.UpdateFriction();
            }
        }

        public void UpdateEngineModel() {
            EngineType engineType = EngineType.Stock;
            if (BlowerStage > 0) {
                engineType = EngineType.Blower;
            }

            if (TurboStage > 0 || DieselStage == 4) {
                engineType = EngineType.Turbo;
            }

            GetComponent<BodyPartsSwitcher>().UpdateEngineModel(engineType);
        }

        public float GetMaxTorque() {
            if (vehicleDataManager == null) {
                return 0f;
            }

            PowerPart part = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.EngineBlock,
                EngineBlockStage);
            PowerPart part2 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Head, HeadStage);
            PowerPart part3 =
                PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Valvetrain, ValvetrainStage);
            PowerPart part4 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Turbo, TurboStage);
            PowerPart part5 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Blower, BlowerStage);
            float num = 0f;
            if (part != null) {
                num += part.IncrementPercantage;
            }

            if (part2 != null) {
                num += part2.IncrementPercantage;
            }

            if (part3 != null) {
                num += part3.IncrementPercantage;
            }

            if (part4 != null) {
                num += part4.IncrementPercantage;
            }

            if (part5 != null) {
                num += part5.IncrementPercantage;
            }

            float f = PerfectFuelRatio - FuelRatio;
            float num2 = Mathf.Lerp(5f, -15f, Mathf.Abs(f) / 10f);
            num += num2;
            float f2 = PerfectTimingRatio - TimingRatio;
            float num3 = Mathf.Lerp(5f, -15f, Mathf.Abs(f2) / 10f);
            num += num3;
            float num4 = ModsAdditionalBoost;
            if (num < 0f) {
                num4 = 1f;
            }

            return BaseTorque / 100f * (100f + num * num4);
        }

        private void UpdateMotorPower() {
            if (!(vehicleDataManager == null)) {
                PowerPart part = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.EngineBlock,
                    EngineBlockStage);
                PowerPart part2 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Head, HeadStage);
                PowerPart part3 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Valvetrain,
                    ValvetrainStage);
                PowerPart part4 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Weight, WeightStage);
                PowerPart part5 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Weight,
                    DurabilityStage);
                PowerPart part6 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Turbo, TurboStage);
                PowerPart part7 = PowerParts.GetPart(vehicleDataManager.vehicleType, PowerPartType.Blower, BlowerStage);
                FinalTorquePercentage = 0f;
                if (part != null) {
                    FinalTorquePercentage += part.IncrementPercantage;
                }

                if (part2 != null) {
                    FinalTorquePercentage += part2.IncrementPercantage;
                }

                if (part3 != null) {
                    FinalTorquePercentage += part3.IncrementPercantage;
                }

                if (part4 != null) {
                    FinalTorquePercentage += part4.IncrementPercantage;
                }

                if (part6 != null) {
                    FinalTorquePercentage += part6.IncrementPercantage;
                }

                if (part7 != null) {
                    FinalTorquePercentage += part7.IncrementPercantage;
                }

                float f = PerfectFuelRatio - FuelRatio;
                float num = Mathf.Lerp(5f, -15f, Mathf.Abs(f) / 10f);
                FinalTorquePercentage += num;
                float f2 = PerfectTimingRatio - TimingRatio;
                float num2 = Mathf.Lerp(5f, -15f, Mathf.Abs(f2) / 10f);
                FinalTorquePercentage += num2;
                float num3 = Mathf.Lerp(0.5f, 1f, CarHealth / 100f);
                float num4 = ModsAdditionalBoost;
                float num5 = ModsMaxSpeedBoost;
                if (FinalTorquePercentage < 0f) {
                    num4 = 1f;
                    num5 = 1f;
                }

                float num6 = BaseTorque / 100f * (100f + FinalTorquePercentage);
                LeveledMaxTorque = BaseTorque * num3 / 100f * (100f + FinalTorquePercentage * num4);
                LeveledMaxSpeed = BaseMaxSpeed * num3 / 100f * (100f + FinalTorquePercentage * num4 * num5);
                //engine.TopGear = 9f - 4f * ((num6 - 80f) / 280f);
                engine.TopGear = Mathf.Max(0.5f, 9f - 4f * ((num6 - 80f) / 280f));
            }
        }

        private void SetupCounterWheels() {
            if (wheels.Count >= 4) {
                wheels[0].wc.wheelCollider.OppositeWheel = wheels[1].wc.wheelCollider;
                wheels[1].wc.wheelCollider.OppositeWheel = wheels[0].wc.wheelCollider;
                wheels[2].wc.wheelCollider.OppositeWheel = wheels[3].wc.wheelCollider;
                wheels[3].wc.wheelCollider.OppositeWheel = wheels[2].wc.wheelCollider;
                wheels[0].wc.wheelCollider.AnotherAxleWheelL =
                    (wheels[1].wc.wheelCollider.AnotherAxleWheelL = wheels[2].wc.wheelCollider);
                wheels[0].wc.wheelCollider.AnotherAxleWheelR =
                    (wheels[1].wc.wheelCollider.AnotherAxleWheelR = wheels[3].wc.wheelCollider);
                wheels[2].wc.wheelCollider.AnotherAxleWheelL =
                    (wheels[3].wc.wheelCollider.AnotherAxleWheelL = wheels[0].wc.wheelCollider);
                wheels[2].wc.wheelCollider.AnotherAxleWheelR =
                    (wheels[3].wc.wheelCollider.AnotherAxleWheelR = wheels[1].wc.wheelCollider);
                if (wheels.Count > 4) {
                    wheels[4].wc.wheelCollider.OppositeWheel = wheels[5].wc.wheelCollider;
                    wheels[5].wc.wheelCollider.OppositeWheel = wheels[4].wc.wheelCollider;
                }
            }
        }

        private void SetupFrictionValues() {
            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc != null) {
                    wheels[i].wc.f_extSlip = ((i <= 1) ? FrontFriction.f_ExtremumSlip : RearFriction.f_ExtremumSlip);
                    wheels[i].wc.f_extVal = ((i <= 1) ? FrontFriction.f_ExtremumValue : RearFriction.f_ExtremumValue);
                    wheels[i].wc.f_asSlip = ((i <= 1) ? FrontFriction.f_AsymptoteSlip : RearFriction.f_AsymptoteSlip);
                    wheels[i].wc.f_asVal = ((i <= 1) ? FrontFriction.f_AsymptoteValue : RearFriction.f_AsymptoteValue);
                    wheels[i].wc.f_tailVal = ((i <= 1) ? FrontFriction.f_TailValue : RearFriction.f_TailValue);
                    wheels[i].wc.s_extSlip = ((i <= 1) ? FrontFriction.s_ExtremumSlip : RearFriction.s_ExtremumSlip);
                    wheels[i].wc.s_extVal = ((i <= 1) ? FrontFriction.s_ExtremumValue : RearFriction.s_ExtremumValue);
                    wheels[i].wc.s_asSlip = ((i <= 1) ? FrontFriction.s_AsymptoteSlip : RearFriction.s_AsymptoteSlip);
                    wheels[i].wc.s_asVal = ((i <= 1) ? FrontFriction.s_AsymptoteValue : RearFriction.s_AsymptoteValue);
                    wheels[i].wc.s_tailVal = ((i <= 1) ? FrontFriction.s_TailValue : RearFriction.s_TailValue);
                    wheels[i].wc.SpringCurve = ((i <= 1) ? frontSpringCurve : rearSpringCurve);
                }
            }
        }

        private void GetDataFromSurfaceManager() {
            if (carUIControl != null) {
                CarUIControl obj = carUIControl;
                Vector3 up = transform.up;
                obj.SwitchFlipButton(up.y < 0f && Mathf.Abs(Speed) < 2f);
            }

            nextSurfaceManagerDataUpdateTime = Time.time + SurfaceManagerDataUpdateInterval;
            UpdateMotorPower();
            CheckOverheating();
            if (surfaceManager != null) {
                CheckWaterDamage();
                UpdateFriction();
            }
        }

        private void SetDiffLock() {
            foreach (_Wheel wheel in wheels) {
                if (wheel.wc.wheelCollider == null) {
                    return;
                }
            }

            for (int i = 0; i < wheels.Count; i++) {
                wheels[i].wc.wheelCollider.DiffLock = ((i <= 1) ? FrontDiffLock : RearDiffLock);
                wheels[i].wc.wheelCollider.InteraxleDifLock = InteraxleDiffLock;
                wheels[i].wc.wheelCollider.DiffLockRatio = ((i <= 1) ? FrontDiffLockRatio : RearDiffLockRatio);
                wheels[i].wc.wheelCollider.InteraxleDiffLockRatio = InteraxleDiffLockRatio;
            }
        }

        private void SetDiffLock(int TypeID) {
            RearDiffLock = (FrontDiffLock = (InteraxleDiffLock = false));
            if (TypeID > 0) {
                RearDiffLock = true;
            }

            if (TypeID > 1) {
                FrontDiffLock = true;
            }

            if (TypeID > 2) {
                InteraxleDiffLock = true;
            }

            OnValidate();
        }

        private void SetDrive(int TypeID) {
            FWD = (RWD = false);
            if (TypeID == 0) {
                RWD = true;
            }

            if (TypeID == 1) {
                FWD = true;
            }

            if (TypeID == 2) {
                FWD = true;
                RWD = true;
            }

            OnValidate();
        }

        public void SteerTowards(Vector3 pos) {
            Vector3 vector = transform.InverseTransformDirection(pos - transform.position);
            float num = (0f - Mathf.Atan2(0f - vector.x, vector.z)) * 57.29578f;
            xInput = Mathf.Clamp(num / maxSteeringAngle, -1f, 1f);
        }

        private void DoInput() {
            xInput = Input.GetAxis("Horizontal") + CrossPlatformInputManager.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical");

            Debug.Log("xInput " + xInput);
            Debug.Log("yInput " + yInput);

            // if (!Application.isEditor)
            //     if (Input.touchCount == 0)
            //     {
            //         yInput = 0f;
            //     }

            if (CrossPlatformInputManager.GetButtonDown("SetDiffLock0")) {
                SetDiffLock(0);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDiffLock1")) {
                SetDiffLock(1);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDiffLock2")) {
                SetDiffLock(2);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDiffLock3")) {
                SetDiffLock(3);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetLowGear")) {
                LowGear = true;
            }

            if (CrossPlatformInputManager.GetButtonDown("SetHighGear")) {
                LowGear = false;
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDrive0")) {
                SetDrive(0);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDrive1")) {
                SetDrive(1);
            }

            if (CrossPlatformInputManager.GetButtonDown("SetDrive2")) {
                SetDrive(2);
            }

            if (CrossPlatformInputManager.GetButtonDown("Repair")) {
                RepairVehicle();
            }
        }

        private void DoCarHandling() {
            Handbraking = (CrossPlatformInputManager.GetButton("Ebrake") ? 1 : 0);
            float target = 0f;
            if (Speed > 1f || transmissionType == TransmissionType.Manual) {
                target = 0f - Mathf.Clamp(yInput, -1f, 0f);
            }

            Braking = Mathf.MoveTowards(Braking, target, Time.fixedDeltaTime * 50f);
            Braking = Mathf.Max(Braking, ExtremeBraking);
            float num = yInput;
            if ((Speed > 1f && Grounded()) || transmissionType == TransmissionType.Manual) {
                num = Mathf.Clamp(yInput, 0f, 1f);
            }

            if (transmissionType == TransmissionType.Manual && engine.ReverseGear) {
                num = 0f - num;
            }

            Throttle = num;
            float leveledMaxSpeed = LeveledMaxSpeed;
            float rPM = engine.RPM;
            float maxRpm = engine.maxRpm;
            float num2 = engine.Gears[engine.Gear];
            float topGear = engine.TopGear;
            float num3 = (!LowGear) ? 1f : LowGearRatio;
            //float num4 = Mathf.Clamp01(1f - Speed / leveledMaxSpeed) / 2f;
            float num4 = Mathf.Clamp01(1f - Speed / leveledMaxSpeed);
            float num5 = (FWD && RWD) ? 1 : 2;
            CurrentTorque = LeveledMaxTorque * DynoCurve.Evaluate(rPM / maxRpm) * num2 * topGear * num3 * num4 * num5;
            if (float.IsNaN(CurrentTorque)) {
                CurrentTorque = 0f;
            }

            if (Throttle == 0f) {
                CurrentTorque = 0f;
            }

            float target2 = Mathf.Lerp(maxSteeringAngle * 0.1f * xInput, maxSteeringAngle * xInput,
                1f - Speed / leveledMaxSpeed * SteerLimitOnSpeed);
            Steering = Mathf.MoveTowards(Steering, target2, Time.fixedDeltaTime * 100f);

            float rearSteerT = Mathf.Clamp01(Mathf.Abs(Speed) / RearSteerFadeOutSpeed);
            InverseSteerMultiplier = EnableRearSteer ? Mathf.Lerp(RearSteerLowSpeedMultiplier, RearSteerHighSpeedMultiplier, rearSteerT) : 0f;

            if (SteeringWheel != null) {
                SteeringWheel.localEulerAngles = new Vector3(0f, 0f,
                    Mathf.LerpUnclamped(SteeringWheelMaxAngle, 0f, Steering / maxSteeringAngle + 1f));
            }

            if (engine != null && engine.NeutralGear) {
                CurrentTorque = 0f;
            }

            currentBrakeTorque = BrakeTorque * Braking;
            foreach (_Wheel wheel in wheels) {
                if (wheel.wc.wheelCollider == null) {
                    break;
                }

                if (wheel.wc.wheelCollider.OppositeWheel == null) {
                    SetupCounterWheels();
                }

                wheel.wc.MotorTorque = ((!wheel.power) ? 0f : (CurrentTorque * Throttle));
                if (wheel.steer) {
                    wheel.wc.Steer = Steering;
                }

                if (wheel.inverseSteer) {
                    wheel.wc.Steer = (0f - Steering) * InverseSteerMultiplier;
                }

                wheel.wc.BrakeTorque = currentBrakeTorque;
                if (wheel.handbrake) {
                    wheel.wc.BrakeTorque = BrakeTorque * Mathf.Max(Handbraking * 3f, Braking);
                }

                if (CurrentTorque * Throttle == 0f && Braking == 0f && Handbraking == 0f && ExtremeBraking == 0f) {
                    wheel.wc.BrakeTorque = BrakeTorque / 2f * RollingResistance;
                }
            }
        }

        private void GotHit(Collision col) {
            bool flag = false;
            ContactPoint[] contacts = col.contacts;
            for (int i = 0; i < contacts.Length; i++) {
                ContactPoint contactPoint = contacts[i];
                Collider[] bodyColliders = BodyColliders;
                foreach (Collider obj in bodyColliders) {
                    if (contactPoint.thisCollider.Equals(obj)) {
                        flag = true;
                        break;
                    }
                }
            }

            if (Vector3.Angle(transform.up, col.impulse) < 20f) {
                flag = false;
            }

            if (flag && !(col.impulse.magnitude < 100f) && !(col.gameObject.GetPhotonView() != null)) {
                float num = Mathf.InverseLerp(0f, MaximumHitDamageForce, col.impulse.magnitude);
                float num2 = MaximumHitDamage * num;
                num2 *= 1f - DurabilityStage * 0.01f;
                if (GameState.GameMode == GameMode.Multiplayer || GameState.SceneName == "StuntPark") {
                    num2 *= 0.5f;
                }

                CarHealth = Mathf.Clamp(CarHealth - num2, 0f, 100f);
            }
        }

        private void CheckWaterDamage() {
            if (surfaceManager.IsCarInWater() && !HasSnorkel) {
                Vector3 position = surfaceManager.WaterMeshes[surfaceManager.WhatWaterMeshIsCarOn()].transform.position;
                Vector3 position2 = DamageWaterline.position;
                if (position2.y < position.y) {
                    DoWaterDamage(WaterDamage);
                }
            }
        }

        private void DoWaterDamage(float Value) {
            CarHealth = Mathf.Clamp(CarHealth - Value, 0f, 100f);
            carUIControl.ShowNotification("Water damage!", blinking: false);
            CameraController.Instance.Shake();
        }

        private void CheckOverheating() {
            float num = 0f;
            if (Mathf.Abs(Speed) > 1f) {
                num += ((!FWD || !RWD) ? 0f : FullWDTemperatureStep);
                num += ((!FrontDiffLock && !RearDiffLock) ? 0f : DiffLockTemperatureStep);
                num += ((!LowGear) ? 0f : LowGearTemperatureStep);
            }

            num -= CoolingStep + CoolingStep * (1f - DurabilityStage * 0.01f);
            DrivetrainTemperature = Mathf.Clamp(DrivetrainTemperature + num, 0f, MaxTemperature);
            if (DrivetrainTemperature > DamageTemperature) {
                DoOverheatDamage(OverheatDamage);
            }

            float temperatureRatio = Mathf.InverseLerp(0f, DamageTemperature, DrivetrainTemperature);
            if (carUIControl != null) {
                carUIControl.UpdateThermometer(temperatureRatio);
            }
        }

        private void DoOverheatDamage(float Value) {
            CarHealth = Mathf.Clamp(CarHealth - Value, 0f, 100f);
            carUIControl.ShowNotification("Overheating!", blinking: false);
            CameraController.Instance.Shake();
        }

        private CarControllerData GetCarControllerData() {
            CarControllerData carControllerData = new CarControllerData();
            carControllerData.CarHealth = CarHealth;
            carControllerData.EngineBlockStage = EngineBlockStage;
            carControllerData.GripStage = GripStage;
            carControllerData.HeadStage = HeadStage;
            carControllerData.ValvetrainStage = ValvetrainStage;
            carControllerData.WeightStage = WeightStage;
            carControllerData.DurabilityStage = DurabilityStage;
            carControllerData.TurboStage = TurboStage;
            carControllerData.BlowerStage = BlowerStage;
            carControllerData.GearingStage = GearingStage;
            carControllerData.DieselStage = DieselStage;
            carControllerData.TransmissionType = (int)transmissionType;
            carControllerData.ManualTransmissionPurchased = ManualTransmissionPurchased;
            carControllerData.DieselPurchased = DieselPurchased;
            carControllerData.PurchasedBlowerStage = PurchasedBlowerStage;
            carControllerData.PurchasedTurboStage = PurchasedTurboStage;
            carControllerData.TankTracksPurchased = TankTracksPurchased;
            carControllerData.TuningEnginePurchased = TuningEnginePurchased;
            carControllerData.PerfectSetupPurchased = PerfectSetupPurchased;
            carControllerData.GearRatios = GearRatios;
            carControllerData.LowGearRatio = LowGearRatio;
            carControllerData.Ebrake = Ebrake;
            carControllerData.FuelRatio = FuelRatio;
            carControllerData.TimingRatio = TimingRatio;
            carControllerData.PerfectFuelRatio = PerfectFuelRatio;
            carControllerData.PerfectTimingRatio = PerfectTimingRatio;
            return carControllerData;
        }

        public void SetCarControllerData(CarControllerData cData) {
            CarHealth = cData.CarHealth;
            EngineBlockStage = cData.EngineBlockStage;
            GripStage = cData.GripStage;
            HeadStage = cData.HeadStage;
            ValvetrainStage = cData.ValvetrainStage;
            WeightStage = cData.WeightStage;
            DurabilityStage = cData.DurabilityStage;
            TurboStage = cData.TurboStage;
            BlowerStage = cData.BlowerStage;
            GearingStage = cData.GearingStage;
            DieselStage = cData.DieselStage;
            transmissionType = (TransmissionType)cData.TransmissionType;
            ManualTransmissionPurchased = cData.ManualTransmissionPurchased;
            PurchasedTurboStage = cData.PurchasedTurboStage;
            PurchasedBlowerStage = cData.PurchasedBlowerStage;
            DieselPurchased = cData.DieselPurchased;
            TankTracksPurchased = cData.TankTracksPurchased;
            TuningEnginePurchased = cData.TuningEnginePurchased;
            PerfectSetupPurchased = cData.PerfectSetupPurchased;
            FuelRatio = cData.FuelRatio;
            TimingRatio = cData.TimingRatio;
            PerfectFuelRatio = cData.PerfectFuelRatio;
            PerfectTimingRatio = cData.PerfectTimingRatio;
            if (cData.GearRatios != null && cData.GearRatios.Length == 5) {
                GearRatios = cData.GearRatios;
                LowGearRatio = cData.LowGearRatio;
            }

            if (PerfectFuelRatio == 0f) {
                PerfectFuelRatio = Random.Range(-10f, 10f);
            }

            if (PerfectTimingRatio == 0f) {
                PerfectTimingRatio = Random.Range(-10f, 10f);
            }

            PerfectFuelRatio = Mathf.Round(PerfectFuelRatio / 0.5f) * 0.5f;
            PerfectTimingRatio = Mathf.Round(PerfectTimingRatio / 0.5f) * 0.5f;
            Ebrake = cData.Ebrake;
            OnValidate();
            UpdateEngineModel();
        }

        public string ExportData() {
            CarControllerData carControllerData = GetCarControllerData();
            return XmlSerialization.SerializeData<CarControllerData>(carControllerData);
        }

        public void ImportData(string XMLString) {
            CarControllerData carControllerData =
                (CarControllerData)XmlSerialization.DeserializeData<CarControllerData>(XMLString);
            SetCarControllerData(carControllerData);
        }
    }
}