using System;
using UnityEngine;

[Serializable]
public class AGS_MTC_WheelsControls
{
	public float DefaultWheelColliderRadius = 0.4f;

	public int Stage;

	public bool TankTracks;

	public float TankTracksWheelCollidersRadius = 1.4f;

	public AGS_MTC_SuspensionValue Rim = new AGS_MTC_SuspensionValue("Rim", AGS_MTC_ValueType.Int, 0f, 0);

	public AGS_MTC_SuspensionValue Tire = new AGS_MTC_SuspensionValue("Tire", AGS_MTC_ValueType.Int, 0f, 0);

	public AGS_MTC_SuspensionValue RimSize = new AGS_MTC_SuspensionValue("Rim size", AGS_MTC_ValueType.Float, 1f, 0);

	public AGS_MTC_SuspensionValue WheelsRadius = new AGS_MTC_SuspensionValue("Wheels radius", AGS_MTC_ValueType.Float, 1f, 0);

	public AGS_MTC_SuspensionValue WheelsWidth = new AGS_MTC_SuspensionValue("Wheels width", AGS_MTC_ValueType.Float, 1f, 0);

	public AGS_MTC_SuspensionValue[] GetAllValues()
	{
		return new AGS_MTC_SuspensionValue[5]
		{
			Rim,
			Tire,
			RimSize,
			WheelsRadius,
			WheelsWidth
		};
	}

	public void SetAllValues(AGS_MTC_SuspensionValue[] values)
	{
        AGS_MTC_SuspensionValue[] allValues = GetAllValues();
		foreach (AGS_MTC_SuspensionValue suspensionValue in allValues)
		{
			foreach (AGS_MTC_SuspensionValue suspensionValue2 in values)
			{
				if (suspensionValue.ValueName == suspensionValue2.ValueName)
				{
					suspensionValue.ReceiveValues(suspensionValue2);
				}
			}
		}
	}

	public AGS_MTC_WheelsControls DeepCopy()
	{
        AGS_MTC_WheelsControls wheelsControls = new AGS_MTC_WheelsControls();
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

	public void SetRandom(AGS_MTC_Suspension suspension)
	{
        AGS_MTC_SuspensionControlLimit limit = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, "Rim");
		AGS_MTC_SuspensionControlLimit limit2 = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, "Tire");
		AGS_MTC_SuspensionControlLimit limit3 = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, "Rim size");
		AGS_MTC_SuspensionControlLimit limit4 = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, "Wheels radius");
		AGS_MTC_SuspensionControlLimit limit5 = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, "Wheels width");
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
