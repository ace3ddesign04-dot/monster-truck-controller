using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdatePlayerStatisticsRequest : PlayFabRequestCommon
	{
		public List<StatisticUpdate> Statistics;
	}
}
