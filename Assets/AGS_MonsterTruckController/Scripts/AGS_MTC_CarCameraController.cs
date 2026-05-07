using AGS_MonsterTruckControl;
using Cinemachine;
using UnityEngine;

public class AGS_MTC_CarCameraController : MonoBehaviour {
    [Header("Truck Reference")]
    public Rigidbody truckRigidbody;
    public AGS_MTC_CarController carController;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera vCamFollow;
    public CinemachineVirtualCamera vCamDonutAndRecover;
    public CinemachineVirtualCamera vCamWheelie;
    public CinemachineVirtualCamera vCamBackflip;

    [Header("Reverse Settings")]
    [Tooltip("How fast the camera orbits around the truck")]
    public float orbitSpeed = 120f;

    [Tooltip("Speed threshold to detect reverse")]
    public float reverseSpeedThreshold = 0.5f;

    // Internal
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineComposer composer;
    private bool isReversing = false;

    // Orbit angles
    private const float FORWARD_ANGLE = 0f;
    private const float REVERSE_ANGLE = 180f;
    private const float DEAD_ZONE = 0.5f;

    // NEW: track whether Follow cam was the selected cam last frame
    private bool followWasSelectedLastFrame = true;

    private void Start() {
        orbitalTransposer = vCamFollow.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        if (orbitalTransposer == null) {
            Debug.LogError("AGS_MTC_CarCameraController: No Orbital Transposer found on vCamFollow. Set Body to Orbital Transposer.");
            enabled = false;
            return;
        }

        composer = vCamFollow.GetCinemachineComponent<CinemachineComposer>();
        if (composer == null) {
            Debug.LogError("AGS_MTC_CarCameraController: No Composer found on vCamFollow. Set Aim to Composer.");
            enabled = false;
            return;
        }
    }

    private void Update() {
        DetectReverse();

        // Choose which camera should be active this frame
        bool followSelectedNow = UpdateCameraPriorityAndReturnIfFollowIsSelected();

        // Only run orbit logic when Follow camera is the currently selected camera
        if (followSelectedNow) {
            // If we just switched back to Follow, snap to a safe angle immediately
            if (!followWasSelectedLastFrame) {
                SnapFollowOrbit();
            }

            OrbitCamera();
        }

        followWasSelectedLastFrame = followSelectedNow;
    }

    private void DetectReverse() {
        Vector3 localVelocity = truckRigidbody.transform.InverseTransformDirection(truckRigidbody.velocity);

        if (localVelocity.z < -reverseSpeedThreshold) isReversing = true;
        else if (localVelocity.z > reverseSpeedThreshold) isReversing = false;
    }

    private void OrbitCamera() {
        float targetAngle = isReversing ? REVERSE_ANGLE : FORWARD_ANGLE;

        float currentAngle = orbitalTransposer.m_XAxis.Value % 360f;
        if (currentAngle < 0f) currentAngle += 360f;

        float difference = targetAngle - currentAngle;
        if (difference > 180f) difference -= 360f;
        if (difference < -180f) difference += 360f;

        if (Mathf.Abs(difference) > DEAD_ZONE) {
            float step = orbitSpeed * Time.deltaTime;

            if (Mathf.Abs(difference) < step) orbitalTransposer.m_XAxis.Value = targetAngle;
            else orbitalTransposer.m_XAxis.Value += Mathf.Sign(difference) * step;
        }
    }

    // NEW: snap the follow cam orbit to avoid "return swing" after backflip/donut/etc.
    private void SnapFollowOrbit() {
        float targetAngle = isReversing ? REVERSE_ANGLE : FORWARD_ANGLE;

        orbitalTransposer.m_XAxis.Value = targetAngle;
        orbitalTransposer.m_XAxis.m_InputAxisValue = 0f; // extra safety in case you use input later
        //orbitalTransposer.m_XAxis.m_Velocity = 0f;       // important: kills stored velocity so it won't "swing"
    }

    // UPDATED: returns true if Follow camera should be the active one
    private bool UpdateCameraPriorityAndReturnIfFollowIsSelected() {

        // IMPORTANT: make sure you use the correct public backflip state.
        // In your earlier code, autoBackflipActive was private, so camera can't see it.
        // Use carController.isDoingBackflip (the public bool you added) if available.
        bool backflip = carController.autoBackflipActive; // <-- prefer this
        // If you really do have carController.autoBackflipActive public, you can use it instead.

        if (backflip) {
            vCamBackflip.Priority = 50;
            vCamWheelie.Priority = 10;
            vCamDonutAndRecover.Priority = 10;
            vCamFollow.Priority = 10;
            return false;
        }

        if (carController.isWheeling) {
            vCamWheelie.Priority = 40;
            vCamDonutAndRecover.Priority = 10;
            vCamFollow.Priority = 10;
            vCamBackflip.Priority = 10;
            return false;
        }

        if (carController.isDonuting || carController.sideSelfRightActive) {
            vCamDonutAndRecover.Priority = 40;
            vCamWheelie.Priority = 10;
            vCamFollow.Priority = 10;
            vCamBackflip.Priority = 10;
            return false;
        }

        // Default = follow
        vCamFollow.Priority = 40;
        vCamDonutAndRecover.Priority = 10;
        vCamWheelie.Priority = 10;
        vCamBackflip.Priority = 10;
        return true;
    }
}