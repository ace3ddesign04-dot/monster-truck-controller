using System;

[Serializable]
public class AGS_MTC_SuspensionControls
{
	public AGS_MTC_SuspensionValue Travel = new AGS_MTC_SuspensionValue("Travel", AGS_MTC_ValueType.Float, 0.3f, 0);

	public AGS_MTC_SuspensionValue Stiffness = new AGS_MTC_SuspensionValue("Stiffness", AGS_MTC_ValueType.Float, 20000f, 0);

	public AGS_MTC_SuspensionValue Damping = new AGS_MTC_SuspensionValue("Damping", AGS_MTC_ValueType.Float, 1000f, 0);

	public AGS_MTC_SuspensionValue ShocksGroup = new AGS_MTC_SuspensionValue("Shocks", AGS_MTC_ValueType.Int, 0f, 0);

	public AGS_MTC_SuspensionValue ShocksSize = new AGS_MTC_SuspensionValue("Shocks size", AGS_MTC_ValueType.Float, 1f, 0);
}
