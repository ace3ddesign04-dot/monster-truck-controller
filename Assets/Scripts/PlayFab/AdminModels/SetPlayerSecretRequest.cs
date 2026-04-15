using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SetPlayerSecretRequest : PlayFabRequestCommon
	{
		public string PlayerSecret;

		public string PlayFabId;
	}
}
