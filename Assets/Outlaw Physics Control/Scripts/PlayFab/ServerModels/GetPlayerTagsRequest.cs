using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerTagsRequest : PlayFabRequestCommon
	{
		public string Namespace;

		public string PlayFabId;
	}
}
