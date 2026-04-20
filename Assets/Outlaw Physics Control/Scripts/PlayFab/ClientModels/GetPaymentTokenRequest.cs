using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPaymentTokenRequest : PlayFabRequestCommon
	{
		public string TokenProvider;
	}
}
