using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantItemsToUserResult : PlayFabResultCommon
	{
		public List<GrantedItemInstance> ItemGrantResults;
	}
}
