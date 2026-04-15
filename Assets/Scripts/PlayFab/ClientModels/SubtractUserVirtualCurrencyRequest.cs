using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class SubtractUserVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string VirtualCurrency;
	}
}
