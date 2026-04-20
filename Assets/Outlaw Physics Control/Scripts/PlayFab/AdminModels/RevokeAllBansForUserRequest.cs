using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RevokeAllBansForUserRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}
