using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RestoreIOSPurchasesRequest : PlayFabRequestCommon
	{
		public string ReceiptData;
	}
}
