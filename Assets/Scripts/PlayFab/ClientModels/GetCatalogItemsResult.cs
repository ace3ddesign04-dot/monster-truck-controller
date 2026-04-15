using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetCatalogItemsResult : PlayFabResultCommon
	{
		public List<CatalogItem> Catalog;
	}
}
