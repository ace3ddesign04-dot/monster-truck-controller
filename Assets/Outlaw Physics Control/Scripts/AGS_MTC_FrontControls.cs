using System;

[Serializable]
public class AGS_MTC_FrontControls : AGS_MTC_SuspensionControls
{
	public AGS_MTC_SuspensionValue AxisWidth = new AGS_MTC_SuspensionValue("Axis width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue ShocksOffset = new AGS_MTC_SuspensionValue("Shocks offset", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue ShocksHeight = new AGS_MTC_SuspensionValue("Shocks height", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue MiddleFrameWidth = new AGS_MTC_SuspensionValue("Middle frame width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue AxleType = new AGS_MTC_SuspensionValue("Axle type", AGS_MTC_ValueType.Int, 0f, 0);

	public AGS_MTC_SuspensionValue BrakeType = new AGS_MTC_SuspensionValue("Brake type", AGS_MTC_ValueType.Int, 0f, 0);

	public AGS_MTC_SuspensionValue ShowArms = new AGS_MTC_SuspensionValue("Show arms", AGS_MTC_ValueType.Int, 0f, 0);
}
