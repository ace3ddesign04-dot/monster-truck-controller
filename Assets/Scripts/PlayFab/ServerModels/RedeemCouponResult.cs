using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RedeemCouponResult : PlayFabResultCommon
	{
		public List<ItemInstance> GrantedItems;
	}
}
