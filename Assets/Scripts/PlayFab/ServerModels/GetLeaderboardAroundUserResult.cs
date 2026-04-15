using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardAroundUserResult : PlayFabResultCommon
	{
		public List<PlayerLeaderboardEntry> Leaderboard;

		public DateTime? NextReset;

		public int Version;
	}
}
