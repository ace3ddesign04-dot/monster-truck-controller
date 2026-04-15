using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayerIdFromAuthTokenRequest : PlayFabRequestCommon
	{
		public string Token;

		public AuthTokenType TokenType;
	}
}
