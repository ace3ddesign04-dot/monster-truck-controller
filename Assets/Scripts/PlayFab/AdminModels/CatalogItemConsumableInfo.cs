using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CatalogItemConsumableInfo
	{
		public uint? UsageCount;

		public uint? UsagePeriod;

		public string UsagePeriodGroup;
	}
}
