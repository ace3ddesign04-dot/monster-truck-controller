using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerTradesResponse : PlayFabResultCommon
	{
		public List<TradeInfo> AcceptedTrades;

		public List<TradeInfo> OpenedTrades;
	}
}
