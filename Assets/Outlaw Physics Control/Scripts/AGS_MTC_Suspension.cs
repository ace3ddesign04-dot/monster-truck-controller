using AGS_MonsterTruckControl;
using UnityEngine;

public class AGS_MTC_Suspension : MonoBehaviour
{
	public string SuspensionName;

	public int UpgradeStage;

	public bool DontLoadWheels;

	public bool DirtBikeWheels;

	public bool ATVWheels;

	[HideInInspector]
	public AGS_MTC_Side side;

	public AGS_MTC_WheelComponent[] wheelColliders;

	public Transform[] Raycasters;

	public Transform[] WheelHolders;

	public virtual void UpdateSuspension(float SteerAngle, float WheelRadius, float rpm)
	{
	}

	public virtual float[] ExportData()
	{
		return null;
	}

	public virtual AGS_MTC_SuspensionValue[] GetControlValues()
	{
		return null;
	}

	public virtual void SetControlValues(AGS_MTC_SuspensionValue[] values)
	{
	}

	public virtual void OnValidate()
	{
	}
}
