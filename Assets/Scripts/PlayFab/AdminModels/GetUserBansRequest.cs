using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetUserBansRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}
