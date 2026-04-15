using CustomVP;
using UnityEngine;

public class SolidAxleDoubleRearSuspension : Suspension
{
	private CarController carController;

	public SolidAxleRearWheel RLWheel;

	public SolidAxleRearWheel RRWheel;

	public SolidAxleRearWheel RRLWheel;

	public SolidAxleRearWheel RRRWheel;

	public Transform RearAxleDummy;

	public Transform RearRearAxleDummy;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform Driveshaft3rdStart;

	public Transform Driveshaft3rdEnd;

	public Transform DriveshaftConnectingStart;

	public Transform DriveshaftConnectingEnd;

	public Transform DriveshaftTarget;

	public SolidAxleDoubleRearSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[12]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.FramesWidth,
			Controls.LeafSpringMountHeight,
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
			side = Side.Rear;
			DoWidth();
			DoSpringBrackets();
			DoShocksOffset();
			DoWheelColliderParameters();
			DoFramesWidth();
			DoLeafSprings();
			DoLeafSpringMountHeight();
			DoShocks();
			ChangeShocks();
			if (carController != null)
			{
				carController.InverseSteerMultiplier = Controls.RearSteering.FloatValue;
			}
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null) && !(wheelColliders[2] == null) && !(wheelColliders[3] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
				wheelColliders[2].OnValidate();
				wheelColliders[3].OnValidate();
			}
		}
	}

	private void DoShocks()
	{
		if (RRWheel.Shocks != null && !(RRWheel.ShockDowns == null) && !(RLWheel.ShockUps == null) && !(RLWheel.ShockDowns == null) && RRRWheel.Shocks != null && !(RRRWheel.ShockDowns == null) && !(RRLWheel.ShockUps == null) && !(RRLWheel.ShockDowns == null))
		{
			RRWheel.ShockUps.LookAt(RRWheel.ShockDowns, base.transform.right);
			RRWheel.ShockDowns.LookAt(RRWheel.ShockUps, -base.transform.right);
			RLWheel.ShockUps.LookAt(RLWheel.ShockDowns, -base.transform.right);
			RLWheel.ShockDowns.LookAt(RLWheel.ShockUps, base.transform.right);
			RRRWheel.ShockUps.LookAt(RRRWheel.ShockDowns, base.transform.right);
			RRRWheel.ShockDowns.LookAt(RRRWheel.ShockUps, -base.transform.right);
			RRLWheel.ShockUps.LookAt(RRLWheel.ShockDowns, -base.transform.right);
			RRLWheel.ShockDowns.LookAt(RRLWheel.ShockUps, base.transform.right);
			RLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRLWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRLWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRRWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
			RRRWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		}
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < RRWheel.Shocks.Length; i++)
		{
			RLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RRLWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RRRWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void DoSpringBrackets()
	{
		RLWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
		RRWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
		RRLWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
		RRRWheel.SpringBracket.localEulerAngles = new Vector3((Controls.SpringBracketsUpperMount.IntValue > 0) ? 180 : 0, 0f, 0f);
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null) && !(wheelColliders[2] == null) && !(wheelColliders[3] == null))
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
			RRLWheel.Axle.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
			RRRWheel.Axle.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
			Transform transform3 = RRLWheel.WheelColliderHolder.transform;
			float num2 = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale3 = base.transform.lossyScale;
			transform3.localPosition = new Vector3(num2 * lossyScale3.x, 0f, 0f);
			Transform transform4 = RRRWheel.WheelColliderHolder.transform;
			float floatValue2 = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale4 = base.transform.lossyScale;
			transform4.localPosition = new Vector3(floatValue2 * lossyScale4.x, 0f, 0f);
		}
	}

	private void DoShocksOffset()
	{
		RLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		RRWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		RRLWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
		RRRWheel.ShockUps.localPosition = new Vector3(Controls.ShocksOffset.FloatValue, 0f, 0f - Controls.ShocksHeight.FloatValue);
	}

	private void DoFramesWidth()
	{
		RLWheel.Frame.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRWheel.Frame.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		RLWheel.SpringBracket.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRWheel.SpringBracket.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		RRLWheel.Frame.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRRWheel.Frame.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
		RRLWheel.SpringBracket.localPosition = new Vector3(0f - Controls.FramesWidth.FloatValue, 0f, 0f);
		RRRWheel.SpringBracket.localPosition = new Vector3(Controls.FramesWidth.FloatValue, 0f, 0f);
	}

	private void DoLeafSpringMountHeight()
	{
		RLWheel.LeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RRWheel.LeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RRLWheel.LeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
		RRRWheel.LeafMount.localPosition = new Vector3(0f, 0f, Controls.LeafSpringMountHeight.FloatValue);
	}

	private void DoWheelColliderParameters()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null) && !(wheelColliders[2] == null) && !(wheelColliders[3] == null))
		{
			WheelComponent obj = wheelColliders[0];
			float floatValue = Controls.Travel.FloatValue;
			wheelColliders[1].suspensionLength = floatValue;
			obj.suspensionLength = floatValue;
			wheelColliders[0].spring = (wheelColliders[1].spring = Controls.Stiffness.FloatValue);
			wheelColliders[0].damper = (wheelColliders[1].damper = Controls.Damping.FloatValue);
			WheelComponent obj2 = wheelColliders[2];
			floatValue = Controls.Travel.FloatValue;
			wheelColliders[3].suspensionLength = floatValue;
			obj2.suspensionLength = floatValue;
			wheelColliders[2].spring = (wheelColliders[3].spring = Controls.Stiffness.FloatValue);
			wheelColliders[2].damper = (wheelColliders[3].damper = Controls.Damping.FloatValue);
		}
	}

	private void DoLeafSprings()
	{
		RLWheel.LeafSpringBone.position = RLWheel.LeafSpringPos.position;
		RRWheel.LeafSpringBone.position = RRWheel.LeafSpringPos.position;
		RRLWheel.LeafSpringBone.position = RRLWheel.LeafSpringPos.position;
		RRRWheel.LeafSpringBone.position = RRRWheel.LeafSpringPos.position;
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		RLWheel.BrakeDisk.Rotate(new Vector3(rpm, 0f, 0f));
		RRWheel.BrakeDisk.Rotate(new Vector3(rpm, 0f, 0f));
		RRLWheel.BrakeDisk.Rotate(new Vector3(rpm, 0f, 0f));
		RRRWheel.BrakeDisk.Rotate(new Vector3(rpm, 0f, 0f));
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RLWheel.Dummy.position = position;
		RLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue);
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
		RRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue);
		position = Raycasters[2].position - Raycasters[2].up * Controls.Travel.FloatValue;
		if (Physics.Raycast(Raycasters[2].position, -Raycasters[2].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RRLWheel.Dummy.position = position;
		RRLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue);
		position = Raycasters[3].position - Raycasters[3].up * Controls.Travel.FloatValue;
		if (Physics.Raycast(Raycasters[3].position, -Raycasters[3].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = RRRWheel.Dummy.localPosition;
		Vector3 vector3 = RRRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector3.z;
		Vector3 vector4 = RRRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.x = vector4.x;
		RRRWheel.Dummy.localPosition = localPosition;
		RRRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - SteerAngle) * Controls.RearSteering.FloatValue);
		DriveshaftStart.Rotate(0f, 0f - rpm, 0f);
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.forward);
		Driveshaft3rdStart.Rotate(0f, 0f - rpm, 0f);
		Driveshaft3rdEnd.LookAt(DriveshaftConnectingEnd, Driveshaft3rdStart.forward);
		DriveshaftConnectingStart.Rotate(0f, 0f - rpm, 0f);
		DriveshaftConnectingEnd.LookAt(Driveshaft3rdEnd, DriveshaftConnectingStart.forward);
		Vector3 localPosition2 = RearAxleDummy.localPosition;
		Vector3 localPosition3 = RLWheel.Dummy.localPosition;
		localPosition2.z = localPosition3.z;
		RearAxleDummy.localPosition = localPosition2;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		localPosition2 = RearRearAxleDummy.localPosition;
		Vector3 localPosition4 = RRLWheel.Dummy.localPosition;
		localPosition2.z = localPosition4.z;
		RearRearAxleDummy.localPosition = localPosition2;
		RearRearAxleDummy.LookAt(RRRWheel.Dummy, base.transform.forward);
		DoLeafSprings();
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
		float num2 = base.wheelColliders[0].rpm * Time.fixedDeltaTime * 16f;
		Vector3 lossyScale = base.transform.lossyScale;
		float x2 = num2 / lossyScale.x;
		RLWheel.BrakeDisk.Rotate(new Vector3(x2, 0f, 0f));
		RLWheel.Dummy.position = base.wheelColliders[0].GetVisualWheelPosition();
		RLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - num) * Controls.RearSteering.FloatValue);
		float num3 = base.wheelColliders[1].rpm * Time.fixedDeltaTime * 16f;
		Vector3 lossyScale2 = base.transform.lossyScale;
		x2 = num3 / lossyScale2.x;
		RRWheel.BrakeDisk.Rotate(new Vector3(x2, 0f, 0f));
		Vector3 localPosition = RRWheel.Dummy.localPosition;
		Vector3 vector = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector.z;
		Vector3 vector2 = RRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[1].GetVisualWheelPosition());
		localPosition.x = vector2.x;
		RRWheel.Dummy.localPosition = localPosition;
		RRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - num) * Controls.RearSteering.FloatValue);
		float num4 = base.wheelColliders[2].rpm * Time.fixedDeltaTime * 16f;
		Vector3 lossyScale3 = base.transform.lossyScale;
		x2 = num4 / lossyScale3.x;
		RRLWheel.Dummy.position = base.wheelColliders[2].GetVisualWheelPosition();
		RRLWheel.BrakeDisk.Rotate(new Vector3(x2, 0f, 0f));
		RRLWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - num) * Controls.RearSteering.FloatValue);
		float num5 = base.wheelColliders[3].rpm * Time.fixedDeltaTime * 16f;
		Vector3 lossyScale4 = base.transform.lossyScale;
		x2 = num5 / lossyScale4.x;
		RRRWheel.BrakeDisk.Rotate(new Vector3(x2, 0f, 0f));
		localPosition = RRRWheel.Dummy.localPosition;
		Vector3 vector3 = RRRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[3].GetVisualWheelPosition());
		localPosition.z = vector3.z;
		Vector3 vector4 = RRRWheel.Dummy.parent.InverseTransformPoint(base.wheelColliders[3].GetVisualWheelPosition());
		localPosition.x = vector4.x;
		RRRWheel.Dummy.localPosition = localPosition;
		RRRWheel.SteeringAxle.localEulerAngles = new Vector3(0f, 0f, (0f - num) * Controls.RearSteering.FloatValue);
		DriveshaftStart.Rotate(0f, x2, 0f);
		DriveshaftEnd.LookAt(DriveshaftTarget, DriveshaftStart.forward);
		Driveshaft3rdStart.Rotate(0f, x2, 0f);
		Driveshaft3rdEnd.LookAt(DriveshaftConnectingEnd, Driveshaft3rdStart.forward);
		DriveshaftConnectingStart.Rotate(0f, x2, 0f);
		DriveshaftConnectingEnd.LookAt(Driveshaft3rdEnd, DriveshaftConnectingStart.forward);
		Vector3 localPosition2 = RearAxleDummy.localPosition;
		Vector3 localPosition3 = RLWheel.Dummy.localPosition;
		localPosition2.z = localPosition3.z;
		RearAxleDummy.localPosition = localPosition2;
		RearAxleDummy.LookAt(RRWheel.Dummy, base.transform.forward);
		localPosition2 = RearRearAxleDummy.localPosition;
		Vector3 localPosition4 = RRLWheel.Dummy.localPosition;
		localPosition2.z = localPosition4.z;
		RearRearAxleDummy.localPosition = localPosition2;
		RearRearAxleDummy.LookAt(RRRWheel.Dummy, base.transform.forward);
		DoLeafSprings();
		DoShocks();
	}
}
