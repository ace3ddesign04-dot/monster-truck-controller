using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddSharedGroupMembersRequest : PlayFabRequestCommon
	{
		public List<string> PlayFabIds;

		public string SharedGroupId;
	}
}
