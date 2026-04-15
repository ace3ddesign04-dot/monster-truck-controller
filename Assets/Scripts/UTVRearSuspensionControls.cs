using System;

[Serializable]
public class UTVRearSuspensionControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue AxisOffset = new SuspensionValue("Axis offset", ValueType.Float, 0f, 0);
}
