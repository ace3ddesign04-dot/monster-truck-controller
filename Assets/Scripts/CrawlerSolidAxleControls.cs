using System;

[Serializable]
public class CrawlerSolidAxleControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue AxleType = new SuspensionValue("Axle type", ValueType.Int, 0f, 0);

	public SuspensionValue BrakeType = new SuspensionValue("Brake type", ValueType.Int, 0f, 0);

	public SuspensionValue RearSteering = new SuspensionValue("Rear steering", ValueType.Float, 0f, 0);
}
