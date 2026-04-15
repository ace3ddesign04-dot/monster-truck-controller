using System.Collections.Generic;

public class StashContent
{
	public int CashAmount;

	public int GoldAmount;

	public BoostCard BoostCard;

	public CratePartType MissingPart;

	public static Dictionary<CratePartType, string> CratePartTypeList()
	{
		Dictionary<CratePartType, string> dictionary = new Dictionary<CratePartType, string>();
		dictionary.Add(CratePartType.FrontAxle, "Front Axle");
		dictionary.Add(CratePartType.RearAxle, "Rear Axle");
		dictionary.Add(CratePartType.Engine, "Engine");
		dictionary.Add(CratePartType.Wheels, "Wheels");
		dictionary.Add(CratePartType.Seats, "Seats");
		dictionary.Add(CratePartType.Drivetrain, "Drivetrain");
		dictionary.Add(CratePartType.Tires, "Tires");
		dictionary.Add(CratePartType.SteeringRack, "Steering Rack");
		dictionary.Add(CratePartType.WindowGlass, "Window Glass");
		dictionary.Add(CratePartType.Transmission, "Transmission");
		return dictionary;
	}
}
