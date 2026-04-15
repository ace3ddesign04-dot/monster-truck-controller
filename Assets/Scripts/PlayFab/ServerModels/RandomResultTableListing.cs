using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RandomResultTableListing
	{
		public string CatalogVersion;

		public List<ResultTableNode> Nodes;

		public string TableId;
	}
}
