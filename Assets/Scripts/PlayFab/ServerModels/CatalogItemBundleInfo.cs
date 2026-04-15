using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CatalogItemBundleInfo
	{
		public List<string> BundledItems;

		public List<string> BundledResultTables;

		public Dictionary<string, uint> BundledVirtualCurrencies;
	}
}
