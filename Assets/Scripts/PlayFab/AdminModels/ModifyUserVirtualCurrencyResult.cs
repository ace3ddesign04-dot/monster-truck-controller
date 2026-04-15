using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ModifyUserVirtualCurrencyResult : PlayFabResultCommon
	{
		public int Balance;

		public int BalanceChange;

		public string PlayFabId;

		public string VirtualCurrency;
	}
}
