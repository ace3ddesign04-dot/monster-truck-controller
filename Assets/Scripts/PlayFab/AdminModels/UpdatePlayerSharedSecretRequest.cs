using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdatePlayerSharedSecretRequest : PlayFabRequestCommon
	{
		public bool Disabled;

		public string FriendlyName;

		public string SecretKey;
	}
}
