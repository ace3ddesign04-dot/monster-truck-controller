using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetFriendLeaderboardRequest : PlayFabRequestCommon
	{
		public bool? IncludeFacebookFriends;

		public bool? IncludeSteamFriends;

		public int MaxResultsCount;

		public string PlayFabId;

		public PlayerProfileViewConstraints ProfileConstraints;

		public int StartPosition;

		public string StatisticName;

		public int? Version;
	}
}
