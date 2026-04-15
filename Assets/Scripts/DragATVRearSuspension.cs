using CustomVP;
using UnityEngine;

public class DragATVRearSuspension : Suspension
{
	public DragATVRearWheel RLWheel;

	public DragATVRearWheel RRWheel;

	public Transform RearAxleDummy;

	public Transform RearAxleTarget;

	public Transform DriveshaftStart;

	public Transform DriveshaftEnd;

	public Transform RotationAxle;

	public Transform RearAxleHolder;

	public Transform RearArmsStart;

	public Transform ShockDowns;

	public Transform ShockUps;

	public Transform[] Shocks;

	public DragATVRearSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	private void Start()
	{
		OnValidate();
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
			wheelColliders[0].OnValidate();
			wheelColliders[1].OnValidate();
		}
	}

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[8]
		{
			Controls.AxisWidth,
			Controls.Damping,
			Controls.RearAxleOffset,
			Controls.ShocksGroup,
			Controls.ShocksSize,
			Controls.ShocksUpsHeight,
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

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			DoRearAxleOffset();
			DoWheelColliderParameters();
			DoAxisWidth();
			ChangeShocks();
			DoShocks();
			side = Side.Rear;
		}
	}

	private void DoRearAxleOffset()
	{
		RearAxleHolder.localPosition = new Vector3(Controls.RearAxleOffset.FloatValue, 0f, 0f);
		DriveshaftEnd.localPosition = new Vector3(0f, 0f, Controls.RearAxleOffset.FloatValue);
		RearArmsStart.localPosition = new Vector3(0f, 0f, 0f - Controls.RearAxleOffset.FloatValue);
	}

	private void DoAxisWidth()
	{
		RLWheel.AxleEnd.localPosition = new Vector3(0f, 0f - Controls.AxisWidth.FloatValue, 0f);
		RRWheel.AxleEnd.localPosition = new Vector3(0f, Controls.AxisWidth.FloatValue, 0f);
		RLWheel.WheelColliderHolder.localPosition = new Vector3(0f - Controls.AxisWidth.FloatValue, 0f, 0f);
		RRWheel.WheelColliderHolder.localPosition = new Vector3(Controls.AxisWidth.FloatValue, 0f, 0f);
	}

	private void DoShocks()
	{
		ShockDowns.LookAt(ShockUps, base.transform.right);
		ShockUps.LookAt(ShockDowns, base.transform.right);
	}

	private void ChangeShocks()
	{
		for (int i = 0; i < Shocks.Length; i++)
		{
			Shocks[i].gameObject.SetActive(i == Controls.ShocksGroup.IntValue);
		}
		ShockDowns.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
		ShockUps.localScale = new Vector3(Controls.ShocksSize.FloatValue, Controls.ShocksSize.FloatValue, 1f);
	}

	private void FixedUpdate()
	{
		if (!NoWheelColliders)
		{
			if (wheelColliders[0] == null || wheelColliders[1] == null)
			{
				NoWheelColliders = true;
				return;
			}
			float perFrameRotation = wheelColliders[0].wheelCollider.perFrameRotation;
			RLWheel.Dummy.position = wheelColliders[0].GetVisualWheelPosition();
			RRWheel.Dummy.position = wheelColliders[1].GetVisualWheelPosition();
			Vector3 localPosition = RearAxleDummy.localPosition;
			Vector3 localPosition2 = RLWheel.Dummy.localPosition;
			localPosition.y = localPosition2.y;
			RearAxleDummy.localPosition = localPosition;
			RearAxleDummy.LookAt(RRWheel.Dummy, RearAxleTarget.position - RearAxleDummy.position);
			RotationAxle.Rotate(RotationAxle.right, perFrameRotation, Space.World);
			DriveshaftStart.Rotate(0f, 0f, perFrameRotation);
			DoShocks();
		}
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RLWheel.Dummy.position = position;
		position = Raycasters[1].position - Raycasters[1].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[1].position, -Raycasters[1].up, out hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		RRWheel.Dummy.position = position;
		Vector3 localPosition = RearAxleDummy.localPosition;
		Vector3 localPosition2 = RLWheel.Dummy.localPosition;
		localPosition.y = localPosition2.y;
		RearAxleDummy.localPosition = localPosition;
		RearAxleDummy.LookAt(RRWheel.Dummy, RearAxleTarget.position - RearAxleDummy.position);
		RotationAxle.Rotate(RotationAxle.right, rpm, Space.World);
		DriveshaftStart.Rotate(0f, 0f, rpm);
		DoShocks();
	}
}
