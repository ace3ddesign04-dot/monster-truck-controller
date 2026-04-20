using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
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
