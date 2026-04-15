using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AddFriendRequest : PlayFabRequestCommon
	{
		public string FriendEmail;

		public string FriendPlayFabId;

		public string FriendTitleDisplayName;

		public string FriendUsername;

		public string PlayFabId;
	}
}
