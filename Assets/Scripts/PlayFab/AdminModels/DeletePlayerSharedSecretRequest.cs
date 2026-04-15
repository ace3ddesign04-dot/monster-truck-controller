using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeletePlayerSharedSecretRequest : PlayFabRequestCommon
	{
		public string SecretKey;
	}
}
