using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class OpenTradeResponse : PlayFabResultCommon
	{
		public TradeInfo Trade;
	}
}
