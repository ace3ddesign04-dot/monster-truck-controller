using CustomVP;
using UnityEngine;

public class CrawlerSolidAxleSuspension : Suspension
{
	public Side SuspensionSide;

	private CarController carController;

	public CrawlerSolidAxleWheel LWheel;

	public CrawlerSolidAxleWheel RWheel;

	public Transform RegularDriveshaft1Start;

	public Transform RegularDriveshaft1End;

	public Transform RegularDriveshaft2Start;

	public Transform RegularDriveshaft2End;

	public Transform RockwellDriveshaft1End;

	public Transform RockwellDriveshaft2Start;

	public Transform RockwellDriveshaft2End;

	public Transform RockwellShaft;

	public Transform RegularAxle;

	public Transform RockwellAxle;

	public Transform RegularBrake;

	public Transform PinionBrake;

	public Transform SteeringRodStart;

	public Transform SteeringRodEnd;

	public Transform AxleDummy;

	public Transform SteeringRack;

	[Space(10f)]
	public CrawlerSolidAxleControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[9]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.Stiffness,
			Controls.Travel,
			Controls.RearSteering,
			Controls.AxleType,
			Controls.BrakeType
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
		OnValidate();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			side = SuspensionSide;
			DoWidth();
			DoWheelColliderParameters();
			ChangeShocks();
			DoShocks();
			SwitchModels();
			if (carController != null && SuspensionSide == Side.Rear)
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
	}

	private void DoShocks()
	{
		RWheel.ShockUps.LookAt(RWheel.ShockDowns, -base.transform.right);
		RWheel.ShockDowns.LookAt(RWheel.ShockUps, base.transform.right);
		LWheel.ShockUps.LookAt(LWheel.ShockDowns, -base.transform.right);
		LWheel.ShockDowns.LookAt(LWheel.ShockUps, base.transform.right);
		LWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		LWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		RWheel.ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		RWheel.ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < RWheel.Shocks.Length; i++)
		{
			LWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
			RWheel.Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
	}

	private void DoWidth()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			LWheel.Axle.localPosition = new Vector3(0f, 0f - Controls.AxisWidth.FloatValue, 0f);
			RWheel.Axle.localPosition = new Vector3(0f, Controls.AxisWidth.FloatValue, 0f);
			Transform transform = LWheel.WheelColliderHolder.transform;
			float num = 0f - Controls.AxisWidth.FloatValue;
			Vector3 lossyScale = base.transform.lossyScale;
			transform.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform2 = RWheel.WheelColliderHolder.transform;
			float floatValue = Controls.AxisWidth.FloatValue;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform2.localPosition = new Vector3(floatValue * lossyScale2.x, 0f, 0f);
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
		if (SuspensionSide == Side.Rear)
		{
			rpm = 0f - rpm;
			SteerAngle = (0f - SteerAngle) * Controls.RearSteering.FloatValue;
		}
		LWheel.BrakeDisk.Rotate(LWheel.BrakeDisk.up, rpm, Space.World);
		RWheel.BrakeDisk.Rotate(RWheel.BrakeDisk.up, rpm, Space.World);
		LWheel.Joint.Rotate(0f, rpm, 0f);
		LWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		LWheel.TieRod.LookAt(RWheel.TieRod, base.transform.up);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		LWheel.Dummy.position = position;
		LWheel.ArmEnd.LookAt(LWheel.ArmStart, base.transform.up);
		LWheel.ArmStart.LookAt(LWheel.ArmEnd, base.transform.up);
		RWheel.Joint.Rotate(0f, rpm, 0f);
		RWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		RWheel.TieRod.LookAt(LWheel.TieRod, base.transform.up);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RWheel.Dummy.position = position;
		RWheel.ArmEnd.LookAt(RWheel.ArmStart, base.transform.up);
		RWheel.ArmStart.LookAt(RWheel.ArmEnd, base.transform.up);
		Vector3 localPosition = AxleDummy.localPosition;
		Vector3 localPosition2 = LWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		AxleDummy.localPosition = localPosition;
		AxleDummy.LookAt(RWheel.Dummy, base.transform.up);
		RegularDriveshaft1Start.Rotate(new Vector3(0f, 0f, rpm));
		RegularDriveshaft1End.LookAt(RegularDriveshaft2End, RegularDriveshaft1Start.up);
		RegularDriveshaft2Start.Rotate(new Vector3(0f, 0f, 0f - rpm));
		RegularDriveshaft2End.LookAt(RegularDriveshaft1End, RegularDriveshaft2Start.up);
		RockwellShaft.Rotate(new Vector3(rpm, 0f, 0f));
		RockwellDriveshaft1End.LookAt(RockwellDriveshaft2End, RockwellShaft.up);
		RockwellDriveshaft2Start.Rotate(new Vector3(0f, 0f, rpm));
		RockwellDriveshaft2End.LookAt(RockwellDriveshaft1End, RockwellDriveshaft2Start.up);
		SteeringRodEnd.LookAt(SteeringRodStart, base.transform.up);
		SteeringRodStart.LookAt(SteeringRodEnd, base.transform.up);
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(Mathf.LerpUnclamped(-0.1f, 0f, SteerAngle / carController.maxSteeringAngle + 1f), 0f, 0f);
		}
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
			num = ((SuspensionSide != 0) ? ((0f - carController.Steering) * carController.InverseSteerMultiplier) : carController.Steering);
		}
		float num2 = base.wheelColliders[0].wheelCollider.perFrameRotation;
		if (SuspensionSide == Side.Rear)
		{
			num2 = 0f - num2;
		}
		LWheel.BrakeDisk.Rotate(LWheel.BrakeDisk.up, num2, Space.World);
		LWheel.Joint.Rotate(0f, num2, 0f);
		LWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		LWheel.TieRod.LookAt(RWheel.TieRod, base.transform.up);
		LWheel.Dummy.position = base.wheelColliders[0].GetVisualWheelPosition();
		LWheel.ArmEnd.LookAt(LWheel.ArmStart, base.transform.up);
		LWheel.ArmStart.LookAt(LWheel.ArmEnd, base.transform.up);
		num2 = base.wheelColliders[1].wheelCollider.perFrameRotation;
		if (SuspensionSide == Side.Rear)
		{
			num2 = 0f - num2;
		}
		RWheel.BrakeDisk.Rotate(RWheel.BrakeDisk.up, num2, Space.World);
		RWheel.Joint.Rotate(0f, num2, 0f);
		RWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		RWheel.TieRod.LookAt(LWheel.TieRod, base.transform.up);
		RWheel.Dummy.position = base.wheelColliders[1].GetVisualWheelPosition();
		RWheel.ArmEnd.LookAt(RWheel.ArmStart, base.transform.up);
		RWheel.ArmStart.LookAt(RWheel.ArmEnd, base.transform.up);
		Vector3 localPosition = AxleDummy.localPosition;
		Vector3 localPosition2 = LWheel.Dummy.localPosition;
		localPosition.z = localPosition2.z;
		AxleDummy.localPosition = localPosition;
		AxleDummy.LookAt(RWheel.Dummy, base.transform.up);
		RegularDriveshaft1Start.Rotate(new Vector3(0f, 0f, num2));
		RegularDriveshaft1End.LookAt(RegularDriveshaft2End, RegularDriveshaft1Start.up);
		RegularDriveshaft2Start.Rotate(new Vector3(0f, 0f, 0f - num2));
		RegularDriveshaft2End.LookAt(RegularDriveshaft1End, RegularDriveshaft2Start.up);
		RockwellShaft.Rotate(new Vector3(num2, 0f, 0f));
		RockwellDriveshaft1End.LookAt(RockwellDriveshaft2End, RockwellShaft.up);
		RockwellDriveshaft2Start.Rotate(new Vector3(0f, 0f, num2));
		RockwellDriveshaft2End.LookAt(RockwellDriveshaft1End, RockwellDriveshaft2Start.up);
		SteeringRodEnd.LookAt(SteeringRodStart, base.transform.up);
		SteeringRodStart.LookAt(SteeringRodEnd, base.transform.up);
		if (carController != null)
		{
			SteeringRack.localPosition = new Vector3(Mathf.LerpUnclamped(-0.1f, 0f, num / carController.maxSteeringAngle + 1f), 0f, 0f);
		}
		DoShocks();
	}
}
