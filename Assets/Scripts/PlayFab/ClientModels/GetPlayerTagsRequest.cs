using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerTagsRequest : PlayFabRequestCommon
	{
		public string Namespace;

		public string PlayFabId;
	}
}
