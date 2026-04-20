using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class VirtualCurrencyData
	{
		public string CurrencyCode;

		public string DisplayName;

		public int? InitialDeposit;

		public int? RechargeMax;

		public int? RechargeRate;
	}
}
