using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class VirtualCurrencyRechargeTime
	{
		public int RechargeMax;

		public DateTime RechargeTime;

		public int SecondsToRecharge;
	}
}
