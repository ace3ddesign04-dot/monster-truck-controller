using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string PlayFabId;

		public string VirtualCurrency;
	}
}
