using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerStatisticVersionsRequest : PlayFabRequestCommon
	{
		public string StatisticName;
	}
}
