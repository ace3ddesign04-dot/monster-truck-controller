using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetLeaderboardAroundPlayerRequest : PlayFabRequestCommon
	{
		public int? MaxResultsCount;

		public string PlayFabId;

		public PlayerProfileViewConstraints ProfileConstraints;

		public string StatisticName;

		public int? Version;
	}
}
