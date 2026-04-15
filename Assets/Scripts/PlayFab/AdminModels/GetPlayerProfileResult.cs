using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayerProfileResult : PlayFabResultCommon
	{
		public PlayerProfileModel PlayerProfile;
	}
}
