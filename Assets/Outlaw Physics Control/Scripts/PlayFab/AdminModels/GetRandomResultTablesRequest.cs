using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetRandomResultTablesRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;
	}
}
