using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantedItemInstance
	{
		public string Annotation;

		public List<string> BundleContents;

		public string BundleParent;

		public string CatalogVersion;

		public string CharacterId;

		public Dictionary<string, string> CustomData;

		public string DisplayName;

		public DateTime? Expiration;

		public string ItemClass;

		public string ItemId;

		public string ItemInstanceId;

		public string PlayFabId;

		public DateTime? PurchaseDate;

		public int? RemainingUses;

		public bool Result;

		public string UnitCurrency;

		public uint UnitPrice;

		public int? UsesIncrementedBy;
	}
}
