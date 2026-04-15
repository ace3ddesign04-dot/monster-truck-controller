using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UpdatePlayerStatisticsRequest : PlayFabRequestCommon
	{
		public bool? ForceUpdate;

		public string PlayFabId;

		public List<StatisticUpdate> Statistics;
	}
}
