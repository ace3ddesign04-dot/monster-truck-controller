using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateRandomResultTablesRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public List<RandomResultTable> Tables;
	}
}
