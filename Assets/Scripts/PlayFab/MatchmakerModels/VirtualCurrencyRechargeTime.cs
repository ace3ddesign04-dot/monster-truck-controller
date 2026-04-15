using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class VirtualCurrencyRechargeTime
	{
		public int RechargeMax;

		public DateTime RechargeTime;

		public int SecondsToRecharge;
	}
}
