using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LoginWithEmailAddressRequest : PlayFabRequestCommon
	{
		public string Email;

		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string Password;

		public string TitleId;
	}
}
