using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SendAccountRecoveryEmailRequest : PlayFabRequestCommon
	{
		public string Email;
	}
}
