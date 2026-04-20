using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetFriendsListResult : PlayFabResultCommon
	{
		public List<FriendInfo> Friends;
	}
}
