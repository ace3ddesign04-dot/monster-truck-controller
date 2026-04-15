using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeleteStoreRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string StoreId;
	}
}
