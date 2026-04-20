using UnityEngine;

public class BoostCard
{
	public BoostCardType Type;

	public int Duration;

	public int ExtensionCost;

	public float MultiplyAmount;

	public static BoostCard GetCard()
	{
		BoostCard boostCard = new BoostCard();
		boostCard.MultiplyAmount = Random.Range(2, 4);
		boostCard.Duration = Random.Range(6, 12) * 10;
		boostCard.Type = (BoostCardType)Random.Range(0, 7);
		return boostCard;
	}
}
