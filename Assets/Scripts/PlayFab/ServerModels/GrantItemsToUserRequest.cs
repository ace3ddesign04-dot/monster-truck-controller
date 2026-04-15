using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantItemsToUserRequest : PlayFabRequestCommon
	{
		public string Annotation;

		public string CatalogVersion;

		public List<string> ItemIds;

		public string PlayFabId;
	}
}
