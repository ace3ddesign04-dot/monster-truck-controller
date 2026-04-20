using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCatalogItemsResult : PlayFabResultCommon
	{
		public List<CatalogItem> Catalog;
	}
}
