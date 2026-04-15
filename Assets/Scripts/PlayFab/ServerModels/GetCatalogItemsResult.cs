using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCatalogItemsResult : PlayFabResultCommon
	{
		public List<CatalogItem> Catalog;
	}
}
