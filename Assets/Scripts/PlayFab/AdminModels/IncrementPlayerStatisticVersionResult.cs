using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class IncrementPlayerStatisticVersionResult : PlayFabResultCommon
	{
		public PlayerStatisticVersion StatisticVersion;
	}
}
