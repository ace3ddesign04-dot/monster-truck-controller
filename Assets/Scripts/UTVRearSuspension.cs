using CustomVP;
using UnityEngine;

public class UTVRearSuspension : Suspension
{
	public UTVRearWheel RLWheel;

	public UTVRearWheel RRWheel;

	private CarController carController;

	public UTVRearSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[7]
		{
			Controls.AxisOffset,
			Controls.AxisWidth,
			Controls.Damping,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.Stiffness,
			Controls.Travel
		};
	}

	public override void SetControlValues(SuspensionValue[] values)
	{
		SuspensionValue[] controlValues = GetControlValues();
		foreach (SuspensionValue suspensionValue in controlValues)
		{
			foreach (SuspensionValue suspensionValue2 in values)
			{
				if (suspensionValue2.ValueName == suspensionValue.ValueName)
				{
					suspensionValue.ReceiveValues(suspensionValue2);
				}
			}
		}
		OnValidate();
	}

	private void Start()
	{
		carController = GetComponentInParent<CarController>();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = Side.Rear;
			DoWheelColliderParameters();
			DoShocks();
			ChangeShocks();
			DoWidth();
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void DoWidth()
	{
		Transform hub = RLWheel.Hub;
		float floatValue = Controls.AxisOffset.FloatValue;
		Vector3 localPosition = RLWheel.Hub.localPosition;
		hub.localPosition = new Vector3(floatValue, localPosition.y, 0f - Controls.AxisWidth.FloatValue);
		Transform wheelColliderHolder = RLWheel.WheelColliderHolder;
		float x = 0f - Controls.AxisWidth.FloatValue;
		Vector3 localPosition2 = RLWheel.WheelColliderHolder.localPosition;
		wheelColliderHolder.localPosition = new Vector3(x, localPosition2.y, 0f - Controls.AxisOffset.FloatValue);
		Transform hub2 = RRWheel.Hub;
		float x2 = 0f - Controls.AxisOffset.FloatValue;
		Vector3 localPosition3 = RRWheel.Hub.localPosition;
		hub2.localPosition = new Vector3(x2, localPosition3.y, 0f - Controls.AxisWidth.FloatValue);
		Transform wheelColliderHolder2 = RRWheel.WheelColliderHolder;
		float floatValue2 = Controls.AxisWidth.FloatValue;
		Vector3 localPosition4 = RRWheel.WheelColliderHolder.localPosition;
		wheelColliderHolder2.localPosition = new Vector3(floatValue2, localPosition4.y, 0f - Controls.AxisOffset.FloatValue);
	}

	private void DoShocks()
	{
		RRWheel.ShockUps.LookAt(RRWheel.ShockDowns, -base.transform.right);
		RRWheel.ShockDowns.LookAt(RRWheel.ShockUps, -base.transform.right);
		RLWheel.ShockUps.LookAt(RLWheel.ShockDowns, -base.transform.right);
		RLWheel.ShockDowns.LookAt(RLWheel.ShockUps, -base.transform.right);
		RLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		RLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		RRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		RRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < RRWheel.Shocks.Length; i++)
		{
			RLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void DoWheelColliderParameters()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			WheelComponent obj = wheelColliders[0];
			float floatValue = Controls.Travel.FloatValue;
			wheelColliders[1].suspensionLength = floatValue;
			obj.suspensionLength = floatValue;
			wheelColliders[0].spring = (wheelColliders[1].spring = Controls.Stiffness.FloatValue);
			wheelColliders[0].damper = (wheelColliders[1].damper = Controls.Damping.FloatValue);
		}
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.up, rpm, Space.World);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RLWheel.Dummy.position = position;
		Vector3 localPosition = RLWheel.Hub.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.y = localPosition2.y;
		RLWheel.Hub.localPosition = localPosition;
		RLWheel.ArmStart.LookAt(RLWheel.ArmEnd, base.transform.up);
		RLWheel.ArmEnd.LookAt(RLWheel.ArmStart, base.transform.up);
		RLWheel.ArmMount.LookAt(RLWheel.ArmStart, base.transform.up);
		Transform armMount = RLWheel.ArmMount;
		Vector3 localEulerAngles = RLWheel.ArmMount.localEulerAngles;
		float x = localEulerAngles.x;
		Vector3 localEulerAngles2 = RLWheel.ArmMount.localEulerAngles;
		armMount.localEulerAngles = new Vector3(x, -90f, localEulerAngles2.z);
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.up, rpm, Space.World);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RRWheel.Dummy.position = position;
		localPosition = RRWheel.Hub.localPosition;
		Vector3 localPosition3 = RRWheel.Dummy.localPosition;
		localPosition.y = localPosition3.y;
		RRWheel.Hub.localPosition = localPosition;
		RRWheel.ArmStart.LookAt(RRWheel.ArmEnd, base.transform.up);
		RRWheel.ArmEnd.LookAt(RRWheel.ArmStart, base.transform.right);
		RRWheel.ArmMount.LookAt(RRWheel.ArmStart, base.transform.right);
		Transform armMount2 = RRWheel.ArmMount;
		Vector3 localEulerAngles3 = RRWheel.ArmMount.localEulerAngles;
		float x2 = localEulerAngles3.x;
		Vector3 localEulerAngles4 = RRWheel.ArmMount.localEulerAngles;
		armMount2.localEulerAngles = new Vector3(x2, 90f, localEulerAngles4.z);
		DoShocks();
	}

	private void FixedUpdate()
	{
		if (NoWheelColliders)
		{
			return;
		}
		WheelComponent[] wheelColliders = base.wheelColliders;
		foreach (WheelComponent x in wheelColliders)
		{
			if (x == null)
			{
				NoWheelColliders = true;
				return;
			}
		}
		float num = 0f;
		if (carController != null)
		{
			num = carController.Steering;
		}
		float perFrameRotation = base.wheelColliders[0].wheelCollider.perFrameRotation;
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.up, perFrameRotation, Space.World);
		RLWheel.Dummy.position = base.wheelColliders[0].GetVisualWheelPosition();
		Vector3 localPosition = RLWheel.Hub.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.y = localPosition2.y;
		RLWheel.Hub.localPosition = localPosition;
		RLWheel.ArmStart.LookAt(RLWheel.ArmEnd, base.transform.up);
		RLWheel.ArmEnd.LookAt(RLWheel.ArmStart, base.transform.up);
		RLWheel.ArmMount.LookAt(RLWheel.ArmStart, base.transform.up);
		Transform armMount = RLWheel.ArmMount;
		Vector3 localEulerAngles = RLWheel.ArmMount.localEulerAngles;
		float x2 = localEulerAngles.x;
		Vector3 localEulerAngles2 = RLWheel.ArmMount.localEulerAngles;
		armMount.localEulerAngles = new Vector3(x2, -90f, localEulerAngles2.z);
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.up, perFrameRotation, Space.World);
		RRWheel.Dummy.position = base.wheelColliders[1].GetVisualWheelPosition();
		localPosition = RRWheel.Hub.localPosition;
		Vector3 localPosition3 = RRWheel.Dummy.localPosition;
		localPosition.y = localPosition3.y;
		RRWheel.Hub.localPosition = localPosition;
		RRWheel.ArmStart.LookAt(RRWheel.ArmEnd, base.transform.up);
		RRWheel.ArmEnd.LookAt(RRWheel.ArmStart, base.transform.right);
		RRWheel.ArmMount.LookAt(RRWheel.ArmStart, base.transform.right);
		Transform armMount2 = RRWheel.ArmMount;
		Vector3 localEulerAngles3 = RRWheel.ArmMount.localEulerAngles;
		float x3 = localEulerAngles3.x;
		Vector3 localEulerAngles4 = RRWheel.ArmMount.localEulerAngles;
		armMount2.localEulerAngles = new Vector3(x3, 90f, localEulerAngles4.z);
		DoShocks();
	}
}
