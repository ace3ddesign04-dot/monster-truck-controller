using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateCharacterStatisticsRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public Dictionary<string, int> CharacterStatistics;
	}
}
