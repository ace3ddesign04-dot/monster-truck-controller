using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ValidateGooglePlayPurchaseRequest : PlayFabRequestCommon
	{
		public string CurrencyCode;

		public uint? PurchasePrice;

		public string ReceiptJson;

		public string Signature;
	}
}
