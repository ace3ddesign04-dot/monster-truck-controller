using AGS_MonsterTruckControl;

public class AGS_MTC_PowerPart
{
	public AGS_MTC_VehicleType vehicleType;

	public AGS_MTC_PowerPartType partType;

	public int Stage;

	public int partCost;

	public float IncrementPercantage;

	public string Description;

	public AGS_MTC_PowerPart(AGS_MTC_VehicleType _vehicleType, AGS_MTC_PowerPartType _partType, int _stage, int _partCost, float _incrementPercentage, string _description)
	{
		vehicleType = _vehicleType;
		partType = _partType;
		Stage = _stage;
		partCost = _partCost;
		IncrementPercantage = _incrementPercentage;
		Description = _description;
	}
}
