using System;

[Serializable]
public class SuspensionControllerData
{
	public int SelectedFrontSuspension;

	public int SelectedRearSuspension;

	public SuspensionData[] AllSuspensionsDatas;

	public WheelsControls FrontWheelsControls;

	public WheelsControls RearWheelsControls;
}
