using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CatalogItemBundleInfo
	{
		public List<string> BundledItems;

		public List<string> BundledResultTables;

		public Dictionary<string, uint> BundledVirtualCurrencies;
	}
}
