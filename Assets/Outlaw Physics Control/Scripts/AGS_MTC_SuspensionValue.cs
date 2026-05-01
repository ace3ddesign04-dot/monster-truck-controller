using System;

[Serializable]
public class AGS_MTC_SuspensionValue
{
	public string ValueName;

	public AGS_MTC_ValueType valueType;

	public float FloatValue;

	public int IntValue;

	public AGS_MTC_SuspensionValue()
	{
	}

	public AGS_MTC_SuspensionValue(string name, AGS_MTC_ValueType type, float floatValue, int intValue)
	{
		ValueName = name;
		valueType = type;
		FloatValue = floatValue;
		IntValue = intValue;
	}

	public AGS_MTC_SuspensionValue DeepCopy()
	{
        AGS_MTC_SuspensionValue suspensionValue = new AGS_MTC_SuspensionValue();
		suspensionValue.FloatValue = FloatValue;
		suspensionValue.IntValue = IntValue;
		suspensionValue.ValueName = ValueName;
		suspensionValue.valueType = valueType;
		return suspensionValue;
	}

	public void ReceiveValues(AGS_MTC_SuspensionValue receiveFrom)
	{
		IntValue = receiveFrom.IntValue;
		FloatValue = receiveFrom.FloatValue;
	}
}
