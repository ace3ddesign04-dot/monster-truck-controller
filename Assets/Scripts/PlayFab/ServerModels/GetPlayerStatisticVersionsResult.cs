using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerStatisticVersionsResult : PlayFabResultCommon
	{
		public List<PlayerStatisticVersion> StatisticVersions;
	}
}
