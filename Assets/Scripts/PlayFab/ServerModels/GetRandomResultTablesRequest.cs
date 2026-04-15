using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetRandomResultTablesRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public List<string> TableIDs;
	}
}
