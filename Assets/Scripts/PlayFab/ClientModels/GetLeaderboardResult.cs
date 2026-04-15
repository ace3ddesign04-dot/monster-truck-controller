using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetLeaderboardResult : PlayFabResultCommon
	{
		public List<PlayerLeaderboardEntry> Leaderboard;

		public DateTime? NextReset;

		public int Version;
	}
}
