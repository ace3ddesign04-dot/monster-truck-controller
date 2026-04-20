using CustomVP;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MotorcycleAssistant : MonoBehaviour
{
	public WheelComponent FrontWC;

	public WheelComponent RearWC;

	private IKDriverController ikDriver;

	public float MaxLean;

	public Transform BikeBody;

	private Rigidbody rb;

	private float Steering;

	private float LeanTarget;

	[HideInInspector]
	public float lean;

	private float vel;

	private float CorrectionStandAngle;

	private Vector3 RightDirection;

	private float FlyingTime;

	private float xInput;

	private float yInput;

	private float smoothxInput;

	private float smoothyInput;

	[HideInInspector]
	public bool IsBikeGrounded;

	private bool Stabilization;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		ikDriver = GetComponent<IKDriverController>();
	}

	private void Update()
	{
		if (!FrontWC.IsGrounded && !RearWC.IsGrounded)
		{
			FlyingTime += Time.deltaTime;
		}
		else
		{
			FlyingTime = 0f;
		}
		FlyingTime = Mathf.Clamp01(FlyingTime);
	}

	private void FixedUpdate()
	{
		float num = 0f;
		if (rb != null)
		{
			Vector3 vector = base.transform.InverseTransformDirection(rb.velocity);
			num = vector.z * 3.6f;
			rb.angularDrag = ((FrontWC.IsGrounded || RearWC.IsGrounded) ? 10 : 0);
		}
		IsBikeGrounded = (FrontWC.IsGrounded && RearWC.IsGrounded);
		float target = UnityEngine.Input.GetAxis("Horizontal") + CrossPlatformInputManager.GetAxis("Horizontal");
		float num2 = UnityEngine.Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical");
		xInput = Mathf.MoveTowards(xInput, target, Time.fixedDeltaTime * 5f);
		yInput = Mathf.MoveTowards(yInput, num2, Time.fixedDeltaTime * 5f);
		smoothxInput = Mathf.MoveTowards(smoothxInput, target, Time.fixedDeltaTime);
		smoothyInput = Mathf.MoveTowards(smoothyInput, num2, Time.fixedDeltaTime);
		if (Mathf.Sign(smoothyInput) != Mathf.Sign(num2))
		{
			smoothyInput = num2;
		}
		CorrectionStandAngle = ((Mathf.Abs(num) < 2f && IsBikeGrounded && ikDriver.enabled) ? (-10) : 0);
		float num3 = Mathf.InverseLerp(0f, 20f, num);
		LeanTarget = Mathf.MoveTowards(LeanTarget, xInput * (0f - MaxLean) * num3 + CorrectionStandAngle, Time.deltaTime * 100f);
		if (!IsBikeGrounded)
		{
			LeanTarget = 0f;
		}
		lean = Mathf.SmoothDamp(lean, LeanTarget, ref vel, Time.deltaTime * 10f);
		BikeBody.localEulerAngles = new Vector3(0f, 0f, lean);
		Vector3 up = base.transform.up;
		if (up.y > 0.8f && IsBikeGrounded)
		{
			goto IL_0293;
		}
		Vector3 up2 = base.transform.up;
		if (up2.y < 0.8f && IsBikeGrounded)
		{
			Vector3 forward = base.transform.forward;
			if (forward.x > 0.8f)
			{
				goto IL_0293;
			}
		}
		goto IL_02a4;
		IL_02a4:
		if (!FrontWC.IsGrounded && !RearWC.IsGrounded)
		{
			RightDirection = Vector3.zero;
		}
		Vector3 a = base.transform.right * (0f - LeanTarget + CorrectionStandAngle) / MaxLean;
		if (num < -1f)
		{
			a = base.transform.right * (0f - xInput) * 0.5f;
		}
		if (!IsBikeGrounded)
		{
			a = base.transform.right * smoothxInput * 2f;
		}
		Vector3 a2 = base.transform.up * smoothyInput * FlyingTime;
		if (FrontWC.IsGrounded || RearWC.IsGrounded)
		{
			a2 = Vector3.zero;
		}
		Stabilization = true;
		if (ikDriver != null)
		{
			Stabilization = !ikDriver.KnockedOut;
		}
		if (Stabilization && rb != null)
		{
			Vector3 a3 = base.transform.position + Vector3.ProjectOnPlane(base.transform.up + a2 * 3f * Time.fixedDeltaTime, Vector3.ProjectOnPlane(RightDirection, Vector3.up));
			rb.MoveRotation(Quaternion.LookRotation(base.transform.forward + a * 2f * Time.fixedDeltaTime + a2 * 3f * Time.fixedDeltaTime, a3 - base.transform.position));
		}
		return;
		IL_0293:
		RightDirection = base.transform.right;
		goto IL_02a4;
	}
}
