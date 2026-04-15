using System;

[Serializable]
public class StatsData
{
	public int Money;

	public int Gold;

	public int XP;

	public bool IsMember;

	public int SelectedTruckID;

	public int DynoRuns;

	public string Dump()
	{
		return Money.ToString() + Gold.ToString() + XP.ToString() + IsMember.ToString();
	}
}
