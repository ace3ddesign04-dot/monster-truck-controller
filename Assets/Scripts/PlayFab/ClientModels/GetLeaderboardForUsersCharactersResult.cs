using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetLeaderboardForUsersCharactersResult : PlayFabResultCommon
	{
		public List<CharacterLeaderboardEntry> Leaderboard;
	}
}
