using CustomVP;
using System;
using UnityEngine;

public class FordAFrontSuspension : Suspension
{
	private CarController carController;

	public FordAFrontWheel FLWheel;

	public FordAFrontWheel FRWheel;

	public Transform SteeringRack;

	public FordAFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[6]
		{
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
			side = Side.Front;
			DoWidth();
			DoWheelColliderParameters();
			DoShocks();
			ChangeShocks();
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void DoShocks()
	{
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, base.transform.forward);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, base.transform.forward);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, base.transform.forward);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, base.transform.forward);
		FLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < FLWheel.Shocks.Length; i++)
		{
			FLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			FRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
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

	private void DoWidth()
	{
		FLWheel.LowerArmEnd.localPosition = new Vector3(0f, 0f, 0f - Controls.AxisWidth.FloatValue);
		Transform wheelColliderHolder = FLWheel.WheelColliderHolder;
		float num = 0f - Controls.AxisWidth.FloatValue;
		Vector3 lossyScale = base.transform.lossyScale;
		wheelColliderHolder.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
		FRWheel.LowerArmEnd.localPosition = new Vector3(0f, 0f, 0f - Controls.AxisWidth.FloatValue);
		Transform wheelColliderHolder2 = FRWheel.WheelColliderHolder;
		float floatValue = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale2 = base.transform.lossyScale;
		wheelColliderHolder2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
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
		FLWheel.BrakeDisk.Rotate(-FLWheel.BrakeDisk.forward, perFrameRotation, Space.World);
		FLWheel.Knuckle.localPosition = FLWheel.Knuckle.parent.InverseTransformPoint(FLWheel.KnucklePos.position);
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FLWheel.UpperArmStart.LookAt(FLWheel.UpperArmEnd, base.transform.up);
		FLWheel.UpperArmEnd.LookAt(FLWheel.UpperArmStart, base.transform.up);
		FLWheel.TieRodEndBone.LookAt(FLWheel.TieRodStartBone, base.transform.up);
		FLWheel.TieRodStartBone.LookAt(FLWheel.TieRodEndBone, base.transform.up);
		Vector3 visualWheelPosition = base.wheelColliders[0].GetVisualWheelPosition();
		float num2 = Vector3.Angle(base.transform.right, FLWheel.LowerArmStart.position - visualWheelPosition);
		float num3 = Mathf.Cos(num2 * ((float)Math.PI / 180f));
		Vector3 localPosition = FLWheel.LowerArmStart.localPosition;
		float x2 = localPosition.x;
		float num4 = Vector3.Distance(FLWheel.LowerArmStart.position, FLWheel.KnucklePos.position);
		Vector3 lossyScale = FLWheel.Dummy.lossyScale;
		localPosition.x = x2 - num4 / lossyScale.x * num3;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(visualWheelPosition);
		localPosition.y = vector.y;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.LowerArmStart.LookAt(FLWheel.Dummy, base.transform.up);
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, perFrameRotation, Space.World);
		FRWheel.Knuckle.localPosition = FRWheel.Knuckle.parent.InverseTransformPoint(FRWheel.KnucklePos.position);
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FRWheel.UpperArmStart.LookAt(FRWheel.UpperArmEnd, base.transform.up);
		FRWheel.UpperArmEnd.LookAt(FRWheel.UpperArmStart, base.transform.up);
		FRWheel.TieRodEndBone.LookAt(FRWheel.TieRodStartBone, base.transform.up);
		FRWheel.TieRodStartBone.LookAt(FRWheel.TieRodEndBone, base.transform.up);
		visualWheelPosition = base.wheelColliders[1].GetVisualWheelPosition();
		num2 = Vector3.Angle(base.transform.right, FRWheel.LowerArmStart.position - visualWheelPosition);
		num3 = Mathf.Cos(num2 * ((float)Math.PI / 180f));
		localPosition = FRWheel.LowerArmStart.localPosition;
		float x3 = localPosition.x;
		float num5 = Vector3.Distance(FRWheel.LowerArmStart.position, FRWheel.KnucklePos.position);
		Vector3 lossyScale2 = FRWheel.Dummy.lossyScale;
		localPosition.x = x3 - num5 / lossyScale2.x * num3;
		Vector3 vector2 = FRWheel.Dummy.parent.InverseTransformPoint(visualWheelPosition);
		localPosition.y = vector2.y;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.LowerArmStart.LookAt(FRWheel.Dummy, base.transform.up);
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(0f, 0f, 0f - Mathf.Lerp(-0.3f, 0.3f, (num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)));
		}
		DoShocks();
	}
}
