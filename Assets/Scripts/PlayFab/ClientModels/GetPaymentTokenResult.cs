using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPaymentTokenResult : PlayFabResultCommon
	{
		public string OrderId;

		public string ProviderToken;
	}
}
