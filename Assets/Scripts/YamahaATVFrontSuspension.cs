using CustomVP;
using UnityEngine;

public class YamahaATVFrontSuspension : Suspension
{
	private CarController carController;

	public YamahaATVFrontWheel FLWheel;

	public YamahaATVFrontWheel FRWheel;

	public Transform SteeringRack;

	public YamahaATVFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[7]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.PerchWidth,
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
		FLWheel.KnuckleDefPos = FLWheel.Frame.InverseTransformPoint(FLWheel.Knuckle.position);
		FRWheel.KnuckleDefPos = FRWheel.Frame.InverseTransformPoint(FRWheel.Knuckle.position);
		OnValidate();
	}

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			DoPerches();
			DoWidth();
			DoWheelColliderParameters();
			DoWheelsOffset();
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
		FRWheel.ShockUps.LookAt(FRWheel.ShockDowns, -base.transform.right);
		FRWheel.ShockDowns.LookAt(FRWheel.ShockUps, -base.transform.right);
		FLWheel.ShockUps.LookAt(FLWheel.ShockDowns, -base.transform.right);
		FLWheel.ShockDowns.LookAt(FLWheel.ShockUps, -base.transform.right);
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

	private void DoWheelsOffset()
	{
		if (!(wheelColliders[0] == null) && !(wheelColliders[1] == null))
		{
			Transform wheelColliderHolder = FLWheel.WheelColliderHolder;
			float num = 0f - FLWheel.Deviation;
			Vector3 lossyScale = base.transform.lossyScale;
			wheelColliderHolder.localPosition = new Vector3(num * lossyScale.x, 0f, 0f);
			Transform transform = FRWheel.WheelColliderHolder.transform;
			float deviation = FRWheel.Deviation;
			Vector3 lossyScale2 = base.transform.lossyScale;
			transform.localPosition = new Vector3(deviation * lossyScale2.x, 0f, 0f);
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
		FLWheel.Frame.localPosition = new Vector3(0f, 0f - Controls.AxisWidth.FloatValue, 0f);
		FRWheel.Frame.localPosition = new Vector3(0f, Controls.AxisWidth.FloatValue, 0f);
	}

	private void DoPerches()
	{
		FLWheel.Perch.localPosition = new Vector3(0f, 0f - Controls.PerchWidth.FloatValue, 0f);
		FRWheel.Perch.localPosition = new Vector3(0f, Controls.PerchWidth.FloatValue, 0f);
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
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector.z - 0.1f;
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.LowerArm.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePosition.position;
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		FLWheel.UpperArm.LookAt(FLWheel.UpperArmTarget, base.transform.forward);
		YamahaATVFrontWheel fLWheel = FLWheel;
		Vector3 vector2 = FLWheel.Frame.InverseTransformPoint(FLWheel.Knuckle.position);
		fLWheel.Deviation = 0f - vector2.y + FLWheel.KnuckleDefPos.y;
		FRWheel.TieRod.LookAt(SteeringRack, base.transform.forward);
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector3 = FRWheel.Dummy.parent.InverseTransformPoint(position);
		localPosition.z = vector3.z - 0.1f;
		FRWheel.Dummy.localPosition = localPosition;
		FRWheel.LowerArm.LookAt(FRWheel.Dummy, base.transform.forward);
		FRWheel.Knuckle.position = FRWheel.KnucklePosition.position;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, SteerAngle);
		FRWheel.UpperArm.LookAt(FRWheel.UpperArmTarget, base.transform.forward);
		YamahaATVFrontWheel fRWheel = FRWheel;
		Vector3 vector4 = FRWheel.Frame.InverseTransformPoint(FRWheel.Knuckle.position);
		fRWheel.Deviation = vector4.y - FRWheel.KnuckleDefPos.y;
		DoShocks();
		DoWheelsOffset();
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
		FLWheel.TieRod.LookAt(SteeringRack, base.transform.forward);
		Vector3 localPosition = FLWheel.Dummy.localPosition;
		Vector3 vector = FLWheel.Dummy.parent.InverseTransformPoint(wheelColliders[0].GetVisualWheelPosition());
		localPosition.z = vector.z - 0.09f;
		Vector3 localPosition2 = FLWheel.Knuckle.localPosition;
		localPosition.y = localPosition2.y;
		if (localPosition.y > 0f)
		{
			localPosition.y = 0f - localPosition.y;
		}
		FLWheel.Dummy.localPosition = localPosition;
		FLWheel.LowerArm.LookAt(FLWheel.Dummy, base.transform.forward);
		FLWheel.Knuckle.position = FLWheel.KnucklePosition.position;
		FLWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FLWheel.UpperArm.LookAt(FLWheel.UpperArmTarget, base.transform.forward);
		YamahaATVFrontWheel fLWheel = FLWheel;
		Vector3 vector2 = FLWheel.Frame.InverseTransformPoint(FLWheel.Knuckle.position);
		fLWheel.Deviation = 0f - vector2.y + FLWheel.KnuckleDefPos.y;
		perFrameRotation = wheelColliders[1].wheelCollider.perFrameRotation;
		FRWheel.BrakeDisk.Rotate(FRWheel.BrakeDisk.up, perFrameRotation, Space.World);
		FRWheel.TieRod.LookAt(SteeringRack, base.transform.forward);
		Vector3 visualWheelPosition = wheelColliders[1].GetVisualWheelPosition();
		Vector3 position = FRWheel.LowerArm.parent.InverseTransformPoint(visualWheelPosition);
		Vector3 localPosition3 = FRWheel.LowerArm.localPosition;
		position.x = localPosition3.x;
		position.z -= 0.09f;
		float num2 = Vector3.Distance(FRWheel.LowerArm.position, FRWheel.KnucklePosition.position);
		float z = position.z;
		Vector3 localPosition4 = FRWheel.LowerArm.localPosition;
		float num3 = z - localPosition4.z;
		Vector3 localPosition5 = FRWheel.LowerArm.localPosition;
		position.y = localPosition5.y + Mathf.Sqrt(num2 * num2 - num3 * num3);
		float y = position.y;
		Vector3 lossyScale = base.transform.lossyScale;
		position.y = y / lossyScale.x;
		Vector3 vector3 = FRWheel.LowerArm.parent.TransformPoint(position);
		FRWheel.Dummy.position = vector3;
		Quaternion rotation = Quaternion.LookRotation(vector3 - FRWheel.LowerArm.position, base.transform.forward);
		FRWheel.LowerArm.rotation = rotation;
		localPosition = FRWheel.Dummy.localPosition;
		Vector3 vector4 = FRWheel.Dummy.parent.InverseTransformPoint(wheelColliders[1].GetVisualWheelPosition());
		localPosition.z = vector4.z - 0.09f;
		Vector3 localPosition6 = FRWheel.Knuckle.localPosition;
		localPosition.y = localPosition6.y;
		if (localPosition.y < 0f)
		{
			localPosition.y = 0f - localPosition.y;
		}
		FRWheel.Knuckle.position = FRWheel.KnucklePosition.position;
		FRWheel.Knuckle.localEulerAngles = new Vector3(0f, 0f, num);
		FRWheel.UpperArm.LookAt(FRWheel.UpperArmTarget, base.transform.forward);
		YamahaATVFrontWheel fRWheel = FRWheel;
		Vector3 vector5 = FRWheel.Frame.InverseTransformPoint(FRWheel.Knuckle.position);
		fRWheel.Deviation = vector5.y - FRWheel.KnuckleDefPos.y;
		DoShocks();
		DoWheelsOffset();
	}
}
