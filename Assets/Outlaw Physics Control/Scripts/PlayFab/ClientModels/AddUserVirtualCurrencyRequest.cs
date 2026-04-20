using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string VirtualCurrency;
	}
}
