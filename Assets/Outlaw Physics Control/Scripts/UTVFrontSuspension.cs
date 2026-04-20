using CustomVP;
using UnityEngine;

public class UTVFrontSuspension : Suspension
{
	private CarController carController;

	public UTVFrontWheel FLWheel;

	public UTVFrontWheel FRWheel;

	public Transform SteeringRack;

	public UTVFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[6]
		{
			Controls.AxisWidth,
			Controls.Damping,
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
		carController = GetComponentInParent<CarController>();
		OnValidate();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			DoWidth();
			DoWheelColliderParameters();
			DoShocks();
			ChangeShocks();
			side = Side.Front;
			if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
			{
				wheelColliders[0].OnValidate();
				wheelColliders[1].OnValidate();
			}
		}
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

	private void DoWidth()
	{
		Transform lowerArmEnd = FLWheel.LowerArmEnd;
		float floatValue = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale = FLWheel.LowerArmEnd.lossyScale;
		lowerArmEnd.localPosition = new Vector3(0f, 0f, floatValue / lossyScale.x);
		Transform upperArmEnd = FLWheel.UpperArmEnd;
		float floatValue2 = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale2 = FLWheel.LowerArmEnd.lossyScale;
		upperArmEnd.localPosition = new Vector3(0f, 0f, floatValue2 / lossyScale2.x);
		Transform lowerArmEnd2 = FRWheel.LowerArmEnd;
		float floatValue3 = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale3 = FRWheel.LowerArmEnd.lossyScale;
		lowerArmEnd2.localPosition = new Vector3(0f, 0f, floatValue3 / lossyScale3.x);
		Transform upperArmEnd2 = FRWheel.UpperArmEnd;
		float floatValue4 = Controls.AxisWidth.FloatValue;
		Vector3 lossyScale4 = FRWheel.LowerArmEnd.lossyScale;
		upperArmEnd2.localPosition = new Vector3(0f, 0f, floatValue4 / lossyScale4.x);
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.up, rpm, Space.World);
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.up, rpm, Space.World);
		FLWheel.TieRod.LookAt(SteeringRack, base.transform.up);
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 position2 = FLWheel.LowerArmStart.parent.InverseTransformPoint(position);
		Vector3 localPosition = FLWheel.LowerArmStart.localPosition;
		position2.x = localPosition.x;
		position2.z -= 0.36f;
		Vector3 lossyScale = FLWheel.LowerArmStart.lossyScale;
		float x = lossyScale.x;
		float num = Vector3.Distance(FLWheel.LowerArmStart.position, FLWheel.LowerArmEnd.position);
		float z = position2.z;
		Vector3 localPosition2 = FLWheel.LowerArmStart.localPosition;
		float num2 = (z - localPosition2.z) * x;
		Vector3 localPosition3 = FLWheel.LowerArmStart.localPosition;
		position2.y = localPosition3.y - Mathf.Sqrt(num * num - num2 * num2) / x;
		Vector3 a = FLWheel.LowerArmStart.parent.TransformPoint(position2);
		if (!float.IsNaN(a.x))
		{
			Quaternion rotation = Quaternion.LookRotation(a - FLWheel.LowerArmStart.position, base.transform.up);
			FLWheel.LowerArmStart.rotation = rotation;
		}
		Vector3 localEulerAngles = FLWheel.LowerArmStart.localEulerAngles;
		if (localEulerAngles.x < 25f)
		{
			Vector3 localEulerAngles2 = FLWheel.LowerArmStart.localEulerAngles;
			if (localEulerAngles2.y > 90f)
			{
				Vector3 localEulerAngles3 = FLWheel.LowerArmStart.localEulerAngles;
				if (localEulerAngles3.z > 90f)
				{
					FLWheel.LowerArmStart.localEulerAngles = new Vector3(25f, 180f, 180f);
				}
			}
		}
		FLWheel.Knuckle.position = FLWheel.KnucklePosition.position;
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		FLWheel.UpperArmStart.LookAt(FLWheel.UpperArmTarget, base.transform.up);
		FRWheel.TieRod.LookAt(SteeringRack, base.transform.up);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Vector3 lossyScale2 = FRWheel.LowerArmStart.lossyScale;
		x = lossyScale2.x;
		FRWheel.TieRod.LookAt(SteeringRack, base.transform.up);
		position2 = FRWheel.LowerArmStart.parent.InverseTransformPoint(position);
		Vector3 localPosition4 = FRWheel.LowerArmStart.localPosition;
		position2.x = localPosition4.x;
		position2.z -= 0.36f;
		num = Vector3.Distance(FRWheel.LowerArmStart.position, FRWheel.LowerArmEnd.position);
		float z2 = position2.z;
		Vector3 localPosition5 = FRWheel.LowerArmStart.localPosition;
		num2 = (z2 - localPosition5.z) * x;
		Vector3 localPosition6 = FRWheel.LowerArmStart.localPosition;
		position2.y = localPosition6.y + Mathf.Sqrt(num * num - num2 * num2) / x;
		a = FRWheel.LowerArmStart.parent.TransformPoint(position2);
		if (!float.IsNaN(a.x))
		{
			Quaternion rotation2 = Quaternion.LookRotation(a - FRWheel.LowerArmStart.position, base.transform.up);
			FRWheel.LowerArmStart.rotation = rotation2;
		}
		Vector3 localEulerAngles4 = FRWheel.LowerArmStart.localEulerAngles;
		if (localEulerAngles4.x > 335f)
		{
			Vector3 localEulerAngles5 = FRWheel.LowerArmStart.localEulerAngles;
			if (localEulerAngles5.y > 90f)
			{
				FRWheel.LowerArmStart.localEulerAngles = new Vector3(335f, 180f, 0f);
			}
		}
		FRWheel.Knuckle.position = FRWheel.KnucklePosition.position;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		FRWheel.UpperArmStart.LookAt(FRWheel.UpperArmTarget, base.transform.up);
		DoShocks();
	}

	private void FixedUpdate()
	{
		if (NoWheelColliders)
		{
			return;
		}
		if (wheelColliders[0] == null || wheelColliders[1] == null)
		{
			NoWheelColliders = true;
			return;
		}
		float num = 0f;
		if (carController != null)
		{
			num = carController.Steering;
			SteeringRack.localPosition = new Vector3(0f, Mathf.Lerp(-0.03f, 0.03f, (0f - num + carController.maxSteeringAngle) / (carController.maxSteeringAngle * 2f)), 0f);
		}
		float perFrameRotation = wheelColliders[0].wheelCollider.perFrameRotation;
		FLWheel.BrakeDisk.Rotate(FLWheel.BrakeDisk.up, perFrameRotation, Space.World);
		FLWheel.TieRod.LookAt(SteeringRack, base.transform.up);
		Vector3 position = FLWheel.LowerArmStart.parent.InverseTransformPoint(wheelColliders[0].GetVisualWheelPosition());
		Vector3 localPosition = FLWheel.LowerArmStart.localPosition;
		position.x = localPosition.x;
		position.z -= 0.36f;
		Vector3 lossyScale = FLWheel.LowerArmStart.lossyScale;
		float x = lossyScale.x;
		float num2 = Vector3.Distance(FLWheel.LowerArmStart.position, FLWheel.LowerArmEnd.position);
		float z = position.z;
		Vector3 localPosition2 = FLWheel.LowerArmStart.localPosition;
		float num3 = (z - localPosition2.z) * x;
		Vector3 localPosition3 = FLWheel.LowerArmStart.localPosition;
		position.y = localPosition3.y - Mathf.Sqrt(num2 * num2 - num3 * num3) / x;
		Vector3 a = FLWheel.LowerArmStart.parent.TransformPoint(position);
		if (!float.IsNaN(a.x))
		{
			Quaternion rotation = Quaternion.LookRotation(a - FLWheel.LowerArmStart.position, base.transform.up);
			FLWheel.LowerArmStart.rotation = rotation;
		}
		Vector3 localEulerAngles = FLWheel.LowerArmStart.localEulerAngles;
		if (localEulerAngles.x < 25f)
		{
			Vector3 localEulerAngles2 = FLWheel.LowerArmStart.localEulerAngles;
			if (localEulerAngles2.y > 90f)
			{
				Vector3 localEulerAngles3 = FLWheel.LowerArmStart.localEulerAngles;
				if (localEulerAngles3.z > 90f)
				{
					FLWheel.LowerArmStart.localEulerAngles = new Vector3(25f, 180f, 180f);
				}
			}
		}
		FLWheel.Knuckle.position = FLWheel.KnucklePosition.position;
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FLWheel.UpperArmStart.LookAt(FLWheel.UpperArmTarget, base.transform.up);
		Transform wheelColliderHolder = FLWheel.WheelColliderHolder;
		Vector3 vector = FLWheel.WheelColliderHolder.parent.InverseTransformPoint(WheelHolders[0].position);
		wheelColliderHolder.localPosition = new Vector3(vector.x, 0f, 0f);
		perFrameRotation = wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.up, perFrameRotation, Space.World);
		Vector3 lossyScale2 = FRWheel.LowerArmStart.lossyScale;
		x = lossyScale2.x;
		FRWheel.TieRod.LookAt(SteeringRack, base.transform.up);
		position = FRWheel.LowerArmStart.parent.InverseTransformPoint(wheelColliders[1].GetVisualWheelPosition());
		Vector3 localPosition4 = FRWheel.LowerArmStart.localPosition;
		position.x = localPosition4.x;
		position.z -= 0.36f;
		num2 = Vector3.Distance(FRWheel.LowerArmStart.position, FRWheel.LowerArmEnd.position);
		float z2 = position.z;
		Vector3 localPosition5 = FRWheel.LowerArmStart.localPosition;
		num3 = (z2 - localPosition5.z) * x;
		Vector3 localPosition6 = FRWheel.LowerArmStart.localPosition;
		position.y = localPosition6.y + Mathf.Sqrt(num2 * num2 - num3 * num3) / x;
		a = FRWheel.LowerArmStart.parent.TransformPoint(position);
		if (!float.IsNaN(a.x))
		{
			Quaternion rotation2 = Quaternion.LookRotation(a - FRWheel.LowerArmStart.position, base.transform.up);
			FRWheel.LowerArmStart.rotation = rotation2;
		}
		Vector3 localEulerAngles4 = FRWheel.LowerArmStart.localEulerAngles;
		if (localEulerAngles4.x > 335f)
		{
			Vector3 localEulerAngles5 = FRWheel.LowerArmStart.localEulerAngles;
			if (localEulerAngles5.y > 90f)
			{
				FRWheel.LowerArmStart.localEulerAngles = new Vector3(335f, 180f, 0f);
			}
		}
		FRWheel.Knuckle.position = FRWheel.KnucklePosition.position;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FRWheel.UpperArmStart.LookAt(FRWheel.UpperArmTarget, base.transform.up);
		Transform wheelColliderHolder2 = FRWheel.WheelColliderHolder;
		Vector3 vector2 = FRWheel.WheelColliderHolder.parent.InverseTransformPoint(WheelHolders[1].position);
		wheelColliderHolder2.localPosition = new Vector3(vector2.x, 0f, 0f);
		DoShocks();
	}
}
