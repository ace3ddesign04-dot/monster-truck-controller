using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTradeStatusRequest : PlayFabRequestCommon
	{
		public string OfferingPlayerId;

		public string TradeId;
	}
}
