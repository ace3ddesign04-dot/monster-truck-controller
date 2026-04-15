using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetFriendsListRequest : PlayFabRequestCommon
	{
		public bool? IncludeFacebookFriends;

		public bool? IncludeSteamFriends;

		public string PlayFabId;

		public PlayerProfileViewConstraints ProfileConstraints;
	}
}
