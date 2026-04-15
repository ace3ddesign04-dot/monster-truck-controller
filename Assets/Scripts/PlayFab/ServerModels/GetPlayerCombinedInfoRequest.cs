using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerCombinedInfoRequest : PlayFabRequestCommon
	{
		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string PlayFabId;
	}
}
