using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCatalogItemsRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;
	}
}
