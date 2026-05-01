using AGS_MonsterTruckControl;

public class PowerPart
{
	public AGS_MTC_VehicleType vehicleType;

	public PowerPartType partType;

	public int Stage;

	public int partCost;

	public float IncrementPercantage;

	public string Description;

	public PowerPart(AGS_MTC_VehicleType _vehicleType, PowerPartType _partType, int _stage, int _partCost, float _incrementPercentage, string _description)
	{
		vehicleType = _vehicleType;
		partType = _partType;
		Stage = _stage;
		partCost = _partCost;
		IncrementPercantage = _incrementPercentage;
		Description = _description;
	}
}
