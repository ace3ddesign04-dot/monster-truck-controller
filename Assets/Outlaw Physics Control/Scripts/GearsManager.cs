public static class GearsManager
{
	public static float[] DefaultGears => new float[5]
	{
		3f,
		1.4f,
		0.9f,
		0.7f,
		0.55f
	};

	public static float[] GearsMinLimits => new float[5]
	{
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f
	};

	public static float[] GearsMaxLimits => new float[5]
	{
		4f,
		4f,
		4f,
		4f,
		4f
	};

	public static float DefaultLowGear => 2f;

	public static float LowGearMinLimit => 0.1f;

	public static float LowGearMaxLimit => 4f;

	public static float TopGear => 9f;

	public static float GetDefaultGear(int GearID)
	{
		if (GearID == -1)
		{
			return DefaultLowGear;
		}
		if (GearID < DefaultGears.Length)
		{
			return DefaultGears[GearID];
		}
		return DefaultGears[DefaultGears.Length - 1];
	}

	public static float GetMinLimit(int GearID)
	{
		if (GearID == -1)
		{
			return LowGearMinLimit;
		}
		if (GearID < GearsMinLimits.Length)
		{
			return GearsMinLimits[GearID];
		}
		return GearsMinLimits[GearsMinLimits.Length - 1];
	}

	public static float GetMaxLimit(int GearID)
	{
		if (GearID == -1)
		{
			return LowGearMaxLimit;
		}
		if (GearID < GearsMaxLimits.Length)
		{
			return GearsMaxLimits[GearID];
		}
		return GearsMaxLimits[GearsMaxLimits.Length - 1];
	}
}
