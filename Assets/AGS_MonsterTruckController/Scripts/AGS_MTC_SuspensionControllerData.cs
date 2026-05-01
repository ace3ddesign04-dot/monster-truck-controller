using System;

[Serializable]
public class AGS_MTC_SuspensionControllerData
{
	public int SelectedFrontSuspension;

	public int SelectedRearSuspension;

	public AGS_MTC_SuspensionData[] AllSuspensionsDatas;

	public AGS_MTC_WheelsControls FrontWheelsControls;

	public AGS_MTC_WheelsControls RearWheelsControls;
}
