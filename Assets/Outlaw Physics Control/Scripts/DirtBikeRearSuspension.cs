using UnityEngine;

public class DirtBikeRearSuspension : Suspension
{
	public Transform Dummy;

	public Transform Arm;

	public Transform Body;

	public Transform[] Shocks;

	public Transform ShockUps;

	public Transform ShockDowns;

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
			DoShocks();
			ChangeShocks();
			side = Side.Rear;
			if (!(wheelColliders[0] == null))
			{
				wheelColliders[0].OnValidate();
			}
		}
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
		Vector3 localPosition = Dummy.transform.localPosition;
		Vector3 vector = Dummy.parent.InverseTransformPoint(position);
		localPosition.y = vector.y;
		Dummy.localPosition = localPosition;
		Arm.LookAt(Dummy, Body.up);
		WheelHolders[0].Rotate(WheelHolders[0].right, rpm, Space.World);
		DoShocks();
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
			Vector3 localPosition = Dummy.transform.localPosition;
			Vector3 vector = Dummy.parent.InverseTransformPoint(wheelColliders[0].GetVisualWheelPosition());
			localPosition.y = vector.y;
			Dummy.localPosition = localPosition;
			Arm.LookAt(Dummy, Body.up);
			float perFrameRotation = wheelColliders[0].wheelCollider.perFrameRotation;
			WheelHolders[0].Rotate(WheelHolders[0].right, perFrameRotation, Space.World);
			DoShocks();
		}
	}
}
