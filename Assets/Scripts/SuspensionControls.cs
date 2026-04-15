using System;

[Serializable]
public class SuspensionControls
{
	public SuspensionValue Travel = new SuspensionValue("Travel", ValueType.Float, 0.3f, 0);

	public SuspensionValue Stiffness = new SuspensionValue("Stiffness", ValueType.Float, 20000f, 0);

	public SuspensionValue Damping = new SuspensionValue("Damping", ValueType.Float, 1000f, 0);

	public SuspensionValue ShocksGroup = new SuspensionValue("Shocks", ValueType.Int, 0f, 0);

	public SuspensionValue ShocksSize = new SuspensionValue("Shocks size", ValueType.Float, 1f, 0);
}
