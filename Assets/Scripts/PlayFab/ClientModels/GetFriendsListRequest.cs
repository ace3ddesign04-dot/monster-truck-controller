using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetFriendsListRequest : PlayFabRequestCommon
	{
		public bool? IncludeFacebookFriends;

		public bool? IncludeSteamFriends;

		public PlayerProfileViewConstraints ProfileConstraints;
	}
}
