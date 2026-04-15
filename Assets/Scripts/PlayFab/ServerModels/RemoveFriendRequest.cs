using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RemoveFriendRequest : PlayFabRequestCommon
	{
		public string FriendPlayFabId;

		public string PlayFabId;
	}
}
