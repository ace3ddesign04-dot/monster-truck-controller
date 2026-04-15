using CustomVP;
using UnityEngine;

public class YamahaATVRearSuspension : Suspension
{
	public YamahaATVRearWheel RLWheel;

	public YamahaATVRearWheel RRWheel;

	public Transform RearArm;

	public Transform RearArmTarget;

	public Transform RearArmEndBone;

	public Transform RearAxle;

	public Transform RearAxleDummy;

	public Transform ShockUp;

	public Transform ShockDown;

	public Transform LOffsetHolder;

	public Transform ROffsetHolder;

	public Transform[] Shocks;

	public YamahaATVRearSuspensionControls Controls;

	private bool NoWheelColliders;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[9]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.RearAxleOffset,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.ShockUpsHeight,
			Controls.ShockUpsOffset,
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
		OnValidate();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			DoRearAxleOffset();
			DoRearArm();
			DoShocks();
			ChangeShocks();
			DoWheelColliderParameters();
			side = Side.Rear;
		}
	}

	private void DoShocks()
	{
		if (!(ShockUp == null))
		{
			ShockUp.localPosition = new Vector3(0f - Controls.ShockUpsOffset.FloatValue, 0f, 0f - Controls.ShockUpsHeight.FloatValue);
			ShockUp.LookAt(ShockDown, -base.transform.up);
			ShockDown.LookAt(ShockUp, base.transform.up);
			ShockUp.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			ShockDown.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		}
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < Shocks.Length; i++)
		{
			Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void DoRearAxleOffset()
	{
		Vector3 localPosition = RearAxleDummy.localPosition;
		localPosition.x = Controls.RearAxleOffset.FloatValue;
		RearAxleDummy.localPosition = localPosition;
		if (!(LOffsetHolder == null) && !(ROffsetHolder == null))
		{
			LOffsetHolder.localPosition = new Vector3(Controls.RearAxleOffset.FloatValue, 0f, 0f - Controls.AxisWidth.FloatValue);
			ROffsetHolder.localPosition = new Vector3(Controls.RearAxleOffset.FloatValue, 0f, Controls.AxisWidth.FloatValue);
			RLWheel.AxleBone.localPosition = new Vector3(0f, 0f - Controls.AxisWidth.FloatValue, 0f);
			RRWheel.AxleBone.localPosition = new Vector3(0f, Controls.AxisWidth.FloatValue, 0f);
			RearArmEndBone.localPosition = new Vector3(Controls.RearAxleOffset.FloatValue, 0f, 0f);
		}
	}

	private void DoRearArm()
	{
		RearArm.LookAt(RearArmTarget, RearAxleDummy.up);
		Vector3 localPosition = RearArmTarget.localPosition;
		localPosition.z = Controls.RearAxleOffset.FloatValue / 4f;
		RearArmTarget.localPosition = localPosition;
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
			wheelColliders[0].OnValidate();
			wheelColliders[1].OnValidate();
		}
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		RearAxle.Rotate(RearAxle.up, rpm, Space.World);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.y = vector.y;
		RLWheel.Dummy.localPosition = localPosition;
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.y = vector2.y;
		RRWheel.Dummy.localPosition = localPosition;
		localPosition = RearAxleDummy.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.y = localPosition2.y;
		RearAxleDummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		DoRearArm();
		DoShocks();
	}

	private void FixedUpdate()
	{
		if (!NoWheelColliders)
		{
			if (wheelColliders[0] == null || wheelColliders[1] == null)
			{
				NoWheelColliders = true;
				return;
			}
			Vector3 localPosition = RLWheel.Dummy.localPosition;
			Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(wheelColliders[0].GetVisualWheelPosition());
			localPosition.y = vector.y;
			RLWheel.Dummy.localPosition = localPosition;
			localPosition = RRWheel.Dummy.localPosition;
			Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(wheelColliders[1].GetVisualWheelPosition());
			localPosition.y = vector2.y;
			RRWheel.Dummy.localPosition = localPosition;
			float perFrameRotation = wheelColliders[0].wheelCollider.perFrameRotation;
			RearAxle.Rotate(RearAxle.up, perFrameRotation, Space.World);
			localPosition = RearAxleDummy.localPosition;
			Vector3 localPosition2 = RLWheel.Dummy.localPosition;
			localPosition.y = localPosition2.y;
			RearAxleDummy.localPosition = localPosition;
			RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
			DoRearArm();
			DoShocks();
		}
	}
}
