using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ValidateIOSReceiptRequest : PlayFabRequestCommon
	{
		public string CurrencyCode;

		public int PurchasePrice;

		public string ReceiptData;
	}
}
