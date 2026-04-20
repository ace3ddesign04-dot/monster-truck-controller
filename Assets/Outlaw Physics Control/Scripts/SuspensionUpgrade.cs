public class SuspensionUpgrade
{
	public int Stage;

	public int upgradeCost;

	public string SuspensionName;

	public SuspensionUpgrade(string _suspensionName, int _stage, int _upgradeCost)
	{
		Stage = _stage;
		upgradeCost = _upgradeCost;
		SuspensionName = _suspensionName;
	}
}
