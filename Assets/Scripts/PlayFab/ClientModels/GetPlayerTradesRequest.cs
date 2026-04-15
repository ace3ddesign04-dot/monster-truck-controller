using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerTradesRequest : PlayFabRequestCommon
	{
		public TradeStatus? StatusFilter;
	}
}
