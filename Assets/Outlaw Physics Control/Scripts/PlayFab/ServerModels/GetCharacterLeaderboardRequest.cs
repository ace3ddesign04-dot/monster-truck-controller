using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterLeaderboardRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string CharacterType;

		public int MaxResultsCount;

		public int StartPosition;

		public string StatisticName;
	}
}
