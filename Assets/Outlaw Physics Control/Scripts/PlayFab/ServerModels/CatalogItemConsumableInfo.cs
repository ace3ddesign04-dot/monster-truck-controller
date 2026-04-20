using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CatalogItemConsumableInfo
	{
		public uint? UsageCount;

		public uint? UsagePeriod;

		public string UsagePeriodGroup;
	}
}
