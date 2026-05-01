using System;

[Serializable]
public class AGS_MTC_IBeamFrontSuspensionControls : AGS_MTC_SuspensionControls {
	public AGS_MTC_SuspensionValue AxisWidth = new AGS_MTC_SuspensionValue("Axis width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue PerchHeight = new AGS_MTC_SuspensionValue("Perch height", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue PerchWidth = new AGS_MTC_SuspensionValue("Perch width", AGS_MTC_ValueType.Float, 0f, 0);

	public AGS_MTC_SuspensionValue TrailingArmMountsWidth = new AGS_MTC_SuspensionValue("Trailing arm mount width", AGS_MTC_ValueType.Float, 0f, 0);
}
