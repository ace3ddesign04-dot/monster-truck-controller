using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RemoveSharedGroupMembersRequest : PlayFabRequestCommon
	{
		public List<string> PlayFabIds;

		public string SharedGroupId;
	}
}
