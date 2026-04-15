using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantItemsToUsersRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public List<ItemGrant> ItemGrants;
	}
}
