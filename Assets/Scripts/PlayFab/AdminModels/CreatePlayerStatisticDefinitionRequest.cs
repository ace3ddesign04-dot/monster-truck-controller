using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CreatePlayerStatisticDefinitionRequest : PlayFabRequestCommon
	{
		public StatisticAggregationMethod? AggregationMethod;

		public string StatisticName;

		public StatisticResetIntervalOption? VersionChangeInterval;
	}
}
