using AGS_MonsterTruckControl;
using System;
using UnityEngine;

public class AGS_MTC_SolidAxleRearSuspension : AGS_MTC_Suspension {
	private AGS_MTC_CarController carController;

    private Quaternion rlSteeringAxleBaseRotation;
    private Quaternion rrSteeringAxleBaseRotation;

    public AGS_MTC_SolidAxleRearWheel RLWheel;

	public AGS_MTC_SolidAxleRearWheel RRWheel;

	public Transform RearAxleDummy;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform DriveshaftTarget;

	public Transform TrackBarStart;

	public Transform TrackBarEnd;

	public AGS_MTC_SolidAxleRearSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override AGS_MTC_SuspensionValue[] GetControlValues()
	{
		return new AGS_MTC_SuspensionValue[14]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.FramesWidth,
			Controls.FrontFrameOffset,
			Controls.LeafSpringMountHeight,
			Controls.RearFrameOffset,
			Controls.RearSteering,
			Controls.ShocksGroup,
			Controls.ShocksHeight,
			Controls.ShocksOffset,
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

	private void Awake()
	{
        carController = GetComponentInParent<AGS_MTC_CarController>();

        if (RLWheel.SteeringAxle != null)
            rlSteeringAxleBaseRotation = RLWheel.SteeringAxle.localRotation;

        if (RRWheel.SteeringAxle != null)
            rrSteeringAxleBaseRotation = RRWheel.SteeringAxle.localRotation;
    }
    private void ApplyRearVisualSteer(float steerAngle) {
        float rearVisualSteer = 0f;

        if (carController != null)
            rearVisualSteer = (0f - steerAngle) * carController.InverseSteerMultiplier;
        else
            rearVisualSteer = (0f - steerAngle) * Controls.RearSteering.FloatValue;

        if (RLWheel.SteeringAxle != null)
            RLWheel.SteeringAxle.localRotation = rlSteeringAxleBaseRotation * Quaternion.Euler(0f, 0f, rearVisualSteer);

        if (RRWheel.SteeringAxle != null)
            RRWheel.SteeringAxle.localRotation = rrSteeringAxleBaseRotation * Quaternion.Euler(0f, 0f, rearVisualSteer);
    }
    public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = AGS_MTC_Side.Rear;
			DoWidth();
			DoSpringBrackets();
			DoShocksOffset();
			DoWheelColliderParameters();
			DoFramesWidth();
			DoLeafSprings();
			DoLeafSpringMountHeight();
			DoShocks();
			ChangeShocks();
			//if (carController != null)
			//{
				//carController.InverseSteerMultiplier = Controls.RearSteering.FloatValue;
			//}
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
	}

	private void DoShocks()
	{
		if (RRWheel.Shocks != null && !(RRWheel.ShockDowns == null) && !(RLWheel.ShockUps == null) && !(RLWheel.ShockDowns == null))
		{
			RRWheel.ShockUps.LookAt(RRWheel.ShockDowns, base.transform.right);
			RRWheel.ShockDowns.LookAt(RRWheel.ShockUps, -base.transform.right);
			RLWheel.ShockUps.LookAt(RLWheel.ShockDowns, -base.transform.right);
			RLWheel.ShockDowns.LookAt(RLWheel.ShockUps, base.transform.right);
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

	private void DoSpringBrackets()
	{
		RLWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
		RRWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			RLWheel.Axle.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
			RRWheel.Axle.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
			Transform transform = RLWheel.WheelColliderHolder.transform;
			float num = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale = base.transform.lossyScale;
			transform.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform2 = RRWheel.WheelColliderHolder.transform;
			float floatValue = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
		}
	}

	private void DoShocksOffset()
	{
		RLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		RRWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
	}

	private void DoFramesWidth()
	{
		RLWheel.Frame.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRWheel.Frame.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		RLWheel.SpringBracket.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRWheel.SpringBracket.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		RLWheel.FrontFrame.localPosition = new Vector3(0f, 0f - Controls.FrontFrameOffset.FloatValue, 0f);
		RRWheel.FrontFrame.localPosition = new Vector3(0f, 0f - Controls.FrontFrameOffset.FloatValue, 0f);
		RLWheel.RearFrame.localPosition = new Vector3(0f, Controls.RearFrameOffset.FloatValue, 0f);
		RRWheel.RearFrame.localPosition = new Vector3(0f, Controls.RearFrameOffset.FloatValue, 0f);
	}

	private void DoLeafSpringMountHeight()
	{
		RLWheel.FrontLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RRWheel.FrontLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RLWheel.RearLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RRWheel.RearLeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
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
		RLWheel.LeafSpringBone.position = RLWheel.LeafSpringPos.position;
		RRWheel.LeafSpringBone.position = RRWheel.LeafSpringPos.position;
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.right, rpm, Space.World);
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, rpm, Space.World);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RLWheel.Dummy.position = position;
        ApplyRearVisualSteer(SteerAngle);
        position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z;
		Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector2.x;
		RRWheel.Dummy.localPosition = localPosition;
        ApplyRearVisualSteer(SteerAngle);
        DriveshaftStart.Rotate(0f, rpm, 0f);
		Vector3 localPosition2 = RearAxleDummy.localPosition;
		Vector3 localPosition3 = RLWheel.Dummy.localPosition;
		localPosition2.z = localPosition3.z;
		RearAxleDummy.localPosition = localPosition2;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.forward);
		if (TrackBarStart != null)
		{
			TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
			TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		}
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
        //    return;
        //}
        float perFrameRotation = base.wheelColliders[0].wheelCollider.perFrameRotation;
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.right, perFrameRotation, Space.World);
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.z = vector.z;
		Vector3 vector2 = RLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].GetVisualWheelPosition());
		localPosition.x = vector2.x;
		float num2 = Controls.AxisWidth.FloatValue + 0.03f;
		Vector3 from = -RLWheel.Dummy.right;
		Vector3 to = base.wheelColliders[1].GetVisualWheelPosition() - base.wheelColliders[0].GetVisualWheelPosition();
		float num3 = Vector3.SignedAngle(from, to, base.wheelColliders[0].transform.forward);
		float num4 = num2 * Mathf.Tan(num3 * ((float)Math.PI / 180f));
		localPosition.z += num4;
		RLWheel.Dummy.localPosition = localPosition;
        ApplyRearVisualSteer(num);
        perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, perFrameRotation, Space.World);
		localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector3 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector3.z;
		Vector3 vector4 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.x = vector4.x;
		RRWheel.Dummy.localPosition = localPosition;
        ApplyRearVisualSteer(num);
        if (DriveshaftStart != null)
		{
			DriveshaftStart.Rotate(0f, perFrameRotation, 0f);
		}
		Vector3 localPosition2 = RearAxleDummy.localPosition;
		Vector3 localPosition3 = RLWheel.Dummy.localPosition;
		localPosition2.z = localPosition3.z;
		RearAxleDummy.localPosition = localPosition2;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.forward);
		if (TrackBarStart != null)
		{
			TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
			TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		}
		DoLeafSprings();
		DoShocks();
	}
}
