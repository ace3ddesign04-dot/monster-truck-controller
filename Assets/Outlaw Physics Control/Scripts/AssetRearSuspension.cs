using CustomVP;
using System;
using UnityEngine;

public class AssetRearSuspension : Suspension
{
	private CarController carController;

	public AssetRearWheel RLWheel;

	public AssetRearWheel RRWheel;

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

	public Transform MiddleFrameL;

	public Transform MiddleFrameR;

	public Transform RearAxleDummy;

	public Transform ControlArms;

	public Transform TrackBarStart;

	public Transform TrackBarEnd;

	public AssetRearSuspensionControls Controls;

	private bool NoWheelColliders;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[13]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.MiddleFrameWidth,
			Controls.RearSteering,
			Controls.ShocksGroup,
			Controls.ShocksHeight,
			Controls.ShocksOffset,
			Controls.ShocksSize,
			Controls.Stiffness,
			Controls.Travel,
			Controls.BrakeType,
			Controls.AxleType,
			Controls.ShowArms
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

	private void Awake()
	{
		carController = GetComponentInParent<CarController>();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = Side.Rear;
			DoWidth();
			DoWheelColliderParameters();
			DoShocksOffset();
			DoControlArmsHiding();
			ChangeShocks();
			DoShocks();
			SwitchModels();
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

	private void SwitchModels()
	{
		RegularAxle.gameObject.SetActive(Controls.AxleType.IntValue == 0);
		RockwellAxle.gameObject.SetActive(Controls.AxleType.IntValue == 1);
		RegularBrake.gameObject.SetActive(Controls.BrakeType.IntValue == 0);
		PinionBrake.gameObject.SetActive(Controls.BrakeType.IntValue == 1);
		DriveshaftEnd.position = ((Controls.AxleType.IntValue != 0) ? DrivehsaftStartRockwellAxlePos.position : DriveshaftStartRegularAxlePos.position);
	}

	private void DoControlArmsHiding()
	{
		ControlArms.gameObject.SetActive(Controls.ShowArms.IntValue == 1);
	}

	private void DoShocks()
	{
		RRWheel.ShockUps.LookAt(RRWheel.ShockDowns, base.transform.up);
		RRWheel.ShockDowns.LookAt(RRWheel.ShockUps, base.transform.up);
		RLWheel.ShockUps.LookAt(RLWheel.ShockDowns, base.transform.up);
		RLWheel.ShockDowns.LookAt(RLWheel.ShockUps, base.transform.up);
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

	private void DoShocksOffset()
	{
		RLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		RRWheel.ShockUps.localPosition = new Vector3(0f - Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			RLWheel.Axle.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
			RRWheel.Axle.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
			Transform wheelColliderHolder = RLWheel.WheelColliderHolder;
			float num = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale = base.transform.lossyScale;
			wheelColliderHolder.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform wheelColliderHolder2 = RRWheel.WheelColliderHolder;
			float floatValue = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale2 = base.transform.lossyScale;
			wheelColliderHolder2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
			MiddleFrameL.localPosition = new Vector3(Controls.MiddleFrameWidth.FloatValue, 0f, 0f);
			MiddleFrameR.localPosition = new Vector3(0f - Controls.MiddleFrameWidth.FloatValue, 0f, 0f);
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
		RLWheel.BrakeDisk.Rotate(RLWheel.BrakeDisk.right, rpm, Space.World);
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, rpm, Space.World);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RLWheel.SteeringAxis.localEulerAngles = new Vector3(0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue, 0f);
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z;
		RLWheel.Dummy.localPosition = localPosition;
		RLWheel.ControlArmEnd.LookAt(RLWheel.ControlArmStart, base.transform.forward);
		RLWheel.ControlArmStart.LookAt(RLWheel.ControlArmEnd, base.transform.forward);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RRWheel.SteeringAxis.localEulerAngles = new Vector3(0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue, 0f);
		localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector2.z;
		Vector3 vector3 = RRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector3.x;
		RRWheel.Dummy.localPosition = localPosition;
		localPosition = RearAxleDummy.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		RearAxleDummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		DriveshaftStart.Rotate(new Vector3(0f, 0f, rpm));
		DriveshaftEnd.Rotate(new Vector3(0f, 0f, rpm));
		PinionShaft.Rotate(new Vector3(rpm, 0f, 0f));
		MiddleDriveshaft.Rotate(new Vector3(0f, 0f, 0f - rpm));
		RRWheel.ControlArmEnd.LookAt(RRWheel.ControlArmStart, base.transform.forward);
		RRWheel.ControlArmStart.LookAt(RRWheel.ControlArmEnd, base.transform.forward);
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
		RLWheel.SteeringAxis.localEulerAngles = new Vector3(0f, (0f - num) * Controls.RearSteering.FloatValue, 0f);
		Vector3 localPosition = RLWheel.Dummy.localPosition;
		Vector3 vector = RLWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[0].VisualWheel.position);
		localPosition.z = vector.z;
		float num2 = Controls.AxisWidth.FloatValue + 0.03f;
		Vector3 from = -RLWheel.Dummy.right;
		Vector3 to = base.wheelColliders[1].GetVisualWheelPosition() - base.wheelColliders[0].GetVisualWheelPosition();
		float num3 = Vector3.SignedAngle(from, to, base.wheelColliders[0].transform.forward);
		float num4 = num2 * Mathf.Tan(num3 * ((float)Math.PI / 180f));
		localPosition.z += num4;
		RLWheel.Dummy.localPosition = localPosition;
		RLWheel.ControlArmEnd.LookAt(RLWheel.ControlArmStart, base.transform.forward);
		RLWheel.ControlArmStart.LookAt(RLWheel.ControlArmEnd, base.transform.forward);
		perFrameRotation = base.wheelColliders[1].wheelCollider.perFrameRotation;
		RRWheel.BrakeDisk.Rotate(RRWheel.BrakeDisk.right, perFrameRotation, Space.World);
		RRWheel.SteeringAxis.localEulerAngles = new Vector3(0f, (0f - num) * Controls.RearSteering.FloatValue, 0f);
		localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition.z = vector2.z;
		Vector3 vector3 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].VisualWheel.position);
		localPosition.x = vector3.x;
		RRWheel.Dummy.localPosition = localPosition;
		localPosition = RearAxleDummy.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		RearAxleDummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		DriveshaftStart.Rotate(new Vector3(0f, 0f, perFrameRotation));
		DriveshaftEnd.Rotate(new Vector3(0f, 0f, perFrameRotation));
		PinionShaft.Rotate(new Vector3(perFrameRotation, 0f, 0f));
		MiddleDriveshaft.Rotate(new Vector3(0f, 0f, 0f - perFrameRotation));
		RRWheel.ControlArmEnd.LookAt(RRWheel.ControlArmStart, base.transform.forward);
		RRWheel.ControlArmStart.LookAt(RRWheel.ControlArmEnd, base.transform.forward);
		TrackBarEnd.LookAt(TrackBarStart, base.transform.forward);
		TrackBarStart.LookAt(TrackBarEnd, base.transform.forward);
		DoShocks();
	}
}
