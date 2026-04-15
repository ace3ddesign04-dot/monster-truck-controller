using System;

[Serializable]
public class FordAFrontSuspensionControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);
}
