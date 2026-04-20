using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RedeemCouponResult : PlayFabResultCommon
	{
		public List<ItemInstance> GrantedItems;
	}
}
