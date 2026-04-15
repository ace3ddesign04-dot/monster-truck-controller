using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTitlePublicKeyRequest : PlayFabRequestCommon
	{
		public string TitleId;

		public string TitleSharedSecret;
	}
}
