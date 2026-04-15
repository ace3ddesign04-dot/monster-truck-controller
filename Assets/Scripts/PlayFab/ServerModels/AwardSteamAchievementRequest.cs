using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AwardSteamAchievementRequest : PlayFabRequestCommon
	{
		public List<AwardSteamAchievementItem> Achievements;
	}
}
