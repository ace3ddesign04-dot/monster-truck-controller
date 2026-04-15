using System;

[Serializable]
public class RearTrophySuspensionControls : SuspensionControls
{
	public SuspensionValue RearSteering = new SuspensionValue("Rear steering", ValueType.Float, 0f, 0);

	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksOffset = new SuspensionValue("Shocks offset", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksTravel = new SuspensionValue("Shocks height", ValueType.Float, 0f, 0);

	public SuspensionValue TrailingArmsOffset = new SuspensionValue("Trailing arms offset", ValueType.Float, 0f, 0);

	public SuspensionValue TrailingArmsHeight = new SuspensionValue("Trailing arms height", ValueType.Float, 0f, 0);
}
