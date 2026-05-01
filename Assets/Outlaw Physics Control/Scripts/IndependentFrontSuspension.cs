using AGS_MonsterTruckControl;
using UnityEngine;

public class IndependentFrontSuspension : Suspension
{
	private AGS_MTC_CarController carController;

	public IndependentFrontWheel FLWheel;

	public IndependentFrontWheel FRWheel;

	public Transform SteeringRack;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform DriveshaftTarget;

	public GameObject FrontDrivetrain;

	public IndependentFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	private float Squared(float number)
	{
		return Mathf.Pow(number, 2f);
	}

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[9]
		{
			Controls.AWD,
			Controls.AxisWidth,
			Controls.Damping,
			Controls.PerchHeight,
			Controls.PerchWidth,
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
		carController = GetComponentInParent<AGS_MTC_CarController>();
		FLWheel.DefBrakeDiskPosition = FLWheel.Frame.InverseTransformPoint(FLWheel.BrakeDisk.position);
		FRWheel.DefBrakeDiskPosition = FRWheel.Frame.InverseTransformPoint(FRWheel.BrakeDisk.position);
	}

	private void OnEnable()
	{
		ToggleAWD();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = Side.Front;
			DoPerches();
			DoWidth();
			DoWheelColliderParameters();
			ToggleAWD();
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
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, -base.transform.up);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, -base.transform.up);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, -base.transform.up);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, -base.transform.up);
		FLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		FRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < FRWheel.Shocks.Length; i++)
		{
			FLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			FRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void ToggleAWD()
	{
		if (carController == null)
		{
			carController = GetComponentInParent<AGS_MTC_CarController>();
		}
		if (carController != null)
		{
			carController.FWD = (Controls.AWD.IntValue > 0);
			carController.OnValidate();
		}
		if (FrontDrivetrain != null)
		{
			FrontDrivetrain.SetActive(Controls.AWD.IntValue > 0);
		}
	}

	private void DoWheelColliderParameters()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
            AGS_MTC_WheelComponent obj = wheelColliders[0];
			float floatValue = Controls.Travel.FloatValue;
			wheelColliders[1].suspensionLength = floatValue;
			obj.suspensionLength = floatValue;
			wheelColliders[0].spring = (wheelColliders[1].spring = Controls.Stiffness.FloatValue);
			wheelColliders[0].damper = (wheelColliders[1].damper = Controls.Damping.FloatValue);
		}
	}

	private void DoWheelsOffset()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			Transform wheelCollidersHolder = FLWheel.WheelCollidersHolder;
			float num = 0f - FLWheel.Deviation;
			Vector3 lossyScale = base.transform.lossyScale;
			wheelCollidersHolder.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform = FRWheel.WheelCollidersHolder.transform;
			float deviation = FRWheel.Deviation;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform.localPosition = new Vector3(deviation * lossyScale2.x, 0f, 0f);
		}
	}

	private void DoPerches()
	{
		FLWheel.PerchBone.localPosition = new Vector3(0f - Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
		FRWheel.PerchBone.localPosition = new Vector3(Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
	}

	private void DoWidth()
	{
		FLWheel.Frame.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
		FRWheel.Frame.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, rpm, Space.World);
		float num = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishbone.position);
		float num2 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishboneTarget.position);
		float number = 0.32f;
		float f = (Squared(num2) + Squared(num) - Squared(number)) / (2f * num2 * num);
		float num3 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles = FLWheel.LowerWishbone.localEulerAngles;
		float num4 = 0f - (localEulerAngles.x - (90f - Vector3.Angle(-FLWheel.LowerWishbone.forward, FLWheel.UpperWishbone.position - FLWheel.Knuckle.position) - num3));
		if (!float.IsNaN(num4))
		{
			FLWheel.Knuckle.localEulerAngles = new Vector3(0f, num4, SteerAngle);
		}
		FLWheel.OuterDriveshaftStartBone.LookAt(FLWheel.InnerDriveshaftStartBone, FLWheel.BrakeDisk.up);
		FLWheel.InnerDriveshaftStartBone.LookAt(FLWheel.OuterDriveshaftStartBone, FLWheel.BrakeDisk.up);
		Vector3 localPosition = FLWheel.InnerDriveshaftStartBone.localPosition;
		float y = localPosition.y;
		Vector3 vector = FLWheel.InnerDriveshaftStartBone.parent.InverseTransformPoint(FLWheel.OuterDriveshaftStartBone.position);
		float num5 = y - vector.y;
		FLWheel.OuterDriveshaftStartBone.localPosition = new Vector3((0f - SteerAngle) / 2000f, num5 / 4f, 0f);
		FLWheel.InnerDriveshaftStartBone.localPosition = new Vector3(0f, (0f - num5) / 4f, 0f);
		FLWheel.InnerDriveshaftEndBone.Rotate(0f, 0f, rpm);
		FLWheel.OuterDriveshaftEndBone.Rotate(0f, 0f, 0f - rpm);
		FLWheel.TieRodEnd.LookAt(FLWheel.TieRodStart, base.transform.forward);
		FLWheel.TieRodStart.LookAt(FLWheel.TieRodEnd, base.transform.forward);
		Vector3 localEulerAngles2 = FLWheel.LowerWishbone.localEulerAngles;
		float num6;
		if (localEulerAngles2.x > 20f)
		{
			Vector3 localEulerAngles3 = FLWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles3.x < 90f)
			{
				Vector3 localEulerAngles4 = FLWheel.LowerWishbone.localEulerAngles;
				num6 = (localEulerAngles4.x - 20f) * FLWheel.HeightCorrectionRatio;
				goto IL_0347;
			}
		}
		num6 = 0f;
		goto IL_0347;
		IL_07d6:
		float num7;
		float num8 = num7;
		Vector3 position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition2 = FRWheel.Dummy.localPosition;
		Vector3 vector2 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition2.z = vector2.z - FRWheel.KnuckleHeight - num8;
		FRWheel.Dummy.localPosition = localPosition2;
		FRWheel.LowerWishbone.LookAt(FRWheel.Dummy, base.transform.up);
		FRWheel.Knuckle.position = FRWheel.KnucklePos.position;
		FRWheel.UpperWishbone.LookAt(FRWheel.UpperWishboneTarget, base.transform.forward);
		IndependentFrontWheel fRWheel = FRWheel;
		Vector3 vector3 = FRWheel.Frame.InverseTransformPoint(FRWheel.BrakeDisk.position);
		fRWheel.Deviation = vector3.x - FRWheel.DefBrakeDiskPosition.x;
		DriveshaftStart.Rotate(new Vector3(0f, 0f, 0f - rpm));
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.up);
		DoWheelsOffset();
		DoShocks();
		return;
		IL_0347:
		num8 = num6;
		position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition2 = FLWheel.Dummy.localPosition;
		Vector3 vector4 = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition2.z = vector4.z - FLWheel.KnuckleHeight - num8;
		FLWheel.Dummy.localPosition = localPosition2;
		FLWheel.LowerWishbone.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePos.position;
		FLWheel.UpperWishbone.LookAt(FLWheel.UpperWishboneTarget, base.transform.forward);
		IndependentFrontWheel fLWheel = FLWheel;
		Vector3 vector5 = FLWheel.Frame.InverseTransformPoint(FLWheel.BrakeDisk.position);
		fLWheel.Deviation = 0f - vector5.x + FLWheel.DefBrakeDiskPosition.x;
		num = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishbone.position);
		f = (Squared(num2) + Squared(num) - Squared(number)) / (2f * num2 * num);
		num3 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles5 = FRWheel.LowerWishbone.localEulerAngles;
		num4 = 0f - (localEulerAngles5.x - (90f - Vector3.Angle(-FRWheel.LowerWishbone.forward, FRWheel.UpperWishbone.position - FRWheel.Knuckle.position) - num3));
		if (!float.IsNaN(num4))
		{
			FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f - num4, SteerAngle);
		}
		FRWheel.OuterDriveshaftStartBone.LookAt(FRWheel.InnerDriveshaftStartBone, FRWheel.BrakeDisk.up);
		FRWheel.InnerDriveshaftStartBone.LookAt(FRWheel.OuterDriveshaftStartBone, FRWheel.BrakeDisk.up);
		Vector3 localPosition3 = FRWheel.InnerDriveshaftStartBone.localPosition;
		float y2 = localPosition3.y;
		Vector3 vector6 = FRWheel.InnerDriveshaftStartBone.parent.InverseTransformPoint(FRWheel.OuterDriveshaftStartBone.position);
		num5 = y2 - vector6.y;
		FRWheel.OuterDriveshaftStartBone.localPosition = new Vector3((0f - SteerAngle) / 2000f, num5 / 4f, 0f);
		FRWheel.InnerDriveshaftStartBone.localPosition = new Vector3(0f, (0f - num5) / 4f, 0f);
		FRWheel.InnerDriveshaftEndBone.Rotate(0f, 0f, 0f - rpm);
		FRWheel.OuterDriveshaftEndBone.Rotate(0f, 0f, 0f - rpm);
		FRWheel.TieRodEnd.LookAt(FRWheel.TieRodStart, base.transform.forward);
		FRWheel.TieRodStart.LookAt(FRWheel.TieRodEnd, base.transform.forward);
		Vector3 localEulerAngles6 = FRWheel.LowerWishbone.localEulerAngles;
		if (localEulerAngles6.x > 20f)
		{
			Vector3 localEulerAngles7 = FRWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles7.x < 90f)
			{
				Vector3 localEulerAngles8 = FRWheel.LowerWishbone.localEulerAngles;
				num7 = (localEulerAngles8.x - 20f) * FRWheel.HeightCorrectionRatio;
				goto IL_07d6;
			}
		}
		num7 = 0f;
		goto IL_07d6;
	}

	private void FixedUpdate()
	{
		if (NoWheelColliders)
		{
			return;
		}
        AGS_MTC_WheelComponent[] wheelColliders = base.wheelColliders;
		foreach (AGS_MTC_WheelComponent x in wheelColliders)
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
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		float num2 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishbone.position);
		float num3 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishboneTarget.position);
		float number = 0.32f;
		float f = (Squared(num3) + Squared(num2) - Squared(number)) / (2f * num3 * num2);
		float num4 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles = FLWheel.LowerWishbone.localEulerAngles;
		float y = 0f - (localEulerAngles.x - (90f - Vector3.Angle(-FLWheel.LowerWishbone.forward, FLWheel.UpperWishbone.position - FLWheel.Knuckle.position) - num4));
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, y, num);
		FLWheel.OuterDriveshaftStartBone.LookAt(FLWheel.InnerDriveshaftStartBone, FLWheel.BrakeDisk.up);
		FLWheel.InnerDriveshaftStartBone.LookAt(FLWheel.OuterDriveshaftStartBone, FLWheel.BrakeDisk.up);
		Vector3 localPosition = FLWheel.InnerDriveshaftStartBone.localPosition;
		float y2 = localPosition.y;
		Vector3 vector = FLWheel.InnerDriveshaftStartBone.parent.InverseTransformPoint(FLWheel.OuterDriveshaftStartBone.position);
		float num5 = y2 - vector.y;
		FLWheel.OuterDriveshaftStartBone.localPosition = new Vector3((0f - num) / 2000f, num5 / 4f, 0f);
		FLWheel.InnerDriveshaftStartBone.localPosition = new Vector3(0f, (0f - num5) / 4f, 0f);
		FLWheel.InnerDriveshaftEndBone.Rotate(0f, 0f, perFrameRotation);
		FLWheel.OuterDriveshaftEndBone.Rotate(0f, 0f, 0f - perFrameRotation);
		FLWheel.TieRodEnd.LookAt(FLWheel.TieRodStart, base.transform.forward);
		FLWheel.TieRodStart.LookAt(FLWheel.TieRodEnd, base.transform.forward);
		Vector3 localEulerAngles2 = FLWheel.LowerWishbone.localEulerAngles;
		float num6;
		if (localEulerAngles2.x > 20f)
		{
			Vector3 localEulerAngles3 = FLWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles3.x < 90f)
			{
				Vector3 localEulerAngles4 = FLWheel.LowerWishbone.localEulerAngles;
				num6 = (localEulerAngles4.x - 20f) * FLWheel.HeightCorrectionRatio;
				goto IL_039a;
			}
		}
		num6 = 0f;
		goto IL_039a;
		IL_039a:
		float num7 = num6;
		Vector3 localPosition2 = FLWheel.Dummy.localPosition;
		Vector3 vector2 = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].VisualWheel.position);
		localPosition2.z = vector2.z - FLWheel.KnuckleHeight - num7;
		FLWheel.Dummy.localPosition = localPosition2;
		FLWheel.LowerWishbone.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePos.position;
		FLWheel.UpperWishbone.LookAt(FLWheel.UpperWishboneTarget, base.transform.forward);
		IndependentFrontWheel fLWheel = FLWheel;
		Vector3 vector3 = FLWheel.Frame.InverseTransformPoint(FLWheel.BrakeDisk.position);
		fLWheel.Deviation = 0f - vector3.x + FLWheel.DefBrakeDiskPosition.x;
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, perFrameRotation, Space.World);
		num2 = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishbone.position);
		f = (Squared(num3) + Squared(num2) - Squared(number)) / (2f * num3 * num2);
		num4 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles5 = FRWheel.LowerWishbone.localEulerAngles;
		y = 0f - (localEulerAngles5.x - (90f - Vector3.Angle(-FRWheel.LowerWishbone.forward, FRWheel.UpperWishbone.position - FRWheel.Knuckle.position) - num4));
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f - y, num);
		FRWheel.OuterDriveshaftStartBone.LookAt(FRWheel.InnerDriveshaftStartBone, FRWheel.BrakeDisk.up);
		FRWheel.InnerDriveshaftStartBone.LookAt(FRWheel.OuterDriveshaftStartBone, FRWheel.BrakeDisk.up);
		Vector3 localPosition3 = FRWheel.InnerDriveshaftStartBone.localPosition;
		float y3 = localPosition3.y;
		Vector3 vector4 = FRWheel.InnerDriveshaftStartBone.parent.InverseTransformPoint(FRWheel.OuterDriveshaftStartBone.position);
		num5 = y3 - vector4.y;
		FRWheel.OuterDriveshaftStartBone.localPosition = new Vector3((0f - num) / 2000f, num5 / 4f, 0f);
		FRWheel.InnerDriveshaftStartBone.localPosition = new Vector3(0f, (0f - num5) / 4f, 0f);
		FRWheel.InnerDriveshaftEndBone.Rotate(0f, 0f, 0f - perFrameRotation);
		FRWheel.OuterDriveshaftEndBone.Rotate(0f, 0f, 0f - perFrameRotation);
		FRWheel.TieRodEnd.LookAt(FRWheel.TieRodStart, base.transform.forward);
		FRWheel.TieRodStart.LookAt(FRWheel.TieRodEnd, base.transform.forward);
		Vector3 localEulerAngles6 = FRWheel.LowerWishbone.localEulerAngles;
		float num8;
		if (localEulerAngles6.x > 20f)
		{
			Vector3 localEulerAngles7 = FRWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles7.x < 90f)
			{
				Vector3 localEulerAngles8 = FRWheel.LowerWishbone.localEulerAngles;
				num8 = (localEulerAngles8.x - 20f) * FRWheel.HeightCorrectionRatio;
				goto IL_07c3;
			}
		}
		num8 = 0f;
		goto IL_07c3;
		IL_07c3:
		num7 = num8;
		localPosition2 = FRWheel.Dummy.localPosition;
		Vector3 vector5 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition2.z = vector5.z - FRWheel.KnuckleHeight - num7;
		FRWheel.Dummy.localPosition = localPosition2;
		FRWheel.LowerWishbone.LookAt(FRWheel.Dummy, base.transform.up);
		FRWheel.Knuckle.position = FRWheel.KnucklePos.position;
		FRWheel.UpperWishbone.LookAt(FRWheel.UpperWishboneTarget, base.transform.forward);
		IndependentFrontWheel fRWheel = FRWheel;
		Vector3 vector6 = FRWheel.Frame.InverseTransformPoint(FRWheel.BrakeDisk.position);
		fRWheel.Deviation = vector6.x - FRWheel.DefBrakeDiskPosition.x;
		DriveshaftStart.Rotate(new Vector3(0f, 0f, perFrameRotation));
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.up);
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(0f, 0f, Mathf.Lerp(0.015f, -0.015f, (num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)));
		}
		DoWheelsOffset();
		DoShocks();
	}
}
