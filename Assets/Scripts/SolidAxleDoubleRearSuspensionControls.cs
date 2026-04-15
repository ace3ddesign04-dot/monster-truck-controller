using System;

[Serializable]
public class SolidAxleDoubleRearSuspensionControls : SuspensionControls
{
	public SuspensionValue RearSteering = new SuspensionValue("Rear steering", ValueType.Float, 0f, 0);

	public SuspensionValue FramesWidth = new SuspensionValue("Frames width", ValueType.Float, 0f, 0);

	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue LeafSpringMountHeight = new SuspensionValue("Leaf spring mount height", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksOffset = new SuspensionValue("Shocks offset", ValueType.Float, 0f, 0);

	public SuspensionValue ShocksHeight = new SuspensionValue("Shocks height", ValueType.Float, 0f, 0);

	public SuspensionValue SpringBracketsUpperMount = new SuspensionValue("Spring brackets upper mount", ValueType.Int, 0f, 0);
}
