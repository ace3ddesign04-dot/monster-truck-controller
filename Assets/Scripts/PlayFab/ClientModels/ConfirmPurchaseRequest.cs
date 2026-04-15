using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ConfirmPurchaseRequest : PlayFabRequestCommon
	{
		public string OrderId;
	}
}
