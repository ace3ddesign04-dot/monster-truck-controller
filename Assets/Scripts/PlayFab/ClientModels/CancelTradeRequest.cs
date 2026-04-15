using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CancelTradeRequest : PlayFabRequestCommon
	{
		public string TradeId;
	}
}
