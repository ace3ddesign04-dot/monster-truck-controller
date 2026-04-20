using CustomVP;

public class BodyPart
{
	public VehicleType vehicleType;

	public PartType partType;

	public string partName;

	public int partCost;

	public BodyPart(VehicleType _vehicleType, PartType _partType, string _partName, int _partCost)
	{
		vehicleType = _vehicleType;
		partType = _partType;
		partName = _partName;
		partCost = _partCost;
	}
}
