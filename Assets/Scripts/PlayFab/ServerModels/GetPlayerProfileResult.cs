using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerProfileResult : PlayFabResultCommon
	{
		public PlayerProfileModel PlayerProfile;
	}
}
