using AGS_MonsterTruckControl;
using System;
using UnityEngine;

public class AGS_MTC_SolidAxleFrontSuspension : AGS_MTC_Suspension {
	private AGS_MTC_CarController carController;

	public AGS_MTC_SolidAxleFrontWheel FLWheel;

	public AGS_MTC_SolidAxleFrontWheel FRWheel;

	public Transform FrontAxleDummy;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform DriveshaftTarget;

	public Transform TieRodStart;

	public Transform TieRodEnd;

	public Transform SteeringRack;

	public Transform TrackBarStart;

	public Transform TrackBarEnd;

	private float Steering;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public AGS_MTC_SolidAxleFrontSuspensionControls Controls;

	public override AGS_MTC_SuspensionValue[] GetControlValues()
	{
		return new AGS_MTC_SuspensionValue[13]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.FramesWidth,
			Controls.FrontFrameOffset,
			Controls.LeafSpringMountHeight,
			Controls.PerchHeight,
			Controls.PerchWidth,
			Controls.RearFrameOffset,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.SpringBracketsUpperMount,
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
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = AGS_MTC_Side.Front;
			DoWidth();
			DoSpringBrackets();
			DoFramesWidth();
			DoLeafSprings();
			DoLeafSpringMountHeight();
			DoPerchWidth();
			DoShocks();
			ChangeShocks();
			DoWheelColliderParameters();
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void DoShocks()
	{
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, -base.transform.right);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, base.transform.right);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, base.transform.right);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, -base.transform.right);
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

	private void DoPerchWidth()
	{
		FLWheel.Perch.localPosition = new Vector3(Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
		FRWheel.Perch.localPosition = new Vector3(0f - Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
	}

	private void DoSpringBrackets()
	{
		FLWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
		FRWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
	}

	private void DoWidth()
	{
		FLWheel.Axle.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
		FRWheel.Axle.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
		Transform transform = FLWheel.WheelColliderHolder.transform;
		float num = 0f - Controls.AxisWidth.FloatValue;
		Vector3 lossyScale = base.transform.lossyScale;
		transform.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
		Transform transform2 = FRWheel.WheelColliderHolder.transform;
		float floatValue = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale2 = base.transform.lossyScale;
		transform2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
	}

	private void DoFramesWidth()
	{
		FLWheel.Frame.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		FRWheel.Frame.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		FLWheel.SpringBracket.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		FRWheel.SpringBracket.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		FLWheel.FrameFront.localPosition = new Vector3(0f, Controls.FrontFrameOffset.FloatValue, 0f);
		FRWheel.FrameFront.localPosition = new Vector3(0f, Controls.FrontFrameOffset.FloatValue, 0f);
		FLWheel.FrameRear.localPosition = new Vector3(0f, Controls.RearFrameOffset.FloatValue, 0f);
		FRWheel.FrameRear.localPosition = new Vector3(0f, Controls.RearFrameOffset.FloatValue, 0f);
	}

	private void DoLeafSpringMountHeight()
	{
		FLWheel.RearLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		FRWheel.RearLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		FLWheel.FrontLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		FRWheel.FrontLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
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

	private void DoLeafSprings()
	{
		FLWheel.LeafSpringBone.position = FLWheel.LeafSpringPos.position;
		FRWheel.LeafSpringBone.position = FRWheel.LeafSpringPos.position;
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector.x;
		Vector3 vector2 = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector2.z;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f - SteerAngle, 90f, 90f);
		FLWheel.TieRodBone.LookAt(FRWheel.TieRodBone, base.transform.forward);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector3.x;
		Vector3 vector4 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector4.z;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f - SteerAngle, 90f, 90f);
		FRWheel.TieRodBone.LookAt(FLWheel.TieRodBone, base.transform.forward);
		DriveshaftStart.Rotate(0f - rpm, 0f, 0f);
		localPosition = FrontAxleDummy.localPosition;
		Vector3 localPosition2 = FLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		FrontAxleDummy.localPosition = localPosition;
		FrontAxleDummy.LookAt(FRWheel.Dummy, base.transform.forward);
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.up);
		TieRodEnd.LookAt(TieRodStart, base.transform.forward);
		TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
		TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		DoShocks();
		DoLeafSprings();
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
		//if (base.wheelColliders.Length < 1 || base.wheelColliders[0] != null) {
		//	return;
		//}
		float perFrameRotation = base.wheelColliders[0].wheelCollider.perFrameRotation;
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.z = vector.z;
		float num2 = Controls.AxisWidth.FloatValue + 0.03f;
		Vector3 forward = FLWheel.Dummy.forward;
		Vector3 to = base.wheelColliders[1].GetVisualWheelPosition() - base.wheelColliders[0].GetVisualWheelPosition();
		float num3 = Vector3.SignedAngle(forward, to, base.wheelColliders[0].transform.forward);
		float num4 = num2 * Mathf.Tan(num3 * ((float)Math.PI / 180f));
		localPosition.z += num4;
		Vector3 vector2 = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.x = vector2.x;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f - num, 90f, 90f);
		FLWheel.TieRodBone.LookAt(FRWheel.TieRodBone, base.transform.forward);
		FLWheel.Driveshaft.Rotate(new Vector3(0f, 0f, 0f - perFrameRotation));
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector3.z;
		Vector3 vector4 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.x = vector4.x;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f - num, 90f, 90f);
		FRWheel.TieRodBone.LookAt(FLWheel.TieRodBone, base.transform.forward);
		FRWheel.Driveshaft.Rotate(new Vector3(0f, 0f, 0f - perFrameRotation));
		DriveshaftStart.Rotate(perFrameRotation, 0f, 0f);
		Vector3 localPosition2 = FrontAxleDummy.localPosition;
		Vector3 localPosition3 = FLWheel.Dummy.localPosition;
		localPosition2.z = localPosition3.z;
		FrontAxleDummy.localPosition = localPosition2;
		FrontAxleDummy.LookAt(FRWheel.Dummy, base.transform.forward);
		TieRodEnd.LookAt(TieRodStart, base.transform.forward);
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(0f, 0f, Mathf.Lerp(0.02f, -0.02f, (num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)));
		}
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.up);
		TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
		TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		DoShocks();
		DoLeafSprings();
	}
}
