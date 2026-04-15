using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RefundPurchaseRequest : PlayFabRequestCommon
	{
		public string OrderId;

		public string PlayFabId;

		public string Reason;
	}
}
