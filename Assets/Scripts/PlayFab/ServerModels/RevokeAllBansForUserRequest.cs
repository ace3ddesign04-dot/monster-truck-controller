using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RevokeAllBansForUserRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}
