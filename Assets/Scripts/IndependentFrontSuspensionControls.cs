using System;

[Serializable]
public class IndependentFrontSuspensionControls : SuspensionControls
{
	public SuspensionValue AWD = new SuspensionValue("AWD", ValueType.Int, 0f, 0);

	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue PerchWidth = new SuspensionValue("Perch width", ValueType.Float, 0f, 0);

	public SuspensionValue PerchHeight = new SuspensionValue("Perch height", ValueType.Float, 0f, 0);
}
