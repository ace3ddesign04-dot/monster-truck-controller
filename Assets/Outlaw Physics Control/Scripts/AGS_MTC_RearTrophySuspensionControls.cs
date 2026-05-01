using System;

[Serializable]
public class AGS_MTC_RearTrophySuspensionControls : AGS_MTC_SuspensionControls {
	public AGS_MTC_SuspensionValue RearSteering = new AGS_MTC_SuspensionValue("Rear steering", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue AxisWidth = new AGS_MTC_SuspensionValue("Axis width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue ShocksOffset = new AGS_MTC_SuspensionValue("Shocks offset", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue ShocksTravel = new AGS_MTC_SuspensionValue("Shocks height", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue TrailingArmsOffset = new AGS_MTC_SuspensionValue("Trailing arms offset", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue TrailingArmsHeight = new AGS_MTC_SuspensionValue("Trailing arms height", AGS_MTC_ValueType.Float, 0f, 0);
}
