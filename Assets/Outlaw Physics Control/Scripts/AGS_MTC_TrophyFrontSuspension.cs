using AGS_MonsterTruckControl;
using UnityEngine;

public class AGS_MTC_TrophyFrontSuspension : AGS_MTC_Suspension {
	private AGS_MTC_CarController carController;

	public AGS_MTC_TrophyFrontWheel FLWheel;

	public AGS_MTC_TrophyFrontWheel FRWheel;

	public Transform SteeringRack;

	public AGS_MTC_TrophyFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	private float Squared(float number)
	{
		return Mathf.Pow(number, 2f);
	}

	public override AGS_MTC_SuspensionValue[] GetControlValues()
	{
		return new AGS_MTC_SuspensionValue[8]
		{
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

	public override void SetControlValues(AGS_MTC_SuspensionValue[] values)
	{
		AGS_MTC_SuspensionValue[] controlValues = GetControlValues();
		foreach (AGS_MTC_SuspensionValue suspensionValue in controlValues)
		{
			foreach (AGS_MTC_SuspensionValue suspensionValue2 in values)
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
		FLWheel.DefBrakeDiskPosition = FLWheel.FrameBone.InverseTransformPoint(FLWheel.BrakeDisk.position);
		FRWheel.DefBrakeDiskPosition = FRWheel.FrameBone.InverseTransformPoint(FRWheel.BrakeDisk.position);
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = AGS_MTC_Side.Front;
			DoWidth();
			DoPerches();
			DoWheelsOffset();
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

	private void DoPerches()
	{
		FLWheel.PerchBone.localPosition = new Vector3(0f - Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
		FRWheel.PerchBone.localPosition = new Vector3(Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
	}

	private void DoWidth()
	{
		FLWheel.FrameBone.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
		FRWheel.FrameBone.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
	}

	private void DoWheelsOffset()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			Transform transform = FLWheel.WheelColliderHolder.transform;
			float num = 0f - FLWheel.Deviation;
			Vector3 lossyScale = base.transform.lossyScale;
			transform.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform2 = FRWheel.WheelColliderHolder.transform;
			float deviation = FRWheel.Deviation;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform2.localPosition = new Vector3(deviation * lossyScale2.x, 0f, 0f);
		}
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.right, rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.right, rpm, Space.World);
		float num = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishbone.position);
		float num2 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishboneTarget.position);
		float number = 1.95f * num2;
		float f = (Squared(num2) + Squared(num) - Squared(number)) / (2f * num2 * num);
		float num3 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles = FLWheel.LowerWishbone.localEulerAngles;
		float num4 = 0f - (localEulerAngles.x - (90f - Vector3.Angle(-FLWheel.LowerWishbone.forward, FLWheel.UpperWishbone.position - FLWheel.Knuckle.position) - num3));
		if (!float.IsNaN(num4) && !float.IsNaN(SteerAngle))
		{
			FLWheel.Knuckle.localEulerAngles = new Vector3(0f, num4, SteerAngle);
		}
		FLWheel.TieRodEndBone.LookAt(FLWheel.TieRodStartBone, base.transform.forward);
		FLWheel.TieRodStartBone.LookAt(FLWheel.TieRodEndBone, base.transform.forward);
		Vector3 localEulerAngles2 = FLWheel.LowerWishbone.localEulerAngles;
		float num5;
		if (localEulerAngles2.x > 20f)
		{
			Vector3 localEulerAngles3 = FLWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles3.x < 90f)
			{
				Vector3 localEulerAngles4 = FLWheel.LowerWishbone.localEulerAngles;
				num5 = (localEulerAngles4.x - 20f) * FLWheel.HeightCorrectionRatio;
				goto IL_022a;
			}
		}
		num5 = 0f;
		goto IL_022a;
		IL_022a:
		float num6 = num5;
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z - num6;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.LowerWishbone.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePos.position;
		FLWheel.UpperWishbone.LookAt(FLWheel.UpperWishboneTarget, base.transform.up);
        AGS_MTC_TrophyFrontWheel fLWheel = FLWheel;
		float x = FLWheel.DefBrakeDiskPosition.x;
		Vector3 vector2 = FLWheel.FrameBone.InverseTransformPoint(FLWheel.BrakeDisk.position);
		fLWheel.Deviation = x - vector2.x;
		num = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishbone.position);
		num2 = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishboneTarget.position);
		number = 1.95f * num2;
		f = (Squared(num2) + Squared(num) - Squared(number)) / (2f * num2 * num);
		num3 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles5 = FRWheel.LowerWishbone.localEulerAngles;
		num4 = 0f - (localEulerAngles5.x - (90f - Vector3.Angle(-FRWheel.LowerWishbone.forward, FRWheel.UpperWishbone.position - FRWheel.Knuckle.position) - num3));
		if (!float.IsNaN(num4) && !float.IsNaN(SteerAngle))
		{
			FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f - num4, SteerAngle);
		}
		FRWheel.TieRodEndBone.LookAt(FRWheel.TieRodStartBone, base.transform.forward);
		FRWheel.TieRodStartBone.LookAt(FRWheel.TieRodEndBone, base.transform.forward);
		Vector3 localEulerAngles6 = FRWheel.LowerWishbone.localEulerAngles;
		float num7;
		if (localEulerAngles6.x > 20f)
		{
			Vector3 localEulerAngles7 = FRWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles7.x < 90f)
			{
				Vector3 localEulerAngles8 = FRWheel.LowerWishbone.localEulerAngles;
				num7 = (localEulerAngles8.x - 20f) * FRWheel.HeightCorrectionRatio;
				goto IL_05bb;
			}
		}
		num7 = 0f;
		goto IL_05bb;
		IL_05bb:
		num6 = num7;
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector3.z - num6;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.LowerWishbone.LookAt(FRWheel.Dummy, base.transform.forward);
		FRWheel.Knuckle.position = FRWheel.KnucklePos.position;
		FRWheel.UpperWishbone.LookAt(FRWheel.UpperWishboneTarget, base.transform.forward);
        AGS_MTC_TrophyFrontWheel fRWheel = FRWheel;
		float num8 = 0f - FRWheel.DefBrakeDiskPosition.x;
		Vector3 vector4 = FRWheel.FrameBone.InverseTransformPoint(FRWheel.BrakeDisk.position);
		fRWheel.Deviation = num8 + vector4.x;
		DoWheelsOffset();
		DoShocks();
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
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.right, perFrameRotation, Space.World);
		float num2 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishbone.position);
		float num3 = Vector3.Distance(FLWheel.Knuckle.position, FLWheel.UpperWishboneTarget.position);
		float number = 1.95f * num3;
		float f = (Squared(num3) + Squared(num2) - Squared(number)) / (2f * num3 * num2);
		float num4 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles = FLWheel.LowerWishbone.localEulerAngles;
		float y = 0f - (localEulerAngles.x - (90f - Vector3.Angle(-FLWheel.LowerWishbone.forward, FLWheel.UpperWishbone.position - FLWheel.Knuckle.position) - num4));
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, y, num);
		FLWheel.TieRodEndBone.LookAt(FLWheel.TieRodStartBone, base.transform.forward);
		FLWheel.TieRodStartBone.LookAt(FLWheel.TieRodEndBone, base.transform.forward);
		Vector3 localEulerAngles2 = FLWheel.LowerWishbone.localEulerAngles;
		float num5;
		if (localEulerAngles2.x > 20f)
		{
			Vector3 localEulerAngles3 = FLWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles3.x < 90f)
			{
				Vector3 localEulerAngles4 = FLWheel.LowerWishbone.localEulerAngles;
				num5 = (localEulerAngles4.x - 20f) * FLWheel.HeightCorrectionRatio;
				goto IL_0271;
			}
		}
		num5 = 0f;
		goto IL_0271;
		IL_0271:
		float num6 = num5;
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.z = vector.z - num6;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.LowerWishbone.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePos.position;
		FLWheel.UpperWishbone.LookAt(FLWheel.UpperWishboneTarget, base.transform.up);
        AGS_MTC_TrophyFrontWheel fLWheel = FLWheel;
		float x2 = FLWheel.DefBrakeDiskPosition.x;
		Vector3 vector2 = FLWheel.FrameBone.InverseTransformPoint(FLWheel.BrakeDisk.position);
		fLWheel.Deviation = x2 - vector2.x;
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.right, perFrameRotation, Space.World);
		num2 = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishbone.position);
		num3 = Vector3.Distance(FRWheel.Knuckle.position, FRWheel.UpperWishboneTarget.position);
		number = 1.95f * num3;
		f = (Squared(num3) + Squared(num2) - Squared(number)) / (2f * num3 * num2);
		num4 = Mathf.Acos(f) * 57.29578f;
		Vector3 localEulerAngles5 = FRWheel.LowerWishbone.localEulerAngles;
		y = 0f - (localEulerAngles5.x - (90f - Vector3.Angle(-FRWheel.LowerWishbone.forward, FRWheel.UpperWishbone.position - FRWheel.Knuckle.position) - num4));
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f - y, num);
		FRWheel.TieRodEndBone.LookAt(FRWheel.TieRodStartBone, base.transform.forward);
		FRWheel.TieRodStartBone.LookAt(FRWheel.TieRodEndBone, base.transform.forward);
		Vector3 localEulerAngles6 = FRWheel.LowerWishbone.localEulerAngles;
		float num7;
		if (localEulerAngles6.x > 20f)
		{
			Vector3 localEulerAngles7 = FRWheel.LowerWishbone.localEulerAngles;
			if (localEulerAngles7.x < 90f)
			{
				Vector3 localEulerAngles8 = FRWheel.LowerWishbone.localEulerAngles;
				num7 = (localEulerAngles8.x - 20f) * FRWheel.HeightCorrectionRatio;
				goto IL_058d;
			}
		}
		num7 = 0f;
		goto IL_058d;
		IL_058d:
		num6 = num7;
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector3.z - num6;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.LowerWishbone.LookAt(FRWheel.Dummy, base.transform.forward);
		FRWheel.Knuckle.position = FRWheel.KnucklePos.position;
		FRWheel.UpperWishbone.LookAt(FRWheel.UpperWishboneTarget, base.transform.forward);
        AGS_MTC_TrophyFrontWheel fRWheel = FRWheel;
		float num8 = 0f - FRWheel.DefBrakeDiskPosition.x;
		Vector3 vector4 = FRWheel.FrameBone.InverseTransformPoint(FRWheel.BrakeDisk.position);
		fRWheel.Deviation = num8 + vector4.x;
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(Mathf.Lerp(-0.015f, 0.015f, (num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)), 0f, 0f);
		}
		DoWheelsOffset();
		DoShocks();
	}
}
