using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerStatisticsResult : PlayFabResultCommon
	{
		public string PlayFabId;

		public List<StatisticValue> Statistics;
	}
}
