using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GrantItemsToUsersResult : PlayFabResultCommon
	{
		public List<GrantedItemInstance> ItemGrantResults;
	}
}
