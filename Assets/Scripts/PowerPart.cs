using CustomVP;

public class PowerPart
{
	public VehicleType vehicleType;

	public PowerPartType partType;

	public int Stage;

	public int partCost;

	public float IncrementPercantage;

	public string Description;

	public PowerPart(VehicleType _vehicleType, PowerPartType _partType, int _stage, int _partCost, float _incrementPercentage, string _description)
	{
		vehicleType = _vehicleType;
		partType = _partType;
		Stage = _stage;
		partCost = _partCost;
		IncrementPercantage = _incrementPercentage;
		Description = _description;
	}
}
