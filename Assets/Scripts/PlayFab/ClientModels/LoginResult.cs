using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LoginResult : PlayFabResultCommon
	{
		public GetPlayerCombinedInfoResultPayload InfoResultPayload;

		public DateTime? LastLoginTime;

		public bool NewlyCreated;

		public string PlayFabId;

		public string SessionTicket;

		public UserSettings SettingsForUser;
	}
}
