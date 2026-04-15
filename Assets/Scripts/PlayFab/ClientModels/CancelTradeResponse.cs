using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CancelTradeResponse : PlayFabResultCommon
	{
		public TradeInfo Trade;
	}
}
