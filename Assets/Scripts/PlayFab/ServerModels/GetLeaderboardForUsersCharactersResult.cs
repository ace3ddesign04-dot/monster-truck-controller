using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardForUsersCharactersResult : PlayFabResultCommon
	{
		public List<CharacterLeaderboardEntry> Leaderboard;
	}
}
