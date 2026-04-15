using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerStatisticsRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public List<string> StatisticNames;

		public List<StatisticNameVersion> StatisticNameVersions;
	}
}
