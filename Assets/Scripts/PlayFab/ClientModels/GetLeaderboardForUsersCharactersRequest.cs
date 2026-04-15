using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetLeaderboardForUsersCharactersRequest : PlayFabRequestCommon
	{
		public int MaxResultsCount;

		public string StatisticName;
	}
}
