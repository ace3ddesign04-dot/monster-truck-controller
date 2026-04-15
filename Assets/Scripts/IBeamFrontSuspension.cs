using CustomVP;
using System;
using UnityEngine;

public class IBeamFrontSuspension : Suspension
{
	public FrontIBeamWheel FLWheel;

	public FrontIBeamWheel FRWheel;

	private CarController carController;

	public Transform SteeringRailLBone;

	public Transform SteeringRailRBone;

	public Transform SteeringRackMovingPart;

	public IBeamFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private bool KeepWheelsVertical = true;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	private void Start()
	{
		carController = GetComponentInParent<CarController>();
		FLWheel.DummyDefPos = FLWheel.Dummy.localPosition;
		FRWheel.DummyDefPos = FRWheel.Dummy.localPosition;
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = Side.Front;
			DoPerches();
			DoWidth();
			DoTrailingArmMounts();
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

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[9]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.PerchHeight,
			Controls.PerchWidth,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.Stiffness,
			Controls.TrailingArmMountsWidth,
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

	private void DoShocks()
	{
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, -base.transform.forward);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, -base.transform.forward);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, -base.transform.forward);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, -base.transform.forward);
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

	private void DoTrailingArmMounts()
	{
		FLWheel.TrailingArmMountBone.localPosition = new Vector3(0f - Controls.TrailingArmMountsWidth.FloatValue, 0f, 0f);
		FRWheel.TrailingArmMountBone.localPosition = new Vector3(Controls.TrailingArmMountsWidth.FloatValue, 0f, 0f);
	}

	private void DoPerches()
	{
		FRWheel.PerchBone.localPosition = new Vector3(Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
		FLWheel.PerchBone.localPosition = new Vector3(0f - Controls.PerchWidth.FloatValue, 0f, Controls.PerchHeight.FloatValue);
	}

	private void DoWidth()
	{
		FLWheel.FrameBone.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
		FRWheel.FrameBone.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
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
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.right, rpm, Space.World);
		float num = -0.002f;
		if (KeepWheelsVertical)
		{
			num /= 2f;
		}
		FLWheel.TieRodEndBone.LookAt(FLWheel.TieRodStartBone, base.transform.up);
		FLWheel.TieRodStartBone.LookAt(FLWheel.TieRodEndBone, base.transform.up);
		FLWheel.SteeringBrace.localEulerAngles = new Vector3(0f, 0f - SteerAngle, 0f);
		SteeringRailLBone.LookAt(SteeringRailRBone, base.transform.up);
		SteeringRailRBone.LookAt(SteeringRailLBone, base.transform.up);
		float num2 = 0.7f - Mathf.Cos(Vector3.Angle(FLWheel.TrailingArmTarget.position - FLWheel.TrailingArm.position, base.transform.forward) * ((float)Math.PI / 180f)) * 0.7f;
		num2 /= 3f;
		Transform transform = FLWheel.WheelColliderHolder.transform;
		float num3 = 0f - Controls.AxisWidth.FloatValue;
		Vector3 lossyScale = base.transform.lossyScale;
		float x = num3 * lossyScale.x;
		float num4 = 0f - num2;
		Vector3 lossyScale2 = base.transform.lossyScale;
		transform.localPosition = new Vector3(x, 0f, num4 * lossyScale2.x);
		FLWheel.RealCamber = Vector3.Angle(base.transform.right, Vector3.ProjectOnPlane(FLWheel.CamberMeasurer.right, base.transform.forward));
		Vector3 vector = base.transform.InverseTransformVector(Vector3.Cross(base.transform.right, Vector3.ProjectOnPlane(FLWheel.CamberMeasurer.right, base.transform.forward)));
		if (vector.z < 0f)
		{
			FLWheel.RealCamber = 0f - FLWheel.RealCamber;
		}
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		localPosition.y = FLWheel.DummyDefPos.y + num2;
		Vector3 vector2 = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector2.z - num + 0.01f;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.Arm.LookAt(FLWheel.Dummy, base.transform.up);
		FLWheel.TrailingArm.LookAt(FLWheel.TrailingArmTarget, base.transform.up);
		Transform knuckle = FLWheel.Knuckle;
		float y = (!KeepWheelsVertical) ? 0f : FLWheel.RealCamber;
		Vector3 localEulerAngles = FLWheel.Arm.localEulerAngles;
		knuckle.localEulerAngles = new Vector3(0f, y, SteerAngle - localEulerAngles.x);
		FRWheel.TieRodEndBone.LookAt(FRWheel.TieRodStartBone, base.transform.up);
		FRWheel.TieRodStartBone.LookAt(FRWheel.TieRodEndBone, base.transform.up);
		FRWheel.SteeringBrace.localEulerAngles = new Vector3(0f, 0f - SteerAngle, 0f);
		num2 = 0.7f - Mathf.Cos(Vector3.Angle(FRWheel.TrailingArmTarget.position - FRWheel.TrailingArm.position, base.transform.forward) * ((float)Math.PI / 180f)) * 0.7f;
		num2 /= 3f;
		Transform transform2 = FRWheel.WheelColliderHolder.transform;
		float floatValue = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale3 = base.transform.lossyScale;
		float x2 = floatValue * lossyScale3.x;
		float num5 = 0f - num2;
		Vector3 lossyScale4 = base.transform.lossyScale;
		transform2.localPosition = new Vector3(x2, 0f, num5 * lossyScale4.x);
		FRWheel.RealCamber = Vector3.Angle(base.transform.right, Vector3.ProjectOnPlane(FRWheel.CamberMeasurer.right, base.transform.forward));
		Vector3 vector3 = base.transform.InverseTransformVector(Vector3.Cross(base.transform.right, Vector3.ProjectOnPlane(FRWheel.CamberMeasurer.right, base.transform.forward)));
		if (vector3.z > 0f)
		{
			FRWheel.RealCamber = 0f - FRWheel.RealCamber;
		}
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = FRWheel.Dummy.localPosition;
		localPosition.y = FRWheel.DummyDefPos.y + num2;
		Vector3 vector4 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector4.z - num + 0.01f;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.Arm.LookAt(FRWheel.Dummy, base.transform.up);
		FRWheel.TrailingArm.LookAt(FRWheel.TrailingArmTarget, base.transform.up);
		Transform knuckle2 = FRWheel.Knuckle;
		float y2 = (!KeepWheelsVertical) ? 0f : (0f - FRWheel.RealCamber);
		Vector3 localEulerAngles2 = FRWheel.Arm.localEulerAngles;
		knuckle2.localEulerAngles = new Vector3(0f, y2, SteerAngle + localEulerAngles2.x);
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
		if (carController != null)
		{
			SteeringRackMovingPart.localPosition = new Vector3(0f, 0f, 0f - Mathf.LerpUnclamped(0.006f, -0.006f, (num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)));
		}
		float num2 = -0.002f;
		if (KeepWheelsVertical)
		{
			num2 /= 2f;
		}
		float perFrameRotation = base.wheelColliders[0].wheelCollider.perFrameRotation;
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.forward, 0f - perFrameRotation, Space.World);
		FLWheel.TieRodEndBone.LookAt(FLWheel.TieRodStartBone, base.transform.up);
		FLWheel.TieRodStartBone.LookAt(FLWheel.TieRodEndBone, base.transform.up);
		FLWheel.SteeringBrace.localEulerAngles = new Vector3(0f, 0f - num, 0f);
		SteeringRailLBone.LookAt(SteeringRailRBone, base.transform.up);
		SteeringRailRBone.LookAt(SteeringRailLBone, base.transform.up);
		float num3 = 0.7f - Mathf.Cos(Vector3.Angle(FLWheel.TrailingArmTarget.position - FLWheel.TrailingArm.position, base.transform.forward) * ((float)Math.PI / 180f)) * 0.7f;
		num3 /= 3f;
		Transform transform = FLWheel.WheelColliderHolder.transform;
		float num4 = 0f - Controls.AxisWidth.FloatValue;
		Vector3 lossyScale = base.transform.lossyScale;
		float x2 = num4 * lossyScale.x;
		float num5 = 0f - num3;
		Vector3 lossyScale2 = base.transform.lossyScale;
		transform.localPosition = new Vector3(x2, 0f, num5 * lossyScale2.x);
		FLWheel.RealCamber = Vector3.Angle(base.transform.right, Vector3.ProjectOnPlane(FLWheel.CamberMeasurer.right, base.transform.forward));
		Vector3 vector = base.transform.InverseTransformVector(Vector3.Cross(base.transform.right, Vector3.ProjectOnPlane(FLWheel.CamberMeasurer.right, base.transform.forward)));
		if (vector.z < 0f)
		{
			FLWheel.RealCamber = 0f - FLWheel.RealCamber;
		}
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		localPosition.y = FLWheel.DummyDefPos.y + num3;
		Vector3 vector2 = FLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].VisualWheel.position);
		localPosition.z = vector2.z - num2 + 0.01f;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.Arm.LookAt(FLWheel.Dummy, base.transform.up);
		FLWheel.TrailingArm.LookAt(FLWheel.TrailingArmTarget, base.transform.up);
		Transform knuckle = FLWheel.Knuckle;
		float y = (!KeepWheelsVertical) ? 0f : FLWheel.RealCamber;
		float num6 = num;
		Vector3 localEulerAngles = FLWheel.Arm.localEulerAngles;
		knuckle.localEulerAngles = new Vector3(0f, y, num6 - localEulerAngles.x);
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.right, perFrameRotation, Space.World);
		FRWheel.TieRodEndBone.LookAt(FRWheel.TieRodStartBone, base.transform.up);
		FRWheel.TieRodStartBone.LookAt(FRWheel.TieRodEndBone, base.transform.up);
		FRWheel.SteeringBrace.localEulerAngles = new Vector3(0f, 0f - num, 0f);
		num3 = 0.7f - Mathf.Cos(Vector3.Angle(FRWheel.TrailingArmTarget.position - FRWheel.TrailingArm.position, base.transform.forward) * ((float)Math.PI / 180f)) * 0.7f;
		num3 /= 3f;
		Transform transform2 = FRWheel.WheelColliderHolder.transform;
		float floatValue = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale3 = base.transform.lossyScale;
		float x3 = floatValue * lossyScale3.x;
		float num7 = 0f - num3;
		Vector3 lossyScale4 = base.transform.lossyScale;
		transform2.localPosition = new Vector3(x3, 0f, num7 * lossyScale4.x);
		FRWheel.RealCamber = Vector3.Angle(base.transform.right, Vector3.ProjectOnPlane(FRWheel.CamberMeasurer.right, base.transform.forward));
		Vector3 vector3 = base.transform.InverseTransformVector(Vector3.Cross(base.transform.right, Vector3.ProjectOnPlane(FRWheel.CamberMeasurer.right, base.transform.forward)));
		if (vector3.z > 0f)
		{
			FRWheel.RealCamber = 0f - FRWheel.RealCamber;
		}
		localPosition = FRWheel.Dummy.localPosition;
		localPosition.y = FRWheel.DummyDefPos.y + num3;
		Vector3 vector4 = FRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition.z = vector4.z - num2 + 0.01f;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.Arm.LookAt(FRWheel.Dummy, base.transform.up);
		FRWheel.TrailingArm.LookAt(FRWheel.TrailingArmTarget, base.transform.up);
		Transform knuckle2 = FRWheel.Knuckle;
		float y2 = (!KeepWheelsVertical) ? 0f : (0f - FRWheel.RealCamber);
		float num8 = num;
		Vector3 localEulerAngles2 = FRWheel.Arm.localEulerAngles;
		knuckle2.localEulerAngles = new Vector3(0f, y2, num8 + localEulerAngles2.x);
		DoShocks();
	}
}
