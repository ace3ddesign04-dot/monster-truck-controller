using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AwardSteamAchievementResult : PlayFabResultCommon
	{
		public List<AwardSteamAchievementItem> AchievementResults;
	}
}
