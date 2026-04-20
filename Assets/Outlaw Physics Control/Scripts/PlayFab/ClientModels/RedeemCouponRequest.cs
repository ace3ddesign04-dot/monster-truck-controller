using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RedeemCouponRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterId;

		public string CouponCode;
	}
}
