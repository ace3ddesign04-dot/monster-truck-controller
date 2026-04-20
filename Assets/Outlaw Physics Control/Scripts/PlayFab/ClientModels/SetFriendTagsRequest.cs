using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class SetFriendTagsRequest : PlayFabRequestCommon
	{
		public string FriendPlayFabId;

		public List<string> Tags;
	}
}
