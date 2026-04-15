using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class PurchaseItemResult : PlayFabResultCommon
	{
		public List<ItemInstance> Items;
	}
}
