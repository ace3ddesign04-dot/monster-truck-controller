using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class EvaluateRandomResultTableRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string TableId;
	}
}
