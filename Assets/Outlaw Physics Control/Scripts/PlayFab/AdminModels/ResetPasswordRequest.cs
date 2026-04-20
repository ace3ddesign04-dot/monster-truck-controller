using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ResetPasswordRequest : PlayFabRequestCommon
	{
		public string Password;

		public string Token;
	}
}
