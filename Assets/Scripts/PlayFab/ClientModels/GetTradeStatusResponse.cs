using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTradeStatusResponse : PlayFabResultCommon
	{
		public TradeInfo Trade;
	}
}
