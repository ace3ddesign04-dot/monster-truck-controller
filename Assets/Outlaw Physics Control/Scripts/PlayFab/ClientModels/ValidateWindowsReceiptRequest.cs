using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ValidateWindowsReceiptRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CurrencyCode;

		public uint PurchasePrice;

		public string Receipt;
	}
}
