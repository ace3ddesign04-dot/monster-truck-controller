using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AuthenticateSessionTicketResult : PlayFabResultCommon
	{
		public UserAccountInfo UserInfo;
	}
}
