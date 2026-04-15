using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCatalogItemsRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;
	}
}
