using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserBansRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}
