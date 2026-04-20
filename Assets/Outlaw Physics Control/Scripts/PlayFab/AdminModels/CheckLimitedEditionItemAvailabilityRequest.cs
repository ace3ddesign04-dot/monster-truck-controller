using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CheckLimitedEditionItemAvailabilityRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string ItemId;
	}
}
