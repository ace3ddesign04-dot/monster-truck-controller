using System;

[Serializable]
public class YamahaATVRearSuspensionControls : SuspensionControls
{
	public SuspensionValue RearAxleOffset = new SuspensionValue("Rear axle offset", ValueType.Float, 0f, 0);

	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue ShockUpsHeight = new SuspensionValue("Shock ups height", ValueType.Float, 0f, 0);

	public SuspensionValue ShockUpsOffset = new SuspensionValue("Shock ups offset", ValueType.Float, 0f, 0);
}
