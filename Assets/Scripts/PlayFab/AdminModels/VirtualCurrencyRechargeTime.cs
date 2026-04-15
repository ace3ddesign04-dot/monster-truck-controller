using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class VirtualCurrencyRechargeTime
	{
		public int RechargeMax;

		public DateTime RechargeTime;

		public int SecondsToRecharge;
	}
}
