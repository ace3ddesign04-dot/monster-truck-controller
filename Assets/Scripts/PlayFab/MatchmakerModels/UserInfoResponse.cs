using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class UserInfoResponse : PlayFabResultCommon
	{
		public List<ItemInstance> Inventory;

		public bool IsDeveloper;

		public string PlayFabId;

		public string SteamId;

		public string TitleDisplayName;

		public string Username;

		public Dictionary<string, int> VirtualCurrency;

		public Dictionary<string, VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
	}
}
