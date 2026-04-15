using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardAroundCharacterResult : PlayFabResultCommon
	{
		public List<CharacterLeaderboardEntry> Leaderboard;
	}
}
