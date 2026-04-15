using System;

[Serializable]
public class SolidAxleFrontSuspensionControls : SuspensionControls
{
	public SuspensionValue FramesWidth = new SuspensionValue("Frames width", ValueType.Float, 0f, 0);

	public SuspensionValue FrontFrameOffset = new SuspensionValue("Front frame offset", ValueType.Float, 0f, 0);

	public SuspensionValue RearFrameOffset = new SuspensionValue("Rear frame offset", ValueType.Float, 0f, 0);

	public SuspensionValue PerchWidth = new SuspensionValue("Perch width", ValueType.Float, 0f, 0);

	public SuspensionValue PerchHeight = new SuspensionValue("Perch height", ValueType.Float, 0f, 0);

	public SuspensionValue AxisWidth = new SuspensionValue("Axis width", ValueType.Float, 0f, 0);

	public SuspensionValue LeafSpringMountHeight = new SuspensionValue("Leaf spring mount height", ValueType.Float, 0f, 0);

	public SuspensionValue SpringBracketsUpperMount = new SuspensionValue("Spring brackets upper mount", ValueType.Int, 0f, 0);
}
