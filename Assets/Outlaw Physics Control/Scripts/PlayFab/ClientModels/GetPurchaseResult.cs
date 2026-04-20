using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPurchaseResult : PlayFabResultCommon
	{
		public string OrderId;

		public string PaymentProvider;

		public DateTime PurchaseDate;

		public string TransactionId;

		public string TransactionStatus;
	}
}
