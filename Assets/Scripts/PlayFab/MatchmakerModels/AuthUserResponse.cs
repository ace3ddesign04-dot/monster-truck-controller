using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class AuthUserResponse : PlayFabResultCommon
	{
		public bool Authorized;

		public string PlayFabId;
	}
}
