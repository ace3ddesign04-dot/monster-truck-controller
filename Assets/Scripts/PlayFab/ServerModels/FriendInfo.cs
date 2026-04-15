using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class FriendInfo
	{
		public string CurrentMatchmakerLobbyId;

		public UserFacebookInfo FacebookInfo;

		public string FriendPlayFabId;

		public UserGameCenterInfo GameCenterInfo;

		public PlayerProfileModel Profile;

		public UserSteamInfo SteamInfo;

		public List<string> Tags;

		public string TitleDisplayName;

		public string Username;
	}
}
