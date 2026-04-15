using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class SetPlayerSecretRequest : PlayFabRequestCommon
	{
		public string EncryptedRequest;

		public string PlayerSecret;
	}
}
