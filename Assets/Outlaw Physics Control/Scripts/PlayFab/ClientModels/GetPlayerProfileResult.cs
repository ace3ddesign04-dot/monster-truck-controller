using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerProfileResult : PlayFabResultCommon
	{
		public PlayerProfileModel PlayerProfile;
	}
}
