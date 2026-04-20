using CustomVP;
using System;
using UnityEngine;

public class TrophyRearSuspension : Suspension
{
	private CarController carController;

	private Vector3 RearAxleDummyDefPos;

	public RearTrophyWheel RLWheel;

	public RearTrophyWheel RRWheel;

	public Transform RearAxleDummy;

	public Transform RearAxleLeft;

	public Transform RearAxleRight;

	public Transform Driveshaft;

	public Transform DriveshaftStart;

	public Transform DriveshaftTarget;

	public RearTrophySuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[11]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.RearSteering,
			Controls.ShocksGroup,
			Controls.ShocksOffset,
			Controls.ShocksSize,
			Controls.ShocksTravel,
			Controls.Stiffness,
			Controls.TrailingArmsHeight,
			Controls.TrailingArmsOffset,
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

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = Side.Rear;
			DoShocksOffset();
			DoTrailingArmsOffset();
			DoWheelColliderParameters();
			DoWidth();
			DoShocks();
			ChangeShocks();
			if (carController != null)
			{
				carController.InverseSteerMultiplier = Controls.RearSteering.FloatValue;
			}
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void Awake()
	{
		carController = GetComponentInParent<CarController>();
		RearAxleDummyDefPos = RearAxleDummy.localPosition;
		RRWheel.DummyDefPos = RRWheel.Dummy.localPosition;
	}

	private void DoShocks()
	{
		if (RRWheel.Shocks != null && !(RRWheel.ShockDowns == null) && !(RLWheel.ShockUps == null) && !(RLWheel.ShockDowns == null))
		{
			RRWheel.ShockUps.LookAt(RRWheel.ShockDowns, base.transform.up);
			RRWheel.ShockDowns.LookAt(RRWheel.ShockUps, -RRWheel.TrailingArm.forward);
			RLWheel.ShockUps.LookAt(RLWheel.ShockDowns, base.transform.up);
			RLWheel.ShockDowns.LookAt(RLWheel.ShockUps, -RLWheel.TrailingArm.forward);
			RLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		}
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < RRWheel.Shocks.Length; i++)
		{
			RLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			RearAxleLeft.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
			RearAxleRight.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
			Transform wheelColliderHolder = RLWheel.WheelColliderHolder;
			float num = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale = base.transform.lossyScale;
			float x = num * lossyScale.x;
			float deviation = RLWheel.Deviation;
			Vector3 lossyScale2 = base.transform.lossyScale;
			wheelColliderHolder.localPosition = new Vector3(x, 0f, deviation * lossyScale2.x);
			Transform wheelColliderHolder2 = RRWheel.WheelColliderHolder;
			float floatValue = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale3 = base.transform.lossyScale;
			float x2 = floatValue * lossyScale3.x;
			float deviation2 = RRWheel.Deviation;
			Vector3 lossyScale4 = base.transform.lossyScale;
			wheelColliderHolder2.localPosition = new Vector3(x2, 0f, deviation2 * lossyScale4.x);
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

	private void DoShocksOffset()
	{
		RLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksTravel.FloatValue);
		RRWheel.ShockUps.localPosition = new Vector3(0f - Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksTravel.FloatValue);
	}

	private void DoTrailingArmsOffset()
	{
		RLWheel.TrailingArmMount.localPosition = new Vector3(Controls.TrailingArmsOffset.FloatValue, Controls.TrailingArmsHeight.FloatValue, 0f);
		RRWheel.TrailingArmMount.localPosition = new Vector3(0f - Controls.TrailingArmsOffset.FloatValue, Controls.TrailingArmsHeight.FloatValue, 0f);
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.right, rpm, Space.World);
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, rpm, Space.World);
		RLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue, 0f);
		RLWheel.TrackBarEndBone.LookAt(RLWheel.TrackBarStartBone, base.transform.forward);
		RLWheel.TrackBarStartBone.LookAt(RLWheel.TrackBarEndBone, base.transform.forward);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z;
		Vector3 vector2 = RLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector2.x;
		RLWheel.Dummy.localPosition = localPosition;
		RRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue, 0f);
		RRWheel.TrackBarEndBone.LookAt(RRWheel.TrackBarStartBone, base.transform.forward);
		RRWheel.TrackBarStartBone.LookAt(RRWheel.TrackBarEndBone, base.transform.forward);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = RRWheel.Helper.localPosition;
		Vector3 vector3 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector3.z;
		Vector3 vector4 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector4.x;
		RRWheel.Helper.localPosition = localPosition;
		RRWheel.Dummy.localPosition = RRWheel.Helper.localPosition;
		localPosition = RLWheel.Helper.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		RLWheel.Deviation = 1.25f - Mathf.Cos(Vector3.Angle(RLWheel.TrailingArm.position - RLWheel.TrailingArmTarget.position, -base.transform.up) * ((float)Math.PI / 180f)) * 1.25f;
		RearTrophyWheel rLWheel = RLWheel;
		float deviation = rLWheel.Deviation;
		Vector3 localScale = base.transform.localScale;
		rLWheel.Deviation = deviation / localScale.x;
		localPosition.y = RearAxleDummyDefPos.y - RLWheel.Deviation;
		RLWheel.Helper.localPosition = localPosition;
		RearAxleDummy.localPosition = RLWheel.Helper.localPosition;
		localPosition = Raycasters[0].transform.localPosition;
		float deviation2 = RLWheel.Deviation;
		Vector3 lossyScale = base.transform.lossyScale;
		localPosition.z = deviation2 * lossyScale.x;
		Raycasters[0].transform.localPosition = localPosition;
		localPosition = RRWheel.Dummy.localPosition;
		RRWheel.Deviation = 1.25f - Mathf.Cos(Vector3.Angle(RRWheel.TrailingArm.position - RRWheel.TrailingArmTarget.position, -base.transform.up) * ((float)Math.PI / 180f)) * 1.25f;
		RearTrophyWheel rRWheel = RRWheel;
		float deviation3 = rRWheel.Deviation;
		Vector3 localScale2 = base.transform.localScale;
		rRWheel.Deviation = deviation3 / localScale2.x;
		localPosition.y = RRWheel.DummyDefPos.y - RRWheel.Deviation;
		RRWheel.Dummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.up);
		localPosition = Raycasters[1].transform.localPosition;
		float deviation4 = RRWheel.Deviation;
		Vector3 lossyScale2 = base.transform.lossyScale;
		localPosition.z = deviation4 * lossyScale2.x;
		Raycasters[1].transform.localPosition = localPosition;
		RLWheel.TrailingArm.LookAt(RLWheel.TrailingArmTarget, base.transform.forward);
		RRWheel.TrailingArm.LookAt(RRWheel.TrailingArmTarget, base.transform.forward);
		DriveshaftStart.Rotate(0f, 0f, 0f - rpm);
		Driveshaft.LookAt(DriveshaftTarget, DriveshaftStart.up);
		DoWidth();
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
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.right, perFrameRotation, Space.World);
		RLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, (0f - num) * Controls.RearSteering.FloatValue, 0f);
		RLWheel.TrackBarEndBone.LookAt(RLWheel.TrackBarStartBone, base.transform.forward);
		RLWheel.TrackBarStartBone.LookAt(RLWheel.TrackBarEndBone, base.transform.forward);
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.z = vector.z;
		Vector3 vector2 = RLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.x = vector2.x;
		RLWheel.Dummy.localPosition = localPosition;
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, perFrameRotation, Space.World);
		RRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, (0f - num) * Controls.RearSteering.FloatValue, 0f);
		RRWheel.TrackBarEndBone.LookAt(RRWheel.TrackBarStartBone, base.transform.forward);
		RRWheel.TrackBarStartBone.LookAt(RRWheel.TrackBarEndBone, base.transform.forward);
		localPosition = RRWheel.Helper.localPosition;
		Vector3 vector3 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector3.z;
		Vector3 vector4 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.x = vector4.x;
		RRWheel.Helper.localPosition = localPosition;
		RRWheel.Dummy.localPosition = RRWheel.Helper.localPosition;
		UnityEngine.Debug.DrawRay(RRWheel.Dummy.parent.TransformPoint(RRWheel.DummyDefPos), Vector3.up, Color.cyan);
		UnityEngine.Debug.DrawRay(RLWheel.Dummy.parent.TransformPoint(RLWheel.DummyDefPos), Vector3.up, Color.cyan);
		Driveshaft.LookAt(DriveshaftTarget, DriveshaftStart.up);
		DriveshaftStart.Rotate(0f, 0f, perFrameRotation);
		localPosition = RLWheel.Helper.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		RLWheel.Deviation = 1.25f - Mathf.Cos(Vector3.Angle(RLWheel.TrailingArm.position - RLWheel.TrailingArmTarget.position, -base.transform.up) * ((float)Math.PI / 180f)) * 1.25f;
		RearTrophyWheel rLWheel = RLWheel;
		float deviation = rLWheel.Deviation;
		Vector3 localScale = base.transform.localScale;
		rLWheel.Deviation = deviation / localScale.x;
		localPosition.y = RearAxleDummyDefPos.y - RLWheel.Deviation;
		RLWheel.Helper.localPosition = localPosition;
		RearAxleDummy.localPosition = RLWheel.Helper.localPosition;
		localPosition = base.wheelColliders[0].transform.localPosition;
		float deviation2 = RLWheel.Deviation;
		Vector3 lossyScale = base.transform.lossyScale;
		localPosition.z = deviation2 * lossyScale.x;
		base.wheelColliders[0].transform.localPosition = localPosition;
		localPosition = RRWheel.Dummy.localPosition;
		RRWheel.Deviation = 1.25f - Mathf.Cos(Vector3.Angle(RRWheel.TrailingArm.position - RRWheel.TrailingArmTarget.position, -base.transform.up) * ((float)Math.PI / 180f)) * 1.25f;
		RearTrophyWheel rRWheel = RRWheel;
		float deviation3 = rRWheel.Deviation;
		Vector3 localScale2 = base.transform.localScale;
		rRWheel.Deviation = deviation3 / localScale2.x;
		localPosition.y = RRWheel.DummyDefPos.y - RRWheel.Deviation;
		RRWheel.Dummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.up);
		localPosition = base.wheelColliders[1].transform.localPosition;
		float deviation4 = RRWheel.Deviation;
		Vector3 lossyScale2 = base.transform.lossyScale;
		localPosition.z = deviation4 * lossyScale2.x;
		base.wheelColliders[1].transform.localPosition = localPosition;
		RLWheel.TrailingArm.LookAt(RLWheel.TrailingArmTarget, base.transform.forward);
		RRWheel.TrailingArm.LookAt(RRWheel.TrailingArmTarget, base.transform.forward);
		Vector3 localPosition3 = RearAxleDummy.localPosition;
		if (localPosition3.y < 0f)
		{
			localPosition = RearAxleDummy.localPosition;
			localPosition.y = 0f - localPosition.y;
			RearAxleDummy.localPosition = localPosition;
		}
		DoWidth();
		DoShocks();
	}
}
