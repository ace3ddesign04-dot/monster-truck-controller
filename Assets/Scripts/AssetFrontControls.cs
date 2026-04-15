using System;

[Serializable]
public class AssetFrontControls : SuspensionControls
{
	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksOffset = new SuspensionValue("Shocks offset", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksHeight = new SuspensionValue("Shocks height", ValueType.Float, 0f, 0);

	public SuspensionValue MiddleFrameWidth = new SuspensionValue("Middle frame width", ValueType.Float, 0f, 0);

	public SuspensionValue AxleType = new SuspensionValue("Axle type", ValueType.Int, 0f, 0);

	public SuspensionValue BrakeType = new SuspensionValue("Brake type", ValueType.Int, 0f, 0);

	public SuspensionValue ShowArms = new SuspensionValue("Show arms", ValueType.Int, 0f, 0);
}
