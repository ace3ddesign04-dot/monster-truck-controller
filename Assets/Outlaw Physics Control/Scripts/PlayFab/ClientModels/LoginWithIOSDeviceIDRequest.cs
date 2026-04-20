using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LoginWithIOSDeviceIDRequest : PlayFabRequestCommon
	{
		public bool? CreateAccount;

		public string DeviceId;

		public string DeviceModel;

		public string EncryptedRequest;

		public GetPlayerCombinedInfoRequestParams InfoRequestParameters;

		public string OS;

		public string PlayerSecret;

		public string TitleId;
	}
}
