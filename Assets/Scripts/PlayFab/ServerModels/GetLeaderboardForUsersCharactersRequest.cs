using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardForUsersCharactersRequest : PlayFabRequestCommon
	{
		public int MaxResultsCount;

		public string PlayFabId;

		public string StatisticName;
	}
}
