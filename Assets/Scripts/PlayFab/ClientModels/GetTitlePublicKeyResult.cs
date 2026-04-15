using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTitlePublicKeyResult : PlayFabResultCommon
	{
		public string RSAPublicKey;
	}
}
