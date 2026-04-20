using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetLeaderboardAroundCharacterRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string CharacterType;

		public int? MaxResultsCount;

		public string StatisticName;
	}
}
