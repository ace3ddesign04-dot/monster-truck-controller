using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GrantItemsToUsersRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public List<ItemGrant> ItemGrants;
	}
}
