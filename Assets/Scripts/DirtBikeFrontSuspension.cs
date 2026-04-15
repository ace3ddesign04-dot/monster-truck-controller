using UnityEngine;

public class DirtBikeFrontSuspension : Suspension
{
	public Transform Dummy;

	public Transform Hub;

	public DirtBikeFrontSuspensionControls Controls;

	private bool NoWheelColliders;

	private Vector3 previousPosition;

	private float previousUpdateTime;

	public override SuspensionValue[] GetControlValues()
	{
		return new SuspensionValue[5]
		{
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

	public override void OnValidate()
	{
		if (base.isActiveAndEnabled)
		{
			DoWheelColliderParameters();
			side = Side.Front;
			if (!(wheelColliders[0] == null))
			{
				wheelColliders[0].OnValidate();
			}
		}
	}

	private void DoWheelColliderParameters()
	{
		if (!(wheelColliders[0] == null))
		{
			wheelColliders[0].suspensionLength = Controls.Travel.FloatValue;
			wheelColliders[0].spring = Controls.Stiffness.FloatValue;
			wheelColliders[0].damper = Controls.Damping.FloatValue;
		}
	}

	public override void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
		Vector3 position = Raycasters[0].position - Raycasters[0].up * (Controls.Travel.FloatValue + 0.2f);
		if (Physics.Raycast(Raycasters[0].position, -Raycasters[0].up, out RaycastHit hitInfo) && hitInfo.distance < Controls.Travel.FloatValue + WheelRadius + 0.2f)
		{
			position = hitInfo.point + new Vector3(0f, WheelRadius, 0f);
		}
		Dummy.position = position;
		Vector3 localPosition = Hub.localPosition;
		Vector3 vector = Hub.parent.InverseTransformPoint(Dummy.position);
		localPosition.z = vector.z;
		Hub.localPosition = localPosition;
		WheelHolders[0].Rotate(WheelHolders[0].right, rpm, Space.World);
	}

	private void FixedUpdate()
	{
		if (!NoWheelColliders)
		{
			if (wheelColliders[0] == null)
			{
				NoWheelColliders = true;
				return;
			}
			Dummy.position = wheelColliders[0].GetVisualWheelPosition();
			Vector3 localPosition = Dummy.localPosition;
			Vector3 vector = Dummy.parent.InverseTransformPoint(Hub.position);
			localPosition.z = vector.z;
			Dummy.localPosition = localPosition;
			localPosition = Hub.localPosition;
			Vector3 vector2 = Hub.parent.InverseTransformPoint(Dummy.position);
			localPosition.z = vector2.z;
			Hub.localPosition = localPosition;
			float perFrameRotation = wheelColliders[0].wheelCollider.perFrameRotation;
			WheelHolders[0].Rotate(WheelHolders[0].right, perFrameRotation, Space.World);
		}
	}
}
