using System;

[Serializable]
public class YamahaATVFrontSuspensionControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue PerchWidth = new SuspensionValue("Perch width", ValueType.Float, 0f, 0);
}
