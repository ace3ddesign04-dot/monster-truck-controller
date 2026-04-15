using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetUserInventoryResult : PlayFabResultCommon
	{
		public List<ItemInstance> Inventory;

		public string PlayFabId;

		public Dictionary<string, int> VirtualCurrency;

		public Dictionary<string, VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
	}
}
