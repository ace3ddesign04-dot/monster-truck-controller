using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerStatisticsRequest : PlayFabRequestCommon
	{
		public List<string> StatisticNames;

		public List<StatisticNameVersion> StatisticNameVersions;
	}
}
