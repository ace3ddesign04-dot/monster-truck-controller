using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RegisterWithWindowsHelloRequest : PlayFabRequestCommon
	{
		public string DeviceName;

		public string EncryptedRequest;

		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string PlayerSecret;

		public string PublicKey;

		public string TitleId;

		public string UserName;
	}
}
