using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SubtractUserVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string PlayFabId;

		public string VirtualCurrency;
	}
}
