using AGS_MonsterTruckControl;
using System;
using UnityEngine;

public class AGS_MTC_FrontSuspension : AGS_MTC_Suspension {
	private AGS_MTC_CarController carController;

	public AGS_MTC_FrontWheel FLWheel;

	public AGS_MTC_FrontWheel FRWheel;

	public Transform SteeringRod;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform PinionShaft;

	public Transform DriveshaftStartRegularAxlePos;

	public Transform DrivehsaftStartRockwellAxlePos;

	public Transform RegularAxle;

	public Transform RockwellAxle;

	public Transform RegularBrake;

	public Transform PinionBrake;

	public Transform MiddleDriveshaft;

	public Transform TieRodStart;

	public Transform TieRodEnd;

	public Transform MiddleFrameL;

	public Transform MiddleFrameR;

	public Transform FrontAxleDummy;

	public Transform ControlArms;

	public Transform TrackBarStart;

	public Transform TrackBarEnd;

	[Space(10f)]
	public AGS_MTC_FrontControls Controls;

	private bool NoWheelColliders;

	public override AGS_MTC_SuspensionValue[] GetControlValues()
	{
		return new AGS_MTC_SuspensionValue[12]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.MiddleFrameWidth,
			Controls.ShocksGroup,
			Controls.ShocksHeight,
			Controls.ShocksOffset,
			Controls.ShocksSize,
			Controls.Stiffness,
			Controls.Travel,
			Controls.AxleType,
			Controls.BrakeType,
			Controls.ShowArms
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
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = AGS_MTC_Side.Front;
			DoWidth();
			DoWheelColliderParameters();
			DoShocksOffset();
			DoControlArmsHiding();
			ChangeShocks();
			DoShocks();
			SwitchModels();
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void DoControlArmsHiding()
	{
		ControlArms.gameObject.SetActive(Controls.ShowArms.IntValue == 1);
	}

	private void SwitchModels()
	{
		RegularAxle.gameObject.SetActive(Controls.AxleType.IntValue == 0);
		RockwellAxle.gameObject.SetActive(Controls.AxleType.IntValue == 1);
		RegularBrake.gameObject.SetActive(Controls.BrakeType.IntValue == 0);
		PinionBrake.gameObject.SetActive(Controls.BrakeType.IntValue == 1);
		DriveshaftEnd.position = ((Controls.AxleType.IntValue != 0) ? DrivehsaftStartRockwellAxlePos.position : DriveshaftStartRegularAxlePos.position);
	}

	private void DoShocks()
	{
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, base.transform.up);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, base.transform.up);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, base.transform.up);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, base.transform.up);
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

	private void DoShocksOffset()
	{
		FLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		FRWheel.ShockUps.localPosition = new Vector3(0f - Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			FLWheel.Axle.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
			FRWheel.Axle.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
			MiddleFrameL.localPosition = new Vector3(0f - Controls.MiddleFrameWidth.FloatValue, 0f, 0f);
			MiddleFrameR.localPosition = new Vector3(Controls.MiddleFrameWidth.FloatValue, 0f, 0f);
			Transform transform = FLWheel.WheelColliderHolder.transform;
			float num = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale = base.transform.lossyScale;
			transform.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform2 = FRWheel.WheelColliderHolder.transform;
			float floatValue = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
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

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		FLWheel.Joint.Rotate(0f, 0f, 0f - rpm);
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, SteerAngle + 90f, 0f);
		FLWheel.ConnectingTieRodBone.LookAt(FRWheel.ConnectingTieRodBone, base.transform.forward);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.ControlArmEnd.LookAt(FLWheel.ControlArmStart, base.transform.forward);
		FLWheel.ControlArmStart.LookAt(FLWheel.ControlArmEnd, base.transform.forward);
		FRWheel.Joint.Rotate(0f, 0f, 0f - rpm);
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, SteerAngle + 90f, 0f);
		FRWheel.ConnectingTieRodBone.LookAt(FLWheel.ConnectingTieRodBone, base.transform.forward);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector2 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector2.z;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector3.x;
		FRWheel.Dummy.localPosition = localPosition;
		localPosition = FrontAxleDummy.localPosition;
		Vector3 localPosition2 = FLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		FrontAxleDummy.localPosition = localPosition;
		FrontAxleDummy.LookAt(FRWheel.Dummy, base.transform.forward);
		DriveshaftStart.Rotate(new Vector3(0f, 0f, rpm));
		DriveshaftEnd.Rotate(new Vector3(0f, 0f, rpm));
		PinionShaft.Rotate(new Vector3(rpm, 0f, 0f));
		MiddleDriveshaft.Rotate(new Vector3(0f, 0f, 0f - rpm));
		FRWheel.ControlArmEnd.LookAt(FRWheel.ControlArmStart, base.transform.forward);
		FRWheel.ControlArmStart.LookAt(FRWheel.ControlArmEnd, base.transform.forward);
		TieRodEnd.LookAt(TieRodStart, base.transform.forward);
		TieRodStart.LookAt(TieRodEnd, base.transform.forward);
		SteeringRod.localEulerAngles = new Vector3(0f, 0f, 0f - SteerAngle);
		TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
		TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
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
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		FLWheel.Joint.Rotate(0f, 0f, 0f - perFrameRotation);
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, num + 90f, 0f);
		FLWheel.ConnectingTieRodBone.LookAt(FRWheel.ConnectingTieRodBone, base.transform.forward);
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].VisualWheel.position);
		localPosition.z = vector.z;
		float num2 = Controls.AxisWidth.FloatValue + 0.03f;
		Vector3 from = -FLWheel.Dummy.forward;
		Vector3 to = base.wheelColliders[1].GetVisualWheelPosition() - base.wheelColliders[0].GetVisualWheelPosition();
		float num3 = Vector3.SignedAngle(from, to, base.wheelColliders[0].transform.forward);
		float num4 = num2 * Mathf.Tan(num3 * ((float)Math.PI / 180f));
		localPosition.z += num4;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.ControlArmEnd.LookAt(FLWheel.ControlArmStart, base.transform.forward);
		FLWheel.ControlArmStart.LookAt(FLWheel.ControlArmEnd, base.transform.forward);
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		FRWheel.Joint.Rotate(0f, 0f, 0f - perFrameRotation);
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, num + 90f, 0f);
		FRWheel.ConnectingTieRodBone.LookAt(FLWheel.ConnectingTieRodBone, base.transform.forward);
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector2 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition.z = vector2.z;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition.x = vector3.x;
		FRWheel.Dummy.localPosition = localPosition;
		localPosition = FrontAxleDummy.localPosition;
		Vector3 localPosition2 = FLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		FrontAxleDummy.localPosition = localPosition;
		FrontAxleDummy.LookAt(FRWheel.Dummy, base.transform.forward);
		DriveshaftStart.Rotate(new Vector3(0f, 0f, perFrameRotation));
		DriveshaftEnd.Rotate(new Vector3(0f, 0f, perFrameRotation));
		PinionShaft.Rotate(new Vector3(perFrameRotation, 0f, 0f));
		MiddleDriveshaft.Rotate(new Vector3(0f, 0f, 0f - perFrameRotation));
		FRWheel.ControlArmEnd.LookAt(FRWheel.ControlArmStart, base.transform.forward);
		FRWheel.ControlArmStart.LookAt(FRWheel.ControlArmEnd, base.transform.forward);
		TieRodEnd.LookAt(TieRodStart, base.transform.forward);
		TieRodStart.LookAt(TieRodEnd, base.transform.forward);
		SteeringRod.localEulerAngles = new Vector3(0f, 0f, 0f - num);
		TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
		TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		DoShocks();
	}
}
