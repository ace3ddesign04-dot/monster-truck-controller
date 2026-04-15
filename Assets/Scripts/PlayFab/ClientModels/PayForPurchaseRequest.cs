using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class PayForPurchaseRequest : PlayFabRequestCommon
	{
		public string Currency;

		public string OrderId;

		public string ProviderName;

		public string ProviderTransactionId;
	}
}
