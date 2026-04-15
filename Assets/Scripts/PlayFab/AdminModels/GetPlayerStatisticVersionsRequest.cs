using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayerStatisticVersionsRequest : PlayFabRequestCommon
	{
		public string StatisticName;
	}
}
