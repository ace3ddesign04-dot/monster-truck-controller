public class VehicleStatus
{
	public float SteeringAngle;

	public float Dirtiness;

	public float Wetness;

	public float WheelsRPM;

	public VehicleStatus(float steeringAngle, float dirtiness, float wetness, float wheelsRPM)
	{
		SteeringAngle = steeringAngle;
		Dirtiness = dirtiness;
		Wetness = wetness;
		WheelsRPM = wheelsRPM;
	}

	public string Serialize()
	{
		return ConvertFromFloat(SteeringAngle).ToString() + "|" + ConvertFromFloat(Dirtiness).ToString() + "|" + ConvertFromFloat(Wetness).ToString() + "|" + ConvertFromFloat(WheelsRPM).ToString();
	}

	public static VehicleStatus DeSerialize(string data)
	{
		VehicleStatus result = new VehicleStatus(0f, 0f, 0f, 0f);
		string[] array = data.Split('|');
		if (array.Length == 4)
		{
			result = new VehicleStatus(ConvertToFloat(int.Parse(array[0])), ConvertToFloat(int.Parse(array[1])), ConvertToFloat(int.Parse(array[2])), ConvertToFloat(int.Parse(array[3])));
		}
		return result;
	}

	private static int ConvertFromFloat(float value)
	{
		return (int)(value * 100f);
	}

	private static float ConvertToFloat(int value)
	{
		return (float)value / 100f;
	}
}
