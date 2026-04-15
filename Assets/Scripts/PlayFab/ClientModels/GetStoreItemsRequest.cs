using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetStoreItemsRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string StoreId;
	}
}
