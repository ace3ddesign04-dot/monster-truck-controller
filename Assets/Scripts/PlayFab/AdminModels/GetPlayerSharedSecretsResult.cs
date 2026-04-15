using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayerSharedSecretsResult : PlayFabResultCommon
	{
		public List<SharedSecret> SharedSecrets;
	}
}
