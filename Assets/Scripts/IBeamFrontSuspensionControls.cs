using System;

[Serializable]
public class IBeamFrontSuspensionControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue PerchHeight = new SuspensionValue("Perch height", ValueType.Float, 0f, 0);

	public SuspensionValue PerchWidth = new SuspensionValue("Perch width", ValueType.Float, 0f, 0);

	public SuspensionValue TrailingArmMountsWidth = new SuspensionValue("Trailing arm mount width", ValueType.Float, 0f, 0);
}
