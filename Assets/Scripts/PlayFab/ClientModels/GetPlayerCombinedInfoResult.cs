using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerCombinedInfoResult : PlayFabResultCommon
	{
		public GetPlayerCombinedInfoResultPayload InfoResultPayload;

		public string PlayFabId;
	}
}
