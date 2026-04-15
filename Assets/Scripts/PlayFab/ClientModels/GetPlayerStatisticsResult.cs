using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerStatisticsResult : PlayFabResultCommon
	{
		public List<StatisticValue> Statistics;
	}
}
