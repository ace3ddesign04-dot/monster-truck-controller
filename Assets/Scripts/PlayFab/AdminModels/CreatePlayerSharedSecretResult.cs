using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CreatePlayerSharedSecretResult : PlayFabResultCommon
	{
		public string SecretKey;
	}
}
