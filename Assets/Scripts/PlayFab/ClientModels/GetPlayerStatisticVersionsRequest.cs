using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerStatisticVersionsRequest : PlayFabRequestCommon
	{
		public string StatisticName;
	}
}
