using System;

[Serializable]
public class SuspensionValue
{
	public string ValueName;

	public ValueType valueType;

	public float FloatValue;

	public int IntValue;

	public SuspensionValue()
	{
	}

	public SuspensionValue(string name, ValueType type, float floatValue, int intValue)
	{
		ValueName = name;
		valueType = type;
		FloatValue = floatValue;
		IntValue = intValue;
	}

	public SuspensionValue DeepCopy()
	{
		SuspensionValue suspensionValue = new SuspensionValue();
		suspensionValue.FloatValue = FloatValue;
		suspensionValue.IntValue = IntValue;
		suspensionValue.ValueName = ValueName;
		suspensionValue.valueType = valueType;
		return suspensionValue;
	}

	public void ReceiveValues(SuspensionValue receiveFrom)
	{
		IntValue = receiveFrom.IntValue;
		FloatValue = receiveFrom.FloatValue;
	}
}
