using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LoginWithKongregateRequest : PlayFabRequestCommon
	{
		public string AuthTicket;

		public bool? CreateAccount;

		public string EncryptedRequest;

		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string KongregateId;

		public string PlayerSecret;

		public string TitleId;
	}
}
