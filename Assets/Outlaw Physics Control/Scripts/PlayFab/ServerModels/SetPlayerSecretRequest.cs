using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetPlayerSecretRequest : PlayFabRequestCommon
	{
		public string PlayerSecret;

		public string PlayFabId;
	}
}
