using System;

[Serializable]
public class RoutePayment
{
	public AwardLevel AwardLevel;

	public int Cash;

	public int Gold;

	public int XP;

	public int CompletionCash;

	public int CompletionXP;

	public bool Trailblazer;

	public int TrailblazerGoldBonus;

	public string AwardLevelString()
	{
		string empty = string.Empty;
		switch (AwardLevel)
		{
		case AwardLevel.Completion:
			return "Completion";
		case AwardLevel.Copper:
			return "Copper";
		case AwardLevel.Silver:
			return "Silver";
		default:
			return "Gold";
		}
	}
}
