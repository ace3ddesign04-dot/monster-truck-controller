using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerCombinedInfoResult : PlayFabResultCommon
	{
		public GetPlayerCombinedInfoResultPayload InfoResultPayload;

		public string PlayFabId;
	}
}
