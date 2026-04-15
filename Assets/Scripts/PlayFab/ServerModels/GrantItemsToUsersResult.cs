using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantItemsToUsersResult : PlayFabResultCommon
	{
		public List<GrantedItemInstance> ItemGrantResults;
	}
}
