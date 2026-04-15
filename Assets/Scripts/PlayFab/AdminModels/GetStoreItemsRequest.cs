using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetStoreItemsRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string StoreId;
	}
}
