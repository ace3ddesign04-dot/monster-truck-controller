using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SubtractCharacterVirtualCurrencyRequest : PlayFabRequestCommon
	{
		public int Amount;

		public string CharacterId;

		public string PlayFabId;

		public string VirtualCurrency;
	}
}
