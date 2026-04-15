using System;
using UnityEngine;

[Serializable]
public class WheelsControls
{
	public float DefaultWheelColliderRadius = 0.4f;

	public int Stage;

	public bool TankTracks;

	public float TankTracksWheelCollidersRadius = 1.4f;

	public SuspensionValue Rim = new SuspensionValue("Rim", ValueType.Int, 0f, 0);

	public SuspensionValue Tire = new SuspensionValue("Tire", ValueType.Int, 0f, 0);

	public SuspensionValue RimSize = new SuspensionValue("Rim size", ValueType.Float, 1f, 0);

	public SuspensionValue WheelsRadius = new SuspensionValue("Wheels radius", ValueType.Float, 1f, 0);

	public SuspensionValue WheelsWidth = new SuspensionValue("Wheels width", ValueType.Float, 1f, 0);

	public SuspensionValue[] GetAllValues()
	{
		return new SuspensionValue[5]
		{
			Rim,
			Tire,
			RimSize,
			WheelsRadius,
			WheelsWidth
		};
	}

	public void SetAllValues(SuspensionValue[] values)
	{
		SuspensionValue[] allValues = GetAllValues();
		foreach (SuspensionValue suspensionValue in allValues)
		{
			foreach (SuspensionValue suspensionValue2 in values)
			{
				if (suspensionValue.ValueName == suspensionValue2.ValueName)
				{
					suspensionValue.ReceiveValues(suspensionValue2);
				}
			}
		}
	}

	public WheelsControls DeepCopy()
	{
		WheelsControls wheelsControls = new WheelsControls();
		wheelsControls.DefaultWheelColliderRadius = DefaultWheelColliderRadius;
		wheelsControls.Rim = Rim.DeepCopy();
		wheelsControls.RimSize = RimSize.DeepCopy();
		wheelsControls.Tire = Tire.DeepCopy();
		wheelsControls.WheelsRadius = WheelsRadius.DeepCopy();
		wheelsControls.WheelsWidth = WheelsWidth.DeepCopy();
		return wheelsControls;
	}

	public void SetStock()
	{
		Rim.IntValue = 0;
		Tire.IntValue = 0;
		RimSize.FloatValue = 1f;
		WheelsRadius.FloatValue = 1f;
		WheelsWidth.FloatValue = 1f;
	}

	public void SetRandom(Suspension suspension)
	{
		SuspensionControlLimit limit = SuspensionControlLimits.getLimit(suspension.gameObject.name, "Rim");
		SuspensionControlLimit limit2 = SuspensionControlLimits.getLimit(suspension.gameObject.name, "Tire");
		SuspensionControlLimit limit3 = SuspensionControlLimits.getLimit(suspension.gameObject.name, "Rim size");
		SuspensionControlLimit limit4 = SuspensionControlLimits.getLimit(suspension.gameObject.name, "Wheels radius");
		SuspensionControlLimit limit5 = SuspensionControlLimits.getLimit(suspension.gameObject.name, "Wheels width");
		Rim.IntValue = UnityEngine.Random.Range(0, limit.iMax);
		Tire.IntValue = UnityEngine.Random.Range(0, limit2.iMax);
		float max = 1f + (limit3.fMax - 1f) / 5f * (float)(Stage + 1);
		float min = 1f - (1f - limit3.fMin) / 5f * (float)(Stage + 1);
		if (limit.iMax > 0)
		{
			RimSize.FloatValue = UnityEngine.Random.Range(min, max);
		}
		float max2 = 1f + (limit4.fMax - 1f) / (float)(5 - Stage);
		float min2 = 1f - (1f - limit4.fMin) / (float)(5 - Stage);
		WheelsRadius.FloatValue = UnityEngine.Random.Range(min2, max2);
		float max3 = 1f + (limit5.fMax - 1f) / (float)(5 - Stage);
		float min3 = 1f - (1f - limit5.fMin) / (float)(5 - Stage);
		WheelsWidth.FloatValue = UnityEngine.Random.Range(min3, max3);
	}
}
