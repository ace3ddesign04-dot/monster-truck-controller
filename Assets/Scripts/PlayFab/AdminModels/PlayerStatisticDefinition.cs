using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PlayerStatisticDefinition
	{
		public StatisticAggregationMethod? AggregationMethod;

		public uint CurrentVersion;

		public string StatisticName;

		public StatisticResetIntervalOption? VersionChangeInterval;
	}
}
