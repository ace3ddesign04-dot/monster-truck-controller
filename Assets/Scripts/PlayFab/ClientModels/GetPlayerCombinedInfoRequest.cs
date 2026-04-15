using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerCombinedInfoRequest : PlayFabRequestCommon
	{
		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string PlayFabId;
	}
}
