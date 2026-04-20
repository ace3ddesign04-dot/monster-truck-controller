using CustomVP;

public class SuspensionPart
{
	public VehicleType vehicleType;

	public string partName;

	public string displayedName;

	public string partDescription;

	public int partCost;

	public SuspensionPart(VehicleType _vehicleType, string _partName, string _displayedName, string _partDescription, int _partCost)
	{
		vehicleType = _vehicleType;
		partName = _partName;
		partDescription = _partDescription;
		partCost = _partCost;
		displayedName = _displayedName;
	}
}
