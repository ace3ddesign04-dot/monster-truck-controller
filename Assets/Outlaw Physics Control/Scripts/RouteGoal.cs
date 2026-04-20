using CustomVP;
using System;
using UnityEngine;

[Serializable]
public class RouteGoal
{
	[Header("Awards")]
	public int BaseCashPayment;

	public int BaseGoldPayment;

	public int BaseXPPayment;

	public int TrailblazerGoldBonus;

	public int LevelUpCashIncrement;

	public int LevelUpGoldIncrement;

	public int LevelUpXPPayment;

	[Space(5f)]
	[Header("Constraints")]
	public long RecordTime;

	public Route Route;

	private long GoldPercentage = 20L;

	private long SilverPercentage = 40L;

	private long CopperPercentage = 100L;

	public int WinchPenalty = 10;

	public int FlipPenalty = 20;

	[Space(5f)]
	[Header("Vehicle")]
	public VehicleType VehicleType;

	public RoutePayment GetPayment(int completionTime, int winchCount, int flipCount, float damageCount)
	{
		RoutePayment routePayment = new RoutePayment();
		if (RecordTime > 0)
		{
			routePayment.AwardLevel = GetAwardLevelAchieved(completionTime, winchCount, flipCount, damageCount);
		}
		else if (Route.TrailblazerEligible)
		{
			routePayment.AwardLevel = AwardLevel.Gold;
			routePayment.Trailblazer = true;
			routePayment.TrailblazerGoldBonus = TrailblazerGoldBonus;
		}
		else
		{
			routePayment.AwardLevel = AwardLevel.Completion;
		}
		routePayment.Cash = BaseCashPayment + LevelUpCashIncrement * (int)routePayment.AwardLevel;
		routePayment.Gold = BaseGoldPayment + LevelUpGoldIncrement * (int)routePayment.AwardLevel;
		routePayment.XP = BaseXPPayment + LevelUpXPPayment * (int)routePayment.AwardLevel;
		return routePayment;
	}

	private AwardLevel GetAwardLevelAchieved(int completionTime, int winchCount, int flipCount, float damageCount)
	{
		AwardLevel result = AwardLevel.Completion;
		completionTime += winchCount * WinchPenalty * 100 + flipCount * FlipPenalty * 100;
		for (int num = 4; num >= 1; num--)
		{
			RouteGoalLimit limits = GetLimits((AwardLevel)num);
			if (completionTime <= limits.TimeLimit)
			{
				result = (AwardLevel)num;
				break;
			}
		}
		return result;
	}

	public RouteGoalLimit GetLimits(AwardLevel awardLevel)
	{
		RouteGoalLimit routeGoalLimit = new RouteGoalLimit();
		routeGoalLimit.AwardLevel = awardLevel;
		routeGoalLimit.TimeLimit = RecordTime;
		switch (awardLevel)
		{
		case AwardLevel.Gold:
			routeGoalLimit.TimeLimit += (long)((float)routeGoalLimit.TimeLimit * ((float)GoldPercentage / 100f));
			break;
		case AwardLevel.Silver:
			routeGoalLimit.TimeLimit += (long)((float)routeGoalLimit.TimeLimit * ((float)SilverPercentage / 100f));
			break;
		case AwardLevel.Copper:
			routeGoalLimit.TimeLimit += (long)((float)routeGoalLimit.TimeLimit * ((float)CopperPercentage / 100f));
			break;
		}
		routeGoalLimit.TimeLimit = ((routeGoalLimit.TimeLimit >= 0) ? routeGoalLimit.TimeLimit : 0);
		routeGoalLimit.WinchLimit = Mathf.Max(routeGoalLimit.WinchLimit, 0);
		routeGoalLimit.FlipLimit = Mathf.Max(routeGoalLimit.FlipLimit, 0);
		routeGoalLimit.DamageLimit = Mathf.Max(routeGoalLimit.DamageLimit, 0f);
		return routeGoalLimit;
	}

	public static RouteGoal Default(long record, Route route = null, VehicleType type = VehicleType.ATV)
	{
		int num = 0;
		if (record > 0)
		{
			if (record < 3000)
			{
				num = -1000;
			}
			else if (record < 7500)
			{
				num = -500;
			}
			if (record > 24000)
			{
				num = 1000;
			}
			else if (record > 18000)
			{
				num = 500;
			}
		}
		if (route != null && !route.TrailblazerEligible && record == 0)
		{
			num = -500;
		}
		switch (type)
		{
		case VehicleType.Crawler:
		{
			RouteGoal routeGoal4 = new RouteGoal();
			routeGoal4.RecordTime = record;
			routeGoal4.BaseCashPayment = Mathf.Max(1500 + num, 750);
			routeGoal4.BaseGoldPayment = 1;
			routeGoal4.BaseXPPayment = 5;
			routeGoal4.LevelUpCashIncrement = 150;
			routeGoal4.LevelUpGoldIncrement = 1;
			routeGoal4.LevelUpXPPayment = 1;
			routeGoal4.TrailblazerGoldBonus = 10;
			routeGoal4.Route = route;
			return routeGoal4;
		}
		case VehicleType.Truck:
		{
			RouteGoal routeGoal3 = new RouteGoal();
			routeGoal3.RecordTime = record;
			routeGoal3.BaseCashPayment = Mathf.Max(1200 + num, 750);
			routeGoal3.BaseGoldPayment = 0;
			routeGoal3.BaseXPPayment = 3;
			routeGoal3.LevelUpCashIncrement = 100;
			routeGoal3.LevelUpGoldIncrement = 1;
			routeGoal3.LevelUpXPPayment = 1;
			routeGoal3.TrailblazerGoldBonus = 10;
			routeGoal3.Route = route;
			return routeGoal3;
		}
		case VehicleType.SideBySide:
		{
			RouteGoal routeGoal2 = new RouteGoal();
			routeGoal2.RecordTime = record;
			routeGoal2.BaseCashPayment = Mathf.Max(1100 + num, 500);
			routeGoal2.BaseGoldPayment = 0;
			routeGoal2.BaseXPPayment = 3;
			routeGoal2.LevelUpCashIncrement = 75;
			routeGoal2.LevelUpGoldIncrement = 0;
			routeGoal2.LevelUpXPPayment = 1;
			routeGoal2.TrailblazerGoldBonus = 5;
			routeGoal2.Route = route;
			return routeGoal2;
		}
		default:
		{
			RouteGoal routeGoal = new RouteGoal();
			routeGoal.RecordTime = record;
			routeGoal.BaseCashPayment = Mathf.Max(750 + Mathf.Max(num, -500), 400);
			routeGoal.BaseGoldPayment = 0;
			routeGoal.BaseXPPayment = 3;
			routeGoal.LevelUpCashIncrement = 50;
			routeGoal.LevelUpGoldIncrement = 0;
			routeGoal.LevelUpXPPayment = 1;
			routeGoal.TrailblazerGoldBonus = 5;
			routeGoal.Route = route;
			return routeGoal;
		}
		}
	}
}
