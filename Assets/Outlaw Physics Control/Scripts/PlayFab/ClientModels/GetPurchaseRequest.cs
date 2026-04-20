using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPurchaseRequest : PlayFabRequestCommon
	{
		public string OrderId;
	}
}
