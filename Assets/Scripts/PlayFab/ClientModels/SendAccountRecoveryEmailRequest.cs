using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class SendAccountRecoveryEmailRequest : PlayFabRequestCommon
	{
		public string Email;

		public string TitleId;
	}
}
