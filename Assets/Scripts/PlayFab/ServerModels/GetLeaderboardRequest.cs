using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardRequest : PlayFabRequestCommon
	{
		public int MaxResultsCount;

		public PlayerProfileViewConstraints ProfileConstraints;

		public int StartPosition;

		public string StatisticName;

		public int? Version;
	}
}
