using AGS_MonsterTruckControl;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class AGS_MTC_CameraController : MonoBehaviour {
    public enum CameraMode {
        Free,
        Follow,
        Side,
        FirstPerson,
        Ragdoll,
        WinchTargetSelection,
        Photo,
        Cinematic
    }

    // Higher = camera follows faster.
    // Lower = smoother/slower follow.
    public float FollowPositionDamping = 8f;

    // Optional vertical damping multiplier.
    // Lower than 1 = softer vertical movement.
    public float FollowHeightDampingMultiplier = 0.75f;

    [Header("Normal Follow Camera")]
    public bool UseCameraControllerFollowValues = true;

    // Horizontal orbit around truck.
    // 0 = behind truck, 90 = side, 180 = front.
    public float NormalFollowXAngle = 0f;

    // Vertical camera angle.
    // Higher value = camera looks more downward toward the ground.
    // Lower value = flatter chase camera.
    public float NormalFollowYAngle = 20f;

    public float NormalFollowDistance = 5f;

    // Moves the target point up/down.
    // This is not the same as look angle.
    public float NormalFollowHeightOffset = 0f;

    public float NormalFollowDistanceLerpSpeed = 10f;

    [Header("Wheelie Camera")]
    public bool LockCameraDuringWheelie = true;

    // Higher value = more downward look during wheelie.
    public float WheelieCameraYAngle = 16f;

    public float WheelieCameraDistance = 7f;
    public float WheelieCameraHeightOffset = 1.25f;

    [Header("Nose Wheelie Camera")]
    public bool LockCameraDuringNoseWheelie = true;

    // Side angle around truck.
    // 90 = right side, -90 = left side.
    public float NoseWheelieCameraSideAngle = 90f;

    // Higher value = camera looks more downward toward the ground.
    public float NoseWheelieCameraYAngle = 16f;

    public float NoseWheelieCameraDistance = 7f;
    public float NoseWheelieCameraHeightOffset = 0f;

    [Header("Donut Camera")]
    public bool LockCameraDuringDonut = true;

    // Higher value = camera looks more downward toward the ground.
    public float DonutCameraYAngle = 12f;

    public float DonutCameraDistance = 7f;
    public float DonutCameraHeightOffset = 0f;

    private bool donutCameraLocked;
    private float donutLockedXAngle;

    [Header("Self Recovery Camera")]
    public bool LockCameraDuringRecover = true;

    public static AGS_MTC_CameraController Instance;

    [HideInInspector] public Transform forcedTarget;

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

    [HideInInspector] public Transform SelectedWinchTarget;
    [HideInInspector] public Transform Ragdoll;

    private float DistanceCam;
    [HideInInspector] public float DistanceCamTarget;

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

    public AGS_MTC_CarController carController;

    private Transform target {
        get {
            if (forcedTarget != null)
                return forcedTarget;

            if (carController != null)
                return carController.transform;

            return null;
        }
    }

    public AGS_MTC_CameraController() {
        if (Instance == null)
            Instance = this;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {

        DistanceCamTarget = DistanceStart;
        DistanceCam = DistanceStart;

        AngleX = XStart;
        TargetYAngle = YStart;
        desiredYAngle = YStart;
    }

    private bool IsDonutCameraActive() {
        if (carController == null)
            return false;

        return carController.donutStuntActive
            && carController.donutIntentTimer > carController.donutIntentTime
            && Mathf.Abs(carController.xInput) > 0.9f
            && Mathf.Abs(carController.yInput) > 0.9f;
    }

    private bool IsRecoverCameraActive() {
        if (carController == null)
            return false;

        return carController.sideSelfRightActive &&
               Mathf.Abs(carController.yInput) > 0.9f;
    }

    private bool IsWheelieCameraActive() {
        if (carController == null)
            return false;

        return carController.EnableWheelieHold;
    }

    public void Shake() {
        ShakeAmount = 1f;
    }

    public string SwitchCamera() {
        if (cameraMode == CameraMode.Follow) {
            cameraMode = CameraMode.Free;
            return "Free camera";
        }

        if (cameraMode == CameraMode.Free) {
            cameraMode = CameraMode.FirstPerson;
            return "First Person";
        }

        if (cameraMode == CameraMode.FirstPerson) {
            cameraMode = CameraMode.Cinematic;
            GenerateCinematicCameraPoint();
            return "Cinematic Camera";
        }

        cameraMode = CameraMode.Follow;
        return "Follow Camera";
    }

    private void GenerateCinematicCameraPoint() {
        if (target == null)
            return;

        CinematicCameraPoint = target.position + Random.insideUnitSphere * 40f;
        CinematicCameraPoint.y += 100f;
        HeightAboveGround = Random.Range(1, 10);

        if (Physics.Raycast(CinematicCameraPoint, Vector3.down, out RaycastHit hitInfo)) {
            CinematicCameraPoint.y = hitInfo.point.y + HeightAboveGround;
        }
    }

    public void SetCameraPos(float X, float Y, float Distance) {
        AngleX = X;
        TargetYAngle = Y;
        desiredYAngle = Y;
        DistanceCamTarget = Distance;
    }

    public void SetWinchCamera() {
        cameraModeBeforeWinchMode = cameraMode;
        cameraMode = CameraMode.WinchTargetSelection;
    }

    private void OnDisable() {
        cameraMode = CameraMode.Follow;
    }

    public void SetRagdollCamera() {
        cameraModeBeforeRagdollMode = cameraMode;
        cameraMode = CameraMode.Ragdoll;
    }

    public void SetSideCamera() {
        cameraModeBeforeWinchMode = cameraMode;
        cameraMode = CameraMode.Side;
    }

    public void GetCameraBack() {
        if (cameraMode == CameraMode.WinchTargetSelection)
            cameraMode = cameraModeBeforeWinchMode;

        if (cameraMode == CameraMode.Side)
            cameraMode = cameraModeBeforeWinchMode;

        if (cameraMode == CameraMode.Ragdoll)
            cameraMode = cameraModeBeforeRagdollMode;

        SelectedWinchTarget = null;
    }

    private void ToggleSlowMo() {
        SlowMo = !SlowMo;
        Time.timeScale = SlowMo ? 0.3f : 1f;
    }

    private void LateUpdate() {

        if (target == null)
            return;

        if (CrossPlatformInputManager.GetButtonDown("Swipe"))
            Swiping = true;

        if (Input.touchCount == 0)
            Swiping = false;

        ShakeAmount = Mathf.MoveTowards(
            ShakeAmount,
            0f,
            Time.deltaTime * 4f
        );

        if (Input.GetKeyDown(SlowMoToggleButton))
            ToggleSlowMo();

        switch (cameraMode) {
            case CameraMode.Ragdoll:
                break;

            case CameraMode.Free:
                DoFreeNavigation();
                DistanceCamTarget = Mathf.Clamp(
                    DistanceCamTarget - Input.GetAxis("Mouse ScrollWheel") * 3f,
                    MinDistance,
                    MaxDistance
                );
                DoSphereCam(false);
                break;

            case CameraMode.Photo:
                DoFreeNavigation();
                DistanceCamTarget = Mathf.Clamp(
                    DistanceCamTarget - Input.GetAxis("Mouse ScrollWheel") * 3f,
                    MinDistance,
                    MaxDistance * 2f
                );
                DoSphereCam(false);
                break;

            case CameraMode.Follow:
                DoFollowCamera();
                break;

            case CameraMode.FirstPerson:
                if (carController != null) {
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        target.transform.rotation,
                        FirstPersonDamping * Time.deltaTime
                    );

                    transform.position = carController.FirstPersonPoint.position;
                }
                break;

            case CameraMode.WinchTargetSelection:
                DoWinchTargetSelectionCamera();
                break;

            case CameraMode.Cinematic:
                DoCinematicCamera();
                break;
        }
    }

    private void DoFollowCamera() {
        if (carController == null)
            return;

        bool donutCam = LockCameraDuringDonut && IsDonutCameraActive();
        bool recoverCam = LockCameraDuringRecover && IsRecoverCameraActive();

        bool noseWheelieCam = LockCameraDuringNoseWheelie && carController.EnableNoseWheelieHold;

        bool wheelieCam =
            LockCameraDuringWheelie &&
            IsWheelieCameraActive();

        if (noseWheelieCam) {
            DoNoseWheelieCamera();
            return;
        }

        if (donutCam || recoverCam) {
            DoDonutStyleCamera();
            return;
        }

        if (wheelieCam) {
            DoWheelieCamera();
            return;
        }

        DoNormalFollowCamera();
    }

    private void DoNormalFollowCamera() {
        donutCameraLocked = false;

        if (UseCameraControllerFollowValues) {
            // Assign every frame for live Inspector tuning.
            AngleX = NormalFollowXAngle;
            desiredYAngle = NormalFollowYAngle;
            DistanceCamTarget = NormalFollowDistance;

            if (ForceRearView ||
                (carController.Speed < -10f && carController.WheelsOffTheGround == 0)) {
                AngleX = 180f;
            }

            MoveTargetYAngleToDesired();
            DoSphereCam(false, NormalFollowHeightOffset);
        }
        else {
            if (carController.Speed >= 0f) {
                AngleX = 0f;
            }

            if (ForceRearView ||
                (carController.Speed < -10f && carController.WheelsOffTheGround == 0)) {
                AngleX = 180f;
            }

            desiredYAngle = carController.FollowYAngle;
            DistanceCamTarget = carController.FollowDistance;

            MoveTargetYAngleToDesired();
            DoSphereCam(false, 0f);
        }
    }

    private void DoDonutStyleCamera() {
        if (!donutCameraLocked) {
            donutCameraLocked = true;

            // Original donut/recovery behavior:
            // lock camera at current camera X angle once.
            donutLockedXAngle = CurrentXAngle;
        }

        AngleX = 0f;

        // Higher = more downward look angle.
        desiredYAngle = DonutCameraYAngle;

        DistanceCamTarget = DonutCameraDistance;

        MoveTargetYAngleToDesired();
        DoSphereCam(true, DonutCameraHeightOffset);
    }
    private void DoNoseWheelieCamera() {
        if (!donutCameraLocked) {
            donutCameraLocked = true;

            // Nose wheelie side view.
            // 90 = right side, -90 = left side.
            donutLockedXAngle = target.eulerAngles.y + NoseWheelieCameraSideAngle;
        }

        AngleX = 0f;

        // Higher = more downward look angle.
        desiredYAngle = NoseWheelieCameraYAngle;

        DistanceCamTarget = NoseWheelieCameraDistance;

        MoveTargetYAngleToDesired();
        DoSphereCam(true, NoseWheelieCameraHeightOffset);
    }

    private void DoWheelieCamera() {
        donutCameraLocked = false;

        AngleX = 0f;

        // Y angle controls downward look angle.
        desiredYAngle = WheelieCameraYAngle;
        DistanceCamTarget = WheelieCameraDistance;

        MoveTargetYAngleToDesired();
        DoSphereCam(false, WheelieCameraHeightOffset);
    }

    private void MoveTargetYAngleToDesired() {
        bool blocked = Physics.CheckSphere(transform.position, 0.7f);

        if (Mathf.Abs(TargetYAngle - desiredYAngle) > 3f && !blocked) {
            TargetYAngle = Mathf.MoveTowards(
                TargetYAngle,
                desiredYAngle,
                Time.deltaTime * 50f
            );
        }
        else if (!blocked) {
            TargetYAngle = desiredYAngle;
        }
    }

    private void DoWinchTargetSelectionCamera() {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position + Vector3.up * WinchTargetSelectionHeight,
            Time.deltaTime * CameraMovingSpeed
        );

        if (SelectedWinchTarget != null) {
            Quaternion targetRotation =
                Quaternion.LookRotation(SelectedWinchTarget.position - transform.position);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 2f
            );
        }
    }

    private void DoCinematicCamera() {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            CinematicCameraPoint + Random.insideUnitSphere * 5f,
            ref movingSpeed,
            Time.deltaTime * 100f
        );

        _height = Mathf.MoveTowards(
            _height,
            HeightAboveGround,
            Time.deltaTime
        );

        if (Physics.Raycast(
                transform.position + Vector3.up * 10f,
                Vector3.down,
                out RaycastHit hitInfo
            ) &&
            hitInfo.collider.GetType() == typeof(TerrainCollider)) {
            transform.position = hitInfo.point + Vector3.up * _height;
        }

        if (Physics.Raycast(
                transform.position,
                target.position - transform.position,
                out hitInfo
            ) &&
            hitInfo.collider.transform.root != target &&
            hitInfo.collider.GetType() == typeof(TerrainCollider)) {
            GenerateCinematicCameraPoint();
        }

        transform.LookAt(target);

        Vector3 targetFlat = target.position;
        Vector3 cinematicFlat = new Vector3(
            CinematicCameraPoint.x,
            target.position.y,
            CinematicCameraPoint.z
        );

        if (Vector3.Distance(targetFlat, cinematicFlat) > 30f)
            GenerateCinematicCameraPoint();
    }

    private void DoFreeNavigation() {
        if (Swiping) {
            if (Input.touchCount == 1) {
                AngleX += Input.GetTouch(0).deltaPosition.x / 10f * SwipeSpeed;

                float verticalSwipe =
                    Input.GetTouch(0).deltaPosition.y / 10f * SwipeSpeed;

                if (!CameraDislocated ||
                    (CameraDislocated && verticalSwipe < 0f)) {
                    TargetYAngle -= verticalSwipe;
                }

                desiredYAngle = TargetYAngle;
            }

            if (Input.touchCount == 2) {
                Vector2 previousTouch0 =
                    Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;

                Vector2 previousTouch1 =
                    Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

                float previousMagnitude =
                    (previousTouch0 - previousTouch1).magnitude;

                float currentMagnitude =
                    (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

                float deltaMagnitude = previousMagnitude - currentMagnitude;

                DistanceCamTarget +=
                    deltaMagnitude *
                    Time.deltaTime /
                    2f *
                    ScrollSpeed;
            }
        }

        bool blocked = Physics.CheckSphere(transform.position, 0.7f);

        if ((Mathf.Abs(TargetYAngle - desiredYAngle) < 3f && !blocked) ||
            desiredYAngle == 0f) {
            desiredYAngle = TargetYAngle;
        }

        if (Mathf.Abs(TargetYAngle - desiredYAngle) > 3f && !blocked) {
            TargetYAngle = Mathf.MoveTowards(
                TargetYAngle,
                desiredYAngle,
                Time.deltaTime * 50f
            );
        }
    }

    private void FixedUpdate() {
        if (target == null || cameraMode != CameraMode.Ragdoll)
            return;

        Vector3 ragdollCamPos =
            Ragdoll.position -
            target.transform.forward * 2f +
            Vector3.up * 2f;

        transform.position = Vector3.Lerp(
            transform.position,
            ragdollCamPos,
            Time.deltaTime * 10f
        );

        transform.LookAt(Ragdoll, transform.up);
    }

    private void DoSphereCam(bool lockXAngle) {
        DoSphereCam(lockXAngle, 0f);
    }

    private void DoSphereCam(bool lockXAngle, float targetHeightOffset) {
        TargetYAngle = Mathf.Clamp(TargetYAngle, -45f, YMax);
        AngleY = TargetYAngle;

        DistanceCam = Mathf.Lerp(
            DistanceCam,
            DistanceCamTarget,
            NormalFollowDistanceLerpSpeed * Time.deltaTime
        );

        bool atLeastOneWheelGrounded = false;

        if (carController != null &&
            carController.WheelsOffTheGround < carController.wheels.Count) {
            atLeastOneWheelGrounded = true;
        }

        if (atLeastOneWheelGrounded && !lockXAngle) {
            float targetYaw = target.transform.eulerAngles.y;

            CurrentXAngle = Mathf.LerpAngle(
                CurrentXAngle,
                targetYaw,
                RotationDamping * Time.deltaTime
            );
        }
        else if (lockXAngle) {
            CurrentXAngle = donutLockedXAngle;
        }

        float bodyPitchAngle;

        if (atLeastOneWheelGrounded) {
            bodyPitchAngle = target.transform.eulerAngles.x;
        }
        else {
            bodyPitchAngle = 0f;
        }

        if (AngleX == 180f)
            bodyPitchAngle = -bodyPitchAngle;

        if (cameraMode == CameraMode.Follow) {
            CurrentYAngle = Mathf.LerpAngle(
                CurrentYAngle,
                bodyPitchAngle,
                HeightDamping * Time.deltaTime
            );
        }

        Vector3 shakeOffset =
            Random.onUnitSphere *
            ShakeAmount *
            ShakeAmplitude;

        Quaternion targetRotation = Quaternion.Euler(
            CurrentYAngle + AngleY,
            CurrentXAngle + AngleX,
            0f
        );

        Vector3 targetPos =
            target.transform.position +
            Vector3.up * targetHeightOffset;

        Vector3 desiredPosition =
            targetPos -
            targetRotation * Vector3.forward * DistanceCam;

        int safety = 0;

        while (Physics.CheckSphere(desiredPosition, 0.5f) && safety < 20) {
            safety++;

            TargetYAngle += 1f;
            AngleY = TargetYAngle;

            targetRotation = Quaternion.Euler(
                CurrentYAngle + AngleY,
                CurrentXAngle + AngleX,
                0f
            );

            desiredPosition =
                targetPos -
                targetRotation * Vector3.forward * DistanceCam;
        }

        Quaternion smoothRotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(
                CurrentYAngle + AngleY,
                CurrentXAngle + AngleX,
                0f
            ),
            Time.deltaTime * 10f
        );

        Vector3 smoothPosition =
    targetPos -
    smoothRotation * Vector3.forward * DistanceCam +
    shakeOffset;

        CameraDislocated = AngleY != TargetYAngle;


        Vector3 currentPosition = transform.position;

        float followT = Time.deltaTime * FollowPositionDamping;

        Vector3 dampedPosition = Vector3.Lerp(
            currentPosition,
            smoothPosition,
            followT
        );

        // Extra softer vertical damping.
        dampedPosition.y = Mathf.Lerp(
            currentPosition.y,
            smoothPosition.y,
            followT * FollowHeightDampingMultiplier
        );

        transform.position = dampedPosition;


        transform.rotation = smoothRotation;
    }
}