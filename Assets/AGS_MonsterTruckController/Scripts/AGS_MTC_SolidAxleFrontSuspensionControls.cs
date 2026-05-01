using System;

[Serializable]
public class AGS_MTC_SolidAxleFrontSuspensionControls : AGS_MTC_SuspensionControls
{
	public AGS_MTC_SuspensionValue FramesWidth = new AGS_MTC_SuspensionValue("Frames width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue FrontFrameOffset = new AGS_MTC_SuspensionValue("Front frame offset", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue RearFrameOffset = new AGS_MTC_SuspensionValue("Rear frame offset", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue PerchWidth = new AGS_MTC_SuspensionValue("Perch width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue PerchHeight = new AGS_MTC_SuspensionValue("Perch height", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue AxisWidth = new AGS_MTC_SuspensionValue("Axis width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue LeafSpringMountHeight = new AGS_MTC_SuspensionValue("Leaf spring mount height", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue SpringBracketsUpperMount = new AGS_MTC_SuspensionValue("Spring brackets upper mount", AGS_MTC_ValueType.Int, 0f, 0);
}
