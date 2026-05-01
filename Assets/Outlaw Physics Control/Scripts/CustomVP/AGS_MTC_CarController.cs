using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace AGS_MonsterTruckControl {
    public class AGS_MTC_CarController : MonoBehaviour {
        [System.Serializable] public class IntUnityEvent : UnityEvent<int> { }

        #region Stunt Events
        [Header("Stunt Events")]
        public UnityEvent OnWheelieEnter;
        public UnityEvent OnWheelieExit;
        public UnityEvent OnNoseWheelieEnter;
        public UnityEvent OnNoseWheelieExit;
        public UnityEvent OnSideWheelieEnter;
        public UnityEvent OnSideWheelieExit;
        public UnityEvent OnDonutEnter;
        public UnityEvent OnDonutExit;
        public UnityEvent OnDonutRoundComplete;
        public UnityEvent OnSideSelfRightEnter;
        public UnityEvent OnSideSelfRightRecovered;
        public UnityEvent OnAutoBackflipLanded;

        public IntUnityEvent OnUserFrontFlip;
        public IntUnityEvent OnUserBackFlip;

        private int userFrontFlipCount = 0;
        private int userBackFlipCount = 0;

        private Vector3 userFlipReferenceForward = Vector3.forward;
        private Vector3 userFlipReferenceRight = Vector3.right;

        private float userFlipLastPitch = 0f;
        private float userFlipUnwrappedPitch = 0f;
        private bool userFlipHasReference = false;

        private bool wheelieEventPrevState = false;
        private bool noseWheelieEventPrevState = false;
        private bool sideWheelieEventPrevState = false;
        private bool donutEventPrevState = false;
        private bool sideSelfRightEventPrevState = false;
        private bool autoBackflipWasActive = false;
        private bool autoBackflipLandingEventPending = false;

        private bool donutStateActive = false;
        private float donutAccumulatedYaw = 0f;
        private Vector3 donutPrevForwardFlat = Vector3.zero;

        private void ResetUserAirFlipEvents() {
            userFrontFlipCount = 0;
            userBackFlipCount = 0;

            userFlipReferenceForward = Vector3.forward;
            userFlipReferenceRight = Vector3.right;

            userFlipLastPitch = 0f;
            userFlipUnwrappedPitch = 0f;
            userFlipHasReference = false;
        }

        private void SetupUserFlipReference() {
            // Use actual travel direction first.
            Vector3 flatForward = Vector3.ProjectOnPlane(m_Rigidbody.velocity, Vector3.up);

            if (flatForward.sqrMagnitude < 0.25f) {
                flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            }

            if (flatForward.sqrMagnitude < 0.001f) {
                flatForward = Vector3.ProjectOnPlane(transform.up, Vector3.up);
            }

            if (flatForward.sqrMagnitude < 0.001f) {
                flatForward = Vector3.forward;
            }

            userFlipReferenceForward = flatForward.normalized;

            userFlipReferenceRight = Vector3.Cross(Vector3.up, userFlipReferenceForward);

            if (userFlipReferenceRight.sqrMagnitude < 0.001f) {
                userFlipReferenceRight = transform.right;
            }

            userFlipReferenceRight.Normalize();

            userFlipLastPitch = GetGroundAlignedPitchAngleDegrees();
            userFlipUnwrappedPitch = userFlipLastPitch;

            userFlipHasReference = true;
        }

        private float GetGroundAlignedPitchAngleDegrees() {
            Vector3 fwdOnPitchPlane =
                Vector3.ProjectOnPlane(transform.forward, userFlipReferenceRight);

            if (fwdOnPitchPlane.sqrMagnitude < 0.001f) {
                return userFlipLastPitch;
            }

            fwdOnPitchPlane.Normalize();

            // Fixed reference:
            // 0 = aligned with ground/reference forward.
            // +360 or -360 = completed flip while aligned back to ground.
            return Vector3.SignedAngle(
                userFlipReferenceForward,
                fwdOnPitchPlane,
                userFlipReferenceRight
            );
        }

        private void UpdateUserAirFlipEvents(bool airborne) {
            // Do not count assisted auto-backflip as user flip.
            if (autoBackflipActive || autoBackflipLandingCatchActive) {
                return;
            }

            if (!airborne) {
                ResetUserAirFlipEvents();
                return;
            }

            if (!userFlipHasReference) {
                SetupUserFlipReference();
                return;
            }

            float currentPitch = GetGroundAlignedPitchAngleDegrees();

            float delta = Mathf.DeltaAngle(userFlipLastPitch, currentPitch);
            userFlipLastPitch = currentPitch;

            userFlipUnwrappedPitch += delta;

            while (userFlipUnwrappedPitch >= 355f * (userFrontFlipCount + 1)) {
                userFrontFlipCount++;
                OnUserFrontFlip?.Invoke(userFrontFlipCount);
            }

            while (userFlipUnwrappedPitch <= -355f * (userBackFlipCount + 1)) {
                userBackFlipCount++;
                OnUserBackFlip?.Invoke(userBackFlipCount);
            }
        }

        private void UpdateAutoBackflipLandingEvent() {
            bool autoBackflipRunning = autoBackflipActive || autoBackflipLandingCatchActive;

            // Backflip has started.
            if (autoBackflipRunning) {
                autoBackflipWasActive = true;
                autoBackflipLandingEventPending = true;
                return;
            }

            // Backflip finished, now wait until all tires are grounded.
            if (autoBackflipWasActive && autoBackflipLandingEventPending) {
                if (AllTiresGrounded()) {
                    autoBackflipLandingEventPending = false;
                    autoBackflipWasActive = false;
                    OnAutoBackflipLanded.Invoke();
                }
            }

            // Safety reset if stunt is over but truck is not landing properly for some reason.
            if (!autoBackflipRunning && Grounded() && !autoBackflipLandingEventPending) {
                autoBackflipWasActive = false;
            }
        }
        private void UpdateStuntEvents() {
            // Rear wheelie
            bool wheelieState = EnableWheelieHold;

            if (wheelieState && !wheelieEventPrevState)
                OnWheelieEnter.Invoke();
            else if (!wheelieState && wheelieEventPrevState)
                OnWheelieExit.Invoke();

            wheelieEventPrevState = wheelieState;


            // Nose wheelie
            bool noseWheelieState = EnableNoseWheelieHold;

            if (noseWheelieState && !noseWheelieEventPrevState)
                OnNoseWheelieEnter.Invoke();
            else if (!noseWheelieState && noseWheelieEventPrevState)
                OnNoseWheelieExit.Invoke();

            noseWheelieEventPrevState = noseWheelieState;


            // Auto backflip
            UpdateAutoBackflipLandingEvent();

            // Side wheelie
            bool sideWheelieState = sideWheelieCOMState != 0 || sideWheeliAssistEnabled;

            if (sideWheelieState && !sideWheelieEventPrevState)
                OnSideWheelieEnter.Invoke();
            else if (!sideWheelieState && sideWheelieEventPrevState)
                OnSideWheelieExit.Invoke();

            sideWheelieEventPrevState = sideWheelieState;


            // Donut
            bool donutState = donutStateActive;

            if (donutState && !donutEventPrevState) {
                OnDonutEnter.Invoke();

                donutAccumulatedYaw = 0f;
                donutCompletedRounds = 0;
                currentDonutTargetYawRate = DonutTargetYawRate;

                donutPrevForwardFlat = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
                if (donutPrevForwardFlat.sqrMagnitude > 0.0001f)
                    donutPrevForwardFlat.Normalize();
            }

            if (donutState) {
                Vector3 currentFlatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

                if (currentFlatForward.sqrMagnitude > 0.0001f) {
                    currentFlatForward.Normalize();

                    if (donutPrevForwardFlat.sqrMagnitude > 0.0001f) {
                        float deltaYaw = Vector3.SignedAngle(
                            donutPrevForwardFlat,
                            currentFlatForward,
                            Vector3.up
                        );

                        donutAccumulatedYaw += deltaYaw;

                        while (Mathf.Abs(donutAccumulatedYaw) >= 360f) {
                            donutAccumulatedYaw -= Mathf.Sign(donutAccumulatedYaw) * 360f;
                            donutCompletedRounds++;
                            OnDonutRoundComplete.Invoke();
                        }
                    }

                    donutPrevForwardFlat = currentFlatForward;
                }
            }
            else if (!donutState && donutEventPrevState) {
                OnDonutExit.Invoke();

                donutAccumulatedYaw = 0f;
                donutCompletedRounds = 0;
                currentDonutTargetYawRate = DonutTargetYawRate;
                donutPrevForwardFlat = Vector3.zero;
            }

            donutEventPrevState = donutState;


            // Side self right
            bool recoverState = sideSelfRightActive;

            if (recoverState && !sideSelfRightEventPrevState)
                OnSideSelfRightEnter.Invoke();

            sideSelfRightEventPrevState = recoverState;
        }
        #endregion

        public AGS_MTC_VehicleType vehicleType;
        public bool vehicleIsActive;

        #region Nose Wheeling

        [Header("Nose Wheelie Stable Pose Lock")]
        public bool NoseWheelieUseStablePoseLock = true;

        [Range(0f, 1f)]
        public float NoseWheeliePoseLockStrength = 1f;
        // 1 = fully locked like current behavior.
        // 0 = no pose lock at all.
        // 0.3 - 0.7 = assisted balance feel.

        // How fast the body rotates into the target pose.
        // Higher = snappier. Lower = smoother.
        public float NoseWheeliePoseLockSpeed = 12f;

        // How much angular velocity is removed while locked.
        // 1 = remove all spin. 0.8 = remove most spin.
        [Range(0f, 1f)]
        public float NoseWheelieAngularVelocityKill = 0.92f;

        // Race decreases from 90, brake increases beyond 90.
        public float NoseWheelieRaceAngleDecrease = 14f;
        public float NoseWheelieBrakeAngleIncrease = 12f;

        // Smoothing speed for race/brake angle change.
        public float NoseWheelieInputAngleResponse = 90f;

        private Vector3 noseWheelieLockedRearDirection = Vector3.forward;

        [Header("Nose Wheelie Dynamic Lock Strength")]
        public bool NoseWheelieUseDynamicLockStrength = true;

        // How low assistance can fall over time.
        // 0 = can become fully manual/free.
        // 0.25 = always keeps some assist.
        [Range(0f, 1f)]
        public float NoseWheelieMinDynamicLockStrength = 0.20f;

        // Strength decrease per second when player gives no race/brake input.
        public float NoseWheelieLockStrengthDecaySpeed = 0.18f;

        // Strength increase per second while player gives race/brake input.
        public float NoseWheelieLockStrengthRecoverSpeed = 0.65f;

        // Minimum race/brake input required to count as active balancing input.
        [Range(0f, 1f)]
        public float NoseWheelieLockStrengthInputThreshold = 0.12f;

        public float noseWheelieCachedInitialLockStrength = 1f;
        public float noseWheelieRuntimeLockStrength = 1f;

        [Header("Nose Wheelie Angle Exit")]
        public bool NoseWheelieUseAngleExit = true;

        // If actual nose-wheelie angle drops below this, exit and land.
        public float NoseWheelieLandExitAngle = 65f;

        // If actual nose-wheelie angle exceeds this, exit and allow front flip.
        public float NoseWheelieFlipExitAngle = 100f;

        // Delay before angle-exit starts checking.
        // Needed so the entry transition does not instantly cancel.
        public float NoseWheelieAngleExitStartDelay = 0.65f;

        // Optional extra pitch kick when exiting toward flip.
        // Keep 0 if you want pure physics.
        public float NoseWheelieFlipExitPitchKick = 0f;

        private float noseWheelieHoldTimer = 0f;

        [Header("Nose Wheelie Re-Entry After Angle Exit")]
        public bool NoseWheelieAllowAngleReEntry = true;

        // How long after angle-exit the truck can re-enter nose-wheelie.
        public float NoseWheelieAngleReEntryWindow = 1.25f;

        // Extra padding inside the valid angle range.
        // Example: with 3 degrees, re-entry happens only between 68 and 97,
        // not exactly at 65 or 100.
        public float NoseWheelieReEntryAnglePadding = 3f;

        // If true, recovery is only possible while rear tires are still in air.
        public bool NoseWheelieReEntryRequiresRearAir = true;

        private bool noseWheelieAngleReEntryPending = false;
        private float noseWheelieAngleReEntryTimer = 0f;

        [Header("Nose Wheelie")]
        public bool EnableNoseWheelieStunt = true;
        public Transform noseWheelieCOM;

        // Main angle control.
        // 0.12 = small nose wheelie.
        // 0.45 = high nose wheelie.
        // 0.75 = very high.
        // 0.90 = almost vertical.
        // Do not use above 0.98.
        [Range(10f, 90f)]
        public float NoseWheelieHoldAngleDegrees = 90f;
        // 90 = truck nose points straight toward ground.

        [Header("Nose Wheelie Tire Balance Input")]
        public bool NoseWheelieUseTireBalanceInput = true;

        // How fast target angle changes from race/brake input.
        public float NoseWheelieBalanceInputResponse = 90f;

        private float noseWheelieRuntimeTargetAngle = 90f;

        public float NoseWheelieCOMLerpSpeed = 6f;
        public float NoseWheelieCOMReturnSpeed = 2.5f;

        [Range(0f, 1f)]
        public float NoseWheelieHoldCOMBackToBase = 0.15f;

        [Header("Nose Wheelie Entry")]
        public float NoseWheelieEntryMinSpeed = 28f;
        public float NoseWheelieLiftStartSpeed = 16f;
        public float NoseWheelieBrakeThreshold = -0.2f;

        [Header("Nose Wheelie Angle Speed")]
        public float NoseWheelieAngleRaiseSpeed = 4f;
        public float NoseWheelieAngleDropSpeed = 12f;

        [Header("Nose Wheelie Steering")]
        public float NoseWheelieSteerMultiplier = 1.35f;
        public float NoseWheelieSteerReturnSpeed = 160f;
        public float NoseWheelieSteerInputResponse = 8f;

        [Header("Nose Wheelie Drive")]
        public float NoseWheelieFrontDriveMultiplier = 0f;
        // Keep 0 if you do NOT want the truck to drive forward during nose-wheelie.
        // Increase only if you want front wheels to pull during the stunt.

        [Range(0f, 1f)]
        public float NoseWheelieLongitudinalAntirollMultiplier = 0.05f;

        private bool noseWheeliePrimed = false;
        public bool EnableNoseWheelieHold = false;

        private float cachedNoseWheelieAngle = 0f;
        private float currentNoseWheelieAngle = 0f;
        private float noseWheelieSmoothedSteer = 0f;
        private bool wasNoseWheelieActiveLastFrame = false;

        private void ArmNoseWheelieAngleReEntry() {
            if (!NoseWheelieAllowAngleReEntry) {
                StopNoseWheelie();
                return;
            }

            noseWheeliePrimed = false;
            EnableNoseWheelieHold = false;
            cachedNoseWheelieAngle = 0f;
            noseWheelieHoldTimer = 0f;

            noseWheelieAngleReEntryPending = true;
            noseWheelieAngleReEntryTimer = NoseWheelieAngleReEntryWindow;

            // Keep the same facing/travel direction for re-entry.
            if (noseWheelieLockedRearDirection.sqrMagnitude < 0.001f) {
                noseWheelieLockedRearDirection = GetNoseWheelieFlatForwardDirection();
            }

            noseWheelieCachedInitialLockStrength = Mathf.Clamp01(NoseWheeliePoseLockStrength);
            noseWheelieRuntimeLockStrength = noseWheelieCachedInitialLockStrength;
        }

        private void CancelNoseWheelieAngleReEntry() {
            noseWheelieAngleReEntryPending = false;
            noseWheelieAngleReEntryTimer = 0f;
        }

        private void ReactivateNoseWheelieFromAngleReEntry() {
            EnableNoseWheelieHold = true;
            noseWheeliePrimed = false;
            noseWheelieHoldTimer = 0f;

            noseWheelieAngleReEntryPending = false;
            noseWheelieAngleReEntryTimer = 0f;

            noseWheelieCachedInitialLockStrength = Mathf.Clamp01(NoseWheeliePoseLockStrength);
            noseWheelieRuntimeLockStrength = noseWheelieCachedInitialLockStrength;

            noseWheelieRuntimeTargetAngle = Mathf.Clamp(
                GetCurrentNoseWheelieAngleDegrees(),
                NoseWheelieLandExitAngle + NoseWheelieReEntryAnglePadding,
                NoseWheelieFlipExitAngle - NoseWheelieReEntryAnglePadding
            );

            cachedNoseWheelieAngle = Mathf.Abs(GetNoseWheelieTargetPitch());
            currentNoseWheelieAngle = -cachedNoseWheelieAngle;
        }

        private bool CanReEnterNoseWheelieFromAngle() {
            if (!noseWheelieAngleReEntryPending)
                return false;

            if (!FrontAxleGrounded())
                return false;

            if (NoseWheelieReEntryRequiresRearAir && RearAxleGrounded())
                return false;

            float actualAngle = GetCurrentNoseWheelieAngleDegrees();

            float minAngle = NoseWheelieLandExitAngle + NoseWheelieReEntryAnglePadding;
            float maxAngle = NoseWheelieFlipExitAngle - NoseWheelieReEntryAnglePadding;

            return actualAngle >= minAngle && actualAngle <= maxAngle;
        }

        private void UpdateNoseWheelieDynamicLockStrength() {
            if (!NoseWheelieUseDynamicLockStrength) {
                noseWheelieRuntimeLockStrength = Mathf.Clamp01(NoseWheeliePoseLockStrength);
                noseWheelieCachedInitialLockStrength = noseWheelieRuntimeLockStrength;
                return;
            }

            float inputAmount = Mathf.Max(
                Mathf.Clamp01(yInput),
                Mathf.Clamp01(-yInput)
            );

            bool userBalancing =
                inputAmount >= NoseWheelieLockStrengthInputThreshold;

            float targetStrength = userBalancing
                ? noseWheelieCachedInitialLockStrength
                : NoseWheelieMinDynamicLockStrength;

            float moveSpeed = userBalancing
                ? NoseWheelieLockStrengthRecoverSpeed
                : NoseWheelieLockStrengthDecaySpeed;

            noseWheelieRuntimeLockStrength = Mathf.MoveTowards(
                noseWheelieRuntimeLockStrength,
                targetStrength,
                Time.fixedDeltaTime * moveSpeed
            );

            noseWheelieRuntimeLockStrength = Mathf.Clamp(
                noseWheelieRuntimeLockStrength,
                NoseWheelieMinDynamicLockStrength,
                noseWheelieCachedInitialLockStrength
            );
        }
        private float GetCurrentNoseWheelieAngleDegrees() {
            Vector3 rearDir = noseWheelieLockedRearDirection;

            if (rearDir.sqrMagnitude < 0.001f) {
                rearDir = GetNoseWheelieFlatForwardDirection();
            }

            rearDir = Vector3.ProjectOnPlane(rearDir, Vector3.up);

            if (rearDir.sqrMagnitude < 0.001f)
                rearDir = Vector3.ProjectOnPlane(transform.up, Vector3.up);

            if (rearDir.sqrMagnitude < 0.001f)
                return 0f;

            rearDir.Normalize();

            Vector3 fwd = transform.forward.normalized;

            // Component toward original travel/rear direction.
            float horizontalComponent = Vector3.Dot(fwd, rearDir);

            // Component toward ground.
            float downComponent = Vector3.Dot(fwd, Vector3.down);

            // 0   = level
            // 90  = nose straight down
            // >90 = passed vertical / front-flip side
            float angle = Mathf.Atan2(downComponent, horizontalComponent) * Mathf.Rad2Deg;

            return angle;
        }
        private Vector3 GetNoseWheelieFlatForwardDirection() {
            // Prefer actual movement direction if available.
            Vector3 flatVelocity = Vector3.ProjectOnPlane(m_Rigidbody.velocity, Vector3.up);

            if (flatVelocity.sqrMagnitude > 0.25f) {
                return flatVelocity.normalized;
            }

            // Fallback to truck forward direction.
            Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

            if (flatForward.sqrMagnitude > 0.001f) {
                return flatForward.normalized;
            }

            return transform.forward;
        }
        private float GetNoseWheelieTargetPitch() {
            float baseAngle = Mathf.Clamp(NoseWheelieHoldAngleDegrees, 10f, 90f);
            float targetAngle = baseAngle;

            if (EnableNoseWheelieHold && NoseWheelieUseTireBalanceInput) {
                float race01 = Mathf.Clamp01(yInput);
                float brake01 = Mathf.Clamp01(-yInput);

                targetAngle =
                    baseAngle
                    - race01 * NoseWheelieRaceAngleDecrease
                    + brake01 * NoseWheelieBrakeAngleIncrease;

                targetAngle = Mathf.Clamp(targetAngle, 55f, 110f);
            }

            noseWheelieRuntimeTargetAngle = Mathf.MoveTowards(
                noseWheelieRuntimeTargetAngle,
                targetAngle,
                Time.fixedDeltaTime * NoseWheelieBalanceInputResponse
            );

            // Dot pitch still only needs -sin(angle).
            // For angles above 90, sin starts reducing again, so this is only used
            // for cached/display angle. Actual rotation is handled in ForceNoseWheelieVerticalPose().
            return -Mathf.Sin(Mathf.Clamp(noseWheelieRuntimeTargetAngle, 10f, 90f) * Mathf.Deg2Rad);
        }
        private bool IsOtherStuntBlockingNoseWheelie() {
            bool donutTryingOrActive =
                donutStuntActive &&
                Mathf.Abs(xInput) > 0.9f &&
                Mathf.Abs(yInput) > 0.9f &&
                !sideWheeliAssistEnabled;

            bool sideWheelieActive =
                sideWheelieCOMState != 0 ||
                sideWheeliAssistEnabled;

            return
                EnableWheelieHold ||
                sideSelfRightActive ||
                upsideDownRollActive ||
                autoBackflipActive ||
                autoBackflipLandingCatchActive ||
                donutTryingOrActive ||
                sideWheelieActive;
        }

        private void StopNoseWheelie() {
            noseWheeliePrimed = false;
            EnableNoseWheelieHold = false;
            cachedNoseWheelieAngle = 0f;
            noseWheelieHoldTimer = 0f;

            CancelNoseWheelieAngleReEntry();

            noseWheelieCachedInitialLockStrength = Mathf.Clamp01(NoseWheeliePoseLockStrength);
            noseWheelieRuntimeLockStrength = noseWheelieCachedInitialLockStrength;
        }
        private void ForceNoseWheelieVerticalPose() {
            if (!NoseWheelieUseStablePoseLock) {
                return;
            }

            float baseAngle = Mathf.Clamp(NoseWheelieHoldAngleDegrees, 10f, 90f);

            float race01 = Mathf.Clamp01(yInput);
            float brake01 = Mathf.Clamp01(-yInput);

            float targetAngle =
                baseAngle
                - race01 * NoseWheelieRaceAngleDecrease
                + brake01 * NoseWheelieBrakeAngleIncrease;

            targetAngle = Mathf.Clamp(targetAngle, 60f, 108f);

            noseWheelieRuntimeTargetAngle = Mathf.MoveTowards(
                noseWheelieRuntimeTargetAngle,
                targetAngle,
                Time.fixedDeltaTime * NoseWheelieInputAngleResponse
            );

            float rad = noseWheelieRuntimeTargetAngle * Mathf.Deg2Rad;

            Vector3 rearDir = noseWheelieLockedRearDirection;

            if (rearDir.sqrMagnitude < 0.001f) {
                rearDir = Vector3.ProjectOnPlane(transform.up, Vector3.up);

                if (rearDir.sqrMagnitude < 0.001f)
                    rearDir = transform.forward;

                rearDir.Normalize();
                noseWheelieLockedRearDirection = rearDir;
            }

            // 90 degrees = nose straight down.
            // Less than 90 = landing side.
            // More than 90 = flip side.
            Vector3 targetForward =
                rearDir * Mathf.Cos(rad) +
                Vector3.down * Mathf.Sin(rad);

            targetForward.Normalize();

            Vector3 targetUp = rearDir * Mathf.Sin(rad) + Vector3.up * Mathf.Cos(rad);

            targetUp = Vector3.ProjectOnPlane(targetUp, targetForward);

            if (targetUp.sqrMagnitude < 0.001f)
                targetUp = Vector3.ProjectOnPlane(transform.up, targetForward);

            if (targetUp.sqrMagnitude < 0.001f)
                targetUp = Vector3.forward;

            targetUp.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(targetForward, targetUp);

            UpdateNoseWheelieDynamicLockStrength();

            float lockStrength = NoseWheelieUseDynamicLockStrength
                ? Mathf.Clamp01(noseWheelieRuntimeLockStrength)
                : Mathf.Clamp01(NoseWheeliePoseLockStrength);

            if (lockStrength <= 0.001f)
                return;

            // Pose lock strength affects how tightly the truck follows the target angle.
            float poseT =
                Time.fixedDeltaTime *
                NoseWheeliePoseLockSpeed *
                lockStrength;

            Quaternion newRotation = Quaternion.Slerp(
                m_Rigidbody.rotation,
                targetRotation,
                poseT
            );

            m_Rigidbody.MoveRotation(newRotation);

            // Angular velocity kill also scales with lock strength.
            // At 1 = stable hard lock.
            // At 0 = physics is free.
            float angularKill =
                NoseWheelieAngularVelocityKill *
                lockStrength;

            m_Rigidbody.angularVelocity = Vector3.Lerp(
                m_Rigidbody.angularVelocity,
                Vector3.zero,
                angularKill
            );
        }

        private void UpdateNoseWheelieState() {
            if (!EnableNoseWheelieStunt || noseWheelieCOM == null) {
                StopNoseWheelie();

                cachedNoseWheelieAngle = Mathf.Abs(GetNoseWheelieTargetPitch());

                return;
            }

            bool brakePressed = yInput <= NoseWheelieBrakeThreshold;

            // IMPORTANT:
            // Once nose-wheelie has entered, do NOT exit.
            // No speed check, no throttle check, no rear-wheel check.
            if (EnableNoseWheelieHold) {
                noseWheeliePrimed = false;
                noseWheelieHoldTimer += Time.fixedDeltaTime;

                float actualAngle = GetCurrentNoseWheelieAngleDegrees();

                if (NoseWheelieUseAngleExit &&
                    noseWheelieHoldTimer >= NoseWheelieAngleExitStartDelay) {

                    // Too low: temporarily release pose lock.
                    // If user brings it back while rear tires are still in air,
                    // nose-wheelie can re-enter.
                    if (actualAngle < NoseWheelieLandExitAngle) {
                        ArmNoseWheelieAngleReEntry();
                        return;
                    }

                    // Too far forward: temporarily release pose lock.
                    // If user saves it with race input and angle returns,
                    // nose-wheelie can re-enter.
                    if (actualAngle > NoseWheelieFlipExitAngle) {
                        ArmNoseWheelieAngleReEntry();

                        if (NoseWheelieFlipExitPitchKick > 0f) {
                            m_Rigidbody.AddRelativeTorque(
                                NoseWheelieFlipExitPitchKick,
                                0f,
                                0f,
                                ForceMode.Acceleration
                            );
                        }

                        return;
                    }
                }

                float targetPitch = GetNoseWheelieTargetPitch();
                cachedNoseWheelieAngle = Mathf.Abs(targetPitch);

                currentNoseWheelieAngle = Mathf.MoveTowards(
                    currentNoseWheelieAngle,
                    targetPitch,
                    Time.fixedDeltaTime * NoseWheelieAngleRaiseSpeed
                );

                return;
            }

            // Angle-exit recovery:
            // If the player saves the angle before rear tires land,
            // re-enter nose-wheelie without requiring the original brake/speed entry.
            if (noseWheelieAngleReEntryPending) {
                noseWheelieAngleReEntryTimer -= Time.fixedDeltaTime;

                bool rearLanded =
                    NoseWheelieReEntryRequiresRearAir &&
                    RearAxleGrounded();

                if (noseWheelieAngleReEntryTimer <= 0f || rearLanded) {
                    CancelNoseWheelieAngleReEntry();
                }
                else if (CanReEnterNoseWheelieFromAngle()) {
                    ReactivateNoseWheelieFromAngleReEntry();
                    return;
                }
            }

            // Do not start nose-wheelie while another stunt is already active.
            // This only blocks entry. It does NOT exit once nose-wheelie is active.
            if (IsOtherStuntBlockingNoseWheelie()) {
                noseWheeliePrimed = false;

                cachedNoseWheelieAngle = Mathf.Abs(GetNoseWheelieTargetPitch());

                return;
            }

            // Step 1: prime when braking from enough speed.
            if (!noseWheeliePrimed && Grounded() && Speed >= NoseWheelieEntryMinSpeed && brakePressed) {
                noseWheeliePrimed = true;

                cachedNoseWheelieAngle = Mathf.Abs(GetNoseWheelieTargetPitch());
            }

            // If player releases brake before lift starts, cancel prime.
            if (noseWheeliePrimed && !brakePressed) {
                noseWheeliePrimed = false;
            }

            // Step 2: enter nose-wheelie when braking slows truck to lift-start speed.
            if (noseWheeliePrimed &&
                brakePressed &&
                Speed <= NoseWheelieLiftStartSpeed &&
                FrontAxleGrounded()) {

                EnableNoseWheelieHold = true;
                noseWheeliePrimed = false;
                noseWheelieHoldTimer = 0f;
                noseWheelieCachedInitialLockStrength = Mathf.Clamp01(NoseWheeliePoseLockStrength);
                noseWheelieRuntimeLockStrength = noseWheelieCachedInitialLockStrength;

                noseWheelieRuntimeTargetAngle = NoseWheelieHoldAngleDegrees;

                // Lock the truck's original travel/facing direction once.
                // At 90° nose-wheelie, transform.forward points down,
                // and transform.up should point in this locked direction.
                // This prevents the body from spinning 180° around Z.
                noseWheelieLockedRearDirection = GetNoseWheelieFlatForwardDirection();

                noseWheelieRuntimeTargetAngle = NoseWheelieHoldAngleDegrees;

                cachedNoseWheelieAngle = Mathf.Abs(GetNoseWheelieTargetPitch());

                currentNoseWheelieAngle = Mathf.MoveTowards(
                    currentNoseWheelieAngle,
                    -cachedNoseWheelieAngle,
                    Time.fixedDeltaTime * NoseWheelieAngleRaiseSpeed
                );

                return;
            }

            // Not active yet.
            currentNoseWheelieAngle = Mathf.MoveTowards(
                currentNoseWheelieAngle,
                0f,
                Time.fixedDeltaTime * NoseWheelieAngleDropSpeed
            );
        }

        private void DoNoseWheelieAssist() {
            bool noseWheelieActive = EnableNoseWheelieHold;

            if (!noseWheelieActive || noseWheelieCOM == null) {
                noseWheelieSmoothedSteer = Mathf.MoveTowards(
                    noseWheelieSmoothedSteer,
                    0f,
                    Time.fixedDeltaTime * NoseWheelieSteerInputResponse
                );
                return;
            }

            float targetPitch = GetNoseWheelieTargetPitch();
            cachedNoseWheelieAngle = Mathf.Abs(targetPitch);

            currentNoseWheelieAngle = Mathf.MoveTowards(
                currentNoseWheelieAngle,
                targetPitch,
                Time.fixedDeltaTime * NoseWheelieAngleRaiseSpeed
            );

            // Stable pose lock handles the 90-degree balance.
            ForceNoseWheelieVerticalPose();

            // Keep steering input smoothing for wheel visual steer only.
            float targetSteerInput = Mathf.Clamp(xInput, -1f, 1f);

            noseWheelieSmoothedSteer = Mathf.MoveTowards(
                noseWheelieSmoothedSteer,
                targetSteerInput,
                Time.fixedDeltaTime * NoseWheelieSteerInputResponse
            );
        }

        #endregion

        #region Rear Wheeling

        [Header("Wheelie Drive")]
        public Transform rearCOM;
        [Range(0f, 1f)] public float WheelieFrontDriveMultiplier = 0f;
        public float WheelieRearDriveMultiplier = 2.2f;

        [Header("Wheelie Hold")]
        public bool EnableWheelieStunt = true;
        public bool EnableWheelieHold = false;
        public float WheelingAngle = 0.16f;

        public float WheeliePitchForce = 14f;
        public float WheeliePitchDamping = 4f;
        public float WheelieMaxPitchAssist = 12f;

        public float WheelieYawDamping = 2.5f;

        public float WheelieRollForce = 6f;
        public float WheelieRollDamping = 3.5f;
        public float WheelieMaxRollAssist = 6f;
        public float WheelieAngleLerpSpeed = 4f;
        public float WheelieHoldMaxSpeed = 25f;

        public float WheelieRearSteerMultiplier = 0.9f;
        public float WheelieTargetYawRate = 1.6f;
        public float WheelieYawForce = 10f;
        public float WheelieMaxYawAssist = 8f;

        public float WheelieExitMinSpeed = 12f;
        public float WheelieReleasedThrottleThreshold = 0.05f;
        public float WheelieEntrySpeedGraceTime = 0.45f;

        private float wheelieEntrySpeedGraceTimer = 0f;

        [Header("Exit Wheeling With Throttle")]
        public float WheelieNoThrottleExitTime = 0.35f;
        private float wheelieNoThrottleTimer = 0f;

        [Header("Wheeling with throttle")]
        public float WheelieAngleDropSpeed = 12f;
        public float WheelieAngleRaiseThrottleThreshold = 0.05f;
        private float cachedWheelieAngle = 0f;

        [Header("Reverse Snap To Wheelie")]
        public float ReverseSnapMinReverseSpeed = 6f;
        public float ReverseSnapForwardBoostSpeed = 8f;   // mph
        public float ReverseSnapForwardAssist = 18f;

        private bool reverseSnapWasTriggering = false;
        private float currentWheelieAngle = 0f;

        private void DoWheelieHoldAssist() {
            if (!EnableWheelieStunt) {
                EnableWheelieHold = false;
                wheelieEntrySpeedGraceTimer = 0f;
                wheelieNoThrottleTimer = 0f;

                currentWheelieAngle = Mathf.MoveTowards(
                    currentWheelieAngle,
                    0f,
                    Time.fixedDeltaTime * WheelieAngleDropSpeed
                );
                return;
            }

            if (wheelieEntrySpeedGraceTimer > 0f) {
                wheelieEntrySpeedGraceTimer -= Time.fixedDeltaTime;
            }

            bool releasedThrottle = yInput <= WheelieReleasedThrottleThreshold;
            bool canUseMinSpeedExit = wheelieEntrySpeedGraceTimer <= 0f;

            if (EnableWheelieHold) {
                if (releasedThrottle) {
                    wheelieNoThrottleTimer += Time.fixedDeltaTime;
                }
                else {
                    wheelieNoThrottleTimer = 0f;
                }

                if (IsOtherStuntBlockingWheelie()) {
                    EnableWheelieHold = false;
                }
                else if (wheelieNoThrottleTimer >= WheelieNoThrottleExitTime) {
                    EnableWheelieHold = false;
                }
                else if (releasedThrottle && canUseMinSpeedExit && Speed <= WheelieExitMinSpeed) {
                    EnableWheelieHold = false;
                }
            }
            else {
                wheelieNoThrottleTimer = 0f;
            }

            if (!EnableWheelieHold || rearCOM == null) {
                currentWheelieAngle = Mathf.MoveTowards(
                    currentWheelieAngle,
                    0f,
                    Time.fixedDeltaTime * WheelieAngleDropSpeed
                );
                return;
            }

            float throttle01 = Mathf.Clamp01(Throttle);

            float targetWheelieAngle =
                (throttle01 > WheelieAngleRaiseThrottleThreshold)
                ? cachedWheelieAngle * throttle01
                : 0f;

            float angleMoveSpeed =
                (targetWheelieAngle > currentWheelieAngle)
                ? WheelieAngleLerpSpeed
                : WheelieAngleDropSpeed;

            currentWheelieAngle = Mathf.MoveTowards(
                currentWheelieAngle,
                targetWheelieAngle,
                Time.fixedDeltaTime * angleMoveSpeed
            );

            if (!RearAxleGrounded())
                return;

            float currentPitch = Vector3.Dot(transform.forward, Vector3.up);
            float currentRoll = Vector3.Dot(transform.right, Vector3.up);

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float pitchRate = localAngularVelocity.x;
            float yawRate = localAngularVelocity.y;
            float rollRate = localAngularVelocity.z;

            float pitchAssist = -(currentWheelieAngle - currentPitch) * WheeliePitchForce
                                - pitchRate * WheeliePitchDamping;
            pitchAssist = Mathf.Clamp(pitchAssist, -WheelieMaxPitchAssist, WheelieMaxPitchAssist);

            float yawAssist;

            if (Mathf.Abs(xInput) > 0.05f) {
                float targetYawRate = xInput * WheelieTargetYawRate;
                yawAssist = (targetYawRate - yawRate) * WheelieYawForce;
            }
            else {
                yawAssist = -yawRate * WheelieYawDamping;
            }

            yawAssist = Mathf.Clamp(yawAssist, -WheelieMaxYawAssist, WheelieMaxYawAssist);

            float rollAssist = (-currentRoll) * WheelieRollForce
                               - rollRate * WheelieRollDamping;
            rollAssist = Mathf.Clamp(rollAssist, -WheelieMaxRollAssist, WheelieMaxRollAssist);

            m_Rigidbody.AddRelativeTorque(pitchAssist, yawAssist, rollAssist, ForceMode.Acceleration);
        }
        private bool FrontAxleGrounded() {
            return wheels.Count >= 4 && (wheels[0].wc.IsGrounded || wheels[1].wc.IsGrounded);
        }

        private bool RearAxleGrounded() {
            return wheels.Count >= 4 && (wheels[2].wc.IsGrounded || wheels[3].wc.IsGrounded);
        }
        private bool AllTiresGrounded() {
            if (wheels == null || wheels.Count == 0)
                return false;

            for (int i = 0; i < wheels.Count; i++) {
                if (wheels[i].wc == null || !wheels[i].wc.IsGrounded)
                    return false;
            }

            return true;
        }

        private bool IsOtherStuntBlockingWheelie() {
            bool donutTryingOrActive =
                donutStuntActive &&
                Mathf.Abs(xInput) > 0.9f &&
                Mathf.Abs(yInput) > 0.9f &&
                !sideWheeliAssistEnabled;

            bool sideWheelieActive =
                sideWheelieCOMState != 0 ||
                sideWheeliAssistEnabled;

            return
                sideSelfRightActive ||
                upsideDownRollActive ||
                autoBackflipActive ||
                autoBackflipLandingCatchActive ||
                donutTryingOrActive ||
                sideWheelieActive ||
                EnableNoseWheelieHold;
        }
        #endregion

        #region Auto Backflip
        [Header("Auto Backflip")]
        public bool EnableAutoBackflip = true;
        public string BackflipLaunchTag = "BackflipLaunch";
        public float BackflipArmMemory = 0.35f;
        public float BackflipMinLaunchSpeed = 8f;

        public float BackflipPitchRateGain = 12f;
        public float BackflipPitchDamping = 3f;
        public float BackflipMaxPitchAssist = 35f;

        public float BackflipYawGain = 6f;
        public float BackflipYawDamping = 3f;

        public float BackflipRollGain = 10f;
        public float BackflipRollDamping = 4f;
        public float BackflipMaxLateralAssist = 18f;

        public float BackflipPredictionMaxTime = 2.5f;
        public float BackflipPredictionStep = 0.05f;
        public float BackflipGroundProbeRadius = 0.8f;
        public float BackflipGroundProbeHeight = 1.0f;
        public LayerMask BackflipLandingMask = ~0;

        public float BackflipExtraHangForce = 0.8f;
        public float BackflipExtraHangDuration = 0.18f;

        public float BackflipCruisePitchRateDeg = 240f;
        public float BackflipCorrectionStartAngle = 300f;
        public float BackflipYawRollCorrectionStartAngle = 320f;

        public float BackflipMinSolvePitchRateDeg = 140f;
        public float BackflipMaxSolvePitchRateDeg = 280f;

        public float BackflipTargetAngle = 360f;

        public float BackflipLandingCatchStartAngle = 40f;   // start catch when this much angle remains
        public float BackflipLandingCatchPitchDamping = 10f;

        public float BackflipLandingCatchYawGain = 8f;
        public float BackflipLandingCatchYawDamping = 4f;

        public float BackflipLandingCatchRollGain = 16f;
        public float BackflipLandingCatchRollDamping = 6f;
        public float BackflipLandingCatchMaxAssist = 18f;

        private float currentBackflipTargetAngle = 0f;
        private bool autoBackflipLandingCatchActive = false;

        private bool backflipArmed = false;
        private float backflipArmTime = -999f;
        private bool autoBackflipActive = false;
        private bool wasGroundedLastFrame = false;

        private Vector3 backflipLaunchForwardFlat;
        private float backflipAccumulatedAngle = 0f;
        private float backflipHangTimer = 0f;

        public float BackflipWheelieBlockTime = 0.6f;
        private float backflipWheelieBlockTimer = 0f;

        private bool IsBodyTouchingWithoutWheels() {
            return TouchingGround && !Grounded();
        }
        private void StartAutoBackflip() {
            autoBackflipActive = true;
            autoBackflipLandingCatchActive = false;
            backflipArmed = false;
            backflipAccumulatedAngle = 0f;
            backflipHangTimer = 0f;

            currentBackflipTargetAngle = BackflipTargetAngle;

            backflipLaunchForwardFlat = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            if (backflipLaunchForwardFlat.sqrMagnitude < 0.0001f)
                backflipLaunchForwardFlat = transform.forward;
            backflipLaunchForwardFlat.Normalize();
        }

        private void StopAutoBackflip() {
            bool wasBackflipRunning = autoBackflipActive || autoBackflipLandingCatchActive;

            autoBackflipActive = false;
            autoBackflipLandingCatchActive = false;
            backflipAccumulatedAngle = 0f;
            backflipHangTimer = 0f;

            if (wasBackflipRunning) {
                backflipWheelieBlockTimer = BackflipWheelieBlockTime;
            }
        }

        private float PredictBackflipTimeToGround() {
            Vector3 start = m_Rigidbody.worldCenterOfMass + Vector3.up * BackflipGroundProbeHeight;
            Vector3 velocity = m_Rigidbody.velocity;

            for (float t = BackflipPredictionStep; t <= BackflipPredictionMaxTime; t += BackflipPredictionStep) {
                Vector3 futurePos = start + velocity * t + 0.5f * Physics.gravity * t * t;

                RaycastHit hit;
                if (Physics.SphereCast(
                        futurePos,
                        BackflipGroundProbeRadius,
                        Vector3.down,
                        out hit,
                        BackflipGroundProbeHeight * 2f,
                        BackflipLandingMask,
                        QueryTriggerInteraction.Ignore)) {
                    return t;
                }
            }

            return -1f;
        }

        private bool UpdateAutoBackflip() {
            if (!autoBackflipActive)
                return false;

            if (IsBodyTouchingWithoutWheels()) {
                StopAutoBackflip();
                autoBackflipLandingCatchActive = false;
                return false;
            }

            if (Grounded()) {
                StopAutoBackflip();
                return false;
            }

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);

            // negative local X = backflip in your setup
            float currentBackflipRateDeg = Mathf.Max(0f, -localAngularVelocity.x * Mathf.Rad2Deg);
            backflipAccumulatedAngle += currentBackflipRateDeg * Time.fixedDeltaTime;
            backflipAccumulatedAngle = Mathf.Min(backflipAccumulatedAngle, currentBackflipTargetAngle);

            float timeToGround = PredictBackflipTimeToGround();
            if (timeToGround < 0f)
                timeToGround = 0.45f;

            timeToGround = Mathf.Max(0.12f, timeToGround);

            float remainingAngle = Mathf.Max(0f, currentBackflipTargetAngle - backflipAccumulatedAngle);

            float pitchBlend = Mathf.InverseLerp(
                currentBackflipTargetAngle - 60f,
                currentBackflipTargetAngle,
                backflipAccumulatedAngle
            );

            float solvePitchRateDeg = Mathf.Clamp(
                remainingAngle / timeToGround,
                BackflipMinSolvePitchRateDeg,
                BackflipMaxSolvePitchRateDeg
            );

            float desiredPitchRateDeg = Mathf.Lerp(
                BackflipCruisePitchRateDeg,
                solvePitchRateDeg,
                pitchBlend
            );

            float desiredPitchRateRad = -desiredPitchRateDeg * Mathf.Deg2Rad;

            float pitchAssist =
                (desiredPitchRateRad - localAngularVelocity.x) * BackflipPitchRateGain
                - localAngularVelocity.x * BackflipPitchDamping;

            pitchAssist = Mathf.Clamp(
                pitchAssist,
                -BackflipMaxPitchAssist,
                BackflipMaxPitchAssist
            );

            Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            if (flatForward.sqrMagnitude < 0.0001f)
                flatForward = backflipLaunchForwardFlat;
            else
                flatForward.Normalize();

            float lateralBlend = Mathf.InverseLerp(
                currentBackflipTargetAngle - 40f,
                currentBackflipTargetAngle,
                backflipAccumulatedAngle
            );

            float yawError = Vector3.SignedAngle(flatForward, backflipLaunchForwardFlat, Vector3.up);
            float yawAssist =
                (yawError * Mathf.Deg2Rad * BackflipYawGain
                - localAngularVelocity.y * BackflipYawDamping) * lateralBlend;

            yawAssist = Mathf.Clamp(
                yawAssist,
                -BackflipMaxLateralAssist,
                BackflipMaxLateralAssist
            );

            float rollAssist =
                ((-LatTilt) * BackflipRollGain
                - localAngularVelocity.z * BackflipRollDamping) * lateralBlend;

            rollAssist = Mathf.Clamp(
                rollAssist,
                -BackflipMaxLateralAssist,
                BackflipMaxLateralAssist
            );

            if (BackflipExtraHangForce > 0f &&
                backflipHangTimer < BackflipExtraHangDuration &&
                pitchBlend < 0.35f) {
                m_Rigidbody.AddForce(-Physics.gravity * BackflipExtraHangForce, ForceMode.Acceleration);
                backflipHangTimer += Time.fixedDeltaTime;
            }

            m_Rigidbody.AddRelativeTorque(pitchAssist, yawAssist, rollAssist, ForceMode.Acceleration);

            // hand over to landing catch near the end
            if (remainingAngle <= BackflipLandingCatchStartAngle) {
                autoBackflipActive = false;
                autoBackflipLandingCatchActive = true;
            }

            return true;
        }
        private bool UpdateAutoBackflipLandingCatch() {
            if (!autoBackflipLandingCatchActive)
                return false;

            if (IsBodyTouchingWithoutWheels()) {
                StopAutoBackflip();
                autoBackflipLandingCatchActive = false;
                return false;
            }

            if (Grounded()) {
                StopAutoBackflip();
                return false;
            }

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);

            // kill remaining pitch speed so it does not continue into extra flips
            float pitchAssist = -localAngularVelocity.x * BackflipLandingCatchPitchDamping;

            Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            if (flatForward.sqrMagnitude < 0.0001f)
                flatForward = backflipLaunchForwardFlat;
            else
                flatForward.Normalize();

            float yawError = Vector3.SignedAngle(flatForward, backflipLaunchForwardFlat, Vector3.up);
            float yawAssist =
                yawError * Mathf.Deg2Rad * BackflipLandingCatchYawGain
                - localAngularVelocity.y * BackflipLandingCatchYawDamping;

            yawAssist = Mathf.Clamp(
                yawAssist,
                -BackflipLandingCatchMaxAssist,
                BackflipLandingCatchMaxAssist
            );

            float rollAssist =
                (-LatTilt) * BackflipLandingCatchRollGain
                - localAngularVelocity.z * BackflipLandingCatchRollDamping;

            rollAssist = Mathf.Clamp(
                rollAssist,
                -BackflipLandingCatchMaxAssist,
                BackflipLandingCatchMaxAssist
            );

            m_Rigidbody.AddRelativeTorque(pitchAssist, yawAssist, rollAssist, ForceMode.Acceleration);
            return true;
        }
        #endregion

        #region Upside Down To Side Roll
        [Header("Upside Down To Side Roll")]
        public bool EnableUpsideDownToSideRoll = true;
        public float UpsideDownRollMaxSpeed = 8f;
        public float UpsideDownRollMinThrottle = 0.25f;
        public float UpsideDownDetectUpDot = -0.75f;     // roof-down detection
        public float UpsideDownReleaseUpDot = -0.25f;    // stop helper once it has rolled enough
        public float UpsideDownRollForce = 8f;
        public float UpsideDownRollDamping = 2.5f;
        public float UpsideDownRollMaxAssist = 10f;
        public float UpsideDownPitchDamping = 1.2f;
        public float UpsideDownYawDamping = 1.0f;
        public float UpsideDownPreferredDirection = 1f;  // 1 or -1

        private bool upsideDownRollActive = false;
        private float upsideDownRollDirection = 1f;

        private bool IsUpsideDownOnRoof() {
            if (!EnableUpsideDownToSideRoll)
                return false;

            // body touching ground, but wheels not touching ground
            if (!TouchingGround)
                return false;

            if (Grounded())
                return false;

            if (transform.up.y > UpsideDownDetectUpDot)
                return false;

            if (Mathf.Abs(Speed) > UpsideDownRollMaxSpeed)
                return false;

            if (Throttle < UpsideDownRollMinThrottle)
                return false;

            return true;
        }

        private void DoUpsideDownToSideRoll() {
            upsideDownRollActive = false;

            if (!IsUpsideDownOnRoof())
                return;

            upsideDownRollActive = true;

            //EnableRearSteer = false;
            EnableSideWheelieAssist = false;
            EnableSideWheelieCOMShift = false;

            sideWheelieCOMState = 0;
            sideWheelieReleaseTimer = 0f;
            sideWheelieIntentTimer = 0f;
            sideWheelieIntentDirection = 0;
            donutIntentTimer = 0f;

            // choose a stable roll direction
            if (Mathf.Abs(LatTilt) > 0.05f) {
                upsideDownRollDirection = Mathf.Sign(LatTilt);
            }
            else if (Mathf.Abs(xInput) > 0.2f) {
                upsideDownRollDirection = Mathf.Sign(xInput);
            }
            else if (Mathf.Abs(upsideDownRollDirection) < 0.5f) {
                upsideDownRollDirection = Mathf.Sign(UpsideDownPreferredDirection);
            }

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float pitchRate = localAngularVelocity.x;
            float yawRate = localAngularVelocity.y;
            float rollRate = localAngularVelocity.z;

            float rollAssist = upsideDownRollDirection * UpsideDownRollForce - rollRate * UpsideDownRollDamping;
            rollAssist = Mathf.Clamp(rollAssist, -UpsideDownRollMaxAssist, UpsideDownRollMaxAssist);

            float pitchAssist = -pitchRate * UpsideDownPitchDamping;
            float yawAssist = -yawRate * UpsideDownYawDamping;

            m_Rigidbody.AddRelativeTorque(pitchAssist, yawAssist, rollAssist, ForceMode.Acceleration);
        }
        #endregion

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

        public float SideSelfRightRecoveredEventWindow = 0.75f;
        public float SideSelfRightRecoveredEventRollThreshold = 0.35f;

        private bool sideSelfRightRecoveryEventPending = false;
        private float sideSelfRightRecoveryEventTimer = 0f;

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
        private void ArmSideSelfRightRecoveredEvent() {
            sideSelfRightRecoveryEventPending = true;
            sideSelfRightRecoveryEventTimer = SideSelfRightRecoveredEventWindow;
        }

        private void UpdateSideSelfRightRecoveredEvent() {
            if (!sideSelfRightRecoveryEventPending)
                return;

            sideSelfRightRecoveryEventTimer -= Time.fixedDeltaTime;

            bool recoveredUpright =
                Grounded() &&
                Mathf.Abs(Vector3.Dot(transform.right, Vector3.up)) < SideSelfRightRecoveredEventRollThreshold;

            if (recoveredUpright) {
                sideSelfRightRecoveryEventPending = false;
                sideSelfRightRecoveryEventTimer = 0f;
                OnSideSelfRightRecovered?.Invoke();
                return;
            }

            if (sideSelfRightRecoveryEventTimer <= 0f) {
                sideSelfRightRecoveryEventPending = false;
                sideSelfRightRecoveryEventTimer = 0f;
            }
        }
        private void FinishSideSelfRight(bool recovered) {
            bool wasLatched = sideSelfRightLatched;

            sideSelfRightActive = false;
            sideSelfRightLatched = false;
            sideSelfRightTimer = 0f;
            RestoreSelfRightWheelGrip();

            if (!wasLatched)
                return;

            if (recovered) {
                sideSelfRightRecoveryEventPending = false;
                sideSelfRightRecoveryEventTimer = 0f;
                OnSideSelfRightRecovered?.Invoke();
            }
            else {
                ArmSideSelfRightRecoveredEvent();
            }
        }
        private void DoSideSelfRight() {
            sideSelfRightActive = false;

            if (upsideDownRollActive)
                return;

            if (!sideSelfRightLatched) {
                if (!CanStartSideSelfRight()) {
                    sideSelfRightTimer = 0f;
                    RestoreSelfRightWheelGrip();
                    return;
                }

                sideSelfRightRecoveryEventPending = false;
                sideSelfRightRecoveryEventTimer = 0f;

                sideSelfRightLatched = true;
                sideSelfRightTimer = 0f;
                sideSelfRightLockedRoll = Vector3.Dot(transform.right, Vector3.up);
            }

            if ((!TouchingGround && !Grounded()) || Mathf.Abs(Speed) > SideSelfRightMaxSpeed || Throttle < SideSelfRightMinThrottle) { 
                bool recoveredUpright = Grounded() && Mathf.Abs(Vector3.Dot(transform.right, Vector3.up)) < 0.30f;
                FinishSideSelfRight(recoveredUpright);
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
                FinishSideSelfRight(true);
                return;
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

        [Header("Donut Yaw Rate Boost")]
        public int DonutYawRateBoostStartRound = 2;   // x rotations
        public float DonutBoostedYawRate = 4.2f;      // y amount
        public float DonutYawRateBoostLerpSpeed = 2.5f;

        private int donutCompletedRounds = 0;
        private float currentDonutTargetYawRate = 0f;

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
            if (!donutStuntActive || sideSelfRightActive) {
                donutStateActive = false;
                return;
            }

            donutStateActive = UpdateDonutIntent();
            bool donutActive = donutStateActive;

            EnableSideWheelieAssist = !donutActive;
            EnableSideWheelieCOMShift = !donutActive;

            ApplyDonutFriction(donutActive);

            if (!donutActive)
                return;

            float steerDir = Mathf.Sign(xInput);

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float yawRate = localAngularVelocity.y;

            float desiredYawRate = (donutCompletedRounds >= DonutYawRateBoostStartRound) ? DonutBoostedYawRate : DonutTargetYawRate;

            currentDonutTargetYawRate = Mathf.MoveTowards(
                currentDonutTargetYawRate,
                desiredYawRate,
                DonutYawRateBoostLerpSpeed * Time.fixedDeltaTime
            );

            float targetYawRate = steerDir * currentDonutTargetYawRate;

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

        public float SideWheelieCounterSteerRollReduction = 0.10f;
        public float SideWheelieCounterSteerExtraBalance = 4f;
        public float SideWheelieCounterSteerExtraDamping = 4f;
        public float SideWheelieCounterSteerCOMReturn = 0.35f;

        public bool sideWheeliAssistEnabled;

        private float GetCounterSteerFactor(bool leftGrounded, bool rightGrounded) {
            // left wheels on ground, right wheels in air -> right steer is the unstable direction
            if (leftGrounded && !rightGrounded)
                return Mathf.Clamp01(xInput);

            // right wheels on ground, left wheels in air -> left steer is the unstable direction
            if (rightGrounded && !leftGrounded)
                return Mathf.Clamp01(-xInput);

            return 0f;
        }
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

            Vector3 targetCOM = baseCOM;

            if (sideWheelieCOMState == -1 && leftWheeliCOM != null)
                targetCOM = leftWheeliCOM.localPosition;
            else if (sideWheelieCOMState == 1 && rightWheeliCOM != null)
                targetCOM = rightWheeliCOM.localPosition;

            bool leftGrounded = wheels.Count >= 4 && (wheels[0].wc.IsGrounded || wheels[2].wc.IsGrounded);
            bool rightGrounded = wheels.Count >= 4 && (wheels[1].wc.IsGrounded || wheels[3].wc.IsGrounded);

            float counterSteer = GetCounterSteerFactor(leftGrounded, rightGrounded);

            // when steering against the wheelie, bring COM slightly back toward base
            return Vector3.Lerp(targetCOM, baseCOM, counterSteer * SideWheelieCounterSteerCOMReturn);
        }

        private void UpdateCenterOfMass() {
            if (!useManualCenterOfMass)
                return;

            if (comBase != null)
                manualCenterOfMass = comBase.localPosition;

            if (EnableWheelieStunt && EnableWheelieHold && rearCOM != null) {
                SetCOM(rearCOM.localPosition);
                return;
            }

            if (EnableNoseWheelieStunt && EnableNoseWheelieHold && noseWheelieCOM != null) {
                Vector3 baseCOM = (comBase != null) ? comBase.localPosition : manualCenterOfMass;

                Vector3 targetNoseCOM = Vector3.Lerp(
                    noseWheelieCOM.localPosition,
                    baseCOM,
                    NoseWheelieHoldCOMBackToBase
                );

                SetCOM(Vector3.Lerp(
                    m_Rigidbody.centerOfMass,
                    targetNoseCOM,
                    Time.fixedDeltaTime * NoseWheelieCOMLerpSpeed
                ));

                wasNoseWheelieActiveLastFrame = true;
                return;
            }

            UpdateSideWheelieCOMState();

            Vector3 targetCOM = GetTargetCenterOfMass();

            float returnSpeed = wasNoseWheelieActiveLastFrame
                ? NoseWheelieCOMReturnSpeed
                : 8f;

            SetCOM(Vector3.Lerp(
                m_Rigidbody.centerOfMass,
                targetCOM,
                Time.fixedDeltaTime * returnSpeed
            ));

            wasNoseWheelieActiveLastFrame = false;
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

            float counterSteer = GetCounterSteerFactor(leftGrounded, rightGrounded);

            // reduce how extreme the wheelie stays when steering against it
            targetRoll = Mathf.MoveTowards(targetRoll, 0f, counterSteer * SideWheelieCounterSteerRollReduction);

            Vector3 localAngularVelocity = transform.InverseTransformVector(m_Rigidbody.angularVelocity);
            float rollRate = localAngularVelocity.z;

            float balanceForce = SideWheelieBalanceForce + counterSteer * SideWheelieCounterSteerExtraBalance;
            float damping = SideWheelieDamping + counterSteer * SideWheelieCounterSteerExtraDamping;

            float error = targetRoll - currentRoll;
            float assist = error * balanceForce - rollRate * damping;
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

        //private BodyPartsSwitcher bodyPartsSwitcher;

        private CarUIControl carUIControl;

        private AGS_MTC_EngineController engine;

        [Header("Setup")][SerializeField] public List<AGS_MTC_Wheel> wheels;

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

        public AGS_MTC_TransmissionType transmissionType;

        public float[] GearRatios = AGS_MTC_GearsManager.DefaultGears;

        public float LowGearRatio = AGS_MTC_GearsManager.DefaultLowGear;

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

        public AGS_MTC_FrictionSettings FrontFriction;

        public AGS_MTC_FrictionSettings RearFriction;

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
            
            cachedWheelieAngle = WheelingAngle;

            OnWheelieEnter.AddListener(() => { print("Stunt: -=> Wheeling Entered"); });
            OnWheelieExit.AddListener(() => { print("Stunt: -=> Wheeling Exit"); });
            OnSideWheelieEnter.AddListener(() => { print("Stunt: -=> Side Wheeling Entered"); });
            OnSideWheelieExit.AddListener(() => { print("Stunt: -=> Side Wheeling Exit"); });
            OnDonutEnter.AddListener(() => { print("Stunt: -=> Donut Entered"); });
            OnDonutExit.AddListener(() => { print("Stunt: -=> Donut Exit"); });
            OnDonutRoundComplete.AddListener(() => { print("Stunt: -=> Donut Round Completed"); });
            OnSideSelfRightEnter.AddListener(() => { print("Stunt: -=> side self right entered"); });
            OnSideSelfRightRecovered.AddListener(() => { print("Stunt: -=> Side self recovered"); });
            OnNoseWheelieEnter.AddListener(() => { print("Stunt: -=> Nose Wheelie Entered"); });
            OnNoseWheelieExit.AddListener(() => { print("Stunt: -=> Nose Wheelie Exit"); });
            OnAutoBackflipLanded.AddListener(() => { print("Stunt: -=> Auto Backflip Landed On All Tires"); });
            OnUserFrontFlip.AddListener((count) => { print("Stunt: -=> User Front Flip x" + count); });
            OnUserBackFlip.AddListener((count) => { print("Stunt: -=> User Back Flip x" + count); });

            if (comBase != null) {
                manualCenterOfMass = comBase.localPosition;
            }

            carUIControl = FindObjectOfType<CarUIControl>();
            m_Rigidbody = GetComponent<Rigidbody>();
            engine = GetComponent<AGS_MTC_EngineController>();
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
            AGS_MTC_CarController[] array = FindObjectsOfType<AGS_MTC_CarController>();
            foreach (AGS_MTC_CarController carController in array) {
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

            IsSlideThrottle = false;
            AGS_MTC_CarController[] array2 = FindObjectsOfType<AGS_MTC_CarController>();
            int num = 0;
            while (true) {
                if (num < array2.Length) {
                    AGS_MTC_CarController carController2 = array2[num];
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
            
            if (EnableAutoBackflip && other.CompareTag(BackflipLaunchTag)) {
                backflipArmed = true;
                backflipArmTime = Time.time;
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
            foreach (AGS_MTC_Wheel wheel in wheels) {
                wheel.wc.BrakeTorque = BrakeTorque;
                wheel.wc.MotorTorque = 0f;
            }
        }

        private void FixedUpdate() {
            Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
            Speed = localVelocity.z * 2.23f;
            AngularSpeed = m_Rigidbody.angularVelocity.magnitude;

            if (backflipWheelieBlockTimer > 0f) {
                backflipWheelieBlockTimer -= Time.fixedDeltaTime;
            }

            if (!vehicleIsActive) {
                foreach (AGS_MTC_Wheel wheel in wheels) {
                    wheel.wc.MotorTorque = 0f;
                    wheel.wc.BrakeTorque = BrakeTorque;
                }

                acceleration = (m_Rigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
                acceleration = transform.InverseTransformVector(acceleration);
                lastVelocity = m_Rigidbody.velocity;
                return;
            }

            UpdateNoseWheelieState();
            DoCarHandling();
            DoWheelieHoldAssist();
            DoNoseWheelieAssist();

            if (IsBodyTouchingWithoutWheels()) {
                StopAutoBackflip();
                autoBackflipLandingCatchActive = false;
                backflipArmed = false;
            }

            bool groundedNow = Grounded();

            if (backflipArmed && Time.time - backflipArmTime > BackflipArmMemory) {
                backflipArmed = false;
            }

            bool justLaunched = !groundedNow && wasGroundedLastFrame;

            if (EnableAutoBackflip &&
                justLaunched &&
                backflipArmed &&
                Mathf.Abs(Speed) >= BackflipMinLaunchSpeed) {
                StartAutoBackflip();
            }

            if (groundedNow && autoBackflipActive) {
                StopAutoBackflip();
            }

            wasGroundedLastFrame = groundedNow;

            DoUpsideDownToSideRoll();

            if (!upsideDownRollActive) {
                DoSideSelfRight();
            }

            UpdateSideWheelieIntent();
            UpdateCenterOfMass();

            if (PreventFromSideSliding) {
                PreventFromSideSlide();
            }

            DoAirForces();
            DoAntiroll();
            DoDonutAssist();
            DoSideWheelieAssist();
            UpdateSideSelfRightRecoveredEvent();
            UpdateStuntEvents();

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

        public void Update() {

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
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
            m_Rigidbody.isKinematic = true;
            
            Invoke("UnfreezeCar", 0.5f);
            carUIControl.SwitchFlipButton(Show: false);
        }

        private void RepairVehicle() {
            CarHealth = 100f;
        }

        public void RespawnCar() {
            if (!DontPreventFromSliding && !loadedOnOtherPlayerTrailer) {
                Debug.Log("Respawn Me Here...");
                Invoke("UnfreezeCar", 0.5f);
            }
        }

        private void UnfreezeCar() {
            m_Rigidbody.isKinematic = false;
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

            foreach (AGS_MTC_Wheel wheel in wheels) {
                if (wheel.wc == null || wheel.wc.wheelCollider == null)
                    return;
            }

            float effectiveFrontLateralAntiroll = FrontLateralAntiroll;
            float effectiveRearLateralAntiroll = RearLateralAntiroll;
            float effectiveLongitudinalAntiroll = LongitudinalAntiroll;

            if (EnableNoseWheelieHold) {
                effectiveLongitudinalAntiroll *= NoseWheelieLongitudinalAntirollMultiplier;
            }

            if (sideSelfRightActive) {
                if (sideSelfRightTimer < SideSelfRightPitchPhaseDuration)
                    return;

                effectiveFrontLateralAntiroll *= SideSelfRightAntirollMultiplier;
                effectiveRearLateralAntiroll *= SideSelfRightAntirollMultiplier;
                effectiveLongitudinalAntiroll *= SideSelfRightAntirollMultiplier;
            }

            float frontLeftCompression = wheels[0].wc.IsGrounded ? wheels[0].wc.Compression : 0f;
            float frontRightCompression = wheels[1].wc.IsGrounded ? wheels[1].wc.Compression : 0f;

            float frontAntirollForce =
                (frontLeftCompression - frontRightCompression) *
                effectiveFrontLateralAntiroll;

            if (wheels[0].wc.IsGrounded) {
                m_Rigidbody.AddForceAtPosition(
                    wheels[0].wc.transform.up * frontAntirollForce,
                    wheels[0].wc.transform.position
                );
            }

            if (wheels[1].wc.IsGrounded) {
                m_Rigidbody.AddForceAtPosition(
                    wheels[1].wc.transform.up * -frontAntirollForce,
                    wheels[1].wc.transform.position
                );
            }

            float rearLeftCompression = wheels[2].wc.IsGrounded ? wheels[2].wc.Compression : 0f;
            float rearRightCompression = wheels[3].wc.IsGrounded ? wheels[3].wc.Compression : 0f;

            float rearAntirollForce =
                (rearLeftCompression - rearRightCompression) *
                effectiveRearLateralAntiroll;

            if (wheels[2].wc.IsGrounded) {
                m_Rigidbody.AddForceAtPosition(
                    wheels[2].wc.transform.up * rearAntirollForce,
                    wheels[2].wc.transform.position
                );
            }

            if (wheels[3].wc.IsGrounded) {
                m_Rigidbody.AddForceAtPosition(
                    wheels[3].wc.transform.up * -rearAntirollForce,
                    wheels[3].wc.transform.position
                );
            }

            float frontAvgCompression =
                (frontLeftCompression + frontRightCompression) * 0.5f;

            float rearAvgCompression =
                (rearLeftCompression + rearRightCompression) * 0.5f;

            float longitudinalAntirollForce =
                (frontAvgCompression - rearAvgCompression) *
                effectiveLongitudinalAntiroll;

            Vector3 frontAxleCenter =
                (wheels[0].wc.transform.position + wheels[1].wc.transform.position) * 0.5f;

            Vector3 rearAxleCenter =
                (wheels[2].wc.transform.position + wheels[3].wc.transform.position) * 0.5f;

            float uprightFactor = Mathf.InverseLerp(0.75f, 1f, transform.up.y);

            m_Rigidbody.AddForceAtPosition(
                transform.up * longitudinalAntirollForce * uprightFactor,
                frontAxleCenter
            );

            m_Rigidbody.AddForceAtPosition(
                -transform.up * longitudinalAntirollForce * uprightFactor,
                rearAxleCenter
            );
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

            UpdateUserAirFlipEvents(flag);

            if (flag && UpdateAutoBackflip()) {
                return;
            }
            if (flag && UpdateAutoBackflipLandingCatch()) {
                return;
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
                        // TODO: notify front flip and backflip here
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
                    // TODO: notify roll over
                }
            }
            else {
                AngleCounter = 0f;
                prevForward = Vector3.zero;
            }
        }

        public void SetZeroFriction() {
            foreach (AGS_MTC_Wheel wheel in wheels) {
                wheel.wc.forwardFrictionCoefficient =
                    (wheel.wc.sideFrictionCoefficient = (wheel.wc.surfaceFrictionCoefficient = 0f));
                wheel.wc.UpdateFriction();
            }
        }

        public void SetDefaultFriction() {
            foreach (AGS_MTC_Wheel wheel in wheels) {
                wheel.wc.forwardFrictionCoefficient =
                    (wheel.wc.sideFrictionCoefficient = (wheel.wc.surfaceFrictionCoefficient = 1f));
                wheel.wc.UpdateFriction();
            }
        }

        public float GetMaxTorque() {
            AGS_MTC_PowerPart part = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.EngineBlock, EngineBlockStage);
            AGS_MTC_PowerPart part2 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Head, HeadStage);
            AGS_MTC_PowerPart part3 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Valvetrain, ValvetrainStage);
            AGS_MTC_PowerPart part4 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Turbo, TurboStage);
            AGS_MTC_PowerPart part5 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Blower, BlowerStage);
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
            AGS_MTC_PowerPart part = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.EngineBlock, EngineBlockStage);
            AGS_MTC_PowerPart part2 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Head, HeadStage);
            AGS_MTC_PowerPart part3 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Valvetrain, ValvetrainStage);
            AGS_MTC_PowerPart part4 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Weight, WeightStage);
            AGS_MTC_PowerPart part5 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Weight, DurabilityStage);
            AGS_MTC_PowerPart part6 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Turbo, TurboStage);
            AGS_MTC_PowerPart part7 = AGS_MTC_PowerParts.GetPart(vehicleType, AGS_MTC_PowerPartType.Blower, BlowerStage);
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
            if (engine)
                engine.TopGear = Mathf.Max(0.5f, 9f - 4f * ((num6 - 80f) / 280f));
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

        private void SetDiffLock() {
            foreach (AGS_MTC_Wheel wheel in wheels) {
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

            bool abruptReverseToForward =
                EnableWheelieStunt &&
                backflipWheelieBlockTimer <= 0f &&
                !IsOtherStuntBlockingWheelie() &&
                Grounded() &&
                Speed <= -ReverseSnapMinReverseSpeed &&
                yInput >= 0.95f;

            float targetBrake = 0f;

            if (!abruptReverseToForward && (Speed > 1f || transmissionType == AGS_MTC_TransmissionType.Manual)) {
                targetBrake = -Mathf.Clamp(yInput, -1f, 0f);
            }

            Braking = abruptReverseToForward
                ? 0f
                : Mathf.MoveTowards(Braking, targetBrake, Time.fixedDeltaTime * 50f);

            Braking = Mathf.Max(Braking, ExtremeBraking);

            float throttleInput = abruptReverseToForward ? 1f : yInput;

            if (!abruptReverseToForward &&
                ((Speed > 1f && Grounded()) || transmissionType == AGS_MTC_TransmissionType.Manual)) {
                throttleInput = Mathf.Clamp(yInput, 0f, 1f);
            }

            if (transmissionType == AGS_MTC_TransmissionType.Manual && engine.ReverseGear) {
                throttleInput = -throttleInput;
            }

            Throttle = throttleInput;

            if (abruptReverseToForward) {
                Vector3 localVel = transform.InverseTransformDirection(m_Rigidbody.velocity);
                localVel.z = Mathf.Max(localVel.z, ReverseSnapForwardBoostSpeed / 2.23f);
                m_Rigidbody.velocity = transform.TransformDirection(localVel);

                m_Rigidbody.AddForce(
                    transform.forward * ReverseSnapForwardAssist,
                    ForceMode.Acceleration
                );

                if (!reverseSnapWasTriggering) {
                    EnableWheelieHold = true;
                    cachedWheelieAngle = WheelingAngle;
                    currentWheelieAngle = 0f;
                    wheelieEntrySpeedGraceTimer = WheelieEntrySpeedGraceTime;
                    wheelieNoThrottleTimer = 0f;
                }
            }

            reverseSnapWasTriggering = abruptReverseToForward;

            float leveledMaxSpeed = Mathf.Max(1f, LeveledMaxSpeed);

            float rPM = engine.RPM;
            float maxRpm = engine.maxRpm;
            float gearRatio = engine.Gears[engine.Gear];
            float topGear = engine.TopGear;
            float lowGearMul = (!LowGear) ? 1f : LowGearRatio;
            float speedFactor = Mathf.Clamp01(1f - Mathf.Abs(Speed) / leveledMaxSpeed);
            float driveMul = (FWD && RWD) ? 1f : 2f;

            CurrentTorque =
                LeveledMaxTorque *
                DynoCurve.Evaluate(rPM / maxRpm) *
                gearRatio *
                topGear *
                lowGearMul *
                speedFactor *
                driveMul;

            if (float.IsNaN(CurrentTorque)) {
                CurrentTorque = 0f;
            }

            if (Throttle == 0f) {
                CurrentTorque = 0f;
            }

            if (engine != null && engine.NeutralGear) {
                CurrentTorque = 0f;
            }

            bool wheelieUseNormalRearSteer =
                EnableWheelieHold &&
                RearAxleGrounded() &&
                !FrontAxleGrounded();

            bool noseWheelieActive =
                EnableNoseWheelieHold &&
                FrontAxleGrounded();

            float speed01 = Mathf.Clamp01(Mathf.Abs(Speed) / leveledMaxSpeed);
            float steerLimitT = Mathf.Clamp01(1f - speed01 * SteerLimitOnSpeed);

            float targetSteering = Mathf.Lerp(
                maxSteeringAngle * 0.1f * xInput,
                maxSteeringAngle * xInput,
                steerLimitT
            );

            float steeringMoveSpeed = noseWheelieActive
                ? NoseWheelieSteerReturnSpeed
                : 100f;

            Steering = Mathf.MoveTowards(
                Steering,
                targetSteering,
                Time.fixedDeltaTime * steeringMoveSpeed
            );

            float appliedSteering = Steering;

            if (noseWheelieActive) {
                appliedSteering = Mathf.Clamp(
                    Steering * NoseWheelieSteerMultiplier,
                    -maxSteeringAngle,
                    maxSteeringAngle
                );
            }

            float rearSteerT = Mathf.Clamp01(Mathf.Abs(Speed) / RearSteerFadeOutSpeed);

            InverseSteerMultiplier = EnableRearSteer
                ? Mathf.Lerp(RearSteerLowSpeedMultiplier, RearSteerHighSpeedMultiplier, rearSteerT)
                : 0f;

            if (wheelieUseNormalRearSteer) {
                InverseSteerMultiplier = Mathf.Max(
                    InverseSteerMultiplier,
                    WheelieRearSteerMultiplier
                );
            }

            if (SteeringWheel != null) {
                SteeringWheel.localEulerAngles = new Vector3(
                    0f,
                    0f,
                    Mathf.LerpUnclamped(
                        SteeringWheelMaxAngle,
                        0f,
                        appliedSteering / maxSteeringAngle + 1f
                    )
                );
            }

            currentBrakeTorque = BrakeTorque * Braking;

            for (int i = 0; i < wheels.Count; i++) {
                AGS_MTC_Wheel wheel = wheels[i];

                if (wheel.wc == null || wheel.wc.wheelCollider == null) {
                    break;
                }

                if (wheel.wc.wheelCollider.OppositeWheel == null) {
                    SetupCounterWheels();
                }

                bool isFrontWheel = i <= 1;
                bool isRearWheel = i >= 2;

                float motorTorque = (!wheel.power) ? 0f : CurrentTorque * Throttle;
                float brakeTorque = currentBrakeTorque;

                // -------------------------
                // Nose wheelie drive
                // -------------------------
                if (noseWheelieActive) {
                    if (isFrontWheel) {
                        // Keep this 0 if you do not want forward drive.
                        motorTorque *= NoseWheelieFrontDriveMultiplier;
                    }
                    else if (isRearWheel) {
                        motorTorque = 0f;
                    }
                }
                else if (wheelieUseNormalRearSteer) {
                    if (isFrontWheel) {
                        motorTorque *= WheelieFrontDriveMultiplier;
                    }
                    else if (isRearWheel) {
                        motorTorque *= WheelieRearDriveMultiplier;
                    }
                }

                // -------------------------
                // Steering
                // -------------------------
                wheel.wc.Steer = 0f;

                if (noseWheelieActive) {
                    if (isFrontWheel && wheel.steer) {
                        wheel.wc.Steer = appliedSteering;
                    }
                    else {
                        wheel.wc.Steer = 0f;
                    }
                }
                else {
                    if (wheel.steer) {
                        wheel.wc.Steer = appliedSteering;
                    }

                    if (wheel.inverseSteer) {
                        if (wheelieUseNormalRearSteer) {
                            wheel.wc.Steer = appliedSteering;
                        }
                        else {
                            wheel.wc.Steer =
                                -appliedSteering *
                                InverseSteerMultiplier;
                        }
                    }
                }

                // -------------------------
                // Brake
                // -------------------------
                if (noseWheelieActive) {
                    if (isFrontWheel) {
                        // Keep wheel visual response, but do not let brakes fight the pose lock.
                        brakeTorque = 0f;
                    }
                    else if (isRearWheel) {
                        motorTorque = 0f;
                        brakeTorque = 0f;
                    }
                }

                if (wheel.handbrake) {
                    brakeTorque =
                        BrakeTorque *
                        Mathf.Max(Handbraking * 3f, Braking);
                }

                if (CurrentTorque * Throttle == 0f &&
                    Braking == 0f &&
                    Handbraking == 0f &&
                    ExtremeBraking == 0f) {
                    brakeTorque =
                        BrakeTorque / 2f *
                        RollingResistance;
                }

                wheel.wc.MotorTorque = motorTorque;
                wheel.wc.BrakeTorque = brakeTorque;
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

            if (flag && !(col.impulse.magnitude < 100f)) {
                float num = Mathf.InverseLerp(0f, MaximumHitDamageForce, col.impulse.magnitude);
                float num2 = MaximumHitDamage * num;
                num2 *= 1f - DurabilityStage * 0.01f;
                
                CarHealth = Mathf.Clamp(CarHealth - num2, 0f, 100f);
            }
        }

        private void DoWaterDamage(float Value) {
            CarHealth = Mathf.Clamp(CarHealth - Value, 0f, 100f);
            // TODO: notify water damage
            AGS_MTC_CameraController.Instance.Shake();
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
            // TODO: notify overheating
            AGS_MTC_CameraController.Instance.Shake();
        }
    }
}