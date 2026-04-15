using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LoginWithWindowsHelloRequest : PlayFabRequestCommon
	{
		public string ChallengeSignature;

		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string PublicKeyHint;

		public string TitleId;
	}
}
