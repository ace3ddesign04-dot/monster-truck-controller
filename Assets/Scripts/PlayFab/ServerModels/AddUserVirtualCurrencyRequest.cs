using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string PlayFabId;

		public string VirtualCurrency;
	}
}
