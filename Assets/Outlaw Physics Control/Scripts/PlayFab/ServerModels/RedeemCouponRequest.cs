using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RedeemCouponRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterId;

		public string CouponCode;

		public string PlayFabId;
	}
}
