using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetLeaderboardAroundCharacterRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string CharacterType;

		public int MaxResultsCount;

		public string PlayFabId;

		public string StatisticName;
	}
}
