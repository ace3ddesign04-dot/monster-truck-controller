using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserAccountInfoRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}
