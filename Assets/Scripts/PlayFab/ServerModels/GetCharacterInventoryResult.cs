using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterInventoryResult : PlayFabResultCommon
	{
		public string CharacterId;

		public List<ItemInstance> Inventory;

		public string PlayFabId;

		public Dictionary<string, int> VirtualCurrency;

		public Dictionary<string, VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
	}
}
