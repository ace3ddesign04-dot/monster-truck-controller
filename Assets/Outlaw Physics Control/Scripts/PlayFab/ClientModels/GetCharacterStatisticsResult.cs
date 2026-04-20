using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetCharacterStatisticsResult : PlayFabResultCommon
	{
		public Dictionary<string, int> CharacterStatistics;
	}
}
